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
import org.ciobanu.school.ad.net.MultiUpload;
import org.ciobanu.school.ad.net.ProgressListener;

public class UploadDialog extends JDialog implements ProgressListener {
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

	public UploadDialog() {
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
					"There was an error while uploading the file!", "Error",
					JOptionPane.ERROR_MESSAGE);
		else {
			JOptionPane.showMessageDialog(this,
					"File upload completed succesefully!", "OK",
					JOptionPane.INFORMATION_MESSAGE);

			setVisible(false);
		}
	}

	private boolean doUpload(String fromFile, String threadCount) {
		final int iThreadCount;

		File file = new File(fromFile);
		
		if (!file.exists() || file.isDirectory() || !file.canRead())
		{
			JOptionPane.showMessageDialog(this,
					"Cannot access the selected file!", "Error",
					JOptionPane.ERROR_MESSAGE);
			
			return false;
		}
			
		try {
			if (fromFile.length() == 0)
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
		
		MultiUpload mu = new MultiUpload(fromFile, baseThread, iThreadCount, this);
		mu.start();

		return true;
	}

	public void uploadFile(ClientThread baseThread) {

		setModal(true);
		ResolutionLocation.setLocation(this);

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
					jLabelDestination.setText("Source:");
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
							fc.setDialogType(JFileChooser.OPEN_DIALOG);

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
					jDownload.setText("Upload");
					jDownload.setBounds(314, 66, 100, 21);
					jDownload.addActionListener(new ActionListener() {
						public void actionPerformed(ActionEvent evt) {
							if (doUpload(jFileName.getText(), jThreadCount
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
