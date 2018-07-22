namespace DataHelpWinform
{
    partial class SelectFile
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.btnSelectExcelFile = new System.Windows.Forms.Button();
            this.SelectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(64, 3);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(231, 21);
            this.txtFilePath.TabIndex = 10;
            this.txtFilePath.Tag = "";
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(5, 6);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(53, 12);
            this.lblFile.TabIndex = 9;
            this.lblFile.Text = "文件路径";
            // 
            // btnSelectExcelFile
            // 
            this.btnSelectExcelFile.Location = new System.Drawing.Point(301, -1);
            this.btnSelectExcelFile.Name = "btnSelectExcelFile";
            this.btnSelectExcelFile.Size = new System.Drawing.Size(64, 25);
            this.btnSelectExcelFile.TabIndex = 8;
            this.btnSelectExcelFile.Text = "选择文件";
            this.btnSelectExcelFile.UseVisualStyleBackColor = true;
            this.btnSelectExcelFile.Click += new System.EventHandler(this.Button_Click);
            // 
            // SelectFileDialog
            // 
            this.SelectFileDialog.FileName = "选择文件";
            // 
            // SelectFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.btnSelectExcelFile);
            this.Name = "SelectFile";
            this.Size = new System.Drawing.Size(372, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Button btnSelectExcelFile;
        private System.Windows.Forms.OpenFileDialog SelectFileDialog;
    }
}
