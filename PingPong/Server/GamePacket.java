public class GamePacket extends DataPacket
{	
	public static final byte PKT_LOGIN = (byte)0xD1;	
	public static final byte PKT_ERROR = (byte)0xD2;
	public static final byte PKT_PING  = (byte)0xD3;
	public static final byte PKT_STATS = (byte)0xD4;
	public static final byte PKT_TABLS = (byte)0xD5;
	public static final byte PKT_CHALLENGE = (byte)0xD6;
	public static final byte PKT_SYNC  = (byte)0xD7;
	public static final byte PKT_DCONN = (byte)0xD8;
	public static final byte PKT_CHAT  = (byte)0xD9;
	public static final byte PKT_READY  = (byte)0xDA;
	public static final byte PKT_HIT    = (byte)0xDB;
	
	public static final byte TYPE_ERROR_REJECTED = (byte)0x01;
	public static final byte TYPE_ERROR_OK       = (byte)0x02;
	public static final byte TYPE_ERROR_NLOG     = (byte)0x03;
	public static final byte TYPE_ERROR_ALOG     = (byte)0x04;
	
	public static final byte PING_TYPE_SERVER    = (byte)0xFF;
	public static final byte PING_TYPE_CLIENT    = (byte)0xCC;
	
	public static final byte STAT_REQ_MINE       = (byte)0x01;
	public static final byte STAT_REQ_PG         = (byte)0x02;
	public static final byte STAT_REQ_ALL        = (byte)0x03;
	
	public static final byte TABL_REQ_MINE       = (byte)0x01;
	public static final byte TABL_REQ_PG         = (byte)0x02;
	public static final byte TABL_REQ_ALL        = (byte)0x03;
	public static final byte TABL_CREATE         = (byte)0x04;
	public static final byte TABL_DROP           = (byte)0x05;
	
	public static final byte CHAT_NOTICE         = (byte)0x00;
	public static final byte CHAT_SYNC           = (byte)0x01;
	public static final byte CHAT_CLIENT         = (byte)0x02;
	public static final byte CHAT_BACK           = (byte)0x03;
	
	public static final byte SYNC_OPP_NAME       = (byte)0x00;
	public static final byte SYNC_OPP_SLP        = (byte)0x01;	
	public static final byte SYNC_OPP_BALL       = (byte)0x02;
	
	
	private byte iPkOp;
	private int iSize;
	
	public GamePacket( byte pktcode )
	{
		iPkOp = pktcode;
		iSize = 5;
	}
	
	public GamePacket( DataPacket b )
	{
		this.addPacket( b );
		
		iPkOp  = getByte();
		iSize  = getInt();
	}
	
	static private int canExtractPacket( byte[] arr, int offs ) throws Exception
	{
		int iSizel;
		
		if (offs < 5) return -1;
		
		DataPacket a = new DataPacket();
		a.addArray( arr, 0, offs );
		a.getByte();
		
		iSizel = a.getInt();
		
		if (iSizel > 2048 || iSizel < 5)
			throw new Exception( "Invalid packet received!" );
		
		if ( ( iSizel - 5 ) <= a.getSize() )
			return iSizel; else return -1;
	}
	
	static public GamePacket extractPacket( byte[] arr, int offs ) throws Exception
	{
		int iSz = canExtractPacket(arr, offs); 
		if ( iSz == -1 ) return null;
		
		DataPacket b = new DataPacket();
		b.addArray( arr, 0, iSz );
		
		byte a2[] = new byte[ arr.length ];
		System.arraycopy( arr, iSz, a2, 0, arr.length - iSz );
		System.arraycopy( a2, 0, arr, 0, arr.length );
		
		return new GamePacket( b );
	}
	
	public byte getOpcode()
	{
		return iPkOp;
	}
	
	public void setOpcode( byte op )
	{
		iPkOp = op;
	}
	
	public byte[] packet()
	{
		DataPacket gm = new DataPacket();
		
		gm.addByte( getOpcode() );
		gm.addInt( iSize + getSize() );
		
		gm.addPacket( this );
		
		byte d[] = new byte[ iSize + getSize() ];
		System.arraycopy( gm.getData(), 0, d, 0, d.length );
		
		return d;
	}

}
