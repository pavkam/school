using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace DCalcDynHelper
{
    /// <summary>
    /// Provides a direct access to remote callable object in the AppDomain.
    /// </summary>
    public sealed class RemoteCallFactory : MarshalByRefObject
    {
        #region Private Fields

        private const BindingFlags m_BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteCallFactory"/> class.
        /// </summary>
        public RemoteCallFactory()
        {

        } 

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the a new instance of remote callable object.
        /// </summary>
        /// <param name="assemblyFile">The assembly file.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="constructorArgs">The constructor args.</param>
        /// <returns></returns>
        public IRemoteCall Create(String assemblyFile, String typeName, Object[] constructorArgs)
        {
            return (IRemoteCall)Activator.CreateInstanceFrom(assemblyFile, typeName, false, m_BindingFlags, null, constructorArgs,
               null, null, null).Unwrap();
        } 

        #endregion
    }
}
