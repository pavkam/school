package Objects;

import java.awt.Color;

import AI.BuilderMachine;
import AI.JAIBuilder;
import AI.JAIGeneric;

public class JWarBuilderMachine extends JWarLifeObject implements BuilderMachine
{

    static public int MAX_LIFE = 50;
    static public int COST = 10;
    static public int MAX_CARRY = 20;
    public static int LIFE_PER_TICK = 1;
    
    private JWarInfoBar ibMinerals;
    
    public void setupObject()
    {
        super.setupObject();
        
        getStates().addObjectState( "builder00", JWarDynamicObject.OBJECT_STATE_DEFAULT );
        getStates().addObjectState( "builder", JWarDynamicObject.OBJECT_STATE_IMAGE );
        
        getStates().addObjectState( "builder_up", JAIGeneric.AI_STATE_UP );
        getStates().addObjectState( "builder_dw", JAIGeneric.AI_STATE_DOWN );
        getStates().addObjectState( "builder_rt", JAIGeneric.AI_STATE_RIGHT );
        getStates().addObjectState( "builder_lt", JAIGeneric.AI_STATE_LEFT );
        getStates().addObjectState( "builder_bb", JAIBuilder.AI_STATE_BUILD );
        
        
        // Let's add our info-bars for this type of object //
        ibMinerals = new JWarInfoBar( Color.GREEN, MAX_CARRY, 0, JWarInfoBar.INFO_TYPE_MINERALS );
        addInfoBar( ibMinerals );
        
        setObjectLife( MAX_LIFE );
        setObjectMaxLife( MAX_LIFE );
        
        setAIModule( new JAIBuilder( this ) );
    }
    
    public String getObjectName()
    {
        return "Builder";
    }

    public int getObjectPredefinedMaxLife()
    {
        return MAX_LIFE;
    }
    
    public int getFreeMineralSlots()
    {
        return ibMinerals.getMaxBarValue() - ibMinerals.getBarValue();
    }
    
    public boolean isMachineReadyForMining()
    {
        return (ibMinerals.getBarValue() == 0);
    }    
    
    public void addMineralSlots( int iCount )
    {
        int iNow = ibMinerals.getBarValue();
        iNow += iCount;
        
        if ( iNow > ibMinerals.getMaxBarValue() )
            iNow = ibMinerals.getMaxBarValue();
        
        ibMinerals.setBarValue( iNow );
        repaint();
    }
    
    public int removeMineralSlots( int iCount )
    {
        int iNow = ibMinerals.getBarValue();
        iNow -= iCount;
        
        if ( iNow < 0 )
            iNow = 0;
            
        ibMinerals.setBarValue( iNow );
        repaint();
        
        if ( iNow < 0 )
            return iCount + iNow; else
            return iCount;
        
    }    
}
