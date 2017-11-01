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
            this.microsoftBrowser1 = new FeatureFrmList.MicrosoftBrowser();
            this.btnTicketQuery = new System.Windows.Forms.Button();
            this.rtbTip = new System.Windows.Forms.RichTextBox();
            this.panelWhere.SuspendLayout();
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
            // microsoftBrowser1
            // 
            this.microsoftBrowser1.Location = new System.Drawing.Point(12, 12);
            this.microsoftBrowser1.Name = "microsoftBrowser1";
            this.microsoftBrowser1.Size = new System.Drawing.Size(476, 388);
            this.microsoftBrowser1.TabIndex = 0;
            // 
            // btnTicketQuery
            // 
            this.btnTicketQuery.Location = new System.Drawing.Point(512, 180);
            this.btnTicketQuery.Name = "btnTicketQuery";
            this.btnTicketQuery.Size = new System.Drawing.Size(75, 23);
            this.btnTicketQuery.TabIndex = 11;
            this.btnTicketQuery.Text = "车票查询";
            this.btnTicketQuery.UseVisualStyleBackColor = true;
            // 
            // rtbTip
            // 
            this.rtbTip.Location = new System.Drawing.Point(503, 223);
            this.rtbTip.Name = "rtbTip";
            this.rtbTip.Size = new System.Drawing.Size(226, 177);
            this.rtbTip.TabIndex = 12;
            this.rtbTip.Text = "";
            // 
            // WebDataCaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 421);
            this.Controls.Add(this.rtbTip);
            this.Controls.Add(this.btnTicketQuery);
            this.Controls.Add(this.panelWhere);
            this.Controls.Add(this.btnUseRuleStation);
            this.Controls.Add(this.btnLoadBaseData);
            this.Controls.Add(this.microsoftBrowser1);
            this.Name = "WebDataCaptureForm";
            this.Text = "WebDataCaptureForm";
            this.panelWhere.ResumeLayout(false);
            this.panelWhere.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FeatureFrmList.MicrosoftBrowser microsoftBrowser1;
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
    }
}