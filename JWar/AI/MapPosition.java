package AI;

public class MapPosition {
    private int intX;
    private int intY;
    
    /**
     * Generic constructor. Assigns the coordinates as given in parameters.  
     * @param iX
     * @param iY
     */
    public MapPosition( int iX, int iY )
    {
        setPositionX( iX );
        setPositionY( iY );
    }
    
    /**
     * Generic constructor. Assigns the coordinates to 0.
     *
     */
    public MapPosition()
    {
        setPositionX( 0 );
        setPositionY( 0 );        
    }
    
    /**
     * Sets the current position of X.
     * @param iX
     */
    public void setPositionX( int iX )
    {
        intX = iX;
    }
    
    /**
     * Sets the current position of Y.
     * @param iY
     */
    public void setPositionY( int iY )
    {
        intY = iY;
    }
    
    /**
     * Returns the X position.
     * @return
     */
    public int getPositionX()
    {
        return intX;
    }

    /**
     * Returns the Y position.
     * @return
     */
    public int getPositionY()
    {
        return intY;        
    }    
    
}
