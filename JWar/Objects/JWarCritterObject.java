package Objects;

import AI.JAICritter;
import AI.JAIGeneric;
import AI.JWarParty;

/**
 * Critter Class. Just roaming the fairy lands of JWar :)
 * @author Ciobanu Alexander
 *
 */
public class JWarCritterObject extends JWarLifeObject {

    static public int MAX_LIFE = 10;
    
    public void setupObject()
    {
        super.setupObject();
        
        setParty( JWarParty.NEUTRAL );
        
        getStates().addObjectState( "critter00", JWarDynamicObject.OBJECT_STATE_DEFAULT );
        getStates().addObjectState( "critter", JWarDynamicObject.OBJECT_STATE_IMAGE );
        
        getStates().addObjectState( "critter_up", JAIGeneric.AI_STATE_UP );
        getStates().addObjectState( "critter_dw", JAIGeneric.AI_STATE_DOWN );
        getStates().addObjectState( "critter_rt", JAIGeneric.AI_STATE_RIGHT );
        getStates().addObjectState( "critter_lt", JAIGeneric.AI_STATE_LEFT );
        
        // Let's add our info-bars for this type of object //
        
        setObjectLife( MAX_LIFE );
        setObjectMaxLife( MAX_LIFE );
        
        setAIModule( new JAICritter( this ) );
    }
    
    public String getObjectName()
    {
        return "Bunny :)";
    }
        
    public int getObjectPredefinedMaxLife()
    {
        return MAX_LIFE;
    }
}
