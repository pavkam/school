using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DCalcCore.Algorithm;
using DCalc.Algorithms;
using DCalcCore.LoadBalancers;

namespace DCalc.Communication
{
    /// <summary>
    /// Xml based workspace.
    /// </summary>
    public class XmlStoredWorkSpace : IWorkSpace
    {
        #region Private Fields

        private String m_XmlFileName;
        private List<IServer> m_Servers;
        private IAlgorithmProvider m_Provider;
        private IAlgorithmCollection m_Algorithms;
        private Int32 m_QueueSize = 1024;
        private Int32 m_NumberOfThreads = 1;
        private Type m_LocalBalancer = typeof(FairLoadBalancer);
        private Type m_RemoteBalancer = typeof(FairLoadBalancer);

        #endregion

        #region Private Fields

        /// <summary>
        /// Checks for local server.
        /// </summary>
        /// <param name="servers">The servers.</param>
        /// <returns></returns>
        private List<IServer> CheckForLocalServer(List<IServer> servers)
        {
            List<IServer> result = new List<IServer>();
            IServer localServer = null;

            foreach(IServer server in servers)
            {
                if (server is LocalServer)
                {
                    if (localServer == null)
                    {
                        localServer = server;
                    }
                }
                else
                    result.Add(server);
            }

            /* Do we have the one local server? */
            if (localServer == null)
                localServer = new LocalServer(false);

            result.Add(localServer);
            return result;
        }

