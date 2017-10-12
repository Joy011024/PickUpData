namespace FeatureFrmList
{
    partial class MicrosoftBrowser
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
            this.headPanel = new System.Windows.Forms.Panel();
            this.rtbCookie = new System.Windows.Forms.RichTextBox();
            this.lblCookies = new System.Windows.Forms.Label();
            this.btnGetCookies = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.rtbUrl = new System.Windows.Forms.RichTextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.bodyPanel = new System.Windows.Forms.Panel();
            this.MicrosoftWebBrowser = new System.Windows.Forms.WebBrowser();
            this.headPanel.SuspendLayout();
            this.bodyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headPanel
            // 
            this.headPanel.Controls.Add(this.rtbCookie);
            this.headPanel.Controls.Add(this.lblCookies);
            this.headPanel.Controls.Add(this.btnGetCookies);
            this.headPanel.Controls.Add(this.btnGo);
            this.headPanel.Controls.Add(this.rtbUrl);
            this.headPanel.Controls.Add(this.lblUrl);
            this.headPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headPanel.Location = new System.Drawing.Point(0, 0);
            this.headPanel.Name = "headPanel";
            this.headPanel.Size = new System.Drawing.Size(476, 70);
            this.headPanel.TabIndex = 0;
            // 
            // rtbCookie
            // 
            this.rtbCookie.Location = new System.Drawing.Point(56, 34);
            this.rtbCookie.Name = "rtbCookie";
            this.rtbCookie.Size = new System.Drawing.Size(337, 27);
            this.rtbCookie.TabIndex = 5;
            this.rtbCookie.Text = "";
            // 
            // lblCookies
            // 
            this.lblCookies.AutoSize = true;
            this.lblCookies.Location = new System.Drawing.Point(3, 43);
            this.lblCookies.Name = "lblCookies";
            this.lblCookies.Size = new System.Drawing.Size(47, 12);
            this.lblCookies.TabIndex = 4;
            this.lblCookies.Text = "Cookies";
            // 
            // btnGetCookies
            // 
            this.btnGetCookies.Location = new System.Drawing.Point(399, 35);
            this.btnGetCookies.Name = "btnGetCookies";
            this.btnGetCookies.Size = new System.Drawing.Size(64, 23);
            this.btnGetCookies.TabIndex = 3;
            this.btnGetCookies.Text = "Get";
            this.btnGetCookies.UseVisualStyleBackColor = true;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(399, 7);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(64, 23);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            // 
            // rtbUrl
            // 
            this.rtbUrl.Location = new System.Drawing.Point(56, 3);
            this.rtbUrl.Name = "rtbUrl";
            this.rtbUrl.Size = new System.Drawing.Size(337, 27);
            this.rtbUrl.TabIndex = 1;
            this.rtbUrl.Text = "";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(3, 10);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(23, 12);
            this.lblUrl.TabIndex = 0;
            this.lblUrl.Text = "Url";
            // 
            // bodyPanel
            // 
            this.bodyPanel.Controls.Add(this.MicrosoftWebBrowser);
            this.bodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyPanel.Location = new System.Drawing.Point(0, 70);
            this.bodyPanel.Name = "bodyPanel";
            this.bodyPanel.Size = new System.Drawing.Size(476, 318);
            this.bodyPanel.TabIndex = 1;
            // 
            // MicrosoftWebBrowser
            // 
            this.MicrosoftWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MicrosoftWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.MicrosoftWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.MicrosoftWebBrowser.Name = "MicrosoftWebBrowser";
            this.MicrosoftWebBrowser.Size = new System.Drawing.Size(476, 318);
            this.MicrosoftWebBrowser.TabIndex = 0;
            // 
            // MicrosoftBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bodyPanel);
            this.Controls.Add(this.headPanel);
            this.Name = "MicrosoftBrowser";
            this.Size = new System.Drawing.Size(476, 388);
            this.headPanel.ResumeLayout(false);
            this.headPanel.PerformLayout();
            this.bodyPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headPanel;
        private System.Windows.Forms.Panel bodyPanel;
        private System.Windows.Forms.RichTextBox rtbUrl;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.RichTextBox rtbCookie;
        private System.Windows.Forms.Label lblCookies;
        private System.Windows.Forms.Button btnGetCookies;
        private System.Windows.Forms.WebBrowser MicrosoftWebBrowser;
    }
}
