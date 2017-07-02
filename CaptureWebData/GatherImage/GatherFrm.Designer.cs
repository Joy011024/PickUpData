namespace GatherImage
{
    partial class GatherFrm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.lsbProcess = new System.Windows.Forms.ListBox();
            this.BtnClearPRocess = new System.Windows.Forms.Button();
            this.gbImagType = new System.Windows.Forms.GroupBox();
            this.rbOrigin = new System.Windows.Forms.RadioButton();
            this.rbZip = new System.Windows.Forms.RadioButton();
            this.gbImagType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 20);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(99, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始采集头像";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lsbProcess
            // 
            this.lsbProcess.FormattingEnabled = true;
            this.lsbProcess.ItemHeight = 12;
            this.lsbProcess.Location = new System.Drawing.Point(12, 57);
            this.lsbProcess.Name = "lsbProcess";
            this.lsbProcess.Size = new System.Drawing.Size(586, 280);
            this.lsbProcess.TabIndex = 1;
            // 
            // BtnClearPRocess
            // 
            this.BtnClearPRocess.Location = new System.Drawing.Point(485, 16);
            this.BtnClearPRocess.Name = "BtnClearPRocess";
            this.BtnClearPRocess.Size = new System.Drawing.Size(75, 23);
            this.BtnClearPRocess.TabIndex = 2;
            this.BtnClearPRocess.Text = "清除进度";
            this.BtnClearPRocess.UseVisualStyleBackColor = true;
            this.BtnClearPRocess.Click += new System.EventHandler(this.BtnClearPRocess_Click);
            // 
            // gbImagType
            // 
            this.gbImagType.Controls.Add(this.rbZip);
            this.gbImagType.Controls.Add(this.rbOrigin);
            this.gbImagType.Location = new System.Drawing.Point(132, 9);
            this.gbImagType.Name = "gbImagType";
            this.gbImagType.Size = new System.Drawing.Size(141, 42);
            this.gbImagType.TabIndex = 3;
            this.gbImagType.TabStop = false;
            this.gbImagType.Text = "图片类型";
            // 
            // rbOrigin
            // 
            this.rbOrigin.AutoSize = true;
            this.rbOrigin.Location = new System.Drawing.Point(35, 18);
            this.rbOrigin.Name = "rbOrigin";
            this.rbOrigin.Size = new System.Drawing.Size(47, 16);
            this.rbOrigin.TabIndex = 0;
            this.rbOrigin.TabStop = true;
            this.rbOrigin.Text = "原图";
            this.rbOrigin.UseVisualStyleBackColor = true;
            // 
            // rbZip
            // 
            this.rbZip.AutoSize = true;
            this.rbZip.Checked = true;
            this.rbZip.Location = new System.Drawing.Point(88, 19);
            this.rbZip.Name = "rbZip";
            this.rbZip.Size = new System.Drawing.Size(47, 16);
            this.rbZip.TabIndex = 1;
            this.rbZip.TabStop = true;
            this.rbZip.Text = "压缩";
            this.rbZip.UseVisualStyleBackColor = true;
            // 
            // GatherFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 361);
            this.Controls.Add(this.gbImagType);
            this.Controls.Add(this.BtnClearPRocess);
            this.Controls.Add(this.lsbProcess);
            this.Controls.Add(this.btnStart);
            this.Name = "GatherFrm";
            this.Text = "Form1";
            this.gbImagType.ResumeLayout(false);
            this.gbImagType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox lsbProcess;
        private System.Windows.Forms.Button BtnClearPRocess;
        private System.Windows.Forms.GroupBox gbImagType;
        private System.Windows.Forms.RadioButton rbZip;
        private System.Windows.Forms.RadioButton rbOrigin;
    }
}

