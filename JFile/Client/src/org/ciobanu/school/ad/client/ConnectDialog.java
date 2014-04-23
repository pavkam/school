package org.ciobanu.school.ad.client;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.net.InetSocketAddress;
import java.net.Socket;
import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JTextField;

import org.ciobanu.school.ad.net.ClientThread;
import org.ciobanu.school.ad.net.DataPacket;

public class ConnectDialog extends JDialog {
	private JTextField jServerName;
	private JLabel jLabelServer;
	private JLabel jLabelPort;
	private JButton jConnect;
	private JButton jCancel;
	private JTextField jPort;

	private ClientThread theThread;
	
	public ConnectDialog() {
		initGUI();
	}

	public ClientThread connectToServer() {
		
		theThread = null;
		
		setModal(true);
		ResolutionLocation.setLocation(this);

		setVisible(true);

		return theThread;
	}

	private boolean connect(String host, String port) {
		int iPort;

		try {
			iPort = Integer.parseInt(port);

			if (host.length() < 1)
				throw new Exception();
		} catch (Exception e) {
			JOptionPane.showMessageDialog(this, "Invalid port or host value!",
					"Error", JOptionPane.ERROR_MESSAGE);
			return false;
		}


		try {
			Socket connectSocket = new Socket();
			connectSocket.connect(new InetSocketAddress(host, iPort));
			
			theThread = new ClientThread(connectSocket, host, iPort);
			theThread.startWorking();
			
			DataPacket helloPacket = new DataPacket();
			helloPacket.addParameter("HELLO");
			
			helloPacket = theThread.sendAndReceivePacket(helloPacket);
			
			if (helloPacket == null)
			{
				theThread.stopWorking();
				theThread = null;
				
				throw new Exception();
			}
			else
			{
				String message = new String(helloPacket.getParameter(0));
				
				if (!message.toUpperCase().equals("HELLO"))
					throw new Exception();
			}
		} catch (Exception e) {
			JOptionPane.showMessageDialog(this,
					"Cannot connect to the server!", "Error",
					JOptionPane.ERROR_MESSAGE);
			return false;
		}
		

		return true;
	}

	private void initGUI() {
		try {
			{
				getContentPane().setLayout(null);
				this.setTitle("Connect To Server ...");
				this.setResizable(false);
				{
					jServerName = new JTextField();
					getContentPane().add(jServerName);
					jServerName.setBounds(57, 4, 215, 22);
					jServerName.setText("localhost");
				}
				{
					jLabelServer = new JLabel();
					getContentPane().add(jLabelServer);
					jLabelServer.setText("Server:");
					jLabelServer.setBounds(4, 5, 47, 16);
				}
				{
					jLabelPort = new JLabel();
					getContentPane().add(jLabelPort);
					jLabelPort.setText("Port:");
					jLabelPort.setBounds(276, 7, 31, 16);
				}
				{
					jPort = new JTextField();
					jPort.setText("8081");
					getContentPane().add(jPort);
					jPort.setBounds(307, 4, 74, 22);
				}
				{
					jConnect = new JButton();
					getContentPane().add(jConnect);
					jConnect.setText("Connect");
					jConnect.setBounds(282, 49, 111, 22);
					jConnect.addActionListener(new ActionListener() {
						public void actionPerformed(ActionEvent evt) {
							if (connect(jServerName.getText(), jPort.getText()))
								setVisible(false);
						}
					});
				}
				{
					jCancel = new JButton();
					getContentPane().add(jCancel);
					jCancel.setText("Cancel");
					jCancel.setBounds(177, 49, 101, 22);
					jCancel.addActionListener(new ActionListener() {
						public void actionPerformed(ActionEvent evt) {
							setVisible(false);
						}
					});
				}
			}
			{
				this.setSize(409, 112);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}
