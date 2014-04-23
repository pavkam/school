using System;
using System.Collections.Generic;
using System.Text;

namespace DCalc.Communication
{
    /// <summary>
    /// Connection protocol mode.
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// Http based transfer.
        /// </summary>
        Http = 0,
        /// <summary>
        /// TCP based transfer.
        /// </summary>
        Tcp = 1,
    }
}
