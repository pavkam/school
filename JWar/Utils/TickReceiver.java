package Utils;

/**
 * Classes that do implement this Interface will be able to
 * register themselves to the TickGenerator.
 * 
 * @author Ciobanu Alexander
 *
 */
public interface TickReceiver 
{
  /**
   * Method will be invoked when TickGenerator has created another
   * tick.
   */
  void TickReceived();
  
  /**
   * Method will be invoked when the object implementing this interface
   * has been registered for tick notification.
   *
   */
  void RegisteredForTicks();
  
  /**
   * Method will be invoked when implementing class has been unregistered
   * from the tick generator.
   */
  void UnregisteredFromTicks();
  
}
