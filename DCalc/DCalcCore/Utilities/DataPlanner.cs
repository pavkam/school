using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;

namespace DCalcCore.Utilities
{
    /// <summary>
    /// ScalarSet management class. Provides means to safely manipulate data parts.
    /// This class is thread-safe.
    /// </summary>
    public sealed class DataPlanner
    {
        #region Private Fields

        private IAlgorithm m_Algorithm;
        private Int32 m_NextInputSetId = 0;
        private List<Int32> m_PlannedSetIds = new List<Int32>();
        private Stack<Int32> m_FailedSetIds = new Stack<Int32>();
        private Int32 m_CompletedCount;
        private String m_SyncRoot = "DataPlanner Sync";

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the next available set.
        /// </summary>
        /// <returns></returns>
        private ScalarSet GetNextSet()
        {
            /* First let's take the next set from the algorithm */
            if (m_NextInputSetId < m_Algorithm.InputSetCount)
            {
                /* Plan this set */
                ScalarSet inputSet = m_Algorithm.GetInputSet(m_NextInputSetId);

                Int32 setId = m_NextInputSetId;
                m_PlannedSetIds.Add(m_NextInputSetId++);

                return inputSet;
            }
            else if (m_FailedSetIds.Count > 0)
            {
                /* No more sets left! Let's check the failed ones */
                Int32 failedSetId = m_FailedSetIds.Pop();

                /* Plan this set again */
                m_PlannedSetIds.Add(failedSetId);
                ScalarSet inputSet = m_Algorithm.GetInputSet(failedSetId);
                Int32 setId = failedSetId;

                return inputSet;
            }
            else
                return null; /* No more data available */
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPlanner&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        public DataPlanner(IAlgorithm algorithm)
        {
            if (algorithm == null)
                throw new ArgumentNullException("algorithm");

            m_Algorithm = algorithm;
        }

        #endregion

        #region DataPlanner Public Methods

        /// <summary>
        /// Consumes the specified number of sets.
        /// </summary>
        /// <param name="numberOfSets">The number of sets.</param>
        /// <returns></returns>
        public ScalarSet[] Consume(Int32 numberOfSets)
        {
            if (numberOfSets < 1)
                throw new ArgumentException("numberOfSets");

            lock (m_SyncRoot)
            {
                List<ScalarSet> result = new List<ScalarSet>();

                for (Int32 i = 0; i < numberOfSets; i++)
                {
                    ScalarSet newSet = GetNextSet();

                    if (newSet != null)
                    {
                        result.Add(newSet);
                    }
                    else
                        break; /* No more? */
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// Consumes one single set.
        /// </summary>
        /// <returns></returns>
        public ScalarSet Consume()
        {
            ScalarSet[] sets = Consume(1);

            if (sets != null && sets.Length == 1)
                return sets[0];
            else
                return null;
        }

        /// <summary>
        /// Confirms that a specified input set has been finished.
        /// </summary>
        /// <param name="inputSetId">The input set id.</param>
        public void Confirm(Int32 inputSetId)
        {
            lock (m_SyncRoot)
            {
                /* Remove this set from the list */
                m_PlannedSetIds.Remove(inputSetId);
                m_CompletedCount++;
            }
        }

        /// <summary>
        /// Returns the specified input set back into the pool.
        /// </summary>
        /// <param name="inputSetId">The input set id.</param>
        public void Return(Int32 inputSetId)
        {
            lock (m_SyncRoot)
            {
                /* Remove this set from the list and add it to failed sets */
                m_PlannedSetIds.Remove(inputSetId);
                m_FailedSetIds.Push(inputSetId);
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (m_SyncRoot)
            {
                m_PlannedSetIds.Clear();
                m_FailedSetIds.Clear();
                m_NextInputSetId = 0;
                m_CompletedCount = 0;
            }
        } 

        #endregion

        #region DataPlanner Public Properties

        /// <summary>
        /// Gets the completed count of sets.
        /// </summary>
        /// <value>The completed count.</value>
        public Int32 CompletedCount
        {
            get
            {
                lock (m_SyncRoot)
                {
                    return m_CompletedCount;
                }
            }
        }

        /// <summary>
        /// Gets the in progress count of sets.
        /// </summary>
        /// <value>The in progress count.</value>
        public Int32 InProgressCount
        {
            get
            {
                lock (m_SyncRoot)
                {
                    return m_PlannedSetIds.Count;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether more sets are available to consume.
        /// </summary>
        /// <value><c>true</c> if more available otherwise, <c>false</c>.</value>
        public Boolean MoreAvailable
        {
            get
            {
                lock (m_SyncRoot)
                {
                    Int32 canBeUsedCount = m_Algorithm.InputSetCount - (InProgressCount + CompletedCount);
                    return (canBeUsedCount > 0);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether all sets were completed.
        /// </summary>
        /// <value><c>true</c> if all completes otherwise, <c>false</c>.</value>
        public Boolean AllComplete
        {
            get
            {
                lock (m_SyncRoot)
                {
                    return (m_CompletedCount == m_Algorithm.InputSetCount);
                }
            }
        }

        #endregion
    }
}
