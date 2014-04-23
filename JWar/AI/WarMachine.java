package AI;

/** 
 * All Dynamic objects that implement this interface are
 * considered candidates for Killer AI.
 * @author Ciobanu Alexander
 *
 */
public interface WarMachine 
{

    int getShootDistance();
    int getParalyseTiming();
    int getHomePatrolMaxDistance();
    int getMinimalProtectionDistance();
    int getMaximalHuntSelectDistance();
    int getDamagePerShot();
    int getGraveVisibilityTiming();
    int getDecisionTiming();
    
}
