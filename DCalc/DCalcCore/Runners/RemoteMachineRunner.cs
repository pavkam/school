using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using DCalcCore.Assemblers;
using DCalcCore.Remoting;
using DCalcCore.Utilities;

namespace DCalcCore.Runners
{
    /// <summary>
    /// Base for all remote based evaluation. This class is thread-safe.
    /// </summary>
    public sealed class RemoteMachineRunner : IRunner
    {
        #region Private Fields

        private IRemoteGateClient m_Gate;
        private String m_SyncRoot = "RemoteMachineRunner Sync"; 

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes a gate client.
        /// </summary>
        /// <param name="client">The gate client.</param>
        private void InitializeGate(IRemoteGateClient client)
        {
            client.SetCompleted -= m_gate_SetCompleted;
            client.SetCompleted += m_gate_SetCompleted;
            client.AsyncOpen();
        }

        /// <summary>
        /// Handles the SetCompleted event of the m_gate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DCalcCore.Utilities.QueueEventArgs"/> instance containing the event data.</param>
        private void m_gate_SetCompleted(Object sender, QueueEventArgs e)
        {
            /* Forward the set to the user */
            if (QueuedWorkCompleted != null)
            {
                QueuedWorkCompleted(this, new ScriptQueueEventArgs((IScript)sender, e.OutputSet, e.SetId));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteMachineRunner"/> class.
        /// </summary>
        /// <param name="gate">The gate.</param>
        public RemoteMachineRunner(IRemoteGateClient gate)
        {
            if (gate == null)
                throw new ArgumentNullException("gate");

            m_Gate = gate;
            InitializeGate(m_Gate);
        }

        #endregion

        #region RemoteMachineRunner Public Members

        /// <summary>
        /// Gets the gate attached to this runner.
        /// </summary>
        /// <value>The gate.</value>
        public IRemoteGateClient Gate
        {
            get 
            {
                lock (m_SyncRoot)
                {
                    return m_Gate;
                }
            }
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
                m_Gate.AsyncRegisterScript(script);
                return true;
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

            lock (m_SyncRoot)
            {
                m_Gate.AsyncCancelScript(script);
                return true;
            }
        }

        /// <summary>
        /// Queues work to be done.
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
                return m_Gate.AsyncQueueWork(script, inputSet);
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
                /* Close the gate */
                m_Gate.AsyncClose();
            }
        }

        #endregion
    }
}