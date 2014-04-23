package AI;

/**
 * AI Classes that do implement this interface posses Building/Mining options
 * and will be treated respectively and protected by attack units.
 * @author Ciobanu Alexander
 *
 */
public interface BuildingOptions {

    /**
     * Commands building of the Main Building.
     *
     */
    void buildMainBuilding();
    
    /**
     * Commands this Builder to start resource collection. 
     *
     */
    void startResourceCollection();
    
    /**
     * Checks if the building of the "Main Building" is possible.
     * @return Check Result.
     */
    boolean canBuildMainBuilding();
    
    /**
     * Checks if the collection of the resources is possible.
     * @return Check Result.
     */
    boolean canStartResourceCollection();    
}
