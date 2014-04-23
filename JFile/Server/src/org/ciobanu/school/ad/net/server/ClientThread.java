package org.ciobanu.school.ad.net.server;

import java.io.IOException;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;
import org.ciobanu.school.ad.net.ClientWorker;
import org.ciobanu.school.ad.net.ClientWorkerListener;
import org.ciobanu.school.ad.net.ControlPacket;
import org.ciobanu.school.ad.net.ControlPacketSerializer;
import org.ciobanu.school.ad.net.PacketController;

public class ClientThread implements ClientWorker, Runnable
{
	private Socket connectionSocket;
	private List<ClientWorkerListener> listeners;
	private int timeOut;
	private boolean keepAlive;
	private ControlPacketSerializer serializer;
	private PacketController controller;
	private Thread runner;

	public ClientThread(Socket clientSocket,
			ControlPacketSerializer packetSerializer,
			PacketController packetController,
			int timeOutMillis) throws IOException
	{
		connectionSocket = clientSocket;
		listeners = new ArrayList<ClientWorkerListener>();
		timeOut = timeOutMillis;
		
		controller = packetController;

		/* Initialize serializer */
		serializer = packetSerializer;

		serializer.setInputStream(connectionSocket.getInputStream());
		serializer.setOutputStream(connectionSocket.getOutputStream());
	}

	/* Listener management */
	public void addListener(ClientWorkerListener listener)
	{
		if (!listeners.contains(listener))
			listeners.add(listener);
	}

	public void removeListener(ClientWorkerListener listener)
	{
		if (listeners.contains(listener))
			listeners.remove(listener);
	}

	private void signalClientConnected()
	{
		/* Notify all listeners and protect against external exceptions */
		try
		{
			for (int i = 0; i < listeners.size(); i++)
				listeners.get(i).clientConnected(this);
		} catch (Exception e)
		{

		}
	}

	private void signalClientDisconnected()
	{
		/* Notify all listeners and protect against external exceptions */
		try
		{
			for (int i = 0; i < listeners.size(); i++)
				listeners.get(i).clientDisconnected(this);
		} catch (Exception e)
		{
		}
	}

	/* Other */
	public String getClientId()
	{
		return connectionSocket.getInetAddress() + ":"
				+ connectionSocket.getPort();
	}

	public void startWorking()
	{
		if (runner != null)
			return; /* This one is already started */

		keepAlive = true;
		runner = new Thread(this);
		runner.start();
	}

	public void stopWorking()
	{
		if (runner == null)
			return; /* Stopped already */

		/* Mark worker as dead! */
		keepAlive = false;

		/* Shutdown socket's streams */
		try
		{
			connectionSocket.shutdownInput();
		} catch (IOException e)
		{
		}

		try
		{
			connectionSocket.shutdownOutput();
		} catch (IOException e)
		{
		}
	}

	/* Internal runner */
	public void run()
	{
		/* Main worker thread */
		signalClientConnected();

		while (keepAlive)
		{
			try
			{
				/* Receive next packet */
				ControlPacket requestPacket = serializer.receivePacket(timeOut);

				if (requestPacket == null)
				{
					/* Wrong packet format or timeout! */
					break;
				}

				/* Good packet! Let's execute the request and return a result */
				ControlPacket responsePacket = controller
						.excuteRequest(requestPacket);

				if (responsePacket == null)
				{
					/* Something went wrong during the processing! */
					break;
				}
								
				serializer.sendPacket(responsePacket);
				
			} catch (Exception e)
			{
				/* Bad things happened! */
				break;
			}
		}

		/* Disconnect from the server */
		try
		{
			connectionSocket.close();
		} catch (Exception e)
		{
			/* Ignore exceptions */
		}

		runner = null;
		signalClientDisconnected();
	}

}
