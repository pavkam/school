package AI;

import Objects.JWarBuilderMachine;
import Objects.JWarDynamicObject;
import Objects.JWarLifeObject;
import Objects.JWarMainBuilding;
import Objects.JWarMineralObject;
import UI.JBattleField;
import UI.JWarUnitInfoPanel;

public class JAIBuilder extends JAIControllable implements BuildingOptions {

    static public int MINING_DECISIONS = 5;
    static public int MINING_COUNT = 2;
    
    static public int AI_STATE_BUILD = JAIControllable.AI_STATE_LAST;
    static public int AI_STATE_PROGRESS = AI_STATE_BUILD + 1;
    static public int AI_STATE_MINE = AI_STATE_BUILD + 2;
    static public int AI_STATE_UNLOAD = AI_STATE_BUILD + 3;
    
    
    static public String GEN_STATE_USR_BUILD = "Building ...";
    static public String GEN_STATE_MINE = "Mining ...";
    static public String GEN_STATE_UNLOAD = "Unloading ...";
    
    private boolean bBuilding = false;
    private boolean bMining = false;
    
    private int iPrevLife;
    private JWarMineralObject objMineral = null;
    private JWarMainBuilding objBase = null;
    
    public JAIBuilder(JWarLifeObject parent) {
        super(parent);
    }
    
    public String getAIModuleName()
    {
        return "Builder AI.";
    }    

    public void buildMainBuilding() 
    {
        if (!canBuildMainBuilding())
            return;
        
        this.addCustomStep( AI_STATE_BUILD, null );
        this.addStateChange( GEN_STATE_USR_BUILD );
    }

    public void startResourceCollection() 
    {
        bMining = true;
    }
    
    public void sendAIPositionChange(int iNewX, int iNewY) 
    {
        if (!bBuilding)
        {
            objBase = null;
            objMineral = null;
            bMining = false;
            
            super.sendAIPositionChange( iNewX, iNewY );
        }
    }
    
    public void sendAIStopMovement() 
    {
        if (bBuilding)
        {
            this.setEnabled( false );
            
            this.clearStepCache();
            
            bBuilding = false;
            
            JWarLifeObject obj = ((JWarLifeObject)getParentObject());
            
            obj.setObjectMaxLife( obj.getObjectPredefinedMaxLife() );
            obj.setObjectLife( iPrevLife );

            addStep( JWarDynamicObject.OBJECT_STATE_DEFAULT );
            addStateChange( JAIControllable.GEN_STATE_INACTIVE );
            
            this.setEnabled( true );
            
            return;
        }
        
        if (bMining)
        {
            this.setEnabled( false );
            this.clearStepCache();
            
            bMining = false;
            
            objBase = null;
            objMineral = null;            

            this.setEnabled( true );
            
            return;
        }        
        
        
            
        super.sendAIStopMovement();
    }    
    
