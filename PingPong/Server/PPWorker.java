import java.net.Socket;
import java.util.ArrayList;

public class PPWorker implements Runnable
{
	static ArrayList sockets = new ArrayList();
	
	static int workerID = 0;
	
	final static int ROUND_SPEEDS = 200;
	final static int SLEEP_TIME = 50;
	final static int PING_TIME  =  10; // Seconds
	final static int BUFFER_SIZE = 2048;
	final static int ROUNDS_PER_GAME = 12;
	
	private Socket mySocket;
	private PPUsers allUsers;
	private PPTables allTables;
	
	private byte[] inputBuffer = new byte[ BUFFER_SIZE ];
	private int inputBuffer_Pos = 0;
	
	private GamePacket outPackets[] = new GamePacket[10];
	private int outPacket_Nr = 0;
	
	private boolean bLoggedIn = false;
	private int iWorkerID;
	private String sUserName = "";
	
	private int iTimeToPing;
	private boolean bWaitForPong;
	
	private int iScore;
	
	private boolean bReady = false;	
	private PPWorker cOpponent = null;
	
	static synchronized int newID()
	{
		return workerID++;
	}
	
	static synchronized void addWorker( PPWorker a )
	{
		sockets.add( a );
	}
	
	static synchronized void removeWorker( PPWorker a )
	{
		sockets.remove( a );
	}
	
	static boolean isAlreadyLoggedIn( String sUser )
	{
		String sU = sUser.toUpperCase();
		
		for (int i = 0; i< sockets.size(); i++)
		{
			if ( ((PPWorker)sockets.get( i )).getUserName().toUpperCase().equals(sU) )
			{
				return ((PPWorker)sockets.get( i )).isLoggedIn();
			}
		}
		
		return false;
	}

	static PPWorker getWorker( String sUser )
	{
		String sU = sUser.toUpperCase();
		
		for (int i = 0; i< sockets.size(); i++)
		{
			if ( ((PPWorker)sockets.get( i )).getUserName().toUpperCase().equals(sU) )
			{
				return ((PPWorker)sockets.get( i ));
			}
		}
		
		return null;
	}
	
	public PPWorker( Socket sSkt, PPUsers users, PPTables tables )
	{
		mySocket    = sSkt;
		iWorkerID   = newID();
		
		iTimeToPing  = (PING_TIME * 1000) / SLEEP_TIME;
		bWaitForPong = false;
		
		allUsers  = users;
		allTables = tables;
		
		logText( "Worker created for connection. ID was given." );
		
		addWorker( this );
		new Thread( this ).start();
	}
	
	
	private int getScoreInc()
	{
		return (++iScore);
	}
	
	private int getScore()
	{
		return iScore;
	}
	
	private String getUserName()
	{
		return sUserName;
	}
	
	private boolean getReady()
	{
		return bReady;
	}
	
	private void clearReady()
	{
		bReady = false;
	}
	
	private boolean isLoggedIn()
	{
		return bLoggedIn;
	}	
	
	private void logText( String str )
	{
		System.out.println( "<" + iWorkerID + "> "  + str );
	}
	
	public synchronized void sendPacket( GamePacket gp )
	{
		// Queue packets for sendings
		outPackets[ outPacket_Nr++ ] = gp;		
	}
	
	/*
	 * Packet processing
	 */
	
	public void packet_CheckLogin( String sUser, String sPass )
	{
		boolean ok;
		
		logText( "User \"" + sUser + "\" trying to login with password \"" + sPass + "\"" );
		
		if (isAlreadyLoggedIn( sUser ))
		{
			GamePacket a = new GamePacket( GamePacket.PKT_ERROR );
			a.addByte( GamePacket.TYPE_ERROR_ALOG );
			sendPacket( a );
			
			logText( "User \"" + sUser + "\" already logged in!" );
			return;
		}
		
		if ( allUsers.getUser( sUser ) == null )
		{
			ok = false;
		} else
		{
			if ( allUsers.getUser( sUser ).getPassword().equals( sPass ) )
				ok = true; else
					ok = false;
		}
		
		GamePacket a = new GamePacket( GamePacket.PKT_ERROR );
		
		if ( ok )
		{
			logText( "User \"" + sUser + "\" logged in!" );			
			a.addByte( GamePacket.TYPE_ERROR_OK );
			
			sUserName = sUser;
			bLoggedIn = true;
		} else
		{
			logText( "User \"" + sUser + "\" rejected!" );			
			a.addByte( GamePacket.TYPE_ERROR_REJECTED );
			
			sUserName = "";
			bLoggedIn = false;
		}
		
		sendPacket( a );
	}
	
