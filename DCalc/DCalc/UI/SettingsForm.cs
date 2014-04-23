using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DCalc.Communication;
using DCalcCore.LoadBalancers;

namespace DCalc.UI
{
    /// <summary>
    /// Settings Dialog UI.
    /// </summary>
    public partial class SettingsForm : Form
    {
        #region Public Static Methods

        /// <summary>
        /// Called externally to interogate the user in a nive UI.
        /// </summary>
        /// <param name="workSpace">The work space.</param>
        public static void SetupDispatcher(IWorkSpace workSpace)
        {
            SettingsForm form = new SettingsForm();

            form.cbbLocalLoadBalancer.SelectedIndex = form.SelectIndexByType(form.cbbLocalLoadBalancer, workSpace.LocalBalancer);
            form.cbbRemoteLoadBalancer.SelectedIndex = form.SelectIndexByType(form.cbbRemoteLoadBalancer, workSpace.RemoteBalancer);

            form.edtQueueSize.Text = workSpace.QueueSize.ToString();

            if (workSpace.NumberOfThreads == 1)
                form.rbSingleThread.Checked = true;
            else if (workSpace.NumberOfThreads == 0)
                form.rbProcCountThreads.Checked = true;
            else
            {
                form.rbCustomCount.Checked = true;
                form.edtCustomCount.Text = workSpace.NumberOfThreads.ToString();
            }

            form.ControlUIChanges();

            if (form.ShowDialog() == DialogResult.OK)
            {
                workSpace.LocalBalancer = form.SelectTypeByIndex(form.cbbLocalLoadBalancer);
                workSpace.RemoteBalancer = form.SelectTypeByIndex(form.cbbRemoteLoadBalancer);
                workSpace.QueueSize = Convert.ToInt32(form.edtQueueSize.Text);

                if (form.rbCustomCount.Checked)
                    workSpace.NumberOfThreads = Convert.ToInt32(form.edtCustomCount.Text);
                else if (form.rbSingleThread.Checked)
                    workSpace.NumberOfThreads = 1;
                else workSpace.NumberOfThreads = 0;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Selects the load balancer index by type.
        /// </summary>
        /// <param name="cbbLoadBalancer">The load balancer combobox control.</param>
        /// <param name="balancerType">Type of the balancer.</param>
        /// <returns></returns>
        private Int32 SelectIndexByType(ComboBox cbbLoadBalancer, Type balancerType)
        {
            if (balancerType == typeof(FairLoadBalancer))
                return 0;

            if (balancerType == typeof(RRLoadBalancer))
                return 1;

            if (balancerType == typeof(PredictiveLoadBalancer))
                return 2;

            return 0;
        }

        /// <summary>
        /// Selects the balancer type by given index.
        /// </summary>
        /// <param name="cbbLoadBalancer">The combobox that holds the load balancers.</param>
        /// <returns></returns>
        private Type SelectTypeByIndex(ComboBox cbbLoadBalancer)
        {
            if (cbbLoadBalancer.SelectedIndex == 0)
                return typeof(FairLoadBalancer);

            if (cbbLoadBalancer.SelectedIndex == 1)
                return typeof(RRLoadBalancer);

            if (cbbLoadBalancer.SelectedIndex == 2)
                return typeof(PredictiveLoadBalancer);

            return typeof(FairLoadBalancer);
        }

        /// <summary>
        /// Fills the in balancers.
        /// </summary>
        /// <param name="cbbLoadBalancer">The combobox holding balancers.</param>
        private void FillInBalancers(ComboBox cbbLoadBalancer)
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
            Boolean acceptEnabled = true;

            if (edtQueueSize.TextLength == 0)
                acceptEnabled = false;

            if (edtCustomCount.TextLength == 0 && rbCustomCount.Checked)
                acceptEnabled = false;

            try
            {
                if (Convert.ToInt32(edtQueueSize.Text) < 1)
                    acceptEnabled = false;

                if (rbCustomCount.Checked && Convert.ToInt32(edtQueueSize.Text) < 1)
                    acceptEnabled = false;
            }
            catch
            {
                acceptEnabled = false;
            }

            edtCustomCount.Enabled = rbCustomCount.Checked;
            btAccept.Enabled = acceptEnabled;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsForm"/> class.
        /// </summary>
        private SettingsForm()
        {
            InitializeComponent();

            FillInBalancers(cbbLocalLoadBalancer);
            FillInBalancers(cbbRemoteLoadBalancer);
        } 

        #endregion

        #region UI Events

        /// <summary>
        /// Handles the TextChanged event of the edtQueueSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void edtQueueSize_TextChanged(object sender, EventArgs e)
        {
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the btAccept control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btAccept_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the Click event of the btCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
        /// Handles the KeyPress event of all text controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
        private void all_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (btAccept.Enabled)
                    btAccept_Click(btAccept, e);
            }

            if (e.KeyChar == 27)
            {
                btCancel_Click(btCancel, e);
            }
        } 

        #endregion
    }
}