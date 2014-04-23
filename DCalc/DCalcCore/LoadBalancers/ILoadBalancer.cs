using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.LoadBalancers
{
    /// <summary>
    /// Defines common interface for all Load Balancers.
    /// </summary>
    public interface ILoadBalancer
    {
        /// <summary>
        /// Registers the object into the observed pool.
        /// </summary>
        /// <param name="obj">The object.</param>
        void RegisterObject(Object obj);

        /// <summary>
        /// Unregisters the object from the observer pool.
        /// </summary>
        /// <param name="obj">The object.</param>
        void UnregisterObject(Object obj);

        /// <summary>
        /// Selects the next object using balancing.
        /// </summary>
        /// <returns></returns>
        Object SelectObject();

        /// <summary>
        /// Returns back the object to the observed pool.
        /// </summary>
        /// <param name="obj">The object.</param>
        void ObjectDone(Object obj);

        /// <summary>
        /// Returns back the object to the observed pool.
        /// Marks the object as failed for those units.
        /// </summary>
        /// <param name="obj">The object.</param>
        void ObjectFailed(Object obj);

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Resets the balance. All object will have the same chance to be selected after this call.
        /// </summary>
        void ResetBalance();
    }
}
