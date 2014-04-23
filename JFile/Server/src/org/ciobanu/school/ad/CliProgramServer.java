package org.ciobanu.school.ad;

import java.io.FileInputStream;
import java.io.IOException;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;
import org.ciobanu.school.ad.data.FileSystemInterface;
import org.ciobanu.school.ad.net.ClientWorker;
import org.ciobanu.school.ad.net.ClientWorkerListener;
import org.ciobanu.school.ad.net.PacketController;
import org.ciobanu.school.ad.net.server.ClientThread;
import org.ciobanu.school.ad.net.server.JFtpFileSystem;
import org.ciobanu.school.ad.net.server.JFtpPacketController;
import org.ciobanu.school.ad.net.server.ServerAcceptor;
import org.ciobanu.school.ad.net.server.ServerNotRunning;
import org.ciobanu.school.ad.net.server.BinarySerializer;

public class CliProgramServer implements ClientWorkerListener {
	/* Static */
	private static final String defaultPropertiesFile = "server.conf";
	private static final char pressToDie = 'q';

	/* Instance */
	private ServerAcceptor server;
	private int serverPort, serverTimeout;
	private String serverPath;
	private List<ClientWorker> registeredWorkers = new ArrayList<ClientWorker>();

	/* Entry point */
	public static void main(String[] args) {
		/* File path */
		String propertiesFile = defaultPropertiesFile;

		/* Check for parameters (other settings file) */
		if (args.length > 0) {
			propertiesFile = args[0];
		}

		CliProgramServer program = new CliProgramServer();
		program.initWork(propertiesFile);
	}

	/* Main application method */
	public void initWork(String propertiesFile) {
		/* Print logo */
		System.out.println("File Server. School project AD.");

		/* Check for valid input files and configuration */

		Properties propFile = new Properties();

		try {
			
			FileInputStream fs = new FileInputStream(propertiesFile);
			propFile.load(fs);

			fs.close();
		} catch (Exception e) {
			System.err.println("Configuration file \"" + propertiesFile
					+ "\" failed to load!");
			return;
		}

		/*
		 * OK, we have loaded our configuration file. Let's try to read all
		 * relevant properties
		 */

		try {
			serverPort = Integer.parseInt((String) propFile.get("server.port"));
			serverTimeout = Integer.parseInt((String) propFile
					.get("server.timeout"));
			serverPath = (String) propFile.get("server.path");
		} catch (Exception e) {
			System.err.println("Configuration file \"" + propertiesFile
					+ "\" seems to be broken!");
			return;
		}

		System.out.println("File Server initializing on port " + serverPort
				+ " with timeout " + serverTimeout + " and on path \""
				+ serverPath + "\".");

		try {
			server = new ServerAcceptor(serverPort, this);

			/* Listen for incoming connections */
			server.start();
		} catch (Exception e) {
			System.err.println("Failed to initialize the server! Stoping!");
			return;
		}

		System.out.println("File Server initialized! Press '" + pressToDie
				+ "' to close the server!");
		/* Stay and do absolutely nothing until a "break" key is pressed */
		while (true) {
			try {
				char charPressed = (char) System.in.read();

				if (Character.toLowerCase(charPressed) == pressToDie) {
					/* Kill character was pressed! Break the loop! */
					break;
				}
			} catch (Exception e) {
			}
		}

		System.out.println("User break signaled! Closing ...");

		/*
		 * Signal the server to stop. This call will block until the server has
		 * stopped
		 */
		try {
			server.stop();
		} catch (ServerNotRunning e) {
			/* Should not happen ever! */
		}

		/*
		 * Notify all workers to discontinue their jobs
		 */
		notifyShutdownWorkers();
		waitAllDead();

		System.out.println("File Server has been shut down! Bye!");
	}

	private synchronized void notifyShutdownWorkers() {
		/* for each worker */
		for (int i = 0; i < registeredWorkers.size(); i++) {
			registeredWorkers.get(i).stopWorking();
		}
	}

	private void waitAllDead() {
		while (registeredWorkers.size() > 0) {
			/* Wait another 10 ms until checking again */
			try {
				Thread.sleep(10);
			} catch (InterruptedException e) {
			}
		}
	}

	/* ClientWorkerListener methods */
	public ClientWorker clientAccepted(Socket clientSocket) {
		/* Actually create the requested serializers and controllers */
		BinarySerializer serializer = new BinarySerializer();
		FileSystemInterface fsIntf = new JFtpFileSystem(serverPath);
		PacketController controller = new JFtpPacketController(fsIntf);

		ClientThread cth;

		try {
			cth = new ClientThread(clientSocket, serializer, controller,
					serverTimeout);
		} catch (IOException e) {
			System.out.println("<Client> Failed a connection!");
			return null;
		}

		System.out.println("<Client> Attempting to connect from '"
				+ cth.getClientId() + "' ...");

		/* Register myself as event listener */
		cth.addListener(this);
		cth.startWorking();

		return cth;
	}

	public synchronized void clientConnected(ClientWorker worker) {
		System.out.println("<Client> Connected client '" + worker.getClientId()
				+ "'!");

		/* Register worker */
		registeredWorkers.add(worker);
	}

	public synchronized void clientDisconnected(ClientWorker worker) {
		System.out.println("<Client> Disconnected client '"
				+ worker.getClientId() + "'!");

		/* Remove worker */
		if (registeredWorkers.contains(worker))
			registeredWorkers.remove(worker);
	}
}
