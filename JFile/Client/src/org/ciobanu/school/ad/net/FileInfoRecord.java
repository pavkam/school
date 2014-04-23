package org.ciobanu.school.ad.net;

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
	
	@Override
	public String toString()
	{
		if (size < 1024)
			return name + "   [" + (size) + " bytes]";
		else
			return name + "   [" + (size/1024) + " Kb]";
	}
}
