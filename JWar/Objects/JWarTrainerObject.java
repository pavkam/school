package Objects;

import java.awt.Color;

public class JWarTrainerObject extends JWarLifeObject {
    
    static public int OBJECT_STATE_PREV = JWarLifeObject.OBJECT_STATE_PREV;
    
    private JWarInfoBar ibTrain;
    
    public void setupObject()
    {
        super.setupObject();
        
        ibTrain = new JWarInfoBar( Color.RED, 0, 0, JWarInfoBar.INFO_TYPE_BUILD );
        addInfoBar( ibTrain );
    }
    
    /**
     * Returns Object's Training.
     * @return
     */
    public int getObjectTrainStatus()
    {
        return ibTrain.getBarValue();
    }

    /**
     * Returns Object's Max Training.
     * @return
     */
    public int getObjectMaxTrainStatus()
    {
        return ibTrain.getMaxBarValue();
    }
    
    /**
     * Sets Objects's Max Training.
     * @param iMax
     */
    public void setObjectMaxTrainStatus( int iMax )
    {
        ibTrain.setMaxBarValue( iMax );
        
        repaint();
    }    
    
    /**
     * Sets Object's Training.
     * @param iTrain
     */
    public void setObjectTrainStatus( int iTrain )
    {
        ibTrain.setBarValue( iTrain );
        
        repaint();
    }
    
}
