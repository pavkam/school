using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.LoadBalancers
{
    /// <summary>
    /// Predictive Load Balancer. This class is thread-safe.
    /// </summary>
    public sealed class PredictiveLoadBalancer : ILoadBalancer
    {
        #region Private Fields

        private Dictionary<Object, Int32> m_ObjectLoad = new Dictionary<Object, Int32>();
        private Dictionary<Object, Int32> m_ObjectFailureCount = new Dictionary<Object, Int32>();

        private String m_SyncRoot = "PredictiveLoadBalancer Sync";

        #endregion

        private Int32 GetObjectSelectionFactor(Object obj)
        {
            /* Decrease the failure count */
            if (m_ObjectFailureCount[obj] > 0)
                m_ObjectFailureCount[obj] = m_ObjectFailureCount[obj] - 1;

            return m_ObjectLoad[obj] + m_ObjectFailureCount[obj];
        }

        #region ILoadBalancer Members

        /// <summary>
        /// Registers the object into the observed pool.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void RegisterObject(Object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            lock (m_SyncRoot)
            {
                if (!m_ObjectLoad.ContainsKey(obj))
                {
                    m_ObjectLoad.Add(obj, 0);
                    m_ObjectFailureCount.Add(obj, 0);
                }
            }
        }

        /// <summary>
        /// Unregisters the object from the observer pool.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void UnregisterObject(Object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            lock (m_SyncRoot)
            {
                if (m_ObjectLoad.ContainsKey(obj))
                {
                    m_ObjectFailureCount.Remove(obj);
                    m_ObjectLoad.Remove(obj);
                }
            }
        }

        /// <summary>
        /// Selects the next object using balancing.
        /// </summary>
        /// <returns></returns>
        public Object SelectObject()
        {
            lock (m_SyncRoot)
            {
                Int32 minFactor = 0;
                Object minFactorObj = null;

                /* Get the smaller load */
                foreach (Object obj in m_ObjectLoad.Keys)
                {
                    Int32 currentFactor = GetObjectSelectionFactor(obj);

                    if (minFactorObj == null || currentFactor < minFactor)
                    {
                        minFactorObj = obj;
                        minFactor = currentFactor;
                    }
                }

                if (minFactorObj != null)
                {
                    /* Update object load factor */
                    m_ObjectLoad[minFactorObj] = m_ObjectLoad[minFactorObj] + 1;
                }

                return minFactorObj;
            }
        }

        /// <summary>
        /// Returns back the object to the observed pool.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void ObjectDone(Object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            lock (m_SyncRoot)
            {
                if (m_ObjectLoad.ContainsKey(obj))
                {
                    /* Update object load */
                    m_ObjectLoad[obj] = m_ObjectLoad[obj] - 1;
                    m_ObjectFailureCount[obj] = m_ObjectFailureCount[obj] - 2;

                    if (m_ObjectFailureCount[obj] < 0)
                        m_ObjectFailureCount[obj] = 0;
                }
            }
        }

        /// <summary>
        /// Returns back the object to the observed pool.
        /// Marks the object as failed for those units.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void ObjectFailed(Object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            lock (m_SyncRoot)
            {
                if (m_ObjectLoad.ContainsKey(obj))
                {
                    /* Update object load */
                    m_ObjectLoad[obj] = m_ObjectLoad[obj] - 1;

                    /* Update object failure counts */
                    m_ObjectFailureCount[obj] = m_ObjectFailureCount[obj] + 3;
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
                m_ObjectLoad.Clear();
                m_ObjectFailureCount.Clear();
            }
        }

        /// <summary>
        /// Resets the balance. All object will have the same chance to be selected after this call.
        /// </summary>
        public void ResetBalance()
        {
            lock (m_SyncRoot)
            {
                List<Object> objList = new List<Object>(m_ObjectFailureCount.Keys);

                foreach (Object obj in objList)
                {
                    m_ObjectLoad[obj] = 0;
                    m_ObjectFailureCount[obj] = 0;
                }
            }
        }

        #endregion
    }
}
