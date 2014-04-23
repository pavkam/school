namespace DCalcServer.UI
{
    partial class DCalcServerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DCalcServerForm));
            this.lbAlgorithms = new System.Windows.Forms.Label();
            this.edtAlgorithms = new System.Windows.Forms.TextBox();
            this.btStart = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.gbConfiguration = new System.Windows.Forms.GroupBox();
            this.cbbServerType = new System.Windows.Forms.ComboBox();
            this.lbServerType = new System.Windows.Forms.Label();
            this.btRandom = new System.Windows.Forms.Button();
            this.edtSecurityKey = new System.Windows.Forms.TextBox();
            this.cbSecure = new System.Windows.Forms.CheckBox();
            this.gbLocalThreads = new System.Windows.Forms.GroupBox();
            this.rbCustomCount = new System.Windows.Forms.RadioButton();
            this.edtCustomCount = new System.Windows.Forms.TextBox();
            this.rbCoreCount = new System.Windows.Forms.RadioButton();
            this.rbSingleThread = new System.Windows.Forms.RadioButton();
            this.cbbLoadBalancer = new System.Windows.Forms.ComboBox();
            this.lbLoadBalancer = new System.Windows.Forms.Label();
            this.edtLifeTime = new System.Windows.Forms.TextBox();
            this.lbLifeTime = new System.Windows.Forms.Label();
            this.edtListeningPort = new System.Windows.Forms.TextBox();
            this.lbPort = new System.Windows.Forms.Label();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.graphView = new CustomUIControls.Graphing.C2DPushGraph();
            this.edtClients = new System.Windows.Forms.TextBox();
            this.lbClients = new System.Windows.Forms.Label();
            this.btExit = new System.Windows.Forms.Button();
            this.statsTimer = new System.Windows.Forms.Timer(this.components);
            this.gbConfiguration.SuspendLayout();
            this.gbLocalThreads.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbAlgorithms
            // 
            this.lbAlgorithms.AutoSize = true;
            this.lbAlgorithms.Location = new System.Drawing.Point(229, 16);
            this.lbAlgorithms.Name = "lbAlgorithms";
            this.lbAlgorithms.Size = new System.Drawing.Size(58, 13);
            this.lbAlgorithms.TabIndex = 2;
            this.lbAlgorithms.Text = "Algorithms:";
            // 
            // edtAlgorithms
            // 
            this.edtAlgorithms.Location = new System.Drawing.Point(293, 13);
            this.edtAlgorithms.Name = "edtAlgorithms";
            this.edtAlgorithms.ReadOnly = true;
            this.edtAlgorithms.Size = new System.Drawing.Size(67, 20);
            this.edtAlgorithms.TabIndex = 3;
            this.edtAlgorithms.TabStop = false;
            this.edtAlgorithms.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(216, 378);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 8;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btStop
            // 
            this.btStop.Location = new System.Drawing.Point(297, 378);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 9;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // gbConfiguration
            // 
            this.gbConfiguration.Controls.Add(this.cbbServerType);
            this.gbConfiguration.Controls.Add(this.lbServerType);
            this.gbConfiguration.Controls.Add(this.btRandom);
            this.gbConfiguration.Controls.Add(this.edtSecurityKey);
            this.gbConfiguration.Controls.Add(this.cbSecure);
            this.gbConfiguration.Controls.Add(this.gbLocalThreads);
            this.gbConfiguration.Controls.Add(this.cbbLoadBalancer);
            this.gbConfiguration.Controls.Add(this.lbLoadBalancer);
            this.gbConfiguration.Controls.Add(this.edtLifeTime);
            this.gbConfiguration.Controls.Add(this.lbLifeTime);
            this.gbConfiguration.Controls.Add(this.edtListeningPort);
            this.gbConfiguration.Controls.Add(this.lbPort);
            this.gbConfiguration.Location = new System.Drawing.Point(2, 3);
            this.gbConfiguration.Name = "gbConfiguration";
            this.gbConfiguration.Size = new System.Drawing.Size(370, 212);
            this.gbConfiguration.TabIndex = 0;
            this.gbConfiguration.TabStop = false;
            this.gbConfiguration.Text = "Configuration";
            // 
            // cbbServerType
            // 
            this.cbbServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbServerType.FormattingEnabled = true;
            this.cbbServerType.Items.AddRange(new object[] {
            "HTTP (Hyper Text Transfer Protocol)",
            "TCP (Transmission Control Protocol)"});
            this.cbbServerType.Location = new System.Drawing.Point(86, 37);
            this.cbbServerType.Name = "cbbServerType";
            this.cbbServerType.Size = new System.Drawing.Size(278, 21);
            this.cbbServerType.TabIndex = 2;
            // 
            // lbServerType
            // 
            this.lbServerType.AutoSize = true;
            this.lbServerType.Location = new System.Drawing.Point(6, 40);
            this.lbServerType.Name = "lbServerType";
            this.lbServerType.Size = new System.Drawing.Size(77, 13);
            this.lbServerType.TabIndex = 19;
            this.lbServerType.Text = "Listener Mode:";
            // 
            // btRandom
            // 
            this.btRandom.Location = new System.Drawing.Point(309, 112);
            this.btRandom.Name = "btRandom";
            this.btRandom.Size = new System.Drawing.Size(55, 23);
            this.btRandom.TabIndex = 6;
            this.btRandom.Text = "Random";
            this.btRandom.UseVisualStyleBackColor = true;
            this.btRandom.Click += new System.EventHandler(this.btRandom_Click);
            // 
            // edtSecurityKey
            // 
            this.edtSecurityKey.Location = new System.Drawing.Point(9, 114);
            this.edtSecurityKey.Name = "edtSecurityKey";
            this.edtSecurityKey.Size = new System.Drawing.Size(294, 20);
            this.edtSecurityKey.TabIndex = 5;
            this.edtSecurityKey.TextChanged += new System.EventHandler(this.all_TextChanged);
            // 
            // cbSecure
            // 
            this.cbSecure.AutoSize = true;
            this.cbSecure.Location = new System.Drawing.Point(9, 95);
            this.cbSecure.Name = "cbSecure";
            this.cbSecure.Size = new System.Drawing.Size(128, 17);
            this.cbSecure.TabIndex = 4;
            this.cbSecure.Text = "Security key required:";
            this.cbSecure.UseVisualStyleBackColor = true;
            this.cbSecure.CheckedChanged += new System.EventHandler(this.cbSecure_CheckedChanged);
            // 
            // gbLocalThreads
            // 
            this.gbLocalThreads.Controls.Add(this.rbCustomCount);
            this.gbLocalThreads.Controls.Add(this.edtCustomCount);
            this.gbLocalThreads.Controls.Add(this.rbCoreCount);
            this.gbLocalThreads.Controls.Add(this.rbSingleThread);
            this.gbLocalThreads.Location = new System.Drawing.Point(9, 137);
            this.gbLocalThreads.Name = "gbLocalThreads";
            this.gbLocalThreads.Size = new System.Drawing.Size(355, 70);
            this.gbLocalThreads.TabIndex = 7;
            this.gbLocalThreads.TabStop = false;
            this.gbLocalThreads.Text = "Number of Local Threads";
            // 
            // rbCustomCount
            // 
            this.rbCustomCount.AutoSize = true;
            this.rbCustomCount.Location = new System.Drawing.Point(158, 18);
            this.rbCustomCount.Name = "rbCustomCount";
            this.rbCustomCount.Size = new System.Drawing.Size(90, 17);
            this.rbCustomCount.TabIndex = 2;
            this.rbCustomCount.Text = "Custom count";
            this.rbCustomCount.UseVisualStyleBackColor = true;
            this.rbCustomCount.CheckedChanged += new System.EventHandler(this.rbCustomCount_CheckedChanged);
            // 
            // edtCustomCount
            // 
            this.edtCustomCount.Enabled = false;
            this.edtCustomCount.Location = new System.Drawing.Point(251, 17);
            this.edtCustomCount.Name = "edtCustomCount";
            this.edtCustomCount.Size = new System.Drawing.Size(100, 20);
            this.edtCustomCount.TabIndex = 3;
            this.edtCustomCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtCustomCount.TextChanged += new System.EventHandler(this.all_TextChanged);
            // 
            // rbCoreCount
            // 
            this.rbCoreCount.AutoSize = true;
            this.rbCoreCount.Location = new System.Drawing.Point(8, 42);
            this.rbCoreCount.Name = "rbCoreCount";
            this.rbCoreCount.Size = new System.Drawing.Size(189, 17);
            this.rbCoreCount.TabIndex = 1;
            this.rbCoreCount.Text = "Multiple Threads (Processor count)";
            this.rbCoreCount.UseVisualStyleBackColor = true;
            // 
            // rbSingleThread
            // 
            this.rbSingleThread.AutoSize = true;
            this.rbSingleThread.Checked = true;
            this.rbSingleThread.Location = new System.Drawing.Point(8, 19);
            this.rbSingleThread.Name = "rbSingleThread";
            this.rbSingleThread.Size = new System.Drawing.Size(91, 17);
            this.rbSingleThread.TabIndex = 0;
            this.rbSingleThread.TabStop = true;
            this.rbSingleThread.Text = "Single Thread";
            this.rbSingleThread.UseVisualStyleBackColor = true;
            // 
            // cbbLoadBalancer
            // 
            this.cbbLoadBalancer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbLoadBalancer.FormattingEnabled = true;
            this.cbbLoadBalancer.Location = new System.Drawing.Point(86, 63);
            this.cbbLoadBalancer.Name = "cbbLoadBalancer";
            this.cbbLoadBalancer.Size = new System.Drawing.Size(278, 21);
            this.cbbLoadBalancer.TabIndex = 3;
            // 
            // lbLoadBalancer
            // 
            this.lbLoadBalancer.AutoSize = true;
            this.lbLoadBalancer.Location = new System.Drawing.Point(6, 66);
            this.lbLoadBalancer.Name = "lbLoadBalancer";
            this.lbLoadBalancer.Size = new System.Drawing.Size(79, 13);
            this.lbLoadBalancer.TabIndex = 15;
            this.lbLoadBalancer.Text = "Load Balancer:";
            // 
            // edtLifeTime
            // 
            this.edtLifeTime.Location = new System.Drawing.Point(264, 13);
            this.edtLifeTime.Name = "edtLifeTime";
            this.edtLifeTime.Size = new System.Drawing.Size(100, 20);
            this.edtLifeTime.TabIndex = 1;
            this.edtLifeTime.Text = "20";
            this.edtLifeTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtLifeTime.TextChanged += new System.EventHandler(this.all_TextChanged);
            // 
            // lbLifeTime
            // 
            this.lbLifeTime.AutoSize = true;
            this.lbLifeTime.Location = new System.Drawing.Point(190, 16);
            this.lbLifeTime.Name = "lbLifeTime";
            this.lbLifeTime.Size = new System.Drawing.Size(75, 13);
            this.lbLifeTime.TabIndex = 10;
            this.lbLifeTime.Text = "Life time (sec):";
            // 
            // edtListeningPort
            // 
            this.edtListeningPort.Location = new System.Drawing.Point(86, 13);
            this.edtListeningPort.Name = "edtListeningPort";
            this.edtListeningPort.Size = new System.Drawing.Size(100, 20);
            this.edtListeningPort.TabIndex = 0;
            this.edtListeningPort.Text = "4456";
            this.edtListeningPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtListeningPort.TextChanged += new System.EventHandler(this.all_TextChanged);
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(6, 16);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(74, 13);
            this.lbPort.TabIndex = 8;
            this.lbPort.Text = "Listening Port:";
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.label1);
            this.gbStatus.Controls.Add(this.graphView);
            this.gbStatus.Controls.Add(this.edtClients);
            this.gbStatus.Controls.Add(this.lbClients);
            this.gbStatus.Controls.Add(this.lbAlgorithms);
            this.gbStatus.Controls.Add(this.edtAlgorithms);
            this.gbStatus.Location = new System.Drawing.Point(2, 216);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(370, 156);
            this.gbStatus.TabIndex = 1;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Server queue evolution in time:";
            // 
            // graphView
            // 
            this.graphView.AutoAdjustPeek = true;
            this.graphView.BackColor = System.Drawing.Color.Black;
            this.graphView.GridColor = System.Drawing.Color.Green;
            this.graphView.GridSize = ((ushort)(15));
            this.graphView.HighQuality = true;
            this.graphView.LineInterval = ((ushort)(5));
            this.graphView.Location = new System.Drawing.Point(9, 63);
            this.graphView.MaxLabel = "0000 sets";
            this.graphView.MaxPeekMagnitude = 10;
            this.graphView.MinLabel = "                          ";
            this.graphView.MinPeekMagnitude = 0;
            this.graphView.Name = "graphView";
            this.graphView.ShowGrid = true;
            this.graphView.ShowLabels = true;
            this.graphView.Size = new System.Drawing.Size(351, 87);
            this.graphView.TabIndex = 4;
            this.graphView.TextColor = System.Drawing.Color.Yellow;
            // 
            // edtClients
            // 
            this.edtClients.Location = new System.Drawing.Point(108, 13);
            this.edtClients.Name = "edtClients";
            this.edtClients.ReadOnly = true;
            this.edtClients.Size = new System.Drawing.Size(55, 20);
            this.edtClients.TabIndex = 3;
            this.edtClients.TabStop = false;
            this.edtClients.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbClients
            // 
            this.lbClients.AutoSize = true;
            this.lbClients.Location = new System.Drawing.Point(6, 16);
            this.lbClients.Name = "lbClients";
            this.lbClients.Size = new System.Drawing.Size(96, 13);
            this.lbClients.TabIndex = 2;
            this.lbClients.Text = "Clients Connected:";
            // 
            // btExit
            // 
            this.btExit.Location = new System.Drawing.Point(2, 378);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 10;
            this.btExit.Text = "Exit";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // statsTimer
            // 
            this.statsTimer.Tick += new System.EventHandler(this.statsTimer_Tick);
            // 
            // DCalcServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 405);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.gbConfiguration);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DCalcServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DCalc Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DCalcServerForm_FormClosing);
            this.gbConfiguration.ResumeLayout(false);
            this.gbConfiguration.PerformLayout();
            this.gbLocalThreads.ResumeLayout(false);
            this.gbLocalThreads.PerformLayout();
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbAlgorithms;
        private System.Windows.Forms.TextBox edtAlgorithms;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.GroupBox gbConfiguration;
        private System.Windows.Forms.TextBox edtListeningPort;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.TextBox edtClients;
        private System.Windows.Forms.Label lbClients;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Timer statsTimer;
        private System.Windows.Forms.Label lbLoadBalancer;
        private System.Windows.Forms.TextBox edtLifeTime;
        private System.Windows.Forms.Label lbLifeTime;
        private System.Windows.Forms.ComboBox cbbLoadBalancer;
        private System.Windows.Forms.GroupBox gbLocalThreads;
        private System.Windows.Forms.RadioButton rbCustomCount;
        private System.Windows.Forms.TextBox edtCustomCount;
        private System.Windows.Forms.RadioButton rbCoreCount;
        private System.Windows.Forms.RadioButton rbSingleThread;
        private CustomUIControls.Graphing.C2DPushGraph graphView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btRandom;
        private System.Windows.Forms.TextBox edtSecurityKey;
        private System.Windows.Forms.CheckBox cbSecure;
        private System.Windows.Forms.ComboBox cbbServerType;
        private System.Windows.Forms.Label lbServerType;
    }
}

