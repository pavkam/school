
public class PPUserInfo 
{
	private String strName, strPassword;
	private int iGamesWon, iGamesLost;
	
	public PPUserInfo( String sName, String sPass, int iW, int iL)
	{
		strName      =  sName;
		strPassword  =  sPass;
		iGamesWon    =  iW;
		iGamesLost   =  iL;
	}
	
	public String getName()
	{
		return strName;
	}
	
	public String getPassword()
	{
		return strPassword;
	}
	
	public synchronized int getGamesWon()
	{
		return iGamesWon;
	}
	
	public synchronized int getGamesLost()
	{
		return iGamesLost;
	}	

	public synchronized void setGamesWon( int iG )
	{
		iGamesWon = iG;
	}
	
	public synchronized void setGamesLost( int iG )
	{
		iGamesLost = iG;
	}	
}
