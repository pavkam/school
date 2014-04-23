using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using CustomUIControls;
using DCalc.Communication;
using DCalc.Algorithms;
using DCalcCore.Algorithm;
using DCalcCore.Remoting;
using DCalcCore.Remoting.Http;
using DCalcCore.Assemblers;
using DCalcCore.Utilities;
using DCalcCore.LoadBalancers;
using DCalcCore;
using DCalcCore.Remoting.Tcp;


namespace DCalc.UI
{
    /// <summary>
    /// Client UI.
    /// </summary>
    public partial class DCalcForm : Form
    {
        #region Private Fields

        /* Dispatcher */
        private IDispatcher m_Dispatcher;
        private IAlgorithm m_AlgorithmInstance;
        private Boolean m_IsRunning;

        /* Workspace */
        private DirectoryAlgorithmCollection m_Algorithms;
        private XmlStoredWorkSpace m_WorkSpace;
        private Boolean m_IsWorkSpaceModified;
        private Boolean m_IsWorkSpaceNew;

        /* Statistics */
        private Nullable<DateTime> m_StartTime;
        private DateTime m_EndTime;
        private String m_EventSync = "DCalcForm Sync";
        private Int64 m_MaxWorkValue;
        private Int64 m_CurrentWorkValue;
        private Int64 m_LastWorkValue;
        private Int64 m_LastProgressValue;
        private Double m_AverageSpeed;
        private Int64 m_MeasurmentsDone;
        private Boolean m_Finished;

        #endregion

        #region Private Methods: Work Space Control

