package org.ciobanu.school.ad.net;

public interface ProgressListener {
	void finishedPart(int countFinished, int countMax);
	void finishedDownload(boolean ok);
}
