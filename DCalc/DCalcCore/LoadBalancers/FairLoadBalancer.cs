using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.LoadBalancers
{
    /// <summary>
    /// Fair Load Balancer. This class is thread-safe.
    /// </summary>
    public sealed class FairLoadBalancer : ILoadBalancer
    {
        #region Private Fields

        private Dictionary<Object, Int32> m_Objects = new Dictionary<Object, Int32>();
        private String m_SyncRoot = "FairLoadBalancer Sync";

        #endregion

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
                if (!m_Objects.ContainsKey(obj))
                    m_Objects.Add(obj, 0);
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
                if (m_Objects.ContainsKey(obj))
                    m_Objects.Remove(obj);
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
                Int32 minValue = 0;
                Object minValueObj = null;

                /* Get the smaller load */
                foreach (Object obj in m_Objects.Keys)
                {
                    Int32 currentValue = m_Objects[obj];

                    if (minValueObj == null || currentValue < minValue)
                    {
                        minValueObj = obj;
                        minValue = currentValue;
                    }
                }

                if (minValueObj != null)
                    m_Objects[minValueObj] = minValue + 1;

                return minValueObj;
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
                if (m_Objects.ContainsKey(obj))
                {
                    Int32 currentValue = m_Objects[obj];
                    m_Objects[obj] = currentValue - 1;
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
            /* Consider the same as done! */
            ObjectDone(obj);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (m_SyncRoot)
            {
                m_Objects.Clear();
            }
        }

        /// <summary>
        /// Resets the balance. All object will have the same chance to be selected after this call.
        /// </summary>
        public void ResetBalance()
        {
            lock (m_SyncRoot)
            {
                List<Object> objList = new List<Object>(m_Objects.Keys);

                foreach (Object obj in objList)
                {
                    m_Objects[obj] = 0;
                }
            }
        }

        #endregion
    }
}