	private void packet_SendMyStats()
	{
		logText( "User stats requested! Sending right now!" );
		
		GamePacket gp = new GamePacket( GamePacket.PKT_STATS );
		gp.addByte( GamePacket.STAT_REQ_MINE );
		
		///
		PPUserInfo pp = allUsers.getUser( getUserName() );
		
		if (pp == null)
		{
			logText( "Internal error! Cannot retreive user stats??" );
			return;
		}
		
		gp.addInt( allUsers.getRank( getUserName() ) );
		gp.addInt( pp.getGamesWon() );
		gp.addInt( pp.getGamesLost() );
		
		///
		
		sendPacket( gp );		
	}
	
	private void packet_SendStatsPages()
	{
		logText( "User stats pages requested!" );
		
		GamePacket gp = new GamePacket( GamePacket.PKT_STATS );
		gp.addByte( GamePacket.STAT_REQ_PG );
			
		gp.addInt( allUsers.getNumberOfPages() );
		
		sendPacket( gp );		
	}
	
	private void packet_SendAllStats( int ipg )
	{
		logText( "Stat page " + ipg + " requested! Sending right now!" );
		
		GamePacket gp = new GamePacket( GamePacket.PKT_STATS );
		gp.addByte( GamePacket.STAT_REQ_ALL );
		
		Object[] uia = allUsers.getUserPage( ipg );
		
		gp.addInt( ipg );
		gp.addInt( uia.length );
		
		for (int i = 0; i< uia.length; i++)
		{
			PPUserInfo ui = (PPUserInfo)uia[i];
			gp.addString( ui.getName() );
			gp.addInt( allUsers.getRank( ui.getName() ) );
			gp.addInt( ui.getGamesWon() );
			gp.addInt( ui.getGamesLost() );
		}
				
		sendPacket( gp );	
	}

	private void packet_SendMyTable()
	{
		logText( "User table requested! Sending right now!" );
		
		GamePacket gp = new GamePacket( GamePacket.PKT_TABLS );
		gp.addByte( GamePacket.TABL_REQ_MINE );
		
		///
		PPTableInfo pp = allTables.getTable( this );
		
		if (pp == null)
		{
			gp.addString( "" );
			gp.addString( "" );
		} else
		{
			if ( pp.getLeft() != null )
				gp.addString( pp.getLeft().getUserName() ); else
					gp.addString( "" );
				
			if ( pp.getRight() != null )
				gp.addString( pp.getRight().getUserName() ); else
					gp.addString( "" );			
		}
		
		sendPacket( gp );		
	}	
	
	private void packet_SendTablePages()
	{
		logText( "User table pages requested!" );
		
		GamePacket gp = new GamePacket( GamePacket.PKT_TABLS );
		gp.addByte( GamePacket.TABL_REQ_PG );
			
		gp.addInt( allTables.getNumberOfTables() );
		
		sendPacket( gp );		
	}
	
	private void packet_SendAllTables( int ipg )
	{
		logText( "Table page " + ipg + " requested! Sending right now!" );
		
		GamePacket gp = new GamePacket( GamePacket.PKT_TABLS );
		gp.addByte( GamePacket.TABL_REQ_ALL );
		
		Object[] uia = allTables.getTablePage( ipg );
		
		gp.addInt( ipg );
		gp.addInt( uia.length );
		
		for (int i = 0; i< uia.length; i++)
		{
			PPTableInfo ui = (PPTableInfo)uia[i];
			
			if ( ui.getLeft() != null )
				gp.addString( ui.getLeft().getUserName() ); else
					gp.addString( "" );
				
			if ( ui.getRight() != null )
				gp.addString( ui.getRight().getUserName() ); else
					gp.addString( "" );			
		}
				
		sendPacket( gp );	
	}
	
	private void packet_CreateTable()
	{
		logText( "Table creation requested! Adding to tables list!" );
		
		GamePacket gp = new GamePacket( GamePacket.PKT_ERROR );
		
		if ( allTables.createTable(this) )
			gp.addByte( GamePacket.TYPE_ERROR_OK ); else
				gp.addByte( GamePacket.TYPE_ERROR_REJECTED );
		
		sendPacket( gp );	
	}	
	
	private void packet_DropTable()
	{
		logText( "Table drop requested! Removing table from list!" );
		
		GamePacket gp = new GamePacket( GamePacket.PKT_ERROR );
		
		if ( allTables.dropTable(this) )
			gp.addByte( GamePacket.TYPE_ERROR_OK ); else
				gp.addByte( GamePacket.TYPE_ERROR_REJECTED );
		
		sendPacket( gp );	
	}	
	
