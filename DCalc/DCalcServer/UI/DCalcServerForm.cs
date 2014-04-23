using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DCalcCore.Remoting;
using DCalcCore.Utilities;
using DCalcCore.Remoting.Http;
using DCalcCore.Remoting.Tcp;
using DCalcCore.LoadBalancers;
using System.Threading;

namespace DCalcServer.UI
{
    /// <summary>
    /// Provides UI to the Http DCalc server.
    /// </summary>
    public partial class DCalcServerForm : Form
    {
        #region Private Fields

        /* Connectivity */
        private Boolean m_IsStarted;
        private IRemoteGateServer m_Server;

        /* Statistics */
        private Int64 m_ClientsConnected;
        private Int64 m_AlgorithmsPresent;
        private Int64 m_QueueSize;
        private Int64 m_PrevQueueSize;
        private Int64 m_NormalizedValue;

        #endregion

        #region Private Methods: Events

        public void m_server_AlgorithmRemoved(object sender, ConnectionEventArgs e)
        {
            Interlocked.Decrement(ref m_AlgorithmsPresent);
        }

        public void m_server_AlgorithmRegistered(object sender, ConnectionEventArgs e)
        {
            Interlocked.Increment(ref m_AlgorithmsPresent);
        }

        public void m_server_ClientUploadedInputSets(object sender, SetUpdateEventArgs e)
        {
            Interlocked.Add(ref m_QueueSize, e.SetCount);
        }

        public void m_server_ClientDownloadedOutputSets(object sender, SetUpdateEventArgs e)
        {
            Interlocked.Add(ref m_QueueSize, -1 * e.SetCount);
        }

        public void m_server_ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            Interlocked.Decrement(ref m_ClientsConnected);
        }

        public void m_server_ClientConnected(object sender, ConnectionEventArgs e)
        {
            Interlocked.Increment(ref m_ClientsConnected);
        }

        #endregion

        #region Private Methods: Helpers

