namespace SelfControlForm
{
    partial class WebKitBrowserData
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
            this.tab = new System.Windows.Forms.TabControl();
            this.tabPageHome = new System.Windows.Forms.TabPage();
            this.tabPageLogic = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPickUpCookie = new System.Windows.Forms.Button();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.rtbUrl = new System.Windows.Forms.RichTextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.rtbCookie = new System.Windows.Forms.RichTextBox();
            this.lblCookie = new System.Windows.Forms.Label();
            this.lblTip = new System.Windows.Forms.Label();
            this.rtbTip = new System.Windows.Forms.RichTextBox();
            this.tab.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tabPageHome);
            this.tab.Controls.Add(this.tabPageLogic);
            this.tab.Location = new System.Drawing.Point(15, 48);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(758, 509);
            this.tab.TabIndex = 0;
            // 
            // tabPageHome
            // 
            this.tabPageHome.Location = new System.Drawing.Point(4, 22);
            this.tabPageHome.Name = "tabPageHome";
            this.tabPageHome.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHome.Size = new System.Drawing.Size(750, 483);
            this.tabPageHome.TabIndex = 0;
            this.tabPageHome.Text = "依赖";
            this.tabPageHome.UseVisualStyleBackColor = true;
            // 
            // tabPageLogic
            // 
            this.tabPageLogic.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogic.Name = "tabPageLogic";
            this.tabPageLogic.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLogic.Size = new System.Drawing.Size(1027, 591);
            this.tabPageLogic.TabIndex = 1;
            this.tabPageLogic.Text = "逻辑";
            this.tabPageLogic.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPickUpCookie);
            this.panel1.Controls.Add(this.btnBrowser);
            this.panel1.Controls.Add(this.rtbUrl);
            this.panel1.Controls.Add(this.lblUrl);
            this.panel1.Location = new System.Drawing.Point(16, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(757, 39);
            this.panel1.TabIndex = 1;
            // 
            // btnPickUpCookie
            // 
            this.btnPickUpCookie.Location = new System.Drawing.Point(637, 8);
            this.btnPickUpCookie.Name = "btnPickUpCookie";
            this.btnPickUpCookie.Size = new System.Drawing.Size(75, 23);
            this.btnPickUpCookie.TabIndex = 5;
            this.btnPickUpCookie.Text = "提取cookie";
            this.btnPickUpCookie.UseVisualStyleBackColor = true;
            this.btnPickUpCookie.Click += new System.EventHandler(this.btnPickUpCookie_Click);
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(547, 8);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(75, 23);
            this.btnBrowser.TabIndex = 2;
            this.btnBrowser.Text = "浏览";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // rtbUrl
            // 
            this.rtbUrl.Location = new System.Drawing.Point(42, 7);
            this.rtbUrl.Name = "rtbUrl";
            this.rtbUrl.Size = new System.Drawing.Size(477, 24);
            this.rtbUrl.TabIndex = 3;
            this.rtbUrl.Text = "https://mail.qq.com";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(3, 10);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(23, 12);
            this.lblUrl.TabIndex = 4;
            this.lblUrl.Text = "url";
            // 
            // rtbCookie
            // 
            this.rtbCookie.Location = new System.Drawing.Point(64, 557);
            this.rtbCookie.Name = "rtbCookie";
            this.rtbCookie.Size = new System.Drawing.Size(705, 25);
            this.rtbCookie.TabIndex = 0;
            this.rtbCookie.Text = "";
            // 
            // lblCookie
            // 
            this.lblCookie.AutoSize = true;
            this.lblCookie.Location = new System.Drawing.Point(17, 560);
            this.lblCookie.Name = "lblCookie";
            this.lblCookie.Size = new System.Drawing.Size(41, 12);
            this.lblCookie.TabIndex = 2;
            this.lblCookie.Text = "Cookie";
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.BackColor = System.Drawing.SystemColors.Control;
            this.lblTip.ForeColor = System.Drawing.Color.Red;
            this.lblTip.Location = new System.Drawing.Point(17, 586);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(29, 12);
            this.lblTip.TabIndex = 3;
            this.lblTip.Text = "提示";
            // 
            // rtbTip
            // 
            this.rtbTip.Location = new System.Drawing.Point(64, 588);
            this.rtbTip.Name = "rtbTip";
            this.rtbTip.Size = new System.Drawing.Size(705, 25);
            this.rtbTip.TabIndex = 1;
            this.rtbTip.Text = "";
            // 
            // WebKitBrowserData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtbTip);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.lblCookie);
            this.Controls.Add(this.rtbCookie);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tab);
            this.Name = "WebKitBrowserData";
            this.Size = new System.Drawing.Size(782, 620);
            this.Load += new System.EventHandler(this.WebKitBrowserData_Load);
            this.tab.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tabPageHome;
        private System.Windows.Forms.TabPage tabPageLogic;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.RichTextBox rtbUrl;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Button btnPickUpCookie;
        private System.Windows.Forms.RichTextBox rtbCookie;
        private System.Windows.Forms.Label lblCookie;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.RichTextBox rtbTip;
    }
}
