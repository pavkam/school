using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;

namespace DCalcCore.Remoting.Common
{
    /// <summary>
    /// Provides deserialization options for Scripts. This class is thread-safe.
    /// </summary>
    internal class ScriptDeserializer
    {
        #region Private Fields

        private String m_Script;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptDeserializer"/> class.
        /// </summary>
        /// <param name="script">The script.</param>
        public ScriptDeserializer(String script)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            if (script.Length == 0)
                throw new ArgumentException("script");

            m_Script = script;
        } 

        #endregion

        #region ScriptDeserializer Public Methods

        /// <summary>
        /// Deserializes this instance.
        /// </summary>
        /// <returns></returns>
        public IScript Deserialize()
        {
            /* Find the first : and split it by that char */
            Int32 delimPos = m_Script.IndexOf(':');

            if (delimPos > 0)
            {
                try
                {
                    String name = m_Script.Substring(0, delimPos);
                    String body = m_Script.Substring(delimPos + 1, m_Script.Length - delimPos - 1);

                    return new TransparentScript(name, body);
                }
                catch
                {
                }
            }

            return null;
        } 

        #endregion
    }
}
