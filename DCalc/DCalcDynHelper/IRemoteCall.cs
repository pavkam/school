using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcDynHelper
{
    /// <summary>
    /// Provides calling interface for the new AppDomain based script.
    /// </summary>
    public interface IRemoteCall
    {
        /// <summary>
        /// Calls the specified method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        Object __CallRemotely(String methodName, Object[] parameters);
    }
}
