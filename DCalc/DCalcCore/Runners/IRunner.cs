using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using DCalcCore.Utilities;
using DCalcCore.LoadBalancers;

namespace DCalcCore.Runners
{
    /// <summary>
    /// Provide a common interface for all Runners.
    /// </summary>
    public interface IRunner : IDisposable
    {
        /// <summary>
        /// Loads the script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        Boolean LoadScript(IScript script);

        /// <summary>
        /// Removes the script. Returns all sets that were not executed.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        Boolean RemoveScript(IScript script);

        /// <summary>
        /// Queues work to be done.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="inputSet">The input set.</param>
        /// <returns></returns>
        Boolean QueueWork(IScript script, ScalarSet inputSet);

        /// <summary>
        /// Occurs when queued work completed (a set has been evaluated).
        /// </summary>
        event ScriptQueueEventHandler QueuedWorkCompleted;
    }
}
