namespace ServiceSmallTool
{
    partial class ClearLog
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
            this.btnClearLog = new System.Windows.Forms.Button();
            this.lblLogDir = new System.Windows.Forms.Label();
            this.txtLogDir = new System.Windows.Forms.TextBox();
            this.lblDayBefore = new System.Windows.Forms.Label();
            this.txtDayBefore = new System.Windows.Forms.TextBox();
            this.lblDayBeforeExt = new System.Windows.Forms.Label();
            this.lstNote = new System.Windows.Forms.ListView();
            this.btnClearNote = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(240, 58);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 0;
            this.btnClearLog.Text = "清理";
            this.btnClearLog.UseVisualStyleBackColor = true;
            // 
            // lblLogDir
            // 
            this.lblLogDir.AutoSize = true;
            this.lblLogDir.Location = new System.Drawing.Point(12, 16);
            this.lblLogDir.Name = "lblLogDir";
            this.lblLogDir.Size = new System.Drawing.Size(53, 12);
            this.lblLogDir.TabIndex = 1;
            this.lblLogDir.Text = "日志目录";
            // 
            // txtLogDir
            // 
            this.txtLogDir.Location = new System.Drawing.Point(92, 13);
            this.txtLogDir.Name = "txtLogDir";
            this.txtLogDir.Size = new System.Drawing.Size(318, 21);
            this.txtLogDir.TabIndex = 2;
            // 
            // lblDayBefore
            // 
            this.lblDayBefore.AutoSize = true;
            this.lblDayBefore.Location = new System.Drawing.Point(12, 61);
            this.lblDayBefore.Name = "lblDayBefore";
            this.lblDayBefore.Size = new System.Drawing.Size(53, 12);
            this.lblDayBefore.TabIndex = 3;
            this.lblDayBefore.Text = "清除时限";
            // 
            // txtDayBefore
            // 
            this.txtDayBefore.Location = new System.Drawing.Point(92, 58);
            this.txtDayBefore.Name = "txtDayBefore";
            this.txtDayBefore.Size = new System.Drawing.Size(75, 21);
            this.txtDayBefore.TabIndex = 4;
            this.txtDayBefore.Text = "5";
            // 
            // lblDayBeforeExt
            // 
            this.lblDayBeforeExt.AutoSize = true;
            this.lblDayBeforeExt.Location = new System.Drawing.Point(173, 61);
            this.lblDayBeforeExt.Name = "lblDayBeforeExt";
            this.lblDayBeforeExt.Size = new System.Drawing.Size(41, 12);
            this.lblDayBeforeExt.TabIndex = 5;
            this.lblDayBeforeExt.Text = "天之前";
            // 
            // lstNote
            // 
            this.lstNote.Location = new System.Drawing.Point(14, 102);
            this.lstNote.Name = "lstNote";
            this.lstNote.Size = new System.Drawing.Size(396, 107);
            this.lstNote.TabIndex = 6;
            this.lstNote.UseCompatibleStateImageBehavior = false;
            // 
            // btnClearNote
            // 
            this.btnClearNote.Location = new System.Drawing.Point(335, 58);
            this.btnClearNote.Name = "btnClearNote";
            this.btnClearNote.Size = new System.Drawing.Size(75, 23);
            this.btnClearNote.TabIndex = 7;
            this.btnClearNote.Text = "清档";
            this.btnClearNote.UseVisualStyleBackColor = true;
            // 
            // ClearLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 221);
            this.Controls.Add(this.btnClearNote);
            this.Controls.Add(this.lstNote);
            this.Controls.Add(this.lblDayBeforeExt);
            this.Controls.Add(this.txtDayBefore);
            this.Controls.Add(this.lblDayBefore);
            this.Controls.Add(this.txtLogDir);
            this.Controls.Add(this.lblLogDir);
            this.Controls.Add(this.btnClearLog);
            this.Name = "ClearLog";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Label lblLogDir;
        private System.Windows.Forms.TextBox txtLogDir;
        private System.Windows.Forms.Label lblDayBefore;
        private System.Windows.Forms.TextBox txtDayBefore;
        private System.Windows.Forms.Label lblDayBeforeExt;
        private System.Windows.Forms.ListView lstNote;
        private System.Windows.Forms.Button btnClearNote;
    }
}

