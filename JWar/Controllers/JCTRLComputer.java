package Controllers;

import AI.BuildingOptions;
import AI.JAIBuilder;
import AI.JAIGeneric;
import AI.KillingOptions;
import AI.TrainerOptions;
import Objects.JWarBuilderMachine;
import Objects.JWarDynamicObject;
import Objects.JWarKillerMachine;
import Objects.JWarMainBuilding;
import Objects.JWarMineralObject;

/**
 * This class is implementing the Control options for computer-driven AI.
 * @author Ciobanu Alexander
 *
 */
public class JCTRLComputer extends JCTRLGeneric {
    
    private static int MAX_START_MINERAL_DISTANCE = 4;
    private static int START_BUILDERS_COUNT       = 4;
    private static int TOTAL_BUILDERS_COUNT       = 10;
    private static int TOTAL_KILLERS_PER_BUILDER  = 2;
    private static int START_KILLERS_COUNT        = 1;
    private static int WAR_KILLERS_COUNT          = 10;
    private boolean bInStart = true;
    private int iTrainKiller = 0;
    
    /*
     *  (non-Javadoc)
     * @see Controllers.JCTRLGeneric#unitInputRequired(Objects.JWarDynamicObject)
     */
    public void unitInputRequired( JWarDynamicObject dyn )
    {
        if ( dyn instanceof JWarBuilderMachine )
        {
            /* Let's see if there are any main buildings built */
            if ( getObjectCount( JWarMainBuilding.class, getParty() ) == 0 )
            {
                /* Let's instruct the building of a new main */
                JAIBuilder pB = (JAIBuilder)dyn.getAIModule();
                JWarMineralObject mO;
                mO = pB.findNearestResource();
                
                if ( mO != null )
                {
                    int iDist;
                    
                    iDist = pB.getRouteCalculator().calculateRoute( 
                            dyn.getPositionX(),
                            dyn.getPositionY(),
                            ((JWarMineralObject)mO).getPositionX(),
                            ((JWarMineralObject)mO).getPositionY(),
                            false
                    );           
                    
                    if ( iDist > MAX_START_MINERAL_DISTANCE )
                    {
                        /* Command the builder to go to the nearest resource */
                        pB.getRouteCalculator().calculateRoute( 
                                dyn.getPositionX(),
                                dyn.getPositionY(),
                                ((JWarMineralObject)mO).getPositionX(),
                                ((JWarMineralObject)mO).getPositionY(),
                                true
                        );
                        
                        return;
                    }
                }
                
                
                /* Command building of the main */
                ((BuildingOptions)dyn.getAIModule()).buildMainBuilding();
                
                return;
            }
            
            
            
            
            /* ----------------- NOTHING TO DO ------------------------------------
             * There are main buildings ... however we're not doing anything useful.
             * Lets start mining :) 
             */
            
            if (((BuildingOptions)dyn.getAIModule()).canStartResourceCollection())
                ((BuildingOptions)dyn.getAIModule()).startResourceCollection();
            
            return;
        }
        
        /* Main building is Idle. Let's command building of some sort of units if possible */
        if ( dyn instanceof JWarMainBuilding )
        {
            /* Were in start game mode now. Lets initialize our way first */
            if ( bInStart )
            {
                if (getObjectCount( JWarBuilderMachine.class, getParty()) < START_BUILDERS_COUNT)
                {
                    /* Train a builder if possible */
                    
                    if ( ((TrainerOptions)dyn.getAIModule()).canTrainBuilderMachine() )
                        ((TrainerOptions)dyn.getAIModule()).trainBuilderMachine();
                    
                    return;
                }
                
                if (getObjectCount( JWarKillerMachine.class, getParty()) < START_KILLERS_COUNT)
                {
                    /* Train a killer if possible */
                    
                    if ( ((TrainerOptions)dyn.getAIModule()).canTrainWarMachine() )
                        ((TrainerOptions)dyn.getAIModule()).trainWarMachine();
                    
                    return;
                }                
                
                bInStart = false;
                iTrainKiller = 0;
            }
            
            
            /* Let's command our army! */
            if ( getObjectCount( JWarKillerMachine.class, getParty()) >= WAR_KILLERS_COUNT )
            {
                commandWar();
            }
            
            
            /* We're not in start ... now lets create units!! Select the priority */
            
            if ( iTrainKiller == TOTAL_KILLERS_PER_BUILDER )
            {
                if ( getObjectCount( JWarBuilderMachine.class, getParty()) < TOTAL_BUILDERS_COUNT )
                {
                    if ( ((TrainerOptions)dyn.getAIModule()).canTrainBuilderMachine() )
                    {
                        ((TrainerOptions)dyn.getAIModule()).trainBuilderMachine();
                        iTrainKiller = 0;
                    }
                } else
                    iTrainKiller = 0;
            } else
            {
                if ( ((TrainerOptions)dyn.getAIModule()).canTrainWarMachine() )
                {
                    ((TrainerOptions)dyn.getAIModule()).trainWarMachine();
                    iTrainKiller++;
                }                
            }
            

            return;
        }
        
        /* A Killer Machine is doing nothing! */
        if ( dyn instanceof JWarKillerMachine )
        {
            ((KillingOptions)dyn.getAIModule()).protectBuilders();

        }
    }
        
