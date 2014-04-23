package UI;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.border.LineBorder;

import AI.BuildingOptions;
import AI.ControllableUnit;
import AI.JWarParty;
import AI.KillingOptions;
import AI.TrainerOptions;
import Objects.JWarDynamicObject;
import Objects.JWarInfoBar;

/**
 * This class provides visual support for information provided
 * by units upon selecting them on the game grid.
 * @author Ciobanu Alexander
 *
 */
final public class JWarUnitInfoPanel extends JPanel implements ActionListener{

    private JLabel lbUnit     = new JLabel();   
    private JLabel lbName     = new JLabel();
    private JLabel lbNameV    = new JLabel();    
    private JLabel lbParty    = new JLabel();
    private JLabel lbPartyV   = new JLabel();    
    private JLabel lbAI       = new JLabel(); 
    private JLabel lbAIV      = new JLabel(); 
    private JLabel lbAIState  = new JLabel();
    private JLabel lbAIStateV = new JLabel();
    private JLabel lbUIco     = new JLabel();    
    private JPanel pnlUnit    = new JPanel();
    private JLabel lbInfo0    = new JLabel();
    private JLabel lbInfo0V   = new JLabel();
    private JLabel lbInfo1    = new JLabel();
    private JLabel lbInfo1V   = new JLabel();
    private JLabel lbResources   = new JLabel();
    private JLabel lbResourcesV  = new JLabel();
    private JButton btStop         = new JButton();
    private JButton btMine         = new JButton();
    private JButton btBuildMain    = new JButton();
    private JButton btTrainBuilder = new JButton();
    private JButton btTrainWar     = new JButton();
    private JButton btPatrol  = new JButton();
    private JButton btProtect = new JButton();
    private JButton btKill   = new JButton();
    
    private static JWarUnitInfoPanel UnitInfoPanel = null;
    private JWarDynamicObject objSel = null;
    
