import java.io.IOException;

import javax.swing.JOptionPane;

import edu.pingpong.net.GamePacket;
import edu.pingpong.net.IConnectionState;
import edu.pingpong.net.PPConnector;
import edu.pingpong.net.PPSocket;
import edu.pingpong.net.PPTableInfo;
import edu.pingpong.physycs.PPPhysycalObjectState;
import edu.pingpong.physycs.PPThreadController;
import edu.pingpong.ui.PPInstance;
import edu.pingpong.ui.PPLoginFrame;
import edu.pingpong.ui.PPMainMenu;

/*
 * This Class is the Application base start point.
 * It will be created first and will invoke the rest of the
 * code.
 */

public class PPApplication implements IConnectionState
{
	private PPInstance frmMain;
	private PPLoginFrame frmSelServer;
	private PPMainMenu frmMainMenu;
	private PPConnector clientSession;
	private PPPhysycalObjectState ppPhys;
	private PPSocket ppSock;

	
	private void exit()
	{
		PPThreadController.stopAllThreads();
		System.exit( 0 );
	}
	
	public void run()
	{
		// Create the login Form.		
		frmSelServer =  new PPLoginFrame();		
		
		while ( frmSelServer.isVisible() )
		{
			try
			{
				Thread.sleep( 10 ); // Wait for the form to close
			} catch ( Exception e)
			{
				
			}
			
		}
		
		if ( frmSelServer.getModalResult() != PPLoginFrame.mrOK )
		{
			// Cancel or closed. Exiting!
			exit();
		}
		
		try
		{
			ppSock = new PPSocket( frmSelServer.getHost(), 2000, this );
		} catch ( IOException e )
		{
			JOptionPane.showMessageDialog( null, "Connection to server could not have been established!" );
			exit();
		}
		
		clientSession = new PPConnector( 
				frmSelServer.getUser(),
				frmSelServer.getPassword(),
				ppSock
				);
		
		if ( !clientSession.logIn() )
		{
			JOptionPane.showMessageDialog( null, "Login Failed! Check your data!" );
			exit();				
		}
		
		while (true)
		{
		frmMainMenu = new PPMainMenu( clientSession );
		frmMainMenu.setLocation( 
				(int)( frmSelServer.getLocation().getX() - ( (frmMainMenu.getWidth() - frmSelServer.getWidth())/2 )), 
				(int)( frmSelServer.getLocation().getY() - ( (frmMainMenu.getHeight() - frmSelServer.getHeight())/2 ))
				);	
		
		frmMainMenu.setTitle( "Server Menu - " + frmSelServer.getUser() );
		frmMainMenu.setVisible( true );
		
		while ( frmMainMenu.isVisible() )
		{
			try
			{
				Thread.sleep( 10 ); // Wait for the form to close
			} catch ( Exception e)
			{
				
			}
			
		}
		
		if (frmMainMenu.getModalResult() == PPLoginFrame.mrCancel) break;
		
		PPTableInfo pp = frmMainMenu.getSelectedTable();
		boolean isRight = false;		
		
		if (frmMainMenu.getModalResult() == PPLoginFrame.mrOK)
		{			
			int iRes = clientSession.joinTable( pp );

			if ( iRes == PPTableInfo.TABLE_BUSY_EMPTY )
			{
				JOptionPane.showMessageDialog( null, "The table you selected is not available anymore! Select another one!" );
				continue;			
			}
			
			if ( iRes == PPTableInfo.TABLE_LEFT_EMPTY )
				isRight = false;
			
			if ( iRes == PPTableInfo.TABLE_RIGHT_EMPTY )
				isRight = true;					
		} else
		{
			if (pp.getStatus() == PPTableInfo.TABLE_RIGHT_EMPTY)
				isRight = false;	
			
			if (pp.getStatus() == PPTableInfo.TABLE_LEFT_EMPTY)
				isRight = true;				
		}
		
		
				
		frmMain = new PPInstance( isRight, clientSession );
		frmMain.setLocation( 
				(int)( frmSelServer.getLocation().getX() - ( (frmMain.getWidth() - frmSelServer.getWidth())/2 )), 
				(int)( frmSelServer.getLocation().getY() - ( (frmMain.getHeight() - frmSelServer.getHeight())/2 ))
				);
		
		ppPhys = new PPPhysycalObjectState( 
				frmMain.getGameBall(), 
				(int)frmMain.getPlayFieldSize().getWidth(), 
				(int)frmMain.getPlayFieldSize().getHeight(),
				frmMain.getSlapper(),
				clientSession
				);
		
		frmMain.setPhysycs( ppPhys );
		frmMain.setTitle( "Play Table - " + frmSelServer.getUser() );
		
		//
		
		clientSession.sendMyUser();		
		frmMain.setVisible( true );
		
		clientSession.sendReady();
		
		ppPhys.startSimulation();
		

		while ( frmMain.isVisible() )
		{
			try
			{
				Thread.sleep( 10 ); // Wait for the form to close
			} catch ( Exception e)
			{
				
			}
			
		}
		
		ppPhys.stopSimulation();
		
		if ( frmMain.getModalResult() != -1 )
		{
			clientSession.sendLeaveTable();
		}		
		
				
		if ( frmMain.getModalResult() == PPLoginFrame.mrCancel )
		{
			break;
		}
		
		}
		
		exit();
	}
		
	public static void main(String[] args) 
	{
		PPApplication pp = new PPApplication();
		pp.run();
	}
	
	/* Connection states and stuff */

	public void connectionLost() 
	{
		try
		{
			JOptionPane.showMessageDialog( null, "Connection to the server has been lost!" );
						
		} catch (Exception e)
		{
			// Swing deinitialized throws errors
		}
			
		if (frmMain != null)
			frmMain.setVisible( false );
		
		if (frmMain != null)
			frmSelServer.setVisible( false );
		
		if (frmMain != null)
			frmMainMenu.setVisible( false );				
		
		exit();
	}

	public void bytesRead(int iLen) {
		// TODO Auto-generated method stub
		
	}

	public void bytesSent(int iLen) {
		// TODO Auto-generated method stub
		
	}

	public void processPacket(GamePacket gp) 
	{
		// We have received an incoming packet. Let's forward it to Connector
		
		if ( gp.getOpcode() == GamePacket.PKT_CHALLENGE && frmMainMenu != null ) 
		{
			frmMainMenu.processChallenge( gp );
			return;
		}
		
		if ( gp.getOpcode() == GamePacket.PKT_DCONN && frmMain != null )
		{
			frmMain.processSync( gp );
			return;
		}
		
		if ( gp.getOpcode() == GamePacket.PKT_CHAT && frmMain != null )
		{
			frmMain.processSync( gp );
			return;
		}		
		
		if ( gp.getOpcode() == GamePacket.PKT_SYNC && frmMain != null )
		{
			frmMain.processSync( gp );
			return;
		}
		
		if ( gp.getOpcode() == GamePacket.PKT_HIT && frmMain != null )
		{
			frmMain.processSync( gp );
			return;
		}			
		
		if ( gp.getOpcode() == GamePacket.PKT_READY && frmMain != null )
		{
			frmMain.processSync( gp );
			return;
		}			
					
		if ( clientSession != null )
			clientSession.queuePacket( gp );
		
	}

}
