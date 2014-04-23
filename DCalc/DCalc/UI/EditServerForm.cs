using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DCalc.Communication;

namespace DCalc.UI
{
    /// <summary>
    /// Server edit dialog.
    /// </summary>
    public partial class EditServerForm : Form
    {
        #region Public Static Methods

        /// <summary>
        /// Creates a new entry server entry giving a UI dialog.
        /// </summary>
        /// <returns></returns>
        public static IServer CreateServerEntry()
        {
            EditServerForm form = new EditServerForm();

            form.cbbType.SelectedIndex = 0;

            if (form.ShowDialog() == DialogResult.OK)
            {
                /* Gather the info form the form into the resulting object */
                return new RemoteServer(form.edtServerName.Text, form.edtServerHost.Text,
                    Convert.ToInt32(form.edServerPort.Text), form.edtSecurityKey.Text, (ConnectionType)form.cbbType.SelectedIndex, form.cbIsEnabled.Checked);
            }
            else
                return null;
        }

        /// <summary>
        /// Edits a server entry using edit dialog.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public static IServer EditServerEntry(IServer server)
        {
            EditServerForm form = new EditServerForm();

            if (!(server is RemoteServer))
            {
                /* Unsupported server type */
                return null;
            }

            RemoteServer rServer = (RemoteServer)server;

            form.edtServerName.Text = rServer.Name;
            form.edtServerHost.Text = rServer.Host;
            form.edServerPort.Text = rServer.Port.ToString();
            form.cbIsEnabled.Checked = rServer.Enabled;
            form.edtSecurityKey.Text = rServer.SecurityKey;
            form.cbbType.SelectedIndex = (Int32)rServer.ConnectionType;

            if (rServer.SecurityKey != null && rServer.SecurityKey.Length > 0)
                form.cbSecure.Checked = true;

            if (form.ShowDialog() == DialogResult.OK)
            {
                /* Gather the info form the form into the resulting object */
                rServer.Name = form.edtServerName.Text;
                rServer.Host = form.edtServerHost.Text;
                rServer.Port = Convert.ToInt32(form.edServerPort.Text);
                rServer.Enabled = form.cbIsEnabled.Checked;
                rServer.ConnectionType = (ConnectionType)form.cbbType.SelectedIndex;

                if (form.cbSecure.Checked)
                    rServer.SecurityKey = form.edtSecurityKey.Text;
                else
                    rServer.SecurityKey = null;

                return rServer;
            }
            else
                return null;
        } 

        #endregion

        #region Private Methods

        /// <summary>
        /// Controls the UI changes.
        /// </summary>
        private void ControlUIChanges()
        {
            Boolean enable = true;

            if (edtServerName.TextLength == 0 || edtServerHost.TextLength == 0 || edServerPort.TextLength == 0)
            {
                enable = false;
            }

            if (cbSecure.Checked && edtSecurityKey.TextLength == 0)
            {
                enable = false;
            }

            try
            {
                Convert.ToUInt32(edServerPort.Text);
            }
            catch
            {
                enable = false;
            }

            btAccept.Enabled = enable;
            edtSecurityKey.Enabled = cbSecure.Checked;
        } 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EditServerForm"/> class.
        /// </summary>
        private EditServerForm()
        {
            InitializeComponent();

            /* Fill in the cbb */
            cbbType.Items.Add("HTTP (Hyper Text Transfer Protocol)");
            cbbType.Items.Add("TCP (Transmission Control Protocol)");

            /* Control */
            ControlUIChanges();
        }

        #endregion

        #region UI Events

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
        /// Handles the TextChanged event of all text controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void text_TextChanged(object sender, EventArgs e)
        {
            ControlUIChanges();
        }

        /// <summary>
        /// Handles the KeyPress event of all text control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
        private void control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                btCancel_Click(btCancel, e);
            if (e.KeyChar == '\r')
            {
                if (btAccept.Enabled)
                    btAccept_Click(btAccept, e);
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbSecure control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cbSecure_CheckedChanged(object sender, EventArgs e)
        {
            ControlUIChanges();
        }

        #endregion
    }
}