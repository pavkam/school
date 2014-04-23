package Controllers;

import Objects.JWarDynamicObject;
import UI.JBattleField;
import AI.JWarParty;

/**
 * Base class for all controllers in the game.
 * @author Ciobanu Alexander
 *
 */
public class JCTRLGeneric {
    
    private JBattleField pBtl;
    private JWarParty pParty;
    
    /**
     * Called whenever an unit is in an idle state and requires Controller's input.
     * @param dyn Dynamic Object that entered Idle state.
     */
    public void unitInputRequired( JWarDynamicObject dyn )
    {
        
    }
    
    /**
     * Called when an unit is attacked!
     * @param dyn Dynamic Object that has been attacked.
     */
    public void unitAttacked( JWarDynamicObject dyn )
    {
        
    }
    
    /**
     * Sets the JBattleField object.
     * @param btl JBattleField Object. 
     */
    public void setBattleField( JBattleField btl )
    {
        pBtl = btl;
    }
    
    /**
     * Returns the JBattleField object.
     * @return JBattleField Object
     */
    public JBattleField getBattleField()
    {
        return pBtl;
    }
    
    /**
     * Sets the party owner for this Controller.
     * @param party Party that is driven by this Controller.
     */
    public void setParty( JWarParty party )
    {
        pParty = party;
    }
    
    /**
     * Returns the owning party for this controller.
     * @return Party that has the control over this Controller.
     */
    public JWarParty getParty()
    {
        return pParty;
    }
    
    /**
     * Returns the count of a given class of objects on the field.
     * @param c The given class type to be searched in the map.
     * @param pt Party that the found objects belong to.
     * @return Number of objects found.
     */
    public int getObjectCount( Class c, JWarParty pt )
    {
        int iX, iY;
        int iW = pBtl.getFieldWidth();
        int iH = pBtl.getFieldHeight();
        int iRes = 0;
        
        for ( iX = 0; iX < iW; iX++ )
            for ( iY = 0; iY < iH; iY++ )
            {
                JWarDynamicObject o = pBtl.getDynamicObject( iX, iY );
                
                if ( c.isInstance( o) )
                {
                    if ( o.getParty() == pt)
                        iRes++;
                }
            }
        
        return iRes;
    }
}
