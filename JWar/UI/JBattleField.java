package UI;

import java.awt.Color;
import java.awt.Cursor;
import java.awt.Font;
import java.awt.Graphics;
import java.util.ArrayList;
import java.util.Random;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;

import javax.swing.JPanel;

import AI.ControllableUnit;
import AI.JWarParty;
import Objects.JWarBuilderMachine;
import Objects.JWarCritterObject;
import Objects.JWarDynamicObject;
import Objects.JWarMineralObject;
import Objects.JWarTerrainObject;

/**
 * JBattleField provides both Visual and Logical support for battle 
 * operation in the game. It can be considered the most important component
 * of the entire visual part of the Game.
 * 
 * @author Ciobanu Alexander
 *
 */
public final class JBattleField extends JPanel implements MouseListener 
{   
    /**
     * The dimension in Pixels of the visual Cell drawn on the Field.
     */
    static public int CELL_DIM   = 50;
    
    static private int MAX_FORREST_COUNT = 10;
    static private int MAX_FORREST_RCT   = 6;
    static private int FORREST_RCT_MT    = 10;
    static private int MAX_FORREST_DRAD  = 6;    
    static private int MAX_RIVER_COUNT   = 10;
    static private int MAX_RIVER_LEN     = 10;    
    static private int MIN_RIVER_LEN     = 5;
    static private int RESOURCE_DENSITY  = 2;
    static private int CRITTER_DENSITY   = 1;

    private int intWidth;
    private int intHeight;
    
    private int[] MapInstance;    
    private ArrayList alObjects = new ArrayList();

    /**
     * Generic Constructor.
     *
     */
    public JBattleField()
    {
        setLayout( null );
        
        addMouseListener( this );
    }
    
    /**
     * Returns the map instance used by this Battle Field.
     * @return Array containing all terrain objects IDs.
     */
    public int[] getMapInstance()
    {
        return MapInstance;
    }
    
    /**
     * Resizes the Battle Field to the newest selected field
     * sizes.
     * @param iWidth New selected Width in Cell Units.
     * @param iHeight New selected Height in Cell Units.
     */
    public void setFieldSizes( int iWidth, int iHeight )
    {
        intWidth  = iWidth; 
        intHeight = iHeight;
        
        /*
         * Create a new instance of the field
         */
        MapInstance   = new int[ iWidth * iHeight ];
    }
    
    /**
     * Returns an item from the map. Note: no bounds checks are performed.
     * @param X The Cell's X coordinate in Cell Units.
     * @param Y The Cell's Y coordinate in Cell Units.
     * @return Terrain Object ID.
     */
    public int getMapCell( int X, int Y )
    {
        return MapInstance[ (Y * intWidth) + X ];
    }
    
    /**
     * Sets an item into the map cell. Note: no bounds checks are performed.
     * @param X The Cell's X coordinate in Cell Units.
     * @param Y The Cell's Y coordinate in Cell Units.
     * @param iID The Cell's Terrain ID number.
     */
    public void setMapCell( int X, int Y, int iID )
    {
        MapInstance[ (Y * intWidth) + X ] = iID;
    }
    
    /**
     * Method does paint a field grid on the canvas surface.
     * @param g Given graphics object to paint to.
     */
    private void paintField(Graphics g)
    {
        int iX, iY;        
        Font ftSmall = new Font( "Fixedsys", 0, 8 );
        Font ftPrev  = g.getFont();   
                
        g.setFont( ftSmall );
        
        g.drawRect( 0, 0, getWidth() - 1, getHeight() - 1 );
        g.setColor( Color.BLACK );
        
        /* Vertical lines in the grid */
        for (iX = 0; iX < intWidth; iX++ )
        {
            g.drawLine( iX * CELL_DIM, 0, iX * CELL_DIM, getHeight() );
        }
        
        /* Horizontal lines in the grid */
        for (iY = 0; iY < intHeight; iY++ )
        {
            g.drawLine( 0, iY * CELL_DIM, getWidth(), iY * CELL_DIM );
        }      
        
        /*  Draw Cell Indexes */
        for (iX = 0; iX < intWidth; iX++ )
        {
            for (iY = 0; iY < intHeight; iY++ )
            {
                if ( getDynamicObject( iX, iY ) != null ) continue;
                    
                int iSWD, iSHT;
                String sTxt = String.valueOf( iX ) + ":" + String.valueOf( iY );
                
                iSWD = g.getFontMetrics().stringWidth( sTxt );
                iSHT = g.getFontMetrics().getHeight();
                
                g.drawString( sTxt ,
                               ((iX + 1) * CELL_DIM) - iSWD, 
                               ((iY) * CELL_DIM ) + iSHT );
            } 
        }
       
        g.setFont( ftPrev );
        
    }
     
