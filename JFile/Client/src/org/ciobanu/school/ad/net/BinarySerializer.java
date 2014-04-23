package org.ciobanu.school.ad.net;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.DataOutputStream;

public class BinarySerializer
{
	private InputStream inputStream;
	private OutputStream outputStream;
	private ByteArrayOutputStream bytePacket = new ByteArrayOutputStream();
	
	/* Simple control */
	public void setInputStream(InputStream stream)
	{
		inputStream = stream;
	}

	public void setOutputStream(OutputStream stream)
	{
		outputStream = stream;
	}
	
	public InputStream getInputStream()
	{
		return inputStream;
	}

	public OutputStream getOutputStream()
	{
		return outputStream;
	}

	/* Serialization routines */
	
	private byte[] serializePacket(DataPacket packet) throws IOException
	{
		/* Build output */
		ByteArrayOutputStream buffer = new ByteArrayOutputStream();
		DataOutputStream dos = new DataOutputStream(buffer);
		
		/* Add all parameters */
		for (int i = 0; i < packet.getParameterCount(); i++)
		{
			dos.writeInt(packet.getParameter(i).length);
			dos.write(packet.getParameter(i));
		}
		
		/* Return our new brand array */
		dos.flush();
		return buffer.toByteArray();
	}
	
	private DataPacket deserializePacket(byte[] packetBuff, int offset, int length) throws IOException
	{	
		ByteArrayInputStream bin = new ByteArrayInputStream(packetBuff, offset, length);
		DataInputStream din = new DataInputStream(bin);
		
		DataPacket packet = new DataPacket();
		
		for (;;)
		{
			if (din.available() == 0)
				break;
			
			int paramLength = din.readInt();
			byte param[] = new byte[paramLength];
			
			din.read(param);
			packet.addParameter(param);
		}
		
		return packet;
	}
	
	/* Actual Networking */
	public DataPacket receivePacket(int timeOutMillis) throws IOException
	{
		long lastCommMillis = System.currentTimeMillis();
		Integer lengthToExpect = null;
		byte[] buffPiece = new byte[1024];

		while (true)
		{
			if (inputStream.available() == 0)
			{
				/* No data in the stream! */
				if ((System.currentTimeMillis() - lastCommMillis) > timeOutMillis)
				{
					/* Timeout!! Exit with a null to notify the problem */
					return null;
				}
				
				/* Wait a bit */
				try
				{
					Thread.sleep(10);
				} catch (InterruptedException e)
				{
					return null;
				}
				
				continue;
			}
			
			/* There is data available */
			int readCount = inputStream.read(buffPiece);
			
			/* EOF? Connection died? */
			if (readCount == -1)
				return null;
			
			/* Add more data */
			bytePacket.write(buffPiece, 0, readCount);
			
			if (lengthToExpect == null && bytePacket.size() > 4)
			{
				/* We can take a decision how much to expect */
				DataInputStream di = new DataInputStream(new ByteArrayInputStream(bytePacket.toByteArray()));
				lengthToExpect = di.readInt();
			}
			
			/* Check for the end */
			if (lengthToExpect != null && (lengthToExpect + 4) <= bytePacket.size())
			{
				byte[] allBuffer = bytePacket.toByteArray();
				DataPacket cPacket = deserializePacket(allBuffer, 4, lengthToExpect);
				
				bytePacket.reset();
				
				/* Re-attach the remaining stuff */
				if (allBuffer.length > (lengthToExpect + 4))
					bytePacket.write(allBuffer, lengthToExpect + 4, (allBuffer.length - lengthToExpect - 4));
				
				lengthToExpect = null;
				
				return cPacket;
			}
		}
	}

	public void sendPacket(DataPacket packet) throws IOException
	{
		ByteArrayOutputStream bout = new ByteArrayOutputStream();
		DataOutputStream dout = new DataOutputStream(bout);
		
		byte[] pck = serializePacket(packet);
		
		dout.writeInt(pck.length);
		dout.write(pck);
		dout.flush();
		
		outputStream.write(bout.toByteArray());
	}
}
