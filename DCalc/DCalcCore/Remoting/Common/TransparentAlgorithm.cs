using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Utilities;
using DCalcCore.Algorithm;

namespace DCalcCore.Remoting.Common
{
    /// <summary>
    /// Implements a simple carrier script to be used in remote servers.
    /// </summary>
    public sealed class TransparentScript : IScript
    {
        #region Private Fields

        private String m_MethodName;
        private String m_MethodBody; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransparentScript"/> class.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodBody">The method body.</param>
        public TransparentScript(String methodName, String methodBody)
        {
            if (methodName == null)
                throw new ArgumentNullException(methodName);

            if (methodBody == null)
                throw new ArgumentNullException(methodBody);

            m_MethodName = methodName;
            m_MethodBody = methodBody;
        } 

        #endregion

        #region IAlgorithm Members

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public string MethodName
        {
            get { return m_MethodName; }
        }

        /// <summary>
        /// Gets the method body.
        /// </summary>
        /// <value>The method body.</value>
        public string MethodBody
        {
            get { return m_MethodBody; }
        }

        #endregion
    }
}
