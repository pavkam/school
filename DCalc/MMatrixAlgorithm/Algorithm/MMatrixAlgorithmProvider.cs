using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using MMatrixAlgorithm.UI;

namespace MMatrixAlgorithm.Algorithm
{
    /// <summary>
    /// Matrix multiplication algorithm for testing purposes only.
    /// </summary>
    public sealed class MMatrixAlgorithmProvider : IAlgorithmProvider
    {
        #region Private Fields

        /* Data */
        private String m_AlgorithmName = "Matrix Multiplication";
        private String m_Developer = "Ciobanu Alexandru";
        private String m_Description = "Provides an example matrix multiplication algorithm. Matrixes to be multiplied are generated at run-time.";
        private Int32 m_VersionMajor = 1;
        private Int32 m_VersionMinor = 5;

        /* Settings */
        private Int32 m_Matrix1X = 200;
        private Int32 m_Matrix1Y = 400;
        private Int32 m_Matrix2X = 300;
        private Int32 m_Matrix2Y = 200;

        #endregion

        #region IAlgorithmProvider Members

        /// <summary>
        /// Gets the name of the algorithm.
        /// </summary>
        /// <value>The name.</value>
        public String Name
        {
            get { return m_AlgorithmName; }
        }

        /// <summary>
        /// Gets the developer of the algorithm.
        /// </summary>
        /// <value>The developer.</value>
        public String Developer
        {
            get { return m_Developer; }
        }

        /// <summary>
        /// Gets the description of the algorithm.
        /// </summary>
        /// <value>The description.</value>
        public String Description
        {
            get { return m_Description; }
        }

        /// <summary>
        /// Gets the version major number.
        /// </summary>
        /// <value>The version major number.</value>
        public Int32 VersionMajor
        {
            get { return m_VersionMajor; }
        }

        /// <summary>
        /// Gets the version minor number.
        /// </summary>
        /// <value>The version minor number.</value>
        public Int32 VersionMinor
        {
            get { return m_VersionMinor; }
        }

        /// <summary>
        /// Gets a new instance of the algorithm.
        /// </summary>
        /// <returns></returns>
        public IAlgorithm GetAlgorithmInstance()
        {
            return new MMatrixAlgorithm(m_Matrix1X, m_Matrix1Y, m_Matrix2X, m_Matrix2Y);
        }

        /// <summary>
        /// Configures the provider.
        /// </summary>
        public void ConfigureProvider()
        {
            ConfigurationForm.Configure(ref m_Matrix1X, ref m_Matrix1Y, 
                ref m_Matrix2X, ref m_Matrix2Y);
        }

        /// <summary>
        /// Shows the resulting GUI/CUI.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        public void ShowResult(IAlgorithm algorithm)
        {
            if (algorithm == null)
                throw new ArgumentNullException("algorithm");

            ResultsForm.ShowResults((MMatrixAlgorithm)algorithm);
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> GetSettings()
        {
            Dictionary<String, String> settings = new Dictionary<String, String>();
            settings.Add("Matrix1X", m_Matrix1X.ToString());
            settings.Add("Matrix1Y", m_Matrix1Y.ToString());
            settings.Add("Matrix2X", m_Matrix2X.ToString());
            settings.Add("Matrix2Y", m_Matrix2Y.ToString());

            return settings;
        }

        /// <summary>
        /// Sets the settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void SetSettings(Dictionary<String, String> settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            try
            {
                Int32 m1x = Convert.ToInt32(settings["Matrix1X"]);
                Int32 m1y = Convert.ToInt32(settings["Matrix1Y"]);
                Int32 m2x = Convert.ToInt32(settings["Matrix2X"]);
                Int32 m2y = Convert.ToInt32(settings["Matrix2Y"]);

                if (m1x > 0) m_Matrix1X = m1x;
                if (m1y > 0) m_Matrix1Y = m1y;
                if (m2x > 0) m_Matrix2X = m2x;
                if (m2y > 0) m_Matrix2Y = m2y;

                if (m1x != m2y)
                {
                    m2y = m1x;
                }
            }
            catch
            {
            }
        }

        #endregion
    }
}
