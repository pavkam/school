package AI;

import java.util.Random;

import Objects.JWarLifeObject;

public class JAICritter extends JAIGeneric {

    public JAICritter(JWarLifeObject parent) {
        super(parent);
        
        addStateChange( "Roaming randomly on the map." );
    }

    private Random aRandom = new Random();
    
    public void getNextDecision()
    {
        /* Get the next direction to move to */
        int iDir = aRandom.nextInt( 4 ) + JAIGeneric.AI_DIR_START;
        
        if ( canGoDirection( iDir ) )
            moveDirection( iDir );
    }    
    
    
    public String getAIModuleName()
    {
        return "Critter Module.";
    }    
    
}
