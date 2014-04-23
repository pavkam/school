using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using DCalcCore.Remoting;
using DCalcCore.Utilities;

namespace DCalcCore
{
    /// <summary>
    /// Provides a common interface for all dispatchers to implement.
    /// </summary>
    public interface IDispatcher : IDisposable
    {
        /// <summary>
        /// Gets the dispatch mode.
        /// </summary>
        /// <value>The dispatch mode.</value>
        DispatchMode DispatchMode { get; }

        /// <summary>
        /// Executes the specified algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <returns></returns>
        Boolean Execute(IAlgorithm algorithm);

        /// <summary>
        /// Cancels the specified algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <returns></returns>
        Boolean Cancel(IAlgorithm algorithm);

        /// <summary>
        /// Cancels all running algorithms.
        /// </summary>
        /// <returns></returns>
        Boolean CancelAll();

        /// <summary>
        /// Registers the remote gate.
        /// </summary>
        /// <param name="gate">The gate.</param>
        /// <returns></returns>
        Boolean RegisterGate(IRemoteGateClient gate);

        /// <summary>
        /// Unregisters the remote gate.
        /// </summary>
        /// <param name="gate">The gate.</param>
        /// <returns></returns>
        Boolean UnregisterGate(IRemoteGateClient gate);

        /// <summary>
        /// Occurs when algorithm progresses.
        /// </summary>
        event ProgressEventHandler AlgorithmProgress;

        /// <summary>
        /// Occurs when algorithm completes.
        /// </summary>
        event EventHandler AlgorithmComplete;
    }
}
