package org.ciobanu.school.ad.net.server;

import java.io.IOException;
import java.net.ServerSocket;
import org.ciobanu.school.ad.net.ClientWorkerListener;

public class ServerAcceptor implements Runnable
{
	private int serverPort;
	private Thread runnerThread = null;
	private ServerSocket serverSocket;
	private boolean mustLive;
	private ClientWorkerListener listener;

	public ServerAcceptor(int port, ClientWorkerListener listener)
			throws IOException
	{
		this.serverPort = port;
		this.listener = listener;

		/* Initialize the server! */
		serverSocket = new ServerSocket(serverPort);
	}

	public synchronized void start() throws ServerAlreadyRunning
	{
		/* Check for already present server */
		if (runnerThread != null)
			throw new ServerAlreadyRunning();

		/* Yahoo baby! Start the server runner thread */
		mustLive = true;
		runnerThread = new Thread(this);
		runnerThread.start();
	}

	public synchronized void stop() throws ServerNotRunning
	{
		/* Check for already present server */
		if (runnerThread == null)
			throw new ServerNotRunning();

		/* Yup! Let's shut it down pretty. */
		mustLive = false;
		
		/* Stop the listener */
		try
		{
			serverSocket.close();
		} catch (IOException e1)
		{
		}

		try
		{
			runnerThread.join();
		} catch (InterruptedException e)
		{
			/* Nothing to see here! Please move along sir ... */
		}

		/* It's down */
		runnerThread = null;
		mustLive = true;
	}

	public void run()
	{
		while (mustLive)
		{
			try
			{
				/* Listen for incoming connections */
				listener.clientAccepted(serverSocket.accept());
			} catch (IOException e)
			{
			}
		}
	}

}
