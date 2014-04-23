using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using DCalcCore.Algorithm;
using DCalcCore.Assemblers;
using DCalcCore.Utilities;
using System.Threading;
using System.IO;
using DCalcCore.Remoting.Common;

namespace DCalcCore.Remoting.Http
{
    public class HttpRemoteGateClient : IRemoteGateClient
    {
        #region Private Fields

        /* Host info */
        private Int32 m_PortNumber;
        private Int32 m_PollTimeMs;
        private String m_Host;
        private String m_Url;
        private String m_SecurityKey;

        /* State info */
        private Thread m_Thread;
        private String m_ClientId;
        private Boolean m_IsClosing;
        private Dictionary<String, IScript> m_UploadedScripts;
        private Queue<IScript> m_ScriptsToUpload;
        private Queue<IScript> m_ScriptsToCancel;
        private ConsumableCollection<IScript> m_DataRegistry; 

        /* Locking */
        private String m_SyncRoot = "HttpRemoteGateClient Sync";

        #endregion

        #region Private Methods: HTTP Networking

        /// <summary>
        /// Contacts the server with parameters.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private XPortCommBody ContactServerWithParams(String operation, String parameters, String content)
        {
            String _url;

            if (parameters == null)
                _url = String.Format("{0}?{1}", m_Url, operation);
            else
                _url = String.Format("{0}?{1}&{2}", m_Url, operation, parameters);

            WebRequest request = WebRequest.Create(_url);

            if (content != null)
            {
                request.Method = "POST";

                Byte[] b_content = Encoding.UTF8.GetBytes(content);

                request.ContentType = "text/utf-8";
                request.ContentLength = b_content.Length;

                Stream cStream = request.GetRequestStream();

                cStream.Write(b_content, 0, b_content.Length);
                cStream.Flush();
                cStream.Close();
            }

            try
            {
                WebResponse response = request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());
                String respString = reader.ReadToEnd();

                response.Close();

                return new XPortCommBodyDeserializer(respString).Deserialize();
            }
            catch
            {
                throw new IOException();
            }
        }

        /// <summary>
        /// Contacts the server.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        private XPortCommBody ContactServer(String operation)
        {
            return ContactServerWithParams(operation, null, null);
        }

        /// <summary>
        /// Contacts the server.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        private XPortCommBody ContactServer(String operation, String clientId)
        {
            return ContactServerWithParams(operation, String.Format("id={0}", Uri.EscapeDataString(clientId)), null);
        }

        /// <summary>
        /// Contacts the server.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="scriptId">The script id.</param>
        /// <returns></returns>
        private XPortCommBody ContactServer(String operation, String clientId, String scriptId)
        {
            return ContactServerWithParams(operation, String.Format("id={0}&sid={1}", 
                Uri.EscapeDataString(clientId), Uri.EscapeDataString(scriptId)), null);
        }

        /// <summary>
        /// Contacts the server.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="sets">The sets.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="scriptId">The script id.</param>
        /// <returns></returns>
        private XPortCommBody ContactServer(String operation, ScalarSet[] sets, String clientId, String scriptId)
        {
            return ContactServerWithParams(operation, String.Format("id={0}&sid={1}",
                Uri.EscapeDataString(clientId), Uri.EscapeDataString(scriptId)), new DataSerializer(sets).Serialize());
        }

        /// <summary>
        /// Contacts the server.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="script">The script.</param>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        private XPortCommBody ContactServer(String operation, IScript script, String clientId)
        {
            return ContactServerWithParams(operation, String.Format("id={0}",
                Uri.EscapeDataString(clientId)), new ScriptSerializer(script).Serialize());
        } 

        #endregion

        #region Private Methods: Operations

        /// <summary>
        /// Operation REGISTER.
        /// </summary>
        /// <returns></returns>
        private String Operation_REGISTER(String securityKey)
        {
            XPortCommBody response = null;

            if (securityKey != null)
                response = ContactServerWithParams("REGISTER", "key=" + Uri.EscapeDataString(securityKey), null);
            else
                response = ContactServer("REGISTER");

            if (response != null && response.Code.Equals("REGISTERED"))
            {
                return response.Content;
            }

            return null;
        }

