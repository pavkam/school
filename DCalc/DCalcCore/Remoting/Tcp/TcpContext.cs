using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace DCalcCore.Remoting.Tcp
{
    /// <summary>
    /// Provides client context to be used in server implementations.
    /// </summary>
    internal class TcpContext
    {
        #region Private Fields

        private TcpClient m_Client;
        private List<String> m_Parameters = new List<String>();
        private Byte[] m_Body; 

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads all data.
        /// </summary>
        /// <returns></returns>
        private Boolean LoadAllData()
        {
            try
            {
                BinaryReader rd = new BinaryReader(m_Client.GetStream());
                Int32 bytesToExpect = rd.ReadInt32();
                Byte[] bytes = rd.ReadBytes(bytesToExpect);

                /* Find first instance of : char */
                String paramBlock = null;

                for (Int32 i = 0; i < bytes.Length; i++)
                {
                    if (bytes[i] == ':')
                    {
                        /* Get param block */
                        paramBlock = Encoding.UTF8.GetString(bytes, 0, i);
                        Int32 bodySize = (bytes.Length - i) - 1;

                        if (bodySize > 0)
                        {
                            m_Body = new Byte[bodySize];
                            Array.Copy(bytes, i + 1, m_Body, 0, bodySize);
                        }
                        else
                            m_Body = null;

                        break;
                    }
                }

                /* Assuming param block is ok */
                String[] parameters = paramBlock.Split('&');

                for (Int32 i = 0; i < parameters.Length; i++)
                {
                    m_Parameters.Add(Uri.UnescapeDataString(parameters[i]));
                }
            }
            catch
            {
                return false;
            }

            return true;
        } 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpContext"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public TcpContext(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            m_Client = client;

            /* try load all data */
            if (!LoadAllData())
                throw new IOException("Tcp Context Read error.");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public IList<String> Parameters
        {
            get { return m_Parameters; }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <value>The body.</value>
        public Byte[] Body
        {
            get { return m_Body; }
        }

        /// <summary>
        /// Writes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public void WriteBytes(Byte[] bytes)
        {
            BinaryWriter br = new BinaryWriter(m_Client.GetStream());
            br.Write((Int32)bytes.Length);
            br.Write(bytes);
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            m_Client.Client.Shutdown(SocketShutdown.Both);
            m_Client.Client.Disconnect(true);
            m_Client.Client.Close();
            m_Client.Close();
        } 

        #endregion
    }
}
