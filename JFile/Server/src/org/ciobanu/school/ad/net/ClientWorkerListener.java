package org.ciobanu.school.ad.net;

import java.net.Socket;

public interface ClientWorkerListener
{
	public ClientWorker clientAccepted(Socket clientSocket);
	
	public void clientDisconnected(ClientWorker worker);
	public void clientConnected(ClientWorker worker);
}
