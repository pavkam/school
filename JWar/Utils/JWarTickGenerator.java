package Utils;

import java.util.ArrayList;

/**
 * Global Tick Generator is a separate thread runner used to
 * update the given objects and avoid Concurent Access problems when
 * running multiple threads.
 * 
 * @author Ciobanu Alexander
 *
 */
final public class JWarTickGenerator extends Thread 
{

    /**
     * The Sleep amount of time. 
     */
    public static int RECOMMENDED_TIME = 50;
    
    private int intUpdateTime;
    private boolean bShouldStop = false;
    
    /**
     * Global Tick generator Object. Instancioated in the main method.
     */
    static public JWarTickGenerator GlobalTicks;
    
    private ArrayList alSubscribers = new ArrayList();
    
    /**
     * Generic Constructor. Accepts the Sleep time.
     * @param iUpdateTime Sleep time before next round of execution.
     */
    public JWarTickGenerator( int iUpdateTime )
    {
        intUpdateTime = iUpdateTime;
    }
    
    /*
     *  (non-Javadoc)
     * @see java.lang.Runnable#run()
     */
    public void run()
    {
        int iCC;
        
        /* Infinite loop for continuos update */
        while ( true )
        {
            if ( bShouldStop ) break; // Die if requested.
            
            /* First, wait the given time, then try do do the job */
            try {
                Thread.sleep( intUpdateTime );
            } catch (InterruptedException e) {
                break; // Break the thread
            }
            
            
            /* Starting the actual processings */
            
            iCC = 0;
            
            while ( iCC < alSubscribers.size() )
            {
                TickReceiver tk =  (TickReceiver)alSubscribers.get( iCC );
                
                tk.TickReceived();
                
                iCC++;
            }
            
        }
        
        iCC = 0;
        
        while ( iCC < alSubscribers.size() )
        {
            TickReceiver tk =  (TickReceiver)alSubscribers.get( iCC );
            
            tk.UnregisteredFromTicks();
            
            iCC++;
        }        
        
        
    }
    
    /**
     * Tells Tick Manager to stop it's execution! Not recommended for use alone.
     * Use @see Utils.JWarTickGenerator#safeStop().
     */
    public void setStopFlag()
    {
        bShouldStop = true;
    }
    
    /**
     * Sets the stop flag and waits for thread to actually stop, before exiting this method.
     *
     */
    public void safeStop()
    {
        setStopFlag();

        while ( this.isAlive() )
        {
            try {
                Thread.sleep( RECOMMENDED_TIME );
            } catch (InterruptedException e) {}
        }
            
        alSubscribers.clear();     
    }
    
    /**
     * Add a new Tick receiver to the list.
     * @param tk Tick receiver that will be notified about new ticks.
     */
    public void subscribeForTicks( TickReceiver tk )
    {
        alSubscribers.add( tk );
        tk.RegisteredForTicks();
    }
    
    /**
     * Removes a Tick Receiver.
     * @param tk Tick receiver object.
     */
    public void unsubscribeFromTicks( TickReceiver tk )
    {
        alSubscribers.remove( tk );
        tk.UnregisteredFromTicks();
    }
        
}
