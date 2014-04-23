using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Utilities;

namespace DCalcCore.Algorithm
{
    /// <summary>
    /// Provides common interface for all algorithms to implement.
    /// </summary>
    public interface IAlgorithm : IScript
    {
        /// <summary>
        /// Gets the input set count.
        /// </summary>
        /// <value>The input set count.</value>
        Int32 InputSetCount { get; }

        /// <summary>
        /// Gets the input set.
        /// </summary>
        /// <param name="setNumber">The set number.</param>
        /// <returns></returns>
        ScalarSet GetInputSet(Int32 setNumber);

        /// <summary>
        /// Returns the output set to the algorithm.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setNumber">The set number.</param>
        void ReceiveOutputSet(ScalarSet set, Int32 setNumber);

        /// <summary>
        /// Instructs the algorithm to prepare it's data.
        /// </summary>
        void PrepareToStart();

        /// <summary>
        /// Instructs the algorithm to processs the result data.
        /// </summary>
        void PrepareToFinish();
    }
}
