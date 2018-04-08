namespace SelfControlForm
{
    partial class PickUpIEWebCookieData
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
            this.WebPanel = new System.Windows.Forms.Panel();
            this.web = new System.Windows.Forms.WebBrowser();
            this.lblUrl = new System.Windows.Forms.Label();
            this.rtbUrl = new System.Windows.Forms.RichTextBox();
            this.btnGoto = new System.Windows.Forms.Button();
            this.btnPickUp = new System.Windows.Forms.Button();
            this.lblCookie = new System.Windows.Forms.Label();
            this.rtbCookie = new System.Windows.Forms.RichTextBox();
            this.btnRemoveCookie = new System.Windows.Forms.Button();
            this.btnClearCookie = new System.Windows.Forms.Button();
            this.lblTip = new System.Windows.Forms.Label();
            this.rtbTip = new System.Windows.Forms.RichTextBox();
            this.WebPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // WebPanel
            // 
            this.WebPanel.Controls.Add(this.web);
            this.WebPanel.Location = new System.Drawing.Point(3, 100);
            this.WebPanel.Name = "WebPanel";
            this.WebPanel.Size = new System.Drawing.Size(624, 577);
            this.WebPanel.TabIndex = 0;
            // 
            // web
            // 
            this.web.Location = new System.Drawing.Point(1, 0);
            this.web.MinimumSize = new System.Drawing.Size(20, 20);
            this.web.Name = "web";
            this.web.Size = new System.Drawing.Size(620, 577);
            this.web.TabIndex = 0;
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(4, 4);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(29, 12);
            this.lblUrl.TabIndex = 1;
            this.lblUrl.Text = "网址";
            // 
            // rtbUrl
            // 
            this.rtbUrl.Location = new System.Drawing.Point(51, -2);
            this.rtbUrl.Name = "rtbUrl";
            this.rtbUrl.Size = new System.Drawing.Size(459, 29);
            this.rtbUrl.TabIndex = 2;
            this.rtbUrl.Text = "https://mail.qq.com";
            // 
            // btnGoto
            // 
            this.btnGoto.Location = new System.Drawing.Point(527, 4);
            this.btnGoto.Name = "btnGoto";
            this.btnGoto.Size = new System.Drawing.Size(47, 23);
            this.btnGoto.TabIndex = 3;
            this.btnGoto.Text = "进入";
            this.btnGoto.UseVisualStyleBackColor = true;
            // 
            // btnPickUp
            // 
            this.btnPickUp.Location = new System.Drawing.Point(580, 4);
            this.btnPickUp.Name = "btnPickUp";
            this.btnPickUp.Size = new System.Drawing.Size(47, 23);
            this.btnPickUp.TabIndex = 4;
            this.btnPickUp.Text = "提取";
            this.btnPickUp.UseVisualStyleBackColor = true;
            // 
            // lblCookie
            // 
            this.lblCookie.AutoSize = true;
            this.lblCookie.Location = new System.Drawing.Point(4, 42);
            this.lblCookie.Name = "lblCookie";
            this.lblCookie.Size = new System.Drawing.Size(41, 12);
            this.lblCookie.TabIndex = 5;
            this.lblCookie.Text = "Cookie";
            // 
            // rtbCookie
            // 
            this.rtbCookie.Location = new System.Drawing.Point(51, 33);
            this.rtbCookie.Name = "rtbCookie";
            this.rtbCookie.Size = new System.Drawing.Size(459, 29);
            this.rtbCookie.TabIndex = 6;
            this.rtbCookie.Text = "";
            // 
            // btnRemoveCookie
            // 
            this.btnRemoveCookie.Location = new System.Drawing.Point(527, 38);
            this.btnRemoveCookie.Name = "btnRemoveCookie";
            this.btnRemoveCookie.Size = new System.Drawing.Size(47, 23);
            this.btnRemoveCookie.TabIndex = 7;
            this.btnRemoveCookie.Text = "移除";
            this.btnRemoveCookie.UseVisualStyleBackColor = true;
            // 
            // btnClearCookie
            // 
            this.btnClearCookie.Location = new System.Drawing.Point(580, 38);
            this.btnClearCookie.Name = "btnClearCookie";
            this.btnClearCookie.Size = new System.Drawing.Size(47, 23);
            this.btnClearCookie.TabIndex = 8;
            this.btnClearCookie.Text = "清除";
            this.btnClearCookie.UseVisualStyleBackColor = true;
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(4, 68);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(29, 12);
            this.lblTip.TabIndex = 9;
            this.lblTip.Text = "提示";
            // 
            // rtbTip
            // 
            this.rtbTip.Location = new System.Drawing.Point(51, 65);
            this.rtbTip.Name = "rtbTip";
            this.rtbTip.Size = new System.Drawing.Size(459, 29);
            this.rtbTip.TabIndex = 10;
            this.rtbTip.Text = "";
            // 
            // PickUpIEWebCookieData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtbTip);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.btnClearCookie);
            this.Controls.Add(this.btnRemoveCookie);
            this.Controls.Add(this.rtbCookie);
            this.Controls.Add(this.lblCookie);
            this.Controls.Add(this.btnPickUp);
            this.Controls.Add(this.btnGoto);
            this.Controls.Add(this.rtbUrl);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.WebPanel);
            this.Name = "PickUpIEWebCookieData";
            this.Size = new System.Drawing.Size(639, 694);
            this.WebPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel WebPanel;
        private System.Windows.Forms.WebBrowser web;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.RichTextBox rtbUrl;
        private System.Windows.Forms.Button btnGoto;
        private System.Windows.Forms.Button btnPickUp;
        private System.Windows.Forms.Label lblCookie;
        private System.Windows.Forms.RichTextBox rtbCookie;
        private System.Windows.Forms.Button btnRemoveCookie;
        private System.Windows.Forms.Button btnClearCookie;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.RichTextBox rtbTip;
    }
}
