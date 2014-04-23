using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DynAlgorithm.UI
{
    public partial class ConfigurationForm : Form
    {
        public static void Configure(ref Int32 intervalStart, ref Int32 intervalEnd, ref String returnType, ref String actualCode)
        {
            ConfigurationForm form = new ConfigurationForm();

            form.edtEndInt.Text = intervalEnd.ToString();
            form.edtStartInt.Text = intervalStart.ToString();
            form.edtCode.Text = actualCode;

            if (returnType.Equals("Int32"))
                form.cbbReturnType.SelectedIndex = 0;
            else if (returnType.Equals("Double"))
                form.cbbReturnType.SelectedIndex = 1;
            else
                form.cbbReturnType.SelectedIndex = 2;

            form.ControlUIChanges();

            if (form.ShowDialog() == DialogResult.OK)
            {
                intervalEnd = Convert.ToInt32(form.edtEndInt.Text);
                intervalStart = Convert.ToInt32(form.edtStartInt.Text);
                actualCode = form.edtCode.Text;
                returnType = form.cbbReturnType.Text;
            }
        }

        private void ControlUIChanges()
        {
            Boolean acceptEnabled = true;

            if (edtEndInt.TextLength == 0)
                acceptEnabled = false;

            if (edtStartInt.TextLength == 0)
                acceptEnabled = false;

            if (edtCode.TextLength == 0)
                acceptEnabled = false;

            try
            {
                if (Convert.ToInt32(edtEndInt.Text) <= Convert.ToInt32(edtStartInt.Text))
                    acceptEnabled = false;
            }
            catch
            {
            }

            btAccept.Enabled = acceptEnabled;
        }

        private ConfigurationForm()
        {
            InitializeComponent();
        }

        private void text_TextChanged(object sender, EventArgs e)
        {
            ControlUIChanges();
        }

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

        private void btAccept_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}