import java.net.ServerSocket;		   
import java.net.Socket;

public class PPApplication implements Runnable
{
	private ServerSocket ssAcceptor;
	private PPUsers users;
	private PPTables tables;
	
	public void run()
	{
		Socket sRecv;
					
		while (true)
		{
			try
			{
				sRecv = ssAcceptor.accept();
				System.out.println( "Client Accepted from \"" + sRecv.getInetAddress().getHostAddress() + "\"" );
				
				new PPWorker( sRecv, users, tables );
					
			} catch ( Exception e )
			{
				return;
			}		
		}
	}	
	
	public void startListening()
	{
		new Thread( this ).start();
	}
	
	public void stopListening()
	{
		try
		{
			ssAcceptor.close();
		} catch( Exception e )
		{
			/* Nothing to treat here ... */
		}		
	}
		
	public boolean runMain( int iPrt )
	{

		/* Main Server Entry Point */
		char cIn;
		
		System.out.print("Loading data files ..");
		
		try
		{
			users = new PPUsers();
		} catch ( Exception e )
		{
			System.out.println("[failed]");
			return false;
		}
		
		System.out.println("[ok]");
		System.out.println( "We have " + users.size() + " registered users!");
		
		System.out.print("Creating tables ...");		
		tables = new PPTables();
		System.out.println("[ok]");
		
		System.out.print("Initializing PP Server ... ");
		
		try
		{
			ssAcceptor = new ServerSocket( iPrt );
			startListening();
			
		} catch ( Exception e )
		{
			System.out.println("[failed]");
			return false;
		}
		
		System.out.println("[ok]");
		System.out.println("Listening on port " + String.valueOf(iPrt) );		
		System.out.println("Press \"Q\" to close this server!");
				
		while (true)
		{
			try
			{
				cIn = (char)System.in.read();
			} catch ( Exception e )
			{
				stopListening();
				
				return false;
			}
			
			if ( ( cIn == 'q' ) || ( cIn == 'Q' ) )
			{
				stopListening();
				
				return true;
			}			
		}
	}
	
	public static void main(String[] args) 
	{
		PPApplication pp = new PPApplication();
		
		if ( !pp.runMain( 2000 ) )
		{
			System.out.println( "Error! Error while running!" );
		} else
		{
			System.out.println( "Server Stopped!" );
		}

		System.exit( 0 );
	}

}
