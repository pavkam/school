package Objects;

import AI.JAIBuilder;
import AI.JAIGeneric;
import AI.JAIKiller;
import AI.WarMachine;

public class JWarKillerMachine extends JWarLifeObject implements WarMachine 
{

    static public int MAX_LIFE = 50;
    static public int COST = 10;
    public static int LIFE_PER_TICK = 1;
    
    public void setupObject()
    {
        super.setupObject();
        
        getStates().addObjectState( "killer00", JWarDynamicObject.OBJECT_STATE_DEFAULT );
        getStates().addObjectState( "killer", JWarDynamicObject.OBJECT_STATE_IMAGE );
        
        getStates().addObjectState( "killer_up", JAIGeneric.AI_STATE_UP );
        getStates().addObjectState( "killer_dw", JAIGeneric.AI_STATE_DOWN );
        getStates().addObjectState( "killer_rt", JAIGeneric.AI_STATE_RIGHT );
        getStates().addObjectState( "killer_lt", JAIGeneric.AI_STATE_LEFT );
        getStates().addObjectState( "killer_bb", JAIBuilder.AI_STATE_BUILD );
        
        setObjectLife( MAX_LIFE );
        setObjectMaxLife( MAX_LIFE );
        
        setAIModule( new JAIKiller( this ) );
    }
    
    public String getObjectName()
    {
        return "Killer";
    }

    public int getObjectPredefinedMaxLife()
    {
        return MAX_LIFE;
    }

    public int getShootDistance() 
    {
        return 3;
    }

    public int getParalyseTiming() 
    {
        return 1;
    }

    public int getHomePatrolMaxDistance() 
    {
        return 5;
    }

    public int getMinimalProtectionDistance() 
    {
        return 5;
    }

    public int getMaximalHuntSelectDistance() 
    {
        return 5;
    }

    public int getDamagePerShot() 
    {
        return 5;
    }

    public int getGraveVisibilityTiming() 
    {
        return 10;
    }

    public int getDecisionTiming() 
    {
        return 2;
    }    
    
}
