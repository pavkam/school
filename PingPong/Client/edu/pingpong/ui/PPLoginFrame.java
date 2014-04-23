package edu.pingpong.ui;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPasswordField;
import javax.swing.JTextField;
import java.util.Properties;
import java.io.FileInputStream;
import java.io.FileOutputStream;

public class PPLoginFrame extends JFrame implements KeyListener, WindowListener
{
	static public int mrAccept = 2;
	static public int mrOK = 1;
	static public int mrCancel = 0;
	
	private String sSettingsFile = "data/properties.ini";
	
	private JLabel jLabelUser;
	private JTextField jTextFieldUser;
	private JTextField jTextFieldServer;
	private JLabel jLabelServer;
	private JButton jButtonCancel;
	private JButton jButtonOK;
	private JPasswordField jPasswordField;
	private JLabel jLabelPass;
	
	private Properties ppSaved;
	
	private int iModalResult;
	
	public PPLoginFrame()
	{
		initGUI();
			
		iModalResult = mrCancel;
		
		/* Load saved properties */
		
		ppSaved = new Properties();
		
		try
		{
			FileInputStream fIn = new FileInputStream( sSettingsFile ); 
			ppSaved.load( fIn );
			fIn.close();
		} catch ( Exception e )
		{
			// Unable to load the file :(
			return; 
		}
		
		setLocation(
				(int)Double.parseDouble( ppSaved.getProperty( "pp.client.x", "300" ) ),
				(int)Double.parseDouble( ppSaved.getProperty( "pp.client.y", "300" ) )
		);
		
		jTextFieldUser.setText( ppSaved.getProperty( "pp.client.user", "" ) );
		jTextFieldServer.setText( ppSaved.getProperty( "pp.client.server", "" ) );
		jPasswordField.setText( ppSaved.getProperty( "pp.client.pass", "" ) );	
		
				
		/* *****/
		ifaceControls();
		
		setResizable( false );
		setVisible( true );
	}
	
	public String getUser()
	{
		return jTextFieldUser.getText();
	}
	
	public String getHost()
	{
		return jTextFieldServer.getText();
	}	
	
	public String getPassword()
	{
		return String.valueOf( jPasswordField.getPassword() );
	}		
	
	private void ifaceControls()
	{
		boolean bAllCompl =
			(
			 (jTextFieldServer.getText().length() != 0) &&
			 (jPasswordField.getPassword().length != 0) &&
			 (jTextFieldUser.getText().length() != 0)
			 );
		
		jButtonOK.setEnabled( bAllCompl );
	}
	
	public int getModalResult()
	{
		return iModalResult;
	}

	private void initGUI() {
		try {
			{
				jLabelUser = new JLabel();
				getContentPane().add(jLabelUser);
				jLabelUser.setText("Username:");
				jLabelUser.setBounds(7, 7, 63, 14);
				jLabelUser.setForeground(new java.awt.Color(0,128,0));
				jLabelUser.setFont(new java.awt.Font("Tahoma",1,11));				
			}
			{
				jLabelPass = new JLabel();
				getContentPane().add(jLabelPass);
				jLabelPass.setText("Password:");
				jLabelPass.setFont(new java.awt.Font("Tahoma",1,11));
				jLabelPass.setForeground(new java.awt.Color(128,0,0));
				jLabelPass.setBounds(7, 49, 63, 14);
			}
			{
				jTextFieldUser = new JTextField();
				getContentPane().add(jTextFieldUser);
				jTextFieldUser.setBounds(7, 21, 266, 21);
				jTextFieldUser.addKeyListener( this );
			}
			{
				jPasswordField = new JPasswordField();
				getContentPane().add(jPasswordField);
				jPasswordField.setBounds(7, 63, 266, 21);
				jPasswordField.addKeyListener( this );
			}
			{
				jButtonOK = new JButton();
				getContentPane().add(jButtonOK);
				jButtonOK.setText("Accept");
				jButtonOK.setBounds(182, 161, 91, 28);
				jButtonOK.setFont(new java.awt.Font("Tahoma",1,11));
				jButtonOK.addActionListener(new ActionListener() {
					
					public void actionPerformed(ActionEvent evt) {
						iModalResult = mrOK;
						windowClosing(null); 
						setVisible( false );
					}
					
				});
			}
			{
				jButtonCancel = new JButton();
				getContentPane().add(jButtonCancel);
				jButtonCancel.setText("Cancel");
				jButtonCancel.setBounds(84, 161, 91, 28);
				jButtonCancel.addActionListener(new ActionListener() {
					public void actionPerformed(ActionEvent evt) {
						windowClosing(null);
						setVisible( false );
					}
				});
			}
			{
				jLabelServer = new JLabel();
				getContentPane().add(jLabelServer);
				jLabelServer.setText("Server Address:");
				jLabelServer.setFont(new java.awt.Font("Tahoma",1,11));
				jLabelServer.setForeground(new java.awt.Color(0,0,255));
				jLabelServer.setBounds(7, 91, 105, 14);
			}
			{
				jTextFieldServer = new JTextField();
				getContentPane().add(jTextFieldServer);
				jTextFieldServer.setBounds(7, 105, 266, 21);
				jTextFieldServer.addKeyListener( this );
			}
			{				
				this.setTitle("Ping Pong Login Screen");				
				getContentPane().setLayout(null);
				this.addWindowListener( this );
			}
			{
				this.setSize(288, 230);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public void keyTyped(KeyEvent arg0)
	{
		ifaceControls();
	}

	public void keyPressed(KeyEvent arg0) 
	{}

	public void keyReleased(KeyEvent arg0)
	{}

	public void windowOpened(WindowEvent arg0) {}

	public void windowClosing(WindowEvent arg0) 
	{		
		ppSaved.put( "pp.client.x", String.valueOf( getLocation().getX() ) );
		ppSaved.put( "pp.client.y", String.valueOf( getLocation().getY() ) );
		
		ppSaved.put( "pp.client.user", jTextFieldUser.getText() );
		ppSaved.put( "pp.client.server", jTextFieldServer.getText() );
		ppSaved.put( "pp.client.pass", String.valueOf( jPasswordField.getPassword() ) );
								
		try
		{
			FileOutputStream fOut = new FileOutputStream( sSettingsFile );
			ppSaved.store( fOut, "--" );
			fOut.close();
			
		} catch ( Exception e )
		{
			// Unable to save the file :(
			return; 
		}		
	}

	public void windowClosed(WindowEvent arg0) {}
	public void windowIconified(WindowEvent arg0) {}
	public void windowDeiconified(WindowEvent arg0) {}
	public void windowActivated(WindowEvent arg0) {}
	public void windowDeactivated(WindowEvent arg0) {}

}
