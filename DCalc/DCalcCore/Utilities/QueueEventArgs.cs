using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;

namespace DCalcCore.Utilities
{
    /// <summary>
    /// Arguments used in Queue reporting events. This class is thread-safe.
    /// </summary>
    public sealed class QueueEventArgs : EventArgs
    {
        #region Private Fields

        private ScalarSet m_OutSet;
        private Int32 m_SetId; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueEventArgs"/> class.
        /// </summary>
        /// <param name="outSet">The out set.</param>
        /// <param name="setId">The set id.</param>
        internal QueueEventArgs(ScalarSet outSet, Int32 setId)
        {
            m_OutSet = outSet;
            m_SetId = setId;
        } 

        #endregion

        #region QueueEventArgs Public Properties

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

        #endregion
    }
}
