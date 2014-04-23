using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using DynAlgorithm.UI;

namespace DynAlgorithm.Algorithm
{
    /// <summary>
    /// Dynamic algorithm for testing purposes only.
    /// </summary>
    public sealed class DynAlgorithmProvider : IAlgorithmProvider
    {
        #region Private Fields

        /* Data */
        private String m_AlgorithmName = "Dynamic Algorithm Testing";
        private String m_Developer = "Ciobanu Alexandru";
        private String m_Description = "Proveis a simple test algorithm built at run-time. An user can imput code to be evaluated in a distributed manner for test purposes.";
        private Int32 m_VersionMajor = 1;
        private Int32 m_VersionMinor = 0;

        /* Settings */
        private Int32 m_StartInterval = 0;
        private Int32 m_EndInterval = 10000;
        private String m_ReturnType = "Int32";
        private String m_Body = Environment.NewLine + Environment.NewLine + "    return (x*x);";

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
            return new DynAlgorithm(m_StartInterval, m_EndInterval, m_ReturnType, m_Body);
        }

        /// <summary>
        /// Configures the provider.
        /// </summary>
        public void ConfigureProvider()
        {
            ConfigurationForm.Configure(ref m_StartInterval, ref m_EndInterval,
                ref m_ReturnType, ref m_Body);
        }

        /// <summary>
        /// Shows the resulting GUI/CUI.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        public void ShowResult(IAlgorithm algorithm)
        {
            if (algorithm == null)
                throw new ArgumentNullException("algorithm");

            ResultsForm.ShowResults((DynAlgorithm)algorithm);
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> GetSettings()
        {
            Dictionary<String, String> settings = new Dictionary<String, String>();

            settings.Add("StartInterval", m_StartInterval.ToString());
            settings.Add("EndInterval", m_EndInterval.ToString());
            settings.Add("ReturnType", m_ReturnType);
            settings.Add("Body", m_Body);

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
                Int32 vStartInterval = Convert.ToInt32(settings["m_StartInterval"]);
                Int32 vEndInterval = Convert.ToInt32(settings["m_EndInterval"]);
                String vReturnType = settings["m_ReturnType"];
                String vBody = settings["m_Body"];

                if (vStartInterval < vEndInterval) { m_StartInterval = vStartInterval; m_EndInterval = vEndInterval; }
                if (vReturnType != null && vReturnType.Length > 0) m_ReturnType = vReturnType;
                if (vBody != null && vBody.Length > 0) m_Body = vBody;
            }
            catch
            {
            }
        }

        #endregion
    }
}