    /**
     * Paints all terrain objects onto the field.
     * @param g Given graphics object to paint to.
     */
    public void paintTerrain(Graphics g)
    {
        int iX, iY;
        
        for (iX = 0; iX < intWidth; iX++ )
        {
            for (iY = 0; iY < intHeight; iY++ )
            {
                if ( getDynamicObject( iX, iY ) == null )
                {
                        JWarTerrainObject.drawToCell( getMapCell( iX, iY ), 
                                iX, iY, CELL_DIM, g, this );
                }
            } 
        }        
    }    
     
    /*
     *  (non-Javadoc)
     * @see java.awt.Component#paint(java.awt.Graphics)
     */
    public void paint(Graphics g)
    {
        try
        {
            super.paint( g );
        } catch ( ArrayIndexOutOfBoundsException e )
        {
            /* Ignore. Exception generated when a child is removed from the Tick thread*/
        }
        
        paintTerrain( g );        
        paintField( g );

    }
    
    /**
     * Re-populates the grid with another terrain and starting set
     * of Dynamic Objects.
     *
     */
    public void populateGrid()
    {
        int iX, iY;
        
        /* Clear the already available objects */
        
        while ( alObjects.size() > 0 )
        {
            JWarDynamicObject a = (JWarDynamicObject)alObjects.get( 0 );
            removeDynamicObject( a );
            
            a.killObject();
        }
         
        
        /* Assign the terrain */
        for ( iX = 0; iX < intWidth; iX++ )
            for (iY = 0; iY < intHeight; iY++)
            {
                int iTrr = JWarTerrainObject.assignRandomTerrain( JWarTerrainObject.TERRAIN_LAND );                     
                setMapCell( iX, iY, iTrr );
            }
        
        
        /* Create forrests */
        Random aRnd = new Random();
        
        
        /* Select a few points on the map to make them centers of our forrests */
        int iForrestsCount, iFI;
        
        while ( (iForrestsCount = aRnd.nextInt( MAX_FORREST_COUNT )) == 0 );
        
        for ( iFI = 0; iFI < iForrestsCount; iFI++ )
        {
            /* Now for each forrest let's create a random radius distribution */

            int iForrestTrees = ( aRnd.nextInt( MAX_FORREST_RCT ) + 1 ) * FORREST_RCT_MT;
            
            int iFX = aRnd.nextInt( intWidth );
            int iFY = aRnd.nextInt( intHeight );
            
            while (iForrestTrees > 0)
            {
                iForrestTrees--;
                
                iX    = iFX + ( MAX_FORREST_DRAD - aRnd.nextInt( MAX_FORREST_DRAD * 2 ) );
                iY    = iFY + ( MAX_FORREST_DRAD - aRnd.nextInt( MAX_FORREST_DRAD * 2 ) );
                int iFTrr = JWarTerrainObject.assignRandomTerrain( JWarTerrainObject.TERRAIN_TREE );

                if ( ( iX >= 0) && ( iX < intWidth ) && 
                     ( iY >= 0) && ( iY < intHeight ) )   
                setMapCell( iX, iY, iFTrr );
                
            }
        }
        
        
        /* Create waters (rivers) */
        
        /* Select a few points on the map to make them start of our rivers */
        int iRiversCount, iRC;
        
        while ( (iRiversCount = aRnd.nextInt( MAX_RIVER_COUNT )) == 0 );
        
        /* Now for each river ... */
        
        for ( iRC = 0; iRC < iRiversCount; iRC++ )
        {
            int iRX = aRnd.nextInt( intWidth );
            int iRY = aRnd.nextInt( intHeight );
            
            int iRvLen;
            
            while ( (iRvLen = ( aRnd.nextInt( MAX_RIVER_LEN ) + 1 )) < MIN_RIVER_LEN );
            
            while ( iRvLen > 0 )
            {
                iRvLen--;
                
                /* Get the new flow direction */
                int iDir = aRnd.nextInt( 4 );
                
                if (iDir == 0) iRX--; // Move Left
                if (iDir == 1) iRX++; // Move Right
                if (iDir == 2) iRY--; // Move Up
                if (iDir == 3) iRY++; // Move Down
                
                int iRTrr = JWarTerrainObject.assignRandomTerrain( JWarTerrainObject.TERRAIN_WATER );

                if ( ( iRX >= 0) && ( iRX < intWidth ) && 
                     ( iRY >= 0) && ( iRY < intHeight ) )   
                setMapCell( iRX, iRY, iRTrr );                
            }
        }
        
        
        /* Populate resources */
        
        int iResFields = ((intWidth * intHeight) / 100) * RESOURCE_DENSITY;
        
        
        for ( iRC = 0; iRC < iResFields; iRC++ )
        {
            int iRX = aRnd.nextInt( intWidth );
            int iRY = aRnd.nextInt( intHeight );
            
            if ( ( getDynamicObject( iRX, iRY ) == null ) && 
                 ( JWarTerrainObject.terrainIsFree( getMapCell( iRX, iRY ) ) ) )            
                this.addDynamicObject( new JWarMineralObject(), iRX, iRY );
        }
        
        /* Populate Critters */
        
        int iCritters = ((intWidth * intHeight) / 100) * CRITTER_DENSITY;
        
        for ( iRC = 0; iRC < iCritters; iRC++ )
        {
            int iRX = aRnd.nextInt( intWidth );
            int iRY = aRnd.nextInt( intHeight );
            
            if ( ( getDynamicObject( iRX, iRY ) == null ) && 
                 ( JWarTerrainObject.terrainIsFree( getMapCell( iRX, iRY ) ) ) )            
                this.addDynamicObject( new JWarCritterObject(), iRX, iRY );
        }
       
        /* Placing Opponents */
        JWarParty.initializeParties( this );
        
        placeOpponent( JWarParty.HUMAN );
        placeOpponent( JWarParty.COMPUTER_0 );
        
        if ( JWarParty.getGameType() > JWarParty.GAME_TYPE_NORMAL )
        {
            placeOpponent( JWarParty.COMPUTER_1 );
            placeOpponent( JWarParty.COMPUTER_2 );
        }
        
        
        this.repaint();
    }

