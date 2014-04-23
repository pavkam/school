namespace DCalc.UI
{
    partial class EditServerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditServerForm));
            this.lbServerName = new System.Windows.Forms.Label();
            this.lbServerHost = new System.Windows.Forms.Label();
            this.lbServerPort = new System.Windows.Forms.Label();
            this.cbIsEnabled = new System.Windows.Forms.CheckBox();
            this.edtServerHost = new System.Windows.Forms.TextBox();
            this.edServerPort = new System.Windows.Forms.TextBox();
            this.edtServerName = new System.Windows.Forms.TextBox();
            this.btAccept = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.cbSecure = new System.Windows.Forms.CheckBox();
            this.edtSecurityKey = new System.Windows.Forms.TextBox();
            this.lbType = new System.Windows.Forms.Label();
            this.cbbType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lbServerName
            // 
            this.lbServerName.AutoSize = true;
            this.lbServerName.Location = new System.Drawing.Point(4, 4);
            this.lbServerName.Name = "lbServerName";
            this.lbServerName.Size = new System.Drawing.Size(38, 13);
            this.lbServerName.TabIndex = 0;
            this.lbServerName.Text = "Name:";
            // 
            // lbServerHost
            // 
            this.lbServerHost.AutoSize = true;
            this.lbServerHost.Location = new System.Drawing.Point(4, 25);
            this.lbServerHost.Name = "lbServerHost";
            this.lbServerHost.Size = new System.Drawing.Size(32, 13);
            this.lbServerHost.TabIndex = 1;
            this.lbServerHost.Text = "Host:";
            // 
            // lbServerPort
            // 
            this.lbServerPort.AutoSize = true;
            this.lbServerPort.Location = new System.Drawing.Point(4, 68);
            this.lbServerPort.Name = "lbServerPort";
            this.lbServerPort.Size = new System.Drawing.Size(29, 13);
            this.lbServerPort.TabIndex = 2;
            this.lbServerPort.Text = "Port:";
            // 
            // cbIsEnabled
            // 
            this.cbIsEnabled.AutoSize = true;
            this.cbIsEnabled.Location = new System.Drawing.Point(166, 67);
            this.cbIsEnabled.Name = "cbIsEnabled";
            this.cbIsEnabled.Size = new System.Drawing.Size(96, 17);
            this.cbIsEnabled.TabIndex = 4;
            this.cbIsEnabled.Text = "&Use this server";
            this.cbIsEnabled.UseVisualStyleBackColor = true;
            this.cbIsEnabled.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.control_KeyPress);
            // 
            // edtServerHost
            // 
            this.edtServerHost.Location = new System.Drawing.Point(80, 22);
            this.edtServerHost.Name = "edtServerHost";
            this.edtServerHost.Size = new System.Drawing.Size(234, 20);
            this.edtServerHost.TabIndex = 1;
            this.edtServerHost.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edtServerHost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.control_KeyPress);
            // 
            // edServerPort
            // 
            this.edServerPort.Location = new System.Drawing.Point(80, 65);
            this.edServerPort.Name = "edServerPort";
            this.edServerPort.Size = new System.Drawing.Size(80, 20);
            this.edServerPort.TabIndex = 3;
            this.edServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edServerPort.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edServerPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.control_KeyPress);
            // 
            // edtServerName
            // 
            this.edtServerName.Location = new System.Drawing.Point(80, 1);
            this.edtServerName.Name = "edtServerName";
            this.edtServerName.Size = new System.Drawing.Size(234, 20);
            this.edtServerName.TabIndex = 0;
            this.edtServerName.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edtServerName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.control_KeyPress);
            // 
            // btAccept
            // 
            this.btAccept.Location = new System.Drawing.Point(239, 142);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(75, 23);
            this.btAccept.TabIndex = 7;
            this.btAccept.Text = "&Accept";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(160, 142);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 8;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // cbSecure
            // 
            this.cbSecure.AutoSize = true;
            this.cbSecure.Location = new System.Drawing.Point(7, 97);
            this.cbSecure.Name = "cbSecure";
            this.cbSecure.Size = new System.Drawing.Size(129, 17);
            this.cbSecure.TabIndex = 5;
            this.cbSecure.Text = "Security &Key required:";
            this.cbSecure.UseVisualStyleBackColor = true;
            this.cbSecure.CheckedChanged += new System.EventHandler(this.cbSecure_CheckedChanged);
            // 
            // edtSecurityKey
            // 
            this.edtSecurityKey.Location = new System.Drawing.Point(7, 115);
            this.edtSecurityKey.Name = "edtSecurityKey";
            this.edtSecurityKey.Size = new System.Drawing.Size(307, 20);
            this.edtSecurityKey.TabIndex = 6;
            this.edtSecurityKey.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edtSecurityKey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.control_KeyPress);
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Location = new System.Drawing.Point(4, 47);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(34, 13);
            this.lbType.TabIndex = 8;
            this.lbType.Text = "Type:";
            // 
            // cbbType
            // 
            this.cbbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbType.FormattingEnabled = true;
            this.cbbType.Location = new System.Drawing.Point(80, 43);
            this.cbbType.Name = "cbbType";
            this.cbbType.Size = new System.Drawing.Size(234, 21);
            this.cbbType.TabIndex = 2;
            // 
            // EditServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 167);
            this.Controls.Add(this.cbbType);
            this.Controls.Add(this.lbType);
            this.Controls.Add(this.edtSecurityKey);
            this.Controls.Add(this.cbSecure);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.edtServerName);
            this.Controls.Add(this.edServerPort);
            this.Controls.Add(this.edtServerHost);
            this.Controls.Add(this.cbIsEnabled);
            this.Controls.Add(this.lbServerPort);
            this.Controls.Add(this.lbServerHost);
            this.Controls.Add(this.lbServerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditServerForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Remote Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbServerName;
        private System.Windows.Forms.Label lbServerHost;
        private System.Windows.Forms.Label lbServerPort;
        private System.Windows.Forms.CheckBox cbIsEnabled;
        private System.Windows.Forms.TextBox edtServerHost;
        private System.Windows.Forms.TextBox edServerPort;
        private System.Windows.Forms.TextBox edtServerName;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.CheckBox cbSecure;
        private System.Windows.Forms.TextBox edtSecurityKey;
        private System.Windows.Forms.Label lbType;
        private System.Windows.Forms.ComboBox cbbType;
    }
}