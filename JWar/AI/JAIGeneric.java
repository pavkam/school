package AI;

import java.util.ArrayList;

import Objects.JWarDynamicObject;
import Objects.JWarLifeObject;
import Objects.JWarTerrainObject;
import UI.JBattleField;
import UI.JWarUnitInfoPanel;
import Utils.JWarTickGenerator;
import Utils.TickReceiver;

/**
 * Genertic AI. Used as base for all AI Classes in game.
 * @author Ciobanu Alexander
 *
 */
public class JAIGeneric implements TickReceiver {
   
    static public int AI_STATE_LEFT = 1;
    static public int AI_STATE_RIGHT = 2;
    static public int AI_STATE_UP = 3;
    static public int AI_STATE_DOWN = 4;    
    
    static public int AI_DIR_LEFT = 5;
    static public int AI_DIR_RIGHT = 6;
    static public int AI_DIR_UP = 7;
    static public int AI_DIR_DOWN = 8;
    
    static public int AI_OP_STATE_CHANGE = 9;
    static public int AI_OP_STATE_SKIP = 10;
    static public int AI_OP_STATE_EVADE = 11;
    static public int AI_OP_KILL_OBJ = 12;
        
    /* A developer will use this constant to extend the children's state list if necessary */
    static public int AI_STATE_LAST = 13;
    static public int AI_DIR_START = AI_DIR_LEFT;
    
    static private int DECISION_TIME = 100;
    static private int WAIT_FOR_EVADE = 10;
    
    private int iTimeToUpdate;
    private ArrayList alSteps = new ArrayList();

    private boolean bEnabled;
    private boolean bLocked = false;
    private boolean bInSteps = false;
    private JWarLifeObject doParent;
    
    private int iLastState; 
    private String sCurrentState = null;
    
    
    /**
     * Enables or disables this AI module. 
     * @param how
     */
    public void setEnabled( boolean how )
    {
        if (bLocked)
            return;
        
        bEnabled = how; 
        
        if (!how)
        {
            while (bInSteps);
        }
    }
    
    /**
     * Clears the cahce where all the steps are saved.
     *
     */
    public void clearStepCache()
    {
        if (bLocked)
            return;
        
        alSteps.clear();
    }
    
    /**
     * AI class constructor. Receives the parent Dynamic Object.
     * @param parent
     */
    public JAIGeneric( JWarLifeObject parent )
    {
        doParent = parent;
        setEnabled( true );
        JWarTickGenerator.GlobalTicks.subscribeForTicks( this );
        
        iLastState = doParent.getObjectState();
    }
    
    /**
     * Override in children to perform decision making for this AI.
     *
     */
    public void getNextDecision()
    {
        
    }
    
    /**
     * Called when a custom step is found in the step cache. Use in children.
     * @param iStep
     * @param o
     * 
     */
    public void performCustomStep( int iStep, Object o )
    {
        
    }
    
