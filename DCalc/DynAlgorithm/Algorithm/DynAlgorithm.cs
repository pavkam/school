using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using DCalcCore.Utilities;

namespace DynAlgorithm.Algorithm
{
    /// <summary>
    /// Dynamic algorithm. This class is thread-safe.
    /// </summary>
    public sealed class DynAlgorithm : IAlgorithm
    {
        #region Private Fields

        private String m_MethodName = "FunctionToDistribute";
        private String m_MethodBody =
          @"FunctionToDistribute(Int32 x)";

        /* Settings */
        private Int32 m_StartInterval;
        private Int32 m_EndInterval;
        private String m_ReturnType;
        private String m_Body;

        /* Locals */
        private StringBuilder m_Results;

        #endregion

        #region Constructors

        public DynAlgorithm(Int32 startInterval, Int32 endInterval, String returnType, String body)
        {
            // todo: check args
            m_StartInterval = startInterval;
            m_EndInterval = endInterval;
            m_ReturnType = returnType;
            m_Body = body;
        } 

        #endregion

        #region DynAlgorithm Public Methods

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <returns></returns>
        public String GetResults()
        {
            if (m_Results != null)
                return m_Results.ToString();
            else
                return String.Empty;
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
            get { return m_ReturnType + " " + m_MethodBody + " {" + m_Body + " }"; }
        }

        /// <summary>
        /// Gets the input set count.
        /// </summary>
        /// <value>The input set count.</value>
        public Int32 InputSetCount
        {
            get 
            {
                return (m_EndInterval - m_StartInterval + 1); 
            }
        }

        /// <summary>
        /// Gets the input set.
        /// </summary>
        /// <param name="setNumber">The set number.</param>
        /// <returns></returns>
        public ScalarSet GetInputSet(Int32 setNumber)
        {
            return new ScalarSet(setNumber, m_StartInterval + setNumber);
        }

        /// <summary>
        /// Returns the output set to the algorithm.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setNumber">The set number.</param>
        public void ReceiveOutputSet(ScalarSet set, Int32 setNumber)
        {
            foreach (Object r in set.AsInvokeParameters)
            {
                m_Results.Append(r.ToString());
                m_Results.Append("    ");
            }

            m_Results.Append(Environment.NewLine);
        }

        /// <summary>
        /// Instructs the algorithm to prepare it's data.
        /// </summary>
        public void PrepareToStart()
        {
            m_Results = new StringBuilder();
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
