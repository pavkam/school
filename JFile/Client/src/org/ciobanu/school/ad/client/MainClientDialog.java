package org.ciobanu.school.ad.client;

import java.awt.FlowLayout;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.util.ArrayList;

import javax.swing.DefaultComboBoxModel;
import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JList;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JSeparator;
import javax.swing.JSplitPane;
import javax.swing.ListModel;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;

import org.ciobanu.school.ad.net.ClientThread;
import org.ciobanu.school.ad.net.DataPacket;
import org.ciobanu.school.ad.net.FileInfoRecord;

public class MainClientDialog extends JDialog {
	private JMenuBar jMainMenu;
	private JMenuItem jServerConnectMenuItem;
	private JList jFileList;
	private JButton jDownloadFile;
	private JButton jUploadFile;
	private JButton jRenameFile;
	private JButton jDeleteFile;
	private JPanel jControlPanel;
	private JSplitPane jSplitPane;
	private JMenuItem jHelpAboutMenuItem;
	private JMenuItem jServerQuitMenuItem;
	private JSeparator jSeparator1;
	private JMenuItem jServerDisconnectMenuItem;
	private JMenu jServerMenu;
	private JMenu jMenuHelp;

	private boolean bIsConnected = false;
	private ClientThread theThread;

	{
		// Set Look & Feel
		try {
			javax.swing.UIManager.setLookAndFeel(javax.swing.UIManager
					.getCrossPlatformLookAndFeelClassName());
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	private void uiControl() {
		jFileList.setEnabled(bIsConnected);
		jServerConnectMenuItem.setEnabled(!bIsConnected);
		jDownloadFile.setEnabled(getSelectedFile() != null);
		jUploadFile.setEnabled(bIsConnected);
		jRenameFile.setEnabled(getSelectedFile() != null);
		jDeleteFile.setEnabled(getSelectedFile() != null);
		jServerQuitMenuItem.setEnabled(!bIsConnected);
		jServerDisconnectMenuItem.setEnabled(bIsConnected);

		if (!bIsConnected) {
			jFileList.setListData(new Object[] {});
		}
	}

	private FileInfoRecord getSelectedFile() {
		if (bIsConnected && jFileList.getSelectedIndex() > -1) {
			return (FileInfoRecord) jFileList.getSelectedValue();
		} else
			return null;
	}

	private String param(byte[] arr) {
		return new String(arr);
	}

	private ArrayList<FileInfoRecord> doGetFileList() {
		ArrayList<FileInfoRecord> files = new ArrayList<FileInfoRecord>();

		DataPacket dp = new DataPacket();
		dp.addParameter("LIST");
		dp.addParameter("./");

		dp = theThread.sendAndReceivePacket(dp);

		if (dp == null) {
			JOptionPane.showMessageDialog(this,
					"Failed to retreive server file list!", "Error",
					JOptionPane.ERROR_MESSAGE);
			doDisconnect();

			return files;
		}

		int fileCount = Integer.valueOf(param(dp.getParameter(0)));

		for (int i = 0; i < fileCount; i++) {
			FileInfoRecord fileInfo = new FileInfoRecord(param(dp.getParameter(1 + (i * 2))), 
					Long.parseLong(param(dp.getParameter(2 + (i * 2)))));
			
			files.add(fileInfo);
		}

		return files;
	}

	private void doDeleteFile(String fileName) {
		DataPacket dp = new DataPacket();
		dp.addParameter("DELETE");
		dp.addParameter("./" + fileName);

		dp = theThread.sendAndReceivePacket(dp);

		if (dp == null) {
			JOptionPane.showMessageDialog(this,
					"Failed delete the file! Server Error!", "Error",
					JOptionPane.ERROR_MESSAGE);
			doDisconnect();

			return;
		}

		if (param(dp.getParameter(0)).equals("0"))
			JOptionPane.showMessageDialog(this,
					"Selected file cannot be deleted!", "Error",
					JOptionPane.ERROR_MESSAGE);

	}

	private void doRenameFile(String fileName, String toFileName) {
		DataPacket dp = new DataPacket();
		dp.addParameter("RENAME");
		dp.addParameter("./" + fileName);
		dp.addParameter("./" + toFileName);

		dp = theThread.sendAndReceivePacket(dp);

		if (dp == null) {
			JOptionPane.showMessageDialog(this,
					"Failed to rename the file! Server Error!", "Error",
					JOptionPane.ERROR_MESSAGE);
			doDisconnect();

			return;
		}

		if (param(dp.getParameter(0)).equals("0"))
			JOptionPane.showMessageDialog(this,
					"Selected file cannot be rename to given name!", "Error",
					JOptionPane.ERROR_MESSAGE);

	}

	private void doDownloadFile(FileInfoRecord fileRec)
	{
		new DownloadDialog().downloadFile(theThread, fileRec);
	}
	
	private void doUploadFile()
	{
		new UploadDialog().uploadFile(theThread);
	}
	
	private void doConnect() {
		theThread = new ConnectDialog().connectToServer();

		if (theThread != null) {
			bIsConnected = true;

			ArrayList<FileInfoRecord> files = doGetFileList();
			jFileList.setListData(files.toArray());
		}

		uiControl();
	}

	public MainClientDialog() {
		initGUI();
		uiControl();
	}

	private void doDisconnect() {
		if (theThread != null && theThread.isAlive()) {
			theThread.stopWorking();
		}

		theThread = null;
		bIsConnected = false;

		uiControl();
		JOptionPane.showMessageDialog(this, "Disconnected from the server!",
				"OK", JOptionPane.INFORMATION_MESSAGE);
	}

	public void runApplication() {
		setModal(true);
		ResolutionLocation.setLocation(this);

		setVisible(true);
	}

	private void initGUI() {
		try {
			{
				this.setTitle("JFtp Client Program");
				GridLayout thisLayout = new GridLayout(1, 1);
				thisLayout.setColumns(1);
				thisLayout.setHgap(5);
				thisLayout.setVgap(5);
				getContentPane().setLayout(thisLayout);
				this.addWindowListener(new WindowAdapter() {
					public void windowClosed(WindowEvent evt) {

					}
				});
				{
					jSplitPane = new JSplitPane();
					getContentPane().add(jSplitPane);
					jSplitPane.setOrientation(JSplitPane.VERTICAL_SPLIT);
					{
						ListModel jFileListModel = new DefaultComboBoxModel(
								new String[] {});
						jFileList = new JList();
						jSplitPane.add(jFileList, JSplitPane.BOTTOM);
						jFileList.setModel(jFileListModel);
						jFileList.setPreferredSize(new java.awt.Dimension(511,
								441));
						jFileList
								.addListSelectionListener(new ListSelectionListener() {
									public void valueChanged(
											ListSelectionEvent evt) {
										uiControl();
									}
								});
					}
					{
						jControlPanel = new JPanel();
						FlowLayout jControlPanelLayout = new FlowLayout();
						jSplitPane.add(jControlPanel, JSplitPane.TOP);
						jControlPanel.setPreferredSize(new java.awt.Dimension(
								509, 43));
						jControlPanel.setOpaque(false);
						jControlPanel.setLayout(jControlPanelLayout);
						jControlPanel.setSize(481, 100);
						{
							jDeleteFile = new JButton();
							jControlPanel.add(jDeleteFile);
							jDeleteFile.setText("Delete");
							jDeleteFile
									.setPreferredSize(new java.awt.Dimension(
											106, 21));
							jDeleteFile.addActionListener(new ActionListener() {
								public void actionPerformed(ActionEvent evt) {

									if (getSelectedFile() != null) {
										doDeleteFile(getSelectedFile().getName());
										ArrayList<FileInfoRecord> files = doGetFileList();
										jFileList.setListData(files.toArray());
										uiControl();
									}
								}
							});
						}
						{
							jRenameFile = new JButton();
							jControlPanel.add(jRenameFile);
							jRenameFile.setText("Rename");
							jRenameFile
									.setPreferredSize(new java.awt.Dimension(
											99, 21));
							jRenameFile.addActionListener(new ActionListener() {
								public void actionPerformed(ActionEvent evt) {

									if (getSelectedFile() != null) {

										String newName = JOptionPane
												.showInputDialog(
														"New file name:",
														getSelectedFile().getName());

										if (newName == null
												|| newName.length() == 0)
											return;

										doRenameFile(getSelectedFile().getName(), newName);
										ArrayList<FileInfoRecord> files = doGetFileList();
										jFileList.setListData(files.toArray());
										uiControl();
									}
								}
							});
						}
						{
							jUploadFile = new JButton();
							jControlPanel.add(jUploadFile);
							jUploadFile.setText("Upload ...");
							jUploadFile
									.setPreferredSize(new java.awt.Dimension(
											126, 21));
							jUploadFile.addActionListener(new ActionListener() {
								public void actionPerformed(ActionEvent evt) {

									doUploadFile();

									ArrayList<FileInfoRecord> files = doGetFileList();
									jFileList.setListData(files.toArray());
									uiControl();
								}
							});
						}
						{
							jDownloadFile = new JButton();
							jControlPanel.add(jDownloadFile);
							jDownloadFile.setText("Download ...");
							jDownloadFile
									.setPreferredSize(new java.awt.Dimension(
											112, 21));
							jDownloadFile.addActionListener(new ActionListener() {
								public void actionPerformed(ActionEvent evt) {
									
									if (getSelectedFile() != null) {
										
										doDownloadFile(getSelectedFile());

										ArrayList<FileInfoRecord> files = doGetFileList();
										jFileList.setListData(files.toArray());
										uiControl();
									}
								}
							});
						}
					}
				}
				{
					jMainMenu = new JMenuBar();
					setJMenuBar(jMainMenu);
					{
						jServerMenu = new JMenu();
						jMainMenu.add(jServerMenu);
						jServerMenu.setText("Server");
						{
							jServerConnectMenuItem = new JMenuItem();
							jServerMenu.add(jServerConnectMenuItem);
							jServerConnectMenuItem.setText("Connect ...");
							jServerConnectMenuItem
									.addActionListener(new ActionListener() {
										public void actionPerformed(
												ActionEvent evt) {
											doConnect();
										}
									});
						}
						{
							jServerDisconnectMenuItem = new JMenuItem();
							jServerMenu.add(jServerDisconnectMenuItem);
							jServerDisconnectMenuItem.setText("Disconnect");
							jServerDisconnectMenuItem
									.addActionListener(new ActionListener() {
										public void actionPerformed(
												ActionEvent evt) {
											doDisconnect();
										}
									});
						}
						{
							jSeparator1 = new JSeparator();
							jServerMenu.add(jSeparator1);
						}
						{
							jServerQuitMenuItem = new JMenuItem();
							jServerMenu.add(jServerQuitMenuItem);
							jServerQuitMenuItem.setText("Quit");
							jServerQuitMenuItem
									.addActionListener(new ActionListener() {
										public void actionPerformed(
												ActionEvent evt) {
											setVisible(false);
										}
									});
						}
					}
					{
						jMenuHelp = new JMenu();
						jMainMenu.add(jMenuHelp);
						jMenuHelp.setText("Help");
						{
							jHelpAboutMenuItem = new JMenuItem();
							jMenuHelp.add(jHelpAboutMenuItem);
							jHelpAboutMenuItem.setText("About ...");
							jHelpAboutMenuItem
									.addActionListener(new ActionListener() {
										public void actionPerformed(
												ActionEvent evt) {
											new AboutDialog().showAbout();
										}
									});
						}
					}
				}
			}
			{
				this.setSize(527, 542);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}
