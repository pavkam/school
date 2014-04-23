package AI;

/**
 * This class represents a square on the map.
 * @author Ciobanu Alexander
 *
 */
public class MapSquare {

    private MapPosition mpP1, mpP2;
    
    public MapSquare()
    {
        mpP1 = new MapPosition();
        mpP2 = new MapPosition();
    }
    
    public MapSquare( int iX1, int iY1, int iX2, int iY2)
    {
        mpP1 = new MapPosition( iX1, iY1 );
        mpP2 = new MapPosition( iX2, iY2 );
    }
    
    public MapPosition getFirstPosition()
    {
        return mpP1;
    }
    
    public MapPosition getSecondPosition()
    {
        return mpP2;
    }
    
}
