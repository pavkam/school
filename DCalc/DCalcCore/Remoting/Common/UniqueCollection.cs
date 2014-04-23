using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.Remoting.Common
{
    /// <summary>
    /// Provides a specialized dictionary which creates it's keys randomly. This class is thread-safe.
    /// </summary>
    /// <typeparam name="T">Type of value objects.</typeparam>
    internal class UniqueCollection<T> 
        where T : class
    {
        #region Private Fields

        private Dictionary<String, T> m_KeyToType = new Dictionary<String, T>();
        private Dictionary<T, String> m_TypeToKey = new Dictionary<T, String>();
        private Random m_Random = new Random();
        private Int32 m_KeySize;
        private String m_SyncRoot = "UniqueCollection Sync";

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates a new key.
        /// </summary>
        /// <param name="charCount">The char count to use.</param>
        /// <returns></returns>
        private String GenerateKey(Int32 charCount)
        {
            String allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZ";
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < charCount; i++)
            {
                Char c = allowedChars[m_Random.Next(allowedChars.Length)];
                sb.Append(c);
            }

            return sb.ToString();
        } 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="keySize">Size of the key.</param>
        public UniqueCollection(Int32 keySize)
        {
            if (keySize < 1)
                throw new ArgumentNullException("keySize");

            m_KeySize = keySize;
        } 

        #endregion

        #region UniqueCollection Public Methods

        /// <summary>
        /// Creates a new unique key for the given object and place it into the dictionary.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <returns></returns>
        public String New(T o)
        {
            if (o == null)
                throw new ArgumentNullException("o");

            lock (m_SyncRoot)
            {
                /* Check if we have this client registered */
                if (m_TypeToKey.ContainsKey(o))
                {
                    /* Return already entered key */
                    return m_TypeToKey[o];
                }
                else
                {
                    String newKey;

                    while (true)
                    {
                        /* Try and generate an unique key */
                        newKey = GenerateKey(m_KeySize);

                        if (m_KeyToType.ContainsKey(newKey))
                            continue;
                        else
                            break;
                    }

                    m_KeyToType.Add(newKey, o);
                    m_TypeToKey.Add(o, newKey);

                    return newKey;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="T"/> with the specified key.
        /// </summary>
        /// <value>Object connected with that key.</value>
        public T this[String key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                if (key.Length != m_KeySize)
                    throw new ArgumentException("key");

                lock (m_SyncRoot)
                {
                    if (m_KeyToType.ContainsKey(key))
                        return m_KeyToType[key];
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// Removes the specified object defined by that key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public Boolean Remove(String key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (key.Length != m_KeySize)
                throw new ArgumentException("key");

            lock (m_SyncRoot)
            {
                if (m_KeyToType.ContainsKey(key))
                {
                    m_TypeToKey.Remove(m_KeyToType[key]);
                    m_KeyToType.Remove(key);

                    return true;
                }
                else
                {
                    return false;
                }
            }
        } 

        #endregion

        #region UniqueCollection Public Properties

        /// <summary>
        /// Gets all the keys.
        /// </summary>
        /// <value>The keys.</value>
        public IEnumerable<String> Keys
        {
            get
            {
                lock (m_SyncRoot)
                {
                    return m_KeyToType.Keys;
                }
            }
        } 

        #endregion
    }
}
