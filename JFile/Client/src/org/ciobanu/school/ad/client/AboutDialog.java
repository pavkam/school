package org.ciobanu.school.ad.client;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.BorderFactory;
import javax.swing.JButton;

import javax.swing.JDialog;
import javax.swing.JTextPane;
import javax.swing.border.BevelBorder;


public class AboutDialog extends JDialog {
	private JTextPane jAboutPane;
	private JButton jClose;

	public AboutDialog()
	{
		initGUI();
	}
	
	public void showAbout()
	{
		setModal(true);
		ResolutionLocation.setLocation(this);
		
		setVisible(true);
	}
	
	private void initGUI() {
		try {
			{
				this.setTitle("About");
				getContentPane().setLayout(null);
				this.setResizable(false);
				{
					jAboutPane = new JTextPane();
					getContentPane().add(jAboutPane);
					jAboutPane.setBounds(38, 6, 245, 104);
					jAboutPane.setBorder(BorderFactory.createEtchedBorder(BevelBorder.LOWERED));
					jAboutPane.setText("Proiect Aplicatii Distribuite:\n   1. Ciobanu Alexandru grupa 1512\n   2. Gugescu Bogdan grupa 1516\n\n\nAplicatie Client/Server FTP improvizata ");
					jAboutPane.setEditable(false);
				}
				{
					jClose = new JButton();
					getContentPane().add(jClose);
					jClose.setText("Close");
					jClose.setBounds(103, 116, 108, 22);
					jClose.addActionListener(new ActionListener() {
						public void actionPerformed(ActionEvent evt) {
							setVisible(false);
						}
					});
				}
			}
			{
				this.setSize(311, 184);
			}
		} catch(Exception e) {
			e.printStackTrace();
		}
	}

}
