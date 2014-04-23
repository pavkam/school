package Objects;
import javax.swing.JPanel;
import javax.swing.ImageIcon;

import AI.ControllableUnit;
import AI.JAIGeneric;
import AI.JWarParty;
import UI.JBattleField;
import UI.JWarUnitInfoPanel;

import java.awt.Color;
import java.awt.Cursor;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.util.ArrayList;

/**
 * This class is the base for all dynamic objects  in the game (e.g. Vehicles)
 * @author Ciobanu Alexander
 *
 */
public class JWarDynamicObject extends JPanel implements MouseListener {
    
    static public int OBJECT_STATE_DEFAULT = 0;
    static public int OBJECT_STATE_IMAGE = -1;
    static public int OBJECT_STATE_PREV = -2;
    
    private JWarObjectStates wsStates = new JWarObjectStates();
    private int intPosX, intPosY;
    private ImageIcon iiCurrent = null;
    private ArrayList alBars = new ArrayList();
    private int iCurrentState;
    private JBattleField btlField;
    private JWarParty pParty;
    private JAIGeneric aiSel;
    
    static private JWarDynamicObject SelectedObject = null;
    
    
    /**
     * Load resources must be implemented here! Must be overriden in children.
     *
     */
    public void setupObject()
    {
        
    }

    /**
     * Constructor
     * 
     */
    public JWarDynamicObject()
    {
        // Invoke the call of resource loading! Must be overriden in children.
        try
        {
            setupObject();
        } catch ( Exception e ) {}
        
        setObjectState( OBJECT_STATE_DEFAULT );
        
        setObjectPosition( 0, 0 );
        addMouseListener( this );
        
        setCursor( new Cursor( Cursor.HAND_CURSOR ) );
    }
    
    /**
     * Selects a new current AI module.
     * @param ai
     */
    public void setAIModule( JAIGeneric ai )
    {
        aiSel = ai;
    }
    
    /**
     * Returns the current selected AI module.
     * @return
     */
    public JAIGeneric getAIModule()
    {
        return aiSel;
    }    
    
    /**
     * Sets this unit's party.
     * @param party
     */
    public void setParty( JWarParty party )
    {
        pParty = party;
    }
    
    /**
     * Gets this unit's party.
     * @return
     */
    public JWarParty getParty()
    {
        return pParty;
    }
    
    /**
     * Assigns the battle field.
     * @param field
     */
    public void setBattleField( JBattleField field )
    {
        btlField = field;
    }

    /**
     * Gets the selected battle field.
     * @return
     */
    public JBattleField getBattleField()
    {
        return btlField;
    }
    
    /**
     * Returns the States object assigned to this Dynamic Object.
     * @return
     */
    public JWarObjectStates getStates()
    {
        return wsStates;
    }
    
    /**
     * Sets the new position on the map.
     * @param iX
     * @param iY
     */
    public void setObjectPosition( int iX, int iY )
    {
        intPosX = iX;
        intPosY = iY;
        
        setBounds( (iX * JBattleField.CELL_DIM) + 1, 
                   (iY * JBattleField.CELL_DIM) + 1, 
                   JBattleField.CELL_DIM - 1, JBattleField.CELL_DIM - 1 );
    }
    
    /**
     * Says if the object at coords given is this one.
     * @param iX
     * @param iY
     * @return
     */
    public boolean objectIsMe( int iX, int iY )
    {
        if ( ( intPosX == iX ) && ( intPosY == iY ) )
            return true; else return false;
    }
    