    /**
     * Generic Constructor.
     *
     */
    public JWarUnitInfoPanel()
    {
        setLayout( null );
        setBorder( new LineBorder( Color.BLACK ) );
        
        pnlUnit.setBounds( 15, 20, 70, 70 );
        pnlUnit.setBorder( new LineBorder( Color.BLACK ) );
        
        lbUnit.setBounds( 10, 5, 100, 15 );
        lbUnit.setText( "Selected Unit:");
        lbUnit.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        
        lbUIco.setText( "" );
        pnlUnit.setLayout( new BorderLayout() );
        pnlUnit.add( lbUIco, BorderLayout.CENTER );
        
        // ----------------------------------
        lbName.setText( "Name:" );
        lbName.setBounds( 120, 5, 50, 25 );
        lbName.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        lbName.setForeground( Color.BLUE );

        lbNameV.setText( "" );
        lbNameV.setBounds( 200, 5, 200, 25 );
        lbNameV.setFont( new Font( "Verdana", 0 , 10 ) );
        lbNameV.setForeground( Color.BLACK );        
        
        // ----------------------------------
        
        // ----------------------------------
        lbParty.setText( "Party:" );
        lbParty.setBounds( 120, 25, 50, 25 );
        lbParty.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        lbParty.setForeground( Color.BLUE );
        
        lbPartyV.setText( "" );
        lbPartyV.setBounds( 200, 25, 200, 25 );
        lbPartyV.setFont( new Font( "Verdana", 0, 10 ) );
        lbPartyV.setForeground( Color.BLACK );        
        // ----------------------------------
        
        // ----------------------------------
        lbAI.setText( "AI Module:" );
        lbAI.setBounds( 120, 45, 80, 25 );
        lbAI.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        lbAI.setForeground( Color.DARK_GRAY );
        
        lbAIV.setText( "" );
        lbAIV.setBounds( 200, 45, 200, 25 );
        lbAIV.setFont( new Font( "Verdana", 0 , 10 ) );
        lbAIV.setForeground( Color.BLACK );        
        // ----------------------------------
        
        // ----------------------------------
        lbAIState.setText( "AI State:" );
        lbAIState.setBounds( 120, 65, 80, 25 );
        lbAIState.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        lbAIState.setForeground( Color.DARK_GRAY );

        
        lbAIStateV.setText( "" );
        lbAIStateV.setBounds( 200, 65, 200, 25 );
        lbAIStateV.setFont( new Font( "Verdana", 0 , 10 ) );
        lbAIStateV.setForeground( Color.BLACK );        
        
        // ----------------------------------
        
        // ----------------------------------
        lbResources.setText( "Party's Resources:" );
        lbResources.setBounds( 430, 5, 120, 25 );
        lbResources.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        lbResources.setForeground( Color.RED );
        
        lbResourcesV.setText( "" );
        lbResourcesV.setBounds( 550, 5, 100, 25 );
        lbResourcesV.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        lbResourcesV.setForeground( Color.RED );        
        // ----------------------------------
        
        
        // ----------------------------------
        lbInfo0.setText( "Life:" );
        lbInfo0.setBounds( 430, 35, 80, 10 );
        lbInfo0.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        lbInfo0.setForeground( Color.BLACK );
        lbInfo0.setVisible( false );
        
        lbInfo0V.setText( "" );
        lbInfo0V.setBounds( 550, 35, 60, 10 );
        lbInfo0V.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        lbInfo0V.setForeground( Color.RED );
        lbInfo0V.setVisible( false );
        lbInfo0V.setBackground( Color.BLACK );
        lbInfo0V.setOpaque( true );
        // ----------------------------------        
        
        
        // ----------------------------------
        lbInfo1.setText( "Carry:" );
        lbInfo1.setBounds( 430, 55, 80, 10 );
        lbInfo1.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        lbInfo1.setForeground( Color.BLACK );
        lbInfo1.setVisible( false );
        
        lbInfo1V.setText( "" );
        lbInfo1V.setBounds( 550, 55, 60, 10 );
        lbInfo1V.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        lbInfo1V.setForeground( Color.RED );
        lbInfo1V.setVisible( false );
        lbInfo1V.setBackground( Color.BLACK );
        lbInfo1V.setOpaque( true );          
        // ----------------------------------  
        
        //000000000000000000000000000000000000000000000
        btStop.setText( "Stop" );
        btStop.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        btStop.setForeground( Color.BLACK );        
        btStop.setBounds( 700, 5, 70, 20 );
        btStop.setVisible( false );
        btStop.addActionListener( this );
        //000000000000000000000000000000000000000000000
        
        //000000000000000000000000000000000000000000000
        btMine.setText( "Mine Nearest Resources" );
        btMine.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        btMine.setForeground( Color.BLACK );        
        btMine.setBounds( 700, 25, 180, 20 );
        btMine.setVisible( false );
        btMine.addActionListener( this );
        //000000000000000000000000000000000000000000000        
        
        //000000000000000000000000000000000000000000000
        btBuildMain.setText( "Build Main Building" );
        btBuildMain.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        btBuildMain.setForeground( Color.BLACK );        
        btBuildMain.setBounds( 700, 45, 180, 20 );
        btBuildMain.setVisible( false );
        btBuildMain.addActionListener( this );
        //000000000000000000000000000000000000000000000       

        //000000000000000000000000000000000000000000000
        btTrainBuilder.setText( "Assemble Builder" );
        btTrainBuilder.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        btTrainBuilder.setForeground( Color.BLUE );        
        btTrainBuilder.setBounds( 700, 65, 160, 20 );
        btTrainBuilder.setVisible( false );
        btTrainBuilder.addActionListener( this );
        //000000000000000000000000000000000000000000000 
        
        
        //000000000000000000000000000000000000000000000
        btTrainWar.setText( "Assemble Killer" );
        btTrainWar.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        btTrainWar.setForeground( Color.RED );        
        btTrainWar.setBounds( 870, 65, 160, 20 );
        btTrainWar.setVisible( false );
        btTrainWar.addActionListener( this );
        //000000000000000000000000000000000000000000000 
        
        //000000000000000000000000000000000000000000000
        btPatrol.setText( "Patrol" );
        btPatrol.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        btPatrol.setForeground( Color.GREEN );        
        btPatrol.setBounds( 700, 65, 80, 20 );
        btPatrol.setVisible( false );
        btPatrol.addActionListener( this );
        //000000000000000000000000000000000000000000000 
        
        //000000000000000000000000000000000000000000000
        btProtect.setText( "Protect" );
        btProtect.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        btProtect.setForeground( Color.BLUE );        
        btProtect.setBounds( 790, 65, 80, 20 );
        btProtect.setVisible( false );
        btProtect.addActionListener( this );
        //000000000000000000000000000000000000000000000 
        
        
        //000000000000000000000000000000000000000000000
        btKill.setText( "Destroy" );
        btKill.setFont( new Font( "Verdana", Font.BOLD , 10 ) );
        btKill.setForeground( Color.RED );        
        btKill.setBounds( 880, 65, 80, 20 );
        btKill.setVisible( false );
        btKill.addActionListener( this );
        //000000000000000000000000000000000000000000000
        
        add( lbInfo0 );
        add( lbInfo0V );
        add( lbInfo1 );
        add( lbInfo1V ); 
        add( lbParty );
        add( lbPartyV );        
        add( lbResources );
        add( lbResourcesV );
        add( lbName );
        add( lbNameV );
        add( lbAI );
        add( lbAIV ); 
        add( lbAIState );
        add( lbAIStateV );
        add( pnlUnit );
        add( lbUnit );
        
        add( btStop );
        add( btMine );
        add( btBuildMain );
        
        add( btTrainBuilder );
        add( btTrainWar );
        
        add( btPatrol );
        add( btProtect );
        add( btKill );

        UnitInfoPanel = this;

    }
    
