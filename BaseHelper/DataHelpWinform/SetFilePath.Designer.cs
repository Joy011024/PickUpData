namespace DataHelpWinform
{
    partial class SetFilePath
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
            this.lblFilePath = new System.Windows.Forms.Label();
            this.btnSelectFilePath = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(62, 0);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(231, 21);
            this.txtFilePath.TabIndex = 13;
            this.txtFilePath.Tag = "";
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(3, 3);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(53, 12);
            this.lblFilePath.TabIndex = 12;
            this.lblFilePath.Text = "存储路径";
            // 
            // btnSelectFilePath
            // 
            this.btnSelectFilePath.Location = new System.Drawing.Point(297, 0);
            this.btnSelectFilePath.Name = "btnSelectFilePath";
            this.btnSelectFilePath.Size = new System.Drawing.Size(64, 25);
            this.btnSelectFilePath.TabIndex = 11;
            this.btnSelectFilePath.Text = "选择路径";
            this.btnSelectFilePath.UseVisualStyleBackColor = true;
            this.btnSelectFilePath.Click += new System.EventHandler(this.Button_Click);
            // 
            // SetFilePath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.btnSelectFilePath);
            this.Name = "SetFilePath";
            this.Size = new System.Drawing.Size(364, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Button btnSelectFilePath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}
