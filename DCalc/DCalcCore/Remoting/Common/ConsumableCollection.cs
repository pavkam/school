using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Utilities;

namespace DCalcCore.Remoting.Common
{
    /// <summary>
    /// A collection that provides options to track down resources. This class is thread-safe.
    /// </summary>
    /// <typeparam name="C">Consumer type.</typeparam>
    public class ConsumableCollection<C>
    {
        #region Private Fields

        private Dictionary<C, Queue<ScalarSet>> m_ConsumableList = new Dictionary<C, Queue<ScalarSet>>();
        private Dictionary<C, List<ScalarSet>> m_ConsumedList = new Dictionary<C, List<ScalarSet>>();
        private String m_SyncRoot = "ConsumableCollection Sync"; 

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers the consumer.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        private void RegisterConsumer(C consumer)
        {
            m_ConsumableList.Add(consumer, new Queue<ScalarSet>());
            m_ConsumedList.Add(consumer, new List<ScalarSet>());
        } 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumableCollection&lt;C&gt;"/> class.
        /// </summary>
        public ConsumableCollection()
        {
        } 

        #endregion

        #region ConsumableCollection Public Methods

        /// <summary>
        /// Consumes a new scalar set.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <returns></returns>
        public ScalarSet Consume(C consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer");

            lock (m_SyncRoot)
            {
                List<ScalarSet> groups = Consume(consumer, 1);

                if (groups.Count == 1)
                    return groups[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// Consumes a number of scalar sets.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="count">The count of sets to consume.</param>
        /// <returns></returns>
        public List<ScalarSet> Consume(C consumer, Int32 count)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer");

            if (count < 1)
                throw new ArgumentNullException("count");

            lock (m_SyncRoot)
            {
                if (!m_ConsumableList.ContainsKey(consumer))
                {
                    RegisterConsumer(consumer);
                }

                Queue<ScalarSet> c_queue = m_ConsumableList[consumer];
                List<ScalarSet> c_consumed = m_ConsumedList[consumer];

                List<ScalarSet> c_list = new List<ScalarSet>();
                Int32 countToGet = ((count < c_queue.Count) ? count : c_queue.Count);

                for (Int32 i = 0; i < countToGet; i++)
                {
                    ScalarSet group = c_queue.Dequeue();
                    c_consumed.Add(group);
                    c_list.Add(group);
                }

                return c_list;
            }
        }

        /// <summary>
        /// Consumes all new scalar sets.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <returns></returns>
        public List<ScalarSet> ConsumeAll(C consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer");

            lock (m_SyncRoot)
            {
                if (!m_ConsumableList.ContainsKey(consumer))
                {
                    RegisterConsumer(consumer);
                }

                Int32 countToConsume = m_ConsumableList[consumer].Count;

                if (countToConsume > 0)
                    return Consume(consumer, m_ConsumableList[consumer].Count);
                else
                    return null;
            }
        }

        /// <summary>
        /// Extracts the specified scalar set from this collection.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="scalarSet">The scalar set.</param>
        public void Extract(C consumer, List<ScalarSet> scalarSet)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer");

            if (scalarSet == null)
                throw new ArgumentNullException("scalarSet");

            lock (m_SyncRoot)
            {
                foreach (ScalarSet group in scalarSet)
                {
                    Extract(consumer, group);
                }
            }
        }

        /// <summary>
        /// Extracts the specified scalar set from this collection.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="scalarSet">The scalar set.</param>
        public void Extract(C consumer, ScalarSet scalarSet)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer");

            if (scalarSet == null)
                throw new ArgumentNullException("scalarSet");

            lock (m_SyncRoot)
            {
                if (!m_ConsumableList.ContainsKey(consumer))
                {
                    RegisterConsumer(consumer);
                }

                foreach (ScalarSet group in m_ConsumedList[consumer])
                {
                    if (group.Id == scalarSet.Id)
                    {
                        m_ConsumedList[consumer].Remove(group);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Places the a scalar set for a consumer to consume.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="scalarSet">The scalar set.</param>
        public void Place(C consumer, ScalarSet scalarSet)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer");

            if (scalarSet == null)
                throw new ArgumentNullException("scalarSet");

            lock (m_SyncRoot)
            {
                if (!m_ConsumableList.ContainsKey(consumer))
                {
                    RegisterConsumer(consumer);
                }

                m_ConsumableList[consumer].Enqueue(scalarSet);
            }
        }

        /// <summary>
        /// Places the a scalar set for a consumer to consume.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="scalarSets">The scalar sets.</param>
        public void Place(C consumer, ScalarSet[] scalarSets)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer");

            if (scalarSets == null)
                throw new ArgumentNullException("scalarSets");

            if (scalarSets.Length == 0)
                throw new ArgumentException("scalarSets");

            lock (m_SyncRoot)
            {
                if (!m_ConsumableList.ContainsKey(consumer))
                {
                    RegisterConsumer(consumer);
                }

                foreach (ScalarSet ss in scalarSets)
                {
                    m_ConsumableList[consumer].Enqueue(ss);
                }
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (m_SyncRoot)
            {
                m_ConsumableList.Clear();
                m_ConsumedList.Clear();
            }
        }

        /// <summary>
        /// Clears the queue for this consumer and retreives it.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <returns></returns>
        public List<ScalarSet> RevertAll(C consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer");

            lock (m_SyncRoot)
            {
                if (!m_ConsumableList.ContainsKey(consumer))
                {
                    RegisterConsumer(consumer);
                }

                /* Consume all first */
                ConsumeAll(consumer);

                List<ScalarSet> result = new List<ScalarSet>(m_ConsumedList[consumer]);

                m_ConsumableList.Remove(consumer);
                m_ConsumedList.Remove(consumer);

                return result;
            }
        }

        #endregion

        #region ConsumableCollection Public Properties

        /// <summary>
        /// Gets the list of all registered consumers.
        /// </summary>
        /// <value>The consumers.</value>
        public IEnumerable<C> Consumers
        {
            get
            {
                lock (m_SyncRoot)
                {
                    return new List<C>(m_ConsumableList.Keys);
                }
            }
        } 

        #endregion
    }
}
