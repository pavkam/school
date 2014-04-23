package UI;

import java.awt.BorderLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.IOException;

import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JEditorPane;
import javax.swing.JOptionPane;
import javax.swing.JScrollPane;

import Utils.JWarResources;

/**
 * Help display Frame. Will be created upon selecting the
 * Show Help option in the Main Frame. 
 *  
 * @author Ciobanu Alexander
 *
 */
final public class JWarHelp extends JDialog implements ActionListener {

    private JButton btClose = new JButton();
    private JEditorPane txtHelp;
    
    /**
     * Generic Constructor.
     * @throws IOException
     */
    public JWarHelp() throws IOException
    {
        setLayout( new BorderLayout() );
        
        setTitle( "Help" );
        setBounds( 100, 100, 500, 500 );        
        setResizable( true );          
        setModal( true );
        
        btClose.setText( "Close" );
        btClose.addActionListener( this );
        
        add( btClose, BorderLayout.SOUTH );
        
        try
        {
            txtHelp = JWarResources.getHTMLPane( "help" );
            txtHelp.setEditable( false );
            
            JScrollPane sp = new JScrollPane( txtHelp );
            
            add( sp, BorderLayout.CENTER );
        } catch ( IOException e )
        {
            JOptionPane.showMessageDialog( this, "Help file not found! Please check the integrity" + 
                    " of the program installation!", 
                    "Error!", JOptionPane.ERROR_MESSAGE );
            
            throw new IOException( "Help file missing!" );           
        }

    }

    /*
     *  (non-Javadoc)
     * @see java.awt.event.ActionListener#actionPerformed(java.awt.event.ActionEvent)
     */
    public void actionPerformed(ActionEvent arg0) 
    {
        if ( arg0.getSource() == btClose )
        {
            setVisible( false );
        }
        
    }    
}
