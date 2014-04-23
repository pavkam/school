using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CarCtrl
{
    public partial class formCtrl : Form
    {
        private Boolean bRight = false, bLeft = false, bUp = false, bDown = false;
        private Boolean inSync = false;
        private CommQueue cq;

        private StringBuilder sbLog = new StringBuilder();
        private StringBuilder sbStatus = new StringBuilder();

        private void Log(String what)
        {
            sbLog.Append(what);
            sbLog.Append(Environment.NewLine);

            mmLog.Text = sbLog.ToString();
            mmLog.SelectionStart = mmLog.Text.Length;
            mmLog.SelectionLength = 0;
            mmLog.ScrollToCaret();
        }

        private void SendPacket(Int32 bit0, Int32 bit1, Int32 bit2)
        {
            if (bit0 == 0)
            {
                cq.Enqueue(CommQueue.Operation.BitSend0);
            }
            else
            {
                cq.Enqueue(CommQueue.Operation.BitSend1);
            }

            cq.Enqueue(CommQueue.Operation.DelayBit);
            cq.Enqueue(CommQueue.Operation.BitReset);
            cq.Enqueue(CommQueue.Operation.DelayBit);

            if (bit1 == 0)
            {
                cq.Enqueue(CommQueue.Operation.BitSend0);
            }
            else
            {
                cq.Enqueue(CommQueue.Operation.BitSend1);
            }

            cq.Enqueue(CommQueue.Operation.DelayBit);
            cq.Enqueue(CommQueue.Operation.BitReset);
            cq.Enqueue(CommQueue.Operation.DelayBit);

            if (bit2 == 0)
            {
                cq.Enqueue(CommQueue.Operation.BitSend0);
            }
            else
            {
                cq.Enqueue(CommQueue.Operation.BitSend1);
            }

            cq.Enqueue(CommQueue.Operation.DelayBit);
            cq.Enqueue(CommQueue.Operation.BitReset);
            cq.Enqueue(CommQueue.Operation.DelayPacket);
        }

        private void SendCOMRequest()
        {
            if (bRight)
            {
                if (bDown)
                    SendPacket(0, 0, 1);
                else if (bUp)
                    SendPacket(0, 1, 0);
                else
                    SendPacket(0, 1, 1);
            }
            else if (bLeft)
            {
                if (bDown)
                    SendPacket(1, 0, 0);
                else if (bUp)
                    SendPacket(1, 0, 1);
                else
                    SendPacket(1, 1, 0);
            }
            else
            {
                if (bDown)
                    SendPacket(0, 0, 0);
                else if (bUp)
                    SendPacket(1, 1, 1);
                else
                    SendPacket(1, 1, 1);
            }
        }

        private void TurnRight()
        {
            if (bRight)
                return;

            bRight = true;
            bLeft = false;

            Log("UI> Turning Right");
            SendCOMRequest();
        }

        private void TurnLeft()
        {
            if (bLeft)
                return;

            bLeft = true;
            bRight = false;

            Log("UI> Turning Left");
            SendCOMRequest();
        }

        private void TurnStop()
        {
            if (!(bRight && bLeft))
            {
                bRight = false;
                bLeft = false;

                Log("UI> Turning Stopped");
                SendCOMRequest();
            }
        }

        private void GoUp()
        {
            if (bUp)
                return;

            bUp = true;
            bDown = false;

            Log("UI> Moving Forward");
            SendCOMRequest();
        }

        private void GoDown()
        {
            if (bDown)
                return;

            bDown = true;
            bUp = false;

            Log("UI> Moving Backwards");
            SendCOMRequest();
        }

        private void GoStop()
        {
            if (!(bUp && bDown))
            {
                bUp = false;
                bDown = false;

                Log("UI> Moving Stopped");
                SendCOMRequest();
            }
        }

        public formCtrl()
        {
            InitializeComponent();

            // Initialize COM Port list

            for (Int32 i = 1; i < 32; i++)
            {
                cbbSerialPort.Items.Add("COM" + i);
            }

            cbbSerialPort.SelectedIndex = 0;
        }

        private void lb_KeyDown(object sender, KeyEventArgs e)
        {
            if (!inSync)
                return;

            if (e.KeyData == Keys.Up)
            {
                lbDown.ForeColor = Color.Black;
                lbDown.BackColor = Color.Gray;

                lbUp.ForeColor = Color.Red;
                lbUp.BackColor = Color.Black;

                e.Handled = true;

                GoUp();
            }

            if (e.KeyData == Keys.Down)
            {
                lbUp.ForeColor = Color.Black;
                lbUp.BackColor = Color.Gray;

                lbDown.ForeColor = Color.Red;
                lbDown.BackColor = Color.Black;

                e.Handled = true;

                GoDown();
            }

            if (e.KeyData == Keys.Left)
            {
                lbRight.ForeColor = Color.Black;
                lbRight.BackColor = Color.Gray;

                lbLeft.ForeColor = Color.Red;
                lbLeft.BackColor = Color.Black;

                e.Handled = true;

                TurnLeft();
            }

            if (e.KeyData == Keys.Right)
            {
                lbLeft.ForeColor = Color.Black;
                lbLeft.BackColor = Color.Gray;

                lbRight.ForeColor = Color.Red;
                lbRight.BackColor = Color.Black;

                e.Handled = true;

                TurnRight();
            }
        }

        private void lb_KeyUp(object sender, KeyEventArgs e)
        {
            if (!inSync)
                return;

            if (e.KeyData == Keys.Up)
            {
                lbUp.ForeColor = Color.Black;
                lbUp.BackColor = Color.Gray;

                e.Handled = true;

                GoStop();
            }

            if (e.KeyData == Keys.Down)
            {
                lbDown.ForeColor = Color.Black;
                lbDown.BackColor = Color.Gray;

                e.Handled = true;

                GoStop();
            }

            if (e.KeyData == Keys.Left)
            {
                lbLeft.ForeColor = Color.Black;
                lbLeft.BackColor = Color.Gray;

                e.Handled = true;

                TurnStop();
            }

            if (e.KeyData == Keys.Right)
            {
                lbRight.ForeColor = Color.Black;
                lbRight.BackColor = Color.Gray;

                e.Handled = true;

                TurnStop();
            }
        }

        private void btStartStop_Click(object sender, EventArgs e)
        {
            if (inSync)
            {
                btStartStop.Text = "Start";
                inSync = false;

                edtBitSync.Enabled = true;
                edtPacketSync.Enabled = true;
                cbbSerialPort.Enabled = true;

                try
                {
                    cq.Stop();
                }
                catch
                {
                }
            }
            else
            {
                String iiCOM = cbbSerialPort.Text;
                Int32 iiBit = 0;
                Int32 iiPacket = 0;

                try
                {
                    iiBit = Convert.ToInt32(edtBitSync.Value);
                    iiPacket = Convert.ToInt32(edtPacketSync.Value);
                }
                catch
                {
                    MessageBox.Show("Invalid Numeric value for Sync Delays!", "Error");
                    return;
                }

                cq = new CommQueue(iiCOM, iiBit, iiPacket);

                try
                {
                    cq.Start();
                }
                catch
                {
                    MessageBox.Show("Invalid Port specified! Cannot be opened!");
                    return;
                }

                btStartStop.Text = "Stop";
                inSync = true;

                edtBitSync.Enabled = false;
                edtPacketSync.Enabled = false;
                cbbSerialPort.Enabled = false;

                sbLog.Length = 0;
                Log("Starting Comm Session ...");
            }
        }

        private void formCtrl_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (inSync)
                btStartStop_Click(btStartStop, EventArgs.Empty);
        }

        private void timerLog_Tick(object sender, EventArgs e)
        {
            if (!inSync || cq == null)
            {
                return;
            }

            lock (cq.Public_Log)
            {
                String st = cq.Public_Log.ToString();
                cq.Public_Log.Length = 0;

                if (st != null && st.Length > 0)
                {
                    sbStatus.Append(st);
                    mmStatus.Text = sbStatus.ToString();
                    mmStatus.SelectionStart = mmStatus.Text.Length;
                    mmStatus.SelectionLength = 0;
                    mmStatus.ScrollToCaret();
                }
            }

            lbSent.Text = String.Format("Bits Sent: {0}", System.Threading.Interlocked.Read(ref cq.BitsSent));
        }
    }
}