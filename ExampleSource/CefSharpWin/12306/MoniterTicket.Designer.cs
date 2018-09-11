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
            this.listView1 = new System.Windows.Forms.ListView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblToStation = new System.Windows.Forms.Label();
            this.lblTicketDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnQueryTicket
            // 
            this.btnQueryTicket.Location = new System.Drawing.Point(453, 9);
            this.btnQueryTicket.Name = "btnQueryTicket";
            this.btnQueryTicket.Size = new System.Drawing.Size(65, 50);
            this.btnQueryTicket.TabIndex = 0;
            this.btnQueryTicket.Text = "车票查询";
            this.btnQueryTicket.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(179, 38);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(121, 21);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(95, 72);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(443, 243);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(179, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 3;
            // 
            // lblToStation
            // 
            this.lblToStation.AutoSize = true;
            this.lblToStation.Location = new System.Drawing.Point(93, 9);
            this.lblToStation.Name = "lblToStation";
            this.lblToStation.Size = new System.Drawing.Size(29, 12);
            this.lblToStation.TabIndex = 4;
            this.lblToStation.Text = "到站";
            // 
            // lblTicketDate
            // 
            this.lblTicketDate.AutoSize = true;
            this.lblTicketDate.Location = new System.Drawing.Point(93, 44);
            this.lblTicketDate.Name = "lblTicketDate";
            this.lblTicketDate.Size = new System.Drawing.Size(53, 12);
            this.lblTicketDate.TabIndex = 5;
            this.lblTicketDate.Text = "乘车日期";
            // 
            // MoniterTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 481);
            this.Controls.Add(this.lblTicketDate);
            this.Controls.Add(this.lblToStation);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.listView1);
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
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblToStation;
        private System.Windows.Forms.Label lblTicketDate;
    }
}