using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Utilities;

namespace DCalcCore.Assemblers
{
    /// <summary>
    /// Provides a common interface for all compiled scripts.
    /// </summary>
    public interface ICompiledScript : IDisposable
    {
        /// <summary>
        /// Executes the specified script.
        /// </summary>
        /// <param name="set">The set to use as input.</param>
        /// <returns>Output result set</returns>
        ScalarSet Execute(ScalarSet set);
    }
}