	private void packet_ProcessChallenge( String sUser )
	{
		logText( "User \"" + sUser + "\" challenged!" );
		PPWorker pp = getWorker( sUser );
		
		if ( pp == null )
		{
			GamePacket gp = new GamePacket( GamePacket.PKT_ERROR );
			gp.addByte( GamePacket.TYPE_ERROR_REJECTED );
			
			sendPacket( gp );
			
			logText( "User \"" + sUser + "\" not connected!" );
			return;
		}
		
		PPTableInfo ti = allTables.getTable( pp );
		
		if ( ti == null )
		{
			GamePacket gp = new GamePacket( GamePacket.PKT_ERROR );
			gp.addByte( GamePacket.TYPE_ERROR_REJECTED );
			
			sendPacket( gp );
			
			logText( "User \"" + sUser + "\" has no table created!" );
			return;
		}
		
		// Add the user on that table
		if (!ti.addWorker( this ))
		{
			GamePacket gp = new GamePacket( GamePacket.PKT_ERROR );
			gp.addByte( GamePacket.TYPE_ERROR_REJECTED );
			
			sendPacket( gp );
			
			logText( "User \"" + sUser + "\" has no space on the table!" );
			return;			
		}
		
		cOpponent = pp;
		iScore = 0;
		
		GamePacket gp = new GamePacket( GamePacket.PKT_ERROR );
		gp.addByte( GamePacket.TYPE_ERROR_OK );
		
		sendPacket( gp );
		
		logText( "User \"" + sUser + "\" has accepted the challenger" );
		return;	
	}
	
