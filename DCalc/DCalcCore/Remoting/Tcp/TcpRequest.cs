using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DCalcCore.Remoting.Tcp
{
    /// <summary>
    /// Used to create Tcp-based requests.
    /// </summary>
    internal sealed class TcpRequest
    {
        private static UInt16 m_NextPort = 1024;
        private static String m_StaticSyncRoot = "TcpRequest Static Sync";

        private String m_Host;
        private Int32 m_Port;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpRequest"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public TcpRequest(String host, Int32 port)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            if (host.Length == 0)
                throw new ArgumentException("host");

            if (port < 0 || port > UInt16.MaxValue)
                throw new ArgumentException("port");

            m_Host = host;
            m_Port = port;
        }

        /// <summary>
        /// Gets the response bytes.
        /// </summary>
        /// <param name="contents">The contents to send.</param>
        /// <returns></returns>
        public Byte[] GetResponse(Byte[] contents)
        {
            TcpClient client = null;

            lock (m_StaticSyncRoot)
            {
                while (true)
                {
                    if (m_NextPort == UInt16.MaxValue)
                        m_NextPort = 1024;
                    else
                        m_NextPort++;

                    try
                    {
                        IPEndPoint ePoint = new IPEndPoint(IPAddress.Any, m_NextPort);
                        client = new TcpClient(ePoint);
                        client.Connect(m_Host, m_Port);
                    }
                    catch (SocketException se)
                    {
                        if (se.SocketErrorCode == SocketError.AddressAlreadyInUse)
                        {
                            /* Network address in use */
                            continue;
                        }
                    }

                    break;
                }
            }

            NetworkStream ns = null;

            try
            {
                ns = client.GetStream();
            }
            catch
            {
            }

            Exception passedException = null;
            Byte[] response = null;

            try
            {
                /* Write the request to the pipe */
                BinaryWriter br = new BinaryWriter(ns);
                br.Write((Int32)contents.Length);

                br.Write(contents);
                br.Flush();

                /* Wait for response */
                BinaryReader brr = new BinaryReader(ns);
                Int32 sizeToWait = brr.ReadInt32();
                response = brr.ReadBytes(sizeToWait);
            }
            catch (Exception e)
            {
                passedException = e;
            }

            try
            {
                client.Client.Shutdown(SocketShutdown.Both);
                client.Client.Disconnect(true);
                client.Close();
            }
            catch
            {
            }

            if (passedException != null)
                throw passedException;

            return response;
        }
    }
}
