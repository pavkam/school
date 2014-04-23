package edu.pingpong.net;

import java.util.ArrayList;

import edu.pingpong.physycs.PPThreadController;


public class PPConnector 
{
	private String strUser, strPass;
	private PPSocket ppSock;
	
	private ArrayList queuedPackets = new ArrayList();
		
	public PPConnector( String sUser, String sPass, PPSocket pp )
	{
		strUser = sUser;
		strPass = sPass;
		
		ppSock = pp;
	}

	private int queuedPacketCount()
	{
		return queuedPackets.size();
	}
	
	private GamePacket waitForPacket()
	{
		PPThreadController.addThread();
		
		while ( queuedPacketCount() == 0 )
		{
			if (PPThreadController.stopSignaled()) break;
			
			try
			{
				Thread.sleep( 10 );
			} catch (Exception e)
			{}				
			
		}
		PPThreadController.removeThread();
		
		if ( queuedPacketCount() > 0 )
			return (GamePacket)(queuedPackets.remove( 0 )); else
				return null;				
	}
	
	public boolean logIn()
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_LOGIN );
		gp.addString( strUser );
		gp.addString( strPass );
		
		ppSock.sendPacket( gp );
		
		//** Wait for a response error to arrive!
		
		GamePacket rp = waitForPacket();
		
		if ( rp == null ) return false;
		
		if ( rp.getOpcode() != GamePacket.PKT_ERROR ) return false;
		if ( rp.getByte() != GamePacket.TYPE_ERROR_OK ) return false;
		
		return true;
	}
	
	public void queuePacket( GamePacket gp )
	{
		
		// If ping, process ourselves and send a reply back
		if ( gp.getOpcode() == GamePacket.PKT_PING )
		{
			GamePacket gpi = new GamePacket( GamePacket.PKT_PING );
			gpi.addByte( GamePacket.PING_TYPE_CLIENT );
			
			ppSock.sendPacket( gpi );
			return;
		}
	
		// Add a packet to process queue
		queuedPackets.add( gp );			
	}
	
