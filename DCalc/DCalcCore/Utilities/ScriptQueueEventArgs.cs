using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;

namespace DCalcCore.Utilities
{
    /// <summary>
    /// Arguments used in script's Queue reporting events. This class is thread-safe.
    /// </summary>
    public sealed class ScriptQueueEventArgs : EventArgs
    {
        #region Private Fields

        private ScalarSet m_OutSet;
        private Int32 m_SetId;
        private IScript m_Script;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueEventArgs"/> class.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="outSet">The out set.</param>
        /// <param name="setId">The set id.</param>
        internal ScriptQueueEventArgs(IScript script, ScalarSet outSet, Int32 setId)
        {
            m_OutSet = outSet;
            m_SetId = setId;
            m_Script = script;
        } 

        #endregion

        #region ScriptQueueEventArgs Public Properties

        /// <summary>
        /// Gets the output set.
        /// </summary>
        /// <value>The output set.</value>
        public ScalarSet OutputSet
        {
            get { return m_OutSet; }
        }

        /// <summary>
        /// Gets the set id.
        /// </summary>
        /// <value>The set id.</value>
        public Int32 SetId
        {
            get { return m_SetId; }
        }

        /// <summary>
        /// Gets the script.
        /// </summary>
        /// <value>The script.</value>
        public IScript Script
        {
            get { return m_Script; }
        }

        #endregion
    }
}
