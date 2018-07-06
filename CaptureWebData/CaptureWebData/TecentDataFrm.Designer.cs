﻿namespace CaptureWebData
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTodayPickUp = new System.Windows.Forms.Label();
            this.lsbStatic = new System.Windows.Forms.ListBox();
            this.gbPollingType = new System.Windows.Forms.GroupBox();
            this.rbDepth = new System.Windows.Forms.RadioButton();
            this.rbGuid = new System.Windows.Forms.RadioButton();
            this.rbNormal = new System.Windows.Forms.RadioButton();
            this.lblPageIndex = new System.Windows.Forms.Label();
            this.txtPageIndex = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.workPanel = new System.Windows.Forms.Panel();
            this.btnTodayPickupResult = new System.Windows.Forms.Button();
            this.rbtWebPanel = new System.Windows.Forms.RadioButton();
            this.rbtWorkPanel = new System.Windows.Forms.RadioButton();
            this.switchPanel = new System.Windows.Forms.Panel();
            this.lblSwitchShow = new System.Windows.Forms.Label();
            this.pickUpIEWebCookie = new SelfControlForm.PickUpIEWebCookieData();
            this.ckSyncUin = new System.Windows.Forms.CheckBox();
            this.ckBackGroundCall = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.gbPollingType.SuspendLayout();
            this.workPanel.SuspendLayout();
            this.switchPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(702, 242);
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
            this.lblTimeSpan.Location = new System.Drawing.Point(698, 350);
            this.lblTimeSpan.Name = "lblTimeSpan";
            this.lblTimeSpan.Size = new System.Drawing.Size(77, 12);
            this.lblTimeSpan.TabIndex = 2;
            this.lblTimeSpan.Text = "轮询间隔(秒)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(698, 385);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "次数(0不停)";
            // 
            // txtTimeSpan
            // 
            this.txtTimeSpan.Location = new System.Drawing.Point(790, 341);
            this.txtTimeSpan.Name = "txtTimeSpan";
            this.txtTimeSpan.Size = new System.Drawing.Size(103, 21);
            this.txtTimeSpan.TabIndex = 4;
            this.txtTimeSpan.Text = "3";
            // 
            // txtRepeact
            // 
            this.txtRepeact.Location = new System.Drawing.Point(790, 376);
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
            this.cmbCity.SelectedIndexChanged += new System.EventHandler(this.cmbCity_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(698, 311);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "查询数量";
            // 
            // txtCurrentLimit
            // 
            this.txtCurrentLimit.Location = new System.Drawing.Point(790, 308);
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
            this.lblGender.Location = new System.Drawing.Point(705, 279);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(29, 12);
            this.lblGender.TabIndex = 14;
            this.lblGender.Text = "性别";
            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Location = new System.Drawing.Point(792, 276);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(103, 20);
            this.cmbGender.TabIndex = 15;
            // 
            // ckStartQuartz
            // 
            this.ckStartQuartz.AutoSize = true;
            this.ckStartQuartz.Location = new System.Drawing.Point(700, 144);
            this.ckStartQuartz.Name = "ckStartQuartz";
            this.ckStartQuartz.Size = new System.Drawing.Size(72, 16);
            this.ckStartQuartz.TabIndex = 16;
            this.ckStartQuartz.Text = "启用轮询";
            this.ckStartQuartz.UseVisualStyleBackColor = true;
            this.ckStartQuartz.CheckedChanged += new System.EventHandler(this.ckStartQuartz_CheckedChanged);
            // 
            // rtbTip
            // 
            this.rtbTip.Location = new System.Drawing.Point(694, 541);
            this.rtbTip.Name = "rtbTip";
            this.rtbTip.Size = new System.Drawing.Size(205, 91);
            this.rtbTip.TabIndex = 17;
            this.rtbTip.Text = "";
            // 
            // btnDeleteQuartz
            // 
            this.btnDeleteQuartz.Location = new System.Drawing.Point(770, 140);
            this.btnDeleteQuartz.Name = "btnDeleteQuartz";
            this.btnDeleteQuartz.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteQuartz.TabIndex = 18;
            this.btnDeleteQuartz.Text = "关闭轮询";
            this.btnDeleteQuartz.UseVisualStyleBackColor = true;
            this.btnDeleteQuartz.Click += new System.EventHandler(this.btnDeleteQuartz_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbProvince);
            this.groupBox1.Controls.Add(this.cmbDistinct);
            this.groupBox1.Controls.Add(this.lblCity);
            this.groupBox1.Controls.Add(this.lblDistinct);
            this.groupBox1.Controls.Add(this.cmbCity);
            this.groupBox1.Location = new System.Drawing.Point(694, 410);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 125);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "所在地";
            // 
            // lblTodayPickUp
            // 
            this.lblTodayPickUp.AutoSize = true;
            this.lblTodayPickUp.Location = new System.Drawing.Point(700, 24);
            this.lblTodayPickUp.Name = "lblTodayPickUp";
            this.lblTodayPickUp.Size = new System.Drawing.Size(89, 12);
            this.lblTodayPickUp.TabIndex = 22;
            this.lblTodayPickUp.Text = "今日采集（条）";
            // 
            // lsbStatic
            // 
            this.lsbStatic.FormattingEnabled = true;
            this.lsbStatic.ItemHeight = 12;
            this.lsbStatic.Location = new System.Drawing.Point(702, 50);
            this.lsbStatic.Name = "lsbStatic";
            this.lsbStatic.Size = new System.Drawing.Size(206, 88);
            this.lsbStatic.TabIndex = 24;
            // 
            // gbPollingType
            // 
            this.gbPollingType.Controls.Add(this.rbDepth);
            this.gbPollingType.Controls.Add(this.rbGuid);
            this.gbPollingType.Controls.Add(this.rbNormal);
            this.gbPollingType.Location = new System.Drawing.Point(702, 166);
            this.gbPollingType.Name = "gbPollingType";
            this.gbPollingType.Size = new System.Drawing.Size(202, 41);
            this.gbPollingType.TabIndex = 25;
            this.gbPollingType.TabStop = false;
            this.gbPollingType.Text = "轮询方式";
            // 
            // rbDepth
            // 
            this.rbDepth.AutoSize = true;
            this.rbDepth.Location = new System.Drawing.Point(121, 19);
            this.rbDepth.Name = "rbDepth";
            this.rbDepth.Size = new System.Drawing.Size(71, 16);
            this.rbDepth.TabIndex = 2;
            this.rbDepth.TabStop = true;
            this.rbDepth.Text = "深度检索";
            this.rbDepth.UseVisualStyleBackColor = true;
            // 
            // rbGuid
            // 
            this.rbGuid.AutoSize = true;
            this.rbGuid.Location = new System.Drawing.Point(68, 20);
            this.rbGuid.Name = "rbGuid";
            this.rbGuid.Size = new System.Drawing.Size(47, 16);
            this.rbGuid.TabIndex = 1;
            this.rbGuid.TabStop = true;
            this.rbGuid.Text = "随机";
            this.rbGuid.UseVisualStyleBackColor = true;
            // 
            // rbNormal
            // 
            this.rbNormal.AutoSize = true;
            this.rbNormal.Location = new System.Drawing.Point(15, 20);
            this.rbNormal.Name = "rbNormal";
            this.rbNormal.Size = new System.Drawing.Size(47, 16);
            this.rbNormal.TabIndex = 0;
            this.rbNormal.TabStop = true;
            this.rbNormal.Text = "常规";
            this.rbNormal.UseVisualStyleBackColor = true;
            // 
            // lblPageIndex
            // 
            this.lblPageIndex.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.lblPageIndex.AutoSize = true;
            this.lblPageIndex.Location = new System.Drawing.Point(702, 209);
            this.lblPageIndex.Name = "lblPageIndex";
            this.lblPageIndex.Size = new System.Drawing.Size(29, 12);
            this.lblPageIndex.TabIndex = 26;
            this.lblPageIndex.Tag = "1";
            this.lblPageIndex.Text = "页码";
            // 
            // txtPageIndex
            // 
            this.txtPageIndex.Location = new System.Drawing.Point(790, 209);
            this.txtPageIndex.Name = "txtPageIndex";
            this.txtPageIndex.Size = new System.Drawing.Size(100, 21);
            this.txtPageIndex.TabIndex = 27;
            this.txtPageIndex.Text = "1";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(851, 140);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(57, 23);
            this.btnExit.TabIndex = 28;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(788, 19);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(57, 23);
            this.btnRefresh.TabIndex = 29;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // workPanel
            // 
            this.workPanel.Controls.Add(this.btnTodayPickupResult);
            this.workPanel.Location = new System.Drawing.Point(8, 38);
            this.workPanel.Name = "workPanel";
            this.workPanel.Size = new System.Drawing.Size(480, 579);
            this.workPanel.TabIndex = 30;
            this.workPanel.Visible = false;
            // 
            // btnTodayPickupResult
            // 
            this.btnTodayPickupResult.Location = new System.Drawing.Point(10, 12);
            this.btnTodayPickupResult.Name = "btnTodayPickupResult";
            this.btnTodayPickupResult.Size = new System.Drawing.Size(126, 23);
            this.btnTodayPickupResult.TabIndex = 0;
            this.btnTodayPickupResult.Text = "今日采集详情";
            this.btnTodayPickupResult.UseVisualStyleBackColor = true;
            // 
            // rbtWebPanel
            // 
            this.rbtWebPanel.AutoSize = true;
            this.rbtWebPanel.Checked = true;
            this.rbtWebPanel.Location = new System.Drawing.Point(147, 7);
            this.rbtWebPanel.Name = "rbtWebPanel";
            this.rbtWebPanel.Size = new System.Drawing.Size(53, 16);
            this.rbtWebPanel.TabIndex = 31;
            this.rbtWebPanel.TabStop = true;
            this.rbtWebPanel.Text = "Web页";
            this.rbtWebPanel.UseVisualStyleBackColor = true;
            // 
            // rbtWorkPanel
            // 
            this.rbtWorkPanel.AutoSize = true;
            this.rbtWorkPanel.Location = new System.Drawing.Point(70, 7);
            this.rbtWorkPanel.Name = "rbtWorkPanel";
            this.rbtWorkPanel.Size = new System.Drawing.Size(59, 16);
            this.rbtWorkPanel.TabIndex = 32;
            this.rbtWorkPanel.Text = "工作页";
            this.rbtWorkPanel.UseVisualStyleBackColor = true;
            // 
            // switchPanel
            // 
            this.switchPanel.Controls.Add(this.lblSwitchShow);
            this.switchPanel.Controls.Add(this.rbtWorkPanel);
            this.switchPanel.Controls.Add(this.rbtWebPanel);
            this.switchPanel.Location = new System.Drawing.Point(15, 8);
            this.switchPanel.Name = "switchPanel";
            this.switchPanel.Size = new System.Drawing.Size(212, 28);
            this.switchPanel.TabIndex = 0;
            // 
            // lblSwitchShow
            // 
            this.lblSwitchShow.AutoSize = true;
            this.lblSwitchShow.Location = new System.Drawing.Point(3, 7);
            this.lblSwitchShow.Name = "lblSwitchShow";
            this.lblSwitchShow.Size = new System.Drawing.Size(53, 12);
            this.lblSwitchShow.TabIndex = 31;
            this.lblSwitchShow.Text = "切换显示";
            // 
            // pickUpIEWebCookie
            // 
            this.pickUpIEWebCookie.CallBack = null;
            this.pickUpIEWebCookie.Location = new System.Drawing.Point(18, 45);
            this.pickUpIEWebCookie.Name = "pickUpIEWebCookie";
            this.pickUpIEWebCookie.Size = new System.Drawing.Size(654, 594);
            this.pickUpIEWebCookie.TabIndex = 19;
            // 
            // ckSyncUin
            // 
            this.ckSyncUin.AutoSize = true;
            this.ckSyncUin.Location = new System.Drawing.Point(818, 245);
            this.ckSyncUin.Name = "ckSyncUin";
            this.ckSyncUin.Size = new System.Drawing.Size(72, 16);
            this.ckSyncUin.TabIndex = 31;
            this.ckSyncUin.Text = "同步数据";
            this.ckSyncUin.UseVisualStyleBackColor = true;
            // 
            // ckBackGroundCall
            // 
            this.ckBackGroundCall.AutoSize = true;
            this.ckBackGroundCall.Checked = true;
            this.ckBackGroundCall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckBackGroundCall.Location = new System.Drawing.Point(851, 24);
            this.ckBackGroundCall.Name = "ckBackGroundCall";
            this.ckBackGroundCall.Size = new System.Drawing.Size(72, 16);
            this.ckBackGroundCall.TabIndex = 32;
            this.ckBackGroundCall.Text = "后台采集";
            this.ckBackGroundCall.UseVisualStyleBackColor = true;
            // 
            // TecentDataFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 656);
            this.Controls.Add(this.ckBackGroundCall);
            this.Controls.Add(this.ckSyncUin);
            this.Controls.Add(this.switchPanel);
            this.Controls.Add(this.workPanel);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtPageIndex);
            this.Controls.Add(this.lblPageIndex);
            this.Controls.Add(this.gbPollingType);
            this.Controls.Add(this.lsbStatic);
            this.Controls.Add(this.lblTodayPickUp);
            this.Controls.Add(this.ckStartQuartz);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pickUpIEWebCookie);
            this.Controls.Add(this.btnDeleteQuartz);
            this.Controls.Add(this.rtbTip);
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
            this.gbPollingType.ResumeLayout(false);
            this.gbPollingType.PerformLayout();
            this.workPanel.ResumeLayout(false);
            this.switchPanel.ResumeLayout(false);
            this.switchPanel.PerformLayout();
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
        private System.Windows.Forms.ListBox lsbStatic;
        private System.Windows.Forms.GroupBox gbPollingType;
        private System.Windows.Forms.RadioButton rbGuid;
        private System.Windows.Forms.RadioButton rbNormal;
        private System.Windows.Forms.RadioButton rbDepth;
        private System.Windows.Forms.Label lblPageIndex;
        private System.Windows.Forms.TextBox txtPageIndex;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel workPanel;
        private System.Windows.Forms.RadioButton rbtWebPanel;
        private System.Windows.Forms.RadioButton rbtWorkPanel;
        private System.Windows.Forms.Panel switchPanel;
        private System.Windows.Forms.Label lblSwitchShow;
        private System.Windows.Forms.Button btnTodayPickupResult;
        private System.Windows.Forms.CheckBox ckSyncUin;
        private System.Windows.Forms.CheckBox ckBackGroundCall;




    }
}