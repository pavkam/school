using System;
using System.Collections.Generic;
using System.Text;

namespace DCalc.Communication
{
    /// <summary>
    /// Provides a common interface all UI server object must implement.
    /// </summary>
    public interface IServer : ICloneable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        String Name { get; set; }

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <value>The signature.</value>
        String Signature { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IServer"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        Boolean Enabled { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        ServerStatus Status { get; set; }
    }
}
