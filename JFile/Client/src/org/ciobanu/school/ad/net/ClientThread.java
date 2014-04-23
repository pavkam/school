package org.ciobanu.school.ad.net;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.Socket;

public class ClientThread implements Runnable {
	private Socket connectionSocket;
	private String host;
	private int port;
	private boolean keepAlive;
	private Thread runner;
	private String mutex = "0";
	private DataPacket packetSendQ;
	private DataPacket packetRecvQ;

	public ClientThread(Socket clientSocket, String host, int port) throws IOException {
		this.connectionSocket = clientSocket;
		this.host = host;
		this.port = port;
	}

	public void startWorking() {
		if (runner != null)
			return; /* This one is already started */

		keepAlive = true;
		runner = new Thread(this);
		runner.start();
	}

	public void stopWorking() {
		if (runner == null)
			return; /* Stopped already */

		/* Mark worker as dead! */
		keepAlive = false;

		/* Shutdown socket's streams */
		try {
			connectionSocket.shutdownInput();
		} catch (IOException e) {
		}

		try {
			connectionSocket.shutdownOutput();
		} catch (IOException e) {
		}
	}

	public boolean isAlive()
	{
		return keepAlive;
	}
	
	public void queuePacket(DataPacket packet) {
		synchronized (mutex) {
			packetSendQ = packet;
		}
	}

	public DataPacket unqueuePacket() {
		synchronized (mutex) {
			DataPacket temp = packetRecvQ;
			packetRecvQ = null;
			
			return temp;
		}
	}

	public DataPacket sendAndReceivePacket(DataPacket packet)
	{
		queuePacket(packet);
		
		try {
			while (isAlive())
			{
				DataPacket pck = unqueuePacket();
				
				if (pck != null)
					return pck;
				
				Thread.sleep(10);
			}
		} catch (Exception e) {
			
		}
		
		return null;
	}
	
	/* Internal runner */
	public void run() {

		BinarySerializer serializer;
		try {
			serializer = new BinarySerializer();
			serializer.setInputStream(connectionSocket.getInputStream());
			serializer.setOutputStream(connectionSocket.getOutputStream());
		} catch (IOException e1) {
			keepAlive = false;
			return;
		}
		
		while (keepAlive) {
			try {
				synchronized (mutex) {

					/* Send */
					if (packetSendQ != null) {
						
						serializer.sendPacket(packetSendQ);

						/* Receive */
						packetRecvQ = serializer.receivePacket(5000);

						if (packetRecvQ == null)
							break;

						packetSendQ = null;
					}
				}

				Thread.sleep(10);
			} catch (Exception e) {
				break;
			}
		}

		try {
			connectionSocket.shutdownInput();
		} catch (IOException e) {
		}

		try {
			connectionSocket.shutdownOutput();
		} catch (IOException e) {
		}

		try {
			connectionSocket.close();
		} catch (IOException e) {
		}
		
		keepAlive = false;
	}

	
	public ClientThread spawn()
	{
		try {
			Socket newChild = new Socket();
			newChild.connect(new InetSocketAddress(host, port));
			
			ClientThread ttx = new ClientThread(newChild, host, port);
			ttx.startWorking();
			
			return ttx;
		} catch (IOException e) {
		}
		
		return null;
	}
}
