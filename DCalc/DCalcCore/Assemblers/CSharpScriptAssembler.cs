using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using DCalcCore.Algorithm;
using DCalcDynHelper;
using System.IO;

namespace DCalcCore.Assemblers
{
    /// <summary>
    /// C# script assembler. This class is thread-safe.
    /// </summary>
    public sealed class CSharpScriptAssembler : IScriptAssembler
    {
        #region IScriptAssembler Members

        /// <summary>
        /// Assembles the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public ICompiledScript Assemble(IScript script)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            CSharpCodeProvider compiler = new CSharpCodeProvider();
            CompilerParameters compilerParams = new CompilerParameters();

            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add(AppDomain.CurrentDomain.BaseDirectory + "DCalcDynHelper.dll");

            compilerParams.GenerateExecutable = false;
            compilerParams.GenerateInMemory = false;

            /* Assign a unique GUID for that new assembly name */
            String assemblyName = Guid.NewGuid().ToString();
            String assemblyPath = AppDomain.CurrentDomain.BaseDirectory + assemblyName + ".dll";
            compilerParams.OutputAssembly = assemblyPath;

            StringBuilder code = new StringBuilder();
            code.Append("using System;");
            code.Append("using System.Collections.Generic;");
            code.Append("using System.Text;");
            code.Append("using System.Reflection;");
            code.Append("using DCalcDynHelper;");
            code.Append("namespace Evaluator {");
            code.Append("  public class _Evaluator : MarshalByRefObject, IRemoteCall {");

            /* Adding the interface call */
            code.Append("public Object __CallRemotely(String methodName, Object[] parameters) {");
            code.Append("return GetType().InvokeMember(methodName, BindingFlags.InvokeMethod, null, this, parameters); }");

            /* Adding the method */
            code.Append("public ");
            code.Append(script.MethodBody);
            code.Append("} }");

            CompilerResults compilerResults = compiler.CompileAssemblyFromSource(compilerParams, code.ToString());

            if (compilerResults.Errors.HasErrors)
            {
                return null;
            }

            /* Compilation succeeded! Let's do more! */

            /* Create a new Application Domain */

            AppDomain newAppDomain = null;

            try
            {
                AppDomainSetup adSetup = new AppDomainSetup();
                adSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                newAppDomain = AppDomain.CreateDomain(assemblyName, null, adSetup);

                /* Failed? */
                if (newAppDomain == null)
                    return null;
            }
            catch
            {
                /* Failed! */
                return null;
            }

            RemoteCallFactory factory = null;

            try
            {
                /* Create the instance of the class remotely */
                factory = (RemoteCallFactory)newAppDomain.CreateInstance("DCalcDynHelper", "DCalcDynHelper.RemoteCallFactory").Unwrap();
            }
            catch
            {
            }

            if (factory != null)
            {
                /* Success! we have the factory now */
                Object rmObject = factory.Create(assemblyPath, "Evaluator._Evaluator", null);

                if (rmObject != null)
                {
                    IRemoteCall rmCall = (IRemoteCall)rmObject;

                    /* We now have what we need! Let's wrap it and return a compiled script then */
                    return new DotNetCompiledScript(newAppDomain, assemblyPath, script.MethodName, rmCall);
                }
            }

            try
            {
                /* If we got here - something is wrong */
                AppDomain.Unload(newAppDomain);
                File.Delete(assemblyPath);
            }
            catch
            {
            }

            return null;
        }
        #endregion
        
    }
}
