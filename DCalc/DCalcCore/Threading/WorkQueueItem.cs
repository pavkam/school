using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Assemblers;
using DCalcCore.Utilities;
using DCalcCore.Algorithm;

namespace DCalcCore.Threading
{
    /// <summary>
    /// Represents an item in the work queue.
    /// </summary>
    internal sealed class WorkQueueItem
    {
        #region Private Fields

        private ICompiledScript m_CompiledScript;
        private IScript m_Script;
        private ScalarSet m_InputSet;
        private Boolean m_IsCancelled; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueueItem"/> class.
        /// </summary>
        /// <param name="compiledScript">The compiled script.</param>
        /// <param name="script">The script.</param>
        /// <param name="scalarSet">The scalar set.</param>
        public WorkQueueItem(ICompiledScript compiledScript, IScript script, ScalarSet scalarSet)
        {
            if (compiledScript == null)
                throw new ArgumentNullException("compiledScript");

            if (script == null)
                throw new ArgumentNullException("script");

            if (scalarSet == null)
                throw new ArgumentNullException("scalarSet");

            m_CompiledScript = compiledScript;
            m_InputSet = scalarSet;
            m_Script = script;
        }

        #endregion

        #region WorkQueueItem Public Properties

        public ICompiledScript CompiledScript
        {
            get { return m_CompiledScript; }
        }

        public IScript Script
        {
            get { return m_Script; }
        }

        public ScalarSet InputSet
        {
            get { return m_InputSet; }
        }

        public Boolean Cancelled
        {
            get { return m_IsCancelled; }
            set { m_IsCancelled = true; }
        } 

        #endregion
    }
}
