using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;

namespace DCalcCore.Assemblers
{
    /// <summary>
    /// Provides a common interface for all code assemblers.
    /// </summary>
    public interface IScriptAssembler
    {
        /// <summary>
        /// Assembles the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        ICompiledScript Assemble(IScript script);
    }
}
