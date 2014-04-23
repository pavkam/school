using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;

namespace DCalc.Communication
{
    /// <summary>
    /// Provides a common interface for all workspaces to follow.
    /// </summary>
    public interface IWorkSpace : ICloneable
    {
        /// <summary>
        /// Gets all the registered servers.
        /// </summary>
        /// <value>The servers.</value>
        IEnumerable<IServer> Servers { get; }

        /// <summary>
        /// Adds a new server.
        /// </summary>
        /// <param name="server">The server.</param>
        void AddServer(IServer server);

        /// <summary>
        /// Removes a server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        IServer RemoveServer(IServer server);

        /// <summary>
        /// Clears all the servers.
        /// </summary>
        void ClearServers();

        /// <summary>
        /// Gets the count of servers.
        /// </summary>
        /// <value>The count of servers.</value>
        Int32 CountOfServers { get; }

        /// <summary>
        /// Gets or sets the algorithm provider.
        /// </summary>
        /// <value>The provider.</value>
        IAlgorithmProvider Provider { get; set; }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns></returns>
        Boolean Load();

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        Boolean Save();

        /// <summary>
        /// Gets or sets the local balancer.
        /// </summary>
        /// <value>The local balancer.</value>
        Type LocalBalancer { get; set; }

        /// <summary>
        /// Gets or sets the remote balancer.
        /// </summary>
        /// <value>The remote balancer.</value>
        Type RemoteBalancer { get; set; }

        /// <summary>
        /// Gets or sets the size of the queue.
        /// </summary>
        /// <value>The size of the queue.</value>
        Int32 QueueSize { get; set; }

        /// <summary>
        /// Gets or sets the number of threads.
        /// </summary>
        /// <value>The number of threads.</value>
        Int32 NumberOfThreads { get; set; }
    }
}
