namespace SqlDBSyncTool
{
    partial class SyncDBFrm
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
            this.panelTarget = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbTargetPassword = new System.Windows.Forms.RichTextBox();
            this.lblTargetPassword = new System.Windows.Forms.Label();
            this.rtbTargetUserId = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbTargetInitialCatalog = new System.Windows.Forms.RichTextBox();
            this.lblInitialCatalog = new System.Windows.Forms.Label();
            this.rtbTargetDataSource = new System.Windows.Forms.RichTextBox();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.btnSyncDb = new System.Windows.Forms.Button();
            this.btnResetTarget = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panelSync = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblSyncDBNewName = new System.Windows.Forms.Label();
            this.richTextBox6 = new System.Windows.Forms.RichTextBox();
            this.lstbTip = new System.Windows.Forms.ListBox();
            this.btnClearTip = new System.Windows.Forms.Button();
            this.btnTestConnTargetSql = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panelTarget.SuspendLayout();
            this.panelSync.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTarget
            // 
            this.panelTarget.Controls.Add(this.btnTestConnTargetSql);
            this.panelTarget.Controls.Add(this.label2);
            this.panelTarget.Controls.Add(this.rtbTargetPassword);
            this.panelTarget.Controls.Add(this.lblTargetPassword);
            this.panelTarget.Controls.Add(this.rtbTargetUserId);
            this.panelTarget.Controls.Add(this.label1);
            this.panelTarget.Controls.Add(this.rtbTargetInitialCatalog);
            this.panelTarget.Controls.Add(this.lblInitialCatalog);
            this.panelTarget.Controls.Add(this.rtbTargetDataSource);
            this.panelTarget.Controls.Add(this.lblDataSource);
            this.panelTarget.Location = new System.Drawing.Point(12, 12);
            this.panelTarget.Name = "panelTarget";
            this.panelTarget.Size = new System.Drawing.Size(466, 182);
            this.panelTarget.TabIndex = 0;
            this.panelTarget.Tag = "TargetDbConnString";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(12, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "目标服务器信息";
            // 
            // rtbTargetPassword
            // 
            this.rtbTargetPassword.Location = new System.Drawing.Point(95, 138);
            this.rtbTargetPassword.Name = "rtbTargetPassword";
            this.rtbTargetPassword.Size = new System.Drawing.Size(357, 28);
            this.rtbTargetPassword.TabIndex = 7;
            this.rtbTargetPassword.Tag = "Password";
            this.rtbTargetPassword.Text = "";
            // 
            // lblTargetPassword
            // 
            this.lblTargetPassword.AutoSize = true;
            this.lblTargetPassword.Location = new System.Drawing.Point(12, 150);
            this.lblTargetPassword.Name = "lblTargetPassword";
            this.lblTargetPassword.Size = new System.Drawing.Size(41, 12);
            this.lblTargetPassword.TabIndex = 6;
            this.lblTargetPassword.Text = "用户名";
            // 
            // rtbTargetUserId
            // 
            this.rtbTargetUserId.Location = new System.Drawing.Point(95, 104);
            this.rtbTargetUserId.Name = "rtbTargetUserId";
            this.rtbTargetUserId.Size = new System.Drawing.Size(357, 28);
            this.rtbTargetUserId.TabIndex = 5;
            this.rtbTargetUserId.Tag = "UserId";
            this.rtbTargetUserId.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "用户名";
            // 
            // rtbTargetInitialCatalog
            // 
            this.rtbTargetInitialCatalog.Location = new System.Drawing.Point(95, 67);
            this.rtbTargetInitialCatalog.Name = "rtbTargetInitialCatalog";
            this.rtbTargetInitialCatalog.Size = new System.Drawing.Size(357, 28);
            this.rtbTargetInitialCatalog.TabIndex = 3;
            this.rtbTargetInitialCatalog.Tag = "InitialCatalog";
            this.rtbTargetInitialCatalog.Text = "";
            // 
            // lblInitialCatalog
            // 
            this.lblInitialCatalog.AutoSize = true;
            this.lblInitialCatalog.Location = new System.Drawing.Point(12, 78);
            this.lblInitialCatalog.Name = "lblInitialCatalog";
            this.lblInitialCatalog.Size = new System.Drawing.Size(65, 12);
            this.lblInitialCatalog.TabIndex = 2;
            this.lblInitialCatalog.Text = "数据库名称";
            // 
            // rtbTargetDataSource
            // 
            this.rtbTargetDataSource.Location = new System.Drawing.Point(95, 33);
            this.rtbTargetDataSource.Name = "rtbTargetDataSource";
            this.rtbTargetDataSource.Size = new System.Drawing.Size(357, 28);
            this.rtbTargetDataSource.TabIndex = 1;
            this.rtbTargetDataSource.Tag = "DataSource";
            this.rtbTargetDataSource.Text = "";
            // 
            // lblDataSource
            // 
            this.lblDataSource.AutoSize = true;
            this.lblDataSource.Location = new System.Drawing.Point(12, 36);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(77, 12);
            this.lblDataSource.TabIndex = 0;
            this.lblDataSource.Text = "服务器实例名";
            // 
            // btnSyncDb
            // 
            this.btnSyncDb.Location = new System.Drawing.Point(12, 425);
            this.btnSyncDb.Name = "btnSyncDb";
            this.btnSyncDb.Size = new System.Drawing.Size(89, 23);
            this.btnSyncDb.TabIndex = 3;
            this.btnSyncDb.Text = "同步";
            this.btnSyncDb.UseVisualStyleBackColor = true;
            this.btnSyncDb.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnResetTarget
            // 
            this.btnResetTarget.Location = new System.Drawing.Point(141, 425);
            this.btnResetTarget.Name = "btnResetTarget";
            this.btnResetTarget.Size = new System.Drawing.Size(108, 23);
            this.btnResetTarget.TabIndex = 4;
            this.btnResetTarget.Text = "重置目标服务器";
            this.btnResetTarget.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(317, 425);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "重置目标服务器";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panelSync
            // 
            this.panelSync.Controls.Add(this.button2);
            this.panelSync.Controls.Add(this.label3);
            this.panelSync.Controls.Add(this.richTextBox2);
            this.panelSync.Controls.Add(this.label4);
            this.panelSync.Controls.Add(this.richTextBox3);
            this.panelSync.Controls.Add(this.label5);
            this.panelSync.Controls.Add(this.richTextBox4);
            this.panelSync.Controls.Add(this.label6);
            this.panelSync.Controls.Add(this.richTextBox5);
            this.panelSync.Controls.Add(this.label7);
            this.panelSync.Location = new System.Drawing.Point(12, 200);
            this.panelSync.Name = "panelSync";
            this.panelSync.Size = new System.Drawing.Size(466, 182);
            this.panelSync.TabIndex = 6;
            this.panelSync.Tag = "SyncDbConnString";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(12, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "待同步服务器信息";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(95, 138);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(357, 28);
            this.richTextBox2.TabIndex = 7;
            this.richTextBox2.Tag = "Password";
            this.richTextBox2.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "用户名";
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(95, 104);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(357, 28);
            this.richTextBox3.TabIndex = 5;
            this.richTextBox3.Tag = "UserId";
            this.richTextBox3.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "用户名";
            // 
            // richTextBox4
            // 
            this.richTextBox4.Location = new System.Drawing.Point(95, 67);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(357, 28);
            this.richTextBox4.TabIndex = 3;
            this.richTextBox4.Tag = "InitialCatalog";
            this.richTextBox4.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "数据库名称";
            // 
            // richTextBox5
            // 
            this.richTextBox5.Location = new System.Drawing.Point(95, 33);
            this.richTextBox5.Name = "richTextBox5";
            this.richTextBox5.Size = new System.Drawing.Size(357, 28);
            this.richTextBox5.TabIndex = 1;
            this.richTextBox5.Tag = "DataSource";
            this.richTextBox5.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "服务器实例名";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(493, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "提示";
            // 
            // lblSyncDBNewName
            // 
            this.lblSyncDBNewName.AutoSize = true;
            this.lblSyncDBNewName.Location = new System.Drawing.Point(24, 392);
            this.lblSyncDBNewName.Name = "lblSyncDBNewName";
            this.lblSyncDBNewName.Size = new System.Drawing.Size(65, 12);
            this.lblSyncDBNewName.TabIndex = 10;
            this.lblSyncDBNewName.Text = "同步新库名";
            // 
            // richTextBox6
            // 
            this.richTextBox6.Location = new System.Drawing.Point(107, 389);
            this.richTextBox6.Name = "richTextBox6";
            this.richTextBox6.Size = new System.Drawing.Size(357, 22);
            this.richTextBox6.TabIndex = 11;
            this.richTextBox6.Text = "";
            // 
            // lstbTip
            // 
            this.lstbTip.FormattingEnabled = true;
            this.lstbTip.ItemHeight = 12;
            this.lstbTip.Location = new System.Drawing.Point(495, 28);
            this.lstbTip.Name = "lstbTip";
            this.lstbTip.Size = new System.Drawing.Size(232, 388);
            this.lstbTip.TabIndex = 12;
            // 
            // btnClearTip
            // 
            this.btnClearTip.Location = new System.Drawing.Point(495, 424);
            this.btnClearTip.Name = "btnClearTip";
            this.btnClearTip.Size = new System.Drawing.Size(75, 23);
            this.btnClearTip.TabIndex = 13;
            this.btnClearTip.Text = "清除提示";
            this.btnClearTip.UseVisualStyleBackColor = true;
            // 
            // btnTestConnTargetSql
            // 
            this.btnTestConnTargetSql.Location = new System.Drawing.Point(370, 4);
            this.btnTestConnTargetSql.Name = "btnTestConnTargetSql";
            this.btnTestConnTargetSql.Size = new System.Drawing.Size(75, 23);
            this.btnTestConnTargetSql.TabIndex = 9;
            this.btnTestConnTargetSql.Text = "测试连接";
            this.btnTestConnTargetSql.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(370, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "测试连接";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // SyncDBFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 462);
            this.Controls.Add(this.btnClearTip);
            this.Controls.Add(this.lstbTip);
            this.Controls.Add(this.richTextBox6);
            this.Controls.Add(this.lblSyncDBNewName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panelSync);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnResetTarget);
            this.Controls.Add(this.btnSyncDb);
            this.Controls.Add(this.panelTarget);
            this.Name = "SyncDBFrm";
            this.Text = "数据库同步工具";
            this.Load += new System.EventHandler(this.SyncDBFrm_Load);
            this.panelTarget.ResumeLayout(false);
            this.panelTarget.PerformLayout();
            this.panelSync.ResumeLayout(false);
            this.panelSync.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTarget;
        private System.Windows.Forms.RichTextBox rtbTargetDataSource;
        private System.Windows.Forms.Label lblDataSource;
        private System.Windows.Forms.Label lblInitialCatalog;
        private System.Windows.Forms.RichTextBox rtbTargetInitialCatalog;
        private System.Windows.Forms.RichTextBox rtbTargetUserId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTargetPassword;
        private System.Windows.Forms.RichTextBox rtbTargetPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSyncDb;
        private System.Windows.Forms.Button btnResetTarget;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelSync;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox richTextBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblSyncDBNewName;
        private System.Windows.Forms.RichTextBox richTextBox6;
        private System.Windows.Forms.Button btnTestConnTargetSql;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox lstbTip;
        private System.Windows.Forms.Button btnClearTip;
    }
}

