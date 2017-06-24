namespace FeatureFrmList
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
            this.rtbFileName = new System.Windows.Forms.RichTextBox();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // rtbFileName
            // 
            this.rtbFileName.Location = new System.Drawing.Point(3, 3);
            this.rtbFileName.Name = "rtbFileName";
            this.rtbFileName.Size = new System.Drawing.Size(271, 30);
            this.rtbFileName.TabIndex = 3;
            this.rtbFileName.Text = "";
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Location = new System.Drawing.Point(280, 10);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(75, 23);
            this.btnSelectPath.TabIndex = 2;
            this.btnSelectPath.Text = "选择路径";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            // 
            // openFile
            // 
            this.openFile.FileName = "openFileDialog1";
            // 
            // SelectFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtbFileName);
            this.Controls.Add(this.btnSelectPath);
            this.Name = "SelectFile";
            this.Size = new System.Drawing.Size(362, 35);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbFileName;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.OpenFileDialog openFile;
    }
}