	private void processPacket( GamePacket pkt )
	{
		
		if (pkt.getOpcode() == GamePacket.PKT_PING)
		{
			logText( "Client replied to sent Ping request!" );
			
			bWaitForPong = false;
			iTimeToPing  = (PING_TIME * 1000) / SLEEP_TIME;
			
			return;
		}
		
		if ( !bLoggedIn == true )
		{
			if (pkt.getOpcode() == GamePacket.PKT_LOGIN)
			{
				packet_CheckLogin( pkt.getString(), pkt.getString() );
			} else
			{
				logText( "User not logged in. Rejecting request!" );
				GamePacket a = new GamePacket( GamePacket.PKT_ERROR );
				a.addByte( GamePacket.TYPE_ERROR_NLOG );
				
				sendPacket( a );
			}
			
			return;
		}
		
		if (pkt.getOpcode() == GamePacket.PKT_STATS)
		{
			byte iType = pkt.getByte();
			
			if ( iType == GamePacket.STAT_REQ_MINE )
				packet_SendMyStats(); else
			
			if ( iType == GamePacket.STAT_REQ_PG )
				packet_SendStatsPages(); else	
							
			if ( iType == GamePacket.STAT_REQ_ALL )
				packet_SendAllStats( pkt.getInt() ); else			
			logText( "Invalid stats request! Dropping packet!" );
			
			return;
		}
		
		if (pkt.getOpcode() == GamePacket.PKT_TABLS)
		{
			byte iType = pkt.getByte();
			
			if ( iType == GamePacket.TABL_REQ_MINE )
				packet_SendMyTable(); else			
			
			if ( iType == GamePacket.TABL_CREATE )
				packet_CreateTable(); else				
				
			if ( iType == GamePacket.TABL_DROP )
				packet_DropTable(); else						
					
			if ( iType == GamePacket.TABL_REQ_PG )
				packet_SendTablePages(); else	
							
			if ( iType == GamePacket.TABL_REQ_ALL )
				packet_SendAllTables( pkt.getInt() ); else			
			logText( "Invalid table request! Dropping packet!" );
			
			return;
		}		
		
		if (pkt.getOpcode() == GamePacket.PKT_CHALLENGE)
		{
			packet_ProcessChallenge( pkt.getString() );
			return;
		}
		
		if (pkt.getOpcode() == GamePacket.PKT_HIT)			
		{		
			//GamePacket gpt = new GamePacket( GamePacket.PKT_HIT );
			if ( cOpponent == null )
			{
				logText( "This player has no opponent!" );
				return;
			}
			
			clearReady();
			cOpponent.clearReady();
			
			if ( ( getScore() + cOpponent.getScore() + 1) == ROUNDS_PER_GAME )
			{
				GamePacket gpl = new GamePacket( GamePacket.PKT_CHAT );
				gpl.addByte( GamePacket.CHAT_NOTICE );
				
				if ( getScore() > cOpponent.getScore() )
				{
					gpl.addString( "Game over! Winner " + getUserName() );
					
					PPUserInfo ppu = allUsers.getUser( getUserName() );
					ppu.setGamesWon( ppu.getGamesWon() + 1 );
					
					PPUserInfo ppu2 = allUsers.getUser( cOpponent.getUserName() );
					ppu2.setGamesLost( ppu.getGamesLost() + 1 );						
				}
				
				if ( getScore() < cOpponent.getScore() )
				{
					gpl.addString( "Game over! Winner " + cOpponent.getUserName() );
					
					PPUserInfo ppu = allUsers.getUser( cOpponent.getUserName() );
					ppu.setGamesWon( ppu.getGamesWon() + 1 );
					
					PPUserInfo ppu2 = allUsers.getUser( getUserName() );
					ppu2.setGamesLost( ppu.getGamesLost() + 1 );					
				}

				if ( getScore() == cOpponent.getScore() )
				{
					gpl.addString( "Game over! Draw game " );
				}
				
				sendPacket( gpl );
				cOpponent.sendPacket( gpl );
				
				GamePacket gpj = new GamePacket( GamePacket.PKT_HIT );
				gpj.addByte( GamePacket.TYPE_ERROR_REJECTED );
				
				sendPacket( gpj );
				cOpponent.sendPacket( gpj );
				
				allUsers.saveUsers();
				
				return;
			}			
			
			sendPacket( pkt );
			cOpponent.sendPacket( pkt );
						
			GamePacket gpl = new GamePacket( GamePacket.PKT_CHAT );
			gpl.addByte( GamePacket.CHAT_NOTICE );
			gpl.addString( "You have lost this round! Score: " + (iScore) );
			
			sendPacket( gpl );
			
			GamePacket gpw = new GamePacket( GamePacket.PKT_CHAT );
			gpw.addByte( GamePacket.CHAT_NOTICE );
			gpw.addString( "You have won this round! Score: " + cOpponent.getScoreInc() );
			
			cOpponent.sendPacket( gpw );
			
			return;
		}
		
		if (pkt.getOpcode() == GamePacket.PKT_READY)
		{
			if ( cOpponent == null )
			{
				logText( "This player has no opponent!" );
				return;
			}
			
			bReady = true;
			
			if ( cOpponent.getReady() )
			{
				logText( "Opponent is also ready! Let's begin the game :P" );
				
				java.util.Random rnd = new java.util.Random();
 
				int mx =  rnd.nextInt( (ROUND_SPEEDS / 2) );
				int my =  rnd.nextInt( (ROUND_SPEEDS / 2) );
				
				int icX = rnd.nextInt( 2 );
				int icY = rnd.nextInt( 2 );
				
				if ( icX == 0 ) icX = -1; else icX = 1;
				if ( icY == 0 ) icY = -1; else icY = 1;
				
				GamePacket gp = new GamePacket( GamePacket.PKT_READY );
				gp.addInt( icX * (ROUND_SPEEDS - mx) );
				gp.addInt( icY * (ROUND_SPEEDS - my) );
				
				gp.addDouble( 1 );
				gp.addDouble( 1 );
								
				sendPacket( gp );
				cOpponent.sendPacket( gp );
				
				GamePacket gpi = new GamePacket( GamePacket.PKT_CHAT );
				gpi.addByte( GamePacket.CHAT_NOTICE );
				gpi.addString( "New round " + (getScore() + cOpponent.getScore() + 1) + " is starting now!" );
				
				sendPacket( gpi );
				cOpponent.sendPacket( gpi );
				
				return;
			}
			
			return;
		}
		
		if (pkt.getOpcode() == GamePacket.PKT_DCONN)
		{
			if ( cOpponent == null )
			{
				logText( "This player has no opponent!" );
				return;
			}
			
			PPTableInfo ti1 = allTables.getTable( this );
			PPTableInfo ti2 = allTables.getTable( cOpponent );
			
			if ( ti1 != ti2 )
			{
				logText( "Not a valid opponent!" );
				return;
			}
			
			GamePacket gpj = new GamePacket( GamePacket.PKT_HIT );
			gpj.addByte( GamePacket.TYPE_ERROR_REJECTED );				
			
			sendPacket( gpj );
			cOpponent.sendPacket( gpj );
			
			GamePacket gpi = new GamePacket( GamePacket.PKT_CHAT );
			gpi.addByte( GamePacket.CHAT_NOTICE );
			gpi.addString( "Player " + getUserName() + " has left the game!" );
			
			cOpponent.sendPacket( gpi );			
			
			cOpponent.dropFromPlay();
			allTables.dropTable( ti1 );
			cOpponent = null;
			
			return;
		}
		
		if (pkt.getOpcode() == GamePacket.PKT_CHAT)
		{
			if ( cOpponent == null )
			{
				logText( "This player has no opponent!" );
				return;
			}
			
			GamePacket gpb = new GamePacket( GamePacket.PKT_CHAT );
			GamePacket gpn = new GamePacket( GamePacket.PKT_CHAT );
						
			gpb.addByte( GamePacket.CHAT_BACK );
			gpn.addByte( GamePacket.CHAT_CLIENT );
			
			pkt.getByte();
			String sMsg = pkt.getString();
			
			gpb.addString( sMsg );
			gpn.addString( sMsg );
			
			cOpponent.sendPacket( gpn );
			sendPacket( gpb );
			
			logText( "Forwarding the chat packet to the opponent!" );

			return;
		}		
		
		if (pkt.getOpcode() == GamePacket.PKT_SYNC)
		{
			if ( cOpponent == null )
			{
				logText( "This player has no opponent!" );
				return;
			}
			
			cOpponent.sendPacket( pkt );
			logText( "Forwarding the sync packet to the opponent!" );

			return;
		}				
		
		logText( "Invalid packet received! Opcode: " + String.valueOf( pkt.getOpcode() ) );
	}
	
