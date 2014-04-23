package edu.pingpong.physycs;
import java.awt.Point;

public class InBoundsCalculus 
{
	private int fX1, fY1, fX2, fY2;
	private int fWidth, fHeight;
	
	public InBoundsCalculus( int ffW, int ffH )
	{
		fX1 = 0;
		fY1 = 0;
		fX2 = 0;
		fY2 = 0;
		
		fWidth  = ffW;
		fHeight = ffH;
	}
	
	public void setPoint( int fX, int fY )
	{
		fX2 = fX1;
		fY2 = fY1;
		
		fX1 = fX;
		fY1 = fY;
	}
	
	public void setPoint( Point pIn )
	{
		setPoint( (int)pIn.getX(), (int)pIn.getY() );
	}
	
	public boolean containsPoint( int fX, int fY )
	{
		double fAreaMax;
		double fMyArea;
		Point A, B, C, D, M;
		
		A = new Point( fX1, fY1 );
		B = new Point( fX1 + fWidth, fY1 + fHeight);
		C = new Point( fX2, fY2 );
		D = new Point( fX2 + fWidth, fY2 + fHeight);
		M = new Point( fX, fY );
		
		fAreaMax = area( A, B, C ) + area( D, B, C );
		fMyArea  = area( M, A, B ) + area( M, B, C ) + 
				   area( M, C, D ) + area( M, A, D );
		
		return (fMyArea == fAreaMax);		
	}
	
	private double length( Point A, Point B )
	{
		return Math.sqrt( Math.pow(A.getX() - B.getX(),2) 
				+ Math.pow(A.getY() - B.getY(),2) );
	}
	
	private double area( Point A, Point B, Point C )
	{
		double l1 = length( A, B );
		double l2 = length( A, C );
		double l3 = length( C, B );
		double p  = l1 + l2 + l3;
		
		return Math.sqrt( p * (p - l1) * (p - l2) * (p -l3));
	}
	
	
}
