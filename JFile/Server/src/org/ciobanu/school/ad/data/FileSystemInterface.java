package org.ciobanu.school.ad.data;

public interface FileSystemInterface
{
	FileInfoRecord[] listFiles(String relativePath);
	boolean deleteFile(String relativePath);
	boolean createEmptyFile(String relativePath, long size);
	boolean renameFile(String relativePath, String newRelativePath);
	
	/* Access */
	byte[] readFile(String relativePath, long startAt, long length);
	boolean writeFile(String relativePath, long startAt, byte[] buffer);
}
