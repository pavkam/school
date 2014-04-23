package AI;

import UI.JBattleField;

/**
 * This class offers Route calculation algorithm and is used directly
 * with AI.
 * @author Ciobanu Alexander
 *
 */
public class JWarRouteCalculator {
    
    private JAIControllable aiMy;
    
    /**
     * Instance constructor. Accepts the AI to give commands to as parameters.
     * @param ai
     */
    public JWarRouteCalculator( JAIControllable ai )
    {
        aiMy = ai;
    }
    
    private boolean fillDirections( int[] aMPP, int iX, int iY, int iWidth, int iHeight )
    {
        int iNow = aMPP[ (iY * iWidth) + iX ];
        boolean bWas = false;
            
        if ( iX > 0 )
        {
            if ( aMPP[ (iY * iWidth) + (iX - 1) ] == 0 )
            {
                aMPP[ (iY * iWidth) + (iX - 1) ] = iNow + 1;
                bWas = true;
            }
        }
        
        if ( iX < (iWidth - 1) )
        {
            if ( aMPP[ (iY * iWidth) + (iX + 1) ] == 0 )
            {
                aMPP[ (iY * iWidth) + (iX + 1) ] = iNow + 1;
                bWas = true;
            }
        }
        
        if ( iY > 0 )
        {
            if ( aMPP[ ((iY - 1) * iWidth) + iX ] == 0 )
            {
                aMPP[ ((iY - 1) * iWidth) + iX ] = iNow + 1;
                bWas = true;
            }
        }
        
        if ( iY < (iHeight - 1) )
        {
            if ( aMPP[ ((iY + 1) * iWidth) + iX ] == 0 )
            {
                aMPP[ ((iY + 1) * iWidth) + iX ] = iNow + 1;
                bWas = true;
            }
        }
        
        return bWas;
    }
    
    private MapPosition getMinRoad( int[] aMPP, int iX, int iY, int iWidth, int iHeight )
    {
        int iNow = aMPP[ (iY * iWidth) + iX ];
        
        int iPX_R = (iWidth * iHeight);
        int iPX_L = (iWidth * iHeight);
        int iPX_U = (iWidth * iHeight);
        int iPX_D = (iWidth * iHeight);
        
        MapPosition pR = new MapPosition( iX + 1, iY );
        MapPosition pL = new MapPosition( iX - 1, iY );
        MapPosition pU = new MapPosition( iX, iY - 1 );
        MapPosition pD = new MapPosition( iX, iY + 1 );
        
        if ( iX > 0 )
        {
            if ( aMPP[ (iY * iWidth) + (iX - 1) ] > 0 )
            {
                iPX_L = aMPP[ (iY * iWidth) + (iX - 1) ];
            }
        }
        
        if ( iX < (iWidth - 1) )
        {
            if ( aMPP[ (iY * iWidth) + (iX + 1) ] > 0 )
            {
                iPX_R = aMPP[ (iY * iWidth) + (iX + 1) ];
            }
        }
        
        if ( iY > 0 )
        {
            if ( aMPP[ ((iY - 1) * iWidth) + iX ] > 0 )
            {
                iPX_U = aMPP[ ((iY - 1) * iWidth) + iX ];
            }
        }
        
        if ( iY < (iHeight - 1) )
        {
            if ( aMPP[ ((iY + 1) * iWidth) + iX ] > 0 )
            {
                iPX_D = aMPP[ ((iY + 1) * iWidth) + iX ];
            }
        }
        
        int iRes = iNow;
               
        MapPosition pRes = null;
        
        if ( iPX_R < iRes ) { iRes = iPX_R; pRes = pR; }
        if ( iPX_L < iRes ) { iRes = iPX_L; pRes = pL; }
        if ( iPX_U < iRes ) { iRes = iPX_U; pRes = pU; }
        if ( iPX_D < iRes ) { iRes = iPX_D; pRes = pD; }
        
        return pRes;
    }
            
