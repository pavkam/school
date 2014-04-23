package AI;

import Objects.JWarLifeObject;

public class JAIControllable extends JAIGeneric implements ControllableUnit{

    static public String GEN_STATE_INACTIVE = "Inactive. Awaiting user input.";
    static public String GEN_STATE_USR_MOVE = "Moving to requested position.";
    
    static public int AI_OP_FIND_WAY = JAIGeneric.AI_STATE_LAST;
    static public int AI_OP_CHECK_POS = JAIGeneric.AI_STATE_LAST + 1;
    
    static public int AI_STATE_LAST  = JAIGeneric.AI_STATE_LAST + 2;
    
    private JWarRouteCalculator rcMy;
    
    public JAIControllable(JWarLifeObject parent) 
    {
        super(parent);
        
        rcMy = new JWarRouteCalculator( this );
        addStateChange( GEN_STATE_INACTIVE );
    }

    public void performCustomStep( int iStep, Object o )
    {
        if ( iStep == AI_OP_FIND_WAY )
        {
            MapPosition mp = (MapPosition)o;

            rcMy.calculateRoute( getParentObject().getPositionX(),
                                 getParentObject().getPositionY(),
                                 mp.getPositionX(),
                                 mp.getPositionY(),
                                 true
                               );
        }
        
        if ( iStep == AI_OP_CHECK_POS )
        {
            MapSquare mp = (MapSquare)o;
            
            if ( ( mp.getFirstPosition().getPositionX() != getParentObject().getPositionX() ) ||
                    ( mp.getFirstPosition().getPositionY() != getParentObject().getPositionY() ) )
            {
                /* We are not on the position we should have been! Recalculate the route! */
                clearStepCache();
                
                rcMy.calculateRoute( getParentObject().getPositionX(),
                        getParentObject().getPositionY(),
                        mp.getSecondPosition().getPositionX(),
                        mp.getSecondPosition().getPositionY(),
                        true
                      );
                      
            }

        }        
    }
    
    public void changeAIPosition( int iNewX, int iNewY)
    {
        this.addCustomStep( AI_OP_FIND_WAY, new MapPosition( iNewX, iNewY ) );
    }    
    
    public String getAIModuleName()
    {
        return "Default Controllable.";
    }

    public void sendAIPositionChange(int iNewX, int iNewY) 
    {
        this.setEnabled( false );
        
        this.clearStepCache();
        changeAIPosition( iNewX, iNewY );
        
        this.setEnabled( true );
    }

    public void sendAIStopMovement() 
    {
        this.setEnabled( false );
        
        this.clearStepCache();
        this.addStateChange( GEN_STATE_INACTIVE );
        
        this.setEnabled( true );
    }

    public boolean canSendAIStopMovement() 
    {
        return true;
    }
    
    public JWarRouteCalculator getRouteCalculator()
    {
        return rcMy;
    }
    
}
