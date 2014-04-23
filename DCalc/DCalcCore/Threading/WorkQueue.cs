using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DCalcCore.Utilities;
using DCalcCore.Assemblers;
using DCalcCore.LoadBalancers;
using DCalcCore.Algorithm;

namespace DCalcCore.Threading
{
    /// <summary>
    /// Threaded Work queue. This class is thread-safe.
    /// </summary>
    public sealed class WorkQueue : IWorkQueue
    {
        #region Private Fields

        private Thread m_WorkThread;
        private List<WorkQueueItem> m_Queue = new List<WorkQueueItem>();
        private String m_SyncRoot = "WorkQueue Sync";

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the next work item from the queue.
        /// </summary>
        /// <returns></returns>
        private WorkQueueItem GetNext()
        {
            if (m_Queue.Count > 0)
            {
                WorkQueueItem result = m_Queue[0];
                m_Queue.RemoveAt(0);

                return result;
            }
            else
                return null;
        }

        #endregion

        #region THREAD: Work Queue

        /// <summary>
        /// Work Queue thread method.
        /// </summary>
        private void WorkQueueMethod()
        {
            try
            {
                while (true)
                {
                    QueueEventArgs eventArgs = null;
                    WorkQueueItem nextItem = null;
                    Boolean shouldSleep = false;

                    lock (m_SyncRoot)
                    {
                        nextItem = GetNext();

                        if (nextItem != null)
                        {
                            ScalarSet outSet = null;

                            if (!nextItem.Cancelled)
                            {
                                try
                                {
                                    outSet = nextItem.CompiledScript.Execute(nextItem.InputSet);
                                }
                                catch
                                {
                                }

                                eventArgs = new QueueEventArgs(outSet, nextItem.InputSet.Id);
                            }
                        }
                        else
                            shouldSleep = true;
                    }

                    if (shouldSleep)
                        Thread.Sleep(10); /* Sleep 10ms if there's nothing to work on */
                    else if (eventArgs != null)
                    {
                        if (QueuedWorkCompleted != null)
                        {
                            QueuedWorkCompleted(nextItem.Script, eventArgs);
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueue"/> class.
        /// </summary>
        public WorkQueue()
        {
            /* Create the thread */
            m_WorkThread = new Thread(WorkQueueMethod);
        }

        #endregion

        #region IWorkQueue Members

        /// <summary>
        /// Queues the evaluation of a script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="compiledScript">The compiled script.</param>
        /// <param name="inputSet">The input set.</param>
        public void QueueEvaluation(IScript script, ICompiledScript compiledScript, ScalarSet inputSet)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            if (compiledScript == null)
                throw new ArgumentNullException("compiledScript");

            if (inputSet == null)
                throw new ArgumentNullException("inputSet");

            lock (m_SyncRoot)
            {
                m_Queue.Add(new WorkQueueItem(compiledScript, script, inputSet));
            }
        }

        /// <summary>
        /// Cancels the evaluation of a script.
        /// </summary>
        /// <param name="compiledScript">The compiled script.</param>
        /// <returns>All the sets that were cancelled.</returns>
        public ScalarSet[] CancelEvaluation(ICompiledScript compiledScript)
        {
            if (compiledScript == null)
                throw new ArgumentNullException("compiledScript");

            List<ScalarSet> toCancel = new List<ScalarSet>();

            lock (m_SyncRoot)
            {
                foreach (WorkQueueItem eg in m_Queue)
                {
                    if (eg.CompiledScript == compiledScript)
                    {
                        /* Cancel me! */
                        eg.Cancelled = true;
                        toCancel.Add(eg.InputSet);
                    }
                }
            }

            return toCancel.ToArray();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            lock (m_SyncRoot)
            {
                if (!m_WorkThread.IsAlive)
                    m_WorkThread.Start();
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            lock (m_SyncRoot)
            {
                if (m_WorkThread.IsAlive)
                    m_WorkThread.Abort();
            }
        }

        /// <summary>
        /// Aborts this instance.
        /// </summary>
        public void Abort()
        {
            lock (m_SyncRoot)
            {
                if (m_WorkThread.IsAlive)
                    m_WorkThread.Abort();

                /* Raise the cancel events for all queued stuff */
                foreach (WorkQueueItem eg in m_Queue)
                {
                    if (QueuedWorkCompleted != null)
                        QueuedWorkCompleted(eg.Script, new QueueEventArgs(null, eg.InputSet.Id));
                }
            }
        }

        /// <summary>
        /// Occurs when queued work completed (a script was evaluated).
        /// </summary>
        public event QueueEventHandler QueuedWorkCompleted;

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Abort();
        }

        #endregion
    }
}
