package edu.pingpong.physycs;
/*
 * This class implements the actual ball that will be used in the game.
 */


import java.awt.Color;
import java.awt.Graphics;

import javax.swing.JPanel;

public class PPBall extends JPanel implements IMassObject
{
	private final int RADIX = 6;
	private final double MASS  = 0.1; // 100 grams :D

	public PPBall()
	{
		setSize( (RADIX * 2) + 1, (RADIX * 2) + 1 );
	}
	
	public void paint( Graphics g )
	{
		int iW = (int)((getWidth() - 1) / 2.0);
		int iH = (int)((getHeight() - 1) / 2.0);
		
		g.setColor( Color.WHITE );		
		g.fillOval( iW - RADIX, iW - RADIX, iH + RADIX, iH + RADIX );
	}

	public double getMass() 
	{
		return MASS;
	}

	public boolean collisionOnX( int iX, int iWallWidth, boolean bOnlyWall )
	{
		if ( iX <= RADIX && !bOnlyWall) return true;
		if ( (iWallWidth - iX) <= RADIX ) return true;
		
		return false;
	}
	
	public boolean collisionOnY( int iY, int iWallHeight, boolean bOnlyWall)
	{
		if ( iY <= RADIX && !bOnlyWall) return true;
		if ( (iWallHeight - iY) <= RADIX ) return true;
		
		return false;
	}
	
	public boolean collisionOnRect( int iY, int iX, int iSlapX, int iSlapY, int iSlapWidth, int iSlapHeight, boolean bLeft )
	{
		if (bLeft)
		{
			if ( (iX + RADIX) == iSlapX &&
					(iY + RADIX) >= iSlapY && (iY - RADIX) <= (iSlapY + iSlapHeight))
					return true;
		} else
		{
			if ( (iX - RADIX) == (iSlapX + iSlapWidth) &&
					(iY + RADIX) >= iSlapY && (iY - RADIX) <= (iSlapY + iSlapHeight))
					return true;			
		}

		return false;		
	}
	
	public void updateVisuals( int iX, int iY, int iZ )
	{
		setLocation( 
				iX - (getWidth()/ 2), 
				iY - (getHeight()/ 2)
				);
	}
}
