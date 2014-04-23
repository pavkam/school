package org.ciobanu.school.ad.net.server;

import java.io.File;
import java.io.FileOutputStream;
import java.io.RandomAccessFile;
import java.util.ArrayList;

import org.ciobanu.school.ad.data.FileInfoRecord;
import org.ciobanu.school.ad.data.FileSystemInterface;

public class JFtpFileSystem implements FileSystemInterface
{
	private String basePath;
	
	public JFtpFileSystem(String path)
	{
		basePath = path;
	}
	
	public boolean createEmptyFile(String relativePath, long size)
	{
		File fileToCreate = new File(basePath + "/" + relativePath);
		
		if (fileToCreate.exists() || fileToCreate.isDirectory())
			return false;
		
		try {
			FileOutputStream fos = new FileOutputStream(fileToCreate);
			
			byte[] temp = new byte[1024 * 20];
			
			while (size > 0)
			{
				if (size >= temp.length)
				{
					fos.write(temp);
					size -= temp.length;
				}	
				else
				{
					fos.write(0);
					size--;
				}
			}
			
			fos.flush();
			fos.close();
		} catch (Exception e) {
			return false;
		}
		
		return true;
	}

	public boolean deleteFile(String relativePath)
	{
		try
		{
			File fileToDelete = new File(basePath + "/" + relativePath);
			
			if (fileToDelete.isDirectory())
				return false;
			
			fileToDelete.delete();
			
		} catch (Exception e)
		{
			return false;
		}
		
		return true;
	}

	public FileInfoRecord[] listFiles(String relativePath)
	{
		ArrayList<FileInfoRecord> fin_array = new ArrayList<FileInfoRecord>();
		
		try
		{
			File currentDir = new File(basePath + "/" + relativePath);
			
			if (!currentDir.isDirectory())
				return new FileInfoRecord[0];
			
			String[] fileNames = currentDir.list();
			
			for (int i = 0; i < fileNames.length; i++)
			{
				File fl = new File(basePath + "/" + relativePath + "/" + fileNames[i]);
				
				if (fl.isDirectory())
					continue;
				
				RandomAccessFile raf = new RandomAccessFile(fl, "r");
				fin_array.add(new FileInfoRecord(fl.getName(), raf.length()));
				
				raf.close();
			}
			
		} catch (Exception e)
		{
			return new FileInfoRecord[0];
		}
		
		FileInfoRecord[] result_arr = new FileInfoRecord[fin_array.size()];
		
		for (int i = 0; i < result_arr.length; i++)
			result_arr[i] = fin_array.get(i);
		
		return result_arr;
	}

	public byte[] readFile(String relativePath, long startAt, long length)
	{
		try
		{
			File fileToRead = new File(basePath + "/" + relativePath);
			RandomAccessFile raf = new RandomAccessFile(fileToRead, "r");
			
			raf.seek(startAt);
			
			byte[] result = new byte[(int)length];
			raf.read(result);
			
			raf.close();
			return result;
		} catch (Exception e)
		{		
		}

		
		return null;
	}

	public boolean renameFile(String relativePath, String newRelativePath)
	{
		try
		{
			File fileToRename = new File(basePath + "/" + relativePath);
			File fileToRenameTo = new File(basePath + "/" + newRelativePath);
			
			if (fileToRename.isDirectory())
				return false;
			
			fileToRename.renameTo(fileToRenameTo);
			
		} catch (Exception e)
		{
			return false;
		}
		
		return true;
	}

	public boolean writeFile(String relativePath, long startAt, byte[] buffer)
	{
		try
		{
			File fileToWrite = new File(basePath + "/" + relativePath);
			RandomAccessFile raf = new RandomAccessFile(fileToWrite, "rw");
			
			raf.seek(startAt);
			raf.write(buffer);
			
			raf.close();
			return true;
		} catch (Exception e)
		{		
		}

		return false;
	}

}
