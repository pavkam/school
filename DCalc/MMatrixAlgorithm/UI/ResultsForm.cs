using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MMatrixAlgorithm.Algorithm;

namespace MMatrixAlgorithm.UI
{
    public partial class ResultsForm : Form
    {
        private Algorithm.MMatrixAlgorithm m_Algorithm;

        public static void ShowResults(Algorithm.MMatrixAlgorithm algorithm)
        {
            ResultsForm form = new ResultsForm();
            form.m_Algorithm = algorithm;

            form.ShowDialog();
        }

        private Boolean SaveMatrix(String fileName, Double[,] matrix)
        {
            try
            {
                StreamWriter sw = File.CreateText(fileName);

                for (Int32 y = 0; y < matrix.GetLength(1); y++)
                {
                    for (Int32 x = 0; x < matrix.GetLength(0); x++)
                    {
                        sw.Write(matrix[x, y].ToString(System.Globalization.CultureInfo.InvariantCulture));
                        sw.Write("  ");
                    }

                    sw.Write(Environment.NewLine);
                }

                sw.Close();
            }
            catch
            {
                MessageBox.Show("Error while saving the matrix to a file! Please check the path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private ResultsForm()
        {
            InitializeComponent();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btSaveMatrixA_Click(object sender, EventArgs e)
        {
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                SaveMatrix(saveDialog.FileName, m_Algorithm.MatrixA);
            }
        }

        private void btSaveMatrixB_Click(object sender, EventArgs e)
        {
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                SaveMatrix(saveDialog.FileName, m_Algorithm.MatrixB);
            }
        }

        private void btSaveMatrixR_Click(object sender, EventArgs e)
        {
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                SaveMatrix(saveDialog.FileName, m_Algorithm.ResultingMatrix);
            }
        }
    }
}