        /// <summary>
        /// Get Type from name.
        /// </summary>
        /// <param name="balancerType">Type of the balancer.</param>
        /// <returns></returns>
        private Type TypeFromName(String balancerType)
        {
            Type typeOfFairLoadBalancer = typeof(FairLoadBalancer);
            Type typeOfRRLoadBalancer = typeof(RRLoadBalancer);
            Type typeOfPredictiveLoadBalancer = typeof(PredictiveLoadBalancer);

            if (balancerType.Equals(typeOfFairLoadBalancer.FullName))
                return typeOfFairLoadBalancer;
            else if (balancerType.Equals(typeOfRRLoadBalancer.FullName))
                return typeOfRRLoadBalancer;
            else if (balancerType.Equals(typeOfPredictiveLoadBalancer.FullName))
                return typeOfPredictiveLoadBalancer;

            return null;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStoredWorkSpace"/> class.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public XmlStoredWorkSpace(String xmlFileName)
        {
            m_XmlFileName = xmlFileName;
            m_Servers = CheckForLocalServer(new List<IServer>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStoredWorkSpace"/> class.
        /// </summary>
        public XmlStoredWorkSpace()
        {
            m_XmlFileName = null;
            m_Servers = CheckForLocalServer(new List<IServer>());
        } 

        #endregion

        #region XmlStoredWorkSpace Public Properties

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public String FileName
        {
            get { return m_XmlFileName; }
            set { m_XmlFileName = value; }
        }

        /// <summary>
        /// Gets or sets the algorithms.
        /// </summary>
        /// <value>The algorithms.</value>
        public IAlgorithmCollection Algorithms
        {
            get { return m_Algorithms; }
            set { m_Algorithms = value; }
        }

        #endregion

        #region IWorkSpace Members

        /// <summary>
        /// Gets all the registered servers.
        /// </summary>
        /// <value>The servers.</value>
        public IEnumerable<IServer> Servers
        {
            get 
            {
                return m_Servers;
            }
        }

        /// <summary>
        /// Adds a new server.
        /// </summary>
        /// <param name="server">The server.</param>
        public void AddServer(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            if (!m_Servers.Contains(server))
                m_Servers.Add(server);
        }

        /// <summary>
        /// Removes a server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public IServer RemoveServer(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            m_Servers.Remove(server);

            return server;
        }

        /// <summary>
        /// Clears all the servers.
        /// </summary>
        public void ClearServers()
        {
            m_Servers.Clear();
        }

        /// <summary>
        /// Gets the count of servers.
        /// </summary>
        /// <value>The count of servers.</value>
        public Int32 CountOfServers
        {
            get 
            {
                return m_Servers.Count;
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns></returns>
        public Boolean Load()
        {
            try
            {
                /* Try to load the XML document that contains the server list */
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFileName);

                /* Get root node */
                XmlElement root = xmlDoc.DocumentElement;

                if (!root.Name.Equals("configuration"))
                    return false;


                /* Load setting */
                try
                {
                    Int32 qsize = Convert.ToInt32(root.Attributes["qsize"].Value);
                    Int32 thc = Convert.ToInt32(root.Attributes["thc"].Value);

                    Type lbt = TypeFromName(root.Attributes["lbid"].Value);
                    Type rbt = TypeFromName(root.Attributes["rbid"].Value);

                    if (qsize < 1 || thc < 0 || lbt == null || rbt == null)
                        return false;

                    m_QueueSize = qsize;
                    m_NumberOfThreads = thc;
                    m_LocalBalancer = lbt;
                    m_RemoteBalancer = rbt;
                }
                catch
                {
                    return false;
                }

                XmlNode xmlServerList;
                
                try
                {
                    xmlServerList = root.GetElementsByTagName("servers")[0];
                }
                catch
                {
                    return false;
                }

                List<IServer> servers = new List<IServer>();

                /* Circle in each node */
                foreach (XmlNode xmlServerNode in xmlServerList.ChildNodes)
                {
                    IServer server = null;

                    if (xmlServerNode.Name.Equals("remote"))
                    {
                        String _name = xmlServerNode.Attributes["name"].Value;
                        String _host = xmlServerNode.Attributes["host"].Value;
                        String _key = xmlServerNode.Attributes["key"].Value;
                        Int32 _port = Convert.ToInt32(xmlServerNode.Attributes["port"].Value);
                        ConnectionType _ctype = ConnectionType.Http;

                        try
                        {
                            _ctype = (ConnectionType)(Convert.ToInt32(xmlServerNode.Attributes["ctype"].Value));
                        }
                        catch
                        {
                        }

                        Boolean _enabled = (Convert.ToInt32(xmlServerNode.Attributes["enabled"].Value) != 0);

                        server = new RemoteServer(_name, _host, _port, _key, _ctype, _enabled);
                    }
                    else if (xmlServerNode.Name.Equals("local"))
                    {
                        Boolean _enabled = (Convert.ToInt32(xmlServerNode.Attributes["enabled"].Value) != 0);

                        server = new LocalServer(_enabled);
                    }
                    else
                        return false;

                    servers.Add(server);
                }

                /* Check if we have the local server and that it's only once opy of it */
                m_Servers = CheckForLocalServer(servers);

                /* Load provider */
                XmlNode xmlProvider;

                try
                {
                    xmlProvider = root.GetElementsByTagName("provider")[0];
                    String _id = xmlProvider.Attributes["id"].Value;

                    Dictionary<String, String> settings = new Dictionary<String, String>();

                    /* Circle in each node */
                    foreach (XmlNode xmlOptionNode in xmlProvider.ChildNodes)
                    {
                        if (xmlOptionNode.Name.Equals("option"))
                        {
                            String _key = xmlOptionNode.Attributes["key"].Value;
                            String _value = xmlOptionNode.Attributes["value"].Value;

                            if (!settings.ContainsKey(_key))
                                settings.Add(_key, _value);
                        }
                    }

                    /* Find the algorithm */
                    foreach(IAlgorithmProvider provider in m_Algorithms.Providers)
                    {
                        if (provider.GetType().FullName.Equals(_id))
                        {
                            provider.SetSettings(settings);
                            m_Provider = provider;

                            break;
                        }
                    }
                }
                catch
                {
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public Boolean Save()
        {
            try
            {
                /* Create new XmlDocument */
                XmlDocument xmlDoc = new XmlDocument();

                /* Add root */
                XmlElement root = xmlDoc.CreateElement("configuration");
                xmlDoc.AppendChild(root);

                XmlAttribute _localBalancer = xmlDoc.CreateAttribute("lbid");
                _localBalancer.Value = m_LocalBalancer.FullName;
                root.Attributes.Append(_localBalancer);

                XmlAttribute _remoteBalancer = xmlDoc.CreateAttribute("rbid");
                _remoteBalancer.Value = m_RemoteBalancer.FullName;
                root.Attributes.Append(_remoteBalancer);

                XmlAttribute _queueSize = xmlDoc.CreateAttribute("qsize");
                _queueSize.Value = m_QueueSize.ToString();
                root.Attributes.Append(_queueSize);

                XmlAttribute _numberOfThreads = xmlDoc.CreateAttribute("thc");
                _numberOfThreads.Value = m_NumberOfThreads.ToString();
                root.Attributes.Append(_numberOfThreads);

                XmlElement serverListElement = xmlDoc.CreateElement("servers");
                root.AppendChild(serverListElement);

                /* Append all servers form the list */
                foreach (IServer server in m_Servers)
                {
                    XmlElement srvElement = null;

                    if (server is LocalServer)
                    {
                        srvElement = xmlDoc.CreateElement("local");
                        XmlAttribute _enabled = xmlDoc.CreateAttribute("enabled");
                        _enabled.Value = Convert.ToString(server.Enabled ? 1 : 0);

                        srvElement.Attributes.Append(_enabled);
                    }
                    else if (server is RemoteServer)
                    {
                        RemoteServer rServer = (RemoteServer)server;
                        srvElement = xmlDoc.CreateElement("remote");

                        XmlAttribute _enabled = xmlDoc.CreateAttribute("enabled");
                        _enabled.Value = Convert.ToString(server.Enabled ? 1 : 0);
                        srvElement.Attributes.Append(_enabled);

                        XmlAttribute _name = xmlDoc.CreateAttribute("name");
                        _name.Value = rServer.Name ?? String.Empty;
                        srvElement.Attributes.Append(_name);

                        XmlAttribute _host = xmlDoc.CreateAttribute("host");
                        _host.Value = rServer.Host ?? String.Empty;
                        srvElement.Attributes.Append(_host);

                        XmlAttribute _secKey = xmlDoc.CreateAttribute("key");
                        _secKey.Value = rServer.SecurityKey ?? String.Empty;
                        srvElement.Attributes.Append(_secKey);

                        XmlAttribute _port = xmlDoc.CreateAttribute("port");
                        _port.Value = Convert.ToString(rServer.Port);
                        srvElement.Attributes.Append(_port);

                        XmlAttribute _ctype = xmlDoc.CreateAttribute("ctype");
                        _ctype.Value = Convert.ToString((Int32)rServer.ConnectionType);
                        srvElement.Attributes.Append(_ctype);
                    }

                    if (srvElement != null)
                        serverListElement.AppendChild(srvElement);
                }

                /* Append configuration option and plugins */
                if (m_Provider != null)
                {
                    XmlElement providerElement = xmlDoc.CreateElement("provider");

                    /* Name/Assembly */
                    XmlAttribute _name = xmlDoc.CreateAttribute("id");
                    _name.Value = m_Provider.GetType().FullName;
                    providerElement.Attributes.Append(_name);

                    /* Options */
                    Dictionary<String, String> options = m_Provider.GetSettings();

                    foreach (String key in options.Keys)
                    {
                        XmlElement optionElement = xmlDoc.CreateElement("option");

                        XmlAttribute _optionName = xmlDoc.CreateAttribute("key");
                        _optionName.Value = key;
                        optionElement.Attributes.Append(_optionName);

                        XmlAttribute _optionValue = xmlDoc.CreateAttribute("value");
                        _optionValue.Value = options[key];
                        optionElement.Attributes.Append(_optionValue);

                        providerElement.AppendChild(optionElement);
                    }

                    root.AppendChild(providerElement);
                }


                xmlDoc.Save(m_XmlFileName);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets or sets the algorithm provider.
        /// </summary>
        /// <value>The provider.</value>
        public IAlgorithmProvider Provider
        {
            get
            {
                return m_Provider;
            }
            set
            {
                m_Provider = value;
            }
        }

        /// <summary>
        /// Gets or sets the local balancer.
        /// </summary>
        /// <value>The local balancer.</value>
        public Type LocalBalancer
        {
            get
            {
                return m_LocalBalancer;
            }
            set
            {
                if (value != typeof(FairLoadBalancer) && value != typeof(RRLoadBalancer) && value != typeof(PredictiveLoadBalancer))
                    throw new ArgumentException("value");

                m_LocalBalancer = value;
            }
        }

        /// <summary>
        /// Gets or sets the remote balancer.
        /// </summary>
        /// <value>The remote balancer.</value>
        public Type RemoteBalancer
        {
            get
            {
                return m_RemoteBalancer;
            }
            set
            {
                if (value != typeof(FairLoadBalancer) && value != typeof(RRLoadBalancer) && value != typeof(PredictiveLoadBalancer))
                    throw new ArgumentException("value");

                m_RemoteBalancer = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the queue.
        /// </summary>
        /// <value>The size of the queue.</value>
        public Int32 QueueSize
        {
            get
            {
                return m_QueueSize;
            }
            set
            {
                m_QueueSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of threads.
        /// </summary>
        /// <value>The number of threads.</value>
        public Int32 NumberOfThreads
        {
            get { return m_NumberOfThreads; }
            set { m_NumberOfThreads = value; }
        }

        #endregion

        #region ICloneable Members

        public Object Clone()
        {
            XmlStoredWorkSpace newCopy = new XmlStoredWorkSpace(m_XmlFileName);

            foreach(IServer server in m_Servers)
            {
                newCopy.m_Servers.Add((IServer)server.Clone());
            }

            return newCopy;
        }

        #endregion
    }
}
