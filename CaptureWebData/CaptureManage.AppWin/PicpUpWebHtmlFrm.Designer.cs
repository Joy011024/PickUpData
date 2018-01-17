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
            this.lblPage = new System.Windows.Forms.Label();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.txtKeywork = new System.Windows.Forms.TextBox();
            this.lblKeyword = new System.Windows.Forms.Label();
            this.LogicPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // htmlPanel
            // 
            this.htmlPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.htmlPanel.Location = new System.Drawing.Point(0, 0);
            this.htmlPanel.Name = "htmlPanel";
            this.htmlPanel.Size = new System.Drawing.Size(696, 493);
            this.htmlPanel.TabIndex = 0;
            // 
            // LogicPanel
            // 
            this.LogicPanel.Controls.Add(this.rtbHtml);
            this.LogicPanel.Controls.Add(this.txtKeywork);
            this.LogicPanel.Controls.Add(this.lblKeyword);
            this.LogicPanel.Controls.Add(this.txtPage);
            this.LogicPanel.Controls.Add(this.lblPage);
            this.LogicPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.LogicPanel.Location = new System.Drawing.Point(702, 0);
            this.LogicPanel.Name = "LogicPanel";
            this.LogicPanel.Size = new System.Drawing.Size(288, 493);
            this.LogicPanel.TabIndex = 1;
            // 
            // rtbHtml
            // 
            this.rtbHtml.Location = new System.Drawing.Point(6, 80);
            this.rtbHtml.Name = "rtbHtml";
            this.rtbHtml.Size = new System.Drawing.Size(279, 410);
            this.rtbHtml.TabIndex = 0;
            this.rtbHtml.Text = "";
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
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(51, 10);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(100, 21);
            this.txtPage.TabIndex = 1;
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
            // PicpUpWebHtmlFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 493);
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
    }
}