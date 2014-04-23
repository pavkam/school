using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;

namespace DCalc.Algorithms
{
    /// <summary>
    /// Provides a common interface for all plugin loaders.
    /// </summary>
    public interface IAlgorithmCollection
    {
        /// <summary>
        /// Gets all loaded providers.
        /// </summary>
        /// <value>The providers.</value>
        IEnumerable<IAlgorithmProvider> Providers { get; }

        /// <summary>
        /// Gets the count of loaded providers.
        /// </summary>
        /// <value>The count.</value>
        Int32 Count { get; }

        /// <summary>
        /// Loads all.
        /// </summary>
        void LoadAll();

        /// <summary>
        /// Unloads all.
        /// </summary>
        void UnLoadAll();
    }
}
