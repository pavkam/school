using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.Utilities
{
    /// <summary>
    /// The basic class to trasport input and output data. This class is thread-safe.
    /// </summary>
    public sealed class ScalarSet
    {
        #region Private Fields

        private Object[] m_DataSet;
        private Int32 m_SetId; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarSet"/> class.
        /// </summary>
        /// <param name="setId">The set id.</param>
        /// <param name="dataSet">The data set.</param>
        public ScalarSet(Int32 setId, params Object[] dataSet)
        {
            /* Check for consistency and allowed types */
            if (dataSet == null)
                throw new ArgumentNullException("dataSet");

            foreach (Object o in dataSet)
            {
                if (o is Int32 || o is Int64 || o is Int16 || o is Byte || o is UInt32 || o is UInt32 ||
                    o is UInt64 || o is SByte || o is Single || o is Double || o is String)
                { }
                else
                {
                    throw new ArgumentException("Only primitive types are allowed in the ScalarSet!");
                }
            }

            m_SetId = setId;
            m_DataSet = dataSet;
        }

        #endregion

        #region ScalarSet Public Properties

        /// <summary>
        /// Gets the count of variables.
        /// </summary>
        /// <value>The count of variables.</value>
        public Int32 Count
        {
            get { return m_DataSet.Length; }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <value>Variable object</value>
        public Object this[Int32 index]
        {
            get { return m_DataSet[index]; }
        }

        /// <summary>
        /// Gets all variables packed as invoke parameters.
        /// </summary>
        /// <value>Object list.</value>
        public Object[] AsInvokeParameters
        {
            get
            {
                return (Object[])m_DataSet.Clone();
            }
        }

        /// <summary>
        /// Gets the id of the set.
        /// </summary>
        /// <value>The id of the set.</value>
        public Int32 Id
        {
            get { return m_SetId; }
        } 

        #endregion
    }
}
