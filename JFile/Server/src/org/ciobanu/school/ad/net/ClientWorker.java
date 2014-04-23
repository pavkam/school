package org.ciobanu.school.ad.net;


public interface ClientWorker
{
	public void addListener(ClientWorkerListener listener);
	public void removeListener(ClientWorkerListener listener);
	
	public String getClientId();
	
	public void startWorking();
	public void stopWorking();
}
