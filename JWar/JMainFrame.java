import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import java.io.IOException;

import javax.swing.JFrame;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JScrollPane;
import javax.swing.UIManager;

import Objects.JWarTerrainObject;
import UI.JBattleField;
import UI.JWarAbout;
import UI.JWarDrawing;
import UI.JWarHelp;
import UI.JWarOptions;
import UI.JWarUnitInfoPanel;
import Utils.JWarTickGenerator;

/**
 * Main Class. Provides the visual Interface along with the main method.
 * @author Ciobanu Alexander
 *
 */
public final class JMainFrame extends JFrame implements ActionListener, WindowListener 
{	

    private int intSelectedWidth   = JWarOptions.MIN_GAME_WIDTH; 
	private int intSelectedHeight  = JWarOptions.MIN_GAME_HEIGHT;
	
    private JMenuBar    mbMain = new JMenuBar();
    private JMenu       mnFile = new JMenu();
    private JMenu       mnGame = new JMenu();
    private JMenu       mnHelp = new JMenu();
    private JMenuItem   miFileExit    = new JMenuItem();
    private JMenuItem   miFileOptions = new JMenuItem();
    private JMenuItem   miGameRandom = new JMenuItem();
    private JMenuItem   miHelpIndex  = new JMenuItem();
    private JMenuItem   miHelpAbout  = new JMenuItem();
	
	/* Will initialize later. */
    private JWarDrawing drwPanel;
    private JScrollPane scpPanel;
    
    private JWarUnitInfoPanel pnlUnits = new JWarUnitInfoPanel();
	
    /**
     * Main Constructor. No parameters required it will select them on it's own.
     *
     */
	public JMainFrame()
	{
		/*
		 * Frame Creation.
		 */
		
		setTitle( "JWar PA2 Project" );		
		setBounds( 200, 100, 400, 500 );
		setLayout( new BorderLayout() ); 
		
		/*
		 * Adding components to the Frame and setting their properties
		 * and captions. 
		 */
		
		mnFile.setText( "File" );
		mnFile.setToolTipText( "File and program options." );
		
		mnGame.setText( "Game" );
		mnGame.setToolTipText( "Game options and functions." );
		
		mnHelp.setText( "Help" );
		mnHelp.setToolTipText( "Help provider." );
		
		
		// ====================================
		
		miFileExit.setText( "Exit" );
		miFileExit.setToolTipText( "Exits thie project." );
		
		miFileOptions.setText( "Options ..." );
		miFileOptions.setToolTipText( "Let's you set the current options." );
		
		miGameRandom.setText( "Random" );
		miGameRandom.setToolTipText( "Generates a new random map." );
		
		miHelpIndex.setText( "Index" );
		miHelpIndex.setToolTipText( "Displays game help." );
		
		miHelpAbout.setText( "About" );
		miHelpAbout.setToolTipText( "Displays an about form." );
		
		// =======================================
		
		mnFile.add( miFileOptions );
		mnFile.addSeparator();
		mnFile.add( miFileExit );
		
		mnGame.add( miGameRandom );
		
		mnHelp.add( miHelpIndex );
		mnHelp.add( miHelpAbout );
				
		mbMain.add( mnFile );
		mbMain.add( mnGame );
		mbMain.add( mnHelp );
		
		// ******************************************
		
		miFileOptions.addActionListener( this );
        miFileExit.addActionListener( this );
        miGameRandom.addActionListener( this );
        miHelpAbout.addActionListener( this );
        miHelpIndex.addActionListener( this );
        
        pnlUnits.setPreferredSize( new Dimension( getWidth(), 100 ));
				
		
		// Add the main game/drawing area.
		
		drwPanel = new JWarDrawing( intSelectedWidth, intSelectedHeight );        
        scpPanel = new JScrollPane( drwPanel );

        // Configure Scroll Options 
        
        scpPanel.getVerticalScrollBar().setUnitIncrement( JBattleField.CELL_DIM );
        scpPanel.getHorizontalScrollBar().setUnitIncrement( JBattleField.CELL_DIM );
        
        // Adding Controls to the Main Frame
        add(scpPanel, BorderLayout.CENTER);
        add( pnlUnits, BorderLayout.SOUTH );
       
		setJMenuBar( mbMain );
        addWindowListener( this );
        
        /* Populate initial grid */
        drwPanel.getBattleField().populateGrid();
	}
	
