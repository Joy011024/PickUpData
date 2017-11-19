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
            this.pbIcon7 = new System.Windows.Forms.PictureBox();
            this.pbIcon3 = new System.Windows.Forms.PictureBox();
            this.pbIcon8 = new System.Windows.Forms.PictureBox();
            this.pbIcon5 = new System.Windows.Forms.PictureBox();
            this.pbIcon6 = new System.Windows.Forms.PictureBox();
            this.pbIcon4 = new System.Windows.Forms.PictureBox();
            this.pbIcon1 = new System.Windows.Forms.PictureBox();
            this.pbIcon2 = new System.Windows.Forms.PictureBox();
            this.pbVerifyCodeImg = new System.Windows.Forms.PictureBox();
            this.microBrowser = new FeatureFrmList.MicrosoftBrowser();
            this.panelWhere.SuspendLayout();
            this.logicPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerifyCodeImg)).BeginInit();
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
            this.rtbTip.Location = new System.Drawing.Point(9, 222);
            this.rtbTip.Name = "rtbTip";
            this.rtbTip.Size = new System.Drawing.Size(285, 52);
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
            this.logicPanel.Controls.Add(this.pbIcon7);
            this.logicPanel.Controls.Add(this.pbIcon3);
            this.logicPanel.Controls.Add(this.pbIcon8);
            this.logicPanel.Controls.Add(this.pbIcon5);
            this.logicPanel.Controls.Add(this.pbIcon6);
            this.logicPanel.Controls.Add(this.pbIcon4);
            this.logicPanel.Controls.Add(this.pbIcon1);
            this.logicPanel.Controls.Add(this.pbIcon2);
            this.logicPanel.Controls.Add(this.pbVerifyCodeImg);
            this.logicPanel.Controls.Add(this.rtbTip);
            this.logicPanel.Location = new System.Drawing.Point(12, 12);
            this.logicPanel.Name = "logicPanel";
            this.logicPanel.Size = new System.Drawing.Size(300, 283);
            this.logicPanel.TabIndex = 15;
            // 
            // pbIcon7
            // 
            this.pbIcon7.Location = new System.Drawing.Point(167, 150);
            this.pbIcon7.Name = "pbIcon7";
            this.pbIcon7.Size = new System.Drawing.Size(25, 22);
            this.pbIcon7.TabIndex = 20;
            this.pbIcon7.TabStop = false;
            this.pbIcon7.Tag = "Icon";
            // 
            // pbIcon3
            // 
            this.pbIcon3.Location = new System.Drawing.Point(167, 79);
            this.pbIcon3.Name = "pbIcon3";
            this.pbIcon3.Size = new System.Drawing.Size(25, 22);
            this.pbIcon3.TabIndex = 19;
            this.pbIcon3.TabStop = false;
            this.pbIcon3.Tag = "Icon";
            // 
            // pbIcon8
            // 
            this.pbIcon8.Location = new System.Drawing.Point(233, 151);
            this.pbIcon8.Name = "pbIcon8";
            this.pbIcon8.Size = new System.Drawing.Size(25, 22);
            this.pbIcon8.TabIndex = 18;
            this.pbIcon8.TabStop = false;
            this.pbIcon8.Tag = "Icon";
            // 
            // pbIcon5
            // 
            this.pbIcon5.Location = new System.Drawing.Point(31, 151);
            this.pbIcon5.Name = "pbIcon5";
            this.pbIcon5.Size = new System.Drawing.Size(25, 22);
            this.pbIcon5.TabIndex = 17;
            this.pbIcon5.TabStop = false;
            this.pbIcon5.Tag = "Icon";
            // 
            // pbIcon6
            // 
            this.pbIcon6.Location = new System.Drawing.Point(100, 151);
            this.pbIcon6.Name = "pbIcon6";
            this.pbIcon6.Size = new System.Drawing.Size(25, 22);
            this.pbIcon6.TabIndex = 16;
            this.pbIcon6.TabStop = false;
            this.pbIcon6.Tag = "Icon";
            // 
            // pbIcon4
            // 
            this.pbIcon4.Location = new System.Drawing.Point(233, 79);
            this.pbIcon4.Name = "pbIcon4";
            this.pbIcon4.Size = new System.Drawing.Size(25, 22);
            this.pbIcon4.TabIndex = 15;
            this.pbIcon4.TabStop = false;
            this.pbIcon4.Tag = "Icon";
            // 
            // pbIcon1
            // 
            this.pbIcon1.Location = new System.Drawing.Point(31, 79);
            this.pbIcon1.Name = "pbIcon1";
            this.pbIcon1.Size = new System.Drawing.Size(25, 22);
            this.pbIcon1.TabIndex = 14;
            this.pbIcon1.TabStop = false;
            this.pbIcon1.Tag = "Icon";
            // 
            // pbIcon2
            // 
            this.pbIcon2.Location = new System.Drawing.Point(100, 79);
            this.pbIcon2.Name = "pbIcon2";
            this.pbIcon2.Size = new System.Drawing.Size(25, 22);
            this.pbIcon2.TabIndex = 13;
            this.pbIcon2.TabStop = false;
            this.pbIcon2.Tag = "Icon";
            // 
            // pbVerifyCodeImg
            // 
            this.pbVerifyCodeImg.Location = new System.Drawing.Point(9, 9);
            this.pbVerifyCodeImg.Name = "pbVerifyCodeImg";
            this.pbVerifyCodeImg.Size = new System.Drawing.Size(285, 207);
            this.pbVerifyCodeImg.TabIndex = 0;
            this.pbVerifyCodeImg.TabStop = false;
            // 
            // microBrowser
            // 
            this.microBrowser.Location = new System.Drawing.Point(12, 12);
            this.microBrowser.Name = "microBrowser";
            this.microBrowser.Size = new System.Drawing.Size(476, 388);
            this.microBrowser.TabIndex = 0;
            // 
            // WebDataCaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 412);
            this.Controls.Add(this.logicPanel);
            this.Controls.Add(this.btnJob);
            this.Controls.Add(this.btnRefreshVerifyCode);
            this.Controls.Add(this.btnTicketQuery);
            this.Controls.Add(this.panelWhere);
            this.Controls.Add(this.btnUseRuleStation);
            this.Controls.Add(this.btnLoadBaseData);
            this.Controls.Add(this.microBrowser);
            this.Name = "WebDataCaptureForm";
            this.Text = "WebDataCaptureForm";
            this.panelWhere.ResumeLayout(false);
            this.panelWhere.PerformLayout();
            this.logicPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerifyCodeImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FeatureFrmList.MicrosoftBrowser microBrowser;
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
        private System.Windows.Forms.PictureBox pbIcon2;
        private System.Windows.Forms.PictureBox pbIcon8;
        private System.Windows.Forms.PictureBox pbIcon5;
        private System.Windows.Forms.PictureBox pbIcon6;
        private System.Windows.Forms.PictureBox pbIcon4;
        private System.Windows.Forms.PictureBox pbIcon1;
        private System.Windows.Forms.PictureBox pbIcon7;
        private System.Windows.Forms.PictureBox pbIcon3;
    }
}