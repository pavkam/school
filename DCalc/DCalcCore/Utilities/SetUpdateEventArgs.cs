using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.Utilities
{
    /// <summary>
    /// Arguments for set related events. This class is thread-safe.
    /// </summary>
    public sealed class SetUpdateEventArgs : EventArgs
    {
        #region Private Fields

        private String m_ClientSignature;
        private Int32 m_CountOfSets; 

        #endregion

        #region Constructors

        internal SetUpdateEventArgs(String clientSignature, Int32 countOfSets)
        {
            if (clientSignature == null)
                throw new ArgumentNullException("clientSignature");

            m_ClientSignature = clientSignature;
            m_CountOfSets = countOfSets;
        }

        #endregion

        #region SetUpdateEventArgs Public Properties

        /// <summary>
        /// Gets the signature of the client.
        /// </summary>
        /// <value>The signature.</value>
        public String Signature
        {
            get { return m_ClientSignature; }
        }

        /// <summary>
        /// Gets the set count.
        /// </summary>
        /// <value>The set count.</value>
        public Int32 SetCount
        {
            get { return m_CountOfSets; }
        } 

        #endregion
    }
}
