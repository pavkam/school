namespace MMatrixAlgorithm.UI
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
            this.lbARows = new System.Windows.Forms.Label();
            this.lbAColumns = new System.Windows.Forms.Label();
            this.lbBRows = new System.Windows.Forms.Label();
            this.lbBColumns = new System.Windows.Forms.Label();
            this.edtBRows = new System.Windows.Forms.TextBox();
            this.edtARows = new System.Windows.Forms.TextBox();
            this.edtAColumns = new System.Windows.Forms.TextBox();
            this.edtBColumns = new System.Windows.Forms.TextBox();
            this.btAccept = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbARows
            // 
            this.lbARows.AutoSize = true;
            this.lbARows.Location = new System.Drawing.Point(2, 3);
            this.lbARows.Name = "lbARows";
            this.lbARows.Size = new System.Drawing.Size(98, 13);
            this.lbARows.TabIndex = 0;
            this.lbARows.Text = "Matrix A row count:";
            // 
            // lbAColumns
            // 
            this.lbAColumns.AutoSize = true;
            this.lbAColumns.Location = new System.Drawing.Point(2, 25);
            this.lbAColumns.Name = "lbAColumns";
            this.lbAColumns.Size = new System.Drawing.Size(115, 13);
            this.lbAColumns.TabIndex = 1;
            this.lbAColumns.Text = "Matrix A column count:";
            // 
            // lbBRows
            // 
            this.lbBRows.AutoSize = true;
            this.lbBRows.Location = new System.Drawing.Point(2, 47);
            this.lbBRows.Name = "lbBRows";
            this.lbBRows.Size = new System.Drawing.Size(98, 13);
            this.lbBRows.TabIndex = 2;
            this.lbBRows.Text = "Matrix B row count:";
            // 
            // lbBColumns
            // 
            this.lbBColumns.AutoSize = true;
            this.lbBColumns.Location = new System.Drawing.Point(2, 69);
            this.lbBColumns.Name = "lbBColumns";
            this.lbBColumns.Size = new System.Drawing.Size(115, 13);
            this.lbBColumns.TabIndex = 3;
            this.lbBColumns.Text = "Matrix B column count:";
            // 
            // edtBRows
            // 
            this.edtBRows.Location = new System.Drawing.Point(126, 44);
            this.edtBRows.Name = "edtBRows";
            this.edtBRows.Size = new System.Drawing.Size(100, 20);
            this.edtBRows.TabIndex = 2;
            this.edtBRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtBRows.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edtBRows.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_KeyPress);
            // 
            // edtARows
            // 
            this.edtARows.Location = new System.Drawing.Point(126, 0);
            this.edtARows.Name = "edtARows";
            this.edtARows.Size = new System.Drawing.Size(100, 20);
            this.edtARows.TabIndex = 0;
            this.edtARows.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtARows.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edtARows.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_KeyPress);
            // 
            // edtAColumns
            // 
            this.edtAColumns.Location = new System.Drawing.Point(126, 22);
            this.edtAColumns.Name = "edtAColumns";
            this.edtAColumns.Size = new System.Drawing.Size(100, 20);
            this.edtAColumns.TabIndex = 1;
            this.edtAColumns.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtAColumns.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edtAColumns.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_KeyPress);
            // 
            // edtBColumns
            // 
            this.edtBColumns.Location = new System.Drawing.Point(126, 66);
            this.edtBColumns.Name = "edtBColumns";
            this.edtBColumns.Size = new System.Drawing.Size(100, 20);
            this.edtBColumns.TabIndex = 3;
            this.edtBColumns.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edtBColumns.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edtBColumns.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_KeyPress);
            // 
            // btAccept
            // 
            this.btAccept.Location = new System.Drawing.Point(151, 106);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(75, 23);
            this.btAccept.TabIndex = 5;
            this.btAccept.Text = "&Accept";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(72, 106);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 131);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.edtBColumns);
            this.Controls.Add(this.edtAColumns);
            this.Controls.Add(this.edtARows);
            this.Controls.Add(this.edtBRows);
            this.Controls.Add(this.lbBColumns);
            this.Controls.Add(this.lbBRows);
            this.Controls.Add(this.lbAColumns);
            this.Controls.Add(this.lbARows);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Matrix Multiplication Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbARows;
        private System.Windows.Forms.Label lbAColumns;
        private System.Windows.Forms.Label lbBRows;
        private System.Windows.Forms.Label lbBColumns;
        private System.Windows.Forms.TextBox edtBRows;
        private System.Windows.Forms.TextBox edtARows;
        private System.Windows.Forms.TextBox edtAColumns;
        private System.Windows.Forms.TextBox edtBColumns;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.Button btCancel;
    }
}