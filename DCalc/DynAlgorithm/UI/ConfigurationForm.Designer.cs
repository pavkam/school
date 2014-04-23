namespace DynAlgorithm.UI
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
            this.btAccept = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.lbIntInterval = new System.Windows.Forms.Label();
            this.edtStartInt = new System.Windows.Forms.TextBox();
            this.edtEndInt = new System.Windows.Forms.TextBox();
            this.gbFunction = new System.Windows.Forms.GroupBox();
            this.edtCode = new System.Windows.Forms.TextBox();
            this.lbDefEnd = new System.Windows.Forms.Label();
            this.cbbReturnType = new System.Windows.Forms.ComboBox();
            this.lbDefinition = new System.Windows.Forms.Label();
            this.gbFunction.SuspendLayout();
            this.SuspendLayout();
            // 
            // btAccept
            // 
            this.btAccept.Location = new System.Drawing.Point(275, 239);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(75, 23);
            this.btAccept.TabIndex = 3;
            this.btAccept.Text = "&Accept";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(196, 239);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // lbIntInterval
            // 
            this.lbIntInterval.AutoSize = true;
            this.lbIntInterval.Location = new System.Drawing.Point(3, 9);
            this.lbIntInterval.Name = "lbIntInterval";
            this.lbIntInterval.Size = new System.Drawing.Size(161, 13);
            this.lbIntInterval.TabIndex = 7;
            this.lbIntInterval.Text = "Generate integers in this interval:";
            // 
            // edtStartInt
            // 
            this.edtStartInt.Location = new System.Drawing.Point(170, 6);
            this.edtStartInt.Name = "edtStartInt";
            this.edtStartInt.Size = new System.Drawing.Size(85, 20);
            this.edtStartInt.TabIndex = 0;
            this.edtStartInt.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edtStartInt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_KeyPress);
            // 
            // edtEndInt
            // 
            this.edtEndInt.Location = new System.Drawing.Point(261, 6);
            this.edtEndInt.Name = "edtEndInt";
            this.edtEndInt.Size = new System.Drawing.Size(89, 20);
            this.edtEndInt.TabIndex = 1;
            this.edtEndInt.TextChanged += new System.EventHandler(this.text_TextChanged);
            this.edtEndInt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_KeyPress);
            // 
            // gbFunction
            // 
            this.gbFunction.Controls.Add(this.edtCode);
            this.gbFunction.Controls.Add(this.lbDefEnd);
            this.gbFunction.Controls.Add(this.cbbReturnType);
            this.gbFunction.Controls.Add(this.lbDefinition);
            this.gbFunction.Location = new System.Drawing.Point(6, 32);
            this.gbFunction.Name = "gbFunction";
            this.gbFunction.Size = new System.Drawing.Size(344, 201);
            this.gbFunction.TabIndex = 2;
            this.gbFunction.TabStop = false;
            this.gbFunction.Text = "Edit Function";
            // 
            // edtCode
            // 
            this.edtCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edtCode.Location = new System.Drawing.Point(6, 43);
            this.edtCode.Multiline = true;
            this.edtCode.Name = "edtCode";
            this.edtCode.Size = new System.Drawing.Size(329, 140);
            this.edtCode.TabIndex = 1;
            // 
            // lbDefEnd
            // 
            this.lbDefEnd.AutoSize = true;
            this.lbDefEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDefEnd.Location = new System.Drawing.Point(3, 185);
            this.lbDefEnd.Name = "lbDefEnd";
            this.lbDefEnd.Size = new System.Drawing.Size(12, 13);
            this.lbDefEnd.TabIndex = 2;
            this.lbDefEnd.Text = "}";
            // 
            // cbbReturnType
            // 
            this.cbbReturnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbReturnType.FormattingEnabled = true;
            this.cbbReturnType.Items.AddRange(new object[] {
            "Int32",
            "Double",
            "Object[]"});
            this.cbbReturnType.Location = new System.Drawing.Point(6, 16);
            this.cbbReturnType.Name = "cbbReturnType";
            this.cbbReturnType.Size = new System.Drawing.Size(83, 21);
            this.cbbReturnType.TabIndex = 0;
            this.cbbReturnType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_KeyPress);
            // 
            // lbDefinition
            // 
            this.lbDefinition.AutoSize = true;
            this.lbDefinition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDefinition.Location = new System.Drawing.Point(95, 19);
            this.lbDefinition.Name = "lbDefinition";
            this.lbDefinition.Size = new System.Drawing.Size(181, 13);
            this.lbDefinition.TabIndex = 0;
            this.lbDefinition.Text = "FunctionToDistribute(Int32 x) {";
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 265);
            this.Controls.Add(this.gbFunction);
            this.Controls.Add(this.edtEndInt);
            this.Controls.Add(this.edtStartInt);
            this.Controls.Add(this.lbIntInterval);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btAccept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure the Dynamic algorithm";
            this.gbFunction.ResumeLayout(false);
            this.gbFunction.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lbIntInterval;
        private System.Windows.Forms.TextBox edtStartInt;
        private System.Windows.Forms.TextBox edtEndInt;
        private System.Windows.Forms.GroupBox gbFunction;
        private System.Windows.Forms.TextBox edtCode;
        private System.Windows.Forms.Label lbDefEnd;
        private System.Windows.Forms.ComboBox cbbReturnType;
        private System.Windows.Forms.Label lbDefinition;
    }
}