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
            this.mLayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.mUrlPanel = new System.Windows.Forms.Panel();
            this.mCookiePanel = new System.Windows.Forms.Panel();
            this.mTipPanel = new System.Windows.Forms.Panel();
            this.mBodyPanel = new System.Windows.Forms.Panel();
            this.web = new System.Windows.Forms.WebBrowser();
            this.mLayoutTable.SuspendLayout();
            this.mUrlPanel.SuspendLayout();
            this.mCookiePanel.SuspendLayout();
            this.mTipPanel.SuspendLayout();
            this.mBodyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(7, 12);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(29, 12);
            this.lblUrl.TabIndex = 1;
            this.lblUrl.Text = "网址";
            // 
            // rtbUrl
            // 
            this.rtbUrl.Location = new System.Drawing.Point(53, 5);
            this.rtbUrl.Name = "rtbUrl";
            this.rtbUrl.Size = new System.Drawing.Size(459, 29);
            this.rtbUrl.TabIndex = 2;
            this.rtbUrl.Text = "https://mail.qq.com";
            // 
            // btnGoto
            // 
            this.btnGoto.Location = new System.Drawing.Point(529, 7);
            this.btnGoto.Name = "btnGoto";
            this.btnGoto.Size = new System.Drawing.Size(47, 23);
            this.btnGoto.TabIndex = 3;
            this.btnGoto.Text = "进入";
            this.btnGoto.UseVisualStyleBackColor = true;
            // 
            // btnPickUp
            // 
            this.btnPickUp.Location = new System.Drawing.Point(582, 7);
            this.btnPickUp.Name = "btnPickUp";
            this.btnPickUp.Size = new System.Drawing.Size(47, 23);
            this.btnPickUp.TabIndex = 4;
            this.btnPickUp.Text = "提取";
            this.btnPickUp.UseVisualStyleBackColor = true;
            // 
            // lblCookie
            // 
            this.lblCookie.AutoSize = true;
            this.lblCookie.Location = new System.Drawing.Point(6, 17);
            this.lblCookie.Name = "lblCookie";
            this.lblCookie.Size = new System.Drawing.Size(41, 12);
            this.lblCookie.TabIndex = 5;
            this.lblCookie.Text = "Cookie";
            // 
            // rtbCookie
            // 
            this.rtbCookie.Location = new System.Drawing.Point(53, 4);
            this.rtbCookie.Name = "rtbCookie";
            this.rtbCookie.Size = new System.Drawing.Size(459, 29);
            this.rtbCookie.TabIndex = 6;
            this.rtbCookie.Text = "";
            // 
            // btnRemoveCookie
            // 
            this.btnRemoveCookie.Location = new System.Drawing.Point(529, 8);
            this.btnRemoveCookie.Name = "btnRemoveCookie";
            this.btnRemoveCookie.Size = new System.Drawing.Size(47, 23);
            this.btnRemoveCookie.TabIndex = 7;
            this.btnRemoveCookie.Text = "移除";
            this.btnRemoveCookie.UseVisualStyleBackColor = true;
            // 
            // btnClearCookie
            // 
            this.btnClearCookie.Location = new System.Drawing.Point(582, 8);
            this.btnClearCookie.Name = "btnClearCookie";
            this.btnClearCookie.Size = new System.Drawing.Size(47, 23);
            this.btnClearCookie.TabIndex = 8;
            this.btnClearCookie.Text = "清除";
            this.btnClearCookie.UseVisualStyleBackColor = true;
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(7, 17);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(29, 12);
            this.lblTip.TabIndex = 9;
            this.lblTip.Text = "提示";
            // 
            // rtbTip
            // 
            this.rtbTip.Location = new System.Drawing.Point(53, 4);
            this.rtbTip.Name = "rtbTip";
            this.rtbTip.Size = new System.Drawing.Size(459, 29);
            this.rtbTip.TabIndex = 10;
            this.rtbTip.Text = "";
            // 
            // mLayoutTable
            // 
            this.mLayoutTable.ColumnCount = 1;
            this.mLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mLayoutTable.Controls.Add(this.mBodyPanel, 0, 3);
            this.mLayoutTable.Controls.Add(this.mTipPanel, 0, 2);
            this.mLayoutTable.Controls.Add(this.mCookiePanel, 0, 1);
            this.mLayoutTable.Controls.Add(this.mUrlPanel, 0, 0);
            this.mLayoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mLayoutTable.Location = new System.Drawing.Point(0, 0);
            this.mLayoutTable.Name = "mLayoutTable";
            this.mLayoutTable.RowCount = 4;
            this.mLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.mLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.mLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.mLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mLayoutTable.Size = new System.Drawing.Size(665, 283);
            this.mLayoutTable.TabIndex = 11;
            // 
            // mUrlPanel
            // 
            this.mUrlPanel.Controls.Add(this.lblUrl);
            this.mUrlPanel.Controls.Add(this.rtbUrl);
            this.mUrlPanel.Controls.Add(this.btnPickUp);
            this.mUrlPanel.Controls.Add(this.btnGoto);
            this.mUrlPanel.Location = new System.Drawing.Point(3, 3);
            this.mUrlPanel.Name = "mUrlPanel";
            this.mUrlPanel.Size = new System.Drawing.Size(643, 40);
            this.mUrlPanel.TabIndex = 0;
            this.mUrlPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // mCookiePanel
            // 
            this.mCookiePanel.Controls.Add(this.rtbCookie);
            this.mCookiePanel.Controls.Add(this.lblCookie);
            this.mCookiePanel.Controls.Add(this.btnRemoveCookie);
            this.mCookiePanel.Controls.Add(this.btnClearCookie);
            this.mCookiePanel.Location = new System.Drawing.Point(3, 49);
            this.mCookiePanel.Name = "mCookiePanel";
            this.mCookiePanel.Size = new System.Drawing.Size(643, 37);
            this.mCookiePanel.TabIndex = 1;
            // 
            // mTipPanel
            // 
            this.mTipPanel.Controls.Add(this.lblTip);
            this.mTipPanel.Controls.Add(this.rtbTip);
            this.mTipPanel.Location = new System.Drawing.Point(3, 92);
            this.mTipPanel.Name = "mTipPanel";
            this.mTipPanel.Size = new System.Drawing.Size(629, 35);
            this.mTipPanel.TabIndex = 2;
            // 
            // mBodyPanel
            // 
            this.mBodyPanel.Controls.Add(this.web);
            this.mBodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mBodyPanel.Location = new System.Drawing.Point(3, 133);
            this.mBodyPanel.Name = "mBodyPanel";
            this.mBodyPanel.Size = new System.Drawing.Size(659, 147);
            this.mBodyPanel.TabIndex = 3;
            // 
            // web
            // 
            this.web.Location = new System.Drawing.Point(3, 3);
            this.web.MinimumSize = new System.Drawing.Size(20, 20);
            this.web.Name = "web";
            this.web.Size = new System.Drawing.Size(653, 112);
            this.web.TabIndex = 0;
            // 
            // PickUpIEWebCookieData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mLayoutTable);
            this.Name = "PickUpIEWebCookieData";
            this.Size = new System.Drawing.Size(665, 283);
            this.mLayoutTable.ResumeLayout(false);
            this.mUrlPanel.ResumeLayout(false);
            this.mUrlPanel.PerformLayout();
            this.mCookiePanel.ResumeLayout(false);
            this.mCookiePanel.PerformLayout();
            this.mTipPanel.ResumeLayout(false);
            this.mTipPanel.PerformLayout();
            this.mBodyPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
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
        private System.Windows.Forms.TableLayoutPanel mLayoutTable;
        private System.Windows.Forms.Panel mBodyPanel;
        private System.Windows.Forms.WebBrowser web;
        private System.Windows.Forms.Panel mTipPanel;
        private System.Windows.Forms.Panel mCookiePanel;
        private System.Windows.Forms.Panel mUrlPanel;
    }
}
