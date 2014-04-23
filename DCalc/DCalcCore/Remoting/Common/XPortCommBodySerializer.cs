using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.Remoting.Common
{
    /// <summary>
    /// Provides serialization routing for body. This class is thread-safe.
    /// </summary>
    internal class XPortCommBodySerializer
    {
        #region Private Fields

        private XPortCommBody m_Body;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpCommBodySerializer"/> class.
        /// </summary>
        /// <param name="body">The body.</param>
        public XPortCommBodySerializer(XPortCommBody body)
        {
            if (body == null)
                throw new ArgumentNullException("body");

            m_Body = body;
        }

        #endregion

        #region HttpCommBodySerializer Public Methods

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns></returns>
        public String Serialize()
        {
            if (m_Body.Content == null || m_Body.Content.Length == 0)
                return m_Body.Code.ToUpper();
            else
                return String.Format("{0} {1}", m_Body.Code.ToUpper(), m_Body.Content);
        } 

        #endregion
    }
}
