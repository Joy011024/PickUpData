namespace CaptureManage.AppWin
{
    partial class WebDataCaptureForm
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
            this.btnLoadBaseData = new System.Windows.Forms.Button();
            this.cmbBeginStation = new System.Windows.Forms.ComboBox();
            this.lblBeginStation = new System.Windows.Forms.Label();
            this.lblToStation = new System.Windows.Forms.Label();
            this.cmbToStation = new System.Windows.Forms.ComboBox();
            this.btnUseRuleStation = new System.Windows.Forms.Button();
            this.dtpGoTime = new System.Windows.Forms.DateTimePicker();
            this.lblGoTime = new System.Windows.Forms.Label();
            this.panelWhere = new System.Windows.Forms.Panel();
            this.btnTicketQuery = new System.Windows.Forms.Button();
            this.rtbTip = new System.Windows.Forms.RichTextBox();
            this.btnRefreshVerifyCode = new System.Windows.Forms.Button();
            this.btnJob = new System.Windows.Forms.Button();
            this.logicPanel = new System.Windows.Forms.Panel();
            this.pbVerifyCodeImg = new System.Windows.Forms.PictureBox();
            this.lsbTip = new System.Windows.Forms.ListBox();
            this.btnClearTip = new System.Windows.Forms.Button();
            this.loginPanel = new System.Windows.Forms.Panel();
            this.lblUserName12306 = new System.Windows.Forms.Label();
            this.txtUserName12306 = new System.Windows.Forms.TextBox();
            this.txtPsw12306 = new System.Windows.Forms.TextBox();
            this.lblPsw12306 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblPanelTypeDesc = new System.Windows.Forms.Label();
            this.panelWhere.SuspendLayout();
            this.logicPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerifyCodeImg)).BeginInit();
            this.loginPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadBaseData
            // 
            this.btnLoadBaseData.Location = new System.Drawing.Point(512, 12);
            this.btnLoadBaseData.Name = "btnLoadBaseData";
            this.btnLoadBaseData.Size = new System.Drawing.Size(75, 23);
            this.btnLoadBaseData.TabIndex = 1;
            this.btnLoadBaseData.Text = "业务数据";
            this.btnLoadBaseData.UseVisualStyleBackColor = true;
            this.btnLoadBaseData.Click += new System.EventHandler(this.Button_Click);
            // 
            // cmbBeginStation
            // 
            this.cmbBeginStation.FormattingEnabled = true;
            this.cmbBeginStation.Location = new System.Drawing.Point(75, 9);
            this.cmbBeginStation.Name = "cmbBeginStation";
            this.cmbBeginStation.Size = new System.Drawing.Size(143, 20);
            this.cmbBeginStation.TabIndex = 2;
            this.cmbBeginStation.Tag = "GoStation";
            this.cmbBeginStation.TextUpdate += new System.EventHandler(this.CombobBox_TextUpdate);
            // 
            // lblBeginStation
            // 
            this.lblBeginStation.AutoSize = true;
            this.lblBeginStation.Location = new System.Drawing.Point(13, 14);
            this.lblBeginStation.Name = "lblBeginStation";
            this.lblBeginStation.Size = new System.Drawing.Size(41, 12);
            this.lblBeginStation.TabIndex = 3;
            this.lblBeginStation.Text = "出发地";
            // 
            // lblToStation
            // 
            this.lblToStation.AutoSize = true;
            this.lblToStation.Location = new System.Drawing.Point(16, 50);
            this.lblToStation.Name = "lblToStation";
            this.lblToStation.Size = new System.Drawing.Size(41, 12);
            this.lblToStation.TabIndex = 4;
            this.lblToStation.Text = "目的地";
            // 
            // cmbToStation
            // 
            this.cmbToStation.FormattingEnabled = true;
            this.cmbToStation.Location = new System.Drawing.Point(75, 47);
            this.cmbToStation.Name = "cmbToStation";
            this.cmbToStation.Size = new System.Drawing.Size(143, 20);
            this.cmbToStation.TabIndex = 5;
            this.cmbToStation.Tag = "ToStation";
            this.cmbToStation.TextUpdate += new System.EventHandler(this.CombobBox_TextUpdate);
            // 
            // btnUseRuleStation
            // 
            this.btnUseRuleStation.Location = new System.Drawing.Point(600, 12);
            this.btnUseRuleStation.Name = "btnUseRuleStation";
            this.btnUseRuleStation.Size = new System.Drawing.Size(97, 23);
            this.btnUseRuleStation.TabIndex = 7;
            this.btnUseRuleStation.Text = "启用实时车站";
            this.btnUseRuleStation.UseVisualStyleBackColor = true;
            this.btnUseRuleStation.Click += new System.EventHandler(this.Button_Click);
            // 
            // dtpGoTime
            // 
            this.dtpGoTime.Location = new System.Drawing.Point(75, 82);
            this.dtpGoTime.Name = "dtpGoTime";
            this.dtpGoTime.Size = new System.Drawing.Size(143, 21);
            this.dtpGoTime.TabIndex = 8;
            this.dtpGoTime.Value = new System.DateTime(2017, 10, 24, 22, 39, 0, 0);
            // 
            // lblGoTime
            // 
            this.lblGoTime.AutoSize = true;
            this.lblGoTime.Location = new System.Drawing.Point(16, 85);
            this.lblGoTime.Name = "lblGoTime";
            this.lblGoTime.Size = new System.Drawing.Size(53, 12);
            this.lblGoTime.TabIndex = 9;
            this.lblGoTime.Text = "出发日期";
            // 
            // panelWhere
            // 
            this.panelWhere.Controls.Add(this.lblToStation);
            this.panelWhere.Controls.Add(this.lblGoTime);
            this.panelWhere.Controls.Add(this.cmbBeginStation);
            this.panelWhere.Controls.Add(this.dtpGoTime);
            this.panelWhere.Controls.Add(this.lblBeginStation);
            this.panelWhere.Controls.Add(this.cmbToStation);
            this.panelWhere.Location = new System.Drawing.Point(503, 41);
            this.panelWhere.Name = "panelWhere";
            this.panelWhere.Size = new System.Drawing.Size(226, 115);
            this.panelWhere.TabIndex = 10;
            // 
            // btnTicketQuery
            // 
            this.btnTicketQuery.Location = new System.Drawing.Point(584, 162);
            this.btnTicketQuery.Name = "btnTicketQuery";
            this.btnTicketQuery.Size = new System.Drawing.Size(75, 23);
            this.btnTicketQuery.TabIndex = 11;
            this.btnTicketQuery.Text = "车票查询";
            this.btnTicketQuery.UseVisualStyleBackColor = true;
            // 
            // rtbTip
            // 
            this.rtbTip.Location = new System.Drawing.Point(503, 365);
            this.rtbTip.Name = "rtbTip";
            this.rtbTip.Size = new System.Drawing.Size(236, 52);
            this.rtbTip.TabIndex = 12;
            this.rtbTip.Text = "";
            // 
            // btnRefreshVerifyCode
            // 
            this.btnRefreshVerifyCode.Location = new System.Drawing.Point(503, 194);
            this.btnRefreshVerifyCode.Name = "btnRefreshVerifyCode";
            this.btnRefreshVerifyCode.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshVerifyCode.TabIndex = 13;
            this.btnRefreshVerifyCode.Tag = "RefreshVerifyCode";
            this.btnRefreshVerifyCode.Text = "刷新验证码";
            this.btnRefreshVerifyCode.UseVisualStyleBackColor = true;
            // 
            // btnJob
            // 
            this.btnJob.Location = new System.Drawing.Point(503, 162);
            this.btnJob.Name = "btnJob";
            this.btnJob.Size = new System.Drawing.Size(75, 23);
            this.btnJob.TabIndex = 14;
            this.btnJob.Text = "作业";
            this.btnJob.UseVisualStyleBackColor = true;
            // 
            // logicPanel
            // 
            this.logicPanel.Controls.Add(this.loginPanel);
            this.logicPanel.Controls.Add(this.pbVerifyCodeImg);
            this.logicPanel.Location = new System.Drawing.Point(12, 12);
            this.logicPanel.Name = "logicPanel";
            this.logicPanel.Size = new System.Drawing.Size(485, 406);
            this.logicPanel.TabIndex = 15;
            // 
            // pbVerifyCodeImg
            // 
            this.pbVerifyCodeImg.Location = new System.Drawing.Point(9, 9);
            this.pbVerifyCodeImg.Name = "pbVerifyCodeImg";
            this.pbVerifyCodeImg.Size = new System.Drawing.Size(285, 207);
            this.pbVerifyCodeImg.TabIndex = 0;
            this.pbVerifyCodeImg.TabStop = false;
            // 
            // lsbTip
            // 
            this.lsbTip.FormattingEnabled = true;
            this.lsbTip.ItemHeight = 12;
            this.lsbTip.Location = new System.Drawing.Point(503, 223);
            this.lsbTip.Name = "lsbTip";
            this.lsbTip.Size = new System.Drawing.Size(236, 136);
            this.lsbTip.TabIndex = 16;
            // 
            // btnClearTip
            // 
            this.btnClearTip.Location = new System.Drawing.Point(585, 194);
            this.btnClearTip.Name = "btnClearTip";
            this.btnClearTip.Size = new System.Drawing.Size(75, 23);
            this.btnClearTip.TabIndex = 17;
            this.btnClearTip.Text = "清空提示";
            this.btnClearTip.UseVisualStyleBackColor = true;
            // 
            // loginPanel
            // 
            this.loginPanel.Controls.Add(this.lblPanelTypeDesc);
            this.loginPanel.Controls.Add(this.btnLogin);
            this.loginPanel.Controls.Add(this.txtPsw12306);
            this.loginPanel.Controls.Add(this.lblPsw12306);
            this.loginPanel.Controls.Add(this.txtUserName12306);
            this.loginPanel.Controls.Add(this.lblUserName12306);
            this.loginPanel.Location = new System.Drawing.Point(301, 9);
            this.loginPanel.Name = "loginPanel";
            this.loginPanel.Size = new System.Drawing.Size(181, 207);
            this.loginPanel.TabIndex = 1;
            // 
            // lblUserName12306
            // 
            this.lblUserName12306.AutoSize = true;
            this.lblUserName12306.Location = new System.Drawing.Point(3, 32);
            this.lblUserName12306.Name = "lblUserName12306";
            this.lblUserName12306.Size = new System.Drawing.Size(41, 12);
            this.lblUserName12306.TabIndex = 0;
            this.lblUserName12306.Text = "用户名";
            // 
            // txtUserName12306
            // 
            this.txtUserName12306.Location = new System.Drawing.Point(5, 55);
            this.txtUserName12306.Name = "txtUserName12306";
            this.txtUserName12306.Size = new System.Drawing.Size(172, 21);
            this.txtUserName12306.TabIndex = 1;
            // 
            // txtPsw12306
            // 
            this.txtPsw12306.Location = new System.Drawing.Point(5, 118);
            this.txtPsw12306.Name = "txtPsw12306";
            this.txtPsw12306.Size = new System.Drawing.Size(172, 21);
            this.txtPsw12306.TabIndex = 3;
            // 
            // lblPsw12306
            // 
            this.lblPsw12306.AutoSize = true;
            this.lblPsw12306.Location = new System.Drawing.Point(3, 95);
            this.lblPsw12306.Name = "lblPsw12306";
            this.lblPsw12306.Size = new System.Drawing.Size(29, 12);
            this.lblPsw12306.TabIndex = 2;
            this.lblPsw12306.Text = "密码";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(5, 155);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // lblPanelTypeDesc
            // 
            this.lblPanelTypeDesc.AutoSize = true;
            this.lblPanelTypeDesc.Location = new System.Drawing.Point(3, 2);
            this.lblPanelTypeDesc.Name = "lblPanelTypeDesc";
            this.lblPanelTypeDesc.Size = new System.Drawing.Size(59, 12);
            this.lblPanelTypeDesc.TabIndex = 5;
            this.lblPanelTypeDesc.Text = "12306账户";
            // 
            // WebDataCaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 430);
            this.Controls.Add(this.btnClearTip);
            this.Controls.Add(this.rtbTip);
            this.Controls.Add(this.lsbTip);
            this.Controls.Add(this.logicPanel);
            this.Controls.Add(this.btnJob);
            this.Controls.Add(this.btnRefreshVerifyCode);
            this.Controls.Add(this.btnTicketQuery);
            this.Controls.Add(this.panelWhere);
            this.Controls.Add(this.btnUseRuleStation);
            this.Controls.Add(this.btnLoadBaseData);
            this.Name = "WebDataCaptureForm";
            this.Text = "WebDataCaptureForm";
            this.panelWhere.ResumeLayout(false);
            this.panelWhere.PerformLayout();
            this.logicPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbVerifyCodeImg)).EndInit();
            this.loginPanel.ResumeLayout(false);
            this.loginPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadBaseData;
        private System.Windows.Forms.ComboBox cmbBeginStation;
        private System.Windows.Forms.Label lblBeginStation;
        private System.Windows.Forms.Label lblToStation;
        private System.Windows.Forms.ComboBox cmbToStation;
        private System.Windows.Forms.Button btnUseRuleStation;
        private System.Windows.Forms.DateTimePicker dtpGoTime;
        private System.Windows.Forms.Label lblGoTime;
        private System.Windows.Forms.Panel panelWhere;
        private System.Windows.Forms.Button btnTicketQuery;
        private System.Windows.Forms.RichTextBox rtbTip;
        private System.Windows.Forms.Button btnRefreshVerifyCode;
        private System.Windows.Forms.Button btnJob;
        private System.Windows.Forms.Panel logicPanel;
        private System.Windows.Forms.PictureBox pbVerifyCodeImg;
        private System.Windows.Forms.ListBox lsbTip;
        private System.Windows.Forms.Button btnClearTip;
        private System.Windows.Forms.Panel loginPanel;
        private System.Windows.Forms.Label lblPanelTypeDesc;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPsw12306;
        private System.Windows.Forms.Label lblPsw12306;
        private System.Windows.Forms.TextBox txtUserName12306;
        private System.Windows.Forms.Label lblUserName12306;
    }
}