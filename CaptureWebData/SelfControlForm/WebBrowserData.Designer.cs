namespace SelfControlForm
{
    partial class WebBrowserData
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
            this.Container = new System.Windows.Forms.SplitContainer();
            this.btnCallBack = new System.Windows.Forms.Button();
            this.rtbWebKitCookie = new System.Windows.Forms.RichTextBox();
            this.lblWebKitCookie = new System.Windows.Forms.Label();
            this.rtbIeCookie = new System.Windows.Forms.RichTextBox();
            this.lblIeCookie = new System.Windows.Forms.Label();
            this.rtbElementSign = new System.Windows.Forms.RichTextBox();
            this.btnPickUp_UseCookie = new System.Windows.Forms.Button();
            this.lblElementSign = new System.Windows.Forms.Label();
            this.btnPickUpCookie = new System.Windows.Forms.Button();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.rtbTip = new System.Windows.Forms.RichTextBox();
            this.lblTip = new System.Windows.Forms.Label();
            this.rtbUrl = new System.Windows.Forms.RichTextBox();
            this.rtbBodyHtml = new System.Windows.Forms.RichTextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.lblBodyHtml = new System.Windows.Forms.Label();
            this.lblElementHtml = new System.Windows.Forms.Label();
            this.rtbElementHtml = new System.Windows.Forms.RichTextBox();
            this.tabBrowser = new System.Windows.Forms.TabControl();
            this.tabIE = new System.Windows.Forms.TabPage();
            this.webBrowserIE = new System.Windows.Forms.WebBrowser();
            this.tabWebKit = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.Container)).BeginInit();
            this.Container.Panel1.SuspendLayout();
            this.Container.Panel2.SuspendLayout();
            this.Container.SuspendLayout();
            this.tabBrowser.SuspendLayout();
            this.tabIE.SuspendLayout();
            this.SuspendLayout();
            // 
            // Container
            // 
            this.Container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Container.Location = new System.Drawing.Point(0, 0);
            this.Container.Name = "Container";
            // 
            // Container.Panel1
            // 
            this.Container.Panel1.Controls.Add(this.btnCallBack);
            this.Container.Panel1.Controls.Add(this.rtbWebKitCookie);
            this.Container.Panel1.Controls.Add(this.lblWebKitCookie);
            this.Container.Panel1.Controls.Add(this.rtbIeCookie);
            this.Container.Panel1.Controls.Add(this.lblIeCookie);
            this.Container.Panel1.Controls.Add(this.rtbElementSign);
            this.Container.Panel1.Controls.Add(this.btnPickUp_UseCookie);
            this.Container.Panel1.Controls.Add(this.lblElementSign);
            this.Container.Panel1.Controls.Add(this.btnPickUpCookie);
            this.Container.Panel1.Controls.Add(this.btnBrowser);
            this.Container.Panel1.Controls.Add(this.rtbTip);
            this.Container.Panel1.Controls.Add(this.lblTip);
            this.Container.Panel1.Controls.Add(this.rtbUrl);
            this.Container.Panel1.Controls.Add(this.rtbBodyHtml);
            this.Container.Panel1.Controls.Add(this.lblUrl);
            this.Container.Panel1.Controls.Add(this.lblBodyHtml);
            this.Container.Panel1.Controls.Add(this.lblElementHtml);
            this.Container.Panel1.Controls.Add(this.rtbElementHtml);
            // 
            // Container.Panel2
            // 
            this.Container.Panel2.Controls.Add(this.tabBrowser);
            this.Container.Size = new System.Drawing.Size(1014, 682);
            this.Container.SplitterDistance = 338;
            this.Container.TabIndex = 1;
            // 
            // btnCallBack
            // 
            this.btnCallBack.Location = new System.Drawing.Point(255, 38);
            this.btnCallBack.Name = "btnCallBack";
            this.btnCallBack.Size = new System.Drawing.Size(75, 23);
            this.btnCallBack.TabIndex = 15;
            this.btnCallBack.Text = "传输cookie";
            this.btnCallBack.UseVisualStyleBackColor = true;
            this.btnCallBack.Click += new System.EventHandler(this.btnCallBack_Click);
            // 
            // rtbWebKitCookie
            // 
            this.rtbWebKitCookie.Location = new System.Drawing.Point(78, 120);
            this.rtbWebKitCookie.Name = "rtbWebKitCookie";
            this.rtbWebKitCookie.Size = new System.Drawing.Size(252, 25);
            this.rtbWebKitCookie.TabIndex = 14;
            this.rtbWebKitCookie.Text = "";
            // 
            // lblWebKitCookie
            // 
            this.lblWebKitCookie.AutoSize = true;
            this.lblWebKitCookie.Location = new System.Drawing.Point(13, 128);
            this.lblWebKitCookie.Name = "lblWebKitCookie";
            this.lblWebKitCookie.Size = new System.Drawing.Size(65, 12);
            this.lblWebKitCookie.TabIndex = 13;
            this.lblWebKitCookie.Tag = "";
            this.lblWebKitCookie.Text = "谷歌Cookie";
            // 
            // rtbIeCookie
            // 
            this.rtbIeCookie.Location = new System.Drawing.Point(78, 89);
            this.rtbIeCookie.Name = "rtbIeCookie";
            this.rtbIeCookie.Size = new System.Drawing.Size(252, 25);
            this.rtbIeCookie.TabIndex = 12;
            this.rtbIeCookie.Text = "";
            // 
            // lblIeCookie
            // 
            this.lblIeCookie.AutoSize = true;
            this.lblIeCookie.Location = new System.Drawing.Point(13, 97);
            this.lblIeCookie.Name = "lblIeCookie";
            this.lblIeCookie.Size = new System.Drawing.Size(59, 12);
            this.lblIeCookie.TabIndex = 11;
            this.lblIeCookie.Text = "Ie Cookie";
            // 
            // rtbElementSign
            // 
            this.rtbElementSign.Location = new System.Drawing.Point(159, 147);
            this.rtbElementSign.Name = "rtbElementSign";
            this.rtbElementSign.Size = new System.Drawing.Size(171, 25);
            this.rtbElementSign.TabIndex = 8;
            this.rtbElementSign.Text = "";
            // 
            // btnPickUp_UseCookie
            // 
            this.btnPickUp_UseCookie.Location = new System.Drawing.Point(132, 302);
            this.btnPickUp_UseCookie.Name = "btnPickUp_UseCookie";
            this.btnPickUp_UseCookie.Size = new System.Drawing.Size(90, 23);
            this.btnPickUp_UseCookie.TabIndex = 10;
            this.btnPickUp_UseCookie.Text = "应用IEcookie";
            this.btnPickUp_UseCookie.UseVisualStyleBackColor = true;
            this.btnPickUp_UseCookie.Click += new System.EventHandler(this.btnPickUp_UseCookie_Click);
            // 
            // lblElementSign
            // 
            this.lblElementSign.AutoSize = true;
            this.lblElementSign.Location = new System.Drawing.Point(94, 150);
            this.lblElementSign.Name = "lblElementSign";
            this.lblElementSign.Size = new System.Drawing.Size(53, 12);
            this.lblElementSign.TabIndex = 7;
            this.lblElementSign.Text = "元素标识";
            // 
            // btnPickUpCookie
            // 
            this.btnPickUpCookie.Location = new System.Drawing.Point(132, 38);
            this.btnPickUpCookie.Name = "btnPickUpCookie";
            this.btnPickUpCookie.Size = new System.Drawing.Size(75, 23);
            this.btnPickUpCookie.TabIndex = 9;
            this.btnPickUpCookie.Text = "查看Cookie";
            this.btnPickUpCookie.UseVisualStyleBackColor = true;
            this.btnPickUpCookie.Click += new System.EventHandler(this.btnPickUpCookie_Click);
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(15, 38);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(75, 23);
            this.btnBrowser.TabIndex = 0;
            this.btnBrowser.Text = "浏览";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // rtbTip
            // 
            this.rtbTip.Location = new System.Drawing.Point(15, 576);
            this.rtbTip.Name = "rtbTip";
            this.rtbTip.Size = new System.Drawing.Size(318, 102);
            this.rtbTip.TabIndex = 6;
            this.rtbTip.Text = "";
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(13, 561);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(29, 12);
            this.lblTip.TabIndex = 5;
            this.lblTip.Text = "提示";
            // 
            // rtbUrl
            // 
            this.rtbUrl.Location = new System.Drawing.Point(73, 8);
            this.rtbUrl.Name = "rtbUrl";
            this.rtbUrl.Size = new System.Drawing.Size(257, 24);
            this.rtbUrl.TabIndex = 1;
            this.rtbUrl.Text = "https://mail.qq.com";
            // 
            // rtbBodyHtml
            // 
            this.rtbBodyHtml.Location = new System.Drawing.Point(15, 331);
            this.rtbBodyHtml.Name = "rtbBodyHtml";
            this.rtbBodyHtml.Size = new System.Drawing.Size(315, 227);
            this.rtbBodyHtml.TabIndex = 4;
            this.rtbBodyHtml.Text = "";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(13, 14);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(23, 12);
            this.lblUrl.TabIndex = 1;
            this.lblUrl.Text = "url";
            // 
            // lblBodyHtml
            // 
            this.lblBodyHtml.AutoSize = true;
            this.lblBodyHtml.Location = new System.Drawing.Point(13, 307);
            this.lblBodyHtml.Name = "lblBodyHtml";
            this.lblBodyHtml.Size = new System.Drawing.Size(53, 12);
            this.lblBodyHtml.TabIndex = 3;
            this.lblBodyHtml.Text = "页面html";
            // 
            // lblElementHtml
            // 
            this.lblElementHtml.AutoSize = true;
            this.lblElementHtml.Location = new System.Drawing.Point(13, 160);
            this.lblElementHtml.Name = "lblElementHtml";
            this.lblElementHtml.Size = new System.Drawing.Size(53, 12);
            this.lblElementHtml.TabIndex = 2;
            this.lblElementHtml.Text = "元素html";
            // 
            // rtbElementHtml
            // 
            this.rtbElementHtml.Location = new System.Drawing.Point(15, 178);
            this.rtbElementHtml.Name = "rtbElementHtml";
            this.rtbElementHtml.Size = new System.Drawing.Size(315, 121);
            this.rtbElementHtml.TabIndex = 0;
            this.rtbElementHtml.Text = "";
            // 
            // tabBrowser
            // 
            this.tabBrowser.Controls.Add(this.tabIE);
            this.tabBrowser.Controls.Add(this.tabWebKit);
            this.tabBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabBrowser.Location = new System.Drawing.Point(0, 0);
            this.tabBrowser.Name = "tabBrowser";
            this.tabBrowser.SelectedIndex = 0;
            this.tabBrowser.Size = new System.Drawing.Size(672, 682);
            this.tabBrowser.TabIndex = 11;
            this.tabBrowser.Tag = "WebKit";
            // 
            // tabIE
            // 
            this.tabIE.Controls.Add(this.webBrowserIE);
            this.tabIE.Location = new System.Drawing.Point(4, 22);
            this.tabIE.Name = "tabIE";
            this.tabIE.Padding = new System.Windows.Forms.Padding(3);
            this.tabIE.Size = new System.Drawing.Size(664, 656);
            this.tabIE.TabIndex = 0;
            this.tabIE.Tag = "IE";
            this.tabIE.Text = "IE内核";
            this.tabIE.UseVisualStyleBackColor = true;
            // 
            // webBrowserIE
            // 
            this.webBrowserIE.Location = new System.Drawing.Point(7, 6);
            this.webBrowserIE.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserIE.Name = "webBrowserIE";
            this.webBrowserIE.Size = new System.Drawing.Size(642, 644);
            this.webBrowserIE.TabIndex = 0;
            // 
            // tabWebKit
            // 
            this.tabWebKit.Location = new System.Drawing.Point(4, 22);
            this.tabWebKit.Name = "tabWebKit";
            this.tabWebKit.Padding = new System.Windows.Forms.Padding(3);
            this.tabWebKit.Size = new System.Drawing.Size(664, 656);
            this.tabWebKit.TabIndex = 1;
            this.tabWebKit.Text = "谷歌内核";
            this.tabWebKit.UseVisualStyleBackColor = true;
            // 
            // WebBrowserData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Container);
            this.Name = "WebBrowserData";
            this.Size = new System.Drawing.Size(1014, 682);
            this.Load += new System.EventHandler(this.WebBrowserData_Load);
            this.Container.Panel1.ResumeLayout(false);
            this.Container.Panel1.PerformLayout();
            this.Container.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Container)).EndInit();
            this.Container.ResumeLayout(false);
            this.tabBrowser.ResumeLayout(false);
            this.tabIE.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer Container;
        private System.Windows.Forms.RichTextBox rtbElementSign;
        private System.Windows.Forms.Label lblElementSign;
        private System.Windows.Forms.RichTextBox rtbTip;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.RichTextBox rtbBodyHtml;
        private System.Windows.Forms.Label lblBodyHtml;
        private System.Windows.Forms.Label lblElementHtml;
        private System.Windows.Forms.RichTextBox rtbElementHtml;
        private System.Windows.Forms.WebBrowser webBrowserIE;
        private System.Windows.Forms.Button btnPickUp_UseCookie;
        private System.Windows.Forms.Button btnPickUpCookie;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.RichTextBox rtbUrl;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TabControl tabBrowser;
        private System.Windows.Forms.TabPage tabIE;
        private System.Windows.Forms.TabPage tabWebKit;
        private System.Windows.Forms.RichTextBox rtbWebKitCookie;
        private System.Windows.Forms.Label lblWebKitCookie;
        private System.Windows.Forms.RichTextBox rtbIeCookie;
        private System.Windows.Forms.Label lblIeCookie;
        private System.Windows.Forms.Button btnCallBack;
    }
}
