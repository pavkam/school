using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime;
using System.Reflection;
using DCalcCore.Algorithm;


namespace DCalc.Algorithms
{
    /// <summary>
    /// Provides a directory-based plugin loading. This class is thread-safe.
    /// </summary>
    public class DirectoryAlgorithmCollection : IAlgorithmCollection
    {
        #region Private Fields

        private String m_DllPattern = "*.dll";
        private String m_Directory;
        private List<IAlgorithmProvider> m_Providers = new List<IAlgorithmProvider>();
        private List<Assembly> m_Assemblies = new List<Assembly>();
        private String m_SyncRoot = "DirectoryAlgorithmCollection Sync";

        #endregion

        #region Private Methods

        /// <summary>
        /// Unloads the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        private void UnloadAssembly(Assembly assembly)
        {
            // TODO: Unload assemplies!
        }

        /// <summary>
        /// Tries to load an assembly.
        /// </summary>
        /// <param name="dllName">Name of the DLL.</param>
        /// <returns></returns>
        private Assembly TryLoadAssembly(String dllName)
        {
            try
            {
                return Assembly.LoadFile(dllName);
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Tries to load all providers.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        private IAlgorithmProvider[] TryLoadProviders(Assembly assembly)
        {
            try
            {
                Type[] allTypes = assembly.GetExportedTypes();
                List<IAlgorithmProvider> allProviders = new List<IAlgorithmProvider>();

                foreach (Type type in allTypes)
                {
                    Type[] interfaces = type.GetInterfaces();

                    Boolean isRight = false;

                    foreach (Type intf in interfaces)
                    {
                        if (intf == typeof(IAlgorithmProvider))
                        {
                            isRight = true;
                            break;
                        }
                    }

                    if (isRight)
                        allProviders.Add((IAlgorithmProvider)Activator.CreateInstance(type));
                }

                return allProviders.ToArray();
            }
            catch
            {
            }

            return null;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryAlgorithmCollection"/> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public DirectoryAlgorithmCollection(String directory)
        {
            if (directory == null)
                throw new ArgumentNullException(directory);

            m_Directory = directory;
        } 

        #endregion

        #region IAlgorithmCollection Members

        /// <summary>
        /// Gets all loaded providers.
        /// </summary>
        /// <value>The providers.</value>
        public IEnumerable<IAlgorithmProvider> Providers
        {
            get 
            {
                lock (m_SyncRoot)
                {
                    return new List<IAlgorithmProvider>(m_Providers);
                }
            }
        }

        /// <summary>
        /// Gets the count of loaded providers.
        /// </summary>
        /// <value>The count.</value>
        public Int32 Count
        {
            get 
            {
                lock (m_SyncRoot)
                {
                    return m_Providers.Count;
                }
            }
        }

        /// <summary>
        /// Loads all.
        /// </summary>
        public void LoadAll()
        {
            lock (m_SyncRoot)
            {
                UnLoadAll();

                try
                {
                    String[] allFiles = Directory.GetFiles(m_Directory, m_DllPattern);

                    foreach (String fileName in allFiles)
                    {
                        Assembly assembly = TryLoadAssembly(fileName);

                        if (assembly != null)
                        {
                            /* It was loaded succesefully */

                            IAlgorithmProvider[] _providers = TryLoadProviders(assembly);

                            if (_providers != null && _providers.Length > 0)
                            {
                                /* All OK */
                                m_Assemblies.Add(assembly);
                                m_Providers.AddRange(_providers);
                            }
                            else
                            {
                                UnloadAssembly(assembly);
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Unloads all.
        /// </summary>
        public void UnLoadAll()
        {
            lock (m_SyncRoot)
            {
                /* Clear providers */
                m_Providers.Clear();

                /* Unload all assemblies */
                foreach (Assembly assembly in m_Assemblies)
                {
                    UnloadAssembly(assembly);
                }
            }
        }

        #endregion
    }
}