    /*
     *  (non-Javadoc)
     * @see java.awt.event.ActionListener#actionPerformed(java.awt.event.ActionEvent)
     */
	public void actionPerformed(ActionEvent arg0) 
	{
		if ( arg0.getSource() == miFileOptions )
		{
            /*
             * Option Dialog 
             */
            
			JWarOptions joptns = new JWarOptions( intSelectedWidth, intSelectedHeight );			
			joptns.setVisible( true );
			
			intSelectedWidth = joptns.getSelectedWidth();
			intSelectedHeight = joptns.getSelectedHeight();
            
            drwPanel.resizeField( intSelectedWidth, intSelectedHeight );
            drwPanel.getBattleField().populateGrid();
		}
        
        if ( arg0.getSource() == miGameRandom )
        {
            /* Do new map generation! */
            drwPanel.getBattleField().populateGrid();
        }        
        
        if ( arg0.getSource() == miFileExit )
        {
            /*
             * Exit Application!
             */
            
            JWarTickGenerator.GlobalTicks.safeStop();
            System.exit( 0 );
        }
        
        if ( arg0.getSource() == miHelpAbout )
        {
            JWarAbout abt = new JWarAbout();            
            abt.setVisible( true );
        }
        
        if ( arg0.getSource() == miHelpIndex )
        {
            
            JWarHelp hlp;
            
            try 
            {
                hlp = new JWarHelp();
                hlp.setVisible( true );
            } catch (IOException e) 
            {
               
            }
            
        }         
	}

    /**
     * Selects the current OS native look&feel for the swing UI.
     *
     */
    public static void setNativeLookAndFeel() 
    {
        try {
          UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
        } catch(Exception e) {}
      }

    /**
     * Main program entry point.
     * @param args Command Line parameters.
     */
	public static void main(String[] args) 
    {
		
		/*
		 * Creation of the Main UI Frame. And setting it visible.
		 */
        
        setNativeLookAndFeel();
        
        /* Instantiate all static data */
        try
        {        
            JWarTerrainObject.loadTerrainInfo();
        } catch ( Exception e )
        {
            /* Program startup error! */
            return;
        }
        
        JWarTickGenerator.GlobalTicks = new JWarTickGenerator( JWarTickGenerator.RECOMMENDED_TIME );
        JWarTickGenerator.GlobalTicks.start();
        
		JMainFrame jmain = new JMainFrame();
		jmain.setVisible( true );
	}

    /*
     *  (non-Javadoc)
     * @see java.awt.event.WindowListener#windowOpened(java.awt.event.WindowEvent)
     */
    public void windowOpened(WindowEvent arg0) {}    
    /*
     *  (non-Javadoc)
     * @see java.awt.event.WindowListener#windowClosing(java.awt.event.WindowEvent)
     */
    public void windowClosing(WindowEvent arg0) 
    {
        JWarTickGenerator.GlobalTicks.safeStop();
        System.exit( 0 );
    }
    /*
     *  (non-Javadoc)
     * @see java.awt.event.WindowListener#windowClosed(java.awt.event.WindowEvent)
     */
    public void windowClosed(WindowEvent arg0) {}
    /*
     *  (non-Javadoc)
     * @see java.awt.event.WindowListener#windowIconified(java.awt.event.WindowEvent)
     */
    public void windowIconified(WindowEvent arg0) {}
    /*
     *  (non-Javadoc)
     * @see java.awt.event.WindowListener#windowDeiconified(java.awt.event.WindowEvent)
     */
    public void windowDeiconified(WindowEvent arg0) {}
    /*
     *  (non-Javadoc)
     * @see java.awt.event.WindowListener#windowActivated(java.awt.event.WindowEvent)
     */
    public void windowActivated(WindowEvent arg0) {}
    /*
     *  (non-Javadoc)
     * @see java.awt.event.WindowListener#windowDeactivated(java.awt.event.WindowEvent)
     */
    public void windowDeactivated(WindowEvent arg0) {}

}
