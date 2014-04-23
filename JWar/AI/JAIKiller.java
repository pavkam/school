package AI;

import java.util.Random;

import Objects.JWarDynamicObject;
import Objects.JWarLifeObject;
import UI.JBattleField;
import UI.JWarUnitInfoPanel;

public class JAIKiller extends JAIControllable implements KillingOptions {

    static public int AI_STATE_PATROL = JAIControllable.AI_STATE_LAST;
    static public int AI_STATE_ATTACK = JAIControllable.AI_STATE_LAST + 1;
    
    static private int IMP_FACTOR_CRITTER = 10000;
    static private int IMP_FACTOR_BUILDER = 5000;
    static private int IMP_FACTOR_BASE = 1000;
    static private int IMP_FACTOR_KILLER = 0;

    static public String GEN_STATE_PATROL = "Patrolling ...";
    static public String GEN_STATE_ATTACK = "Attacking ...";
        
    private boolean bPatrolling = false;
    private boolean bProtecting = false;
    private boolean bHunting    = false;

    private boolean bWasPatrolling = false;
    private boolean bWasProtecting = false;    
    
    private JWarLifeObject objHunt = null; 
    private WarMachine wmParent;
    
    private Random aRandom = new Random();
    
    public JAIKiller(JWarLifeObject parent) {
        super(parent);
        
        wmParent = (WarMachine)parent;
    }
    
    public String getAIModuleName()
    {
        return "Killer AI.";
    }

    public void patrolNearHome() 
    {
        if (!canPatrolNearHome())
            return;
        
        bProtecting = false;
        bHunting    = false;
        bPatrolling = true; 
        
        bWasPatrolling = false;
        bWasProtecting = false;        
    }
    
    public void sendAIPositionChange(int iNewX, int iNewY) 
    {
        bPatrolling = false;
        bProtecting = false;
        bHunting    = false;
        
        bWasPatrolling = false;
        bWasProtecting = false;
        
        super.sendAIPositionChange( iNewX, iNewY );
    }
    
    public void sendAIStopMovement() 
    {        
        bPatrolling = false;
        bProtecting = false;
        bHunting    = false;
        
        bWasPatrolling = false;
        bWasProtecting = false;
        
        super.sendAIStopMovement();
    }      

    public boolean canPatrolNearHome() 
    {
        
        if (bPatrolling)
            return false;
        
        return true;
    }
    
    public void getNextDecision()
    {
        if (bPatrolling)
        {
            addWaitStep( wmParent.getDecisionTiming() );
            
            JWarLifeObject mb = findNearestMainBuilding();
            
            if ( mb != null )
            {
                int iDist;
                
                iDist = getRouteCalculator().calculateRoute( 
                        getParentObject().getPositionX(),
                        getParentObject().getPositionY(),
                        ((mb)).getPositionX(),
                        ((mb)).getPositionY(),
                        false
                        );
                
                if ( iDist > wmParent.getHomePatrolMaxDistance() )
                {
                    goToObject( mb );
                    return;
                }
            }
            
            int iDir = aRandom.nextInt( 4 ) + JAIGeneric.AI_DIR_START;
            
            if ( canGoDirection( iDir ) )
                moveDirection( iDir );
            
            this.addStateChange( GEN_STATE_PATROL );
        }
        
        if ( (bProtecting) && (aRandom.nextBoolean()) )
        {
            addWaitStep( wmParent.getDecisionTiming() );
            
            JWarLifeObject mb = findFarestBuilder();
            
            if ( mb != null )
            {
                int iDist;
                
                iDist = getRouteCalculator().calculateRoute( 
                        getParentObject().getPositionX(),
                        getParentObject().getPositionY(),
                        ((mb)).getPositionX(),
                        ((mb)).getPositionY(),
                        false
                        );
                
                if ( iDist > wmParent.getMinimalProtectionDistance() )
                {
                    goToObject( mb );
                    return;
                }
            }
            
            int iDir = aRandom.nextInt( 4 ) + JAIGeneric.AI_DIR_START;
            
            if ( canGoDirection( iDir ) )
                moveDirection( iDir );
            
            this.addStateChange( GEN_STATE_PATROL );
        }
        
        if ( (bHunting) && (objHunt != null) )
        {
            int iDist;
            
            iDist = getRouteCalculator().calculateRoute( 
                    getParentObject().getPositionX(),
                    getParentObject().getPositionY(),
                    ((objHunt)).getPositionX(),
                    ((objHunt)).getPositionY(),
                    false
                    );
            
            if ( ( iDist > 0) && ( iDist <= wmParent.getShootDistance() )) 
            {
                if ( objHunt.getObjectLife() <= 0 )
                {
                    objHunt  = null;
                    bHunting = false;

                    bPatrolling = bWasPatrolling;
                    bProtecting = bWasProtecting;
                    
                    JWarUnitInfoPanel.updateIfObject( getParentObject() );
                    
                    return;
                }
                    
                addWaitStep( wmParent.getDecisionTiming() );
                addCustomStep(AI_STATE_ATTACK, null);
                addStateChange( GEN_STATE_ATTACK );

                return;
            } else
            {
                addWaitStep(wmParent.getDecisionTiming());
                
                iDist = getRouteCalculator().calculateRoute( 
                        getParentObject().getPositionX(),
                        getParentObject().getPositionY(),
                        ((objHunt)).getPositionX(),
                        ((objHunt)).getPositionY(),
                        false
                        );
                
                if (iDist <= 0)
                {
                    objHunt  = null;
                    bHunting = false;
                    
                    bPatrolling = bWasPatrolling;
                    bProtecting = bWasProtecting;
                    
                    JWarUnitInfoPanel.updateIfObject( getParentObject() );
                    
                    return;                    
                }
                
                goToObject(objHunt);
            }
            
            return;
        }
        
        
        
        /* No other action taken ... resume to the default behaviour */
        
        JWarLifeObject obj = findNearestThreat();
        
        if (obj != null)
        {
            /* Take action! */
            int iDist;
            
            iDist = getRouteCalculator().calculateRoute( 
                    getParentObject().getPositionX(),
                    getParentObject().getPositionY(),
                    ((obj)).getPositionX(),
                    ((obj)).getPositionY(),
                    false
                    );
            
            if ( iDist <= wmParent.getMaximalHuntSelectDistance() )
            {
                bHunting = true;
                objHunt  = obj;
                
                bWasPatrolling = bPatrolling;
                bWasProtecting = bProtecting;
                
                bPatrolling = false;
                bProtecting = false;
                
                return;
            }            
        }
        
        if ( getParentObject().getParty().getController() != null )
            getParentObject().getParty().getController().unitInputRequired( getParentObject() );        
    }

