using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EchoAlgorithm.UI
{
    /// <summary>
    /// Echo plugin cunfiguration dialog.
    /// </summary>
    partial class ConfigurationForm : Form
    {
        #region Public Static Methods

        /// <summary>
        /// Configures the algorithm.
        /// </summary>
        /// <param name="currentCycleCount">The current cycle count.</param>
        /// <param name="numberOfSets">The number of sets.</param>
        public static void ConfigureAlgorithm(ref Int32 currentCycleCount, ref Int32 numberOfSets)
        {
            ConfigurationForm form = new ConfigurationForm();
            form.edtDelayCycles.Text = currentCycleCount.ToString();
            form.edtNumberOfSets.Text = numberOfSets.ToString();
            form.ControlUIChanges();

            if (form.ShowDialog() == DialogResult.OK)
            {
                currentCycleCount = Convert.ToInt32(form.edtDelayCycles.Text);
                numberOfSets = Convert.ToInt32(form.edtNumberOfSets.Text);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Controls the UI changes.
        /// </summary>
        private void ControlUIChanges()
        {
            Boolean acceptEnabled = true;

            if (edtDelayCycles.TextLength == 0)
                acceptEnabled = false;

            if (edtNumberOfSets.TextLength == 0)
                acceptEnabled = false;

            try
            {
                if (Convert.ToInt32(edtDelayCycles.Text) < 1)
                    acceptEnabled = false;

                if (Convert.ToInt32(edtNumberOfSets.Text) < 1)
                    acceptEnabled = false;
            }
            catch
            {
                acceptEnabled = false;
            }

            btAccept.Enabled = acceptEnabled;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationForm"/> class.
        /// </summary>
        private ConfigurationForm()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

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
        /// Handles the Click event of the btAccept control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btAccept_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the TextChanged event of all text controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void text_TextChanged(object sender, EventArgs e)
        {
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the KeyPress event of the all text controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
        private void text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (btAccept.Enabled)
                {
                    btAccept_Click(btAccept, e);
                }
            }

            if (e.KeyChar == 27)
            {
                btCancel_Click(btCancel, e);
            }
        } 

        #endregion
    }
}