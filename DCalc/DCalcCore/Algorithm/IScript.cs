using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore.Algorithm
{
    /// <summary>
    /// Provides a common interface for all scripts.
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        String MethodName { get; }

        /// <summary>
        /// Gets the method body.
        /// </summary>
        /// <value>The method body.</value>
        String MethodBody { get; }
    }
}
