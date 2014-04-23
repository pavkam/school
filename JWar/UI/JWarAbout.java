package UI;

import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JTextArea;

/**
 * About Frame. Displays information about the author of this project.
 * @author Ciobanu Alexander
 *
 */
final public class JWarAbout extends JDialog implements ActionListener 
{
    private JTextArea lbAbout = new JTextArea();
    private JButton btClose  = new JButton();
    
    /**
     * Generic Constructor
     *
     */
    public JWarAbout()
    {
        setLayout( null );
        
        setTitle( "About JWar Project" );
        setBounds( 250, 200, 300, 300 );        
        setResizable( false );          
        setModal( true );
        
        btClose.setText( "Close" );
        btClose.setBounds( (getWidth() - 100) / 2, getHeight() - 60, 100, 20 );
        btClose.addActionListener( this );
        
        lbAbout.setText( 
                "Proiect Programarea Algoritmilor 2.\n" +                
                "Proiect realizat de Ciobanu Alexandru. Grupa 1312.\n\n\n" +
                "Nume: Joc de strategie realizat in Java."
                );
        
        lbAbout.setFont( new Font( "Verdana", Font.BOLD, 10) );
        
        lbAbout.setEditable( false );
        lbAbout.setFocusable( false );
        lbAbout.setBounds( 0, 0, getWidth(), btClose.getY() - 10 );
        
        add( lbAbout ); 
        
        add( btClose );
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
