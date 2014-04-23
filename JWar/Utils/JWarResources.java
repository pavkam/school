package Utils;

import java.io.File;
import java.io.IOException;

import javax.swing.ImageIcon;
import javax.swing.JEditorPane;

/**
 * This class provides access to files on the local
 * hard disk in a standartized manner.
 * 
 * @author Ciobanu Alexander
 *
 */
final public class JWarResources {
    
    private static String DataPath = getProgramDataDirectory();
    
    /**
     * Returns the data path to be used when accesing files. 
     * @return Data path on the local hard drive.
     */
    private static String getProgramDataDirectory()
    {
        File fdNew;        
        int iIndex;
        String sSeparator;
        String sCWD;

        fdNew = new File( "." );
        sSeparator = File.separator;
        iIndex = fdNew.getAbsolutePath().lastIndexOf( sSeparator );

        try
        {
            sCWD = fdNew.getAbsolutePath().substring( 0, iIndex );
        }
        catch ( StringIndexOutOfBoundsException e )
        {
            return "";
        }

        return sCWD + sSeparator + "data" + sSeparator;
    }
    
    /**
     * Loads a specified Icon from the local source.
     * @param sName The name of the Image to load. Note that no extension is required.
     * @return ImageIcon object containing the loaded image.
     */
    static public ImageIcon loadIcon( String sName )
    {
        return new ImageIcon( DataPath + sName + ".jpg" );
    }
   
    /**
     * Loads a specified HTML web page from the local source.
     * @param sName The name of the HTML page to load. Note that no extension is required.
     * @return JEditorPane containing the HTML page.
     * @throws IOException
     */
    static public JEditorPane getHTMLPane( String sName ) throws IOException
    { 
        return new JEditorPane( "file:" + DataPath + sName + ".html" );
    }    

}
