package Objects;
import java.awt.Component;
import java.awt.Graphics;
import java.util.Random;

import javax.swing.ImageIcon;

import Utils.JWarResources;

/**
 * Terrain Object is to be used with JBattleField and specifies
 * an object to be placed on the battle terrain 
 * @author Schultz
 *
 */
public class JWarTerrainObject {
    
    public static int TERRAIN_LAND  = 0; 
    public static int TERRAIN_TREE  = 1;
    public static int TERRAIN_WATER = 2;
    
    private static ImageIcon[] icoTr_Land  = new ImageIcon[4];
    private static ImageIcon[] icoTr_Water = new ImageIcon[4];
    private static ImageIcon[] icoTr_Tree  = new ImageIcon[4];
    
    
    /**
     * Returns the terrain type by given pseudo-type id.
     * @param iID
     * @return
     */
    static public int getTerrainType( int iID )
    {
        if ( ( iID < 4 ) && ( iID >= 0 ) )
            return TERRAIN_LAND;
        
        if ( ( iID < 8 ) && ( iID >= 4 ) )
            return TERRAIN_TREE;
        
        if ( ( iID < 12 ) && ( iID >= 8 ) )
            return TERRAIN_WATER;  
        
        return -1;
    }
    
    /**
     * Returns true if the specified pseudo-terrain ID represents a free location.
     * @param iID
     * @return
     */
    static public boolean terrainIsFree( int iID )
    {
        return ( getTerrainType( iID ) == TERRAIN_LAND );
    }
    
    
    /**
     * Draws this terrain object to a canvas at given coordinates
     * and with given cell size
     * @param iX
     * @param iY
     * @param iCellDim
     * @param g
     */
    static public void drawToCell( int iTID, int iX, int iY, int iCellDim, Graphics g, Component c )
    {
        if ((iTID < 4) && (iTID >=0 ))            
            icoTr_Land[ iTID ].paintIcon( c, g, (iX * iCellDim) + 1, (iY * iCellDim) + 1 );
        
        if ((iTID < 8) && (iTID >=4 ))           
            icoTr_Tree[ iTID - 4 ].paintIcon( c, g, (iX * iCellDim) + 1, (iY * iCellDim) + 1 );
        
        if ((iTID < 12) && (iTID >=8 ))
            icoTr_Water[ iTID - 8 ].paintIcon( c, g, (iX * iCellDim) + 1, (iY * iCellDim) + 1 );        
    }
    
    /**
     * Loads the data files from the local directory.
     *
     */
    static public void loadTerrainInfo()
    {
        icoTr_Land[0]  = JWarResources.loadIcon( "land00" );
        icoTr_Land[1]  = JWarResources.loadIcon( "land01" );
        icoTr_Land[2]  = JWarResources.loadIcon( "land02" );
        icoTr_Land[3]  = JWarResources.loadIcon( "land03" );

        icoTr_Water[0]  = JWarResources.loadIcon( "water00" );
        icoTr_Water[1]  = JWarResources.loadIcon( "water01" );
        icoTr_Water[2]  = JWarResources.loadIcon( "water02" );
        icoTr_Water[3]  = JWarResources.loadIcon( "water03" );

        icoTr_Tree[0]  = JWarResources.loadIcon( "tree00" );
        icoTr_Tree[1]  = JWarResources.loadIcon( "tree01" );
        icoTr_Tree[2]  = JWarResources.loadIcon( "tree02" );
        icoTr_Tree[3]  = JWarResources.loadIcon( "tree03" );
    }

    /**
     * Returns a random icon ID for a given terrain type.
     * @param iType
     * @return
     */
    static public int assignRandomTerrain( int iType )
    {
        Random aRnd = new Random();
        return (iType * 4) + aRnd.nextInt( 4 );        
    }
}
