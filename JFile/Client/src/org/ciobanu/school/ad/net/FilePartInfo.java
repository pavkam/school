package org.ciobanu.school.ad.net;

public class FilePartInfo {
	
	private long startAt;
	private long length;
	private ClientThread worker;

	public FilePartInfo(long startAt, long length)
	{
		this.length = length;
		this.startAt = startAt;
	}
	
	public long getStartAt()
	{
		return startAt;
	}
	
	public long getLength()
	{
		return length;
	}
	
	public void setWorker(ClientThread worker)
	{
		this.worker = worker;
	}
	
	public ClientThread getWorker()
	{
		return worker;
	}
}
