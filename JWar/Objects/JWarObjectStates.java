package Objects;
import java.util.ArrayList;
import javax.swing.ImageIcon;

import Utils.JWarResources;

/**
 * Identifies a list of states assigned to a dynamic object.
 * @author Ciobanu Alexander
 *
 */

public class JWarObjectStates {

    private ArrayList alStates = new ArrayList();
    private ArrayList alIDS    = new ArrayList();
    
    /**
     * Loads a given resource file for a state.
     * @param sName
     * @return
     */
    private ImageIcon loadResource( String sName )
    {
        ImageIcon iiTmp = JWarResources.loadIcon( sName );
        return iiTmp;
    }
    
    /**
     * Adds a new state and an ID to identify it.
     * @param sName
     * @param iID
     */
    public void addObjectState( String sName, int iID )
    {
        alStates.add( loadResource(sName) );
        alIDS.add( Integer.valueOf( iID ) );
    }
    
    
    public ImageIcon getStateIcon( int iStateID )
    {
        int iPX;
        Object[] aList = alIDS.toArray();
        
        for ( iPX = 0; iPX < aList.length; iPX++ )
        {
            if ( ((Integer)aList[iPX]).intValue() == iStateID )
            {
                return (ImageIcon)alStates.get( iPX );
            }
        }
        
        return null;
    }
}
