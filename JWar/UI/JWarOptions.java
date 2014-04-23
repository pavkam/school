package UI;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.ButtonGroup;
import javax.swing.JOptionPane;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JRadioButton;
import javax.swing.JTextField;
import javax.swing.JDialog;

import AI.JWarParty;

/**
 * Option Select Dialog class. Will be created and shown upon
 * selecting "Options" menu item in the Main Frame.
 * 
 * @author Ciobanu Alexander
 *
 */
public final class JWarOptions extends JDialog implements ActionListener 
{
    /**
     * Specifies the maximal possible value for Field Width.
     */
    static public int MAX_GAME_WIDTH  = 50;
    
    /**
     * Specifies the maximal possible value for Field Height.
     */    
	static public int MAX_GAME_HEIGHT = 50;
    
    /**
     * Specifies the minimal possible value for Field Width.
     */     
    static public int MIN_GAME_WIDTH  = 25;
    
    /**
     * Specifies the minimal possible value for Field Height.
     */     
    static public int MIN_GAME_HEIGHT = 25;
    
	private JButton   btAccept = new JButton();
	private JButton   btCancel = new JButton();
	
	private JTextField edtWidth  = new JTextField();
	private JTextField edtHeight = new JTextField();
	
	private JLabel lbWidth    = new JLabel();
	private JLabel lbHeight   = new JLabel();
    private JLabel lbGameType = new JLabel();
    
    private JRadioButton rbGameNormal = new JRadioButton();
    private JRadioButton rbMelee = new JRadioButton();
    private JRadioButton rbFreeForAll = new JRadioButton();
    private JRadioButton rb2vs2 = new JRadioButton();
    private ButtonGroup  bgGameType = new ButtonGroup();
    
	private int ifOldWidth, ifOldHeight;
	
    /**
     * Returns the currently selected grid Width
     * @return Currently selected Width in Cell Units.
     */
	public int getSelectedWidth()
	{
		return ifOldWidth;
	}

    /**
     * Returns the currently selected grid Height
     * @return Currently selected Height in Cell Units.
     */
	public int getSelectedHeight()
	{
		return ifOldHeight;
	}

    /**
     * Creates a new Dialog with given grid sizes to be edited
     * and returned back to the caller.
     * 
     * @param oldWidth Old selected Width in Cell Units.
     * @param oldHeight Old selected Height in Cell Units.
     */
	public JWarOptions( int oldWidth, int oldHeight )
	{
		ifOldWidth  = oldWidth; 
		ifOldHeight = oldHeight;
		
		setLayout( null );
		
		setTitle( "JWar Configuration Pane" );
		
		btAccept.setText( "Accept" );
		btAccept.setToolTipText( "Accepts the current settings." );
		
		btCancel.setText( "Cancel" );
		btCancel.setToolTipText( "Cancels all changes and exists." );
		
		edtWidth.setText( Integer.toString( oldWidth ) );
		edtHeight.setText( Integer.toString( oldHeight ) );
				
		lbWidth.setText( "Width:" );
		lbHeight.setText( "Height:" );
		
		btAccept.setBounds( 212, 140, 80, 25 );
		btCancel.setBounds( 130, 140, 80, 25 );
		
		lbWidth.setBounds( 5, 10, 80, 20 );
		lbHeight.setBounds( 5, 55, 80, 20 );
		
		edtWidth.setBounds( 5, 30, 80, 23 );
		edtHeight.setBounds( 5, 75, 80, 23 );
		
		btAccept.addActionListener( this );
		btCancel.addActionListener( this );
		
        rbGameNormal.setText( "Normal" );
        rbMelee.setText( "Melee" );
        rbFreeForAll.setText( "Free For All" );
        rb2vs2.setText( "2 vs 2" );
        
        rbGameNormal.setBounds( 100, 30, 80, 20 );
        rbMelee.setBounds( 100, 30 + 20, 80, 20 );
        rbFreeForAll.setBounds( 100, 30 +40, 80, 20 );
        rb2vs2.setBounds( 100, 30 + 60, 80, 20 );
        
        bgGameType.add( rbGameNormal );
        bgGameType.add( rbMelee );
        bgGameType.add( rbFreeForAll );
        bgGameType.add( rb2vs2 );
          
        lbGameType.setText( "-- Game Type --");
        lbGameType.setBounds( 100, 5, 80, 20 );
        
        int iGT = JWarParty.getGameType();
        
        if ( iGT == JWarParty.GAME_TYPE_2VS2 )
            rb2vs2.setSelected( true );
        
        if ( iGT == JWarParty.GAME_TYPE_FREE )
            rbFreeForAll.setSelected( true );

        if ( iGT == JWarParty.GAME_TYPE_MELEE )
            rbMelee.setSelected( true );

        if ( iGT == JWarParty.GAME_TYPE_NORMAL )
            rbGameNormal.setSelected( true );

        add( lbGameType );
        add( rbGameNormal );
        add( rbMelee );
        add( rbFreeForAll );
        add( rb2vs2 );
        
		add( btAccept );
		add( btCancel );
		add( lbWidth );
		add( lbHeight );
		add( edtWidth );
		add( edtHeight );
		
		setBounds( 250, 200, 300, 200 );		
		setResizable( false );	
		setModal( true );		
	}

    /*
     *  (non-Javadoc)
     * @see java.awt.event.ActionListener#actionPerformed(java.awt.event.ActionEvent)
     */
	public void actionPerformed(ActionEvent arg0) {
		
		if ( arg0.getSource() == btAccept )
		{
			int iW, iH;
			
			
			
			try
			{				
				iW = Integer.parseInt( edtWidth.getText() ); 
				iH = Integer.parseInt( edtHeight.getText() );
				
				if ( ( iW < 0 ) || ( iW > MAX_GAME_WIDTH ) || ( iW < MIN_GAME_WIDTH ))
					throw new Exception();

				if ( ( iH < 0 ) || ( iH > MAX_GAME_HEIGHT ) || ( iH < MIN_GAME_HEIGHT ))
					throw new Exception();
								
			} catch (Exception e)
			{
				JOptionPane.showMessageDialog(this, "Invalid X or Y dimension supplied!");
				return;
			}

            if ( rb2vs2.isSelected() )
                JWarParty.setGameType( JWarParty.GAME_TYPE_2VS2 );

            if ( rbFreeForAll.isSelected() )
                JWarParty.setGameType( JWarParty.GAME_TYPE_FREE );

            if ( rbMelee.isSelected() )
                JWarParty.setGameType( JWarParty.GAME_TYPE_MELEE );

            if ( rbGameNormal.isSelected() )
                JWarParty.setGameType( JWarParty.GAME_TYPE_NORMAL );
            
			ifOldWidth  = iW;
			ifOldHeight = iH;
			
			setVisible( false );
		}
		
		if ( arg0.getSource() == btCancel )
		{
			setVisible( false );
		}

	}

}