    public int calculateRoute( int iStartX, int iStartY, int iEndX, int iEndY, boolean bTrace )
    {
        JBattleField btl = aiMy.getParentObject().getBattleField();
        int[] aMPP = new int[ btl.getFieldWidth() * btl.getFieldHeight() ];
        
        int iX, iY;
        int iW = btl.getFieldWidth(); 
        int iH = btl.getFieldHeight();
        

        /* Fill up the temporary grid */
        for ( iX = 0; iX < iW; iX ++)
            for ( iY = 0; iY < iH; iY++ )
            {
                if ( btl.cellIsFree( iX, iY ) )
                   aMPP[ ( iY * iW ) + iX ] = 0; else
                   aMPP[ ( iY * iW ) + iX ] = -1;
            }
        
        aMPP[ ( iStartY * iW ) + iStartX ] = 1; // Start Point
        aMPP[ ( iEndY * iW ) + iEndX ] = 0; // End Point
        
        /* Start the Liniar process */
        
        boolean bSomethingToDo = true;
        
        while ( bSomethingToDo )
        {
            bSomethingToDo = false;
            
            for ( iX = 0; iX < iW; iX ++)
                for ( iY = 0; iY < iH; iY++ )
                {
                    if ( aMPP[ ( iY * iW ) + iX ] > 0 )
                        if ( fillDirections( aMPP, iX, iY, iW, iH ) )
                            bSomethingToDo = true;
                }
        }
        
        /* Let's lay out the road */
        int iDist = aMPP[ ( iEndY * iW ) + iEndX ] - 1;
        
        if (!bTrace)
            return iDist;
        
        if ( iDist != -1 )
        {
            int aRoadX[] = new int[ iDist ];
            int aRoadY[] = new int[ iDist ];
            
            iX = iEndX;
            iY = iEndY;
            
            while ( ( iX != iStartX ) || ( iY != iStartY ) )
            {
                aRoadX[ (iDist - 1) ] = iX;
                aRoadY[ (iDist - 1) ] = iY;

                MapPosition rd = getMinRoad( aMPP, iX, iY, iW, iH );
                
                iX = rd.getPositionX();
                iY = rd.getPositionY();
                
                iDist--;
            }
            
            
            /* Now transform the coordinate based road to a direction one and supply
             * it to the AI module for processing. 
             */
            
            iX = iStartX;
            iY = iStartY;
                       
            if ( aRoadX.length > 0)
            {
                aiMy.addStateChange( JAIControllable.GEN_STATE_USR_MOVE );
            } else
            {
                aiMy.addStateChange( JAIControllable.GEN_STATE_INACTIVE );                
            }
            
            
            for ( int iCC = 0; iCC < aRoadX.length; iCC++ )
            {
                if ( (aRoadX[iCC] - iX) == 1)
                    aiMy.moveDirection( JAIGeneric.AI_DIR_RIGHT );
                
                if ( (aRoadX[iCC] - iX) == -1)
                    aiMy.moveDirection( JAIGeneric.AI_DIR_LEFT );
                
                if ( (aRoadY[iCC] - iY) == 1)
                    aiMy.moveDirection( JAIGeneric.AI_DIR_DOWN );
                
                if ( (aRoadY[iCC] - iY) == -1)
                    aiMy.moveDirection( JAIGeneric.AI_DIR_UP );
                
                iX = aRoadX[iCC];
                iY = aRoadY[iCC];
                
                /* Add a check position step */
                if ( ((iCC+1) % 3) == 0)
                    aiMy.addCustomStep( JAIControllable.AI_OP_CHECK_POS, new MapSquare( iX, iY, iEndX, iEndY ) );
                               
                if ( iCC >= (aRoadX.length - 1))
                {
                    aiMy.addStateChange( JAIControllable.GEN_STATE_INACTIVE );
                }
            }
                    
            
            
            /* We have succesefully uploaded new directions to AI. Have fun! */
        }
        
        return -1;
    }
    

}
