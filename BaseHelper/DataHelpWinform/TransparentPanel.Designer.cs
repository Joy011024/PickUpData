namespace DataHelpWinform
{
    partial class TransparentPanel
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
            this.baseTextBox = new DataHelpWinform.BaseTextBox();
            this.SuspendLayout();
            // 
            // baseTextBox
            // 
            this.baseTextBox.ImeMode = System.Windows.Forms.ImeMode.On;
            this.baseTextBox.Location = new System.Drawing.Point(4, 5);
            this.baseTextBox.Name = "baseTextBox";
            this.baseTextBox.Size = new System.Drawing.Size(100, 21);
            this.baseTextBox.TabIndex = 0;
            // 
            // TransparentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseTextBox);
            this.Name = "TransparentPanel";
            this.Size = new System.Drawing.Size(117, 29);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BaseTextBox baseTextBox;

    }
}
