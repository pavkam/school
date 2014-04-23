package org.ciobanu.school.ad.net;

public interface ControlPacket
{
	int getParameterCount();
	byte[] getParameter(int i);
}
