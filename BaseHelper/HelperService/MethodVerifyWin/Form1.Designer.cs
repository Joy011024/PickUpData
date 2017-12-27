namespace MethodVerifyWin
{
    partial class Form1
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
            this.btnConvertInt16 = new System.Windows.Forms.Button();
            this.rtbInt = new System.Windows.Forms.RichTextBox();
            this.lblInt = new System.Windows.Forms.Label();
            this.lblInt16 = new System.Windows.Forms.Label();
            this.rtbInt16 = new System.Windows.Forms.RichTextBox();
            this.btnConvertInt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConvertInt16
            // 
            this.btnConvertInt16.Location = new System.Drawing.Point(59, 201);
            this.btnConvertInt16.Name = "btnConvertInt16";
            this.btnConvertInt16.Size = new System.Drawing.Size(75, 23);
            this.btnConvertInt16.TabIndex = 0;
            this.btnConvertInt16.Tag = "Convert16";
            this.btnConvertInt16.Text = "转16";
            this.btnConvertInt16.UseVisualStyleBackColor = true;
            this.btnConvertInt16.Click += new System.EventHandler(this.Button_Click);
            // 
            // rtbInt
            // 
            this.rtbInt.Location = new System.Drawing.Point(59, 154);
            this.rtbInt.Name = "rtbInt";
            this.rtbInt.Size = new System.Drawing.Size(283, 38);
            this.rtbInt.TabIndex = 1;
            this.rtbInt.Text = "";
            // 
            // lblInt
            // 
            this.lblInt.AutoSize = true;
            this.lblInt.Location = new System.Drawing.Point(12, 166);
            this.lblInt.Name = "lblInt";
            this.lblInt.Size = new System.Drawing.Size(41, 12);
            this.lblInt.TabIndex = 2;
            this.lblInt.Text = "十进制";
            // 
            // lblInt16
            // 
            this.lblInt16.AutoSize = true;
            this.lblInt16.Location = new System.Drawing.Point(12, 255);
            this.lblInt16.Name = "lblInt16";
            this.lblInt16.Size = new System.Drawing.Size(41, 12);
            this.lblInt16.TabIndex = 3;
            this.lblInt16.Text = "十进制";
            // 
            // rtbInt16
            // 
            this.rtbInt16.Location = new System.Drawing.Point(59, 239);
            this.rtbInt16.Name = "rtbInt16";
            this.rtbInt16.Size = new System.Drawing.Size(283, 38);
            this.rtbInt16.TabIndex = 4;
            this.rtbInt16.Text = "";
            // 
            // btnConvertInt
            // 
            this.btnConvertInt.Location = new System.Drawing.Point(234, 201);
            this.btnConvertInt.Name = "btnConvertInt";
            this.btnConvertInt.Size = new System.Drawing.Size(75, 23);
            this.btnConvertInt.TabIndex = 5;
            this.btnConvertInt.Tag = "Convert10";
            this.btnConvertInt.Text = "解析10";
            this.btnConvertInt.UseVisualStyleBackColor = true;
            this.btnConvertInt.Click += new System.EventHandler(this.Button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 288);
            this.Controls.Add(this.btnConvertInt);
            this.Controls.Add(this.rtbInt16);
            this.Controls.Add(this.lblInt16);
            this.Controls.Add(this.lblInt);
            this.Controls.Add(this.rtbInt);
            this.Controls.Add(this.btnConvertInt16);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConvertInt16;
        private System.Windows.Forms.RichTextBox rtbInt;
        private System.Windows.Forms.Label lblInt;
        private System.Windows.Forms.Label lblInt16;
        private System.Windows.Forms.RichTextBox rtbInt16;
        private System.Windows.Forms.Button btnConvertInt;
    }
}

