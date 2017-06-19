namespace CaptureWebData
{
    partial class TecentDataFrm
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
            this.btnQuery = new System.Windows.Forms.Button();
            this.lblTimeSpan = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTimeSpan = new System.Windows.Forms.TextBox();
            this.txtRepeact = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbProvince = new System.Windows.Forms.ComboBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.cmbCity = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCurrentLimit = new System.Windows.Forms.TextBox();
            this.lblDistinct = new System.Windows.Forms.Label();
            this.cmbDistinct = new System.Windows.Forms.ComboBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.ckStartQuartz = new System.Windows.Forms.CheckBox();
            this.rtbTip = new System.Windows.Forms.RichTextBox();
            this.btnDeleteQuartz = new System.Windows.Forms.Button();
            this.pickUpIEWebCookie = new SelfControlForm.PickUpIEWebCookieData();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTodayPickUp = new System.Windows.Forms.Label();
            this.txtTodayPickUp = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(567, 190);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "查询qq";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lblTimeSpan
            // 
            this.lblTimeSpan.AutoSize = true;
            this.lblTimeSpan.Location = new System.Drawing.Point(563, 337);
            this.lblTimeSpan.Name = "lblTimeSpan";
            this.lblTimeSpan.Size = new System.Drawing.Size(77, 12);
            this.lblTimeSpan.TabIndex = 2;
            this.lblTimeSpan.Text = "轮询间隔(秒)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(563, 372);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "次数(0不停)";
            // 
            // txtTimeSpan
            // 
            this.txtTimeSpan.Location = new System.Drawing.Point(655, 328);
            this.txtTimeSpan.Name = "txtTimeSpan";
            this.txtTimeSpan.Size = new System.Drawing.Size(103, 21);
            this.txtTimeSpan.TabIndex = 4;
            this.txtTimeSpan.Text = "30";
            // 
            // txtRepeact
            // 
            this.txtRepeact.Location = new System.Drawing.Point(655, 363);
            this.txtRepeact.Name = "txtRepeact";
            this.txtRepeact.Size = new System.Drawing.Size(103, 21);
            this.txtRepeact.TabIndex = 5;
            this.txtRepeact.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "省/直辖市";
            // 
            // cmbProvince
            // 
            this.cmbProvince.FormattingEnabled = true;
            this.cmbProvince.Location = new System.Drawing.Point(98, 25);
            this.cmbProvince.Name = "cmbProvince";
            this.cmbProvince.Size = new System.Drawing.Size(103, 20);
            this.cmbProvince.TabIndex = 7;
            this.cmbProvince.SelectedIndexChanged += new System.EventHandler(this.cmbProvince_SelectedIndexChanged);
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(6, 66);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(35, 12);
            this.lblCity.TabIndex = 8;
            this.lblCity.Text = "市/区";
            // 
            // cmbCity
            // 
            this.cmbCity.FormattingEnabled = true;
            this.cmbCity.Location = new System.Drawing.Point(98, 66);
            this.cmbCity.Name = "cmbCity";
            this.cmbCity.Size = new System.Drawing.Size(103, 20);
            this.cmbCity.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(563, 298);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "查询数量";
            // 
            // txtCurrentLimit
            // 
            this.txtCurrentLimit.Location = new System.Drawing.Point(655, 295);
            this.txtCurrentLimit.Name = "txtCurrentLimit";
            this.txtCurrentLimit.Size = new System.Drawing.Size(103, 21);
            this.txtCurrentLimit.TabIndex = 11;
            this.txtCurrentLimit.Text = "30";
            // 
            // lblDistinct
            // 
            this.lblDistinct.AutoSize = true;
            this.lblDistinct.Location = new System.Drawing.Point(6, 101);
            this.lblDistinct.Name = "lblDistinct";
            this.lblDistinct.Size = new System.Drawing.Size(17, 12);
            this.lblDistinct.TabIndex = 12;
            this.lblDistinct.Text = "县";
            // 
            // cmbDistinct
            // 
            this.cmbDistinct.FormattingEnabled = true;
            this.cmbDistinct.Location = new System.Drawing.Point(98, 98);
            this.cmbDistinct.Name = "cmbDistinct";
            this.cmbDistinct.Size = new System.Drawing.Size(103, 20);
            this.cmbDistinct.TabIndex = 13;
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(563, 252);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(29, 12);
            this.lblGender.TabIndex = 14;
            this.lblGender.Text = "性别";
            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Location = new System.Drawing.Point(655, 252);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(103, 20);
            this.cmbGender.TabIndex = 15;
            // 
            // ckStartQuartz
            // 
            this.ckStartQuartz.AutoSize = true;
            this.ckStartQuartz.Location = new System.Drawing.Point(665, 219);
            this.ckStartQuartz.Name = "ckStartQuartz";
            this.ckStartQuartz.Size = new System.Drawing.Size(72, 16);
            this.ckStartQuartz.TabIndex = 16;
            this.ckStartQuartz.Text = "启用轮询";
            this.ckStartQuartz.UseVisualStyleBackColor = true;
            // 
            // rtbTip
            // 
            this.rtbTip.Location = new System.Drawing.Point(559, 541);
            this.rtbTip.Name = "rtbTip";
            this.rtbTip.Size = new System.Drawing.Size(205, 155);
            this.rtbTip.TabIndex = 17;
            this.rtbTip.Text = "";
            // 
            // btnDeleteQuartz
            // 
            this.btnDeleteQuartz.Location = new System.Drawing.Point(567, 219);
            this.btnDeleteQuartz.Name = "btnDeleteQuartz";
            this.btnDeleteQuartz.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteQuartz.TabIndex = 18;
            this.btnDeleteQuartz.Text = "关闭轮询";
            this.btnDeleteQuartz.UseVisualStyleBackColor = true;
            this.btnDeleteQuartz.Click += new System.EventHandler(this.btnDeleteQuartz_Click);
            // 
            // pickUpIEWebCookie
            // 
            this.pickUpIEWebCookie.CallBack = null;
            this.pickUpIEWebCookie.Location = new System.Drawing.Point(12, 12);
            this.pickUpIEWebCookie.Name = "pickUpIEWebCookie";
            this.pickUpIEWebCookie.Size = new System.Drawing.Size(545, 694);
            this.pickUpIEWebCookie.TabIndex = 19;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbProvince);
            this.groupBox1.Controls.Add(this.cmbDistinct);
            this.groupBox1.Controls.Add(this.lblCity);
            this.groupBox1.Controls.Add(this.lblDistinct);
            this.groupBox1.Controls.Add(this.cmbCity);
            this.groupBox1.Location = new System.Drawing.Point(559, 390);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 125);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "所在地";
            // 
            // lblTodayPickUp
            // 
            this.lblTodayPickUp.AutoSize = true;
            this.lblTodayPickUp.Location = new System.Drawing.Point(565, 24);
            this.lblTodayPickUp.Name = "lblTodayPickUp";
            this.lblTodayPickUp.Size = new System.Drawing.Size(89, 12);
            this.lblTodayPickUp.TabIndex = 22;
            this.lblTodayPickUp.Text = "今日采集（条）";
            // 
            // txtTodayPickUp
            // 
            this.txtTodayPickUp.Enabled = false;
            this.txtTodayPickUp.Location = new System.Drawing.Point(655, 21);
            this.txtTodayPickUp.Name = "txtTodayPickUp";
            this.txtTodayPickUp.Size = new System.Drawing.Size(100, 21);
            this.txtTodayPickUp.TabIndex = 23;
            // 
            // TecentDataFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 708);
            this.Controls.Add(this.txtTodayPickUp);
            this.Controls.Add(this.lblTodayPickUp);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pickUpIEWebCookie);
            this.Controls.Add(this.btnDeleteQuartz);
            this.Controls.Add(this.rtbTip);
            this.Controls.Add(this.ckStartQuartz);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.txtCurrentLimit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRepeact);
            this.Controls.Add(this.txtTimeSpan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTimeSpan);
            this.Controls.Add(this.btnQuery);
            this.Name = "TecentDataFrm";
            this.Text = "TecentDataFrm";
            this.Load += new System.EventHandler(this.TecentDataFrm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label lblTimeSpan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTimeSpan;
        private System.Windows.Forms.TextBox txtRepeact;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbProvince;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.ComboBox cmbCity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCurrentLimit;
        private System.Windows.Forms.Label lblDistinct;
        private System.Windows.Forms.ComboBox cmbDistinct;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.CheckBox ckStartQuartz;
        private System.Windows.Forms.RichTextBox rtbTip;
        private System.Windows.Forms.Button btnDeleteQuartz;
        private SelfControlForm.PickUpIEWebCookieData pickUpIEWebCookie;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTodayPickUp;
        private System.Windows.Forms.TextBox txtTodayPickUp;




    }
}