    /**
     * Command all Killers to search for targets and destroy them.
     */
    public void commandWar()
    {
        int iX, iY;
        int iW = getBattleField().getFieldWidth();
        int iH = getBattleField().getFieldHeight();
        
        for ( iX = 0; iX < iW; iX++ )
            for ( iY = 0; iY < iH; iY++ )
            {
                JWarDynamicObject o = getBattleField().getDynamicObject( iX, iY );
                
                if ( o instanceof JWarKillerMachine )
                {
                    ((KillingOptions)o.getAIModule()).killNearestTarget();
                }
            }
    }    
    
    /**
     * Commands all Killers to get to the nearest friendly bases and defend them.
     */
    public void commandRetreat()
    {
        int iX, iY;
        int iW = getBattleField().getFieldWidth();
        int iH = getBattleField().getFieldHeight();
        
        for ( iX = 0; iX < iW; iX++ )
            for ( iY = 0; iY < iH; iY++ )
            {
                JWarDynamicObject o = getBattleField().getDynamicObject( iX, iY );
                
                if ( o instanceof JWarKillerMachine )
                {
                    if ( o.getParty() == getParty() )
                        ((KillingOptions)o.getAIModule()).patrolNearHome();
                }
            }
    }  
    
    /*
     *  (non-Javadoc)
     * @see Controllers.JCTRLGeneric#unitAttacked(Objects.JWarDynamicObject)
     */
    public void unitAttacked( JWarDynamicObject dyn )
    {
        if (dyn.getAIModule() instanceof BuildingOptions)
        {
            /* Run!!! */
            
            if ( dyn.getAIModule().canGoDirection( JAIGeneric.AI_DIR_DOWN ) )
                dyn.getAIModule().moveDirection( JAIGeneric.AI_DIR_DOWN ); else
            
            if ( dyn.getAIModule().canGoDirection( JAIGeneric.AI_DIR_UP ) )
                dyn.getAIModule().moveDirection( JAIGeneric.AI_DIR_UP ); else
            
            if ( dyn.getAIModule().canGoDirection( JAIGeneric.AI_DIR_LEFT ) )
                dyn.getAIModule().moveDirection( JAIGeneric.AI_DIR_LEFT ); else
            
            if ( dyn.getAIModule().canGoDirection( JAIGeneric.AI_DIR_RIGHT ) )
                dyn.getAIModule().moveDirection( JAIGeneric.AI_DIR_RIGHT );
            
            return;
        }
        
        if (dyn instanceof JWarMainBuilding)
        {
            /* Call Defences !!! */
            
            commandRetreat();
            return;
        }        
    }
}
