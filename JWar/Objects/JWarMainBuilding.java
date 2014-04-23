package Objects;

import AI.JAIMainBuilding;
import AI.MainBuilding;

public class JWarMainBuilding extends JWarTrainerObject implements MainBuilding {

    
    static public int OBJECT_STATE_ATTACKED = JWarTrainerObject.OBJECT_STATE_PREV;
    static public int OBJECT_STATE_TRAIN_BUILDER = JWarTrainerObject.OBJECT_STATE_PREV - 1;
    static public int OBJECT_STATE_TRAIN_WAR = JWarTrainerObject.OBJECT_STATE_PREV - 2;
    
    
    public static int MAX_LIFE = 50;
    public static int COST     = 800;
    public static int LIFE_PER_TICK = 1;
       
    public void setupObject()
    {
        super.setupObject();
        
        getStates().addObjectState( "mbd00", JWarDynamicObject.OBJECT_STATE_DEFAULT );
        getStates().addObjectState( "mbd_bb", OBJECT_STATE_TRAIN_BUILDER );
        getStates().addObjectState( "mbd_bk", OBJECT_STATE_TRAIN_WAR );
        getStates().addObjectState( "mbd", JWarDynamicObject.OBJECT_STATE_IMAGE );
              
        setObjectLife( MAX_LIFE );
        setObjectMaxLife( MAX_LIFE );
        
        setAIModule( new JAIMainBuilding( this ) );
    }
    
    public String getObjectName()
    {
        return "Main Building";
    }    
 
    public int getObjectPredefinedMaxLife()
    {
        return MAX_LIFE;
    }
}
