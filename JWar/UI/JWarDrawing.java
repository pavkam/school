package UI;
import java.awt.BorderLayout;
import java.awt.Dimension;

import javax.swing.JPanel;

/**
 * The Drawing container. Contains the JBattleField object.
 * This class is just a plain container and doesn't provide any
 * functionality.
 *   
 * @author Ciobanu Alexander
 *
 */
public final class JWarDrawing extends JPanel
{ 
    private JBattleField drwBattle  = new JBattleField();

    /**
     * Resizes this container to the new given size.
     * Automatically will resize the contained component also.
     * 
     * @param iWidth New given Width in Cell Units.
     * @param iHeight New given Height in Cell Units. 
     */
    public void resizeField( int iWidth, int iHeight )
    {
        drwBattle.setFieldSizes( iWidth, iHeight );
                
        setBounds( 0, 0, (iWidth * JBattleField.CELL_DIM) + 1, (iHeight * JBattleField.CELL_DIM) + 1 );
        drwBattle.setBounds( 0, 0, getWidth(), getHeight() );
        
        setPreferredSize( new Dimension( getWidth(), getHeight() ) );
    }
	    
    /**
     * Generic constructor. Accepts the Width and Height of the
     * Field in Cell Units.
     * 
     * @param iWidth Given Width in Cell Units.
     * @param iHeight Given Height in Cell Units.
     */
	public JWarDrawing( int iWidth, int iHeight )
	{
        resizeField( iWidth, iHeight );
        
        setAutoscrolls( true );                                
        setLayout( new BorderLayout() );        
        add( drwBattle, BorderLayout.CENTER );                     
	}
    
    /**
     * Returns the currently used JBattleField object.
     * @return JBattleField object contained in this container.
     */
    public JBattleField getBattleField()
    {
        return drwBattle;
    }

}
