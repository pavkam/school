using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using DCalcCore.Utilities;

namespace EchoAlgorithm.Algorithm
{
    /// <summary>
    /// Echo-like algorithm. This class is thread-safe.
    /// </summary>
    public sealed class EchoAlgorithm : IAlgorithm
    {
        #region Private Fields

        private String m_MethodName = "Echo";
        private String m_MethodBody = "int Echo(int i) { for (int x = 0; x < [I]; x++) {} return i; }";
        private Int32 m_InputSetCount;
        private Int32 m_DelayCycles;

        #endregion

        #region Constructors

        public EchoAlgorithm(Int32 delayCycles, Int32 setCount)
        {
            if (delayCycles < 1)
                throw new ArgumentException("delayCycles");

            if (setCount < 1)
                throw new ArgumentException("setCount");

            m_DelayCycles = delayCycles;
            m_InputSetCount = setCount;
        } 

        #endregion

        #region IAlgorithm Members

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public String MethodName
        {
            get { return m_MethodName; }
        }

        /// <summary>
        /// Gets the method body.
        /// </summary>
        /// <value>The method body.</value>
        public String MethodBody
        {
            get { return m_MethodBody.Replace("[I]", m_DelayCycles.ToString()); }
        }

        /// <summary>
        /// Gets the input set count.
        /// </summary>
        /// <value>The input set count.</value>
        public Int32 InputSetCount
        {
            get { return m_InputSetCount; }
        }

        /// <summary>
        /// Gets the input set.
        /// </summary>
        /// <param name="setNumber">The set number.</param>
        /// <returns></returns>
        public ScalarSet GetInputSet(Int32 setNumber)
        {
            return new ScalarSet(setNumber, setNumber);
        }

        /// <summary>
        /// Returns the output set to the algorithm.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setNumber">The set number.</param>
        public void ReceiveOutputSet(ScalarSet set, Int32 setNumber)
        {
        }

        /// <summary>
        /// Instructs the algorithm to prepare it's data.
        /// </summary>
        public void PrepareToStart()
        {
        }

        /// <summary>
        /// Instructs the algorithm to processs the result data.
        /// </summary>
        public void PrepareToFinish()
        {
        }

        #endregion
    }
}
