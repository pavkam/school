using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using DCalcCore.Assemblers;
using DCalcCore.Runners;
using DCalcCore.Utilities;

namespace DCalcCore.Remoting
{
    /// <summary>
    /// Provides common interface for all remote gates to implement.
    /// </summary>
    public interface IRemoteGateClient : IDisposable
    {
        /// <summary>
        /// Queues the opening of the connection to the server.
        /// </summary>
        void AsyncOpen();

        /// <summary>
        /// Queues the closing of the connection to the server.
        /// </summary>
        void AsyncClose();

        /// <summary>
        /// Queues the registration of a script remotely.
        /// </summary>
        /// <param name="script">The script.</param>
        void AsyncRegisterScript(IScript script);

        /// <summary>
        /// Queues the cancelling of a script remotely.
        /// </summary>
        /// <param name="script">The script.</param>
        void AsyncCancelScript(IScript script);

        /// <summary>
        /// Queues some work to be done remotely.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="set">The set.</param>
        Boolean AsyncQueueWork(IScript script, ScalarSet set);

        /// <summary>
        /// Occurs when set completed.
        /// </summary>
        event QueueEventHandler SetCompleted;

        /// <summary>
        /// Occurs when gate connected to server.
        /// </summary>
        event EventHandler ConnectedToServer;

        /// <summary>
        /// Occurs when gate disconnected from server.
        /// </summary>
        event EventHandler DisconnectedFromServer;
    }
}
