import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.util.ArrayList;
import java.util.Properties;

public class PPUsers 
{
	final static private int USERS_PER_PAGE = 10;
	final private String sUsersFile = "data/users.ini";
	private Properties ppSaved;
	private ArrayList alUsers;
	
	public PPUsers() throws Exception
	{
		ppSaved = new Properties();
		
		try
		{
			FileInputStream fIn = new FileInputStream( sUsersFile ); 
			ppSaved.load( fIn );
			fIn.close();
		} catch ( Exception e )
		{
			// Unable to load the file :(
			throw new Exception( "Users file cannot be load!" );
		}		
		
		alUsers = new ArrayList();
	
		loadProperties();
	}
	
	private void loadProperties()
	{
		alUsers.clear();
		
		int iUsers = (int)Double.parseDouble( ppSaved.getProperty( "users.count", "0" ) );
		
		for (int i = 0; i< iUsers; i++)
		{
			String sProp = "user." + i + ".";
			String sName;
			String sPass;
			
			int iW;
			int iL;
			
			sName = ppSaved.getProperty( sProp + "name" );
			sPass = ppSaved.getProperty( sProp + "pass" );
			iW    = (int)Double.parseDouble( ppSaved.getProperty( sProp + "won", "0" ));
			iL    = (int)Double.parseDouble( ppSaved.getProperty( sProp + "lost", "0" ));
			
			if (sName == null)
				sName = "";
			
			if (sPass == null)
				sPass = "";
			
			alUsers.add( new PPUserInfo( sName, sPass, iW, iL ) ); 
		}
		
	}
	
	public PPUserInfo getUser( String sName )
	{
		for (int i = 0; i < alUsers.size(); i++)
		{
			if ( ((PPUserInfo)alUsers.get(i)).getName().toUpperCase().equals( sName.toUpperCase() ) )
			{
				return (PPUserInfo)alUsers.get( i );
			}
		}
		
		return null;
	}
	
	public void saveUsers()
	{
		
		ppSaved.put( "users.count", String.valueOf( alUsers.size() ) );
				
		for (int i = 0; i < alUsers.size(); i++)
		{
			String sProp = "user." + i + ".";
			PPUserInfo pp = (PPUserInfo)alUsers.get( i );
			
			ppSaved.put( sProp + "name", pp.getName() );
			ppSaved.put( sProp + "pass", pp.getPassword() );
			ppSaved.put( sProp + "won", String.valueOf( pp.getGamesWon() ) );
			ppSaved.put( sProp + "lost", String.valueOf( pp.getGamesLost() ) );
		}
		
		try
		{
			FileOutputStream fOut = new FileOutputStream( sUsersFile );
			ppSaved.store( fOut, "--" );
			fOut.close();
			
		} catch ( Exception e )
		{
			// Unable to save the file :(
			return; 
		}			
	}
	
	public int size()
	{
		return alUsers.size();
	}

	public int getRank( String sName )
	{
		PPUserInfo user = getUser( sName );		
		if (user == null ) return 0;
		
		double eqs = (user.getGamesWon() + 1);
		eqs /= (user.getGamesLost() + 1);
		
		int rank = 1;
		
		
		for ( int i = 0; i < alUsers.size(); i++ )
		{
			PPUserInfo user2 = (PPUserInfo)alUsers.get( i );
			
			double eqd = (user2.getGamesWon() + 1);
			eqd /= (user2.getGamesLost() + 1);
			
			if (eqd > eqs) rank++;
		}
		
		return rank;
	}
	
	public int getNumberOfPages()
	{
		int iR = (alUsers.size() / USERS_PER_PAGE);
		
		if ( (alUsers.size() % USERS_PER_PAGE) != 0 )
			return iR+1; else
				return iR;
	}
	
	public Object[] getUserPage( int iPg )
	{
		int i, u;
		ArrayList al = new ArrayList();
		
		for (i = iPg * USERS_PER_PAGE, u = 0; i < alUsers.size() && u < USERS_PER_PAGE; i++, u++)
		{
			al.add( alUsers.get(i) );
		}
		
		return al.toArray();
	}
}
