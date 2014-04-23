using System;
using System.Collections.Generic;
using System.Text;

namespace DCalcCore
{
    /// <summary>
    /// Represents the mode that will be used to evaluate the algorithm.
    /// </summary>
    public enum DispatchMode
    {
        /// <summary>
        /// Local only evaluation. All calculations will be performed only on this machine.
        /// </summary>
        LocalOnly,
        /// <summary>
        /// Remote only evaluation. All calculations will be performed only on remote machines.
        /// </summary>
        RemoteOnly,
        /// <summary>
        /// Combined evaluation. All calculations will be performed on both local and remote machines.
        /// </summary>
        Combined,
    }
}
