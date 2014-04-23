using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DCalcCore.Utilities;
using DCalcDynHelper;
using System.IO;

namespace DCalcCore.Assemblers
{
    /// <summary>
    /// .NET compiled script. This class is thread-safe.
    /// </summary>
    public sealed class DotNetCompiledScript : ICompiledScript
    {
        #region Private Fields

        private AppDomain m_Domain;
        private IRemoteCall m_RemoteCall;
        private String m_AssemblyFileName;
        private String m_MethodName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCompiledScript"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="remoteCall">The remote call.</param>
        public DotNetCompiledScript(AppDomain domain, String assemblyFileName, String methodName, IRemoteCall remoteCall)
        {
            if (domain == null)
                throw new ArgumentNullException("domain");

            if (remoteCall == null)
                throw new ArgumentNullException("remoteCall");

            if (assemblyFileName == null)
                throw new ArgumentNullException("assemblyFileName");

            if (methodName == null)
                throw new ArgumentNullException("methodName");

            m_AssemblyFileName = assemblyFileName;
            m_Domain = domain;
            m_RemoteCall = remoteCall;
            m_MethodName = methodName;
        } 

        #endregion

        #region ICompiledScript Members

        /// <summary>
        /// Executes the specified script.
        /// </summary>
        /// <param name="set">The set to use as input.</param>
        /// <returns>Output result set</returns>
        public ScalarSet Execute(ScalarSet set)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            Object result = m_RemoteCall.__CallRemotely(m_MethodName, set.AsInvokeParameters);

            if (result == null)
            {
                return null;
            }

            /* Check for basic types */
            if (result is Byte || result is SByte || result is Int16 || result is UInt16 ||
                result is Int32 || result is UInt32 || result is Int64 || result is UInt64 || result is Single ||
                result is Double || result is String)
                return new ScalarSet(set.Id, result);
            else if (result is Object[])
                return new ScalarSet(set.Id, (Object[])result);
            else return null;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            /* Destroy all associated info */
            try
            {
                AppDomain.Unload(m_Domain);
                File.Delete(m_AssemblyFileName);
            }
            catch
            {
            }
        }

        #endregion
    }
}
