package AI;

import java.awt.Color;

import UI.JBattleField;

import Controllers.JCTRLComputer;
import Controllers.JCTRLGeneric;

/**
 * Specifies the party to which a selected unit applies to.
 * 
 * @author Ciobanu Alexander
 *
 */
public class JWarParty {
    
    static public int GAME_TYPE_NORMAL = 0;
    static public int GAME_TYPE_MELEE  = 1;
    static public int GAME_TYPE_FREE   = 2;
    static public int GAME_TYPE_2VS2   = 3;
    
    private static int intGameType = GAME_TYPE_NORMAL; 
    
    static public JWarParty HUMAN = null;
    static public JWarParty COMPUTER_0 = null;
    static public JWarParty COMPUTER_1 = null;
    static public JWarParty COMPUTER_2 = null;
    static public JWarParty NEUTRAL = new JWarParty( null, Color.BLUE, "Neutral", 0 );
    
    private Color pColor;
    private String sName;
    private int intResources;
    private JCTRLGeneric pController;
    

    /**
     * Default constructor. Accepts a color as parameter that identifies the party's color.
     * @param c
     */
    public JWarParty( JCTRLGeneric ctrl, Color c, String name, int res )
    {
        pColor = c;
        sName = name;
        intResources = res;
        pController = ctrl;
        
        if (pController != null)
            pController.setParty( this );
    }
    
    /**
     * Initializes the parties.
     *
     */
    public static void initializeParties( JBattleField btl )
    {
        HUMAN = new JWarParty( null, Color.PINK, "Human", 1000 );
        COMPUTER_0 = new JWarParty( new JCTRLComputer(), Color.RED, "Computer 0", 1000 );
        COMPUTER_1 = new JWarParty( new JCTRLComputer(), Color.CYAN, "Computer 1", 1000 );
        COMPUTER_2 = new JWarParty( new JCTRLComputer(), Color.WHITE, "Computer 2", 1000 );
        
        COMPUTER_0.getController().setBattleField( btl );
        COMPUTER_1.getController().setBattleField( btl );
        COMPUTER_2.getController().setBattleField( btl );
    }
    
    /**
     * Gets this Party's color.
     * @return
     */
    public Color getPartyColor()
    {
        return pColor;
    }
    
    /**
     * Returns party's name.
     * @return
     */
    public String getPartyName()
    {
        return sName;
    }
    
    /**
     * Retrns party's resources.
     * @return
     */
    public int getPartyResources()
    {
        return intResources;
    }    
    
    /**
     * Sets current party resources
     * @param iRes
     * @return
     */
    public void setPartyResources( int iRes )
    {
        intResources = iRes;
    } 
    
    /**
     * Decides if this party is in war with a given opponent party.
     * @param other
     * @return
     */
    public boolean inWarWith( JWarParty other )
    {
        if (this == HUMAN)
        {
            if ( other == COMPUTER_0 )
            {
                return true;
            }
            
            if ( other == COMPUTER_1 )
            {
                return true;
            }  
            
            if ( ( other == COMPUTER_2 ) && ( intGameType != GAME_TYPE_2VS2 ) )
            {
                return true;
            }             
        }
        
        if (this == COMPUTER_0)
        {
            if ( other == HUMAN )
            {
                return true;
            }
            
            if ( ( other == COMPUTER_1 ) && ( intGameType == GAME_TYPE_FREE ) )
            {
                return true;
            }  
            
            if ( ( other == COMPUTER_2 ) && ( (intGameType == GAME_TYPE_2VS2) || (intGameType == GAME_TYPE_FREE) ) )
            {
                return true;
            }             
        }
        
        if (this == COMPUTER_1)
        {
            if ( other == HUMAN )
            {
                return true;
            }
            
            if ( ( other == COMPUTER_0 ) && ( intGameType == GAME_TYPE_FREE ) )
            {
                return true;
            }  
            
            if ( ( other == COMPUTER_2 ) && ( (intGameType == GAME_TYPE_2VS2) || (intGameType == GAME_TYPE_FREE) ) )
            {
                return true;
            }             
        }
        
        if (this == COMPUTER_2)
        {
            if ( ( other == HUMAN ) && ( intGameType != GAME_TYPE_2VS2 ) )
            {
                return true;
            }
            
            if ( ( other == COMPUTER_0 ) && ( (intGameType == GAME_TYPE_2VS2) || (intGameType == GAME_TYPE_FREE) ) )
            {
                return true;
            }  
            
            if ( ( other == COMPUTER_1 ) && ( (intGameType == GAME_TYPE_2VS2) || (intGameType == GAME_TYPE_FREE) ) )
            {
                return true;
            }             
        }        
        
        return false;
    }
    
    
    /**
     * Selects currently used game type.
     * @param iGT
     */
    public static void setGameType( int iGT )
    {
        intGameType = iGT;
    }
    
    /**
     * Gets currently selected game type.
     * @return
     */
    public static int getGameType()
    {
        return intGameType;
    }
    
    /**
     * Returns the controller object.
     * @return
     */
    public JCTRLGeneric getController()
    {
        return pController;
    }
}
