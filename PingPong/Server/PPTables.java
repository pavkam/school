import java.util.ArrayList;


public class PPTables 
{
	private ArrayList alTables;
	final static private int TABLES_PER_PAGE = 10;
	
	public PPTables()
	{
		alTables = new ArrayList();
	}

	public int getNumberOfTables()
	{
		int iR = (alTables.size() / TABLES_PER_PAGE);
		
		if ( (alTables.size() % TABLES_PER_PAGE) != 0 )
			return iR+1; else
				return iR;
	}
	
	public synchronized Object[] getTablePage( int iPg )
	{
		int i, u;
		ArrayList al = new ArrayList();
		
		for (i = iPg * TABLES_PER_PAGE, u = 0; i < alTables.size() && u < TABLES_PER_PAGE; i++, u++)
		{
			al.add( alTables.get(i) );
		}
		
		return al.toArray();
	}	

	public synchronized PPTableInfo getTable( PPWorker me )
	{
		for (int i=0; i<alTables.size(); i++)
		{
			if ( ((PPTableInfo)alTables.get(i)).playerAtTable(me) )
				return ((PPTableInfo)alTables.get(i));
		}
		
		return null;
	}

	public synchronized boolean createTable( PPWorker me )
	{
		if ( getTable( me ) != null ) return false;
		
		alTables.add( new PPTableInfo( me, null ) );
		return true;
	}
	
	public synchronized boolean dropTable( PPWorker me )
	{
		if ( getTable( me ) == null ) return false;
		
		PPTableInfo x = getTable( me );
		x.dropWorker( me );
		
		alTables.remove( x );
			
		return true;
	}	
	
	public synchronized void dropTable( PPTableInfo me )
	{
		alTables.remove( me );
	}		
}
