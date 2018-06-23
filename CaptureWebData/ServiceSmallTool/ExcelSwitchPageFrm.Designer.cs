namespace ServiceSmallTool
{
    partial class ExcelSwitchPageFrm
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
            this.selectFile = new FeatureFrmList.SelectFile();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblNumber = new System.Windows.Forms.Label();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.btnCut = new System.Windows.Forms.Button();
            this.rtbProcess = new System.Windows.Forms.RichTextBox();
            this.btnClearData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selectFile
            // 
            this.selectFile.CallBack = null;
            this.selectFile.Location = new System.Drawing.Point(97, 12);
            this.selectFile.Name = "selectFile";
            this.selectFile.Size = new System.Drawing.Size(362, 35);
            this.selectFile.TabIndex = 1;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(12, 24);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(53, 12);
            this.lblFile.TabIndex = 2;
            this.lblFile.Text = "文件路径";
            // 
            // lblNumber
            // 
            this.lblNumber.AutoSize = true;
            this.lblNumber.Location = new System.Drawing.Point(12, 68);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(53, 12);
            this.lblNumber.TabIndex = 3;
            this.lblNumber.Text = "数目(份)";
            // 
            // txtNumber
            // 
            this.txtNumber.Location = new System.Drawing.Point(97, 59);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(100, 21);
            this.txtNumber.TabIndex = 4;
            this.txtNumber.Text = "30000";
            // 
            // btnCut
            // 
            this.btnCut.Location = new System.Drawing.Point(214, 59);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(75, 23);
            this.btnCut.TabIndex = 5;
            this.btnCut.Text = "切分";
            this.btnCut.UseVisualStyleBackColor = true;
            // 
            // rtbProcess
            // 
            this.rtbProcess.Location = new System.Drawing.Point(14, 108);
            this.rtbProcess.Name = "rtbProcess";
            this.rtbProcess.Size = new System.Drawing.Size(445, 71);
            this.rtbProcess.TabIndex = 6;
            this.rtbProcess.Text = "";
            // 
            // btnClearData
            // 
            this.btnClearData.Location = new System.Drawing.Point(380, 58);
            this.btnClearData.Name = "btnClearData";
            this.btnClearData.Size = new System.Drawing.Size(75, 23);
            this.btnClearData.TabIndex = 7;
            this.btnClearData.Text = "清档";
            this.btnClearData.UseVisualStyleBackColor = true;
            // 
            // ExcelSwitchPageFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 185);
            this.Controls.Add(this.btnClearData);
            this.Controls.Add(this.rtbProcess);
            this.Controls.Add(this.btnCut);
            this.Controls.Add(this.txtNumber);
            this.Controls.Add(this.lblNumber);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.selectFile);
            this.Name = "ExcelSwitchPageFrm";
            this.Text = "excel切分";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FeatureFrmList.SelectFile selectFile;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.Button btnCut;
        private System.Windows.Forms.RichTextBox rtbProcess;
        private System.Windows.Forms.Button btnClearData;
    }
}