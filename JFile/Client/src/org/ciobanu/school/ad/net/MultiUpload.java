package org.ciobanu.school.ad.net;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.RandomAccessFile;
import java.util.ArrayList;

public class MultiUpload implements Runnable {

	private String fileName;
	private String relFileName;
	private long relFileSize;
	private ClientThread baseThread;
	private int threadsToSpawn;
	private ProgressListener listener;

	public MultiUpload(String fileName,
			ClientThread baseThread, int threadsToSpawn,
			ProgressListener listener) {
		this.fileName = fileName;
		this.baseThread = baseThread;
		this.threadsToSpawn = threadsToSpawn;
		this.listener = listener;
		
		File fl = new File(fileName);
		RandomAccessFile raf;
		try {
			raf = new RandomAccessFile(fl, "r");
			this.relFileSize = raf.length();
		} catch (Exception e) {
		}

		this.relFileName = fl.getName();
	}

	public void start() {
		Thread th = new Thread(this);
		th.start();

	}

	private ArrayList<FilePartInfo> splitFileIntoPieces(long fileLength,
			int pieceSize) {
		ArrayList<FilePartInfo> pieces = new ArrayList<FilePartInfo>();

		long pieceCount = (fileLength / pieceSize);

		long startAt = 0;
		for (int i = 0; i < pieceCount; i++) {
			if (i > 0)
				startAt += pieceSize;

			pieces.add(new FilePartInfo(startAt, pieceSize));
		}

		if ((fileLength % pieceSize) > 0)
			pieces.add(new FilePartInfo(startAt + pieceSize,
					(fileLength - (pieceCount * pieceSize))));

		return pieces;
	}

	private byte[] readFile(FilePartInfo fpi) {

		try {
			File file = new File(fileName);
			RandomAccessFile raf = new RandomAccessFile(file, "r");

			raf.seek(fpi.getStartAt());
			
			byte[] result = new byte[(int)fpi.getLength()];
			
			int read = raf.read(result);
			
			if (read != result.length)
				throw new Exception();
			
			raf.close();
			
			return result;
		} catch (Exception e) {
		}

		return null;
	}

	private void assignPiece(ArrayList<FilePartInfo> freePieces,
			ArrayList<FilePartInfo> busyPieces,
			ArrayList<ClientThread> openThreads) {
		/* Find a free thread */
		for (int i = 0; i < openThreads.size(); i++) {
			boolean isBusy = false;

			for (int x = 0; x < busyPieces.size(); x++) {
				if (busyPieces.get(x).getWorker() == openThreads.get(i)) {
					isBusy = true;
					break;
				}
			}

			if (!isBusy && freePieces.size() > 0) {
				/* Not a busy dude */
				FilePartInfo fpi = freePieces.remove(0);
				fpi.setWorker(openThreads.get(i));

				/* Request */
				DataPacket dp = new DataPacket();
				dp.addParameter("WRITE");
				dp.addParameter(relFileName);
				dp.addParameter(fpi.getStartAt());
				dp.addParameter(readFile(fpi));
				openThreads.get(i).queuePacket(dp);

				busyPieces.add(fpi);
			}
		}
	}

	private void checkFinishedPieces(ArrayList<FilePartInfo> busyPieces) {
		/* Find a free thread */
		boolean somethingChnaged = true;

		while (somethingChnaged) {
			somethingChnaged = false;

			for (int i = 0; i < busyPieces.size(); i++) {
				DataPacket response = busyPieces.get(i).getWorker()
						.unqueuePacket();

				if (response != null) {
					busyPieces.remove(i);

					somethingChnaged = true;
				}
			}
		}
	}

	public void run() {

		ArrayList<ClientThread> openThreads = new ArrayList<ClientThread>();

		/* Let's spawn more threads and do da dew */
		for (int i = 0; i < (threadsToSpawn - 1); i++) {
			ClientThread cl0 = baseThread.spawn();

			if (cl0 != null)
				openThreads.add(cl0);
		}

		openThreads.add(baseThread);
		
		/* Create the file */
		DataPacket dp = new DataPacket();
		dp.addParameter("NEW");
		dp.addParameter(relFileName);
		dp.addParameter(relFileSize);
		
		dp = baseThread.sendAndReceivePacket(dp);

		if (dp == null)
		{
			listener.finishedDownload(false);
			return;
		}
		else
		{
			if ((new String(dp.getParameter(0))).equals("0"))
			{
				listener.finishedDownload(false);
				return;
			}
		}
		/* Split the file */
		ArrayList<FilePartInfo> freePieces = splitFileIntoPieces(relFileSize, 1024 * 20);
		ArrayList<FilePartInfo> busyPieces = new ArrayList<FilePartInfo>();

		int allPieceCount = freePieces.size();

		while (freePieces.size() > 0 || busyPieces.size() > 0) {
			/* Assign all free threads something to work on */
			assignPiece(freePieces, busyPieces, openThreads);

			/* Check if anything finished */
			checkFinishedPieces(busyPieces);

			listener.finishedPart(allPieceCount - freePieces.size()
					- busyPieces.size(), allPieceCount);

			try {
				Thread.sleep(10);
			} catch (InterruptedException e) {
			}
		}

		for (int i = 0; i < openThreads.size(); i++) {
			if (openThreads.get(i) != baseThread)
				openThreads.get(i).stopWorking();
		}

		listener.finishedDownload(true);
	}
}
