using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.Utilities
{
    /// <summary>
    /// Arguments for the connection events. This class is thread-safe.
    /// </summary>
    public sealed class ConnectionEventArgs : EventArgs
    {
        #region Private Fields

        private String m_ClientSignature; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionEventArgs"/> class.
        /// </summary>
        /// <param name="clientSignature">The client signature.</param>
        internal ConnectionEventArgs(String clientSignature)
        {
            if (clientSignature == null)
                throw new ArgumentNullException("clientSignature");

            m_ClientSignature = clientSignature;
        } 

        #endregion

        #region ConnectionEventArgs Public Properties

        /// <summary>
        /// Gets the signature of the server.
        /// </summary>
        /// <value>The signature.</value>
        public String Signature
        {
            get { return m_ClientSignature; }
        } 

        #endregion
    }
}
