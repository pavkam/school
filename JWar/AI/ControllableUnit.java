package AI;

/**
 * AI Classes that implement this interface will be considered
 * controllable by either a Computer or Human opponent.   
 * 
 * @author Ciobanu Alexander
 *
 */
public interface ControllableUnit 
{
    void sendAIPositionChange( int iNewX, int iNewY ); 
    void sendAIStopMovement();
    boolean canSendAIStopMovement();
}