        /// <summary>
        /// Operation UNREGISTER.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        private Boolean Operation_UNREGISTER(String clientId)
        {
            XPortCommBody response = ContactServer("UNREGISTER", clientId);

            if (response != null && response.Code.Equals("UNREGISTERED"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Operation LOAD.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        private String Operation_LOAD(String clientId, IScript script)
        {
            XPortCommBody response = ContactServer("LOAD", script, clientId);

            if (response != null && response.Code.Equals("LOADED"))
            {
                return response.Content;
            }

            return null;
        }

        /// <summary>
        /// Operation UNLOAD.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="scriptId">The script id.</param>
        /// <returns></returns>
        private Boolean Operation_UNLOAD(String clientId, String scriptId)
        {
            XPortCommBody response = ContactServer("UNLOAD", clientId, scriptId);

            if (response != null && response.Code.Equals("UNLOADED"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Operation DATAIN.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="scriptId">The script id.</param>
        /// <param name="sets">The sets.</param>
        /// <returns></returns>
        private Boolean Operation_DATAIN(String clientId, String scriptId, ScalarSet[] sets)
        {
            XPortCommBody response = ContactServer("DATAIN", sets, clientId, scriptId);

            if (response != null && response.Code.Equals("DATAINED"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Operation DATAOUT.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="scriptId">The script id.</param>
        /// <returns></returns>
        private ScalarSet[] Operation_DATAOUT(String clientId, String scriptId)
        {
            XPortCommBody response = ContactServer("DATAOUT", clientId, scriptId);

            if (response != null && response.Code.Equals("DATAOUTED"))
            {
                try
                {
                    if (response.Content == null || response.Content.Length == 0)
                    {
                        return new ScalarSet[0];
                    }
                    else
                    {
                        return new DataDeserializer(response.Content).Deserialize();
                    }
                }
                catch
                {
                }
            }

            return null;
        }

        #endregion

        #region Private Methods: Helpers

        /// <summary>
        /// Marks connection as broken.
        /// </summary>
        private void ConnectionIsBroken()
        {
            if (m_ClientId == null)
                return;

            m_Thread.Priority = ThreadPriority.Lowest;
            m_ClientId = null;

            ClearScripts();
        }


        /// <summary>
        /// Gets the script id.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        private String GetScriptId(IScript script)
        {
            foreach (String key in m_UploadedScripts.Keys)
            {
                if (m_UploadedScripts[key] == script)
                {
                    return key;
                }
            }

            return null;
        }

        /// <summary>
        /// Clears out all the scripts.
        /// </summary>
        private void ClearScripts()
        {
            /* Free all the scripts that are registered */
            lock (m_DataRegistry)
            {
                foreach (IScript script in m_DataRegistry.Consumers)
                {
                    IEnumerable<ScalarSet> consumed = m_DataRegistry.RevertAll(script);
                    List<ScalarSet> toBeReleased = new List<ScalarSet>();

                    foreach (ScalarSet sg in consumed)
                    {
                        toBeReleased.Add(sg);
                    }

                    foreach (ScalarSet set in toBeReleased)
                    {
                        if (SetCompleted != null)
                            SetCompleted(script, new QueueEventArgs(null, set.Id));
                    }

                    String scriptId = GetScriptId(script);

                    if (scriptId != null)
                    {
                        m_ScriptsToUpload.Enqueue(script);
                        m_UploadedScripts.Remove(GetScriptId(script));
                    }
                }
            }
        }

        /// <summary>
        /// Clears out everything.
        /// </summary>
        private void ClearEverything()
        {
            ClearScripts();

            m_DataRegistry.Clear();
            m_ScriptsToCancel.Clear();
            m_ScriptsToUpload.Clear();
            m_UploadedScripts.Clear();
        } 

        #endregion

        #region THREAD: Connection

        /// <summary>
        /// Connection thread method.
        /// </summary>
        private void ConnectionThread()
        {
            try
            {
                while (true)
                {
                    #region Unregistering

                    if (m_IsClosing)
                    {
                        if (m_ClientId != null)
                        {
                            /* Disconnect! */
                            try
                            {
                                Operation_UNREGISTER(m_ClientId);
                            }
                            catch
                            {
                                /* Nothing here! Disconnect even if there's an error */
                            }

                        }
                        
                        break;
                    }

                    #endregion

                    #region Registering

                    if (m_ClientId == null)
                    {
                        /* First, try to connect if we did not do it yet */

                        try
                        {
                            m_ClientId = Operation_REGISTER(m_SecurityKey);
                            m_Thread.Priority = ThreadPriority.Normal;
                        }
                        catch
                        {
                            /* No Connection */
                            m_Thread.Priority = ThreadPriority.Lowest;
                        }

                        if (m_ClientId == null)
                        {
                            /* Registration Failed! */

                            if (DisconnectedFromServer != null)
                                DisconnectedFromServer(this, EventArgs.Empty);

                            continue;
                        }
                        else
                        {
                            /* Registered OK */
                            if (ConnectedToServer != null)
                                ConnectedToServer(this, EventArgs.Empty);
                        }
                    }

                    #endregion

                    #region Uploading Scripts

                    while (true)
                    {
                        IScript script = null;

                        lock (m_SyncRoot)
                        {
                            /* Take the last queue item */
                            if (m_ScriptsToUpload.Count > 0)
                                script = m_ScriptsToUpload.Dequeue();
                        }

                        if (script == null)
                            break; /* Nothing in te queue*/

                        String scriptKey = null;

                        
                        try
                        {
                            /* Load the script */
                            scriptKey = Operation_LOAD(m_ClientId, script);
                        }
                        catch
                        {
                            /* No connection! Will be performed later. */
                            lock (m_SyncRoot)
                            {
                                m_ScriptsToUpload.Enqueue(script);
                            }

                            break;
                        }

                        if (scriptKey != null)
                        {
                            /* Script loading succeeded! */
                            m_UploadedScripts.Add(scriptKey, script);
                        }
                        else
                        {
                            /* Script upload failed for some reason */
                            lock (m_SyncRoot)
                            {
                                m_ScriptsToUpload.Enqueue(script);
                            }
                        }
                    }

                    #endregion

                    #region Cancelling Scripts

                    while (true)
                    {
                        String scriptId = null;
                        IScript script = null;

                        lock (m_SyncRoot)
                        {
                            /* Take the last queue item */
                            if (m_ScriptsToCancel.Count > 0)
                            {
                                script = m_ScriptsToCancel.Dequeue();
                                scriptId = GetScriptId(script);
                            }
                        }

                        if (scriptId == null)
                            break; /* Nothing in the queue*/

                        try
                        {
                            Operation_UNLOAD(m_ClientId, scriptId);
                        }
                        catch
                        {
                            /* Connection problems! No problem, just continue. */
                        }

                        IEnumerable<ScalarSet> consumedSets = null;

                        lock (m_SyncRoot)
                        {
                            consumedSets = m_DataRegistry.RevertAll(script);
                            m_UploadedScripts.Remove(scriptId);
                        }

                        foreach (ScalarSet set in consumedSets)
                        {
                            if (SetCompleted != null)
                                SetCompleted(script, new QueueEventArgs(null, set.Id));
                        }

                        /* Unregister the script */
                        m_UploadedScripts.Remove(scriptId);
                    }

                    #endregion

                    #region Uploading Sets

                    /* Load all consumers */
                    List<IScript> scriptList = new List<IScript>(m_DataRegistry.Consumers);

                    foreach (IScript script in scriptList)
                    {
                        String scriptId;

                        scriptId = GetScriptId(script);

                        if (scriptId != null)
                        {
                            List<ScalarSet> setList = m_DataRegistry.ConsumeAll(script);

                            if (setList != null && setList.Count > 0)
                            {
                                Boolean success = false;

                                try
                                {
                                    success = Operation_DATAIN(m_ClientId, scriptId, setList.ToArray());
                                }
                                catch
                                {
                                    /* Connection problems. No problems, just return the set back. */
                                }

                                if (success)
                                {
                                    /* Sets uploaded */
                                }
                                else
                                {
                                    /* Return sets back */
                                    m_DataRegistry.Extract(script, setList);

                                    foreach (ScalarSet set in setList)
                                    {
                                        if (SetCompleted != null)
                                            SetCompleted(script, new QueueEventArgs(null, set.Id));
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region Downloading Sets

                    /* Load all scripts that are registered */
                    foreach (String scriptId in m_UploadedScripts.Keys)
                    {
                        IScript script = m_UploadedScripts[scriptId];
                        ScalarSet[] sets = null;

                        try
                        {
                            /* check for data */
                            sets = Operation_DATAOUT(m_ClientId, scriptId);
                        }
                        catch
                        {
                            /* Connection problems. */
                        }

                        if (sets != null)
                        {
                            foreach (ScalarSet set in sets)
                            {
                                m_DataRegistry.Extract(script, set);

                                /* Report about success of the scalar set */
                                if (SetCompleted != null)
                                    SetCompleted(script, new QueueEventArgs(set, set.Id));
                            }
                        }
                        else
                        {
                            /* Failed! */
                            ConnectionIsBroken();
                            break;
                        }
                    }


                    #endregion
                }
            }
            catch
            {
                return;
            }

            lock (m_SyncRoot)
            {
                ClearEverything();
                m_IsClosing = false;
            }

            /* Connection closed! */
            if (DisconnectedFromServer != null)
                DisconnectedFromServer(this, EventArgs.Empty);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRemoteGateClient"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="portNumber">The port number.</param>
        /// <param name="securityKey">The security key.</param>
        /// <param name="pollTimeMs">The poll time in milliseconds.</param>
        public HttpRemoteGateClient(String host, Int32 portNumber, String securityKey, Int32 pollTimeMs)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            if (host.Length == 0)
                throw new ArgumentException("host");

            if (portNumber < 0 || portNumber > UInt16.MaxValue)
                throw new ArgumentException("portNumber");

            if (pollTimeMs < 1)
                throw new ArgumentException("pollTimeMs");

            if (securityKey == null)
                throw new ArgumentNullException("securityKey");

            m_PortNumber = portNumber;
            m_PollTimeMs = pollTimeMs;
            m_Host = host;
            m_SecurityKey = securityKey;

            m_Thread = new Thread(ConnectionThread);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRemoteGateClient"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="portNumber">The port number.</param>
        /// <param name="pollTimeMs">The poll time in milliseconds.</param>
        public HttpRemoteGateClient(String host, Int32 portNumber, Int32 pollTimeMs)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            if (host.Length == 0)
                throw new ArgumentException("host");

            if (portNumber < 0 || portNumber > UInt16.MaxValue)
                throw new ArgumentException("portNumber");

            if (pollTimeMs < 1)
                throw new ArgumentException("pollTimeMs");

            m_PortNumber = portNumber;
            m_PollTimeMs = pollTimeMs;
            m_Host = host;

            m_Thread = new Thread(ConnectionThread);
        }

        #endregion

        #region IRemoteGate Members

        /// <summary>
        /// Queues the opening of the connection to the server.
        /// </summary>
        public void AsyncOpen()
        {
            lock (m_SyncRoot)
            {
                /* Check if we're already running */
                if (!m_Thread.IsAlive)
                {
                    /* Build URL */
                    m_Url = String.Format("http://{0}:{1}/", m_Host, m_PortNumber);

                    /* Reset everything */
                    m_ClientId = null;
                    m_UploadedScripts = new Dictionary<String, IScript>();
                    m_ScriptsToUpload = new Queue<IScript>();
                    m_ScriptsToCancel = new Queue<IScript>();
                    m_DataRegistry = new ConsumableCollection<IScript>();
                    m_IsClosing = false;

                    /* Open connection thread */
                    m_Thread.Start();
                }
            }
        }

        /// <summary>
        /// Queues the closing of the connection to the server.
        /// </summary>
        public void AsyncClose()
        {
            lock (m_SyncRoot)
            {
                /* Check if we're already connected */
                if (m_Thread.IsAlive)
                {
                    /* Kill thread */
                    m_IsClosing = true;
                }
            }
        }

        /// <summary>
        /// Queues the registration of a script remotely.
        /// </summary>
        /// <param name="script">The script.</param>
        public void AsyncRegisterScript(IScript script)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            lock (m_SyncRoot)
            {
                /* Put the script into a queue */
                m_ScriptsToUpload.Enqueue(script);
            }
        }

        /// <summary>
        /// Queues the cancelling of a script remotely.
        /// </summary>
        /// <param name="script">The script.</param>
        public void AsyncCancelScript(IScript script)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            lock (m_SyncRoot)
            {
                /* Put the script into a queue */
                m_ScriptsToCancel.Enqueue(script);
            }
        }

        /// <summary>
        /// Queues some work to be done remotely.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="set">The set.</param>
        /// <returns></returns>
        public Boolean AsyncQueueWork(IScript script, ScalarSet set)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            if (set == null)
                throw new ArgumentNullException("set");

            lock (m_SyncRoot)
            {
                if (m_ClientId == null)
                    return false;

                /* Put the script into a queue */
                m_DataRegistry.Place(script, set);

                return true;
            }
        }

        /// <summary>
        /// Occurs when set completed.
        /// </summary>
        public event QueueEventHandler SetCompleted;

        /// <summary>
        /// Occurs when gate connected to server.
        /// </summary>
        public event EventHandler ConnectedToServer;

        /// <summary>
        /// Occurs when gate disconnected from server.
        /// </summary>
        public event EventHandler DisconnectedFromServer;

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            /* Close connection. That's all */
            AsyncClose();
        }

        #endregion
    }
}