    /**
     * Places an opponent's Builder onto the map.
     * @param pt The party to which this opponent is a part from. 
     */
    private void placeOpponent( JWarParty pt )
    {
        Random aRnd = new Random();
        
        while (true)
        {
            int iRX = aRnd.nextInt( intWidth );
            int iRY = aRnd.nextInt( intHeight );
            
            if ( cellIsFree( iRX, iRY ) )
            {
                JWarBuilderMachine bp = new JWarBuilderMachine();
                bp.setParty( pt );
            
                this.addDynamicObject( bp, iRX, iRY );
                
                break;
            }
        }        
    }

    /**
     * Places a new Dymanic object into the map.
     * @param obj The given Dynamic Object to place.
     * @param iX The Cell's X coordinate in Cell Units.
     * @param iY The Cell's Y coordinate in Cell Units.
     */
    public void addDynamicObject( JWarDynamicObject obj, int iX, int iY )
    {
        obj.setObjectPosition( iX, iY );
        obj.setBattleField( this );
        alObjects.add( obj );
        
        add( obj );
    }
    
    /**
     * Removes a given Dynamic Object from the map.
     * @param obj Given Dynamic Object to remove from the map.
     */
    public void removeDynamicObject( JWarDynamicObject obj )
    {
        obj.setVisible( false );
        alObjects.remove( obj );
        remove( obj );
        
        if ( JWarDynamicObject.getSelectedObject() == obj)
            JWarDynamicObject.setSelectedObject( null );
        
    }
    