    /**
     * Performs the next step. If steps are present in the cache, do them, otherwise
     * call the decision making methods.
     */
    public void performNextStep()
    {
        if ( alSteps.size() > 0 )
        {
            /* Peform cached steps */
            int iStep = ((Integer)alSteps.get( 0 )).intValue();
            alSteps.remove( 0 );
            
            if (iStep == AI_OP_STATE_CHANGE)
            {
                String ss = null;
                
                if ( alSteps.size() > 0 );
                {
                    ss = (String)alSteps.get( 0 );
                    alSteps.remove( 0 );
                }
                
                sCurrentState = ss;
                JWarUnitInfoPanel.updateIfObject( getParentObject() );
                
                return;
            }
            
            if (iStep == AI_OP_STATE_SKIP)
            {
                Integer ii = null;
                
                if ( alSteps.size() > 0 );
                {
                    ii = (Integer)alSteps.get( 0 );
                    alSteps.remove( 0 );
                }
                
                iTimeToUpdate += ( DECISION_TIME * ii.intValue() );
                return;
            }
            
            if (iStep == AI_OP_KILL_OBJ)
            {
                alSteps.remove( 0 );
                getParentObject().killObject();
                
                return;
            }            
 
            
            if (iStep >= AI_STATE_LAST)
            {
                Object o = null;
                
                if ( alSteps.size() > 0 );
                    {
                        o = alSteps.get( 0 );
                        alSteps.remove( 0 );
                    }
                    
                performCustomStep( iStep, o );
                return;
            }
            
            if (iStep >= AI_OP_STATE_EVADE)
            {
                alSteps.remove( 0 );
                iStep = -1;

                if (canGoDirection( AI_DIR_DOWN ))
                    iStep = AI_DIR_DOWN;
                
                if (canGoDirection( AI_DIR_UP ))
                    iStep = AI_DIR_UP;
       
                if (canGoDirection( AI_DIR_LEFT ))
                    iStep = AI_DIR_LEFT;

                if (canGoDirection( AI_DIR_RIGHT ))
                    iStep = AI_DIR_RIGHT;
                
                if ( iStep == -1 )
                {
                    addWaitStep( WAIT_FOR_EVADE );
                    addCustomStep( AI_OP_STATE_EVADE, null );
                    
                    return;
                }
                
            }            
            
            
            
            /* Actual Movement */
            
            if ( ( iStep >= AI_DIR_LEFT ) && ( iStep <= AI_DIR_DOWN ) )
            {
                if ( !canGoDirection( iStep ) )
                    return;
                
                if ( iStep == AI_DIR_DOWN )
                    doParent.setObjectPosition( doParent.getPositionX(), doParent.getPositionY() + 1 );

                if ( iStep == AI_DIR_UP )
                    doParent.setObjectPosition( doParent.getPositionX(), doParent.getPositionY() - 1 );

                if ( iStep == AI_DIR_LEFT )
                    doParent.setObjectPosition( doParent.getPositionX() - 1, doParent.getPositionY() );

                if ( iStep == AI_DIR_RIGHT )
                    doParent.setObjectPosition( doParent.getPositionX() + 1, doParent.getPositionY() );
                
                return;
            }
            
            if ( iStep == JWarLifeObject.OBJECT_STATE_ATTACKED )
            {
                if ( doParent.getParty().getController() != null )
                    doParent.getParty().getController().unitAttacked( doParent );
            }
            
            /* State Change */
            doParent.setObjectState( iStep );
            return;
            
        }
        
        /* No steps in the cache, let's make another decision */

        getNextDecision();
    }

    public void TickReceived() 
    {
        if ( !bEnabled )
            return;
        
        iTimeToUpdate -= JWarTickGenerator.RECOMMENDED_TIME;
        
        if ( iTimeToUpdate <= 0 )
        {
            RegisteredForTicks();
            
            bInSteps = true;
            performNextStep();
            bInSteps = false;
        }
    }

    public void RegisteredForTicks() 
    {
        iTimeToUpdate = DECISION_TIME;
    }

    public void UnregisteredFromTicks() {}
    
    /**
     * Checks if a move to the direction given is possible.
     * @param iDir
     * @return
     */
    public boolean canGoDirection( int iDir )
    {
        JBattleField btField = doParent.getBattleField();
        int iPX = doParent.getPositionX();
        int iPY = doParent.getPositionY();

        /* Check Positions first */
        if ( iDir == AI_DIR_LEFT )
        {
            if ( doParent.getPositionX() <= 0 )
                return false;
            
            iPX--;
        }
        
        if ( iDir == AI_DIR_RIGHT )
        {
            if ( doParent.getPositionX() >= ( btField.getFieldWidth() - 1) )
                return false;
            
            iPX++;
        }
        
        if ( iDir == AI_DIR_UP )
        {
            if ( doParent.getPositionY() <= 0 )
                return false;
            
            iPY--;
        }
        
        if ( iDir == AI_DIR_DOWN )
        {
            if ( doParent.getPositionY() >= ( btField.getFieldHeight() - 1) )
                return false;
            
            iPY++;
        }
        
        /* Check cell availability */
              
        int iID = btField.getMapCell( iPX, iPY );
            
        if ( !JWarTerrainObject.terrainIsFree( iID ) )
            return false;
        
        if ( btField.getDynamicObject( iPX, iPY ) != null )
            return false;
        
        return true;
    }
    
    /**
     * Adds a new step into the step cache.
     * @param iStep
     */
    public void addStep( int iStep )
    {
        if (bLocked)
            return;
        
        alSteps.add( Integer.valueOf(iStep) );
        
        if ( ( iStep >= AI_STATE_LEFT ) && ( iStep <= AI_STATE_DOWN ) )
            iLastState = iStep;
    }
    
