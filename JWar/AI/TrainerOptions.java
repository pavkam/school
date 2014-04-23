package AI;

/**
 * AI Classes that implement this interface will be considered as
 * candidates to be used as Main Buildings.
 * @author Ciobanu Alexander
 *
 */
public interface TrainerOptions 
{

    void trainBuilderMachine();
    void trainWarMachine();
    
    boolean canTrainBuilderMachine();
    boolean canTrainWarMachine();
}
