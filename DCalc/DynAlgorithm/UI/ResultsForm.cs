using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DynAlgorithm.Algorithm;

namespace DynAlgorithm.UI
{
    public partial class ResultsForm : Form
    {
        private Algorithm.DynAlgorithm m_Algorithm;

        public static void ShowResults(Algorithm.DynAlgorithm algorithm)
        {
            ResultsForm form = new ResultsForm();
            form.m_Algorithm = algorithm;
            form.edtResults.Text = algorithm.GetResults();

            form.ShowDialog();
        }

        private ResultsForm()
        {
            InitializeComponent();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}