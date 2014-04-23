namespace CarCtrl
{
    partial class formCtrl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formCtrl));
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btRight = new System.Windows.Forms.Panel();
            this.lbRight = new System.Windows.Forms.Label();
            this.btDown = new System.Windows.Forms.Panel();
            this.lbDown = new System.Windows.Forms.Label();
            this.btLeft = new System.Windows.Forms.Panel();
            this.lbLeft = new System.Windows.Forms.Label();
            this.btUp = new System.Windows.Forms.Panel();
            this.lbUp = new System.Windows.Forms.Label();
            this.mmLog = new System.Windows.Forms.TextBox();
            this.lbCmdLog = new System.Windows.Forms.Label();
            this.lbSerial = new System.Windows.Forms.Label();
            this.cbbSerialPort = new System.Windows.Forms.ComboBox();
            this.lbBitSend = new System.Windows.Forms.Label();
            this.edtBitSync = new System.Windows.Forms.NumericUpDown();
            this.edtPacketSync = new System.Windows.Forms.NumericUpDown();
            this.lbPacketSend = new System.Windows.Forms.Label();
            this.btStartStop = new System.Windows.Forms.Button();
            this.mmStatus = new System.Windows.Forms.TextBox();
            this.lbPortStat = new System.Windows.Forms.Label();
            this.timerLog = new System.Windows.Forms.Timer(this.components);
            this.lbSent = new System.Windows.Forms.Label();
            this.panelButtons.SuspendLayout();
            this.btRight.SuspendLayout();
            this.btDown.SuspendLayout();
            this.btLeft.SuspendLayout();
            this.btUp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtBitSync)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPacketSync)).BeginInit();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelButtons.Controls.Add(this.btRight);
            this.panelButtons.Controls.Add(this.btDown);
            this.panelButtons.Controls.Add(this.btLeft);
            this.panelButtons.Controls.Add(this.btUp);
            this.panelButtons.Location = new System.Drawing.Point(271, 225);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(192, 187);
            this.panelButtons.TabIndex = 0;
            // 
            // btRight
            // 
            this.btRight.BackColor = System.Drawing.Color.Gray;
            this.btRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btRight.Controls.Add(this.lbRight);
            this.btRight.Font = new System.Drawing.Font("Webdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btRight.Location = new System.Drawing.Point(125, 63);
            this.btRight.Name = "btRight";
            this.btRight.Size = new System.Drawing.Size(60, 60);
            this.btRight.TabIndex = 3;
            // 
            // lbRight
            // 
            this.lbRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRight.Font = new System.Drawing.Font("Webdings", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lbRight.Location = new System.Drawing.Point(0, 0);
            this.lbRight.Name = "lbRight";
            this.lbRight.Size = new System.Drawing.Size(58, 58);
            this.lbRight.TabIndex = 1;
            this.lbRight.Text = "4";
            this.lbRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btDown
            // 
            this.btDown.BackColor = System.Drawing.Color.Gray;
            this.btDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btDown.Controls.Add(this.lbDown);
            this.btDown.Font = new System.Drawing.Font("Webdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btDown.Location = new System.Drawing.Point(65, 123);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(60, 60);
            this.btDown.TabIndex = 2;
            // 
            // lbDown
            // 
            this.lbDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDown.Font = new System.Drawing.Font("Webdings", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lbDown.Location = new System.Drawing.Point(0, 0);
            this.lbDown.Name = "lbDown";
            this.lbDown.Size = new System.Drawing.Size(58, 58);
            this.lbDown.TabIndex = 1;
            this.lbDown.Text = "6";
            this.lbDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btLeft
            // 
            this.btLeft.BackColor = System.Drawing.Color.Gray;
            this.btLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btLeft.Controls.Add(this.lbLeft);
            this.btLeft.Font = new System.Drawing.Font("Webdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btLeft.Location = new System.Drawing.Point(5, 63);
            this.btLeft.Name = "btLeft";
            this.btLeft.Size = new System.Drawing.Size(60, 60);
            this.btLeft.TabIndex = 2;
            // 
            // lbLeft
            // 
            this.lbLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLeft.Font = new System.Drawing.Font("Webdings", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lbLeft.Location = new System.Drawing.Point(0, 0);
            this.lbLeft.Name = "lbLeft";
            this.lbLeft.Size = new System.Drawing.Size(58, 58);
            this.lbLeft.TabIndex = 1;
            this.lbLeft.Text = "3";
            this.lbLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btUp
            // 
            this.btUp.BackColor = System.Drawing.Color.Gray;
            this.btUp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btUp.Controls.Add(this.lbUp);
            this.btUp.Font = new System.Drawing.Font("Webdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btUp.Location = new System.Drawing.Point(65, 3);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(60, 60);
            this.btUp.TabIndex = 1;
            // 
            // lbUp
            // 
            this.lbUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbUp.Font = new System.Drawing.Font("Webdings", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lbUp.Location = new System.Drawing.Point(0, 0);
            this.lbUp.Name = "lbUp";
            this.lbUp.Size = new System.Drawing.Size(58, 58);
            this.lbUp.TabIndex = 0;
            this.lbUp.Text = "5";
            this.lbUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mmLog
            // 
            this.mmLog.Location = new System.Drawing.Point(5, 20);
            this.mmLog.Multiline = true;
            this.mmLog.Name = "mmLog";
            this.mmLog.ReadOnly = true;
            this.mmLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mmLog.Size = new System.Drawing.Size(325, 167);
            this.mmLog.TabIndex = 1;
            this.mmLog.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lb_KeyUp);
            this.mmLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lb_KeyDown);
            // 
            // lbCmdLog
            // 
            this.lbCmdLog.AutoSize = true;
            this.lbCmdLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbCmdLog.Location = new System.Drawing.Point(2, 4);
            this.lbCmdLog.Name = "lbCmdLog";
            this.lbCmdLog.Size = new System.Drawing.Size(90, 13);
            this.lbCmdLog.TabIndex = 2;
            this.lbCmdLog.Text = "Command Log:";
            // 
            // lbSerial
            // 
            this.lbSerial.AutoSize = true;
            this.lbSerial.Location = new System.Drawing.Point(2, 225);
            this.lbSerial.Name = "lbSerial";
            this.lbSerial.Size = new System.Drawing.Size(58, 13);
            this.lbSerial.TabIndex = 3;
            this.lbSerial.Text = "Serial Port:";
            // 
            // cbbSerialPort
            // 
            this.cbbSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSerialPort.FormattingEnabled = true;
            this.cbbSerialPort.Location = new System.Drawing.Point(5, 241);
            this.cbbSerialPort.Name = "cbbSerialPort";
            this.cbbSerialPort.Size = new System.Drawing.Size(136, 21);
            this.cbbSerialPort.TabIndex = 4;
            // 
            // lbBitSend
            // 
            this.lbBitSend.AutoSize = true;
            this.lbBitSend.Location = new System.Drawing.Point(2, 267);
            this.lbBitSend.Name = "lbBitSend";
            this.lbBitSend.Size = new System.Drawing.Size(76, 13);
            this.lbBitSend.TabIndex = 5;
            this.lbBitSend.Text = "Bit Sync Delay";
            // 
            // edtBitSync
            // 
            this.edtBitSync.Location = new System.Drawing.Point(5, 283);
            this.edtBitSync.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.edtBitSync.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.edtBitSync.Name = "edtBitSync";
            this.edtBitSync.Size = new System.Drawing.Size(136, 20);
            this.edtBitSync.TabIndex = 6;
            this.edtBitSync.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // edtPacketSync
            // 
            this.edtPacketSync.Location = new System.Drawing.Point(5, 324);
            this.edtPacketSync.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.edtPacketSync.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.edtPacketSync.Name = "edtPacketSync";
            this.edtPacketSync.Size = new System.Drawing.Size(136, 20);
            this.edtPacketSync.TabIndex = 8;
            this.edtPacketSync.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // lbPacketSend
            // 
            this.lbPacketSend.AutoSize = true;
            this.lbPacketSend.Location = new System.Drawing.Point(2, 308);
            this.lbPacketSend.Name = "lbPacketSend";
            this.lbPacketSend.Size = new System.Drawing.Size(98, 13);
            this.lbPacketSend.TabIndex = 7;
            this.lbPacketSend.Text = "Packet Sync Delay";
            // 
            // btStartStop
            // 
            this.btStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btStartStop.Location = new System.Drawing.Point(5, 389);
            this.btStartStop.Name = "btStartStop";
            this.btStartStop.Size = new System.Drawing.Size(75, 23);
            this.btStartStop.TabIndex = 9;
            this.btStartStop.Text = "Start";
            this.btStartStop.UseVisualStyleBackColor = true;
            this.btStartStop.Click += new System.EventHandler(this.btStartStop_Click);
            this.btStartStop.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lb_KeyUp);
            this.btStartStop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lb_KeyDown);
            // 
            // mmStatus
            // 
            this.mmStatus.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mmStatus.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mmStatus.Location = new System.Drawing.Point(336, 20);
            this.mmStatus.Multiline = true;
            this.mmStatus.Name = "mmStatus";
            this.mmStatus.ReadOnly = true;
            this.mmStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mmStatus.Size = new System.Drawing.Size(127, 167);
            this.mmStatus.TabIndex = 10;
            this.mmStatus.WordWrap = false;
            this.mmStatus.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lb_KeyUp);
            this.mmStatus.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lb_KeyDown);
            // 
            // lbPortStat
            // 
            this.lbPortStat.AutoSize = true;
            this.lbPortStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbPortStat.Location = new System.Drawing.Point(335, 6);
            this.lbPortStat.Name = "lbPortStat";
            this.lbPortStat.Size = new System.Drawing.Size(110, 13);
            this.lbPortStat.TabIndex = 11;
            this.lbPortStat.Text = "DTR   RTS  STAT";
            // 
            // timerLog
            // 
            this.timerLog.Enabled = true;
            this.timerLog.Interval = 50;
            this.timerLog.Tick += new System.EventHandler(this.timerLog_Tick);
            // 
            // lbSent
            // 
            this.lbSent.AutoSize = true;
            this.lbSent.Location = new System.Drawing.Point(335, 190);
            this.lbSent.Name = "lbSent";
            this.lbSent.Size = new System.Drawing.Size(61, 13);
            this.lbSent.TabIndex = 12;
            this.lbSent.Text = "Bits Sent: 0";
            // 
            // formCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 416);
            this.Controls.Add(this.lbSent);
            this.Controls.Add(this.lbPortStat);
            this.Controls.Add(this.mmStatus);
            this.Controls.Add(this.btStartStop);
            this.Controls.Add(this.edtPacketSync);
            this.Controls.Add(this.lbPacketSend);
            this.Controls.Add(this.edtBitSync);
            this.Controls.Add(this.lbBitSend);
            this.Controls.Add(this.cbbSerialPort);
            this.Controls.Add(this.lbSerial);
            this.Controls.Add(this.lbCmdLog);
            this.Controls.Add(this.mmLog);
            this.Controls.Add(this.panelButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "formCtrl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Car Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formCtrl_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lb_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lb_KeyDown);
            this.panelButtons.ResumeLayout(false);
            this.btRight.ResumeLayout(false);
            this.btDown.ResumeLayout(false);
            this.btLeft.ResumeLayout(false);
            this.btUp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edtBitSync)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPacketSync)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Panel btRight;
        private System.Windows.Forms.Panel btDown;
        private System.Windows.Forms.Panel btLeft;
        private System.Windows.Forms.Panel btUp;
        private System.Windows.Forms.Label lbRight;
        private System.Windows.Forms.Label lbDown;
        private System.Windows.Forms.Label lbLeft;
        private System.Windows.Forms.Label lbUp;
        private System.Windows.Forms.TextBox mmLog;
        private System.Windows.Forms.Label lbCmdLog;
        private System.Windows.Forms.Label lbSerial;
        private System.Windows.Forms.ComboBox cbbSerialPort;
        private System.Windows.Forms.Label lbBitSend;
        private System.Windows.Forms.NumericUpDown edtBitSync;
        private System.Windows.Forms.NumericUpDown edtPacketSync;
        private System.Windows.Forms.Label lbPacketSend;
        private System.Windows.Forms.Button btStartStop;
        private System.Windows.Forms.TextBox mmStatus;
        private System.Windows.Forms.Label lbPortStat;
        private System.Windows.Forms.Timer timerLog;
        private System.Windows.Forms.Label lbSent;
    }
}

