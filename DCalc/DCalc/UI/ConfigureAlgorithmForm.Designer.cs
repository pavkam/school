namespace DCalc.UI
{
    partial class ConfigureAlgorithmForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureAlgorithmForm));
            this.lbSelectAlgorithm = new System.Windows.Forms.Label();
            this.cbbAlgorithm = new System.Windows.Forms.ComboBox();
            this.btAccept = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.gbAlgorithmInfo = new System.Windows.Forms.GroupBox();
            this.lbDescription = new System.Windows.Forms.Label();
            this.edtDescription = new System.Windows.Forms.TextBox();
            this.edtDeveloper = new System.Windows.Forms.TextBox();
            this.edtVersion = new System.Windows.Forms.TextBox();
            this.edtName = new System.Windows.Forms.TextBox();
            this.lbVersion = new System.Windows.Forms.Label();
            this.lbDeveloper = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.btConfigure = new System.Windows.Forms.Button();
            this.gbAlgorithmInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbSelectAlgorithm
            // 
            this.lbSelectAlgorithm.AutoSize = true;
            this.lbSelectAlgorithm.Location = new System.Drawing.Point(3, 6);
            this.lbSelectAlgorithm.Name = "lbSelectAlgorithm";
            this.lbSelectAlgorithm.Size = new System.Drawing.Size(86, 13);
            this.lbSelectAlgorithm.TabIndex = 0;
            this.lbSelectAlgorithm.Text = "Select Algorithm:";
            // 
            // cbbAlgorithm
            // 
            this.cbbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbAlgorithm.FormattingEnabled = true;
            this.cbbAlgorithm.Location = new System.Drawing.Point(95, 3);
            this.cbbAlgorithm.Name = "cbbAlgorithm";
            this.cbbAlgorithm.Size = new System.Drawing.Size(351, 21);
            this.cbbAlgorithm.TabIndex = 0;
            this.cbbAlgorithm.SelectedIndexChanged += new System.EventHandler(this.cbbAlgorithm_SelectedIndexChanged);
            this.cbbAlgorithm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbbAlgorithm_KeyPress);
            // 
            // btAccept
            // 
            this.btAccept.Location = new System.Drawing.Point(371, 285);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(75, 23);
            this.btAccept.TabIndex = 1;
            this.btAccept.Text = "&Accept";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(290, 285);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // gbAlgorithmInfo
            // 
            this.gbAlgorithmInfo.Controls.Add(this.lbDescription);
            this.gbAlgorithmInfo.Controls.Add(this.edtDescription);
            this.gbAlgorithmInfo.Controls.Add(this.edtDeveloper);
            this.gbAlgorithmInfo.Controls.Add(this.edtVersion);
            this.gbAlgorithmInfo.Controls.Add(this.edtName);
            this.gbAlgorithmInfo.Controls.Add(this.lbVersion);
            this.gbAlgorithmInfo.Controls.Add(this.lbDeveloper);
            this.gbAlgorithmInfo.Controls.Add(this.lbName);
            this.gbAlgorithmInfo.Location = new System.Drawing.Point(6, 25);
            this.gbAlgorithmInfo.Name = "gbAlgorithmInfo";
            this.gbAlgorithmInfo.Size = new System.Drawing.Size(440, 254);
            this.gbAlgorithmInfo.TabIndex = 4;
            this.gbAlgorithmInfo.TabStop = false;
            this.gbAlgorithmInfo.Text = "Summary Information";
            // 
            // lbDescription
            // 
            this.lbDescription.AutoSize = true;
            this.lbDescription.Location = new System.Drawing.Point(6, 89);
            this.lbDescription.Name = "lbDescription";
            this.lbDescription.Size = new System.Drawing.Size(63, 13);
            this.lbDescription.TabIndex = 8;
            this.lbDescription.Text = "Description:";
            // 
            // edtDescription
            // 
            this.edtDescription.Location = new System.Drawing.Point(89, 87);
            this.edtDescription.Multiline = true;
            this.edtDescription.Name = "edtDescription";
            this.edtDescription.ReadOnly = true;
            this.edtDescription.Size = new System.Drawing.Size(345, 158);
            this.edtDescription.TabIndex = 7;
            this.edtDescription.TabStop = false;
            // 
            // edtDeveloper
            // 
            this.edtDeveloper.Location = new System.Drawing.Point(89, 41);
            this.edtDeveloper.Name = "edtDeveloper";
            this.edtDeveloper.ReadOnly = true;
            this.edtDeveloper.Size = new System.Drawing.Size(345, 20);
            this.edtDeveloper.TabIndex = 6;
            this.edtDeveloper.TabStop = false;
            // 
            // edtVersion
            // 
            this.edtVersion.Location = new System.Drawing.Point(89, 64);
            this.edtVersion.Name = "edtVersion";
            this.edtVersion.ReadOnly = true;
            this.edtVersion.Size = new System.Drawing.Size(100, 20);
            this.edtVersion.TabIndex = 5;
            this.edtVersion.TabStop = false;
            // 
            // edtName
            // 
            this.edtName.Location = new System.Drawing.Point(89, 17);
            this.edtName.Name = "edtName";
            this.edtName.ReadOnly = true;
            this.edtName.Size = new System.Drawing.Size(345, 20);
            this.edtName.TabIndex = 4;
            this.edtName.TabStop = false;
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(6, 67);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(45, 13);
            this.lbVersion.TabIndex = 2;
            this.lbVersion.Text = "Version:";
            // 
            // lbDeveloper
            // 
            this.lbDeveloper.AutoSize = true;
            this.lbDeveloper.Location = new System.Drawing.Point(6, 44);
            this.lbDeveloper.Name = "lbDeveloper";
            this.lbDeveloper.Size = new System.Drawing.Size(59, 13);
            this.lbDeveloper.TabIndex = 1;
            this.lbDeveloper.Text = "Developer:";
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(6, 20);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(38, 13);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Name:";
            // 
            // btConfigure
            // 
            this.btConfigure.Location = new System.Drawing.Point(6, 285);
            this.btConfigure.Name = "btConfigure";
            this.btConfigure.Size = new System.Drawing.Size(136, 23);
            this.btConfigure.TabIndex = 5;
            this.btConfigure.Text = "C&onfigure Algorithm";
            this.btConfigure.UseVisualStyleBackColor = true;
            this.btConfigure.Click += new System.EventHandler(this.btConfigure_Click);
            // 
            // ConfigureAlgorithmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 312);
            this.Controls.Add(this.btConfigure);
            this.Controls.Add(this.gbAlgorithmInfo);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.cbbAlgorithm);
            this.Controls.Add(this.lbSelectAlgorithm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureAlgorithmForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select and configure your Algorithm";
            this.gbAlgorithmInfo.ResumeLayout(false);
            this.gbAlgorithmInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbSelectAlgorithm;
        private System.Windows.Forms.ComboBox cbbAlgorithm;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.GroupBox gbAlgorithmInfo;
        private System.Windows.Forms.Label lbDescription;
        private System.Windows.Forms.TextBox edtDescription;
        private System.Windows.Forms.TextBox edtDeveloper;
        private System.Windows.Forms.TextBox edtVersion;
        private System.Windows.Forms.TextBox edtName;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label lbDeveloper;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Button btConfigure;
    }
}