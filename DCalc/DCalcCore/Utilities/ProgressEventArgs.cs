using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.Utilities
{
    /// <summary>
    /// Arguments used in progress-reporting events. This class is thread-safe.
    /// </summary>
    public sealed class ProgressEventArgs : EventArgs
    {
        #region Private Fields

        private Int32 m_CurrentValue;
        private Int32 m_MaxValue; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressEventArgs"/> class.
        /// </summary>
        /// <param name="currentValue">The current value.</param>
        /// <param name="maxValue">The max value.</param>
        internal ProgressEventArgs(Int32 currentValue, Int32 maxValue)
        {
            m_CurrentValue = currentValue;
            m_MaxValue = maxValue;
        } 

        #endregion

        #region ProgressEventArgs Public Properties

        /// <summary>
        /// Gets the current progress value.
        /// </summary>
        /// <value>Progress value.</value>
        public Int32 Current
        {
            get { return m_CurrentValue; }
        }

        /// <summary>
        /// Gets the maximal progress value.
        /// </summary>
        /// <value>Maximal value.</value>
        public Int32 Max
        {
            get { return m_MaxValue; }
        } 

        #endregion
    }
}
