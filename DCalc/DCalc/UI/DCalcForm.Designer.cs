namespace DCalc.UI
{
    partial class DCalcForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DCalcForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.algorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelServers = new System.Windows.Forms.Panel();
            this.btEditServer = new System.Windows.Forms.Button();
            this.btRemoveServer = new System.Windows.Forms.Button();
            this.btAddServer = new System.Windows.Forms.Button();
            this.tvServers = new System.Windows.Forms.TreeView();
            this.imageListServerStatus = new System.Windows.Forms.ImageList(this.components);
            this.lbServers = new System.Windows.Forms.Label();
            this.panelComponent = new System.Windows.Forms.Panel();
            this.lbStats = new System.Windows.Forms.Label();
            this.graphView = new CustomUIControls.Graphing.C2DPushGraph();
            this.pgBar = new System.Windows.Forms.ProgressBar();
            this.lbProgressLog = new System.Windows.Forms.Label();
            this.panelSep0 = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsSelectedProvider = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.pgBarTimer = new System.Windows.Forms.Timer(this.components);
            this.mainMenu.SuspendLayout();
            this.panelServers.SuspendLayout();
            this.panelComponent.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.executionToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainMenu.Size = new System.Drawing.Size(556, 24);
            this.mainMenu.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::DCalc.Properties.Resources.file_new;
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::DCalc.Properties.Resources.file_open;
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openToolStripMenuItem.Text = "&Open ...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::DCalc.Properties.Resources.file_save;
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::DCalc.Properties.Resources.file_saveas;
            this.saveAsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As ...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(136, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::DCalc.Properties.Resources.file_exit;
            this.exitToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // executionToolStripMenuItem
            // 
            this.executionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.toolStripMenuItem2,
            this.algorithmToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.executionToolStripMenuItem.Name = "executionToolStripMenuItem";
            this.executionToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.executionToolStripMenuItem.Text = "&Execution";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Image = global::DCalc.Properties.Resources.execution_start;
            this.startToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.startToolStripMenuItem.Text = "St&art";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Image = global::DCalc.Properties.Resources.execution_stop;
            this.stopToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.stopToolStripMenuItem.Text = "St&op";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(192, 6);
            // 
            // algorithmToolStripMenuItem
            // 
            this.algorithmToolStripMenuItem.Image = global::DCalc.Properties.Resources.execution_configure_algorithm;
            this.algorithmToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.algorithmToolStripMenuItem.Name = "algorithmToolStripMenuItem";
            this.algorithmToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.algorithmToolStripMenuItem.Text = "&Configure Algorithm ...";
            this.algorithmToolStripMenuItem.Click += new System.EventHandler(this.algorithmToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::DCalc.Properties.Resources.execution_options;
            this.optionsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.optionsToolStripMenuItem.Text = "O&ptions ...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::DCalc.Properties.Resources.help_about;
            this.aboutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // panelServers
            // 
            this.panelServers.Controls.Add(this.btEditServer);
            this.panelServers.Controls.Add(this.btRemoveServer);
            this.panelServers.Controls.Add(this.btAddServer);
            this.panelServers.Controls.Add(this.tvServers);
            this.panelServers.Controls.Add(this.lbServers);
            this.panelServers.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelServers.Location = new System.Drawing.Point(0, 24);
            this.panelServers.Name = "panelServers";
            this.panelServers.Size = new System.Drawing.Size(556, 221);
            this.panelServers.TabIndex = 1;
            // 
            // btEditServer
            // 
            this.btEditServer.Location = new System.Drawing.Point(454, 45);
            this.btEditServer.Name = "btEditServer";
            this.btEditServer.Size = new System.Drawing.Size(93, 23);
            this.btEditServer.TabIndex = 5;
            this.btEditServer.Text = "Ed&it";
            this.btEditServer.UseVisualStyleBackColor = true;
            this.btEditServer.Click += new System.EventHandler(this.btEditServer_Click);
            // 
            // btRemoveServer
            // 
            this.btRemoveServer.Location = new System.Drawing.Point(454, 71);
            this.btRemoveServer.Name = "btRemoveServer";
            this.btRemoveServer.Size = new System.Drawing.Size(93, 23);
            this.btRemoveServer.TabIndex = 4;
            this.btRemoveServer.Text = "&Remove";
            this.btRemoveServer.UseVisualStyleBackColor = true;
            this.btRemoveServer.Click += new System.EventHandler(this.btRemoveServer_Click);
            // 
            // btAddServer
            // 
            this.btAddServer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAddServer.Location = new System.Drawing.Point(454, 19);
            this.btAddServer.Name = "btAddServer";
            this.btAddServer.Size = new System.Drawing.Size(93, 23);
            this.btAddServer.TabIndex = 3;
            this.btAddServer.Text = "A&dd";
            this.btAddServer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btAddServer.UseVisualStyleBackColor = true;
            this.btAddServer.Click += new System.EventHandler(this.btAddServer_Click);
            // 
            // tvServers
            // 
            this.tvServers.CheckBoxes = true;
            this.tvServers.ImageIndex = 0;
            this.tvServers.ImageList = this.imageListServerStatus;
            this.tvServers.Location = new System.Drawing.Point(6, 19);
            this.tvServers.Name = "tvServers";
            this.tvServers.SelectedImageIndex = 0;
            this.tvServers.Size = new System.Drawing.Size(442, 196);
            this.tvServers.TabIndex = 2;
            this.tvServers.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvServers_NodeMouseDoubleClick);
            this.tvServers.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvServers_AfterCheck);
            this.tvServers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvServers_AfterSelect);
            // 
            // imageListServerStatus
            // 
            this.imageListServerStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListServerStatus.ImageStream")));
            this.imageListServerStatus.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListServerStatus.Images.SetKeyName(0, "status_disabled.bmp");
            this.imageListServerStatus.Images.SetKeyName(1, "status_down.bmp");
            this.imageListServerStatus.Images.SetKeyName(2, "status_running.bmp");
            // 
            // lbServers
            // 
            this.lbServers.AutoSize = true;
            this.lbServers.Location = new System.Drawing.Point(4, 5);
            this.lbServers.Name = "lbServers";
            this.lbServers.Size = new System.Drawing.Size(56, 13);
            this.lbServers.TabIndex = 1;
            this.lbServers.Text = "Server list:";
            // 
            // panelComponent
            // 
            this.panelComponent.Controls.Add(this.lbStats);
            this.panelComponent.Controls.Add(this.graphView);
            this.panelComponent.Controls.Add(this.pgBar);
            this.panelComponent.Controls.Add(this.lbProgressLog);
            this.panelComponent.Controls.Add(this.panelSep0);
            this.panelComponent.Controls.Add(this.statusStrip);
            this.panelComponent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelComponent.Location = new System.Drawing.Point(0, 245);
            this.panelComponent.Name = "panelComponent";
            this.panelComponent.Size = new System.Drawing.Size(556, 195);
            this.panelComponent.TabIndex = 2;
            // 
            // lbStats
            // 
            this.lbStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStats.Location = new System.Drawing.Point(217, 6);
            this.lbStats.Name = "lbStats";
            this.lbStats.Size = new System.Drawing.Size(330, 13);
            this.lbStats.TabIndex = 5;
            this.lbStats.Text = "Evaluating set {0} of {1}. Average speed: {2}.";
            this.lbStats.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // graphView
            // 
            this.graphView.AutoAdjustPeek = true;
            this.graphView.BackColor = System.Drawing.Color.Black;
            this.graphView.ForeColor = System.Drawing.Color.White;
            this.graphView.GridColor = System.Drawing.Color.Green;
            this.graphView.GridSize = ((ushort)(25));
            this.graphView.HighQuality = true;
            this.graphView.LineInterval = ((ushort)(5));
            this.graphView.Location = new System.Drawing.Point(7, 48);
            this.graphView.MaxLabel = "0000 sets/s";
            this.graphView.MaxPeekMagnitude = 100;
            this.graphView.MinLabel = "                    ";
            this.graphView.MinPeekMagnitude = 0;
            this.graphView.Name = "graphView";
            this.graphView.ShowGrid = true;
            this.graphView.ShowLabels = true;
            this.graphView.Size = new System.Drawing.Size(540, 120);
            this.graphView.TabIndex = 4;
            this.graphView.TextColor = System.Drawing.Color.Yellow;
            // 
            // pgBar
            // 
            this.pgBar.Location = new System.Drawing.Point(7, 22);
            this.pgBar.Name = "pgBar";
            this.pgBar.Size = new System.Drawing.Size(540, 21);
            this.pgBar.TabIndex = 3;
            // 
            // lbProgressLog
            // 
            this.lbProgressLog.AutoSize = true;
            this.lbProgressLog.Location = new System.Drawing.Point(4, 6);
            this.lbProgressLog.Name = "lbProgressLog";
            this.lbProgressLog.Size = new System.Drawing.Size(68, 13);
            this.lbProgressLog.TabIndex = 2;
            this.lbProgressLog.Text = "Progress log:";
            // 
            // panelSep0
            // 
            this.panelSep0.BackColor = System.Drawing.SystemColors.ControlText;
            this.panelSep0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSep0.Location = new System.Drawing.Point(0, 0);
            this.panelSep0.Name = "panelSep0";
            this.panelSep0.Size = new System.Drawing.Size(556, 1);
            this.panelSep0.TabIndex = 1;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsSelectedProvider});
            this.statusStrip.Location = new System.Drawing.Point(0, 173);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(556, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tsSelectedProvider
            // 
            this.tsSelectedProvider.Name = "tsSelectedProvider";
            this.tsSelectedProvider.Size = new System.Drawing.Size(136, 17);
            this.tsSelectedProvider.Text = "Selected Algorithm: [None]";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.dcalc";
            this.openFileDialog.Filter = "Workspace Files|*.dcalc";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "*.dcalc";
            this.saveFileDialog.Filter = "Workspace Files|*.dcalc";
            // 
            // pgBarTimer
            // 
            this.pgBarTimer.Interval = 200;
            this.pgBarTimer.Tick += new System.EventHandler(this.pgBarTimer_Tick);
            // 
            // DCalcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 440);
            this.Controls.Add(this.panelComponent);
            this.Controls.Add(this.panelServers);
            this.Controls.Add(this.mainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.Name = "DCalcForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DCalc Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DCalcForm_FormClosing);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.panelServers.ResumeLayout(false);
            this.panelServers.PerformLayout();
            this.panelComponent.ResumeLayout(false);
            this.panelComponent.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem algorithmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel panelServers;
        private System.Windows.Forms.Panel panelComponent;
        private System.Windows.Forms.ImageList imageListServerStatus;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Label lbServers;
        private System.Windows.Forms.TreeView tvServers;
        private System.Windows.Forms.Button btRemoveServer;
        private System.Windows.Forms.Button btAddServer;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Panel panelSep0;
        private System.Windows.Forms.Button btEditServer;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripStatusLabel tsSelectedProvider;
        private System.Windows.Forms.ProgressBar pgBar;
        private System.Windows.Forms.Label lbProgressLog;
        private System.Windows.Forms.Timer pgBarTimer;
        private CustomUIControls.Graphing.C2DPushGraph graphView;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.Label lbStats;
    }
}

