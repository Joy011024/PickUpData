﻿namespace CefSharpWin
{
    partial class WebFrm
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.layoutPanel = new System.Windows.Forms.Panel();
            this.headPanel = new System.Windows.Forms.Panel();
            this.lblUrl = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.btnGetCookie = new System.Windows.Forms.Button();
            this.table.SuspendLayout();
            this.headPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // table
            // 
            this.table.ColumnCount = 1;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.Controls.Add(this.layoutPanel, 0, 1);
            this.table.Controls.Add(this.headPanel, 0, 0);
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.RowCount = 2;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.Size = new System.Drawing.Size(969, 634);
            this.table.TabIndex = 0;
            // 
            // layoutPanel
            // 
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(3, 53);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.Size = new System.Drawing.Size(963, 578);
            this.layoutPanel.TabIndex = 2;
            // 
            // headPanel
            // 
            this.headPanel.Controls.Add(this.btnGetCookie);
            this.headPanel.Controls.Add(this.lblUrl);
            this.headPanel.Controls.Add(this.txtUrl);
            this.headPanel.Controls.Add(this.btnBrowser);
            this.headPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headPanel.Location = new System.Drawing.Point(3, 3);
            this.headPanel.Name = "headPanel";
            this.headPanel.Size = new System.Drawing.Size(963, 44);
            this.headPanel.TabIndex = 1;
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(26, 14);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(29, 12);
            this.lblUrl.TabIndex = 2;
            this.lblUrl.Text = "网址";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(76, 3);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(696, 35);
            this.txtUrl.TabIndex = 1;
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(788, 9);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(75, 23);
            this.btnBrowser.TabIndex = 0;
            this.btnBrowser.Text = "浏览";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // btnGetCookie
            // 
            this.btnGetCookie.Location = new System.Drawing.Point(879, 9);
            this.btnGetCookie.Name = "btnGetCookie";
            this.btnGetCookie.Size = new System.Drawing.Size(75, 23);
            this.btnGetCookie.TabIndex = 3;
            this.btnGetCookie.Text = "意外";
            this.btnGetCookie.UseVisualStyleBackColor = true;
            this.btnGetCookie.Click += new System.EventHandler(this.btnGetCookie_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 634);
            this.Controls.Add(this.table);
            this.Name = "Form1";
            this.Text = "webFrm";
            this.table.ResumeLayout(false);
            this.headPanel.ResumeLayout(false);
            this.headPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel table;
        private System.Windows.Forms.Panel layoutPanel;
        private System.Windows.Forms.Panel headPanel;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.Button btnGetCookie;
    }
}

