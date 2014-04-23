package org.ciobanu.school.ad.net;

import java.io.File;
import java.io.RandomAccessFile;
import java.util.ArrayList;

public class MultiDownload implements Runnable {

	private String fileName;
	private ClientThread baseThread;
	private int threadsToSpawn;
	private FileInfoRecord fileInfo;
	private ProgressListener listener;

	public MultiDownload(String fileName, FileInfoRecord fileInfo,
			ClientThread baseThread, int threadsToSpawn,
			ProgressListener listener) {
		this.fileName = fileName;
		this.baseThread = baseThread;
		this.threadsToSpawn = threadsToSpawn;
		this.fileInfo = fileInfo;
		this.listener = listener;
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

	private boolean mergeFile(DataPacket packet, FilePartInfo fpi) {

		try {
			File file = new File(fileName);
			RandomAccessFile raf = new RandomAccessFile(file, "rw");

			raf.seek(fpi.getStartAt());
			raf.write(packet.getParameter(1));

			raf.close();
		} catch (Exception e) {
			return false;
		}

		return true;
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
				dp.addParameter("READ");
				dp.addParameter(fileInfo.getName());
				dp.addParameter(fpi.getStartAt());
				dp.addParameter(fpi.getLength());
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
					mergeFile(response, busyPieces.get(i));

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

		/* Split the file */
		ArrayList<FilePartInfo> freePieces = splitFileIntoPieces(fileInfo
				.getSize(), 1024 * 20);
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
