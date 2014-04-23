using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.LoadBalancers
{
    /// <summary>
    /// Round Robin Load Balancer. This class is thread-safe.
    /// </summary>
    public sealed class RRLoadBalancer : ILoadBalancer
    {
        #region Private Fields

        private List<Object> m_Objects = new List<Object>();
        private String m_SyncRoot = "RRLoadBalancer Sync";

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
                if (!m_Objects.Contains(obj))
                    m_Objects.Add(obj);
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
                if (m_Objects.Contains(obj))
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
                /* Get the first object */

                if (m_Objects.Count == 0)
                    return null;

                Object getMe = m_Objects[0];
                m_Objects.RemoveAt(0);
                m_Objects.Add(getMe);

                return getMe;
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
                /* Don't do anything. */
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
                /* Nothing to correct in Round-Robin */
            }
        }

        #endregion
    }
}
