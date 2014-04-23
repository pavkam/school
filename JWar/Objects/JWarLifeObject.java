package Objects;

import java.awt.Color;

/**
 * This class is the base for all Life possesing objects.
 * @author Ciobanu Alexander
 *
 */
public class JWarLifeObject extends JWarDynamicObject 
{
    private JWarInfoBar ibLife;
    
    static public int OBJECT_STATE_ATTACKED = JWarDynamicObject.OBJECT_STATE_PREV;
    static public int OBJECT_STATE_KILLED = JWarDynamicObject.OBJECT_STATE_PREV - 1;
    static public int OBJECT_STATE_PREV = JWarDynamicObject.OBJECT_STATE_PREV - 2;
    
    public void setupObject()
    {
        getStates().addObjectState( "attacked", OBJECT_STATE_ATTACKED );
        getStates().addObjectState( "killed", OBJECT_STATE_KILLED );
        
        ibLife = new JWarInfoBar( Color.YELLOW, 0, 0, JWarInfoBar.INFO_TYPE_LIFE );
        addInfoBar( ibLife );
    }
    
    /**
     * Returns Object's Life.
     * @return
     */
    public int getObjectLife()
    {
        return ibLife.getBarValue();
    }

    /**
     * Returns Object's Max Life.
     * @return
     */
    public int getObjectMaxLife()
    {
        return ibLife.getMaxBarValue();
    }
    
    /**
     * Sets Objects's Max Life.
     * @param iMax
     */
    public void setObjectMaxLife( int iMax )
    {
        ibLife.setMaxBarValue( iMax );
        repaint();
    }    
    
    /**
     * Sets Object's Life.
     * @param iLife
     */
    public void setObjectLife( int iLife )
    {
        ibLife.setBarValue( iLife );
        repaint();
    }
    
    /**
     * Gets predefined object's Life.
     * @return
     */
    public int getObjectPredefinedMaxLife()
    {
        return 0;
    }
    
    /**
     * Removes some life from an object and destroys it if necessary
     * @param iCount
     */
    public boolean extractLife( int iCount )
    {
        int iNow = ibLife.getBarValue();        
        iNow -= iCount;
        
        if ( iNow <= 0 )
        {
            ibLife.setBarValue( 0 );            
            return true;
        };

        
        ibLife.setBarValue( iNow );
        
        repaint();
        return false;
    }
}
