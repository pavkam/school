using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using DCalcCore.Assemblers;
using DCalcCore.Utilities;
using DCalcCore.LoadBalancers;

namespace DCalcCore.Threading
{
    /// <summary>
    /// Multi-threaded work queue. This class is thread-safe.
    /// </summary>
    /// <typeparam name="LB">Load balancer to be used across threads.</typeparam>
    public sealed class ThreadedWorkQueue<LB> : IWorkQueue
        where LB : ILoadBalancer, new()
    {
        #region Private Fields

        private Int32 m_ThreadsSpawned;
        private List<WorkQueue> m_Workers = new List<WorkQueue>();
        private LB m_LoadBalancer = new LB();
        private String m_SyncRoot = "ThreadedWorkQueue Sync"; 

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes all the workers.
        /// </summary>
        /// <param name="count">The count of threads to spawn.</param>
        private void InitWorkers(Int32 count)
        {
            for (Int32 i = 0; i < count; i++)
            {
                WorkQueue newQ = new WorkQueue();
                newQ.QueuedWorkCompleted += workQueue_QueuedWorkCompleted;
                m_Workers.Add(newQ);

                m_LoadBalancer.RegisterObject(newQ);
            }
        }

        /// <summary>
        /// Handles the QueuedWorkCompleted event of a Work Queue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DCalcCore.Utilities.QueueEventArgs"/> instance containing the event data.</param>
        private void workQueue_QueuedWorkCompleted(Object sender, QueueEventArgs e)
        {
            /* Forward to the user */
            if (QueuedWorkCompleted != null)
                QueuedWorkCompleted(sender, e);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadedWorkQueue"/> class.
        /// </summary>
        /// <param name="threadCount">The thread count.</param>
        public ThreadedWorkQueue(Int32 threadCount)
        {
            if (threadCount < 1)
                throw new ArgumentException("threadCount");

            m_ThreadsSpawned = threadCount;
            InitWorkers(threadCount);
        } 

        #endregion

        #region ThreadedWorkQueue Public Properties

        /// <summary>
        /// Gets the threads spawned.
        /// </summary>
        /// <value>The threads spawned.</value>
        public Int32 ThreadsSpawned
        {
            get 
            {
                return m_ThreadsSpawned; 
            }
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
            if (compiledScript == null)
                throw new ArgumentNullException("compiledScript");

            if (script == null)
                throw new ArgumentNullException("script");

            if (inputSet == null)
                throw new ArgumentNullException("inputSet");

            lock (m_SyncRoot)
            {
                /* Select next queue (balanced) */
                WorkQueue nextQueue = (WorkQueue)m_LoadBalancer.SelectObject();
                nextQueue.QueueEvaluation(script, compiledScript, inputSet);
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

            lock (m_SyncRoot)
            {
                List<ScalarSet> cancelled = new List<ScalarSet>();

                foreach (WorkQueue queue in m_Workers)
                {
                    /* Cancel execution for this executive from all queues */
                    cancelled.AddRange(queue.CancelEvaluation(compiledScript));
                }

                return cancelled.ToArray();
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            lock (m_SyncRoot)
            {
                foreach (WorkQueue queue in m_Workers)
                {
                    queue.Start();
                }
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            lock (m_SyncRoot)
            {
                foreach (WorkQueue queue in m_Workers)
                {
                    queue.Stop();
                }
            }
        }

        /// <summary>
        /// Aborts this instance.
        /// </summary>
        public void Abort()
        {
            lock (m_SyncRoot)
            {
                foreach (WorkQueue queue in m_Workers)
                {
                    queue.Abort();
                }

                /* Clear load balancer */
                m_LoadBalancer.ResetBalance();
            }
        }

        /// <summary>
        /// Occurs when queued work completed (a script was evaluated).
        /// </summary>
        public event QueueEventHandler QueuedWorkCompleted;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            /* Abort all workers */
            Abort();
        }

        #endregion
    }
}
