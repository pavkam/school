using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.Algorithm
{
    /// <summary>
    /// Provides a common interface for all algorithm providers to implement.
    /// </summary>
    public interface IAlgorithmProvider
    {
        /// <summary>
        /// Gets the name of the algorithm.
        /// </summary>
        /// <value>The name.</value>
        String Name { get; }

        /// <summary>
        /// Gets the developer of the algorithm.
        /// </summary>
        /// <value>The developer.</value>
        String Developer { get; }

        /// <summary>
        /// Gets the description of the algorithm.
        /// </summary>
        /// <value>The description.</value>
        String Description { get; }

        /// <summary>
        /// Gets the version major number.
        /// </summary>
        /// <value>The version major number.</value>
        Int32 VersionMajor { get; }

        /// <summary>
        /// Gets the version minor number.
        /// </summary>
        /// <value>The version minor number.</value>
        Int32 VersionMinor { get; }

        /// <summary>
        /// Gets a new instance of the algorithm.
        /// </summary>
        /// <returns></returns>
        IAlgorithm GetAlgorithmInstance();

        /// <summary>
        /// Configures the provider.
        /// </summary>
        void ConfigureProvider();

        /// <summary>
        /// Shows the resulting GUI/CUI.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        void ShowResult(IAlgorithm algorithm);

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns></returns>
        Dictionary<String, String> GetSettings();

        /// <summary>
        /// Sets the settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void SetSettings(Dictionary<String, String> settings);
    }
}
