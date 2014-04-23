package edu.pingpong.physycs;

import edu.pingpong.net.PPConnector;

/*
 * Provides common means to control an object's state.
 * Data Specs:
 *  Speeds are expressed in pixels/sec => speed(in tick) = pixels / (sec/SIM_PERIOD) = (pixels * SIM_PERIOD) / sec =
 *  = (pixels * SIM_PERIOD) / 1000 msec
 *  
 * 
 */

public class PPPhysycalObjectState extends Thread
{
	private final int SIM_PERIOD = 1; // milliseconds
	
	private IMassObject moObject;
	private PPSlapper jSlapper;
	private double fX, fY, fZ;
	private int ivX, ivY, ivZ;
	
	private double fAccelX, fAccelY, fAccelZ;
	private double fSpeedX, fSpeedY, fSpeedZ;
	private int iWidth, iHeight;
	private boolean bDie;
	private PPConnector clientSession;
	
	private double fImpactDen;
	
	public void startSimulation()
	{
		PPThreadController.addThread();
		
		bDie = false;
		start();
	}
	
	public void stopSimulation()
	{
		bDie = true;
		
		while( bDie )
		{
			
			try
			{
				Thread.sleep( SIM_PERIOD ); // wait
			} catch (Exception e) {}
			
		}
		
		PPThreadController.removeThread();
	}
	
	private double accelDen( double fAccel )
	{
		if ( fAccel == 0 ) return 0;
		
		double sign = fAccel / Math.abs( fAccel );
		double newA = Math.abs(fAccelX) - fImpactDen;
		
		if ( newA < 0 ) newA = 0;
		
		return sign * newA;
	}
	
	public void checkSlap()
	{
		boolean bSlapped;
		bSlapped = ( moObject.collisionOnRect( (int)fY, (int)fX, 
					(int)jSlapper.getLocation().getX(),
					(int)jSlapper.getLocation().getY(),
					(int)jSlapper.getSize().getWidth(),
					(int)jSlapper.getSize().getHeight(), (fSpeedX > 0))
					);
		
		if (bSlapped)
			{				
				fSpeedX = -1 * fSpeedX;
				fAccelX = -1 * fAccelX;
				
				fAccelX = accelDen( fAccelX );
				
				clientSession.sendBallUpdate( fX, fY, fSpeedX, fSpeedY, fAccelX, fAccelY);
			}		
	}
	
	public void run()
	{
		int iX, iY, iZ;
		
		while ( !bDie )
		{
			if ( PPThreadController.stopSignaled() )
			{
				break;
			}
			
			// Simulate ph. parameters for the mass
			try
			{
				Thread.sleep( SIM_PERIOD ); // wait
			} catch (Exception e) {}
			
			fSpeedX += ( (fAccelX * SIM_PERIOD) / 1000 );
			fSpeedY += ( (fAccelY * SIM_PERIOD) / 1000 );
			fSpeedZ += ( (fAccelZ * SIM_PERIOD) / 1000 );			
			
			fX += ( (fSpeedX * SIM_PERIOD) / 1000 );
			fY += ( (fSpeedY * SIM_PERIOD) / 1000 );
			fZ += ( (fSpeedZ * SIM_PERIOD) / 1000 );
			
			// Check for hits 
			
			if ( moObject.collisionOnX( (int)fX, iWidth, false ) )
			{
				fSpeedX = -1 * fSpeedX;
				fAccelX = -1 * fAccelX;
				
				fAccelX = accelDen( fAccelX );
				
				if ( jSlapper.isRight() && (fX > (iWidth/2)) && (fX != 0) )
					clientSession.sendWallHit();
		
				if ( !jSlapper.isRight() && (fX < (iWidth/2)) && (fX != 0))
					clientSession.sendWallHit();				
			}
			
			if ( moObject.collisionOnY( (int)fY, iHeight, false ) )
			{
				fSpeedY = -1 * fSpeedY;
				fAccelY = -1 * fAccelY;
				
				fAccelY = accelDen( fAccelY ); 
			}
				
			if ( fZ <= 0 )
			{
				fSpeedZ = -1 * fSpeedZ;
				fAccelZ = -1 * fAccelZ;
				
				fAccelZ = accelDen( fAccelZ ); 
			}			
			
			checkSlap();
			
			iX = (int) fX;
			iY = (int) fY;
			iZ = (int) fZ;
			
			if ( iX != ivX || iY != ivY || iZ != ivZ )
			{
				ivX = iX;
				ivY = iY;
				ivZ = iZ;
				
				moObject.updateVisuals( ivX, ivY, ivZ );
			}
		}
		
		bDie = false;
	}
	
	public PPPhysycalObjectState( IMassObject massObject, int iRectWidth, int iRectHeight, PPSlapper obSlapper, PPConnector ppConn )
	{
		moObject = massObject;
		clientSession = ppConn;
		
		setXAcceleration( 0 );
		setYAcceleration( 0 );
		setZAcceleration( 0 );
		
		setXSpeed( 0 );
		setYSpeed( 0 );
		setZSpeed( 0 );
		
		setX( 0 );
		setY( 0 );
		setZ( 0 );
		
		setImpactDen( 0 );
		
		iWidth  = iRectWidth;
		iHeight = iRectHeight;
		
		jSlapper = obSlapper;
	}
	
	public double getXAcceleration()
	{
		return fAccelX;
	}
	
	public double getYAcceleration()
	{
		return fAccelY;
	}
	
	public double getZAcceleration()
	{
		return fAccelZ;
	}
	
	public double getXSpeed()
	{
		return fSpeedX;
	}
	
	public double getYSpeed()
	{
		return fSpeedY;
	}
	
	public double getZSpeed()
	{
		return fSpeedZ;
	}		

	public void setXAcceleration( double fNewAccel )
	{
		fAccelX = fNewAccel;
	}
	
	public void setYAcceleration( double fNewAccel )
	{
		fAccelY = fNewAccel;
	}
	
	public void setZAcceleration( double fNewAccel )
	{
		fAccelZ = fNewAccel;
	}	
	
	public void setXSpeed( double fNewSpd )
	{
		fSpeedX = fNewSpd;
	}	
	
	public void setYSpeed( double fNewSpd )
	{
		fSpeedY = fNewSpd;
	}
	
	public void setZSpeed( double fNewSpd )
	{
		fSpeedZ = fNewSpd;
	}	
	
	public double getX()
	{
		return fX;
	}
	
	public double getY()
	{
		return fY;
	}	

	public double getZ()
	{
		return fZ;
	}	

	public void setX( double fNew )
	{
		fX  = fNew;
		ivX = (int) fX;
	}
	
	public void setY( double fNew )
	{
		fY  = fNew;
		ivY = (int) fY;
	}
	
	public void setZ( double fNew )
	{
		fZ  = fNew;
		ivZ = (int) fZ;
	}	
	
	public double getImpactDen()
	{
		return fImpactDen;
	}	
	
	public void setImpactDen( double fNew )
	{
		fImpactDen  = fNew;
	}	

	public void updatePh( double x, double y, double sx, double sy, double ax, double ay)
	{
		fX = x;
		fY = y;
		fSpeedX = sx;
		fSpeedY = sy;
		fAccelX = ax;
		fAccelY = ay;
	}
}
