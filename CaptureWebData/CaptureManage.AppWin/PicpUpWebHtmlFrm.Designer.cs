namespace CaptureManage.AppWin
{
    partial class PicpUpWebHtmlFrm
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
            this.htmlPanel = new System.Windows.Forms.Panel();
            this.LogicPanel = new System.Windows.Forms.Panel();
            this.rtbHtml = new System.Windows.Forms.RichTextBox();
            this.txtKeywork = new System.Windows.Forms.TextBox();
            this.lblKeyword = new System.Windows.Forms.Label();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.lblPage = new System.Windows.Forms.Label();
            this.lstData = new System.Windows.Forms.ListBox();
            this.btnClearText = new System.Windows.Forms.Button();
            this.txtSelectUrl = new System.Windows.Forms.TextBox();
            this.LogicPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // htmlPanel
            // 
            this.htmlPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.htmlPanel.Location = new System.Drawing.Point(0, 0);
            this.htmlPanel.Name = "htmlPanel";
            this.htmlPanel.Size = new System.Drawing.Size(696, 579);
            this.htmlPanel.TabIndex = 0;
            // 
            // LogicPanel
            // 
            this.LogicPanel.Controls.Add(this.txtSelectUrl);
            this.LogicPanel.Controls.Add(this.btnClearText);
            this.LogicPanel.Controls.Add(this.lstData);
            this.LogicPanel.Controls.Add(this.rtbHtml);
            this.LogicPanel.Controls.Add(this.txtKeywork);
            this.LogicPanel.Controls.Add(this.lblKeyword);
            this.LogicPanel.Controls.Add(this.txtPage);
            this.LogicPanel.Controls.Add(this.lblPage);
            this.LogicPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.LogicPanel.Location = new System.Drawing.Point(702, 0);
            this.LogicPanel.Name = "LogicPanel";
            this.LogicPanel.Size = new System.Drawing.Size(288, 579);
            this.LogicPanel.TabIndex = 1;
            // 
            // rtbHtml
            // 
            this.rtbHtml.Location = new System.Drawing.Point(6, 80);
            this.rtbHtml.Name = "rtbHtml";
            this.rtbHtml.Size = new System.Drawing.Size(279, 224);
            this.rtbHtml.TabIndex = 0;
            this.rtbHtml.Text = "";
            // 
            // txtKeywork
            // 
            this.txtKeywork.Location = new System.Drawing.Point(51, 48);
            this.txtKeywork.Name = "txtKeywork";
            this.txtKeywork.Size = new System.Drawing.Size(100, 21);
            this.txtKeywork.TabIndex = 3;
            // 
            // lblKeyword
            // 
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Location = new System.Drawing.Point(4, 51);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(41, 12);
            this.lblKeyword.TabIndex = 2;
            this.lblKeyword.Text = "关键字";
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(51, 10);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(100, 21);
            this.txtPage.TabIndex = 1;
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Location = new System.Drawing.Point(4, 13);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(29, 12);
            this.lblPage.TabIndex = 0;
            this.lblPage.Text = "页码";
            // 
            // lstData
            // 
            this.lstData.FormattingEnabled = true;
            this.lstData.ItemHeight = 12;
            this.lstData.Location = new System.Drawing.Point(6, 341);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(270, 148);
            this.lstData.TabIndex = 4;
            this.lstData.SelectedIndexChanged += new System.EventHandler(this.lstData_SelectedIndexChanged);
            // 
            // btnClearText
            // 
            this.btnClearText.Location = new System.Drawing.Point(6, 310);
            this.btnClearText.Name = "btnClearText";
            this.btnClearText.Size = new System.Drawing.Size(75, 23);
            this.btnClearText.TabIndex = 5;
            this.btnClearText.Text = "清除文本";
            this.btnClearText.UseVisualStyleBackColor = true;
            this.btnClearText.Click += new System.EventHandler(this.btnClearText_Click);
            // 
            // txtSelectUrl
            // 
            this.txtSelectUrl.Location = new System.Drawing.Point(6, 507);
            this.txtSelectUrl.Multiline = true;
            this.txtSelectUrl.Name = "txtSelectUrl";
            this.txtSelectUrl.Size = new System.Drawing.Size(270, 60);
            this.txtSelectUrl.TabIndex = 6;
            // 
            // PicpUpWebHtmlFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 579);
            this.Controls.Add(this.LogicPanel);
            this.Controls.Add(this.htmlPanel);
            this.Name = "PicpUpWebHtmlFrm";
            this.Text = "PicpUpWebHtmlFrm";
            this.Load += new System.EventHandler(this.PicpUpWebHtmlFrm_Load);
            this.LogicPanel.ResumeLayout(false);
            this.LogicPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel htmlPanel;
        private System.Windows.Forms.Panel LogicPanel;
        private System.Windows.Forms.RichTextBox rtbHtml;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.TextBox txtKeywork;
        private System.Windows.Forms.Label lblKeyword;
        private System.Windows.Forms.ListBox lstData;
        private System.Windows.Forms.Button btnClearText;
        private System.Windows.Forms.TextBox txtSelectUrl;
    }
}