package edu.pingpong.physycs;

public class PPThreadController 
{
	static private int iThreadsRunning = 0;
	static private boolean bKill = false;
	
	static public synchronized void addThread()
	{
		iThreadsRunning++;
	}

	static public synchronized void removeThread()
	{
		iThreadsRunning--;
	}	
	
	static public void stopAllThreads()
	{
		bKill = true;
		
		while ( iThreadsRunning > 0 )
		{
			try
			{
				Thread.sleep( 10 );
			} catch ( Exception e )
			{
				
			}
		}
		
		bKill = false;
	}
	
	static public synchronized boolean stopSignaled()
	{
		return bKill;
	}	
}