    public void paint( Graphics g )
    {
        if ( g == null )
            return;
        
        super.paint(g);

        int iFSz = 9;
        Font ftSmall = new Font( "Fixedsys", 0, iFSz );
        Font ftPrev  = g.getFont();   
                
        g.setFont( ftSmall );        
        
        if ( iiCurrent != null )
            iiCurrent.paintIcon( this, g, 0, 0 );
        
        if ( getParty() != JWarParty.NEUTRAL)
        {
            Color cc = g.getColor();
            g.setColor( getParty().getPartyColor() );
            g.fillRect( getWidth() - 8, 1, 7, 7 );
            g.setColor( cc );
        }
        
        
        // Other elements of the interface
        
        if ( SelectedObject == this )
        {
            // Let's paint the InfoBars now
            
            Object aList[] = this.alBars.toArray();
            int iCC;
            
            for ( iCC = 0; iCC < aList.length; iCC++ )
            {
                ( (JWarInfoBar)aList[iCC]).paintInfoBar( g, getWidth(), getHeight() );
            }  
            
            g.setColor( Color.BLUE );
            g.drawRect( 0, 0, getWidth() - 1, getHeight() - 1 );
            g.drawLine( getWidth(), 0, getWidth(), getHeight() );
            
        }       
        
        
        g.setFont( ftPrev );
    }
    
    /**
     * Selects a new state(icon) for this Dynamic Object
     * @param iState
     */
    public void setObjectState( int iState )
    {
        iCurrentState = iState;
        iiCurrent = wsStates.getStateIcon( iState );
        this.repaint();
    }
    
    /**
     * Returns object's current state.
     * @return
     */
    public int getObjectState()
    {
        return iCurrentState;
    }
    
    /**
     * Returns the name of this object. Override in children!
     * @return
     */
    public String getObjectName()
    {
        return "Undef!";
    }
    
    /**
     * Returns the current object's position X in the grid.
     * @return
     */
    public int getPositionX()
    {
        return intPosX;
    }
    
    /**
     * Returns the current object's position Y in the grid.
     * @return
     */
    public int getPositionY()
    {
        return intPosY;
    }
    
    /**
     * Sets the current selected object.
     * @param obj
     */
    static public void setSelectedObject( JWarDynamicObject obj )
    {
        JWarDynamicObject soLast = SelectedObject;
        
        SelectedObject = obj;
        
        if ( soLast != null )
            soLast.repaint();
        
        if ( obj != null )
        {
            if ( obj.getAIModule() instanceof ControllableUnit )
                obj.getBattleField().setCursor( new Cursor( Cursor.CROSSHAIR_CURSOR)  );
            
            obj.repaint();
        }
        
        // If this dynamic object has an image let's get it 
        
        JWarUnitInfoPanel.setSelectedObject( obj );        
                
    }
    
    /**
     * Returns currently selected Object
     * @return
     */
    static public JWarDynamicObject getSelectedObject()
    {
        return SelectedObject;
    }
    
    /**
     * Adds a new info bar to the selected dynamic object.
     * @param inf
     */
    public void addInfoBar( JWarInfoBar inf )
    {
        inf.setBarOrder( alBars.size() );
        alBars.add( inf );
    }

    /**
     * Kills ( removes ) this object.
     *
     */
    public void killObject()
    {
        getBattleField().removeDynamicObject( this );

        if ( getAIModule() != null )
        {       
            getAIModule().discontinueAIModule();
        }
        
        alBars.clear();
    }
        
    /**
     * Returns all the infobars of this object;
     * @return
     */
    public JWarInfoBar[] getInfoBars()
    {
        Object[] ar  = alBars.toArray();
        JWarInfoBar[] ib = new JWarInfoBar[ ar.length ];
        
        for ( int iCnt = 0; iCnt < ar.length; iCnt++ )
            ib[iCnt] = (JWarInfoBar)ar[iCnt];
        
        return ib;
    }
    
    public void mouseClicked(MouseEvent arg0) 
    {
        if ( arg0.getButton() == MouseEvent.BUTTON1 )
        {
           JWarDynamicObject.setSelectedObject( this );
        }
    }

    public void mousePressed(MouseEvent arg0) {}
    public void mouseReleased(MouseEvent arg0) {}
    public void mouseEntered(MouseEvent arg0) {}
    public void mouseExited(MouseEvent arg0) {}
    
}
