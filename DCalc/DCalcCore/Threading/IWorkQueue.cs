using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Utilities;
using DCalcCore.Assemblers;
using DCalcCore.LoadBalancers;
using DCalcCore.Algorithm;

namespace DCalcCore.Threading
{
    /// <summary>
    /// Provides a common interface for all Work Queues.
    /// </summary>
    public interface IWorkQueue : IDisposable
    {
        /// <summary>
        /// Queues the evaluation of a script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="compiledScript">The compiled script.</param>
        /// <param name="inputSet">The input set.</param>
        void QueueEvaluation(IScript script, ICompiledScript compiledScript, ScalarSet inputSet);

        /// <summary>
        /// Cancels the evaluation of a script.
        /// </summary>
        /// <param name="compiledScript">The compiled script.</param>
        /// <returns>All the sets that were cancelled.</returns>
        ScalarSet[] CancelEvaluation(ICompiledScript compiledScript);

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Aborts this instance.
        /// </summary>
        void Abort();

        /// <summary>
        /// Occurs when queued work completed (a script was evaluated).
        /// </summary>
        event QueueEventHandler QueuedWorkCompleted;
    }
}
