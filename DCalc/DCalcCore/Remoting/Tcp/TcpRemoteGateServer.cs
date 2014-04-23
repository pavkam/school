using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using DCalcCore.Utilities;
using DCalcCore.Assemblers;
using DCalcCore.Runners;
using DCalcCore.Algorithm;
using DCalcCore.LoadBalancers;
using DCalcCore.Remoting.Common;

namespace DCalcCore.Remoting.Tcp
{
    public class TcpRemoteGateServer<L> : IRemoteGateServer 
        where L : ILoadBalancer, new()
    {
        #region Private Fields

        /* State */
        private LocalMachineRunner<CSharpScriptAssembler, L> m_LocalRunner;
        private TcpListener m_Listener;
        private Thread m_ListenThread;
        private Thread m_WatchThread;
        private Int32 m_KillClientAfterMs;
        private Int32 m_UniqueKeySize;
        private String m_SecuityKey;
        private Int32 m_ThreadCount;

        /* Clients */
        private UniqueCollection<UniqueCollection<IScript>> m_RegisteredClients;
        private Dictionary<String, DateTime> m_LastAccessDt;
        private Dictionary<IScript, List<ScalarSet>> m_OutResults;

        /* Threading */
        private String m_SyncRoot = "TcpRemoteGateServer Sync";

        #endregion

        #region Private Methods: TCP Networking

        /// <summary>
        /// Writes the response back to the client.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="body">The body.</param>
        private void WriteResponse(TcpContext context, XPortCommBody body)
        {
            String ser = new XPortCommBodySerializer(body).Serialize();

            Byte[] bytes = Encoding.UTF8.GetBytes(ser);
            context.WriteBytes(bytes);
        }

        /// <summary>
        /// Writes the response back to the client.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="code">The code.</param>
        /// <param name="content">The content.</param>
        private void WriteResponse(TcpContext context, String code, String content)
        {
            WriteResponse(context, new XPortCommBody(code, content));
        }

        /// <summary>
        /// Gets the body of the HTTP request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private String GetBody(TcpContext context)
        {
            /* Transform back to string */
            String responseString = Encoding.UTF8.GetString(context.Body);
            return responseString;
        } 

        #endregion

        #region Private Methods: Operations

        /// <summary>
        /// Operation: HELLO.
        /// </summary>
        /// <param name="context">The context.</param>
        private void Operation_HELLO(TcpContext context)
        {
            /* Send a greeting back + number of local cores */

            if (context != null)
                WriteResponse(context, "HELLO", Environment.ProcessorCount.ToString());
        }

        /// <summary>
        /// Operation: REGISTER.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="securityKey">The security key.</param>
        private void Operation_REGISTER(TcpContext context, String securityKey)
        {
            String clientId = null;

            lock (m_SyncRoot)
            {
                if (m_SecuityKey != null && m_SecuityKey.Length > 0)
                {
                    if (securityKey == null | (!m_SecuityKey.Equals(securityKey)))
                    {
                        if (context != null)
                            WriteResponse(context, "ERROR", "BAD_SECURITY_KEY");

                        return;
                    }
                }

                /* Client wants to register itself */
                clientId = m_RegisteredClients.New(new UniqueCollection<IScript>(m_UniqueKeySize));

                m_LastAccessDt.Add(clientId, DateTime.Now);

                if (context != null)
                    WriteResponse(context, "REGISTERED", clientId);
            }

            if (ClientConnected != null)
            {
                ClientConnected(this, new ConnectionEventArgs(clientId));
            }
        }

        /// <summary>
        /// Operation: UNREGISTER.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="clientId">The client id.</param>
        private void Operation_UNREGISTER(TcpContext context, String clientId)
        {
            List<String> keys = null;

            lock (m_SyncRoot)
            {
                UniqueCollection<IScript> scriptCollection = m_RegisteredClients[clientId];

                if (scriptCollection != null)
                {
                    keys = new List<String>(scriptCollection.Keys);
                }
                else
                {
                    if (context != null)
                        WriteResponse(context, "ERROR", "NOT_REGISTERED");
                }
            }

            foreach (String key in keys)
            {
                /* Unregister each script */
                Operation_UNLOAD(null, clientId, key);
            }

            lock (m_SyncRoot)
            {
                m_LastAccessDt.Remove(clientId);
                m_RegisteredClients.Remove(clientId);

                if (context != null)
                    WriteResponse(context, "UNREGISTERED", clientId);
            }

            if (ClientDisconnected != null)
            {
                ClientDisconnected(this, new ConnectionEventArgs(clientId));
            }
        }

        /// <summary>
        /// Operation: LOAD.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="clientId">The client id.</param>
        private void Operation_LOAD(TcpContext context, String clientId)
        {
            lock (m_SyncRoot)
            {
                UniqueCollection<IScript> scriptCollection = m_RegisteredClients[clientId];

                if (scriptCollection != null)
                {
                    /* This client is registered */
                    IScript script = new ScriptDeserializer(GetBody(context)).Deserialize();

                    if (m_LocalRunner.LoadScript(script))
                    {
                        /* Compiled perfectly! */
                        String scriptId = scriptCollection.New(script);

                        m_LastAccessDt[clientId] = DateTime.Now;

                        if (AlgorithmRegistered != null)
                        {
                            AlgorithmRegistered(this, new ConnectionEventArgs(clientId));
                        }

                        if (context != null)
                            WriteResponse(context, "LOADED", scriptId);
                    }
                    else
                    {
                        if (context != null)
                            WriteResponse(context, "ERROR", "COMPILATION");
                    }
                }
                else
                {
                    if (context != null)
                        WriteResponse(context, "ERROR", "NOT_REGISTERED");
                }
            }
        }

        /// <summary>
        /// Operation: UNLOAD.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="scriptId">The script id.</param>
        private void Operation_UNLOAD(TcpContext context, String clientId, String scriptId)
        {
            UniqueCollection<IScript> scriptCollection = null;
            IScript script = null;

            lock (m_SyncRoot)
            {
                scriptCollection = m_RegisteredClients[clientId];

                if (scriptCollection != null)
                {
                    script = scriptCollection[scriptId];
                }

                if (script == null)
                {
                    if (context != null)
                        WriteResponse(context, "ERROR", "NOT_REGISTERED");

                    return;
                }
            }

            if (m_LocalRunner.RemoveScript(script))
            {
                Int32 countOf = 0;

                lock (m_SyncRoot)
                {
                    /* Removed perfectly! */
                    scriptCollection.Remove(scriptId);
                    m_LastAccessDt[clientId] = DateTime.Now;

                    /* Check the number of output results */
                    if (m_OutResults.ContainsKey(script))
                    {
                        countOf = m_OutResults[script].Count;
                        m_OutResults.Remove(script);
                    }
                }

                if (ClientDownloadedOutputSets != null && countOf > 0)
                {
                    ClientDownloadedOutputSets(this, new SetUpdateEventArgs(clientId, countOf));
                }

                if (AlgorithmRemoved != null)
                {
                    AlgorithmRemoved(this, new ConnectionEventArgs(clientId));
                }

                if (context != null)
                    WriteResponse(context, "UNLOADED", scriptId);
            }
            else
            {
                if (context != null)
                    WriteResponse(context, "ERROR", "REMOVAL");
            }
        }

        /// <summary>
        /// Operation: DATAIN.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="scriptId">The script id.</param>
        private void Operation_DATAIN(TcpContext context, String clientId, String scriptId)
        {
            lock (m_SyncRoot)
            {
                UniqueCollection<IScript> scriptCollection = m_RegisteredClients[clientId];

                if (scriptCollection != null)
                {
                    IScript script = scriptCollection[scriptId];

                    if (script != null)
                    {
                        /* Get the scalar groups */
                        ScalarSet[] groups = new DataDeserializer(GetBody(context)).Deserialize();

                        foreach (ScalarSet group in groups)
                        {
                            m_LocalRunner.QueueWork(script, group);
                        }

                        m_LastAccessDt[clientId] = DateTime.Now;

                        if (this.ClientUploadedInputSets != null)
                        {
                            ClientUploadedInputSets(this, new SetUpdateEventArgs(clientId, groups.Length));
                        }

                        if (context != null)
                            WriteResponse(context, "DATAINED", groups.Length.ToString());
                    }
                    else
                    {
                        if (context != null)
                            WriteResponse(context, "ERROR", "SCRIPT_NOT_REGISTERED");
                    }
                }
                else
                {
                    if (context != null)
                        WriteResponse(context, "ERROR", "NOT_REGISTERED");
                }
            }
        }

        /// <summary>
        /// Operation: DATAOUT.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="scriptId">The script id.</param>
        private void Operation_DATAOUT(TcpContext context, String clientId, String scriptId)
        {
            lock (m_SyncRoot)
            {
                UniqueCollection<IScript> scriptCollection = m_RegisteredClients[clientId];

                if (scriptCollection != null)
                {
                    IScript script = scriptCollection[scriptId];

                    if (script != null)
                    {
                        String resultText = String.Empty;
                        Int32 countOf = 0;

                        lock (m_OutResults)
                        {
                            if (m_OutResults.ContainsKey(script))
                            {
                                List<ScalarSet> list = m_OutResults[script];
                                countOf = list.Count;

                                if (countOf > 0)
                                    resultText = new DataSerializer(list.ToArray()).Serialize();

                                list.Clear();
                            }
                        }

                        m_LastAccessDt[clientId] = DateTime.Now;

                        if (this.ClientDownloadedOutputSets != null)
                        {
                            ClientDownloadedOutputSets(this, new SetUpdateEventArgs(clientId, countOf));
                        }

                        if (context != null)
                            WriteResponse(context, "DATAOUTED", resultText);
                    }
                    else
                    {
                        if (context != null)
                            WriteResponse(context, "ERROR", "SCRIPT_NOT_REGISTERED");
                    }
                }
                else
                {
                    if (context != null)
                        WriteResponse(context, "ERROR", "NOT_REGISTERED");
                }
            }
        }

        #endregion

        #region Private Methods: Helpers

        /// <summary>
        /// Serves a client.
        /// </summary>
        /// <param name="context">The context.</param>
        private void ServeClient(TcpContext context)
        {
            /* Check for validity */
            if (context.Parameters.Count < 1)
                return;

            /* Figure out what he wants */
            try
            {
                String operation = context.Parameters[0].ToUpper();

                if (operation.Equals("HELLO"))
                {
                    #region Operation: HELLO

                    Operation_HELLO(context);

                    #endregion
                }
                else if (operation.Equals("REGISTER"))
                {
                    #region Operation: REGISTER

                    String securityKey = null;

                    try
                    {
                        securityKey = context.Parameters[1];
                    }
                    catch
                    {
                    }

                    Operation_REGISTER(context, securityKey);

                    #endregion
                }
                else if (operation.Equals("UNREGISTER"))
                {
                    #region Operation: UNREGISTER

                    /* Client wants to unregister itself */
                    String clientId = context.Parameters[1];

                    Operation_UNREGISTER(context, clientId);

                    #endregion
                }
                else if (operation.Equals("LOAD"))
                {
                    #region Operation: LOAD

                    /* Client wants to register an algorithm */
                    String clientId = context.Parameters[1];

                    Operation_LOAD(context, clientId);

                    #endregion
                }
                else if (operation.Equals("UNLOAD"))
                {
                    #region Operation: UNLOAD

                    /* Client wants to un-register an algorithm */
                    String clientId = context.Parameters[1];
                    String scriptId = context.Parameters[2];

                    Operation_UNLOAD(context, clientId, scriptId);

                    #endregion
                }
                else if (operation.Equals("DATAIN"))
                {
                    #region Operation: DATAIN

                    /* Client wants to upload input sets */
                    String clientId = context.Parameters[1];
                    String scriptId = context.Parameters[2];

                    Operation_DATAIN(context, clientId, scriptId);

                    #endregion
                }
                else if (operation.Equals("DATAOUT"))
                {
                    #region Operation: DATAOUT

                    /* Client wants to upload input sets */
                    String clientId = context.Parameters[1];
                    String scriptId = context.Parameters[2];

                    Operation_DATAOUT(context, clientId, scriptId);

                    #endregion
                }
            }
            catch
            {
                /* Protect this piece of code! */
                try
                {
                    WriteResponse(context, "ERROR", "UNKNOWN");
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Handles the QueuedWorkCompleted event of the LocalRunner control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DCalcCore.Utilities.ScriptQueueEventArgs"/> instance containing the event data.</param>
        private void m_localRunner_QueuedWorkCompleted(object sender, ScriptQueueEventArgs e)
        {
            lock (m_SyncRoot)
            {
                /* Work complete on a set, put the result into a list */
                IScript script = e.Script;

                if (m_OutResults.ContainsKey(script))
                {
                    m_OutResults[script].Add(e.OutputSet);
                }
                else
                {
                    List<ScalarSet> groups = new List<ScalarSet>();
                    groups.Add(e.OutputSet);

                    m_OutResults.Add(script, groups);
                }
            }
        }

        #endregion

        #region THREAD: Watch Thread

        /// <summary>
        /// Watch thread method.
        /// </summary>
        private void WatchMethod()
        {
            try
            {
                List<String> clientsToKill = new List<String>();

                while (true)
                {
                    /* Sleep */
                    Thread.Sleep(100);

                    /* Check all clients for expiration */
                    lock (m_SyncRoot)
                    {
                        foreach (String key in m_RegisteredClients.Keys)
                        {
                            if (m_LastAccessDt.ContainsKey(key))
                            {
                                DateTime lastTime = m_LastAccessDt[key];

                                TimeSpan ts = (DateTime.Now - lastTime);

                                if (ts.TotalMilliseconds > m_KillClientAfterMs)
                                    clientsToKill.Add(key);
                            }
                        }
                    }

                    /* Kill clients */
                    if (clientsToKill.Count > 0)
                    {
                        foreach (String key in clientsToKill)
                        {
                            Operation_UNREGISTER(null, key);
                        }

                        clientsToKill.Clear();
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region THREAD: Listening Thread

        /// <summary>
        /// Tcp Listener method.
        /// </summary>
        private void ListenMethod()
        {
            try
            {
                while (true)
                {
                    TcpContext context = null;

                    try
                    {
                        context = new TcpContext(m_Listener.AcceptTcpClient());
                        ServeClient(context);

                        context.Close();
                    }
                    catch (Exception e)
                    {
                        if (e is ThreadAbortException)
                        {
                            try
                            {
                                if (context != null)
                                    context.Close();
                            }
                            catch
                            {
                            }

                            throw e;
                        }
                    }
                }
            }
            catch
            {
            }
        } 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpRemoteGateServer&lt;L&gt;"/> class.
        /// </summary>
        /// <param name="threadCount">The thread count.</param>
        /// <param name="port">The port.</param>
        /// <param name="timeOut">The timeout.</param>
        /// <param name="uniqueKeySize">Size of the unique key.</param>
        /// <param name="securityKey">The security key.</param>
        public TcpRemoteGateServer(Int32 threadCount, Int32 port, Int32 timeOut, Int32 uniqueKeySize, String securityKey)
        {
            if (threadCount < 1)
                throw new ArgumentException("threadCount");

            if (port < 0 || port > UInt16.MaxValue)
                throw new ArgumentException("port");

            if (timeOut < 1)
                throw new ArgumentException("timeOut");

            if (uniqueKeySize < 2)
                throw new ArgumentException("uniqueKeySize");

            /* Configure listener */
            m_Listener = new TcpListener(IPAddress.Any, port);

            /* Thread */
            m_ListenThread = new Thread(ListenMethod);

            m_UniqueKeySize = uniqueKeySize;
            m_SecuityKey = securityKey;
            m_ThreadCount = threadCount;

            /* Watcher thread */
            m_KillClientAfterMs = timeOut;
            m_WatchThread = new Thread(WatchMethod);
        }

        #endregion

        #region IRemoteGateServer Members

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns></returns>
        public Boolean Start()
        {
            lock (m_SyncRoot)
            {
                /* Try to start */
                if (m_ListenThread.IsAlive)
                    return false;

                try
                {
                    m_Listener.Start();
                }
                catch
                {
                    return false;
                }

                /* Reset all variables */
                m_LastAccessDt = new Dictionary<String, DateTime>();
                m_RegisteredClients = new UniqueCollection<UniqueCollection<IScript>>(m_UniqueKeySize);
                m_LocalRunner = new LocalMachineRunner<CSharpScriptAssembler, L>(m_ThreadCount);
                m_LocalRunner.QueuedWorkCompleted += m_localRunner_QueuedWorkCompleted;
                m_OutResults = new Dictionary<IScript, List<ScalarSet>>();

                m_ListenThread.Start();
                m_WatchThread.Start();

                return true;
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <returns></returns>
        public Boolean Stop()
        {
            lock (m_SyncRoot)
            {
                if (!m_ListenThread.IsAlive)
                    return false;

                m_LocalRunner.Dispose();

                /* Stop the server brutally */
                try
                {
                    m_Listener.Stop();
                    m_WatchThread.Abort();
                    m_ListenThread.Abort();
                }
                catch
                {
                }

                return true;
            }
        }

        /// <summary>
        /// Occurs when a client has connected.
        /// </summary>
        public event ConnectionEventHandler ClientConnected;

        /// <summary>
        /// Occurs when a client has disconnected.
        /// </summary>
        public event ConnectionEventHandler ClientDisconnected;

        /// <summary>
        /// Occurs when an algorithm was registered.
        /// </summary>
        public event ConnectionEventHandler AlgorithmRegistered;

        /// <summary>
        /// Occurs when an algorithm was removed.
        /// </summary>
        public event ConnectionEventHandler AlgorithmRemoved;

        /// <summary>
        /// Occurs when a client uploaded input sets.
        /// </summary>
        public event SetUpdateEventHandler ClientUploadedInputSets;

        /// <summary>
        /// Occurs when a client downloaded output sets.
        /// </summary>
        public event SetUpdateEventHandler ClientDownloadedOutputSets;

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        #endregion
    }
}
