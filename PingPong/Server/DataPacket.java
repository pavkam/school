/*
 * This class manages raw data.
 */

public class DataPacket
{
	final static private int MAX_DATA = 1024;
	
	private byte aData[] = new byte[ MAX_DATA ];
	private int iStart = 0;
	private int iEnd   = 0;
	
	public DataPacket( byte[] arr )
	{
		System.arraycopy( arr, 0, aData, 0, arr.length );
		iEnd = arr.length;
	}
	
	public DataPacket()
	{}

	protected int getPStart()
	{
		return iStart;
	}
	
	protected int getPEnd()
	{
		return iEnd;
	}
	
	protected void setPStart( int start )
	{
		iStart = start;
	}	
	
	protected void setPEnd( int end )
	{
		iEnd = end;
	}		
	
	protected byte[] getData()
	{
		return aData;
	}
	
	public int getSize()
	{
		return iEnd - iStart;
	}
	
	public void clear()
	{
		iStart = 0;
		iEnd   = 0;
	}
	
	public void addByte( byte bt )
	{
		aData[iEnd] = bt;
		iEnd++;
	}
	
	public void addInt( int it )
	{
		addByte( (byte)( (it & 0x000000FF) ) );
		addByte( (byte)( (it & 0x0000FF00) >> 8 ) );
		addByte( (byte)( (it & 0x00FF0000) >> 16) );
		addByte( (byte)( (it & 0xFF000000) >> 24) );
	}
	
	public void addBoolean( boolean b )
	{
		if ( b )
			addByte( (byte)1 ); else
				addByte( (byte)0 );
	}
	
	public void addDouble( double b )
	{
		long l = Double.doubleToLongBits( b );
		addInt( (int)( (l >>  0) & 0x00000000FFFFFFFFL ) );
		addInt( (int)( (l >> 32) & 0x00000000FFFFFFFFL ) );
	}
	
	public void addString( String s )
	{
		char c[] = s.toCharArray();
		
		addInt( c.length );
		
		for (int i=0; i< c.length; i++)
		{
			addInt( (int)(c[i]));
		}
	}
	
	public byte getByte()
	{
		return aData[ iStart++ ];
	}
	
	public int getInt()
	{
		int iL0 = ((int)getByte() & 0xFF);
		int iL1 = ((int)getByte() & 0xFF);
		int iL2 = ((int)getByte() & 0xFF);
		int iL3 = ((int)getByte() & 0xFF);
		
		return iL0 | (iL1 << 8) | (iL2 << 16) | (iL3 << 24);
	}
	
	public double getDouble()
	{
		return Double.longBitsToDouble(
				( (( (long)getInt() & 0x00000000FFFFFFFFL) <<  0)) |
				( (( (long)getInt() & 0x00000000FFFFFFFFL) << 32))
				);
	}
	
	
	public boolean getBoolean()
	{
		if ( getByte() == 1 )
			return true; else
				return false;
	}
	
	public String getString()
	{
		int iLen = getInt();
		int i = 0;
		char c[] = new char[iLen];
		
		while (i<iLen)
		{
			c[i] = (char)getInt();
			i++;
		}
		
		return String.valueOf( c );
	}
	
	public void addArray( byte[] arr, int offs, int len )
	{
		for (int i=offs; i< len + offs; i++)
		{
			addByte( arr[i] );
		}
	}
	
	public void addArray( byte[] arr )
	{
		addArray( arr, 0, arr.length );
	}
	
	public void addPacket( DataPacket p )
	{
		addArray( p.getData(), p.getPStart(), p.getPEnd() - p.getPStart() );
	}
}
