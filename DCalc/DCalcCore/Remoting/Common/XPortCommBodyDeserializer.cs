using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.Remoting.Common
{
    /// <summary>
    /// Provides serialization options for transfer body. This class is thread-safe.
    /// </summary>
    internal class XPortCommBodyDeserializer
    {
        #region Private Fields

        private String m_Message;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpCommBodyDeserializer"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public XPortCommBodyDeserializer(String message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (message.Length == 0)
                throw new ArgumentException("message");

            m_Message = message;
        } 

        #endregion

        #region HttpCommBodyDeserializer Public Methods

        /// <summary>
        /// Deserializes this instance.
        /// </summary>
        /// <returns></returns>
        public XPortCommBody Deserialize()
        {
            /* Find the first Space and split it by that char */
            Int32 spacePos = m_Message.IndexOf(' ');
            String code;
            String content = String.Empty;

            if (spacePos == -1)
            {
                code = m_Message;
            }
            else
            {
                try
                {
                    code = m_Message.Substring(0, spacePos).ToUpper();
                    content = m_Message.Substring(spacePos + 1, m_Message.Length - spacePos - 1);
                }
                catch
                {
                    return null;
                }
            }

            return new XPortCommBody(code, content);
        } 

        #endregion
    }
}
