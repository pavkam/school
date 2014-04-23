package org.ciobanu.school.ad.net;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

public interface ControlPacketSerializer
{
	void setInputStream(InputStream stream);
	InputStream getInputStream();
	
	void setOutputStream(OutputStream stream);
	OutputStream getOutputStream();
	
	ControlPacket receivePacket(int timeOutMillis) throws IOException;
	void sendPacket(ControlPacket packet) throws IOException;
}
