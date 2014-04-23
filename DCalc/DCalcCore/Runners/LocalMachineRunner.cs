using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using DCalcCore.Assemblers;
using DCalcCore.Utilities;
using DCalcCore.Threading;
using DCalcCore.LoadBalancers;
using System.Threading;

namespace DCalcCore.Runners
{
    /// <summary>
    /// Local runner. Provides means of executing a script on local cores. This class is thread-safe.
    /// </summary>
    /// <typeparam name="A">Script assembler to use.</typeparam>
    /// <typeparam name="LB">Load balancer to be used across threads.</typeparam>
    public sealed class LocalMachineRunner<A, LB> : IRunner
        where A : IScriptAssembler, new()
        where LB : ILoadBalancer, new()
    {
        #region Private Fields

        private A m_Assembler = new A();
        private Dictionary<IScript, ICompiledScript> m_ScriptsToCompiled = new Dictionary<IScript, ICompiledScript>();
        private Dictionary<ICompiledScript, IScript> m_CompiledToScripts = new Dictionary<ICompiledScript, IScript>();
        private ThreadedWorkQueue<LB> m_WorkQueue;
        private String m_SyncRoot = "LocalMachineRunner Sync";

        #endregion

        #region Private Methods

        /// <summary>
        /// Compiles the script.
        /// </summary>
        /// <param name="Script">The script.</param>
        /// <returns></returns>
        private ICompiledScript CompileScript(IScript Script)
        {
            /* Check if we have this Script already */
            if (m_ScriptsToCompiled.ContainsKey(Script))
                return m_ScriptsToCompiled[Script];

            /* Try to actually compile the Script using the given assembler */
            ICompiledScript compiled = m_Assembler.Assemble(Script);

            if (compiled != null)
            {
                m_ScriptsToCompiled.Add(Script, compiled);
                m_CompiledToScripts.Add(compiled, Script);
            }

            return compiled;
        }

        /// <summary>
        /// Initiates the queue.
        /// </summary>
        /// <param name="threadCount">The thread count.</param>
        private void InitiateQueue(Int32 threadCount)
        {
            /* Create new work queue */
            m_WorkQueue = new ThreadedWorkQueue<LB>(threadCount);
            m_WorkQueue.QueuedWorkCompleted -= m_workQueue_QueuedWorkCompleted;
            m_WorkQueue.QueuedWorkCompleted += m_workQueue_QueuedWorkCompleted;
            m_WorkQueue.Start();
        }

        /// <summary>
        /// Handles the QueuedWorkCompleted event of the m_workQueue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DCalcCore.Utilities.QueueEventArgs"/> instance containing the event data.</param>
        private void m_workQueue_QueuedWorkCompleted(object sender, QueueEventArgs e)
        {
            if (QueuedWorkCompleted != null)
                QueuedWorkCompleted(this, new ScriptQueueEventArgs((IScript)sender, e.OutputSet, e.SetId));
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalMachineRunner&lt;A&gt;"/> class. Using the number of processors
        /// as default.
        /// </summary>
        public LocalMachineRunner()
        {
            /* Take the number of processors default */
            InitiateQueue(Environment.ProcessorCount);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalMachineRunner&lt;A&gt;"/> class.
        /// </summary>
        /// <param name="threadsToSpawn">The threads to spawn.</param>
        public LocalMachineRunner(Int32 threadsToSpawn)
        {
            if (threadsToSpawn < 1)
                throw new ArgumentException("threadsToSpawn");

            InitiateQueue(threadsToSpawn);
        }

        #endregion

        #region IRunner Members

        /// <summary>
        /// Loads the script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public Boolean LoadScript(IScript script)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            lock (m_SyncRoot)
            {
                /* Try and compile */
                ICompiledScript compiledScript = CompileScript(script);
                return (compiledScript != null);
            }
        }

        /// <summary>
        /// Removes the script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public Boolean RemoveScript(IScript script)
        {
            if (script == null)
                throw new ArgumentNullException("script");


            ScalarSet[] cancelledSets = null;

            lock (m_SyncRoot)
            {
                /* Check if we already have it loaded */
                if (m_ScriptsToCompiled.ContainsKey(script))
                {
                    ICompiledScript compiledScript = m_ScriptsToCompiled[script];

                    /* Cancel executive in the queue */
                    cancelledSets = m_WorkQueue.CancelEvaluation(compiledScript);
                    Thread.Sleep(10);

                    m_ScriptsToCompiled.Remove(script);
                    m_CompiledToScripts.Remove(compiledScript);

                    compiledScript.Dispose();
                }
                else
                    return false;
            }

            if (cancelledSets != null && cancelledSets.Length > 0)
            {
                foreach (ScalarSet set in cancelledSets)
                {
                    if (QueuedWorkCompleted != null)
                        QueuedWorkCompleted(this, new ScriptQueueEventArgs(script, null, set.Id));
                }
            }

            return true;
        }

        /// <summary>
        /// Queues the work.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="inputSet">The input set.</param>
        /// <returns></returns>
        public Boolean QueueWork(IScript script, ScalarSet inputSet)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            if (inputSet == null)
                throw new ArgumentNullException("inputSet");

            lock (m_SyncRoot)
            {
                /* First of all see if we have this Script compiled */
                if (m_ScriptsToCompiled.ContainsKey(script))
                {
                    m_WorkQueue.QueueEvaluation(script, m_ScriptsToCompiled[script], inputSet);
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Occurs when queued work completed (a set has been evaluated).
        /// </summary>
        public event ScriptQueueEventHandler QueuedWorkCompleted;

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (m_SyncRoot)
            {
                /* We must clear out all queued items */
                List<IScript> allScripts = new List<IScript>(m_ScriptsToCompiled.Keys);

                foreach (IScript Script in allScripts)
                {
                    /* Remove all Scripts */
                    RemoveScript(Script);
                }

                /* Stop working queue */
                m_WorkQueue.Stop();
                m_WorkQueue.Dispose();
            }
        }

        #endregion
    }
}
