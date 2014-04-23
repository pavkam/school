package AI;

public interface KillingOptions {

    void patrolNearHome();
    boolean canPatrolNearHome();
    
    void protectBuilders();
    boolean canProtectBuilders();
    
    void killNearestTarget();
    boolean canKillNearestTarget();   
     
}
