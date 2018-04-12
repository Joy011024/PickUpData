namespace CaptureWebData
{
    partial class WebDataFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRefreshWeb = new System.Windows.Forms.Button();
            this.PickUpCookie = new System.Windows.Forms.Button();
            this.btnForceSave = new System.Windows.Forms.Button();
            this.rtbTip = new System.Windows.Forms.RichTextBox();
            this.btnClearText = new System.Windows.Forms.Button();
            this.txtEleValue = new System.Windows.Forms.TextBox();
            this.lblHtmlEleValue = new System.Windows.Forms.Label();
            this.btnSetHtmlEle = new System.Windows.Forms.Button();
            this.txtHtmlEle = new System.Windows.Forms.TextBox();
            this.lblHtmlEle = new System.Windows.Forms.Label();
            this.btnPickUp = new System.Windows.Forms.Button();
            this.lblUrl = new System.Windows.Forms.Label();
            this.btnLoading = new System.Windows.Forms.Button();
            this.rtbUrl = new System.Windows.Forms.RichTextBox();
            this.spContainer = new System.Windows.Forms.SplitContainer();
            this.rtbHtmlData = new System.Windows.Forms.RichTextBox();
            this.rtbHtml = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spContainer)).BeginInit();
            this.spContainer.Panel1.SuspendLayout();
            this.spContainer.Panel2.SuspendLayout();
            this.spContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(799, 543);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Url = new System.Uri("https://www.qqzeng.com/ip/", System.UriKind.Absolute);
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRefreshWeb);
            this.panel1.Controls.Add(this.PickUpCookie);
            this.panel1.Controls.Add(this.btnForceSave);
            this.panel1.Controls.Add(this.rtbTip);
            this.panel1.Controls.Add(this.btnClearText);
            this.panel1.Controls.Add(this.txtEleValue);
            this.panel1.Controls.Add(this.lblHtmlEleValue);
            this.panel1.Controls.Add(this.btnSetHtmlEle);
            this.panel1.Controls.Add(this.txtHtmlEle);
            this.panel1.Controls.Add(this.lblHtmlEle);
            this.panel1.Controls.Add(this.btnPickUp);
            this.panel1.Controls.Add(this.lblUrl);
            this.panel1.Controls.Add(this.btnLoading);
            this.panel1.Controls.Add(this.rtbUrl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1204, 103);
            this.panel1.TabIndex = 1;
            // 
            // btnRefreshWeb
            // 
            this.btnRefreshWeb.Location = new System.Drawing.Point(587, 27);
            this.btnRefreshWeb.Name = "btnRefreshWeb";
            this.btnRefreshWeb.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshWeb.TabIndex = 15;
            this.btnRefreshWeb.Text = "刷新网页";
            this.btnRefreshWeb.UseVisualStyleBackColor = true;
            this.btnRefreshWeb.Click += new System.EventHandler(this.btnRefreshWeb_Click);
            // 
            // PickUpCookie
            // 
            this.PickUpCookie.Location = new System.Drawing.Point(324, 60);
            this.PickUpCookie.Name = "PickUpCookie";
            this.PickUpCookie.Size = new System.Drawing.Size(75, 23);
            this.PickUpCookie.TabIndex = 14;
            this.PickUpCookie.Text = "提取Cookie";
            this.PickUpCookie.UseVisualStyleBackColor = true;
            this.PickUpCookie.Click += new System.EventHandler(this.btnPickUpCookie_Click);
            // 
            // btnForceSave
            // 
            this.btnForceSave.Location = new System.Drawing.Point(705, 60);
            this.btnForceSave.Name = "btnForceSave";
            this.btnForceSave.Size = new System.Drawing.Size(75, 23);
            this.btnForceSave.TabIndex = 13;
            this.btnForceSave.Text = "强制保存";
            this.btnForceSave.UseVisualStyleBackColor = true;
            this.btnForceSave.Click += new System.EventHandler(this.btnForceSave_Click);
            // 
            // rtbTip
            // 
            this.rtbTip.Location = new System.Drawing.Point(925, 8);
            this.rtbTip.Name = "rtbTip";
            this.rtbTip.Size = new System.Drawing.Size(267, 75);
            this.rtbTip.TabIndex = 12;
            this.rtbTip.Text = "";
            // 
            // btnClearText
            // 
            this.btnClearText.Location = new System.Drawing.Point(145, 60);
            this.btnClearText.Name = "btnClearText";
            this.btnClearText.Size = new System.Drawing.Size(75, 23);
            this.btnClearText.TabIndex = 11;
            this.btnClearText.Text = "清除文本";
            this.btnClearText.UseVisualStyleBackColor = true;
            this.btnClearText.Click += new System.EventHandler(this.btnClearText_Click);
            // 
            // txtEleValue
            // 
            this.txtEleValue.Location = new System.Drawing.Point(762, 33);
            this.txtEleValue.Name = "txtEleValue";
            this.txtEleValue.Size = new System.Drawing.Size(145, 21);
            this.txtEleValue.TabIndex = 10;
            this.txtEleValue.Text = " 59.45.31.82";
            // 
            // lblHtmlEleValue
            // 
            this.lblHtmlEleValue.AutoSize = true;
            this.lblHtmlEleValue.Location = new System.Drawing.Point(703, 38);
            this.lblHtmlEleValue.Name = "lblHtmlEleValue";
            this.lblHtmlEleValue.Size = new System.Drawing.Size(53, 12);
            this.lblHtmlEleValue.TabIndex = 9;
            this.lblHtmlEleValue.Text = "元素赋值";
            // 
            // btnSetHtmlEle
            // 
            this.btnSetHtmlEle.Location = new System.Drawing.Point(817, 60);
            this.btnSetHtmlEle.Name = "btnSetHtmlEle";
            this.btnSetHtmlEle.Size = new System.Drawing.Size(75, 23);
            this.btnSetHtmlEle.TabIndex = 8;
            this.btnSetHtmlEle.Text = "赋值";
            this.btnSetHtmlEle.UseVisualStyleBackColor = true;
            this.btnSetHtmlEle.Click += new System.EventHandler(this.btnSetHtmlEle_Click);
            // 
            // txtHtmlEle
            // 
            this.txtHtmlEle.Location = new System.Drawing.Point(762, 8);
            this.txtHtmlEle.Name = "txtHtmlEle";
            this.txtHtmlEle.Size = new System.Drawing.Size(145, 21);
            this.txtHtmlEle.TabIndex = 7;
            this.txtHtmlEle.Text = "ipInfo";
            // 
            // lblHtmlEle
            // 
            this.lblHtmlEle.AutoSize = true;
            this.lblHtmlEle.Location = new System.Drawing.Point(703, 11);
            this.lblHtmlEle.Name = "lblHtmlEle";
            this.lblHtmlEle.Size = new System.Drawing.Size(53, 12);
            this.lblHtmlEle.TabIndex = 6;
            this.lblHtmlEle.Text = "页面元素";
            // 
            // btnPickUp
            // 
            this.btnPickUp.Location = new System.Drawing.Point(50, 60);
            this.btnPickUp.Name = "btnPickUp";
            this.btnPickUp.Size = new System.Drawing.Size(75, 23);
            this.btnPickUp.TabIndex = 5;
            this.btnPickUp.Text = "提取Html";
            this.btnPickUp.UseVisualStyleBackColor = true;
            this.btnPickUp.Click += new System.EventHandler(this.btnPickUp_Click);
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(3, 22);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(29, 12);
            this.lblUrl.TabIndex = 4;
            this.lblUrl.Text = "网址";
            // 
            // btnLoading
            // 
            this.btnLoading.Location = new System.Drawing.Point(506, 60);
            this.btnLoading.Name = "btnLoading";
            this.btnLoading.Size = new System.Drawing.Size(75, 23);
            this.btnLoading.TabIndex = 2;
            this.btnLoading.Text = "加载Ip列表";
            this.btnLoading.UseVisualStyleBackColor = true;
            this.btnLoading.Click += new System.EventHandler(this.btnLoading_Click);
            // 
            // rtbUrl
            // 
            this.rtbUrl.Location = new System.Drawing.Point(50, 19);
            this.rtbUrl.Name = "rtbUrl";
            this.rtbUrl.Size = new System.Drawing.Size(531, 35);
            this.rtbUrl.TabIndex = 3;
            this.rtbUrl.Text = "https://www.qqzeng.com/ip/";
            // 
            // spContainer
            // 
            this.spContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spContainer.Location = new System.Drawing.Point(0, 103);
            this.spContainer.Name = "spContainer";
            // 
            // spContainer.Panel1
            // 
            this.spContainer.Panel1.Controls.Add(this.rtbHtmlData);
            this.spContainer.Panel1.Controls.Add(this.rtbHtml);
            // 
            // spContainer.Panel2
            // 
            this.spContainer.Panel2.Controls.Add(this.webBrowser);
            this.spContainer.Size = new System.Drawing.Size(1204, 568);
            this.spContainer.SplitterDistance = 401;
            this.spContainer.TabIndex = 6;
            // 
            // rtbHtmlData
            // 
            this.rtbHtmlData.Location = new System.Drawing.Point(0, 3);
            this.rtbHtmlData.Name = "rtbHtmlData";
            this.rtbHtmlData.Size = new System.Drawing.Size(366, 331);
            this.rtbHtmlData.TabIndex = 1;
            this.rtbHtmlData.Text = "";
            // 
            // rtbHtml
            // 
            this.rtbHtml.Location = new System.Drawing.Point(5, 340);
            this.rtbHtml.Name = "rtbHtml";
            this.rtbHtml.Size = new System.Drawing.Size(361, 254);
            this.rtbHtml.TabIndex = 0;
            this.rtbHtml.Text = "";
            // 
            // WebDataFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1204, 671);
            this.Controls.Add(this.spContainer);
            this.Controls.Add(this.panel1);
            this.Name = "WebDataFrm";
            this.Text = "WebDataFrm";
            this.Load += new System.EventHandler(this.WebDataFrm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.spContainer.Panel1.ResumeLayout(false);
            this.spContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spContainer)).EndInit();
            this.spContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.RichTextBox rtbUrl;
        private System.Windows.Forms.Button btnPickUp;
        private System.Windows.Forms.Button btnLoading;
        private System.Windows.Forms.SplitContainer spContainer;
        private System.Windows.Forms.RichTextBox rtbHtml;
        private System.Windows.Forms.RichTextBox rtbHtmlData;
        private System.Windows.Forms.TextBox txtEleValue;
        private System.Windows.Forms.Label lblHtmlEleValue;
        private System.Windows.Forms.Button btnSetHtmlEle;
        private System.Windows.Forms.TextBox txtHtmlEle;
        private System.Windows.Forms.Label lblHtmlEle;
        private System.Windows.Forms.Button btnClearText;
        private System.Windows.Forms.RichTextBox rtbTip;
        private System.Windows.Forms.Button btnForceSave;
        private System.Windows.Forms.Button PickUpCookie;
        private System.Windows.Forms.Button btnRefreshWeb;
    }
}