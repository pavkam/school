using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Utilities;

namespace DCalcCore.Remoting.Common
{
    /// <summary>
    /// Provides deserealization options for scalar sets. This class is thread-safe.
    /// </summary>
    internal class DataDeserializer
    {
        #region Private Fields

        private String m_Data;

        #endregion

        #region Private Methods

        /// <summary>
        /// Reads the till character.
        /// </summary>
        /// <param name="x">The column marker.</param>
        /// <param name="c">The character.</param>
        /// <returns></returns>
        private String ReadTillChar(ref Int32 x, Char c)
        {
            Int32 index = m_Data.IndexOf(c, x);

            if (index == -1)
                throw new Exception();

            String result = ReadTill(ref x, (index - x));
            x++; /* Skip the char */
            return result;
        }

        /// <summary>
        /// Reads a number of characters.
        /// </summary>
        /// <param name="x">The column marker.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        private String ReadTill(ref Int32 x, Int32 count)
        {
            Int32 xx = x;
            x += count;

            return m_Data.Substring(xx, count);
        }

        /// <summary>
        /// Reads the set id.
        /// </summary>
        /// <param name="x">The column marker.</param>
        /// <returns></returns>
        private Int32 ReadSetId(ref Int32 x)
        {
            String read = ReadTillChar(ref x, ':');
            return Convert.ToInt32(read);
        }

        /// <summary>
        /// Reads the param count.
        /// </summary>
        /// <param name="x">The column marker.</param>
        /// <returns></returns>
        private Int32 ReadParamCount(ref Int32 x)
        {
            String read = ReadTillChar(ref x, ':');
            return Convert.ToInt32(read);
        }

        /// <summary>
        /// Reads the variable.
        /// </summary>
        /// <param name="x">The column marker.</param>
        /// <returns></returns>
        private Object ReadVar(ref Int32 x)
        {
            String typeId = ReadTillChar(ref x, '>').ToUpper();
            String value = ReadTillChar(ref x, '<');

            if (typeId.Equals("S"))
            {
                /* String, special case */
                Int32 length = Convert.ToInt32(value);
                value = ReadTill(ref x, length);

                return value;
            }

            else if (typeId.Equals("U8")) { return Convert.ToByte(value); }
            else if (typeId.Equals("U16")) { return Convert.ToUInt16(value); }
            else if (typeId.Equals("U32")) { return Convert.ToUInt32(value); }
            else if (typeId.Equals("U64")) { return Convert.ToUInt64(value); }
            else if (typeId.Equals("I8")) { return Convert.ToSByte(value); }
            else if (typeId.Equals("I16")) { return Convert.ToInt16(value); }
            else if (typeId.Equals("I32")) { return Convert.ToInt32(value); }
            else if (typeId.Equals("I64")) { return Convert.ToInt64(value); }
            else if (typeId.Equals("SF")) { return Convert.ToSingle(value, System.Globalization.CultureInfo.InvariantCulture); }
            else if (typeId.Equals("DF")) { return Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture); }

            return null;
        }

        /// <summary>
        /// Reads the set.
        /// </summary>
        /// <param name="x">The column marker.</param>
        /// <param name="itsId">Id of the set.</param>
        /// <param name="paramCount">The param count.</param>
        /// <returns></returns>
        private ScalarSet ReadSet(ref Int32 x, Int32 itsId, Int32 paramCount)
        {
            List<Object> list = new List<Object>();

            for (Int32 i = 0; i < paramCount; i++)
            {
                Object var_x = ReadVar(ref x);

                if (var_x == null)
                    throw new Exception();

                list.Add(var_x);
            }

            return new ScalarSet(itsId, list.ToArray());
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataDeserializer"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public DataDeserializer(String data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (data.Length == 0)
                throw new ArgumentException("data");

            m_Data = data;
        } 

        #endregion

        #region DataDeserializer Public Methods

        /// <summary>
        /// Deserializes this instance.
        /// </summary>
        /// <returns></returns>
        public ScalarSet[] Deserialize()
        {
            try
            {
                List<ScalarSet> groups = new List<ScalarSet>();
                Int32 x = 0;

                Int32 setCount = ReadParamCount(ref x);

                for (Int32 i = 0; i < setCount; i++)
                {
                    /* Read Group Id */
                    Int32 groupId = ReadSetId(ref x);
                    Int32 paramCount = ReadParamCount(ref x);

                    ScalarSet set = ReadSet(ref x, groupId, paramCount);

                    if (set == null)
                        throw new Exception();

                    groups.Add(set);
                }

                return groups.ToArray();
            }
            catch
            {
            }

            return null;
        } 

        #endregion
    }
}