//////////////////////////////////////////////////////////////////

	public PPMyStats getMyStats()
	{
		PPMyStats mys = new PPMyStats( 0, 0, 0, strUser );
		
		GamePacket gp = new GamePacket( GamePacket.PKT_STATS );
		gp.addByte( GamePacket.STAT_REQ_MINE );		
		ppSock.sendPacket( gp );
		
		//** Wait for a response to arrive!
		
		GamePacket rp = waitForPacket();
		
		if ( rp == null ) return mys;
		
		if ( rp.getOpcode() != GamePacket.PKT_STATS ) return mys;
		if ( rp.getByte() != GamePacket.STAT_REQ_MINE ) return mys;
		
		int iLost, iWon, iRank;
		
		iRank = rp.getInt();
		iWon  = rp.getInt();
		iLost = rp.getInt();
						
		return new PPMyStats( iRank, iWon, iLost, strUser );
	}
	
	public int getRankListPages()
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_STATS );
		gp.addByte( GamePacket.STAT_REQ_PG );		
		ppSock.sendPacket( gp );
		
		//** Wait for a response to arrive!
		
		GamePacket rp = waitForPacket();
		
		if ( rp == null ) return 0;
		
		if ( rp.getOpcode() != GamePacket.PKT_STATS ) return 0;
		if ( rp.getByte() != GamePacket.STAT_REQ_PG ) return 0;
						
		return rp.getInt();
	}
		
	public PPMyStats[] getRankListPage( int iPage )
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_STATS );
		gp.addByte( GamePacket.STAT_REQ_ALL );
		gp.addInt( iPage );
		
		ppSock.sendPacket( gp );
		
		//** Wait for a response to arrive!
		
		GamePacket rp = waitForPacket();
		
		if ( rp == null ) return new PPMyStats[0];
		
		if ( rp.getOpcode() != GamePacket.PKT_STATS ) return new PPMyStats[0];
		if ( rp.getByte() != GamePacket.STAT_REQ_ALL ) return new PPMyStats[0];
						
		rp.getInt();
		
		int iCnt = rp.getInt();
		PPMyStats[] res = new PPMyStats[iCnt];
		
		for (int i = 0; i< iCnt; i++)
		{
			int iRank, iWon, iLost;
			String sName;
			
			sName = rp.getString();
			iRank = rp.getInt();
			iWon  = rp.getInt();
			iLost = rp.getInt();
			
			res[i] = new PPMyStats( iRank, iWon, iLost, sName );
		}
		
		return res;
	}	
		
	public PPTableInfo getMyTable()
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_TABLS );
		gp.addByte( GamePacket.TABL_REQ_MINE );		
		ppSock.sendPacket( gp );
		
		//** Wait for a response to arrive!
		
		GamePacket rp = waitForPacket();
		
		if ( rp == null ) return null;
		
		if ( rp.getOpcode() != GamePacket.PKT_TABLS ) return null;
		if ( rp.getByte() != GamePacket.TABL_REQ_MINE ) return null;
		
		String sLeft, sRight;
		int iTp = 0;
		
		sLeft  = rp.getString();
		sRight = rp.getString();		
		
		if (sLeft.equals( "" ) && sRight.equals( "" )) return null;
		
		if (sLeft.equals( "" ) && !sRight.equals( "" )) 
			iTp = PPTableInfo.TABLE_LEFT_EMPTY;

		if (!sLeft.equals( "" ) && sRight.equals( "" )) 
			iTp = PPTableInfo.TABLE_RIGHT_EMPTY;

		if (!sLeft.equals( "" ) && !sRight.equals( "" )) 
			iTp = PPTableInfo.TABLE_BUSY_EMPTY;
		
		return new PPTableInfo( sLeft, sRight, iTp );
	}	
		
	public int getTableListPages()
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_TABLS );
		gp.addByte( GamePacket.TABL_REQ_PG );		
		ppSock.sendPacket( gp );
		
		//** Wait for a response to arrive!
		
		GamePacket rp = waitForPacket();
		
		if ( rp == null ) return 0;
		
		if ( rp.getOpcode() != GamePacket.PKT_TABLS ) return 0;
		if ( rp.getByte() != GamePacket.TABL_REQ_PG ) return 0;
						
		return rp.getInt();
	}	
	
	public PPTableInfo[] getTableListPage( int iPage )
	{	
		GamePacket gp = new GamePacket( GamePacket.PKT_TABLS );
		gp.addByte( GamePacket.TABL_REQ_ALL );
		gp.addInt( iPage );
		
		ppSock.sendPacket( gp );
		
		//** Wait for a response to arrive!
		
		GamePacket rp = waitForPacket();
		
		if ( rp == null ) return new PPTableInfo[0];
		
		if ( rp.getOpcode() != GamePacket.PKT_TABLS ) return new PPTableInfo[0];
		if ( rp.getByte() != GamePacket.TABL_REQ_ALL ) return new PPTableInfo[0];
						
		rp.getInt();
		
		int iCnt = rp.getInt();
		PPTableInfo[] res = new PPTableInfo[iCnt];
		
		for (int i = 0; i< iCnt; i++)
		{
			String sLeft, sRight;
			int iTp = 0;
			
			sLeft  = rp.getString();
			sRight = rp.getString();		
			
			if (sLeft.equals( "" ) && sRight.equals( "" ))
				iTp = PPTableInfo.TABLE_LEFT_EMPTY;
			
			if (sLeft.equals( "" ) && !sRight.equals( "" )) 
				iTp = PPTableInfo.TABLE_LEFT_EMPTY;

			if (!sLeft.equals( "" ) && sRight.equals( "" )) 
				iTp = PPTableInfo.TABLE_RIGHT_EMPTY;

			if (!sLeft.equals( "" ) && !sRight.equals( "" )) 
				iTp = PPTableInfo.TABLE_BUSY_EMPTY;
			
			res[i] = new PPTableInfo( sLeft, sRight, iTp );
		}
		
		return res;
	}		
		
	public boolean dropMyTable()
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_TABLS );
		gp.addByte( GamePacket.TABL_DROP );
		
		ppSock.sendPacket( gp );
		
		//** Wait for a response error to arrive!
		
		GamePacket rp = waitForPacket();
		
		if ( rp == null ) return false;
		
		if ( rp.getOpcode() != GamePacket.PKT_ERROR ) return false;
		if ( rp.getByte() != GamePacket.TYPE_ERROR_OK ) return false;
		
		return true;
	}
	
	public boolean createMyTable()
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_TABLS );
		gp.addByte( GamePacket.TABL_CREATE );
		
		ppSock.sendPacket( gp );
		
		//** Wait for a response error to arrive!
		
		GamePacket rp = waitForPacket();
		
		if ( rp == null ) return false;
		
		if ( rp.getOpcode() != GamePacket.PKT_ERROR ) return false;
		if ( rp.getByte() != GamePacket.TYPE_ERROR_OK ) return false;
		
		return true;
	}
	
	public int joinTable( PPTableInfo pp )
	{
		String sChl = "";
		int ir = PPTableInfo.TABLE_BUSY_EMPTY;
		
		if ( pp.getStatus() == PPTableInfo.TABLE_BUSY_EMPTY ) return PPTableInfo.TABLE_BUSY_EMPTY;
		
		if ( pp.getStatus() == PPTableInfo.TABLE_LEFT_EMPTY ) 
			{ sChl = pp.getOpponent2(); ir = PPTableInfo.TABLE_LEFT_EMPTY; }
		
		if ( pp.getStatus() == PPTableInfo.TABLE_RIGHT_EMPTY ) 
			{ sChl = pp.getOpponent1(); ir = PPTableInfo.TABLE_RIGHT_EMPTY; }
		
		GamePacket gp = new GamePacket( GamePacket.PKT_CHALLENGE );
		gp.addString( sChl );
		
		ppSock.sendPacket( gp );
		
		//** Wait for a response error to arrive!
		
		GamePacket rp = waitForPacket();
		
		if ( rp == null ) return PPTableInfo.TABLE_BUSY_EMPTY;
		
		if ( rp.getOpcode() != GamePacket.PKT_ERROR ) return PPTableInfo.TABLE_BUSY_EMPTY;
		if ( rp.getByte() != GamePacket.TYPE_ERROR_OK ) return PPTableInfo.TABLE_BUSY_EMPTY;
		
		return ir;		
	}
	
	public void sendError( byte iCode )
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_ERROR );
		gp.addByte( iCode );
		
		ppSock.sendPacket( gp );
	}
	
	public void sendLeaveTable()
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_DCONN );
		gp.addByte( GamePacket.TYPE_ERROR_OK );
		
		ppSock.sendPacket( gp );
	}

	public void sendChat( String sLine )
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_CHAT );
		gp.addByte( GamePacket.CHAT_CLIENT );
		gp.addString( sLine );
		
		ppSock.sendPacket( gp );
	}

	public void sendMyUser()
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_SYNC );
		gp.addByte( GamePacket.SYNC_OPP_NAME );
		gp.addString( strUser );
		
		ppSock.sendPacket( gp );
	}
	
	public void sendSlapperCoordUpdate( int iNewX, int iNewY )
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_SYNC );
		gp.addByte( GamePacket.SYNC_OPP_SLP );
		gp.addInt( iNewX );
		gp.addInt( iNewY );
		
		ppSock.sendPacket( gp );		
	}
	
	public void sendReady()
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_READY );
		gp.addByte( GamePacket.TYPE_ERROR_OK );
		
		ppSock.sendPacket( gp );
	}
	
	public void sendBallUpdate( double fX, double fY, double fSX, double fSY, double fAX, double fAY )
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_SYNC );
		gp.addByte( GamePacket.SYNC_OPP_BALL );
		gp.addDouble( fX );
		gp.addDouble( fY );
		gp.addDouble( fSX );
		gp.addDouble( fSY );
		gp.addDouble( fAX );
		gp.addDouble( fAY );
		
		ppSock.sendPacket( gp );
	}
	
	public void sendWallHit()
	{
		GamePacket gp = new GamePacket( GamePacket.PKT_HIT );
		gp.addByte( GamePacket.TYPE_ERROR_OK );
		
		ppSock.sendPacket( gp );		
	}
	
	public String getUser()
	{
		return strUser;
	}
}
