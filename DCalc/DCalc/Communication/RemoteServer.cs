using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Remoting;

namespace DCalc.Communication
{
    /// <summary>
    /// Provides a remote server definition.
    /// </summary>
    public class RemoteServer : IServer
    {
        #region Private Fields

        private String m_ServerName;
        private String m_ServerHost;
        private Int32 m_ServerPort;
        private String m_ServerKey;
        private Boolean m_Enabled;
        private IRemoteGateClient m_Client;
        private ServerStatus m_Status;
        private ConnectionType m_ConnectionType;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteServer"/> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="serverHost">The server host.</param>
        /// <param name="serverPort">The server port.</param>
        /// <param name="securityKey">The security key.</param>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        public RemoteServer(String serverName, String serverHost, Int32 serverPort, String securityKey, 
            ConnectionType connectionType, Boolean isEnabled)
        {
            if (serverName == null)
                throw new ArgumentNullException("serverName");

            if (serverHost == null)
                throw new ArgumentNullException("serverHost");

            if (serverHost.Length == 0)
                throw new ArgumentException("serverHost");

            if (serverPort < 0 || serverPort > UInt16.MaxValue)
                throw new ArgumentException("serverPort");

            m_ServerName = serverName;
            m_ServerHost = serverHost;
            m_ServerPort = serverPort;
            m_Enabled = isEnabled;
            m_ServerKey = securityKey;
            m_ConnectionType = connectionType;
        }

        #endregion

        #region RemoteServer Public Properties

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public String Host
        {
            get { return m_ServerHost; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (value.Length == 0)
                    throw new ArgumentException("value");

                m_ServerHost = value;
            }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public Int32 Port
        {
            get { return m_ServerPort; }
            set
            {
                if (value < 0 || value > UInt16.MaxValue)
                    throw new ArgumentException("value");

                m_ServerPort = value;
            }
        }

        /// <summary>
        /// Gets or sets the security key.
        /// </summary>
        /// <value>The security key.</value>
        public String SecurityKey
        {
            get { return m_ServerKey; }
            set { m_ServerKey = value; }
        }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public IRemoteGateClient Client
        {
            get { return m_Client; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                m_Client = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the connection.
        /// </summary>
        /// <value>The type of the connection.</value>
        public ConnectionType ConnectionType
        {
            get { return m_ConnectionType; }
            set { m_ConnectionType = value; }
        }

        #endregion

        #region IServer Members

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <value>The signature.</value>
        public String Signature
        {
            get
            {
                return String.Format("{0} [{1}:{2}]", m_ServerName, m_ServerHost, m_ServerPort);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public String Name
        {
            get 
            {
                return m_ServerName;
            }

            set 
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                m_ServerName = value; 
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IServer"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public Boolean Enabled
        {
            get
            {
                return m_Enabled;
            }

            set
            {
                m_Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ServerStatus Status
        {
            get
            {
                return m_Status;
            }

            set
            {
                m_Status = value;
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public Object Clone()
        {
            RemoteServer copy = new RemoteServer(m_ServerName, m_ServerHost, m_ServerPort, m_ServerKey, m_ConnectionType, m_Enabled);
            return copy;
        }

        #endregion
    }
}
