package org.ciobanu.school.ad.net;

public interface EditableControlPacket extends ControlPacket
{
	void addParameter(byte[] param);
	void addParameter(String param);
	void addParameter(int param);
	void addParameter(long param);
	void addParameter(boolean param);
	
	void modifyParameter(int i, byte[] param);
	void modifyParameter(int i, String param);
	void modifyParameter(int i, int param);
	void modifyParameter(int i, long param);
	void modifyParameter(int i, boolean param);
	
	void removeParameter(int i);
}
