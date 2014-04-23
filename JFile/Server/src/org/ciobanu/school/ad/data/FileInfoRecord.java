package org.ciobanu.school.ad.data;

public class FileInfoRecord
{
	private String name;
	private long size;
	
	public FileInfoRecord(String name, long size)
	{
		this.name = name;
		this.size = size;
	}
	
	public String getName()
	{
		return name;
	}
	
	public long getSize()
	{
		return size;
	}
}
