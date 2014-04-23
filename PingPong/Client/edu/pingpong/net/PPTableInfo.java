package edu.pingpong.net;

public class PPTableInfo 
{
	public static int TABLE_LEFT_EMPTY  = 0;
	public static int TABLE_RIGHT_EMPTY = 1;
	public static int TABLE_BUSY_EMPTY  = 2;
	
	private int iStatus;
	private String sOpponent1, sOpponent2;
	
	public PPTableInfo( String sOpp1, String sOpp2, int iStt )
	{
		iStatus    = iStt;
		sOpponent1 = sOpp1;
		sOpponent2  = sOpp2;
	}
	
	public String getOpponent1()
	{
		return sOpponent1;
	}

	public String getOpponent2()
	{
		return sOpponent2;
	}	

	public int getStatus()
	{
		return iStatus;
	}	
}
