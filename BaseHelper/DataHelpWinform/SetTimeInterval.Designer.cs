namespace DataHelpWinform
{
    partial class SetTimeInterval
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
            this.dtpSetTimeInterval = new System.Windows.Forms.DateTimePicker();
            this.lblSetTime = new System.Windows.Forms.Label();
            this.cmbTimeCategory = new System.Windows.Forms.ComboBox();
            this.lblIntervalSize = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.lbl = new System.Windows.Forms.Label();
            this.txtTimeSpan = new System.Windows.Forms.TextBox();
            this.cmbTimeSpan = new System.Windows.Forms.ComboBox();
            this.panelStatic = new System.Windows.Forms.Panel();
            this.panelTimeSpan = new System.Windows.Forms.Panel();
            this.btnSet = new System.Windows.Forms.Button();
            this.cbAfter = new System.Windows.Forms.CheckBox();
            this.panelAfter = new System.Windows.Forms.Panel();
            this.dtpAfter = new System.Windows.Forms.DateTimePicker();
            this.panelStatic.SuspendLayout();
            this.panelTimeSpan.SuspendLayout();
            this.panelAfter.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpSetTimeInterval
            // 
            this.dtpSetTimeInterval.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpSetTimeInterval.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSetTimeInterval.Location = new System.Drawing.Point(4, 3);
            this.dtpSetTimeInterval.MinDate = new System.DateTime(2017, 12, 25, 23, 59, 0, 0);
            this.dtpSetTimeInterval.Name = "dtpSetTimeInterval";
            this.dtpSetTimeInterval.Size = new System.Drawing.Size(213, 21);
            this.dtpSetTimeInterval.TabIndex = 14;
            this.dtpSetTimeInterval.Tag = "FixedTime";
            this.dtpSetTimeInterval.Value = new System.DateTime(2017, 12, 25, 23, 59, 0, 0);
            // 
            // lblSetTime
            // 
            this.lblSetTime.AutoSize = true;
            this.lblSetTime.Location = new System.Drawing.Point(3, 9);
            this.lblSetTime.Name = "lblSetTime";
            this.lblSetTime.Size = new System.Drawing.Size(53, 12);
            this.lblSetTime.TabIndex = 13;
            this.lblSetTime.Text = "定时触发";
            // 
            // cmbTimeCategory
            // 
            this.cmbTimeCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeCategory.FormattingEnabled = true;
            this.cmbTimeCategory.Location = new System.Drawing.Point(62, 6);
            this.cmbTimeCategory.Name = "cmbTimeCategory";
            this.cmbTimeCategory.Size = new System.Drawing.Size(113, 20);
            this.cmbTimeCategory.TabIndex = 16;
            this.cmbTimeCategory.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectIndex);
            // 
            // lblIntervalSize
            // 
            this.lblIntervalSize.AutoSize = true;
            this.lblIntervalSize.Location = new System.Drawing.Point(3, 4);
            this.lblIntervalSize.Name = "lblIntervalSize";
            this.lblIntervalSize.Size = new System.Drawing.Size(53, 12);
            this.lblIntervalSize.TabIndex = 17;
            this.lblIntervalSize.Text = "执行次数";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(60, 4);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(34, 21);
            this.txtSize.TabIndex = 18;
            this.txtSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Location = new System.Drawing.Point(100, 4);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(29, 12);
            this.lbl.TabIndex = 19;
            this.lbl.Text = "间隔";
            // 
            // txtTimeSpan
            // 
            this.txtTimeSpan.Location = new System.Drawing.Point(135, 3);
            this.txtTimeSpan.Name = "txtTimeSpan";
            this.txtTimeSpan.Size = new System.Drawing.Size(44, 21);
            this.txtTimeSpan.TabIndex = 20;
            this.txtTimeSpan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // cmbTimeSpan
            // 
            this.cmbTimeSpan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeSpan.FormattingEnabled = true;
            this.cmbTimeSpan.Location = new System.Drawing.Point(182, 4);
            this.cmbTimeSpan.Name = "cmbTimeSpan";
            this.cmbTimeSpan.Size = new System.Drawing.Size(39, 20);
            this.cmbTimeSpan.TabIndex = 21;
            // 
            // panelStatic
            // 
            this.panelStatic.Controls.Add(this.dtpSetTimeInterval);
            this.panelStatic.Location = new System.Drawing.Point(6, 35);
            this.panelStatic.Name = "panelStatic";
            this.panelStatic.Size = new System.Drawing.Size(220, 28);
            this.panelStatic.TabIndex = 22;
            this.panelStatic.Tag = "FixedTime";
            // 
            // panelTimeSpan
            // 
            this.panelTimeSpan.Controls.Add(this.lblIntervalSize);
            this.panelTimeSpan.Controls.Add(this.txtSize);
            this.panelTimeSpan.Controls.Add(this.cmbTimeSpan);
            this.panelTimeSpan.Controls.Add(this.txtTimeSpan);
            this.panelTimeSpan.Controls.Add(this.lbl);
            this.panelTimeSpan.Location = new System.Drawing.Point(7, 32);
            this.panelTimeSpan.Name = "panelTimeSpan";
            this.panelTimeSpan.Size = new System.Drawing.Size(224, 27);
            this.panelTimeSpan.TabIndex = 23;
            this.panelTimeSpan.Tag = "CustomTime";
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(182, 6);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(47, 23);
            this.btnSet.TabIndex = 24;
            this.btnSet.Text = "设置";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // cbAfter
            // 
            this.cbAfter.AutoSize = true;
            this.cbAfter.Location = new System.Drawing.Point(3, 8);
            this.cbAfter.Name = "cbAfter";
            this.cbAfter.Size = new System.Drawing.Size(48, 16);
            this.cbAfter.TabIndex = 15;
            this.cbAfter.Text = "之后";
            this.cbAfter.UseVisualStyleBackColor = true;
            // 
            // panelAfter
            // 
            this.panelAfter.Controls.Add(this.cbAfter);
            this.panelAfter.Controls.Add(this.dtpAfter);
            this.panelAfter.Location = new System.Drawing.Point(5, 69);
            this.panelAfter.Name = "panelAfter";
            this.panelAfter.Size = new System.Drawing.Size(226, 28);
            this.panelAfter.TabIndex = 25;
            this.panelAfter.Tag = "CustomTime";
            // 
            // dtpAfter
            // 
            this.dtpAfter.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpAfter.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAfter.Location = new System.Drawing.Point(60, 3);
            this.dtpAfter.MinDate = new System.DateTime(2017, 12, 25, 23, 59, 0, 0);
            this.dtpAfter.Name = "dtpAfter";
            this.dtpAfter.Size = new System.Drawing.Size(163, 21);
            this.dtpAfter.TabIndex = 14;
            this.dtpAfter.Tag = "FixedTime";
            this.dtpAfter.Value = new System.DateTime(2017, 12, 25, 23, 59, 0, 0);
            // 
            // SetTimeInterval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelAfter);
            this.Controls.Add(this.panelTimeSpan);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.panelStatic);
            this.Controls.Add(this.cmbTimeCategory);
            this.Controls.Add(this.lblSetTime);
            this.Name = "SetTimeInterval";
            this.Size = new System.Drawing.Size(239, 105);
            this.panelStatic.ResumeLayout(false);
            this.panelTimeSpan.ResumeLayout(false);
            this.panelTimeSpan.PerformLayout();
            this.panelAfter.ResumeLayout(false);
            this.panelAfter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpSetTimeInterval;
        private System.Windows.Forms.Label lblSetTime;
        private System.Windows.Forms.ComboBox cmbTimeCategory;
        private System.Windows.Forms.Label lblIntervalSize;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.TextBox txtTimeSpan;
        private System.Windows.Forms.ComboBox cmbTimeSpan;
        private System.Windows.Forms.Panel panelStatic;
        private System.Windows.Forms.Panel panelTimeSpan;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.CheckBox cbAfter;
        private System.Windows.Forms.Panel panelAfter;
        private System.Windows.Forms.DateTimePicker dtpAfter;
    }
}
