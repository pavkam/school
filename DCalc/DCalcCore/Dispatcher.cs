using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Runners;
using DCalcCore.LoadBalancers;
using DCalcCore.Assemblers;
using DCalcCore.Algorithm;
using DCalcCore.Utilities;
using DCalcCore.Remoting;
using System.Threading;

namespace DCalcCore
{
    /// <summary>
    /// Unified control over evaluation mechanisms. This class is thread-safe.
    /// </summary>
    /// <typeparam name="A">Algorithm assembler to be used locally.</typeparam>
    /// <typeparam name="RLB">Load balancer to be used to balance between runners.</typeparam>
    /// <typeparam name="LLB">Load banalcer to be used to balance between local processors.</typeparam>
    public sealed class Dispatcher<A, LLB, RLB> : IDispatcher 
        where A : IScriptAssembler, new() 
        where RLB : ILoadBalancer, new()
        where LLB : ILoadBalancer, new()
    {
        #region Private Fields

        private DispatchMode m_DispatchMode;
        private Int32 m_InputQueueSize;
        private LocalMachineRunner<A, LLB> m_LocalRunner;
        private List<RemoteMachineRunner> m_RemoteRunners = new List<RemoteMachineRunner>();
        private Dictionary<IAlgorithm, DataPlanner> m_DataPlanners = new Dictionary<IAlgorithm, DataPlanner>();
        private RLB m_LoadBalancer = new RLB();
        private Thread m_DispatcherThread;
        private Int32 m_LocalThreads;

        private Queue<IAlgorithm> m_JustAdded = new Queue<IAlgorithm>();
        private Queue<IAlgorithm> m_JustFinished = new Queue<IAlgorithm>();
        private Queue<IAlgorithm> m_JustCancelled = new Queue<IAlgorithm>();

        private String m_SyncRoot = "Dispatcher Sync";

        #endregion

        #region Private Methods

        /// <summary>
        /// Uploads the next piece to the free runners using balancers.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <param name="dataPlanner">The data planner.</param>
        private void UploadNextPiece(IAlgorithm algorithm, DataPlanner dataPlanner)
        {
            /* First let's check how many pieces we need to upload */
            Int32 setsToUpload = (m_InputQueueSize - dataPlanner.InProgressCount);

            if (setsToUpload == 0)
                return; /* Nothing to do here! */

            /* Lets take out the data we need */
            ScalarSet[] sets = dataPlanner.Consume(setsToUpload);

            if (sets == null || sets.Length == 0)
            {
                /* Hm, bug? */
                return;
            }

            foreach (ScalarSet set in sets)
            {
                /* Balance each set to a runner */
                IRunner runner = (IRunner)m_LoadBalancer.SelectObject();

                if (!runner.QueueWork(algorithm, set))
                {
                    dataPlanner.Return(set.Id);
                    m_LoadBalancer.ObjectFailed(runner);
                }
            }
        }

        /// <summary>
        /// Initializes the runner.
        /// </summary>
        /// <param name="runner">The runner.</param>
        private void InitializeRunner(IRunner runner)
        {
            runner.QueuedWorkCompleted -= runner_QueuedWorkCompleted;
            runner.QueuedWorkCompleted += runner_QueuedWorkCompleted;

            m_LoadBalancer.RegisterObject(runner);
        }

        /// <summary>
        /// Handles the QueuedWorkCompleted event of a runner.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DCalcCore.Utilities.ScriptQueueEventArgs"/> instance containing the event data.</param>
        private void runner_QueuedWorkCompleted(object sender, ScriptQueueEventArgs e)
        {
            IRunner runner = (IRunner)sender;
            IAlgorithm algorithm = (IAlgorithm)e.Script;
            DataPlanner dataPlanner = null;
            ProgressEventArgs pgEvent = null;

            lock (m_SyncRoot)
            {
                if (m_DataPlanners.ContainsKey(algorithm))
                    dataPlanner = m_DataPlanners[algorithm];

                if (dataPlanner == null)
                    return;

                /* Work complete for a data piece. Let's upload another one */
                if (e.OutputSet == null)
                {
                    /* Failed! */
                    dataPlanner.Return(e.SetId);
                    m_LoadBalancer.ObjectFailed(runner);
                }
                else
                {
                    /* Succeded! */
                    dataPlanner.Confirm(e.SetId);
                    m_LoadBalancer.ObjectDone(runner);

                    /* Upload data to the algorithm */
                    algorithm.ReceiveOutputSet(e.OutputSet, e.SetId);

                    if (dataPlanner.MoreAvailable)
                    {
                        /* Try to add more data */
                        UploadNextPiece(algorithm, dataPlanner);
                    }
                    else if (dataPlanner.AllComplete)
                    {
                        /* Script evaluation has completed! */
                        m_JustFinished.Enqueue(algorithm);
                    }

                    /* Build arguments */
                    pgEvent = new ProgressEventArgs(dataPlanner.CompletedCount, algorithm.InputSetCount);
                }
            }

            /* Notify user */
            if (AlgorithmProgress != null && pgEvent != null)
            {
                AlgorithmProgress(algorithm, pgEvent);
            }
        }

        /// <summary>
        /// Cancels a script.
        /// </summary>
        /// <param name="script">The script to cancel.</param>
        private void CancelScript(IAlgorithm script)
        {
            /* Remove the script from the local runner */
            if (m_LocalRunner != null)
            {
                m_LocalRunner.RemoveScript(script);
            }

            /* Remove the script from the remote runners */
            foreach (RemoteMachineRunner machine in m_RemoteRunners)
            {
                machine.RemoveScript(script);
            }

            /* Remove the associated data planner */
            m_DataPlanners.Remove(script);
        }

        /// <summary>
        /// Initiates the script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        private Boolean InitiateScript(IAlgorithm script)
        {
            if (m_DispatchMode == DispatchMode.LocalOnly)
            {
                /* Run only on local machine */
                Boolean result = m_LocalRunner.LoadScript(script);

                if (result)
                {
                    DataPlanner planner = new DataPlanner(script);
                    script.PrepareToStart();

                    m_DataPlanners.Add(script, planner);

                    UploadNextPiece(script, planner);
                }

                return result;
            }
            else if (m_DispatchMode == DispatchMode.RemoteOnly)
            {
                /* Run only on remote machines */
                Boolean result = false;

                foreach (RemoteMachineRunner machine in m_RemoteRunners)
                {
                    Boolean machineResult = machine.LoadScript(script);
                    result = result || machineResult;
                }

                if (result)
                {
                    DataPlanner planner = new DataPlanner(script);
                    script.PrepareToStart();

                    m_DataPlanners.Add(script, planner);

                    UploadNextPiece(script, planner);
                }

                return result;
            }
            else
            {
                /* Run on both remote and local */
                if (!m_LocalRunner.LoadScript(script))
                    return false;

                foreach (RemoteMachineRunner machine in m_RemoteRunners)
                {
                    machine.LoadScript(script);
                }

                DataPlanner planner = new DataPlanner(script);
                script.PrepareToStart();

                m_DataPlanners.Add(script, planner);
                UploadNextPiece(script, planner);

                return true;
            }
        }

        /// <summary>
        /// Finishes the script.
        /// </summary>
        /// <param name="script">The script.</param>
        private void FinishScript(IAlgorithm script)
        {
            script.PrepareToFinish();
            CancelScript(script);
        }

        #endregion

        #region THREAD: Dispatch helper method

        private void DispatcherMethod()
        {
            try
            {
                Queue<IAlgorithm> nowFinished = new Queue<IAlgorithm>();

                while (true)
                {
                    nowFinished.Clear();
 
                    lock (m_SyncRoot)
                    {
                        /* Check the just added algorithms */
                        while (m_JustAdded.Count > 0)
                        {
                            IAlgorithm algorithm = m_JustAdded.Dequeue();
                            InitiateScript(algorithm);
                        }

                        /* Check the just cancelled algorithms */
                        while (m_JustCancelled.Count > 0)
                        {
                            IAlgorithm algorithm = m_JustCancelled.Dequeue();
                            CancelScript(algorithm);
                        }

                        /* Check the just finished algorithms */
                        while (m_JustFinished.Count > 0)
                        {
                            IAlgorithm algorithm = m_JustFinished.Dequeue();
                            FinishScript(algorithm);

                            nowFinished.Enqueue(algorithm);
                        }

                        foreach (IAlgorithm algorithm in m_DataPlanners.Keys)
                        {
                            DataPlanner dataPlanner = m_DataPlanners[algorithm];

                            if (dataPlanner.MoreAvailable)
                            {
                                /* Still some work */
                                UploadNextPiece(algorithm, dataPlanner);
                            }
                        }
                    }

                    /* Notify the user of the scripts being finished */
                    while (nowFinished.Count > 0)
                    {
                        IAlgorithm algorithm = nowFinished.Dequeue();

                        if (AlgorithmComplete != null)
                        {
                            AlgorithmComplete(algorithm, EventArgs.Empty);
                        }
                    }

                    Thread.Sleep(500);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Dispatcher&lt;A, RLB, LLB&gt;"/> class.
        /// </summary>
        /// <param name="inputQueueSize">Size of the input queue.</param>
        /// <param name="localThreadsToOpen">The number of local threads to open.</param>
        public Dispatcher(Int32 inputQueueSize, Int32 localThreadsToOpen)
        {
            if (localThreadsToOpen < 1)
                throw new ArgumentException("localThreadsToOpen");

            if (inputQueueSize < 1)
                throw new ArgumentException("inputQueueSize");

            m_DispatchMode = DispatchMode.Combined;
            m_InputQueueSize = inputQueueSize;
            m_LocalThreads = localThreadsToOpen;

            /* Initialize local runner */
            m_LocalRunner = new LocalMachineRunner<A, LLB>();
            InitializeRunner(m_LocalRunner);

            /* Dispatch helper */
            m_DispatcherThread = new Thread(DispatcherMethod);
            m_DispatcherThread.Start();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dispatcher&lt;A, RLB, LLB&gt;"/> class.
        /// </summary>
        /// <param name="inputQueueSize">Size of the input queue.</param>
        /// <param name="localThreadsToOpen">The number of local threads to open. (Only valid in LocalOnly and Combined modes).</param>
        /// <param name="mode">The mode.</param>
        public Dispatcher(Int32 inputQueueSize, Int32 localThreadsToOpen, DispatchMode mode)
        {
            if (localThreadsToOpen < 1)
                throw new ArgumentException("localThreadsToOpen");

            if (inputQueueSize < 1)
                throw new ArgumentException("inputQueueSize");

            m_DispatchMode = mode;
            m_InputQueueSize = inputQueueSize;
            m_LocalThreads = localThreadsToOpen;

            /* Initialize local runner */
            if (mode != DispatchMode.RemoteOnly)
            {
                m_LocalRunner = new LocalMachineRunner<A, LLB>();
                InitializeRunner(m_LocalRunner);
            }

            /* Dispatch Helper */
            m_DispatcherThread = new Thread(DispatcherMethod);
            m_DispatcherThread.Start();
        }

        #endregion

        #region IDispatcher Members

        /// <summary>
        /// Gets the dispatch mode.
        /// </summary>
        /// <value>The dispatch mode.</value>
        public DispatchMode DispatchMode
        {
            get { return m_DispatchMode; }
        }

        /// <summary>
        /// Executes the specified algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <returns></returns>
        public Boolean Execute(IAlgorithm algorithm)
        {
            if (algorithm == null)
                throw new ArgumentNullException("algorithm");

            lock (m_SyncRoot)
            {
                /* Put into a queue */
                m_JustAdded.Enqueue(algorithm);

                return true;
            }
        }

        /// <summary>
        /// Cancels the specified algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <returns></returns>
        public Boolean Cancel(IAlgorithm algorithm)
        {
            if (algorithm == null)
                throw new ArgumentNullException("algorithm");

            lock (m_SyncRoot)
            {
                /* Queue unload */
                m_JustCancelled.Enqueue(algorithm);
                return true;
            }
        }

        /// <summary>
        /// Cancels all running algorithms.
        /// </summary>
        /// <returns></returns>
        public Boolean CancelAll()
        {
            lock (m_SyncRoot)
            {
                List<IAlgorithm> loaded = new List<IAlgorithm>(m_DataPlanners.Keys);

                foreach (IAlgorithm algorithm in loaded)
                {
                    CancelScript(algorithm);
                }

                return true;
            }
        }

        /// <summary>
        /// Registers the remote gate.
        /// </summary>
        /// <param name="gate">The gate.</param>
        /// <returns></returns>
        public Boolean RegisterGate(IRemoteGateClient gate)
        {
            if (gate == null)
                throw new ArgumentNullException("gate");

            lock (m_SyncRoot)
            {
                /* Check if this gate is already registered */
                foreach (RemoteMachineRunner rr in m_RemoteRunners)
                {
                    if (rr.Gate == gate)
                        return false;
                }

                RemoteMachineRunner rrNew = new RemoteMachineRunner(gate);
                InitializeRunner(rrNew);

                /* Register this runner */
                m_RemoteRunners.Add(rrNew);

                return true;
            }
        }

        /// <summary>
        /// Unregisters the remote gate.
        /// </summary>
        /// <param name="gate">The gate.</param>
        /// <returns></returns>
        public Boolean UnregisterGate(IRemoteGateClient gate)
        {
            if (gate == null)
                throw new ArgumentNullException("gate");

            lock (m_SyncRoot)
            {
                /* Check if this gate is registered */
                foreach (RemoteMachineRunner rr in m_RemoteRunners)
                {
                    if (rr.Gate == gate)
                    {
                        /* Remove the gate from the load balancer */
                        m_LoadBalancer.UnregisterObject(rr);

                        /* Dispose the gate */
                        rr.Dispose();

                        /* Unregister the gate */
                        m_RemoteRunners.Remove(rr);

                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Occurs when algorithm progresses.
        /// </summary>
        public event ProgressEventHandler AlgorithmProgress;

        /// <summary>
        /// Occurs when algorithm completes.
        /// </summary>
        public event EventHandler AlgorithmComplete; 

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (m_SyncRoot)
            {
                /* Abort dispatcher */
                m_DispatcherThread.Abort();

                /* Cancel all algorithms */
                CancelAll();

                /* Stop all runners */
                if (m_LocalRunner != null)
                {
                    m_LocalRunner.Dispose();
                    m_LocalRunner = null;
                }

                foreach (RemoteMachineRunner rr in m_RemoteRunners)
                {
                    rr.Dispose();
                }
            }
        }

        #endregion
    }
}
