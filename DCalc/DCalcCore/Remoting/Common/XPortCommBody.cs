using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.Remoting.Common
{
    /// <summary>
    /// Provides a very simple transport object. This class is thread-safe.
    /// </summary>
    internal sealed class XPortCommBody
    {
        #region Private Fields

        private String m_Code;
        private String m_Content;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpCommBody"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="content">The content.</param>
        public XPortCommBody(String code, String content)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            if (code.Length == 0)
                throw new ArgumentException("code");

            m_Code = code;
            m_Content = content ?? String.Empty;
        } 

        #endregion

        #region HttpCommBody Public Properties

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        public String Code
        {
            get { return m_Code; }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>The content.</value>
        public String Content
        {
            get { return m_Content; }
        } 

        #endregion
    }
}