    public void protectBuilders() 
    {
        if (!canProtectBuilders())
            return;
        
        bProtecting = true;
        bPatrolling = false;
        bHunting    = false;
        
        bWasPatrolling = false;
        bWasProtecting = false;        
    }

    public boolean canProtectBuilders() 
    {
        if (bProtecting) 
            return false;
        
        return true;
    }

    public void killNearestTarget() 
    {
        if (!canKillNearestTarget())
            return;
        
        JWarLifeObject obj = findNearestThreat();
        
        if (obj != null)
        {
            bHunting = true;
            objHunt  = obj;
                
            bWasPatrolling = false;
            bWasProtecting = false;
                
            bPatrolling = false;
            bProtecting = false;
        }        
    }

    public boolean canKillNearestTarget() 
    {
        if (bHunting)
            return false;
        
        return true;
    }     
        
    private JWarLifeObject findNearestMainBuilding()
    {
        int iX, iY, iW, iH;
        int iMinDist, iDist;
        JWarLifeObject minBld = null;
        JBattleField btl = getParentObject().getBattleField();
        
        iW = btl.getFieldWidth();
        iH = btl.getFieldHeight();
        iMinDist = iW * iH;
        
        for ( iX = 0; iX < iW; iX++ )
            for ( iY = 0; iY < iH; iY++ )
            {
                Object o = btl.getDynamicObject( iX, iY );
                
                if ( o instanceof MainBuilding )
                {
                    if ( ((JWarDynamicObject)o).getParty() != getParentObject().getParty() )
                        continue;
                    
                    iDist = getRouteCalculator().calculateRoute( 
                            getParentObject().getPositionX(),
                            getParentObject().getPositionY(),
                            ((JWarLifeObject)o).getPositionX(),
                            ((JWarLifeObject)o).getPositionY(),
                            false
                            );
                    
                    if ( ( iDist < iMinDist ) && ( iDist > 0 ) )
                    {
                        minBld = ((JWarLifeObject)o);
                        iMinDist = iDist;
                    }
                }
            }
        
        return minBld;
    }
    
