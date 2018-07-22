namespace DataHelpWinform
{
    partial class DataGenerateHelper
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
            this.lblData = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.rtbData = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(-2, 4);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(53, 12);
            this.lblData.TabIndex = 0;
            this.lblData.Text = "随机数据";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(226, -1);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(41, 23);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "生成";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.Button_Click);
            // 
            // rtbData
            // 
            this.rtbData.Location = new System.Drawing.Point(3, 23);
            this.rtbData.Name = "rtbData";
            this.rtbData.Size = new System.Drawing.Size(264, 24);
            this.rtbData.TabIndex = 3;
            this.rtbData.Text = "";
            // 
            // DataGenerateHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtbData);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.lblData);
            this.Name = "DataGenerateHelper";
            this.Size = new System.Drawing.Size(273, 47);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.RichTextBox rtbData;
    }
}
