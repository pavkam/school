package Objects;
import java.awt.Color;
import java.awt.Graphics;

/**
 * Class to represent an info bar for a Dynamic Object.
 * @author Ciobanu Alexander
 *
 */
public class JWarInfoBar {
    
    public static int INFO_BAR_HEIGHT = 10;
    
    public static String INFO_TYPE_LIFE = "Life";
    public static String INFO_TYPE_MINERALS = "Minerals";
    public static String INFO_TYPE_BUILD = "Build Time";
    
    private Color clrInfoBar;
    private int intMaxVal;
    private int intVal;
    private int intOrder;
    private String sName;
    
    /**
     * Constructor for this class.
     * @param c
     * @param iMaxVal
     * @param iNowVal
     */
    public JWarInfoBar( Color c, int iMaxVal, int iNowVal, String Name )
    {
        setMaxBarValue( iMaxVal );
        setBarValue( iNowVal );
        
        sName = Name;
        clrInfoBar = c;
    }
    
    /**
     * Sets the info bar max value.
     * @param iMaxVal
     */
    public void setMaxBarValue( int iMaxVal )
    {
        intMaxVal = iMaxVal;
    }

    /**
     * Sets the current info bar value.
     * @param iVal
     */
    public void setBarValue( int iVal )
    {
        intVal = iVal;
    }
    
    /**
     * Gets the value contained in this info bar.
     * @return
     */
    public int getBarValue()
    {
        return intVal;
    }
    
    /**
     * Gets the maximum value of this info bar.
     * @return
     */
    public int getMaxBarValue()
    {
        return intMaxVal;
    }
    
    /**
     * Sets the bar's order in the Objects's view area.
     * @param iOrder
     */
    public void setBarOrder( int iOrder )
    {
        intOrder = iOrder;
    }
    
    public void paintInfoBar( Graphics g, int iWidth, int iHeigth )
    {
        g.setColor( Color.PINK );
        g.drawRect( 0, iHeigth - ((intOrder + 1) * INFO_BAR_HEIGHT), iWidth, INFO_BAR_HEIGHT );
        
        g.setColor( clrInfoBar );
        g.fillRect( 1, iHeigth - ((intOrder + 1) * INFO_BAR_HEIGHT) + 1, iWidth - 1, INFO_BAR_HEIGHT - 1);

        int iSWD;
        String sTxt = getInfoBarVisualValue();
        
        
        g.setColor( Color.BLACK );        
        iSWD = g.getFontMetrics().stringWidth( sTxt );
        g.drawString( sTxt , ( iWidth - iSWD ) / 2, 
                (iHeigth - ((intOrder + 1) * INFO_BAR_HEIGHT)) + g.getFont().getSize() );
    }
    
    /**
     * Returns info bar's name.
     * @return
     */
    public String getInfoBarName()
    {
        return sName;
    }
    
    /**
     * Returns InfoBars' color.
     * @return
     */
    public Color getInfoBarColor()
    {
        return clrInfoBar;
    }    
    
    /**
     * Returns the visual display value of this info bar.
     * @return
     */
    public String getInfoBarVisualValue()
    {
        return String.valueOf( intVal ) + "/" + String.valueOf( intMaxVal );
    }    

}