    public void performCustomStep( int iStep, Object o )
    {
        if ( iStep == AI_STATE_BUILD )
        {
            bBuilding = true;
            
            JWarLifeObject obj = ((JWarLifeObject)getParentObject());
            obj.setObjectMaxLife( JWarMainBuilding.MAX_LIFE );
            
            iPrevLife = obj.getObjectLife();
            obj.setObjectLife( 0 );
            
            getParentObject().getParty().setPartyResources( 
                    getParentObject().getParty().getPartyResources() - JWarMainBuilding.COST );
            
            addCustomStep( AI_STATE_PROGRESS, null );
            
            getParentObject().setObjectState( AI_STATE_BUILD );
            
            return;            
        };
        
        if ( ( iStep == AI_STATE_PROGRESS ) && (bBuilding) )
        {
            JWarLifeObject obj = ((JWarLifeObject)getParentObject());
            
            if ( obj.getObjectLife() < obj.getObjectMaxLife() )
            {
                int iNow = obj.getObjectLife() + JWarMainBuilding.LIFE_PER_TICK;
                
                if ( iNow > JWarMainBuilding.MAX_LIFE )
                    iNow = JWarMainBuilding.MAX_LIFE;
                        
                obj.setObjectLife( iNow );
                
                JWarUnitInfoPanel.updateIfObject( getParentObject() );
                
                if ( iNow < JWarMainBuilding.MAX_LIFE )
                    addCustomStep( AI_STATE_PROGRESS, null ); else
                    {
                        obj.setObjectMaxLife( obj.getObjectPredefinedMaxLife() );
                        obj.setObjectLife( iPrevLife );
                        
                        addStep( JWarDynamicObject.OBJECT_STATE_DEFAULT );
                        addStateChange( JAIControllable.GEN_STATE_INACTIVE );
                        
                        if ( canGoDirection( JAIGeneric.AI_DIR_LEFT ) )
                        {
                            moveDirection( JAIGeneric.AI_DIR_LEFT );
                        } else
                            if ( canGoDirection( JAIGeneric.AI_DIR_RIGHT ) )
                            {
                                moveDirection( JAIGeneric.AI_DIR_RIGHT );
                            } else
                                if ( canGoDirection( JAIGeneric.AI_DIR_UP ) )
                                {
                                    moveDirection( JAIGeneric.AI_DIR_UP );
                                } else
                                    if ( canGoDirection( JAIGeneric.AI_DIR_DOWN ) )
                                    {
                                        moveDirection( JAIGeneric.AI_DIR_DOWN );  
                                    } else
                                    {
                                        addCustomStep( JAIGeneric.AI_OP_STATE_EVADE, null );
                                    }                        
                        
                        JWarMainBuilding mb = new JWarMainBuilding();
                        mb.setParty( getParentObject().getParty() );
                        
                        getParentObject().getBattleField().addDynamicObject( 
                                mb, getParentObject().getPositionX(), getParentObject().getPositionY()
                                );
                        
                        bBuilding = false;
                    }

                return;
            }
        }
        
        if ( (iStep == AI_STATE_MINE ) && (objMineral != null) && (bMining))
        {
            int iExtr = MINING_COUNT;
            
            if ( ((JWarBuilderMachine)getParentObject()).getFreeMineralSlots() < MINING_COUNT )
                iExtr -= ((JWarBuilderMachine)getParentObject()).getFreeMineralSlots();
                    
            iExtr = objMineral.extractResources( iExtr );
            
            ((JWarBuilderMachine)getParentObject()).addMineralSlots( iExtr );
            JWarUnitInfoPanel.updateIfObject( getParentObject() );
            JWarUnitInfoPanel.updateIfObject( objMineral );
            
            return;
        }
        
        if ( (iStep == AI_STATE_UNLOAD ) && (objBase != null) && (bMining))
        {
            int iExtr = ((JWarBuilderMachine)getParentObject()).removeMineralSlots( MINING_COUNT );

            getParentObject().getParty().setPartyResources( getParentObject().getParty().getPartyResources() + iExtr );

            if ( JWarDynamicObject.getSelectedObject() != null )
            {
                if ( JWarDynamicObject.getSelectedObject().getParty() == JWarParty.HUMAN )
                    JWarUnitInfoPanel.updateIfObject( JWarDynamicObject.getSelectedObject() );
            }
            
            return;
        }        
        
        
        
        super.performCustomStep( iStep, o );
    }

    public boolean canBuildMainBuilding() 
    {
        if (getParentObject().getParty().getPartyResources() < JWarMainBuilding.COST)
            return false;
        
        if ( (bBuilding) || (bMining) )
            return false;
        
        return true;
    }

    public boolean canStartResourceCollection() 
    {
        if ( (bBuilding) || (bMining) )
            return false;
        
        return true;
    }
    
    public JWarMineralObject findNearestResource()
    {
        int iX, iY, iW, iH;
        int iMinDist, iDist;
        JWarMineralObject minMineral = null;
        JBattleField btl = getParentObject().getBattleField();
        
        iW = btl.getFieldWidth();
        iH = btl.getFieldHeight();
        iMinDist = iW * iH;
        
        for ( iX = 0; iX < iW; iX++ )
            for ( iY = 0; iY < iH; iY++ )
            {
                Object o = btl.getDynamicObject( iX, iY );
                
                if ( o instanceof JWarMineralObject )
                {
                    iDist = getRouteCalculator().calculateRoute( 
                            getParentObject().getPositionX(),
                            getParentObject().getPositionY(),
                            ((JWarMineralObject)o).getPositionX(),
                            ((JWarMineralObject)o).getPositionY(),
                            false
                            );
                    
                    if ( ( iDist < iMinDist ) && ( iDist > 0 ) )
                    {
                        minMineral = ((JWarMineralObject)o);
                        iMinDist = iDist;
                    }
                }
            }
        
        return minMineral;
    }
    
