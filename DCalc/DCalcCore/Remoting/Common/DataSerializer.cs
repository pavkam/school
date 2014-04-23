using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Utilities;

namespace DCalcCore.Remoting.Common
{
    /// <summary>
    /// Provides serialization options for scalar sets. This class is thread-safe.
    /// </summary>
    internal class DataSerializer
    {
        #region Private Fields

        private ScalarSet[] m_Sets;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSerializer"/> class.
        /// </summary>
        /// <param name="sets">The sets.</param>
        public DataSerializer(ScalarSet[] sets)
        {
            if (sets == null)
                throw new ArgumentNullException("sets");

            if (sets.Length == 0)
                throw new ArgumentException("sets");

            m_Sets = sets;
        }

        #endregion

        #region DataSerializer Public Methods

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns></returns>
        public String Serialize()
        {
            /* For each group we serialize the type + value */
            StringBuilder sb = new StringBuilder();

            /* Number of records */
            sb.Append(m_Sets.Length);
            sb.Append(':');

            foreach (ScalarSet group in m_Sets)
            {
                Serialize(sb, group);
            }

            return sb.ToString();
        } 

        #endregion


        public static Int32 Serialize(StringBuilder toBuilder, ScalarSet scalarSet)
        {
            Int32 preCount = toBuilder.Length;

            /* Group Id */
            toBuilder.Append(scalarSet.Id);
            toBuilder.Append(':');
            toBuilder.Append(scalarSet.Count);
            toBuilder.Append(':');

            /* Each parameter */
            foreach (Object o in scalarSet.AsInvokeParameters)
            {
                /* Append type first */
                if (o is Byte) { toBuilder.Append("u8>"); toBuilder.Append((Byte)o); toBuilder.Append("<"); }
                else if (o is SByte) { toBuilder.Append("i8>"); toBuilder.Append((SByte)o); toBuilder.Append("<"); }
                else if (o is UInt16) { toBuilder.Append("u16>"); toBuilder.Append((UInt16)o); toBuilder.Append("<"); }
                else if (o is Int16) { toBuilder.Append("i16>"); toBuilder.Append((Int16)o); toBuilder.Append("<"); }
                else if (o is UInt32) { toBuilder.Append("u32>"); toBuilder.Append((UInt32)o); toBuilder.Append("<"); }
                else if (o is Int32) { toBuilder.Append("i32>"); toBuilder.Append((Int32)o); toBuilder.Append("<"); }
                else if (o is UInt64) { toBuilder.Append("u64>"); toBuilder.Append((UInt64)o); toBuilder.Append("<"); }
                else if (o is Int64) { toBuilder.Append("i64>"); toBuilder.Append((Int64)o); toBuilder.Append("<"); }
                else if (o is Single) { toBuilder.Append("sf>"); toBuilder.Append(((Single)o).ToString(System.Globalization.CultureInfo.InvariantCulture)); toBuilder.Append("<"); }
                else if (o is Double) { toBuilder.Append("df>"); toBuilder.Append(((Double)o).ToString(System.Globalization.CultureInfo.InvariantCulture)); toBuilder.Append("<"); }
                else if (o is String) { toBuilder.Append("s>"); toBuilder.Append(((String)o).Length); toBuilder.Append(">"); toBuilder.Append((String)o); }
                else
                {
                    return -1;
                }
            }

            return toBuilder.Length - preCount;
        }
    }
}
