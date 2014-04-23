package edu.pingpong.physycs;
/*
 * Objects that implement this interface can be considered
 * mass-like physycal objects and used in PPPhysycs.
 */

public interface IMassObject 
{
	double getMass();
	void updateVisuals( int iX, int iY, int iZ );
	
	boolean collisionOnX( int iX, int iWallWidth, boolean bOnlyWall );
	boolean collisionOnY( int iY, int iWallHeight, boolean bOnlyWall);
	
	boolean collisionOnRect( int iY, int iX, int iSlapX, int iSlapY, int iSlapWidth, int iSlapHeight, boolean bLeft);
}
