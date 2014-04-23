using System;
using System.Collections.Generic;
using System.Text;

namespace DCalc.Communication
{
    /// <summary>
    /// Prvides a "Local Machine" server.
    /// </summary>
    public sealed class LocalServer : IServer
    {
        #region Private Fields

        private String m_ServerName;
        private Boolean m_Enabled;
        private ServerStatus m_ServerStatus;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalServer"/> class.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        public LocalServer(Boolean isEnabled)
        {
            m_ServerName = "Local Machine";
            m_Enabled = isEnabled;
        } 

        #endregion

        #region IServer Members

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <value>The signature.</value>
        public String Signature
        {
            get { return m_ServerName; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public String Name
        {
            get { return m_ServerName; }
            set 
            {
                /* Doesn't actually chnage it */
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
                return m_ServerStatus;
            }

            set
            {
                m_ServerStatus = value;
            }
        }

        #endregion

        #region ICloneable Members

        public Object Clone()
        {
            LocalServer copy = new LocalServer(m_Enabled);
            return copy;
        }

        #endregion
    }
}
