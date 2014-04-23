package org.ciobanu.school.ad.net.server;

import org.ciobanu.school.ad.data.FileInfoRecord;
import org.ciobanu.school.ad.data.FileSystemInterface;
import org.ciobanu.school.ad.net.ControlPacket;
import org.ciobanu.school.ad.net.EditableControlPacket;
import org.ciobanu.school.ad.net.PacketController;

public class JFtpPacketController implements PacketController
{
	private FileSystemInterface fsInterface;

	private String getCommand(ControlPacket packet)
	{
		return new String(packet.getParameter(0));
	}
	
	private String getRelativePath(ControlPacket packet)
	{
		return new String(packet.getParameter(1));
	}
	
	private String getNewRelativePath(ControlPacket packet)
	{
		return new String(packet.getParameter(2));
	}
	
	private long getFileSize(ControlPacket packet)
	{
		return Long.parseLong(new String(packet.getParameter(2)));
	}
	
	private long getReadLength(ControlPacket packet)
	{
		return Long.parseLong(new String(packet.getParameter(3)));
	}
	
	private long getReadStartAt(ControlPacket packet)
	{
		return Long.parseLong(new String(packet.getParameter(2)));
	}
	
	private byte[] getFileBuffer(ControlPacket packet)
	{
		return packet.getParameter(3);
	}
	
	public JFtpPacketController(FileSystemInterface intf)
	{
		fsInterface = intf;
	}
	
	public EditableControlPacket excuteRequest(ControlPacket packet)
	{
		String command = getCommand(packet);
		
		if (command.equals("HELLO"))
		{
			/* Hello command! Reply back */
			DataPacket response = new DataPacket();
			response.addParameter("HELLO");
			
			return response;
		}
		if (command.equals("LIST"))
		{
			/* List files */
			FileInfoRecord[] files = fsInterface.listFiles(getRelativePath(packet));
			
			DataPacket response = new DataPacket();
			int count;
			
			if (files == null || files.length == 0)
				count = 0;
			else
				count = files.length;
			
			response.addParameter(count);
			
			for (int i = 0; i < count; i++)
			{
				response.addParameter(files[i].getName());
				response.addParameter(files[i].getSize());
			}
			
			return response;
		}
		if (command.equals("DELETE"))
		{
			DataPacket response = new DataPacket();
			response.addParameter(fsInterface.deleteFile(getRelativePath(packet)));
			
			return response;
		}
		if (command.equals("NEW"))
		{
			DataPacket response = new DataPacket();
			response.addParameter(fsInterface.createEmptyFile(getRelativePath(packet), getFileSize(packet)));
			
			return response;
		}
		if (command.equals("RENAME"))
		{
			DataPacket response = new DataPacket();
			response.addParameter(fsInterface.renameFile(getRelativePath(packet), getNewRelativePath(packet)));
			
			return response;
		}
		if (command.equals("READ"))
		{
			DataPacket response = new DataPacket();
			
			byte[] read = fsInterface.readFile(getRelativePath(packet), getReadStartAt(packet), getReadLength(packet));
			
			if (read != null)
			{
				response.addParameter(true);
				response.addParameter(read);
			}
			else
				response.addParameter(false);

			return response;
		}
		if (command.equals("WRITE"))
		{
			DataPacket response = new DataPacket();
			response.addParameter(fsInterface.writeFile(getRelativePath(packet), getReadStartAt(packet), getFileBuffer(packet)));
			
			return response;
		}
		
		return null;
	}

}