    private JWarMainBuilding findNearestMainBuilding()
    {
        int iX, iY, iW, iH;
        int iMinDist, iDist;
        JWarMainBuilding minBld = null;
        JBattleField btl = getParentObject().getBattleField();
        
        iW = btl.getFieldWidth();
        iH = btl.getFieldHeight();
        iMinDist = iW * iH;
        
        for ( iX = 0; iX < iW; iX++ )
            for ( iY = 0; iY < iH; iY++ )
            {
                Object o = btl.getDynamicObject( iX, iY );
                
                if ( o instanceof JWarMainBuilding )
                {
                    if ( ((JWarDynamicObject)o).getParty() != getParentObject().getParty() )
                        continue;
                    
                    iDist = getRouteCalculator().calculateRoute( 
                            getParentObject().getPositionX(),
                            getParentObject().getPositionY(),
                            ((JWarMainBuilding)o).getPositionX(),
                            ((JWarMainBuilding)o).getPositionY(),
                            false
                            );
                    
                    if ( ( iDist < iMinDist ) && ( iDist > 0 ) )
                    {
                        minBld = ((JWarMainBuilding)o);
                        iMinDist = iDist;
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
    
    
    public void getNextDecision()
    {
        int iX = getParentObject().getPositionX();
        int iY = getParentObject().getPositionY();
        
        JBattleField btl = getParentObject().getBattleField();
        
        if (bMining)
        {
            /* Mineral Object collection. */
            if ((objMineral != null)) 
            {
                if (((JWarBuilderMachine) getParentObject())
                        .getFreeMineralSlots() == 0) 
                {
                    /* Next Decision is to go home to bring the minerals back */
                    objMineral = null;
                    objBase = findNearestMainBuilding();

                    if (objBase != null) 
                    {
                        addWaitStep(MINING_DECISIONS);
                        goToObject(objBase);
                    }

                    return;
                }

                if ((btl.getDynamicObject(iX - 1, iY) instanceof JWarMineralObject)
                        || (btl.getDynamicObject(iX + 1, iY) instanceof JWarMineralObject)
                        || (btl.getDynamicObject(iX, iY - 1) instanceof JWarMineralObject)
                        || (btl.getDynamicObject(iX, iY + 1) instanceof JWarMineralObject)) {
                    addWaitStep(MINING_DECISIONS);
                    addCustomStep(AI_STATE_MINE, null);
                    addStateChange( GEN_STATE_MINE );
                    return;
                } else 
                {
                    /* Next Decision is to go and find another source */
                    objMineral = findNearestResource();

                    if (objMineral != null) 
                    {
                        addWaitStep(MINING_DECISIONS);
                        goToObject(objMineral);
                    }

                    return;
                }

            }

            /* Home Return. */
            if ((objBase != null)) 
            {
                if (((JWarBuilderMachine) getParentObject())
                        .isMachineReadyForMining()) 
                {
                    /* Next Decision is to go and restart the mineral search */
                    objMineral = findNearestResource();

                    if (objMineral != null) 
                    {
                        addWaitStep(MINING_DECISIONS);
                        goToObject(objMineral);
                    }

                    return;
                }

                if ((btl.getDynamicObject(iX - 1, iY) instanceof JWarMainBuilding)
                        || (btl.getDynamicObject(iX + 1, iY) instanceof JWarMainBuilding)
                        || (btl.getDynamicObject(iX, iY - 1) instanceof JWarMainBuilding)
                        || (btl.getDynamicObject(iX, iY + 1) instanceof JWarMainBuilding)) 
                {
                    addWaitStep(MINING_DECISIONS);
                    addCustomStep(AI_STATE_UNLOAD, null);
                    addStateChange( GEN_STATE_UNLOAD );
                    
                    return;
                } else
                {
                    /* Next Decision is to go and find the base once again */
                    objBase = findNearestMainBuilding();

                    if (objBase != null) 
                    {
                        addWaitStep(MINING_DECISIONS);
                        goToObject(objBase);
                    }

                    return;
                }

            }
            
            /* Something went not quite well ... Let's review hte way */
            objBase = null;
            objMineral = null;
            
            if (((JWarBuilderMachine)getParentObject())
                    .isMachineReadyForMining())
            {
                /* Next Decision is to go and restart the mineral search */
                objMineral = findNearestResource();

                if (objMineral != null) 
                {
                    addWaitStep(MINING_DECISIONS);
                    goToObject(objMineral);
                }

                return;
            } else
            {
                /* Next Decision is to go and find the base once again */
                objBase = findNearestMainBuilding();

                if (objBase != null) 
                {
                    addWaitStep(MINING_DECISIONS);
                    goToObject(objBase);
                }

                return;
            }
            
            
        }
        
        if ( getParentObject().getParty().getController() != null )
            getParentObject().getParty().getController().unitInputRequired( getParentObject() );
    }    

}