        /// <summary>
        /// Checks the work space is saved.
        /// </summary>
        /// <returns></returns>
        private Boolean CheckWorkSpaceIsSaved()
        {
            if (!m_IsWorkSpaceModified)
                return true;

            DialogResult dr = MessageBox.Show("Current workspace has been modified. Do you wish to save it?", "Confirm",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

            if (dr == DialogResult.Cancel)
            {
                return false;
            }

            if (dr == DialogResult.Yes)
            {
                /* Save current file */
                try
                {
                    saveToolStripMenuItem_Click(saveToolStripMenuItem, EventArgs.Empty);
                }
                catch
                {
                    /* Aborted */
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a new work space.
        /// </summary>
        private void CreateNewWorkSpace()
        {
            m_WorkSpace = new XmlStoredWorkSpace("Untitled");
            m_WorkSpace.Algorithms = m_Algorithms;

            m_IsWorkSpaceNew = true;
            m_IsWorkSpaceModified = false;

            /* Clear the tree up */
            RedrawNodeList();
        }

        /// <summary>
        /// Loads a work space.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private void LoadWorkSpace(String fileName)
        {
            m_WorkSpace = new XmlStoredWorkSpace(fileName);
            m_WorkSpace.Algorithms = m_Algorithms;

            if (m_WorkSpace.Load())
            {
                m_IsWorkSpaceModified = false;
                m_IsWorkSpaceNew = false;
                RedrawNodeList();
                UpdateStatusBar();
            }
            else
            {
                MessageBox.Show("Could not load the selected server list file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves a work space.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private void SaveWorkSpace(String fileName)
        {
            /* Save this list */
            m_WorkSpace.FileName = fileName;

            if (m_WorkSpace.Save())
            {
                m_IsWorkSpaceModified = false;
                m_IsWorkSpaceNew = false;
            }
            else
            {
                MessageBox.Show("Could not save the selected server list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 

        #endregion

        #region Private Methods: Tree View Control

        /// <summary>
        /// Updates the node for server.
        /// </summary>
        /// <param name="server">The server.</param>
        private void UpdateNodeForServer(IServer server)
        {
            foreach (TreeNode node in tvServers.Nodes)
            {
                IServer sv = (IServer)node.Tag;

                if (server == sv)
                {
                    node.Text = sv.Signature;

                    node.ImageIndex = GetImageIndexByStatus(server.Status);
                    node.SelectedImageIndex = GetImageIndexByStatus(server.Status);
                    node.StateImageIndex = GetImageIndexByStatus(server.Status);

                    break;
                }
            }
        }

        /// <summary>
        /// Updates all nodes.
        /// </summary>
        private void UpdateAllNodes()
        {
            foreach (TreeNode node in tvServers.Nodes)
            {
                IServer sv = (IServer)node.Tag;

                node.Text = sv.Signature;
                node.Checked = sv.Enabled;

                node.ImageIndex = GetImageIndexByStatus(sv.Status);
                node.SelectedImageIndex = GetImageIndexByStatus(sv.Status);
                node.StateImageIndex = GetImageIndexByStatus(sv.Status);
            }
        }

        /// <summary>
        /// Redraws the node list.
        /// </summary>
        private void RedrawNodeList()
        {
            /* Save the Signature of the selected item */
            String signature = null;

            if (tvServers.SelectedNode != null)
            {
                signature = ((IServer)tvServers.SelectedNode.Tag).Signature.ToUpper();
            }

            /* Re-create node list */
            tvServers.Nodes.Clear();

            TreeNode toSelect = null;

            foreach (IServer server in m_WorkSpace.Servers)
            {
                TreeNode newNode = new TreeNode();
                newNode.Text = server.Signature;
                newNode.Checked = server.Enabled;
                newNode.SelectedImageIndex = GetImageIndexByStatus(server.Status);
                newNode.ImageIndex = GetImageIndexByStatus(server.Status);
                newNode.StateImageIndex = GetImageIndexByStatus(server.Status);
                newNode.Tag = server;

                tvServers.Nodes.Add(newNode);

                if (server.Signature.ToUpper().Equals(signature))
                    toSelect = newNode;
            }

            /* Re-select the previous item */
            if (toSelect != null)
                tvServers.SelectedNode = toSelect;
        }

        /// <summary>
        /// Check is at least one node is selected.
        /// </summary>
        /// <returns></returns>
        private Boolean AtLeastOneNodeIsSelected()
        {
            foreach (IServer server in m_WorkSpace.Servers)
            {
                if (server.Enabled)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the image index by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        private Int32 GetImageIndexByStatus(ServerStatus status)
        {
            if (status == ServerStatus.Disabled)
                return 0;
            else if (status == ServerStatus.Down)
                return 1;
            else if (status == ServerStatus.Running)
                return 2;
            else
                return -1;
        }

        #endregion

        #region Private Methods: UI Helpers

        /// <summary>
        /// Updates the status bar.
        /// </summary>
        private void UpdateStatusBar()
        {
            if (m_WorkSpace.Provider == null)
            {
                tsSelectedProvider.Text = "Selected Algorithm: [None]";
            }
            else
            {
                tsSelectedProvider.Text = "Selected Algorithm: \"" + m_WorkSpace.Provider.Name + "\"";
            }
        }

        /// <summary>
        /// Sets the application title.
        /// </summary>
        private void SetApplicationTitle()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Path.GetFileName(m_WorkSpace.FileName));

            if (m_IsWorkSpaceModified)
                sb.Append("*");

            sb.Append(" - DCalc Client");

            Text = sb.ToString();
        }

        /// <summary>
        /// Controls the UI changes.
        /// </summary>
        private void ControlUIChanges()
        {
            SetApplicationTitle();

            if (m_IsRunning)
            {
                lbStats.Visible = true;

                btAddServer.Enabled = false;
                btEditServer.Enabled = false;
                btRemoveServer.Enabled = false;

                newToolStripMenuItem.Enabled = false;
                openToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;


                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;
                algorithmToolStripMenuItem.Enabled = false;
                helpToolStripMenuItem.Enabled = false;
                optionsToolStripMenuItem.Enabled = false;

                tvServers.Enabled = false;

                return;
            }
            else
            {
                lbStats.Visible = false;

                btAddServer.Enabled = true;

                newToolStripMenuItem.Enabled = true;
                openToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;

                startToolStripMenuItem.Enabled = (m_WorkSpace.Provider != null) && (AtLeastOneNodeIsSelected());
                stopToolStripMenuItem.Enabled = false;
                algorithmToolStripMenuItem.Enabled = true;
                helpToolStripMenuItem.Enabled = true;
                optionsToolStripMenuItem.Enabled = true;

                tvServers.Enabled = true;
            }

            exitToolStripMenuItem.Enabled = (!m_IsRunning);

            if (tvServers.SelectedNode != null)
            {
                IServer server = (IServer)tvServers.SelectedNode.Tag;

                btEditServer.Enabled = (server is RemoteServer);
                btRemoveServer.Enabled = (server is RemoteServer);
            }
            else
            {
                btEditServer.Enabled = false;
                btRemoveServer.Enabled = false;
            }
        }

        /// <summary>
        /// Updates the status label.
        /// </summary>
        private void UpdateStatusLabel()
        {
            lbStats.Text = String.Format("Evaluating set {0} of {1}. Average speed: {2} sets/s.",
                m_CurrentWorkValue, m_MaxWorkValue, Math.Round(m_AverageSpeed, 2));
        }

        /// <summary>
        /// Updates the excution status.
        /// </summary>
        private void UpdateExcutionStatus()
        {
            if (m_Finished)
            {
                
                TimeSpan totalTime = m_EndTime - m_StartTime.Value;

                StopExecution();

                if (MessageBox.Show(
                    String.Format("Execution of the selected algorithm has completed in {0}m:{1}s:{2}ms.\nAverage execution speed was: {3} sets/s.\nDo you wish to see the results?",
                    totalTime.Minutes, totalTime.Seconds, totalTime.Milliseconds, Math.Round(m_AverageSpeed, 2)), "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m_WorkSpace.Provider.ShowResult(m_AlgorithmInstance);
                }

                ControlUIChanges();

                return;
            }

            Int64 maxValue = m_MaxWorkValue;
            Int64 currValue = m_CurrentWorkValue;

            lock (m_EventSync)
            {
                maxValue = m_MaxWorkValue;
                currValue = m_CurrentWorkValue;
            }

            pgBar.Maximum = (Int32)maxValue;
            pgBar.Value = (Int32)currValue;

            /* Refresh the nodes */
            foreach (TreeNode node in tvServers.Nodes)
            {
                IServer server = (IServer)node.Tag;
                node.ImageIndex = GetImageIndexByStatus(server.Status);
                node.SelectedImageIndex = GetImageIndexByStatus(server.Status);
                node.StateImageIndex = GetImageIndexByStatus(server.Status);
            }

            /* Graphic */
            Int64 progress = (currValue - m_LastWorkValue);
            m_LastWorkValue = m_CurrentWorkValue;
            Int64 normalizedProgress = (m_LastProgressValue + progress) / 2;

            m_AverageSpeed = (((progress * 1000.0) / pgBarTimer.Interval) + 
                (m_AverageSpeed * m_MeasurmentsDone)) / (m_MeasurmentsDone + 1);

            m_MeasurmentsDone++;
            m_LastProgressValue = progress;

            graphView.MaxLabel = String.Format("{0:D5} sets/s", ((normalizedProgress * 1000) / pgBarTimer.Interval));

            graphView.Push((Int32)normalizedProgress, 0);
            graphView.UpdateGraph();

            UpdateStatusLabel();
        }

        #endregion

        #region Private Methods: Execution Helpers

        /// <summary>
        /// Starts the execution.
        /// </summary>
        private void StartExecution()
        {
            pgBar.Value = 0;
            Boolean localEnabled = false, remoteEnabled = false;
            List<IRemoteGateClient> clients = new List<IRemoteGateClient>();

            /* Manually control the status of local machine */
            foreach (IServer server in m_WorkSpace.Servers)
            {
                if (server is LocalServer)
                {
                    if (server.Enabled)
                    {
                        server.Status = ServerStatus.Running;
                        localEnabled = true;
                    }
                }

                if (server is RemoteServer)
                {
                    if (server.Enabled)
                    {
                        RemoteServer rServer = (RemoteServer)server;

                        server.Status = ServerStatus.Down;
                        IRemoteGateClient newClient = null;

                        if (rServer.ConnectionType == ConnectionType.Http)
                        {
                            if (rServer.SecurityKey != null)
                                newClient = new HttpRemoteGateClient(rServer.Host, rServer.Port, rServer.SecurityKey, 1000);
                            else
                                newClient = new HttpRemoteGateClient(rServer.Host, rServer.Port, 1000);
                        }
                        else if (rServer.ConnectionType == ConnectionType.Tcp)
                        {
                            if (rServer.SecurityKey != null)
                                newClient = new TcpRemoteGateClient(rServer.Host, rServer.Port, rServer.SecurityKey, 1000);
                            else
                                newClient = new TcpRemoteGateClient(rServer.Host, rServer.Port, 1000);
                        }

                        newClient.ConnectedToServer += newClient_ConnectedToServer;
                        newClient.DisconnectedFromServer += newClient_DisconnectedFromServer;
                        rServer.Client = newClient;

                        clients.Add(newClient);
                        remoteEnabled = true;
                    }
                }
            }

            DispatchMode mode = DispatchMode.Combined;

            if (localEnabled && remoteEnabled)
                mode = DispatchMode.Combined;
            else if (localEnabled)
                mode = DispatchMode.LocalOnly;
            else if (remoteEnabled)
                mode = DispatchMode.RemoteOnly;

            UpdateAllNodes();

            /* Runtime creation of Generic type */
            Type dynamicDispatcher = typeof(Dispatcher<,,>).MakeGenericType(new Type[] { typeof(CSharpScriptAssembler), m_WorkSpace.LocalBalancer, m_WorkSpace.RemoteBalancer });

            m_Dispatcher = (IDispatcher)Activator.CreateInstance(dynamicDispatcher,
                new Object[] { m_WorkSpace.QueueSize, 
                    (m_WorkSpace.NumberOfThreads == 0) ? Environment.ProcessorCount : m_WorkSpace.NumberOfThreads, 
                    mode });

            m_Dispatcher.AlgorithmProgress += m_dispatcher_AlgorithmProgress;
            m_Dispatcher.AlgorithmComplete += m_dispatcher_AlgorithmComplete;

            /* Add remote hosts */
            foreach (IRemoteGateClient client in clients)
            {
                m_Dispatcher.RegisterGate(client);
            }

            m_AlgorithmInstance = m_WorkSpace.Provider.GetAlgorithmInstance();

            m_CurrentWorkValue = 0;
            m_LastWorkValue = 0;
            m_MaxWorkValue = 0;
            m_MeasurmentsDone = 0;
            m_AverageSpeed = 0;

            m_Finished = false;
            UpdateStatusLabel();

            graphView.AddLine(0, Color.Red).Thickness = 2;

            m_Dispatcher.Execute(m_AlgorithmInstance);
            pgBarTimer.Start();

            m_IsRunning = true;
        }

        /// <summary>
        /// Stops the execution.
        /// </summary>
        private void StopExecution()
        {
            pgBar.Value = 0;
            m_StartTime = null;

            m_Dispatcher.CancelAll();
            m_Dispatcher.Dispose();

            pgBarTimer.Stop();

            /* Manually control the status of local machine */
            foreach (IServer server in m_WorkSpace.Servers)
            {
                if (server is RemoteServer)
                {
                    RemoteServer rServer = (RemoteServer)server;

                    if (rServer.Client != null)
                    {
                        rServer.Client.ConnectedToServer -= newClient_ConnectedToServer;
                        rServer.Client.DisconnectedFromServer -= newClient_DisconnectedFromServer;
                    }
                }

                server.Status = ServerStatus.Disabled;
            }

            UpdateAllNodes();

            m_LastWorkValue = 0;
            m_MaxWorkValue = 0;
            m_CurrentWorkValue = 0;
            m_LastProgressValue = 0;

            graphView.MaxLabel = "0000 sets/s";
            graphView.RemoveLine(0);
            graphView.UpdateGraph();

            m_IsRunning = false;
        }

        /// <summary>
        /// Tries to stop execution.
        /// </summary>
        /// <returns></returns>
        private Boolean TryStopExecution()
        {
            if (MessageBox.Show("The execution process is currently on the way. Do you still want to exit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Private Methods: Events

        /// <summary>
        /// Handles the DisconnectedFromServer event of a gate.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void newClient_DisconnectedFromServer(object sender, EventArgs e)
        {
            lock (m_EventSync)
            {
                IRemoteGateClient client = (IRemoteGateClient)sender;

                /* Find the attached server */
                foreach (IServer server in m_WorkSpace.Servers)
                {
                    if (server is RemoteServer)
                    {
                        RemoteServer rServer = (RemoteServer)server;
                        if (rServer.Client == client)
                        {
                            /* Update state */
                            rServer.Status = ServerStatus.Down;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the ConnectedToServer event of a gate.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void newClient_ConnectedToServer(object sender, EventArgs e)
        {
            lock (m_EventSync)
            {
                IRemoteGateClient client = (IRemoteGateClient)sender;

                /* Find the attached server */
                foreach (IServer server in m_WorkSpace.Servers)
                {
                    if (server is RemoteServer)
                    {
                        RemoteServer rServer = (RemoteServer)server;
                        if (rServer.Client == client)
                        {
                            /* Update state */
                            rServer.Status = ServerStatus.Running;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the AlgorithmComplete event of the the dispatcher.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void m_dispatcher_AlgorithmComplete(object sender, EventArgs e)
        {
            lock (m_EventSync)
            {
                m_EndTime = DateTime.Now;
                m_Finished = true;
            }
        }

        /// <summary>
        /// Handles the AlgorithmProgress event of the dispatcher.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DCalcCore.Utilities.ProgressEventArgs"/> instance containing the event data.</param>
        public void m_dispatcher_AlgorithmProgress(Object sender, ProgressEventArgs e)
        {
            lock (m_EventSync)
            {
                if (m_StartTime == null)
                    m_StartTime = DateTime.Now;

                m_MaxWorkValue = e.Max;
                m_CurrentWorkValue = e.Current;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DCalcForm"/> class.
        /// </summary>
        public DCalcForm()
        {
            InitializeComponent();

            /* Initialize work space */
            CreateNewWorkSpace();
            UpdateStatusBar();
            ControlUIChanges();

            /* Search for algorithms */
            m_Algorithms = new DirectoryAlgorithmCollection(
                AppDomain.CurrentDomain.BaseDirectory + "AddIns");

            m_Algorithms.LoadAll();
        }

        #endregion

        #region UI Events

        /// <summary>
        /// Handles the Click event of the newToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Create a new work space */
            if (!CheckWorkSpaceIsSaved())
                return;

            CreateNewWorkSpace();
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the openToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckWorkSpaceIsSaved())
                return;

            /* Browse for a server list */
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String fileName = openFileDialog.FileName;

                LoadWorkSpace(fileName);
            }

            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the saveToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Save server list */
            if (m_IsWorkSpaceNew)
                saveAsToolStripMenuItem_Click(sender, e);
            else
                SaveWorkSpace(m_WorkSpace.FileName);

            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the saveAsToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Browse for a server list */
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                String fileName = saveFileDialog.FileName;
                SaveWorkSpace(fileName);
            }
            else
            {
                if (sender == saveToolStripMenuItem)
                {
                    throw new Exception();
                }
            }

            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the exitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the FormClosing event of the DCalcForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void DCalcForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_IsRunning)
            {
                if (TryStopExecution())
                {
                    StopExecution();
                    ControlUIChanges();
                }
                else
                {
                    e.Cancel = true;
                }
            }

            if (!CheckWorkSpaceIsSaved())
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Handles the Click event of the btAddServer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btAddServer_Click(object sender, EventArgs e)
        {
            /* Add a new server */
            IServer server = EditServerForm.CreateServerEntry();

            if (server != null)
            {
                m_WorkSpace.AddServer(server);

                /* Refresh node list */
                RedrawNodeList();

                m_IsWorkSpaceModified = true;
            }

            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the btEditServer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btEditServer_Click(object sender, EventArgs e)
        {
            /* Find the selected server */
            IServer server = (IServer)tvServers.SelectedNode.Tag;

            if (server is RemoteServer)
            {
                server = EditServerForm.EditServerEntry(server);

                if (server != null)
                {
                    /* Refresh node list */
                    UpdateNodeForServer(server);
                    m_IsWorkSpaceModified = true;
                }
            }

            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the btRemoveServer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btRemoveServer_Click(object sender, EventArgs e)
        {
            /* Find the selected server */
            IServer server = (IServer)tvServers.SelectedNode.Tag;

            if (server is RemoteServer)
            {
                m_WorkSpace.RemoveServer(server);

                /* Refresh node list */
                RedrawNodeList();

                m_IsWorkSpaceModified = true;
            }

            ControlUIChanges();
        }

        /// <summary>
        /// Handles the AfterSelect event of the tvServers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void tvServers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the NodeMouseDoubleClick event of the tvServers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        private void tvServers_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            /* Find the selected server */
            IServer server = (IServer)e.Node.Tag;

            if (server is RemoteServer)
            {
                server = EditServerForm.EditServerEntry(server);

                if (server != null)
                {
                    /* Refresh node list */
                    UpdateNodeForServer(server);
                    m_IsWorkSpaceModified = true;
                }
            }

            ControlUIChanges();
        }

        /// <summary>
        /// Handles the AfterCheck event of the tvServers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void tvServers_AfterCheck(object sender, TreeViewEventArgs e)
        {
            /* Find the server that was toggled */
            IServer server = (IServer)e.Node.Tag;

            server.Enabled = e.Node.Checked;
            m_IsWorkSpaceModified = true;

            UpdateNodeForServer(server);
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the algorithmToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void algorithmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Configure algorithm to use */
            m_WorkSpace.Provider = ConfigureAlgorithmForm.SelectAlgorithmProvider(m_WorkSpace.Provider, m_Algorithms);

            UpdateStatusBar();
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the startToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartExecution();
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the stopToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopExecution();
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Tick event of the pgBarTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void pgBarTimer_Tick(object sender, EventArgs e)
        {
            UpdateExcutionStatus();
        }

        /// <summary>
        /// Handles the Click event of the optionsToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm.SetupDispatcher(m_WorkSpace);
            m_IsWorkSpaceModified = true;
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the aboutToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("DCalc Distributed Algorithm Calculus Environment. Created by Ciobanu Alexandru. 2008.", "About", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        #endregion
    }
}