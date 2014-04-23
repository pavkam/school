using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;

namespace DCalcCore.Remoting.Common
{
    /// <summary>
    /// Provides serialization options for scripts. This class is thread-safe.
    /// </summary>
    internal class ScriptSerializer
    {
        #region Private Fields

        private IScript m_Script;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptSerializer"/> class.
        /// </summary>
        /// <param name="script">The script.</param>
        public ScriptSerializer(IScript script)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            m_Script = script;
        } 

        #endregion

        #region ScriptSerializer Public Methods

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns></returns>
        public String Serialize()
        {
            return String.Format("{0}:{1}", m_Script.MethodName, m_Script.MethodBody);
        } 

        #endregion
    }
}
