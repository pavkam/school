package AI;


import Objects.JWarBuilderMachine;
import Objects.JWarDynamicObject;
import Objects.JWarKillerMachine;
import Objects.JWarLifeObject;
import Objects.JWarMainBuilding;
import Objects.JWarTrainerObject;
import UI.JWarUnitInfoPanel;

public class JAIMainBuilding extends JAIGeneric implements TrainerOptions {


    static public int AI_STATE_TRAIN_BUILDER = JAIGeneric.AI_STATE_LAST;
    static public int AI_STATE_TRAIN_WAR = JAIGeneric.AI_STATE_LAST + 1;
    
    static public int AI_STATE_PROGRESS_BUILDER = JAIGeneric.AI_STATE_LAST + 2;
    static public int AI_STATE_PROGRESS_WAR = JAIGeneric.AI_STATE_LAST + 3;
    
    static public String GEN_STATE_USR_TRAIN = "Assembling Machine ...";
    static public String GEN_STATE_INACTIVE_BLD = "Inactive. Sitting ... havin' a bud!";
    
    private boolean bTrainingBuilderMachine = false;
    private boolean bTrainingWarMachine = false;
    
    public JAIMainBuilding(JWarLifeObject parent) {
        super(parent);
        
        addStateChange( GEN_STATE_INACTIVE_BLD );
    }
       
    public String getAIModuleName()
    {
        return "Main Building AI.";
    }
    
    private void buildMachineInitialStep( int iMaxLife, int iCost, int iStateStep, int iVisualState )
    {
        JWarTrainerObject obj = ((JWarTrainerObject)getParentObject());
        obj.setObjectMaxTrainStatus( iMaxLife );            
        obj.setObjectTrainStatus( 0 );
        
        getParentObject().getParty().setPartyResources( 
                getParentObject().getParty().getPartyResources() - iCost );
        
        addCustomStep( iStateStep, null );
        
        getParentObject().setObjectState( iVisualState );
    }
    
    private boolean buildMachineForwardSteps( int iLifePerTick, int iMaxLife, int iProgressStep, Class cls )
    {
        JWarTrainerObject obj = ((JWarTrainerObject)getParentObject());
        
        if ( obj.getObjectTrainStatus() < obj.getObjectMaxTrainStatus() )
        {
            int iNow = obj.getObjectTrainStatus() + iLifePerTick;
            
            if ( iNow > iMaxLife )
                iNow = iMaxLife;
                    
            obj.setObjectTrainStatus( iNow );
            
            JWarUnitInfoPanel.updateIfObject( getParentObject() );

            if ( iNow < iMaxLife )
                addCustomStep( iProgressStep, null ); else
                {
                    obj.setObjectTrainStatus( 0 );
                    obj.setObjectMaxTrainStatus( 0 );
                    
                    addStep( JWarDynamicObject.OBJECT_STATE_DEFAULT );
                    addStateChange( JAIControllable.GEN_STATE_INACTIVE );
                    
                    JWarLifeObject lo;
                    
                    try {
                        lo = (JWarLifeObject)cls.newInstance();
                    } catch (InstantiationException e) {
                        lo = null;
                    } catch (IllegalAccessException e) {
                        lo = null;
                    }
                    
                    if ( lo == null )
                        return true;
                    
                    lo.setParty( getParentObject().getParty() );
                    
                    getParentObject().getBattleField().addDynamicObject( 
                            lo, getParentObject().getPositionX(), getParentObject().getPositionY()
                            );
                    
                    JAIGeneric ai = lo.getAIModule();
                    
                    if ( ai.canGoDirection( JAIGeneric.AI_DIR_LEFT ) )
                    {
                        ai.moveDirection( JAIGeneric.AI_DIR_LEFT );
                    } else
                        if ( canGoDirection( JAIGeneric.AI_DIR_RIGHT ) )
                        {
                            ai.moveDirection( JAIGeneric.AI_DIR_RIGHT );
                        } else
                            if ( canGoDirection( JAIGeneric.AI_DIR_UP ) )
                            {
                                ai.moveDirection( JAIGeneric.AI_DIR_UP );
                            } else
                                if ( canGoDirection( JAIGeneric.AI_DIR_DOWN ) )
                                {
                                    ai.moveDirection( JAIGeneric.AI_DIR_DOWN );  
                                } else
                                {
                                    ai.addCustomStep( JAIGeneric.AI_OP_STATE_EVADE, null );
                                }
                    
                    return true;
                }
        }
        
        return false;
    }    
    
    public void performCustomStep( int iStep, Object o )
    {
        if ( iStep == AI_STATE_TRAIN_BUILDER )
        {
            bTrainingBuilderMachine = true;
            
            buildMachineInitialStep( 
                    JWarBuilderMachine.MAX_LIFE,
                    JWarBuilderMachine.COST,
                    AI_STATE_PROGRESS_BUILDER,
                    JWarMainBuilding.OBJECT_STATE_TRAIN_BUILDER
                    );
            
            return;            
        };
        
        if ( iStep == AI_STATE_TRAIN_WAR )
        {
            bTrainingWarMachine = true;
            
            buildMachineInitialStep( 
                    JWarKillerMachine.MAX_LIFE,
                    JWarKillerMachine.COST,
                    AI_STATE_PROGRESS_WAR,
                    JWarMainBuilding.OBJECT_STATE_TRAIN_WAR
                    );
            
            return;            
        };        
        
        if ( ( iStep == AI_STATE_PROGRESS_BUILDER ) && (bTrainingBuilderMachine) )
        {
           if (buildMachineForwardSteps( 
                    JWarBuilderMachine.LIFE_PER_TICK, 
                    JWarBuilderMachine.MAX_LIFE,
                    AI_STATE_PROGRESS_BUILDER,
                    JWarBuilderMachine.class )
                    )
               bTrainingBuilderMachine = false;

           return;
        }
        
        if ( ( iStep == AI_STATE_PROGRESS_WAR ) && (bTrainingWarMachine) )
        {
           if (buildMachineForwardSteps( 
                    JWarKillerMachine.LIFE_PER_TICK, 
                    JWarKillerMachine.MAX_LIFE,
                    AI_STATE_PROGRESS_WAR,
                    JWarKillerMachine.class )
                    )
               bTrainingWarMachine = false;

           return;
        }        
        
        
        super.performCustomStep( iStep, o );
    }
    
    public void trainBuilderMachine() 
    {
        if (!canTrainBuilderMachine())
            return;
        
        this.addCustomStep( AI_STATE_TRAIN_BUILDER, null );
        this.addStateChange( GEN_STATE_USR_TRAIN );
    }

    public void trainWarMachine() 
    {
        if (!canTrainWarMachine())
            return;
        
        this.addCustomStep( AI_STATE_TRAIN_WAR, null );
        this.addStateChange( GEN_STATE_USR_TRAIN );
    }

    public boolean canTrainBuilderMachine() 
    {
        if (getParentObject().getParty().getPartyResources() < JWarBuilderMachine.COST)
            return false;
        
        if ( (bTrainingBuilderMachine) || (bTrainingWarMachine) )
            return false;
        
        return true;
    }

    public boolean canTrainWarMachine() 
    {
        if (getParentObject().getParty().getPartyResources() < JWarKillerMachine.COST)
            return false;
        
        if ( (bTrainingBuilderMachine) || (bTrainingWarMachine) )
            return false;
        
        return true;
    }

    public void getNextDecision()
    {
        if ( getParentObject().getParty().getController() != null )
            getParentObject().getParty().getController().unitInputRequired( getParentObject() ); 
    }
}
