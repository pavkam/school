package edu.pingpong.ui;

import javax.swing.JButton;
import javax.swing.JFrame;

import java.awt.Dimension;
import java.awt.Point;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.util.ArrayList;

import javax.swing.JLabel;
import javax.swing.JTextField;
import javax.swing.JTextPane;
import javax.swing.border.BevelBorder;
import javax.swing.border.SoftBevelBorder;
import javax.swing.text.html.HTMLEditorKit;

import edu.pingpong.physycs.PPBall;
import edu.pingpong.physycs.PPPhysycalObjectState;
import edu.pingpong.physycs.PPSlapper;
import edu.pingpong.net.GamePacket;
import edu.pingpong.net.PPConnector;


public class PPInstance extends JFrame implements MouseListener, MouseMotionListener {
	private PPPlayTable jLabelTable;
	private PPSlapper jSlapperA;
	private PPSlapper jSlapperO;
	private PPBall jBall;
	private JLabel jPlayerOppRight;
	private JTextField jMessage;
	private JButton jExit;
	private JTextPane jChat;
	private JButton jMainMenu;
	private JButton jSend;
	private JLabel jLblChat;
	private JLabel jPlayerOppLeft;
	private PPPhysycalObjectState ppObj;
	private PPConnector ppConn;
	final private int inObjDepl = 10;
	
	private ArrayList alLines = new ArrayList();
	
	private int iModalResult = PPLoginFrame.mrOK;
	
	private long iLastTime = 0; 

	public PPInstance( boolean isRight, PPConnector ppc )
	{
		initGUI();
		
		ppConn = ppc;
		
		jBall = new PPBall();		
		jLabelTable.add( jBall );
		
		jSlapperA = new PPSlapper( isRight );
		jSlapperO = new PPSlapper( !isRight );
		
		jLabelTable.add( jSlapperA );
		jLabelTable.add( jSlapperO );
				
		if ( isRight )
		{
			jSlapperA.setLocation(  
					( jLabelTable.getWidth() - inObjDepl ), 
					( jLabelTable.getHeight() / 2 )
					);
						
			jSlapperO.setLocation(  
					( inObjDepl ), 
					( jLabelTable.getHeight() / 2 )
					);
		} else
		{
			jSlapperO.setLocation(  
					( jLabelTable.getWidth() - inObjDepl ), 
					( jLabelTable.getHeight() / 2 )
					);
						
			jSlapperA.setLocation(  
					( inObjDepl ), 
					( jLabelTable.getHeight() / 2 )
					);
		}

		jSlapperA.setVisible( true );		
		jSlapperO.setVisible( true );		
		
		jLabelTable.addMouseListener( this );
		jLabelTable.addMouseMotionListener( this );

		setResizable( false );
		
		ppObj = null;
		
		iLastTime = System.currentTimeMillis();
	}
	
	private boolean shouldSendUpdate()
	{
		long iNowTime = System.currentTimeMillis();
		boolean res = false;
		
		// Update every 1/2 second ... not more!
		if ( ((iNowTime - iLastTime) / 1000.0) >= 0.1 )
			{ res = true; iLastTime = iNowTime; }
		
		return res;
	}
	
	private void addChatLine( String sLine )
	{
		alLines.add( sLine );
		if ( alLines.size() > 3 )
			alLines.remove( 0 );
		
		String sBlock = "";
		
		for (int i = 0; i<alLines.size(); i++)
		{
			sBlock += (String)alLines.get(i);
		}
		
		jChat.setText( sBlock );
	}
	
	public int getModalResult()
	{
		return iModalResult;
	}
	
	public Dimension getPlayFieldSize()
	{
		return jLabelTable.getSize();
	}
	
	public void setPhysycs( PPPhysycalObjectState obj )
	{
		ppObj = obj;
	}

	private boolean locationInRange( int iX, int iY )
	{
		
		int iCx, iCy, iR;
		
		iCx = (jLabelTable.getWidth() / 2);
		iCy = jLabelTable.getHeight() / 2;
		
		iR  = iCy;  
		
		if ( (iR*iR) < ( Math.pow(iCx - iX, 2) + Math.pow(iCy - iY, 2) ) )
			return true; else return false;		
	}
	
	private int locationRange( int iX, int iY )
	{
		int iCx, iCy, iR;
		
		iCx = (jLabelTable.getWidth() / 2);
		iCy = jLabelTable.getHeight() / 2;
		
		iR  = iCy;
		
		//xi = (r^2 - (y-y0)^2)^(1/2) + x0;
		
		return (int)(Math.sqrt((iR*iR) - Math.pow(iCy - iY, 2)) + iCx);	
	}	
	
	public PPBall getGameBall()
	{
		return jBall;
	}
	
	public PPSlapper getSlapper()
	{
		return jSlapperA;
	}
	
