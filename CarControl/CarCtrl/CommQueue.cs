using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace CarCtrl
{
    public class CommQueue
    {
        public StringBuilder Public_Log = new StringBuilder();
        public Int64 BitsSent = 0;

        public enum Operation
        {
            BitReset,

            BitSend1,
            BitSend0,

            DelayBit,
            DelayPacket,
        }

        private SerialPort sp = new SerialPort();
        private Queue<Operation> qSend = new Queue<Operation>();
        private Thread runTh;
        private Boolean mustRun;
        private Int32 iiBit, iiPacket;

        public CommQueue(String comPort, Int32 bitDelay, Int32 pckDelay)
        {
            iiBit = bitDelay;
            iiPacket = pckDelay;
            sp.PortName = comPort;
            sp.BaudRate = 9600;
        }

        public void Enqueue(Operation op)
        {
            lock (qSend)
            {
                qSend.Enqueue(op);
            }
        }

        public void Start()
        {
            // Open port ;)
            sp.Open();

            if (mustRun)
                return;

            runTh = new Thread(ThreadRun);
            runTh.IsBackground = true;

            mustRun = true;

            runTh.Start();
        }

        public void Stop()
        {
            mustRun = false;

            while (runTh.IsAlive)
                Thread.Sleep(10);

            // close port
            sp.Close();
        }

        private void Status(Boolean Dtr, Boolean Rts, Boolean Open)
        {
            String dtr;
            String rts;
            String open;

            if (Dtr)
                dtr = "1";
            else
                dtr = "0";

            if (Rts)
                rts = "1";
            else
                rts = "0";

            if (Open)
                open = "O";
            else
            {
                open = "C";
                dtr = "-";
                rts = "-";
            }

            lock (Public_Log)
            {
                Public_Log.Append(String.Format(" {0}    {1}    {2}", dtr, rts, open));
                Public_Log.Append(Environment.NewLine);
            }
        }

        private void ThreadRun()
        {
            while (mustRun)
            {
                Thread.Sleep(1);

                Operation co;

                lock (qSend)
                {
                    if (qSend.Count == 0)
                        continue;

                    co = qSend.Dequeue();
                }

                if (co == Operation.DelayBit)
                {
                    Thread.Sleep(iiBit);
                }

                if (co == Operation.DelayPacket)
                {
                    Thread.Sleep(iiPacket);
                }

                if (co == Operation.BitReset)
                {
                    sp.DtrEnable = false;
                    sp.RtsEnable = false;
                }

                if (co == Operation.BitSend0)
                {
                    Interlocked.Increment(ref BitsSent);

                    sp.DtrEnable = true;
                    sp.RtsEnable = false;
                }

                if (co == Operation.BitSend1)
                {
                    Interlocked.Increment(ref BitsSent);

                    sp.DtrEnable = false;
                    sp.RtsEnable = true;
                }

                Status(sp.DtrEnable, sp.RtsEnable, sp.IsOpen);
            }
        }
    }
}
