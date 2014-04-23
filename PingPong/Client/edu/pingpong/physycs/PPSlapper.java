package edu.pingpong.physycs;
/*
 * This class provides the functionality of a controller of the game.
 * You can control the ball with it. 
 */


import javax.swing.JPanel;
import java.awt.Graphics;
import java.awt.Color;
import java.awt.Point;

public class PPSlapper extends JPanel 
{	
	private boolean bRight;
	private double fX, fY;
	private InBoundsCalculus obClc; 
	
	public PPSlapper( boolean bbRight )
	{
		bRight = bbRight;		
		setSize( 5, 30 );
		
		obClc  = new InBoundsCalculus( getWidth(), getHeight() );		
	}	
	
	public boolean isRight()
	{
		return bRight;
	}
	
	public void setDelayedLocation( int X, int Y )
	{
		fX = getLocation().getX();
		fY = getLocation().getX();
		obClc.setPoint( getLocation() );
		
		setLocation( X, Y );
	}
	
	public Point getLastLocation()
	{
		return new Point( (int)fX, (int)fY );
	}
	
	public boolean ballInMoveRange( Point bll )
	{
		return obClc.containsPoint( (int)bll.getLocation().getX(), (int)bll.getLocation().getY() );
	}
	
	public void paint( Graphics g )
	{
		g.setColor( Color.blue );
		
		g.setColor( Color.blue );
		g.fillRect( 0, 0, getWidth(), getHeight() );
		
		g.setColor( Color.black );
		g.drawRect( 0, 0, getWidth(), getHeight() );
	}
	
}