    /**
     * Removes a given object from the map by it's coordinates.
     * @param iX The Cell's X coordinate in Cell Units.
     * @param iY The Cell's Y coordinate in Cell Units.
     */
    public void removeDynamicObject( int iX, int iY )
    {
        JWarDynamicObject obj = getDynamicObject( iX, iY );
        
        if ( obj != null )
            removeDynamicObject( obj );
    }
    
    /**
     * Returns the dynamic object at given coordinates.
     * @param iX The Cell's X coordinate in Cell Units.
     * @param iY The Cell's Y coordinate in Cell Units.
     * @return Dynamic Object located at that coordinates, or null if none found.
     */
    public JWarDynamicObject getDynamicObject( int iX, int iY )
    {
        Object[] aList = alObjects.toArray();
        int iC;
        
        for ( iC = 0; iC < aList.length; iC++ )
        {
            JWarDynamicObject o = ((JWarDynamicObject)aList[iC]);

            if ( o != null )
            {
                if ( o.objectIsMe( iX, iY ) )
                    { return o; }
            }
        }
        
        return null;
    }
    
    /**
     * Returns field's Width in Cells Units.
     * @return Field Width.
     */
    public int getFieldWidth()
    {
        return intWidth;
    }

    /**
     * Returns field's Height in Cells Units.
     * @return Field Width.
     */
    public int getFieldHeight()
    {
        return intHeight;
    }
    
    /**
     * Returns true if the given cell doesn't contain anything. It will check
     * if the terrain is proper for a ground unit and also if here is not another
     * object already there.
     * @param iX The Cell's X coordinate in Cell Units.
     * @param iY The Cell's Y coordinate in Cell Units.
     * @return Check result.
     */
    public boolean cellIsFree( int iX, int iY )
    {
        if (
                ( JWarTerrainObject.terrainIsFree( getMapCell( iX, iY ) ) ) &&
                ( getDynamicObject( iX, iY ) == null )
                ) 
            return true; else
                return false;
    
    }
    
    /*
     *  (non-Javadoc)
     * @see java.awt.event.MouseListener#mouseClicked(java.awt.event.MouseEvent)
     */
    public void mouseClicked(MouseEvent arg0) {
        
        if ( arg0.getButton() == MouseEvent.BUTTON1 )
        {
            JWarDynamicObject.setSelectedObject( null );
            
            setCursor( new Cursor( Cursor.DEFAULT_CURSOR)  );
            
            return;
        }
        
        if ( arg0.getButton() == MouseEvent.BUTTON3 )
        {
            JWarDynamicObject obj = JWarDynamicObject.getSelectedObject();
            
            if ( obj != null )
            {
                if ( ( obj.getAIModule() instanceof ControllableUnit ) && (obj.getParty() == JWarParty.HUMAN))
                {
                    /* Get the position of the cell which was clicked */
                    
                    int iCX = ( arg0.getX() / CELL_DIM ); 
                    int iCY = ( arg0.getY() / CELL_DIM );
                    
                    if ( JWarTerrainObject.terrainIsFree( getMapCell( iCX, iCY ) ) )
                    {
                        ((ControllableUnit)obj.getAIModule()).sendAIPositionChange( iCX, iCY );
                    }
                }
            }
        }
        
    }

    /*
     *  (non-Javadoc)
     * @see java.awt.event.MouseListener#mousePressed(java.awt.event.MouseEvent)
     */
    public void mousePressed(MouseEvent arg0) {
        // TODO Auto-generated method stub
        
    }

    /*
     *  (non-Javadoc)
     * @see java.awt.event.MouseListener#mouseReleased(java.awt.event.MouseEvent)
     */
    public void mouseReleased(MouseEvent arg0) {
        // TODO Auto-generated method stub
        
    }

    /*
     *  (non-Javadoc)
     * @see java.awt.event.MouseListener#mouseEntered(java.awt.event.MouseEvent)
     */
    public void mouseEntered(MouseEvent arg0) {
        // TODO Auto-generated method stub
        
    }

    /*
     *  (non-Javadoc)
     * @see java.awt.event.MouseListener#mouseExited(java.awt.event.MouseEvent)
     */
    public void mouseExited(MouseEvent arg0) {
        // TODO Auto-generated method stub
        
    }    
}
