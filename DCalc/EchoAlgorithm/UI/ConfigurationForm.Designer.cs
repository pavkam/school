namespace EchoAlgorithm.UI
{
    partial class ConfigurationForm
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
            this.btCancel = new System.Windows.Forms.Button();
            this.lbDelayCycles = new System.Windows.Forms.Label();
            this.edtDelayCycles = new System.Windows.Forms.TextBox();
            this.btAccept = new System.Windows.Forms.Button();
            this.edtNumberOfSets = new System.Windows.Forms.TextBox();
            this.lbNumberOfSets = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(52, 74);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // lbDelayCycles
            // 
            this.lbDelayCycles.AutoSize = true;
            this.lbDelayCycles.Location = new System.Drawing.Point(1, 7);
            this.lbDelayCycles.Name = "lbDelayCycles";
            this.lbDelayCycles.Size = new System.Drawing.Size(70, 13);
            this.lbDelayCycles.TabIndex = 1;
            this.lbDelayCycles.Text = "Delay cycles:";
            // 
            // edtDelayCycles
            // 
            this.edtDelayCycles.Location = new System.Drawing.Point(88, 4);
            this.edtDelayCycles.Name = "edtDelayCycles";
            this.edtDelayCycles.Size = new System.Drawing.Size(118, 20);
            this.edtDelayCycles.TabIndex = 0;
            this.edtDelayCycles.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtDelayCycles.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edtDelayCycles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_KeyPress);
            // 
            // btAccept
            // 
            this.btAccept.Location = new System.Drawing.Point(131, 74);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(75, 23);
            this.btAccept.TabIndex = 2;
            this.btAccept.Text = "&Accept";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // edtNumberOfSets
            // 
            this.edtNumberOfSets.Location = new System.Drawing.Point(88, 30);
            this.edtNumberOfSets.Name = "edtNumberOfSets";
            this.edtNumberOfSets.Size = new System.Drawing.Size(118, 20);
            this.edtNumberOfSets.TabIndex = 1;
            this.edtNumberOfSets.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtNumberOfSets.TextChanged += new System.EventHandler(this.text_TextChanged);
            // 
            // lbNumberOfSets
            // 
            this.lbNumberOfSets.AutoSize = true;
            this.lbNumberOfSets.Location = new System.Drawing.Point(1, 33);
            this.lbNumberOfSets.Name = "lbNumberOfSets";
            this.lbNumberOfSets.Size = new System.Drawing.Size(81, 13);
            this.lbNumberOfSets.TabIndex = 4;
            this.lbNumberOfSets.Text = "Number of sets:";
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(208, 100);
            this.Controls.Add(this.edtNumberOfSets);
            this.Controls.Add(this.lbNumberOfSets);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.edtDelayCycles);
            this.Controls.Add(this.lbDelayCycles);
            this.Controls.Add(this.btCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure Echo Algorithm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lbDelayCycles;
        private System.Windows.Forms.TextBox edtDelayCycles;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.TextBox edtNumberOfSets;
        private System.Windows.Forms.Label lbNumberOfSets;

    }
}