    /**
     * Adds a custom step.
     * @param iStep
     * @param o
     */
    public void addCustomStep( int iStep, Object o )
    {
        if (bLocked)
            return;
        
        alSteps.add( Integer.valueOf(iStep) );
        alSteps.add( o );
    }
       
    /**
     * Caches a set of steps to perform uniform rotation.
     * @param iDir
     * @param iState 
     */
    public void addRotationStep( int iDir, int iState )
    {
        if ( iState == AI_STATE_DOWN )
        {
            if ( iDir == AI_DIR_LEFT )
            {
                addStep( AI_STATE_LEFT );
            }
            
            if ( iDir == AI_DIR_UP )
            {
                addStep( AI_STATE_LEFT );
                addStep( AI_STATE_UP );
            }
            
            if ( iDir == AI_DIR_RIGHT )
            {
                addStep( AI_STATE_RIGHT );
            }            
        }
        
        if ( iState == AI_STATE_LEFT )
        {
            if ( iDir == AI_DIR_DOWN )
            {
                addStep( AI_STATE_DOWN );
            }
            
            if ( iDir == AI_DIR_UP )
            {
                addStep( AI_STATE_UP );
            }
            
            if ( iDir == AI_DIR_RIGHT )
            {
                addStep( AI_STATE_UP );
                addStep( AI_STATE_RIGHT );
            }            
        }
        
        if ( iState == AI_STATE_RIGHT )
        {
            if ( iDir == AI_DIR_DOWN )
            {
                addStep( AI_STATE_DOWN );
            }
            
            if ( iDir == AI_DIR_UP )
            {
                addStep( AI_STATE_UP );
            }
            
            if ( iDir == AI_DIR_LEFT )
            {
                addStep( AI_STATE_DOWN );
                addStep( AI_STATE_LEFT );
            }            
        }
        
        if ( iState == AI_STATE_UP )
        {
            if ( iDir == AI_DIR_DOWN )
            {
                addStep( AI_STATE_RIGHT );
                addStep( AI_STATE_DOWN );
            }
            
            if ( iDir == AI_DIR_RIGHT )
            {
                addStep( AI_STATE_RIGHT );
            }
            
            if ( iDir == AI_DIR_LEFT )
            {
                addStep( AI_STATE_LEFT );
            }            
        }
        
        if ( iState == JWarDynamicObject.OBJECT_STATE_DEFAULT )
        {
            if ( iDir == AI_DIR_DOWN )
            {
                addStep( AI_STATE_DOWN );
            }
            
            if ( iDir == AI_DIR_RIGHT )
            {
                addStep( AI_STATE_RIGHT );
            }
            
            if ( iDir == AI_DIR_LEFT )
            {
                addStep( AI_STATE_LEFT );
            } 
            
            if ( iDir == AI_DIR_UP )
            {
                addStep( AI_STATE_UP );
            }             
         
        }         
    }
    
    /**
     * Moves the unit to a given direction.
     * @param iDir
     */
    public void moveDirection( int iDir )
    {
        addRotationStep( iDir, iLastState );
        addStep( iDir );
    }
    
    /**
     * Adds a custom state change notification.
     * @param state
     */
    public void addStateChange( String state )
    {
        addCustomStep( AI_OP_STATE_CHANGE, state );
    }
    
    /**
     * Returns the Parent Object associated with this AI module.
     * @return
     */
    public JWarLifeObject getParentObject()
    {
        return doParent;
    }
    
    /**
     * Returns current state's text
     * @return
     */
    public String getCurrentStateText()
    {
        return sCurrentState;
    }
    
    /**
     * Returns AI Module's name.
     * @return
     */
    public String getAIModuleName()
    {
        return "Generic. Dead";
    }
    
    /**
     * Adds a Waiting step.
     * @param iDecisions
     */
    public void addWaitStep( int iDecisions )
    {
        addCustomStep( AI_OP_STATE_SKIP, Integer.valueOf(iDecisions) );
    }
    
    /**
     * Removes this AI from Tick subscription and prepares it for dying.
     *
     */
    public void discontinueAIModule()
    {
        doParent = null;
        JWarTickGenerator.GlobalTicks.unsubscribeFromTicks( this );
    }
    
    /**
     * Adds a standard kill step.
     *
     */
    public void addKillStep()
    {
        addCustomStep( AI_OP_KILL_OBJ, null );
        bLocked = true;        
    }
}
