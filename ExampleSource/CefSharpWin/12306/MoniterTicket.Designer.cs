namespace CefSharpWin
{
    partial class MoniterTicket
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
            this.btnQueryTicket = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.lstContact = new System.Windows.Forms.ListView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblToStation = new System.Windows.Forms.Label();
            this.lblTicketDate = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblFromStation = new System.Windows.Forms.Label();
            this.cmbFromStation = new System.Windows.Forms.ComboBox();
            this.lstSchedule = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCarType = new System.Windows.Forms.Label();
            this.ckAll = new System.Windows.Forms.CheckBox();
            this.ckZCar = new System.Windows.Forms.CheckBox();
            this.ckTCar = new System.Windows.Forms.CheckBox();
            this.ckKCar = new System.Windows.Forms.CheckBox();
            this.ckDCar = new System.Windows.Forms.CheckBox();
            this.ckGCar = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnQueryTicket
            // 
            this.btnQueryTicket.Location = new System.Drawing.Point(739, 6);
            this.btnQueryTicket.Name = "btnQueryTicket";
            this.btnQueryTicket.Size = new System.Drawing.Size(65, 50);
            this.btnQueryTicket.TabIndex = 0;
            this.btnQueryTicket.Text = "车票查询";
            this.btnQueryTicket.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(574, 19);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(121, 21);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // lstContact
            // 
            this.lstContact.Location = new System.Drawing.Point(103, 55);
            this.lstContact.Name = "lstContact";
            this.lstContact.Size = new System.Drawing.Size(121, 153);
            this.lstContact.TabIndex = 2;
            this.lstContact.UseCompatibleStateImageBehavior = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(345, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 3;
            // 
            // lblToStation
            // 
            this.lblToStation.AutoSize = true;
            this.lblToStation.Location = new System.Drawing.Point(272, 20);
            this.lblToStation.Name = "lblToStation";
            this.lblToStation.Size = new System.Drawing.Size(29, 12);
            this.lblToStation.TabIndex = 4;
            this.lblToStation.Text = "到站";
            // 
            // lblTicketDate
            // 
            this.lblTicketDate.AutoSize = true;
            this.lblTicketDate.Location = new System.Drawing.Point(498, 22);
            this.lblTicketDate.Name = "lblTicketDate";
            this.lblTicketDate.Size = new System.Drawing.Size(53, 12);
            this.lblTicketDate.TabIndex = 5;
            this.lblTicketDate.Text = "乘车日期";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(31, 68);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(41, 12);
            this.lblUser.TabIndex = 6;
            this.lblUser.Text = "乘车人";
            // 
            // lblFromStation
            // 
            this.lblFromStation.AutoSize = true;
            this.lblFromStation.Location = new System.Drawing.Point(31, 23);
            this.lblFromStation.Name = "lblFromStation";
            this.lblFromStation.Size = new System.Drawing.Size(41, 12);
            this.lblFromStation.TabIndex = 7;
            this.lblFromStation.Text = "起点站";
            // 
            // cmbFromStation
            // 
            this.cmbFromStation.FormattingEnabled = true;
            this.cmbFromStation.Location = new System.Drawing.Point(103, 20);
            this.cmbFromStation.Name = "cmbFromStation";
            this.cmbFromStation.Size = new System.Drawing.Size(121, 20);
            this.cmbFromStation.TabIndex = 8;
            // 
            // lstSchedule
            // 
            this.lstSchedule.Location = new System.Drawing.Point(475, 68);
            this.lstSchedule.Name = "lstSchedule";
            this.lstSchedule.Size = new System.Drawing.Size(329, 140);
            this.lstSchedule.TabIndex = 9;
            this.lstSchedule.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(413, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "计划";
            // 
            // lblCarType
            // 
            this.lblCarType.AutoSize = true;
            this.lblCarType.Location = new System.Drawing.Point(47, 236);
            this.lblCarType.Name = "lblCarType";
            this.lblCarType.Size = new System.Drawing.Size(29, 12);
            this.lblCarType.TabIndex = 11;
            this.lblCarType.Text = "车次";
            // 
            // ckAll
            // 
            this.ckAll.AutoSize = true;
            this.ckAll.Location = new System.Drawing.Point(103, 236);
            this.ckAll.Name = "ckAll";
            this.ckAll.Size = new System.Drawing.Size(48, 16);
            this.ckAll.TabIndex = 12;
            this.ckAll.Text = "全部";
            this.ckAll.UseVisualStyleBackColor = true;
            // 
            // ckZCar
            // 
            this.ckZCar.AutoSize = true;
            this.ckZCar.Location = new System.Drawing.Point(157, 236);
            this.ckZCar.Name = "ckZCar";
            this.ckZCar.Size = new System.Drawing.Size(78, 16);
            this.ckZCar.TabIndex = 13;
            this.ckZCar.Text = "直达（Z）";
            this.ckZCar.UseVisualStyleBackColor = true;
            // 
            // ckTCar
            // 
            this.ckTCar.AutoSize = true;
            this.ckTCar.Location = new System.Drawing.Point(241, 236);
            this.ckTCar.Name = "ckTCar";
            this.ckTCar.Size = new System.Drawing.Size(78, 16);
            this.ckTCar.TabIndex = 14;
            this.ckTCar.Text = "特快（T）";
            this.ckTCar.UseVisualStyleBackColor = true;
            // 
            // ckKCar
            // 
            this.ckKCar.AutoSize = true;
            this.ckKCar.Location = new System.Drawing.Point(325, 236);
            this.ckKCar.Name = "ckKCar";
            this.ckKCar.Size = new System.Drawing.Size(78, 16);
            this.ckKCar.TabIndex = 15;
            this.ckKCar.Text = "普快（K）";
            this.ckKCar.UseVisualStyleBackColor = true;
            // 
            // ckDCar
            // 
            this.ckDCar.AutoSize = true;
            this.ckDCar.Location = new System.Drawing.Point(415, 236);
            this.ckDCar.Name = "ckDCar";
            this.ckDCar.Size = new System.Drawing.Size(78, 16);
            this.ckDCar.TabIndex = 16;
            this.ckDCar.Text = "动车（D）";
            this.ckDCar.UseVisualStyleBackColor = true;
            // 
            // ckGCar
            // 
            this.ckGCar.AutoSize = true;
            this.ckGCar.Location = new System.Drawing.Point(514, 236);
            this.ckGCar.Name = "ckGCar";
            this.ckGCar.Size = new System.Drawing.Size(78, 16);
            this.ckGCar.TabIndex = 17;
            this.ckGCar.Text = "高铁（G）";
            this.ckGCar.UseVisualStyleBackColor = true;
            // 
            // MoniterTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 597);
            this.Controls.Add(this.ckGCar);
            this.Controls.Add(this.ckDCar);
            this.Controls.Add(this.ckKCar);
            this.Controls.Add(this.ckTCar);
            this.Controls.Add(this.ckZCar);
            this.Controls.Add(this.ckAll);
            this.Controls.Add(this.lblCarType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstSchedule);
            this.Controls.Add(this.cmbFromStation);
            this.Controls.Add(this.lblFromStation);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.lblTicketDate);
            this.Controls.Add(this.lblToStation);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lstContact);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.btnQueryTicket);
            this.Name = "MoniterTicket";
            this.Text = "MoniterTicket";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQueryTicket;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ListView lstContact;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblToStation;
        private System.Windows.Forms.Label lblTicketDate;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblFromStation;
        private System.Windows.Forms.ComboBox cmbFromStation;
        private System.Windows.Forms.ListView lstSchedule;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCarType;
        private System.Windows.Forms.CheckBox ckAll;
        private System.Windows.Forms.CheckBox ckZCar;
        private System.Windows.Forms.CheckBox ckTCar;
        private System.Windows.Forms.CheckBox ckKCar;
        private System.Windows.Forms.CheckBox ckDCar;
        private System.Windows.Forms.CheckBox ckGCar;
    }
}