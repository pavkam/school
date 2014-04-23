
public class PPTableInfo 
{
	private PPWorker cOppLeft, cOppRight;
	
	public PPTableInfo( PPWorker cLeft, PPWorker cRight )
	{
		cOppLeft  = cLeft;
		cOppRight = cRight;
	}
	
	public PPWorker getRight()
	{
		return cOppRight;
	}
	
	public PPWorker getLeft()
	{
		return cOppLeft;
	}

	public boolean playerAtTable( PPWorker wrk )
	{
		return ( cOppRight == wrk || cOppLeft == wrk);
	}
	
	public void dropWorker( PPWorker wrk )
	{
		if (cOppRight == wrk && cOppRight != null)
			{ cOppLeft.dropFromPlay(); cOppRight = null; }
		
		if (cOppLeft == wrk && cOppRight != null)
			{ cOppRight.dropFromPlay(); cOppLeft = null; }		
	}
	
	public synchronized boolean addWorker( PPWorker wrk )
	{
		if (cOppRight != null )
		{ cOppRight.includeIntoPlay( wrk ); cOppLeft = wrk; return true; }
	
		if (cOppLeft != null )
		{ cOppLeft.includeIntoPlay( wrk ); cOppRight = wrk; return true; }
			
		return false;
	}
}
