using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Utilities;

namespace DCalcCore.Remoting
{
    /// <summary>
    /// Provides a common interface for all gate servers to implement.
    /// </summary>
    public interface IRemoteGateServer : IDisposable
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns></returns>
        Boolean Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <returns></returns>
        Boolean Stop();

        /// <summary>
        /// Occurs when a client has connected.
        /// </summary>
        event ConnectionEventHandler ClientConnected;

        /// <summary>
        /// Occurs when a client has disconnected.
        /// </summary>
        event ConnectionEventHandler ClientDisconnected;

        /// <summary>
        /// Occurs when an algorithm was registered.
        /// </summary>
        event ConnectionEventHandler AlgorithmRegistered;

        /// <summary>
        /// Occurs when an algorithm was removed.
        /// </summary>
        event ConnectionEventHandler AlgorithmRemoved;

        /// <summary>
        /// Occurs when a client uploaded input sets.
        /// </summary>
        event SetUpdateEventHandler ClientUploadedInputSets;

        /// <summary>
        /// Occurs when a client downloaded output sets.
        /// </summary>
        event SetUpdateEventHandler ClientDownloadedOutputSets;
    }
}