        /// <summary>
        /// Starts the server.
        /// </summary>
        private void StartServer()
        {
            String securityKey = null;

            if (cbSecure.Checked)
                securityKey = edtSecurityKey.Text;

            Int32 port = Convert.ToInt32(edtListeningPort.Text);
            Int32 timeOut = Convert.ToInt32(edtLifeTime.Text) * 1000;
            Int32 threadCount = 0;

            if (rbSingleThread.Checked)
                threadCount = 1;
            else if (rbCoreCount.Checked)
                threadCount = Environment.ProcessorCount;
            else if (rbCustomCount.Checked)
                threadCount = Convert.ToInt32(edtCustomCount.Text);

            /* Create a new instance of the server */
            if (cbbServerType.SelectedIndex == 0)
            {
                if (cbbLoadBalancer.SelectedIndex == 0)
                {
                    m_Server = new HttpRemoteGateServer<FairLoadBalancer>(threadCount, port, timeOut, 16, securityKey);
                }
                else if (cbbLoadBalancer.SelectedIndex == 1)
                {
                    m_Server = new HttpRemoteGateServer<RRLoadBalancer>(threadCount, port, timeOut, 16, securityKey);
                }
                else if (cbbLoadBalancer.SelectedIndex == 2)
                {
                    m_Server = new HttpRemoteGateServer<PredictiveLoadBalancer>(threadCount, port, timeOut, 16, securityKey);
                }
                else
                    throw new Exception();
            }
            else
            {
                if (cbbLoadBalancer.SelectedIndex == 0)
                {
                    m_Server = new TcpRemoteGateServer<FairLoadBalancer>(threadCount, port, timeOut, 16, securityKey);
                }
                else if (cbbLoadBalancer.SelectedIndex == 1)
                {
                    m_Server = new TcpRemoteGateServer<RRLoadBalancer>(threadCount, port, timeOut, 16, securityKey);
                }
                else if (cbbLoadBalancer.SelectedIndex == 2)
                {
                    m_Server = new TcpRemoteGateServer<PredictiveLoadBalancer>(threadCount, port, timeOut, 16, securityKey);
                }
                else
                    throw new Exception();
            }

            m_Server.ClientConnected += m_server_ClientConnected;
            m_Server.ClientDisconnected += m_server_ClientDisconnected;
            m_Server.ClientDownloadedOutputSets += m_server_ClientDownloadedOutputSets;
            m_Server.ClientUploadedInputSets += m_server_ClientUploadedInputSets;
            m_Server.AlgorithmRegistered += m_server_AlgorithmRegistered;
            m_Server.AlgorithmRemoved += m_server_AlgorithmRemoved;

            /* Reset locals */
            m_ClientsConnected = 0;
            m_AlgorithmsPresent = 0;
            m_QueueSize = 0;

            if (!m_Server.Start())
            {
                /* Something went wrong */
                MessageBox.Show("Could not start listening on the selected port!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                m_Server.Dispose();
                return;
            }

            /* Reset the graph */
            graphView.AddLine(0, Color.Yellow).Thickness = 2;

            UpdateStatistics();

            statsTimer.Start();
            m_IsStarted = true;
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        private void StopServer()
        {
            /* Stop and dispose the server */
            m_Server.Stop();
            m_Server.Dispose();

            m_ClientsConnected = 0;
            m_AlgorithmsPresent = 0;
            m_QueueSize = 0;
            m_PrevQueueSize = 0;
            m_NormalizedValue = 0;

            UpdateStatistics();

            statsTimer.Stop();
            m_IsStarted = false;

            /* Update graph */
            graphView.RemoveLine(0);
            graphView.UpdateGraph();
        }

        /// <summary>
        /// Fills in the balancers.
        /// </summary>
        private void FillInBalancers()
        {
            cbbLoadBalancer.Items.Clear();

            /* Add those balancers */
            cbbLoadBalancer.Items.Add("Fair Load Balancer (recommended)");
            cbbLoadBalancer.Items.Add("Round Robin Load Balancer");
            cbbLoadBalancer.Items.Add("Predictive Load Balancer");

            cbbLoadBalancer.SelectedIndex = 0;
        }

        /// <summary>
        /// Controls the UI changes.
        /// </summary>
        private void ControlUIChanges()
        {
            if (!m_IsStarted)
            {
                Boolean enableStart = true;

                if (edtListeningPort.TextLength == 0)
                    enableStart = false;

                if (edtLifeTime.TextLength == 0)
                    enableStart = false;

                try
                {
                    Convert.ToUInt16(edtListeningPort.Text);

                    if (Convert.ToUInt16(edtLifeTime.Text) < 1)
                        throw new Exception();
                }
                catch
                {
                    enableStart = false;
                }

                if (rbCustomCount.Checked)
                {
                    if (edtCustomCount.TextLength == 0)
                        enableStart = false;

                    try
                    {
                        Convert.ToUInt16(edtCustomCount.Text);
                    }
                    catch
                    {
                        enableStart = false;
                    }
                }

                if (edtSecurityKey.TextLength < 1 && cbSecure.Checked)
                    enableStart = false;

                btStart.Enabled = enableStart;
                btStop.Enabled = false;

                edtListeningPort.Enabled = true;
                edtLifeTime.Enabled = true;
                cbbLoadBalancer.Enabled = true;
                rbSingleThread.Enabled = true;
                rbCoreCount.Enabled = true;
                rbCustomCount.Enabled = true;
                cbbServerType.Enabled = true;
                edtCustomCount.Enabled = (rbCustomCount.Checked);

                btRandom.Enabled = cbSecure.Checked;
                edtSecurityKey.Enabled = cbSecure.Checked;

                cbSecure.Enabled = true;
            }
            else
            {
                btStart.Enabled = false;
                btStop.Enabled = true;
                edtListeningPort.Enabled = false;

                edtLifeTime.Enabled = false;
                cbbLoadBalancer.Enabled = false;

                rbSingleThread.Enabled = false;
                rbCoreCount.Enabled = false;
                rbCustomCount.Enabled = false;
                edtCustomCount.Enabled = false;
                cbbServerType.Enabled = false;

                cbSecure.Enabled = false;
                btRandom.Enabled = false;
                edtSecurityKey.Enabled = false;
            }
        }

        /// <summary>
        /// Updates the statistics.
        /// </summary>
        private void UpdateStatistics()
        {
            edtAlgorithms.Text = Interlocked.Read(ref m_AlgorithmsPresent).ToString();
            edtClients.Text = Interlocked.Read(ref m_ClientsConnected).ToString();

            graphView.MaxLabel = String.Format("{0:D4} sets", m_NormalizedValue);
        } 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DCalcServerForm"/> class.
        /// </summary>
        public DCalcServerForm()
        {
            InitializeComponent();

            FillInBalancers();

            ControlUIChanges();
            UpdateStatistics();

            cbbServerType.SelectedIndex = 0;
        }

        #endregion

        #region UI Events

        /// <summary>
        /// Handles the Click event of the btStart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btStart_Click(object sender, EventArgs e)
        {
            StartServer();
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the btStop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btStop_Click(object sender, EventArgs e)
        {
            StopServer();

            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the btExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the TextChanged event of the all control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void all_TextChanged(object sender, EventArgs e)
        {
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the FormClosing event of the DCalcServerForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void DCalcServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_IsStarted)
            {
                /* Confirm first */
                if (MessageBox.Show("Do you really want to stop the server?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    StopServer();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Handles the Tick event of the statsTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void statsTimer_Tick(object sender, EventArgs e)
        {
            Int64 newQueueSize = Interlocked.Read(ref m_QueueSize);
            m_NormalizedValue = (newQueueSize + m_PrevQueueSize) / 2;
            m_PrevQueueSize = newQueueSize;

            /* Update graph */
            graphView.Push((Int32)m_NormalizedValue, 0);
            graphView.UpdateGraph();

            UpdateStatistics();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rbCustomCount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void rbCustomCount_CheckedChanged(object sender, EventArgs e)
        {
            edtCustomCount.Enabled = rbCustomCount.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbSecure control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cbSecure_CheckedChanged(object sender, EventArgs e)
        {
            edtSecurityKey.Enabled = cbSecure.Checked;
            btRandom.Enabled = cbSecure.Checked;
        }

        /// <summary>
        /// Handles the Click event of the btRandom control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btRandom_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            String allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZ";

            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < 40; i++)
            {
                Char c = allowedChars[random.Next(allowedChars.Length)];
                sb.Append(c);
            }

            edtSecurityKey.Text = sb.ToString();
        }

        #endregion
    }
}