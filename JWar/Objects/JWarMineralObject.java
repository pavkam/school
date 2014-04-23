package Objects;

import java.awt.Color;

import AI.JWarParty;

/**
 * The Minerals in the game. Derived from Dynamic Object.
 * @author Ciobanu Alexander
 *
 */
public class JWarMineralObject extends JWarDynamicObject {
    
    static public int MAX_MINERAL_COUNT = 500;
    private JWarInfoBar ibResources;
    
    public void setupObject()
    {
        setParty( JWarParty.NEUTRAL );
        
        getStates().addObjectState( "mineral00", JWarDynamicObject.OBJECT_STATE_DEFAULT );
        getStates().addObjectState( "mineral", JWarDynamicObject.OBJECT_STATE_IMAGE );
        
        // Let's add our info-bars for this type of object //
        ibResources = new JWarInfoBar( Color.GREEN, MAX_MINERAL_COUNT, MAX_MINERAL_COUNT, JWarInfoBar.INFO_TYPE_MINERALS ); 
            
        addInfoBar( ibResources );
        setAIModule( null );
    }
    
    public String getObjectName()
    {
        return "Mineral";
    }    
    
    public int extractResources( int iCount )
    {
        int iNow = ibResources.getBarValue();        
        iNow -= iCount;
        
        if ( iNow <= 0 )
        {
            getBattleField().removeDynamicObject( this );
            
            return (iCount + iNow);
        }
        
        ibResources.setBarValue( iNow );
        
        repaint();
        return iCount;
    }
}
