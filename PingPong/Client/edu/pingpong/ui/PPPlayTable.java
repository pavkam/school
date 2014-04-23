package edu.pingpong.ui;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.image.BufferedImage;

import javax.swing.ImageIcon;
import javax.swing.JLabel;

public class PPPlayTable extends JLabel {
	
	private void drawCircle( Graphics g, int iX, int iY, int iR )
	{
		int i;
		
		for (i = iY - iR; i <= iY + iR; i++ )
		{				
			int iCx = (int)(Math.sqrt( iR*iR - Math.pow( i - iY, 2 ) ) + iX);
			
			if (i % 4 == 0) continue;
			
			g.drawLine( iCx, i, iCx, i);
			g.drawLine( iX - (iCx-iX), i, iX - (iCx-iX), i);
		}
	}
	
	private ImageIcon generateTable()
	{
		BufferedImage im = new BufferedImage( getWidth(), getHeight(), BufferedImage.TYPE_BYTE_INDEXED );
		
	
		Graphics g = im.getGraphics();
		
		///
		
		g.setColor( Color.BLACK );
		g.fillRect( 0, 0, getWidth(), getHeight() );
		
		g.setColor( Color.WHITE );
		g.drawRect( 0, 0, getWidth() - 1, getHeight() - 1 );
		g.drawRect( 1, 1, getWidth() - 2, getHeight() - 2 );
		
		g.drawRect( (getWidth() / 2) - 1, 0, (getWidth() / 2) + 1, getHeight() );		
		
		int iX, iY, iR;
		
		iX = getWidth() / 2;
		iY = getHeight() / 2;
		iR = iY;
		
		drawCircle( g, iX, iY, iR );
				
		g.setColor( Color.DARK_GRAY );
		g.fillRect( 0 , 0, 5, getHeight() );
		g.fillRect( getWidth() - 5 , 0, getWidth(), getHeight() );
		
		return new ImageIcon( im );
	}
	
	public PPPlayTable( int iWidth, int iHeight )
	{
		setSize( iWidth, iHeight );
		setIcon( generateTable() );
		//setIcon(new ImageIcon(getClass().getClassLoader().getResource("data/table.JPG")));		
	}
}
