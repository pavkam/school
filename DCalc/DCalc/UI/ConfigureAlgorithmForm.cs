using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DCalcCore.Algorithm;
using DCalc.Algorithms;

namespace DCalc.UI
{
    /// <summary>
    /// Configure Algorithm Dialog.
    /// </summary>
    public partial class ConfigureAlgorithmForm : Form
    {
        #region Private Fields

        private IAlgorithmProvider m_Provider;
        private IAlgorithmCollection m_Algorithms;

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Selects the algorithm provider using the UI.
        /// </summary>
        /// <param name="currentProvider">The current provider.</param>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IAlgorithmProvider SelectAlgorithmProvider(IAlgorithmProvider currentProvider,
                IAlgorithmCollection collection)
        {
            ConfigureAlgorithmForm form = new ConfigureAlgorithmForm();
            form.m_Provider = currentProvider;
            form.m_Algorithms = collection;

            form.LoadAlgorithms();

            if (form.ShowDialog() == DialogResult.OK)
            {
                /* Selected Accept */
                return form.m_Provider;
            }
            else
                return currentProvider;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Controls the UI changes.
        /// </summary>
        private void ControlUIChanges()
        {
            btAccept.Enabled = (cbbAlgorithm.SelectedItem != null);
            btConfigure.Enabled = (cbbAlgorithm.SelectedItem != null);
        }

        /// <summary>
        /// Loads the algorithms.
        /// </summary>
        private void LoadAlgorithms()
        {
            foreach (IAlgorithmProvider provider in m_Algorithms.Providers)
            {
                cbbAlgorithm.Items.Add(new Tagger<IAlgorithmProvider>(provider.Name, provider));

                if (provider == m_Provider)
                    cbbAlgorithm.SelectedIndex = cbbAlgorithm.Items.Count - 1;
            }

            LoadAlgorithmInfo();
            ControlUIChanges();
        }

        /// <summary>
        /// Loads the algorithm info.
        /// </summary>
        private void LoadAlgorithmInfo()
        {
            if (cbbAlgorithm.SelectedItem != null)
            {
                IAlgorithmProvider provider = ((Tagger<IAlgorithmProvider>)cbbAlgorithm.SelectedItem).Object;

                edtDescription.Text = provider.Description;
                edtDeveloper.Text = provider.Developer;
                edtName.Text = provider.Name;
                edtVersion.Text = provider.VersionMajor + "." + provider.VersionMinor;
            }
            else
            {
                edtDescription.Text = String.Empty;
                edtDeveloper.Text = String.Empty;
                edtName.Text = String.Empty;
                edtVersion.Text = String.Empty;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureAlgorithmForm"/> class.
        /// </summary>
        private ConfigureAlgorithmForm()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cbbAlgorithm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cbbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAlgorithmInfo();
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the Click event of the btAccept control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btAccept_Click(object sender, EventArgs e)
        {
            if (cbbAlgorithm.SelectedItem != null)
                m_Provider = ((Tagger<IAlgorithmProvider>)cbbAlgorithm.SelectedItem).Object;

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
        /// Handles the Click event of the btConfigure control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btConfigure_Click(object sender, EventArgs e)
        {
            /* Run provider's code */
            if (cbbAlgorithm.SelectedItem != null)
            {
                IAlgorithmProvider provider = ((Tagger<IAlgorithmProvider>)cbbAlgorithm.SelectedItem).Object;
                provider.ConfigureProvider();
            }
        }

        /// <summary>
        /// Handles the KeyPress event of the cbbAlgorithm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
        private void cbbAlgorithm_KeyPress(object sender, KeyPressEventArgs e)
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