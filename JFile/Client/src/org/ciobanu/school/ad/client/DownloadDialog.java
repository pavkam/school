package org.ciobanu.school.ad.client;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JFileChooser;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JProgressBar;
import javax.swing.JTextField;
import org.ciobanu.school.ad.net.ClientThread;
import org.ciobanu.school.ad.net.FileInfoRecord;
import org.ciobanu.school.ad.net.MultiDownload;
import org.ciobanu.school.ad.net.ProgressListener;

public class DownloadDialog extends JDialog implements ProgressListener {
	private JLabel jLabelDestination;
	private JTextField jFileName;
	private JLabel jNumberOfThreads;
	private JButton jCancel;
	private JButton jDownload;
	private JTextField jThreadCount;
	private JButton jBrowseFile;
	private JProgressBar jProgress;

	private FileInfoRecord fileInfo;
	private ClientThread baseThread;

	public DownloadDialog() {
		initGUI();
	}

	public void finishedPart(int countFinished, int countMax)
	{
		jProgress.setMaximum(countMax);
		jProgress.setValue(countFinished);
	}
	
	public void finishedDownload(boolean ok)
	{
		setEnabled(true);

		if (!ok)
			JOptionPane.showMessageDialog(this,
					"There was an error while downloading the file!", "Error",
					JOptionPane.ERROR_MESSAGE);
		else {
			JOptionPane.showMessageDialog(this,
					"File download completed succesefully!", "OK",
					JOptionPane.INFORMATION_MESSAGE);

			setVisible(false);
		}
	}

	private boolean doDownload(String toFile, String threadCount) {
		final int iThreadCount;

		File file = new File(toFile);
		
		if (file.exists() && (file.isDirectory() || !file.canWrite()))
		{
			JOptionPane.showMessageDialog(this,
					"Cannot access the selected file!", "Error",
					JOptionPane.ERROR_MESSAGE);
			
			return false;
		}
		
		if (file.exists())
			if (JOptionPane.showConfirmDialog(this, "File already exists! Rewrite?", "Confirm", 
					JOptionPane.YES_NO_OPTION, JOptionPane.QUESTION_MESSAGE) != JOptionPane.YES_OPTION)
			{
				return false;
			}
		
		file.delete();
		
		try {
			if (toFile.length() == 0)
				throw new Exception();

			iThreadCount = Integer.valueOf(threadCount);

			if (iThreadCount < 1 || iThreadCount > 10)
				throw new Exception();
		} catch (Exception e) {
			JOptionPane.showMessageDialog(this,
					"Invalid file name or thread count!", "Error",
					JOptionPane.ERROR_MESSAGE);
			return false;
		}
		
		MultiDownload mu = new MultiDownload(toFile, fileInfo, baseThread, iThreadCount, this);
		mu.start();

		return true;
	}

	public void downloadFile(ClientThread baseThread, FileInfoRecord fileInfo) {

		setModal(true);
		ResolutionLocation.setLocation(this);

		this.fileInfo = fileInfo;
		this.baseThread = baseThread;

		setVisible(true);
	}

	private void initGUI() {
		try {
			{
				this.setResizable(false);
				this.setTitle("Download file");
				getContentPane().setLayout(null);
				{
					jLabelDestination = new JLabel();
					getContentPane().add(jLabelDestination);
					jLabelDestination.setText("Destination:");
					jLabelDestination.setBounds(5, 7, 87, 14);
				}
				{
					jFileName = new JTextField();
					getContentPane().add(jFileName);
					jFileName.setBounds(98, 4, 288, 21);
				}
				{
					jBrowseFile = new JButton();
					getContentPane().add(jBrowseFile);
					jBrowseFile.setText("...");
					jBrowseFile.setBounds(391, 4, 23, 21);
					jBrowseFile.addActionListener(new ActionListener() {
						public void actionPerformed(ActionEvent evt) {
							JFileChooser fc = new JFileChooser();

							fc.setSelectedFile(new File(jFileName.getText()));

							fc.setFileSelectionMode(JFileChooser.FILES_ONLY);
							fc.setVisible(true);
							fc.setMultiSelectionEnabled(false);
							fc.setDialogType(JFileChooser.SAVE_DIALOG);

							if (fc.showOpenDialog(null) == JFileChooser.APPROVE_OPTION)
								jFileName.setText(fc.getSelectedFile()
										.getAbsolutePath());

						}
					});
				}
				{
					jNumberOfThreads = new JLabel();
					getContentPane().add(jNumberOfThreads);
					jNumberOfThreads.setText("Threads:");
					jNumberOfThreads.setBounds(5, 33, 87, 14);
				}
				{
					jThreadCount = new JTextField();
					getContentPane().add(jThreadCount);
					jThreadCount.setBounds(98, 30, 25, 21);
					jThreadCount.setText("5");
				}
				{
					jDownload = new JButton();
					getContentPane().add(jDownload);
					jDownload.setText("Download");
					jDownload.setBounds(314, 66, 100, 21);
					jDownload.addActionListener(new ActionListener() {
						public void actionPerformed(ActionEvent evt) {
							if (doDownload(jFileName.getText(), jThreadCount
									.getText())) {
								setEnabled(false);
							}
						}
					});
				}
				{
					jCancel = new JButton();
					getContentPane().add(jCancel);
					jCancel.setText("Cancel");
					jCancel.setBounds(204, 66, 104, 21);
					jCancel.addActionListener(new ActionListener() {
						public void actionPerformed(ActionEvent evt) {
							setVisible(false);
						}
					});
				}
				{
					jProgress = new JProgressBar();
					getContentPane().add(jProgress);
					jProgress.setBounds(129, 30, 285, 20);
				}
			}
			{
				this.setSize(435, 128);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}
