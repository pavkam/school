namespace MMatrixAlgorithm.UI
{
    partial class ResultsForm
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
            this.btClose = new System.Windows.Forms.Button();
            this.btSaveMatrixR = new System.Windows.Forms.Button();
            this.btSaveMatrixB = new System.Windows.Forms.Button();
            this.btSaveMatrixA = new System.Windows.Forms.Button();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(75, 113);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 0;
            this.btClose.Text = "&Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btSaveMatrixR
            // 
            this.btSaveMatrixR.Location = new System.Drawing.Point(46, 62);
            this.btSaveMatrixR.Name = "btSaveMatrixR";
            this.btSaveMatrixR.Size = new System.Drawing.Size(135, 23);
            this.btSaveMatrixR.TabIndex = 1;
            this.btSaveMatrixR.Text = "Save &Resulting Matrix";
            this.btSaveMatrixR.UseVisualStyleBackColor = true;
            this.btSaveMatrixR.Click += new System.EventHandler(this.btSaveMatrixR_Click);
            // 
            // btSaveMatrixB
            // 
            this.btSaveMatrixB.Location = new System.Drawing.Point(46, 33);
            this.btSaveMatrixB.Name = "btSaveMatrixB";
            this.btSaveMatrixB.Size = new System.Drawing.Size(135, 23);
            this.btSaveMatrixB.TabIndex = 2;
            this.btSaveMatrixB.Text = "Save Matrix &B";
            this.btSaveMatrixB.UseVisualStyleBackColor = true;
            this.btSaveMatrixB.Click += new System.EventHandler(this.btSaveMatrixB_Click);
            // 
            // btSaveMatrixA
            // 
            this.btSaveMatrixA.Location = new System.Drawing.Point(46, 4);
            this.btSaveMatrixA.Name = "btSaveMatrixA";
            this.btSaveMatrixA.Size = new System.Drawing.Size(135, 23);
            this.btSaveMatrixA.TabIndex = 3;
            this.btSaveMatrixA.Text = "Save Matrix &A";
            this.btSaveMatrixA.UseVisualStyleBackColor = true;
            this.btSaveMatrixA.Click += new System.EventHandler(this.btSaveMatrixA_Click);
            // 
            // saveDialog
            // 
            this.saveDialog.DefaultExt = "*.txt";
            this.saveDialog.Filter = "Text Files|*.txt";
            // 
            // ResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 138);
            this.Controls.Add(this.btSaveMatrixA);
            this.Controls.Add(this.btSaveMatrixB);
            this.Controls.Add(this.btSaveMatrixR);
            this.Controls.Add(this.btClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResultsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Multiplication Results";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btSaveMatrixR;
        private System.Windows.Forms.Button btSaveMatrixB;
        private System.Windows.Forms.Button btSaveMatrixA;
        private System.Windows.Forms.SaveFileDialog saveDialog;
    }
}