package edu.pingpong.net;

import java.io.IOException;
import java.net.Socket;

import edu.pingpong.physycs.PPThreadController;

public class PPSocket implements Runnable
{
	private Socket mySocket;
	
	final static int SLEEP_TIME = 50;
	final static int DCONN_TIME = 15; // Seconds
	final static int BUFFER_SIZE = 2048;
	
	private byte[] inputBuffer = new byte[ BUFFER_SIZE ];
	private int inputBuffer_Pos = 0;
	
	private GamePacket outPackets[] = new GamePacket[10];
	private int outPacket_Nr = 0;
	
	private int iTimeToDie = 0;
	
	private IConnectionState icc;
	
	public PPSocket( String sHost, int iPort, IConnectionState ic ) throws IOException
	{
			mySocket = new Socket( sHost, iPort );
			icc = ic;
						
			new Thread( this ).start();
	}
	
	public void run()
	{
		int iAvail;
		
		PPThreadController.addThread();
		
		iTimeToDie = DCONN_TIME * 1000;
		
		while ( !PPThreadController.stopSignaled() )
		{
			try
			{
				if ( ( iAvail = mySocket.getInputStream().available() ) > 0 )
				{					
					inputBuffer_Pos += ( mySocket.getInputStream().read( inputBuffer, inputBuffer_Pos, iAvail ) );
					icc.bytesRead( iAvail );
				}
			} catch ( IOException e )
			{
				break;
			}
			
			
			/* *** */
			GamePacket gp = null;
			
			try
			{
				gp = GamePacket.extractPacket( inputBuffer, inputBuffer_Pos );				
				
			} catch (Exception e)
			{
				try 
				{
					mySocket.close();
				} catch (IOException x) {}
				
				break;
			}
			
			if (gp != null)
			{
				iTimeToDie = DCONN_TIME * 1000;
				
				inputBuffer_Pos -= gp.getSize() + 5;
				icc.processPacket( gp );
			}
			
			////////
			/////// Sent queued packets
			
			if (outPacket_Nr > 0)
			{
				for (int i = 0; i < outPacket_Nr; i++ )
				{
					try
					{
						byte[] b = outPackets[i].packet();						
						mySocket.getOutputStream().write( b );
						icc.bytesSent( b.length );
					} catch (IOException e)
					{
						break;
					}
				}
				
				outPacket_Nr = 0;
			}
			
			try{
			Thread.sleep( SLEEP_TIME );
			} catch( InterruptedException e)
			{
				break;
			}

			iTimeToDie -= SLEEP_TIME;
			
			if ( iTimeToDie <= 0 )
			{
				break;
				// Disconnect/slow connection.
			}
			
		}
		
		if (mySocket.isConnected() || !mySocket.isClosed())
		{
			try
			{
				mySocket.close();
			} catch ( IOException e )
			{
				
			}
		}
		
		PPThreadController.removeThread();
		icc.connectionLost();
	}

	
	public void sendPacket( GamePacket gp )
	{
		// Queue packets for sendings
		outPackets[ outPacket_Nr++ ] = gp;			
	}
}