    /**
     * Sets the given inmage into the image container in this 
     * information panel.
     * @param img Image to be displayed.
     */
    private void setUnitImage( ImageIcon img )
    {
        lbUIco.setIcon( img );
    }
    
    /**
     * Sets the current Dynamic Object to be monitored by this
     * visual class.
     * @param obj The Dynamic Object to be monitored.
     */
    private void setWarObject( JWarDynamicObject obj )
    {
        objSel = obj;
    }
    
    /**
     * Returns the Dynamic object monitored by this class.
     * @return The Dynamic Object monitored.
     */
    private JWarDynamicObject getWarObject()
    {
        return objSel;
    }    
    
    /**
     * Sets the global monitored Dynamic Object.
     * @param obj Dynamic Object to be monitored.
     */
    public static void setSelectedObject( JWarDynamicObject obj )
    {
        UnitInfoPanel.setWarObject( obj );
        UnitInfoPanel.displaySelectedObject();
    }
    
    /**
     * Updates the information on this container by re-reading
     * all data from the selected Dynamic Object. This will happen only
     * if the given object is the same as the selected one.
     * @param obj The Dynamic Object that has updated. 
     */
    public static void updateIfObject( JWarDynamicObject obj )
    {
        if ( UnitInfoPanel.getWarObject() == obj)            
            UnitInfoPanel.displaySelectedObject();
    }
    
    /**
     * Draws the Info Bars that the selected object posseses.
     * Only 2 bars can be displayed at a time.
     *
     */
    private void drawInfoBars()
    {
        if ( objSel == null )
        {
            lbInfo0.setVisible( false );
            lbInfo0V.setVisible( false );
            
            lbInfo1.setVisible( false );
            lbInfo1V.setVisible( false );
            
            return;
        }
        
        JWarInfoBar[] ib = objSel.getInfoBars();
        
        if ( ib.length == 0 )
        {
            lbInfo0.setVisible( false );
            lbInfo0V.setVisible( false );
            
            lbInfo1.setVisible( false );
            lbInfo1V.setVisible( false );                    
        }
        
        if ( ib.length == 1 )
        {
            lbInfo0.setVisible( true );
            lbInfo0V.setVisible( true );
            
            lbInfo1.setVisible( false );
            lbInfo1V.setVisible( false );            
        }         
        
        if ( ib.length >= 2 )
        {
            lbInfo0.setVisible( true );
            lbInfo0V.setVisible( true );
            
            lbInfo1.setVisible( true );
            lbInfo1V.setVisible( true );            
        }           
        
        for ( int i = 0; i < ib.length; i++ )
        {
            if ( i > 1 ) break; // Only 2 info bars for now. I'll add later if needed.
            
            if (i == 0)
            {
                lbInfo0.setText( ib[0].getInfoBarName() + ":" );
                
                lbInfo0V.setText( ib[0].getInfoBarVisualValue() );
                lbInfo0V.setForeground( ib[0].getInfoBarColor() ); 
            }
            
            if (i == 1)
            {
                lbInfo1.setText( ib[1].getInfoBarName() + ":" );
                
                lbInfo1V.setText( ib[1].getInfoBarVisualValue() );
                lbInfo1V.setForeground( ib[1].getInfoBarColor() ); 
            }            
        }
    }
    