	public void dropFromPlay()
	{
		cOpponent = null;
		
		GamePacket gp = new GamePacket( GamePacket.PKT_DCONN );
		gp.addByte( GamePacket.TYPE_ERROR_OK );
		
		sendPacket( gp );
	}
	
	public void includeIntoPlay( PPWorker pp )
	{
		iScore = 0;
		cOpponent = pp;
		
		GamePacket gp = new GamePacket( GamePacket.PKT_CHALLENGE );
		gp.addString( pp.getUserName() );
		
		sendPacket( gp );
	}	
	
	public void run()
	{
		int iAvail;
		
		while ( true )
		{
			try
			{
				if ( ( iAvail = mySocket.getInputStream().available() ) > 0 )
				{					
					logText( "Received " + iAvail + " bytes of data." );
					inputBuffer_Pos += ( mySocket.getInputStream().read( inputBuffer, inputBuffer_Pos, iAvail ) );
				}
			} catch ( Exception e )
			{
				logText( "Socket read error. Possible Disconnect?" );
				break;
			}
			
			
			/* *** */
			GamePacket gp = null;
			
			try
			{
				gp = GamePacket.extractPacket( inputBuffer, inputBuffer_Pos );				
				
			} catch (Exception e)
			{
				logText( "Invalid packet collected. Disconnecting!" );
				
				try 
				{
					mySocket.close();
				} catch (Exception x) {}
				
				break;
			}
			
			if (gp != null)
			{
				inputBuffer_Pos -= gp.getSize() + 5;
				processPacket( gp );
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
						logText( "Sent " + b.length + " bytes of data. Q["+i+"]" );
						mySocket.getOutputStream().write( b );
					} catch (Exception e)
					{
						logText( "Socket write error. Possible Disconnect?" );
						break;
					}
				}
				
				outPacket_Nr = 0;
			}
			
			try{
			Thread.sleep( SLEEP_TIME );
			} catch( Exception e)
			{
				break;
			}
			
			iTimeToPing--;
			
			if (iTimeToPing <= 0)
			{
				// Time to send a ping :P
				
				if (bWaitForPong)
				{
					logText( "Ping request failed. Client too slow or disconnected!" );
					
					try 
					{
						mySocket.close();
					} catch (Exception x) {}
					
					break;					
				}
				
				GamePacket gpi = new GamePacket( GamePacket.PKT_PING );
				gpi.addByte( GamePacket.PING_TYPE_SERVER );
				sendPacket( gpi );
				
				logText( "Ping request initiated! Waiting for client to respond!" );
				
				iTimeToPing  = (PING_TIME * 1000) / SLEEP_TIME;
				bWaitForPong = true;
			}
		}
		
		// Drop from any connected tables this player had
		
		if (cOpponent != null)
		{
			PPTableInfo ti1 = allTables.getTable( this );
			PPTableInfo ti2 = allTables.getTable( cOpponent );
			
			if ( ti1 == ti2 )
			{
				cOpponent.dropFromPlay();
				
				allTables.dropTable( ti1 );
				cOpponent = null;
			}				
			
		}
		
		removeWorker( this );
		logText( "Worker has been closed!" );
	}
}