	private void initGUI() {
		try {
			{
				jLabelTable = new PPPlayTable( 400, 250 );
				getContentPane().add(jLabelTable);
				jLabelTable.setLocation(70, 50);				
			}
			{
				jPlayerOppLeft = new JLabel();
				getContentPane().add(jPlayerOppLeft);
				jPlayerOppLeft.setText("Player1");
				jPlayerOppLeft.setBounds(154, 28, 112, 21);
				jPlayerOppLeft.setFont(new java.awt.Font("Tahoma",1,12));
				jPlayerOppLeft.setForeground(new java.awt.Color(0,128,0));
			}
			{
				jPlayerOppRight = new JLabel();
				getContentPane().add(jPlayerOppRight);
				jPlayerOppRight.setText("Player2");
				jPlayerOppRight.setBounds(357, 28, 112, 21);
				jPlayerOppRight.setFont(new java.awt.Font("Tahoma",1,12));
				jPlayerOppRight.setForeground(new java.awt.Color(0,0,255));
			}
			{
				jLblChat = new JLabel();
				getContentPane().add(jLblChat);
				jLblChat.setText("Messages:");
				jLblChat.setBounds(7, 315, 63, 14);
				jLblChat.setForeground(new java.awt.Color(0,64,64));
				jLblChat.setFont(new java.awt.Font("Tahoma",1,12));
			}
			{
				jMessage = new JTextField();
				getContentPane().add(jMessage);
				jMessage.setBounds(7, 399, 385, 21);
				jMessage.addKeyListener(new KeyAdapter() {
					public void keyTyped(KeyEvent evt) {
						jMessageKeyTyped(evt);
					}
				});
			}
			{
				jSend = new JButton();
				getContentPane().add(jSend);
				jSend.setBounds(399, 399, 63, 21);
				jSend.setText("Send");
				jSend.setForeground(new java.awt.Color(0,0,255));
				jSend.setFont(new java.awt.Font("Tahoma",1,11));
				jSend.addActionListener(new ActionListener() {
					public void actionPerformed(ActionEvent evt) {
						jSendActionPerformed(evt);
					}
				});
			}
			{
				jMainMenu = new JButton();
				getContentPane().add(jMainMenu);
				jMainMenu.setText("Exit to Server Menu");
				jMainMenu.setBounds(399, 350, 154, 21);
				jMainMenu.setFont(new java.awt.Font("Tahoma",1,11));
				jMainMenu.setForeground(new java.awt.Color(128,0,64));
				jMainMenu.addActionListener(new ActionListener() {
					public void actionPerformed(ActionEvent evt) {
						jMainMenuActionPerformed(evt);
					}
				});
			}
			{
				jExit = new JButton();
				getContentPane().add(jExit);
				jExit.setText("Exit Game");
				jExit.setBounds(399, 329, 154, 21);
				jExit.setFont(new java.awt.Font("Tahoma",1,11));
				jExit.setForeground(new java.awt.Color(255,0,0));
				jExit.addActionListener(new ActionListener() {
					public void actionPerformed(ActionEvent evt) {
						jExitActionPerformed(evt);
					}
				});
			}
			{
				jChat = new JTextPane();
				getContentPane().add(jChat);
				jChat.setBounds(7, 329, 385, 63);
				jChat.setBorder(new SoftBevelBorder(BevelBorder.LOWERED, null, null, null, null));
				jChat.setEditable( false );
				jChat.setEditorKit( new HTMLEditorKit() );
				
			}
			{
				getContentPane().setLayout(null);
				getContentPane().setBackground(new java.awt.Color(192,192,192));
				this.setTitle("Ping Pong");
			}
			{
				this.setSize(568, 461);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public void mouseClicked(MouseEvent arg0) {}

	public void mousePressed(MouseEvent arg0) {}

	public void mouseReleased(MouseEvent arg0) {}

	public void mouseEntered(MouseEvent arg0) {}

	public void mouseExited(MouseEvent arg0) {}

	public void mouseDragged(MouseEvent arg0) 
	{
		mouseMoved( arg0 );
	}

	public void mouseMoved(MouseEvent arg0) 
	{
		int iNewX, iNewY;
		final int iDepl = 5;
		

		if ( jSlapperA.isRight() )
		{
			if ( arg0.getX() < ( jLabelTable.getWidth() / 2) ) return;
		} else
		{
			if ( arg0.getX() > ( jLabelTable.getWidth() / 2) ) return;
		}
		
		iNewX = (arg0.getX() - (int)((jSlapperA.getWidth() - 1) / 2.0 ));
		iNewY = (arg0.getY() - (int)((jSlapperA.getHeight() - 1) / 2.0 ));
				
		if ( ( (jLabelTable.getWidth() - iNewX) < inObjDepl) && (jSlapperA.isRight()) ) return;
		if ( ( iNewX < inObjDepl) && (!jSlapperA.isRight()) ) return;
		
		if ( (iNewY < iDepl) ) return;
		if ( (iNewY + (jSlapperA.getHeight())) > (jLabelTable.getHeight() - iDepl) ) return;
		
		if ( !locationInRange( arg0.getX(), arg0.getY() ) )
		{
			iNewX = locationRange( arg0.getX(), arg0.getY() );
			
			if ( !jSlapperA.isRight() )
				iNewX = jLabelTable.getWidth() - iNewX;							
		}
		
		if ( shouldSendUpdate() )
		{
			ppConn.sendSlapperCoordUpdate(iNewX, iNewY);
		}
			
		
		jSlapperA.setDelayedLocation( 
				iNewX, 
				iNewY
				);
		
		if ( jSlapperA.ballInMoveRange( new Point( (int)ppObj.getX(), (int)ppObj.getY() ) ) )
		{
			ppObj.setXSpeed( 0 );
		}
		
		ppObj.checkSlap();
	}
	
	private void jExitActionPerformed(ActionEvent evt) 
	{
		iModalResult = PPLoginFrame.mrCancel;
		setVisible( false );
	}
	
	private void jMainMenuActionPerformed(ActionEvent evt) 
	{
		// Leave table :P
		iModalResult = PPLoginFrame.mrOK;
		setVisible( false );
	}

	public void processSync( GamePacket gp )
	{
		if ( gp.getOpcode() == GamePacket.PKT_DCONN && isVisible() )
		{						
			return;
		}
		
		if ( gp.getOpcode() == GamePacket.PKT_CHAT )
		{
			byte iType = gp.getByte();
			String c = "";
			String sFrom = "";
			String sTxt = gp.getString();
			
			if ( iType == GamePacket.CHAT_NOTICE )
			{
				sFrom = "Notice:";
				c     = "red";
			}
			
			if ( iType == GamePacket.CHAT_SYNC )
			{
				sFrom = "Game:";
				c     = "maroon";
			}
			
			if ( iType == GamePacket.CHAT_CLIENT )
			{
				if ( jSlapperA.isRight() )
				{
					c = "green";
					sFrom = jPlayerOppLeft.getText() + ":";
				} else
				{
					c = "blue";
					sFrom = jPlayerOppRight.getText() + ":";
				}
			}	
			
			if ( iType == GamePacket.CHAT_BACK )
			{
				if ( jSlapperA.isRight() )
				{
					c = "blue";
					sFrom = jPlayerOppRight.getText() + ":";
				} else
				{
					c = "green";
					sFrom = jPlayerOppLeft.getText() + ":";
				}
			}				
						
			addChatLine( "<font color="+c+">" + sFrom + " " + sTxt + "</font><br>" );
			
			return;
		}
		
		if ( gp.getOpcode() == GamePacket.PKT_READY )
		{
			ppObj.setX( jLabelTable.getWidth() / 2 );
			ppObj.setY( jLabelTable.getHeight() / 2 );
			
			ppObj.setXSpeed( gp.getInt() );
			ppObj.setYSpeed( gp.getInt() );
			
			ppObj.setXAcceleration( gp.getDouble() );
			ppObj.setYAcceleration( gp.getDouble() );
			
			jBall.setLocation( jLabelTable.getWidth() / 2, jLabelTable.getHeight() / 2 );
		}
		
		if (gp.getOpcode() == GamePacket.PKT_HIT)
		{				
			ppObj.setX( jLabelTable.getWidth() / 2 );
			ppObj.setY( jLabelTable.getHeight() / 2 );
			
			ppObj.setXSpeed( 0 );
			ppObj.setYSpeed( 0 );
			
			ppObj.setXAcceleration( 0 );
			ppObj.setYAcceleration( 0 );
			
			jBall.setLocation( jLabelTable.getWidth() / 2, jLabelTable.getHeight() / 2 );
			
			if ( gp.getByte() == GamePacket.TYPE_ERROR_OK )
				ppConn.sendReady();
		}		
		
		if ( gp.getOpcode() == GamePacket.PKT_SYNC )
		{
			byte iType = gp.getByte();
			
			if (iType == GamePacket.SYNC_OPP_BALL)
			{
				double fX = gp.getDouble();
				double fY = gp.getDouble();
				double fSX = gp.getDouble();
				double fSY = gp.getDouble();
				double fAX = gp.getDouble();
				double fAY = gp.getDouble();
							
				ppObj.updatePh(fX, fY, fSX, fSY, fAX, fAY);				
			}
			
			if (iType == GamePacket.SYNC_OPP_NAME)
			{
				if ( jSlapperA.isRight() )
				{
					jPlayerOppLeft.setText( gp.getString() );
					jPlayerOppRight.setText( ppConn.getUser() );
				} else
				{
					jPlayerOppRight.setText( gp.getString() );
					jPlayerOppLeft.setText( ppConn.getUser() );
				}
				
				return;
			}			
		
			if (iType == GamePacket.SYNC_OPP_SLP)
			{	
				int iX = gp.getInt();
				int iY = gp.getInt();
				
				jSlapperO.setLocation( iX, iY );
				
				return;
			}							
			
		}
	}
	
	private void jSendActionPerformed(ActionEvent evt) 
	{
		String sMsg = jMessage.getText();
		
		if ( sMsg.length() > 500 || sMsg.length() == 0 )
			return;
		
		jMessage.setText( "" );
		ppConn.sendChat( sMsg );
	}
	
	private void jMessageKeyTyped(KeyEvent evt) 
	{
		char cCode = evt.getKeyChar();
		
		if ( cCode == '\n' )
			jSendActionPerformed( null );
	}
}