    /**
     * Displays the currently selected Dynamic Object or updates
     * the displayed information.
     */
    private void displaySelectedObject()
    {             
        ImageIcon img = null;
        String name = null;
        String pname = null;
        String ainame = null;
        String aistate = null;
        String spres = null; 
        Color pcolor = Color.BLACK;
              
        if (objSel != null)
        {
            img  = objSel.getStates().getStateIcon( JWarDynamicObject.OBJECT_STATE_IMAGE );
            name = objSel.getObjectName();
            
            pname  = objSel.getParty().getPartyName();
            pcolor = objSel.getParty().getPartyColor();
            
            if ( objSel.getAIModule() != null )
            {
                ainame  = objSel.getAIModule().getAIModuleName();
                aistate = objSel.getAIModule().getCurrentStateText();
            } else
            {
                ainame  = "None Selected.";
                aistate = "Stall";
            }

            spres = String.valueOf( objSel.getParty().getPartyResources() );
            
            if ( objSel.getParty() == JWarParty.HUMAN )
            {
                
                if ( objSel.getAIModule() instanceof ControllableUnit )
                {
                    btStop.setVisible( true );
                    
                    btStop.setEnabled(((ControllableUnit)objSel.getAIModule()).canSendAIStopMovement());
                    
                } else
                {
                    btStop.setVisible( false );
                }
                
                if ( objSel.getAIModule() instanceof BuildingOptions )
                {
                    btMine.setVisible( true );
                    btBuildMain.setVisible( true );
                    
                    btMine.setEnabled(((BuildingOptions)objSel.getAIModule()).canStartResourceCollection());
                    btBuildMain.setEnabled(((BuildingOptions)objSel.getAIModule()).canBuildMainBuilding());
                } else
                {
                    btMine.setVisible( false );
                    btBuildMain.setVisible( false );
                }
                
                if ( objSel.getAIModule() instanceof TrainerOptions )
                {
                    btTrainBuilder.setVisible( true );
                    btTrainWar.setVisible( true );
                    
                    btTrainBuilder.setEnabled(((TrainerOptions)objSel.getAIModule()).canTrainBuilderMachine());
                    btTrainWar.setEnabled(((TrainerOptions)objSel.getAIModule()).canTrainWarMachine());
                } else
                {
                    btTrainBuilder.setVisible( false );
                    btTrainWar.setVisible( false );
                }
                
                if ( objSel.getAIModule() instanceof KillingOptions )
                {
                    btPatrol.setVisible( true );
                    btProtect.setVisible( true );
                    btKill.setVisible( true );
                    
                    btPatrol.setEnabled(((KillingOptions)objSel.getAIModule()).canPatrolNearHome());
                    btProtect.setEnabled(((KillingOptions)objSel.getAIModule()).canProtectBuilders());
                    btKill.setEnabled(((KillingOptions)objSel.getAIModule()).canKillNearestTarget());
                } else
                {
                    btPatrol.setVisible( false );
                    btProtect.setVisible( false );
                    btKill.setVisible( false );
                }                 
            } else
            {
                btStop.setVisible( false );
                btMine.setVisible( false );
                btBuildMain.setVisible( false );
                btTrainBuilder.setVisible( false );
                btTrainWar.setVisible( false );
                btPatrol.setVisible( false );
                btProtect.setVisible( false );
                btKill.setVisible( false );                
            }
        } else
        {
            btStop.setVisible( false );
            btMine.setVisible( false );
            btBuildMain.setVisible( false );
            
            btTrainBuilder.setVisible( false );
            btTrainWar.setVisible( false );
            
            btPatrol.setVisible( false );
            btProtect.setVisible( false );
            btKill.setVisible( false );
        }

        
        
        setUnitImage( img );
        lbNameV.setText( name );
        lbPartyV.setText( pname );        
        lbPartyV.setForeground( pcolor );
        lbAIV.setText( ainame );
        lbAIStateV.setText( aistate );
        lbResourcesV.setText( spres );
        
        drawInfoBars();
    }

    /*
     *  (non-Javadoc)
     * @see java.awt.event.ActionListener#actionPerformed(java.awt.event.ActionEvent)
     */
    public void actionPerformed(ActionEvent arg0) 
    {
        if ( arg0.getSource() == btStop )
        {
            ((ControllableUnit)objSel.getAIModule()).sendAIStopMovement();
        }
        
        if ( arg0.getSource() == btMine )
        {
            ((BuildingOptions)objSel.getAIModule()).startResourceCollection();
        }
        
        if ( arg0.getSource() == btBuildMain )
        {
            ((BuildingOptions)objSel.getAIModule()).buildMainBuilding();
        }
        
        if ( arg0.getSource() == btTrainBuilder )
        {
            ((TrainerOptions)objSel.getAIModule()).trainBuilderMachine();
         }
        
        if ( arg0.getSource() == btTrainWar )
        {
            ((TrainerOptions)objSel.getAIModule()).trainWarMachine();
        }   
        
        if ( arg0.getSource() == btPatrol )
        {
            ((KillingOptions)objSel.getAIModule()).patrolNearHome();
        }   
        
        if ( arg0.getSource() == btProtect )
        {
            ((KillingOptions)objSel.getAIModule()).protectBuilders();
        } 
        
        if ( arg0.getSource() == btKill )
        {
            ((KillingOptions)objSel.getAIModule()).killNearestTarget();
        }        
    } 
   
}