    private JWarLifeObject findFarestBuilder()
    {
        int iX, iY, iW, iH;
        int iMaxDist, iDist;
        JWarLifeObject minBld = null;
        JBattleField btl = getParentObject().getBattleField();
        
        iW = btl.getFieldWidth();
        iH = btl.getFieldHeight();
        iMaxDist = 0;
        
        for ( iX = 0; iX < iW; iX++ )
            for ( iY = 0; iY < iH; iY++ )
            {
                Object o = btl.getDynamicObject( iX, iY );
                
                if ( o instanceof BuilderMachine )
                {
                    if ( ((JWarDynamicObject)o).getParty() != getParentObject().getParty() )
                        continue;
                        
                    iDist = getRouteCalculator().calculateRoute( 
                            getParentObject().getPositionX(),
                            getParentObject().getPositionY(),
                            ((JWarDynamicObject)o).getPositionX(),
                            ((JWarDynamicObject)o).getPositionY(),
                            false
                            );
                    
                    if ( ( iDist > iMaxDist ) && ( iDist > 0 ) )
                    {
                        minBld = ((JWarLifeObject)o);
                        iMaxDist = iDist;
                    }
                }
            }
        
        return minBld;
    }    
    
    private JWarLifeObject findNearestThreat()
    {
        int iX, iY, iW, iH;
        int iMaxDist, iDist;
        JWarLifeObject minBld = null;
        JBattleField btl = getParentObject().getBattleField();
        
        iW = btl.getFieldWidth();
        iH = btl.getFieldHeight();
        iMaxDist = Integer.MAX_VALUE;
        
        for ( iX = 0; iX < iW; iX++ )
            for ( iY = 0; iY < iH; iY++ )
            {
                Object o = btl.getDynamicObject( iX, iY );
                
                if ( o instanceof JWarLifeObject )
                {
                    JWarParty hisParty = ((JWarDynamicObject)o).getParty();
                    JWarParty myParty  = getParentObject().getParty();
                    
                    if ( !(hisParty.inWarWith( myParty )) && ( hisParty != JWarParty.NEUTRAL ) )
                        continue;
                    
                    iDist = getRouteCalculator().calculateRoute( 
                            getParentObject().getPositionX(),
                            getParentObject().getPositionY(),
                            ((JWarDynamicObject)o).getPositionX(),
                            ((JWarDynamicObject)o).getPositionY(),
                            false
                            );
                    
                    if ( ((JWarDynamicObject)o).getParty() == JWarParty.NEUTRAL )
                    {
                        if (iDist > 0)
                            iDist += IMP_FACTOR_CRITTER;
                    }
                    
                    if ( (((JWarDynamicObject)o).getAIModule()) instanceof BuildingOptions )
                    {
                        if (iDist > 0)
                            iDist += IMP_FACTOR_BUILDER; 
                    }
                    
                    if ( (((JWarDynamicObject)o).getAIModule()) instanceof TrainerOptions )
                    {
                        if (iDist > 0)
                            iDist += IMP_FACTOR_BASE; 
                    }   
                    
                    if ( (((JWarDynamicObject)o).getAIModule()) instanceof KillingOptions )
                    {
                        if (iDist > 0)
                            iDist += IMP_FACTOR_KILLER; 
                    }
                    
                    if ( ( iDist < iMaxDist ) && ( iDist > 0 ) )
                    {
                        minBld = ((JWarLifeObject)o);
                        iMaxDist = iDist;
                    }
                }
            }
        
        return minBld;
    }      
         
    private void goToObject( JWarDynamicObject obj )
    {
        getRouteCalculator().calculateRoute( 
                getParentObject().getPositionX(),
                getParentObject().getPositionY(),
                obj.getPositionX(),
                obj.getPositionY(),
                true
                );
    }
    
    public void performCustomStep( int iStep, Object o )
    {
        if ( ( iStep == AI_STATE_ATTACK ) && (objHunt != null) && (bHunting) )
        {
            objHunt.getAIModule().clearStepCache();
            int iState_ = objHunt.getObjectState();
            
            if (!objHunt.extractLife( wmParent.getDamagePerShot() ))
            {
                objHunt.getAIModule().addStep( JWarLifeObject.OBJECT_STATE_ATTACKED );
                objHunt.getAIModule().addWaitStep( wmParent.getParalyseTiming() );
                objHunt.getAIModule().addStep( iState_ ); 
            } else
            {
                objHunt.getAIModule().addStep( JWarLifeObject.OBJECT_STATE_KILLED );
                objHunt.getAIModule().addWaitStep( wmParent.getGraveVisibilityTiming() );
                objHunt.getAIModule().addKillStep();
            }

            JWarUnitInfoPanel.updateIfObject( getParentObject() );
            JWarUnitInfoPanel.updateIfObject( objHunt );
            
            return;            
        }        
        super.performCustomStep( iStep, o );
    }
    
}
