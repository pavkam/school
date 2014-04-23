package edu.pingpong.net;

public class PPMyStats 
{
	private int iGamesLost = 0;
	private int iGamesWon  = 0;
	private int iRank      = 0;
	private String sName   = "";
	
	public PPMyStats( int iMyRank, int iMyWon, int iMyLost, String sMyName )
	{
		iGamesLost = iMyLost;
		iRank = iMyRank;
		iGamesWon = iMyWon;
		
		sName = sMyName;
	}
	
	public int getRank()
	{
		return iRank;
	}

	public int getGamesLost()
	{
		return iGamesLost;
	}
	
	public int getGamesWon()
	{
		return iGamesWon;
	}	
	
	public String getName()
	{
		return  sName;
	}
}
