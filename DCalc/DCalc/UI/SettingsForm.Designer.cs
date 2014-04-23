namespace DCalc.UI
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.gbControl = new System.Windows.Forms.GroupBox();
            this.edtCustomCount = new System.Windows.Forms.TextBox();
            this.rbCustomCount = new System.Windows.Forms.RadioButton();
            this.rbProcCountThreads = new System.Windows.Forms.RadioButton();
            this.rbSingleThread = new System.Windows.Forms.RadioButton();
            this.edtQueueSize = new System.Windows.Forms.TextBox();
            this.cbbLocalLoadBalancer = new System.Windows.Forms.ComboBox();
            this.cbbRemoteLoadBalancer = new System.Windows.Forms.ComboBox();
            this.lbExecutiveQueue = new System.Windows.Forms.Label();
            this.lbRemoteLoadBalancer = new System.Windows.Forms.Label();
            this.lbLocalLoadBalancer = new System.Windows.Forms.Label();
            this.btAccept = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.gbControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbControl
            // 
            this.gbControl.Controls.Add(this.edtCustomCount);
            this.gbControl.Controls.Add(this.rbCustomCount);
            this.gbControl.Controls.Add(this.rbProcCountThreads);
            this.gbControl.Controls.Add(this.rbSingleThread);
            this.gbControl.Controls.Add(this.edtQueueSize);
            this.gbControl.Controls.Add(this.cbbLocalLoadBalancer);
            this.gbControl.Controls.Add(this.cbbRemoteLoadBalancer);
            this.gbControl.Controls.Add(this.lbExecutiveQueue);
            this.gbControl.Controls.Add(this.lbRemoteLoadBalancer);
            this.gbControl.Controls.Add(this.lbLocalLoadBalancer);
            this.gbControl.Location = new System.Drawing.Point(1, -1);
            this.gbControl.Name = "gbControl";
            this.gbControl.Size = new System.Drawing.Size(309, 190);
            this.gbControl.TabIndex = 0;
            this.gbControl.TabStop = false;
            this.gbControl.Text = "Dispatcher Setup";
            // 
            // edtCustomCount
            // 
            this.edtCustomCount.Location = new System.Drawing.Point(196, 162);
            this.edtCustomCount.Name = "edtCustomCount";
            this.edtCustomCount.Size = new System.Drawing.Size(72, 20);
            this.edtCustomCount.TabIndex = 6;
            this.edtCustomCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtCustomCount.TextChanged += new System.EventHandler(this.edtQueueSize_TextChanged);
            this.edtCustomCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.all_KeyPress);
            // 
            // rbCustomCount
            // 
            this.rbCustomCount.AutoSize = true;
            this.rbCustomCount.Location = new System.Drawing.Point(9, 163);
            this.rbCustomCount.Name = "rbCustomCount";
            this.rbCustomCount.Size = new System.Drawing.Size(187, 17);
            this.rbCustomCount.TabIndex = 5;
            this.rbCustomCount.TabStop = true;
            this.rbCustomCount.Text = "Spawn a c&ustom count of threads:";
            this.rbCustomCount.UseVisualStyleBackColor = true;
            this.rbCustomCount.CheckedChanged += new System.EventHandler(this.rbCustomCount_CheckedChanged);
            // 
            // rbProcCountThreads
            // 
            this.rbProcCountThreads.AutoSize = true;
            this.rbProcCountThreads.Location = new System.Drawing.Point(9, 145);
            this.rbProcCountThreads.Name = "rbProcCountThreads";
            this.rbProcCountThreads.Size = new System.Drawing.Size(244, 17);
            this.rbProcCountThreads.TabIndex = 4;
            this.rbProcCountThreads.TabStop = true;
            this.rbProcCountThreads.Text = "Spawn a thread on &each locally available CPU";
            this.rbProcCountThreads.UseVisualStyleBackColor = true;
            // 
            // rbSingleThread
            // 
            this.rbSingleThread.AutoSize = true;
            this.rbSingleThread.Location = new System.Drawing.Point(9, 128);
            this.rbSingleThread.Name = "rbSingleThread";
            this.rbSingleThread.Size = new System.Drawing.Size(174, 17);
            this.rbSingleThread.TabIndex = 3;
            this.rbSingleThread.TabStop = true;
            this.rbSingleThread.Text = "Spawn one &single thread locally";
            this.rbSingleThread.UseVisualStyleBackColor = true;
            // 
            // edtQueueSize
            // 
            this.edtQueueSize.Location = new System.Drawing.Point(127, 100);
            this.edtQueueSize.Name = "edtQueueSize";
            this.edtQueueSize.Size = new System.Drawing.Size(72, 20);
            this.edtQueueSize.TabIndex = 2;
            this.edtQueueSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtQueueSize.TextChanged += new System.EventHandler(this.edtQueueSize_TextChanged);
            this.edtQueueSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.all_KeyPress);
            // 
            // cbbLocalLoadBalancer
            // 
            this.cbbLocalLoadBalancer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbLocalLoadBalancer.FormattingEnabled = true;
            this.cbbLocalLoadBalancer.Location = new System.Drawing.Point(9, 32);
            this.cbbLocalLoadBalancer.Name = "cbbLocalLoadBalancer";
            this.cbbLocalLoadBalancer.Size = new System.Drawing.Size(294, 21);
            this.cbbLocalLoadBalancer.TabIndex = 0;
            this.cbbLocalLoadBalancer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.all_KeyPress);
            // 
            // cbbRemoteLoadBalancer
            // 
            this.cbbRemoteLoadBalancer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbRemoteLoadBalancer.FormattingEnabled = true;
            this.cbbRemoteLoadBalancer.Location = new System.Drawing.Point(9, 72);
            this.cbbRemoteLoadBalancer.Name = "cbbRemoteLoadBalancer";
            this.cbbRemoteLoadBalancer.Size = new System.Drawing.Size(294, 21);
            this.cbbRemoteLoadBalancer.TabIndex = 1;
            this.cbbRemoteLoadBalancer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.all_KeyPress);
            // 
            // lbExecutiveQueue
            // 
            this.lbExecutiveQueue.AutoSize = true;
            this.lbExecutiveQueue.Location = new System.Drawing.Point(6, 103);
            this.lbExecutiveQueue.Name = "lbExecutiveQueue";
            this.lbExecutiveQueue.Size = new System.Drawing.Size(115, 13);
            this.lbExecutiveQueue.TabIndex = 2;
            this.lbExecutiveQueue.Text = "Executive Queue Size:";
            // 
            // lbRemoteLoadBalancer
            // 
            this.lbRemoteLoadBalancer.AutoSize = true;
            this.lbRemoteLoadBalancer.Location = new System.Drawing.Point(6, 56);
            this.lbRemoteLoadBalancer.Name = "lbRemoteLoadBalancer";
            this.lbRemoteLoadBalancer.Size = new System.Drawing.Size(210, 13);
            this.lbRemoteLoadBalancer.TabIndex = 1;
            this.lbRemoteLoadBalancer.Text = "Load Balancer between all executive units:";
            // 
            // lbLocalLoadBalancer
            // 
            this.lbLocalLoadBalancer.AutoSize = true;
            this.lbLocalLoadBalancer.Location = new System.Drawing.Point(6, 16);
            this.lbLocalLoadBalancer.Name = "lbLocalLoadBalancer";
            this.lbLocalLoadBalancer.Size = new System.Drawing.Size(193, 13);
            this.lbLocalLoadBalancer.TabIndex = 0;
            this.lbLocalLoadBalancer.Text = "Load bancer between local processors:";
            // 
            // btAccept
            // 
            this.btAccept.Location = new System.Drawing.Point(235, 195);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(75, 23);
            this.btAccept.TabIndex = 1;
            this.btAccept.Text = "&Accept";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(156, 195);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 220);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.gbControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure Execution Options";
            this.gbControl.ResumeLayout(false);
            this.gbControl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbControl;
        private System.Windows.Forms.TextBox edtQueueSize;
        private System.Windows.Forms.ComboBox cbbLocalLoadBalancer;
        private System.Windows.Forms.ComboBox cbbRemoteLoadBalancer;
        private System.Windows.Forms.Label lbExecutiveQueue;
        private System.Windows.Forms.Label lbRemoteLoadBalancer;
        private System.Windows.Forms.Label lbLocalLoadBalancer;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.RadioButton rbProcCountThreads;
        private System.Windows.Forms.RadioButton rbSingleThread;
        private System.Windows.Forms.TextBox edtCustomCount;
        private System.Windows.Forms.RadioButton rbCustomCount;
    }
}