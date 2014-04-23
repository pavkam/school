using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MMatrixAlgorithm.UI
{
    public partial class ConfigurationForm : Form
    {
        public static void Configure(ref Int32 matrix1X, ref Int32 matrix1Y, 
            ref Int32 matrix2X, ref Int32 matrix2Y)
        {
            ConfigurationForm form = new ConfigurationForm();

            form.edtAColumns.Text = matrix1X.ToString();
            form.edtARows.Text = matrix1Y.ToString();
            form.edtBColumns.Text = matrix2X.ToString();
            form.edtBRows.Text = matrix2Y.ToString();
            form.ControlUIChanges();

            if (form.ShowDialog() == DialogResult.OK)
            {
                matrix1X = Convert.ToInt32(form.edtAColumns.Text);
                matrix1Y = Convert.ToInt32(form.edtARows.Text);
                matrix2X = Convert.ToInt32(form.edtBColumns.Text);
                matrix2Y = Convert.ToInt32(form.edtBRows.Text);
            }
        }

        private void ControlUIChanges()
        {
            Boolean acceptEnabled = true;

            if (edtAColumns.TextLength == 0)
                acceptEnabled = false;

            if (edtARows.TextLength == 0)
                acceptEnabled = false;

            if (edtBColumns.TextLength == 0)
                acceptEnabled = false;

            if (edtBRows.TextLength == 0)
                acceptEnabled = false;

            try
            {
                if (Convert.ToInt32(edtAColumns.Text) < 1)
                    acceptEnabled = false;

                if (Convert.ToInt32(edtARows.Text) < 1)
                    acceptEnabled = false;

                if (Convert.ToInt32(edtBColumns.Text) < 1)
                    acceptEnabled = false;

                if (Convert.ToInt32(edtBRows.Text) < 1)
                    acceptEnabled = false;

                if (Convert.ToInt32(edtAColumns.Text) != Convert.ToInt32(edtBRows.Text))
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