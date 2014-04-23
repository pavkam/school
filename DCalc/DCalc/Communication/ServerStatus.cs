using System;
using System.Collections.Generic;
using System.Text;

namespace DCalc.Communication
{
    /// <summary>
    /// Defines the status a server can be in.
    /// </summary>
    public enum ServerStatus
    {
        /// <summary>
        /// Server is disabled.
        /// </summary>
        Disabled,
        /// <summary>
        /// Server is unreacheble.
        /// </summary>
        Down,
        /// <summary>
        /// Server is running.
        /// </summary>
        Running,
    }
}
