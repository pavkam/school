package org.ciobanu.school.ad.net;

import java.util.ArrayList;
import java.util.List;

public class DataPacket
{
	private List<byte[]> parameters = new ArrayList<byte[]>();

	public void addParameter(byte[] param)
	{
		parameters.add(param);
	}

	public void modifyParameter(int i, byte[] param)
	{
		parameters.set(i, param);
	}

	public void removeParameter(int i)
	{
		parameters.remove(i);
	}

	public byte[] getParameter(int i)
	{
		return parameters.get(i);
	}

	public int getParameterCount()
	{
		return parameters.size();
	}

	public byte[] getByteArrayParam(int i)
	{
		return parameters.get(i);
	}

	
	public void addParameter(String param)
	{
		addParameter(param.getBytes());
	}

	public void addParameter(int param)
	{
		addParameter(String.valueOf(param));
	}

	public void modifyParameter(int i, String param)
	{
		modifyParameter(i, param.getBytes());
	}

	public void modifyParameter(int i, int param)
	{
		modifyParameter(i, String.valueOf(param));
	}

	public void addParameter(long param)
	{
		addParameter(String.valueOf(param));
	}

	public void modifyParameter(int i, long param)
	{
		modifyParameter(i, String.valueOf(param));
	}

	public void addParameter(boolean param)
	{
		addParameter(param ? 1 : 0);
	}

	public void modifyParameter(int i, boolean param)
	{
		modifyParameter(i, param ? 1 : 0);
	}
}
