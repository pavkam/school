package org.ciobanu.school.ad.client;

import java.awt.GraphicsEnvironment;
import java.awt.Window;


public class ResolutionLocation {

	public static void setLocation(Window window)
	{
		int height = GraphicsEnvironment.getLocalGraphicsEnvironment().getDefaultScreenDevice().getDisplayMode().getHeight();
		int width = GraphicsEnvironment.getLocalGraphicsEnvironment().getDefaultScreenDevice().getDisplayMode().getWidth();
		
		window.setLocation((width - window.getSize().width) / 2, (height - window.getSize().height) / 2);
	}
}
