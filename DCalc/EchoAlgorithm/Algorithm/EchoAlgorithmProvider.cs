using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using EchoAlgorithm.UI;

namespace EchoAlgorithm.Algorithm
{
    /// <summary>
    /// Simple Echo-like algorithm for testing purposes only.
    /// </summary>
    public sealed class EchoAlgorithmProvider : IAlgorithmProvider
    {
        #region Private Fields

        /* Data */
        private String m_AlgorithmName = "Simple Echo";
        private String m_Developer = "Ciobanu Alexandru";
        private String m_Description = "Provides a simple ping-like algorithm. It only replies back with the data it received. It was designed for testing only!";
        private Int32 m_VersionMajor = 1;
        private Int32 m_VersionMinor = 2;

        /* Settings */
        private Int32 m_DelayCycles = 100000;
        private Int32 m_SetCount = 100000;

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
            return new EchoAlgorithm(m_DelayCycles, m_SetCount);
        }

        /// <summary>
        /// Configures the provider.
        /// </summary>
        public void ConfigureProvider()
        {
            ConfigurationForm.ConfigureAlgorithm(ref m_DelayCycles, ref m_SetCount);
        }

        /// <summary>
        /// Shows the resulting GUI/CUI.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        public void ShowResult(IAlgorithm algorithm)
        {
            System.Windows.Forms.MessageBox.Show("No results here! It's just a test algorithm!", "Hello", 
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> GetSettings()
        {
            Dictionary<String, String> settings = new Dictionary<String, String>();
            settings.Add("DelayCycles", m_DelayCycles.ToString());
            settings.Add("SetCount", m_SetCount.ToString());

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

            if (settings.ContainsKey("DelayCycles"))
            {
                try
                {
                    Int32 dc = Convert.ToInt32(settings["DelayCycles"]);
                    Int32 sc = Convert.ToInt32(settings["SetCount"]);

                    if (dc > 0)
                        m_DelayCycles = dc;

                    if (sc > 0)
                        m_DelayCycles = sc;
                }
                catch
                {
                }
            }
        }

        #endregion
    }
}
