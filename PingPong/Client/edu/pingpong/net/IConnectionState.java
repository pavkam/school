package edu.pingpong.net;

public interface IConnectionState 
{
	void connectionLost();	
	void bytesRead( int iLen );
	void bytesSent( int iLen );
	void processPacket( GamePacket gp );
}
