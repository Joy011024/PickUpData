namespace DataHelpWinform
{
    partial class ColorPickUp
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
            this.lblRGB = new System.Windows.Forms.Label();
            this.txtR = new System.Windows.Forms.TextBox();
            this.btnGenerateRGB = new System.Windows.Forms.Button();
            this.lblR = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarR = new System.Windows.Forms.TrackBar();
            this.trackBarG = new System.Windows.Forms.TrackBar();
            this.trackBarB = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtG = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRGB = new System.Windows.Forms.TextBox();
            this.panelRGB = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarB)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRGB
            // 
            this.lblRGB.AutoSize = true;
            this.lblRGB.Location = new System.Drawing.Point(4, 7);
            this.lblRGB.Name = "lblRGB";
            this.lblRGB.Size = new System.Drawing.Size(59, 12);
            this.lblRGB.TabIndex = 0;
            this.lblRGB.Text = "RGB色彩值";
            // 
            // txtR
            // 
            this.txtR.Location = new System.Drawing.Point(82, 4);
            this.txtR.MaxLength = 2;
            this.txtR.Name = "txtR";
            this.txtR.Size = new System.Drawing.Size(36, 21);
            this.txtR.TabIndex = 1;
            // 
            // btnGenerateRGB
            // 
            this.btnGenerateRGB.Location = new System.Drawing.Point(301, 147);
            this.btnGenerateRGB.Name = "btnGenerateRGB";
            this.btnGenerateRGB.Size = new System.Drawing.Size(51, 33);
            this.btnGenerateRGB.TabIndex = 3;
            this.btnGenerateRGB.Text = "提取RGB色";
            this.btnGenerateRGB.UseVisualStyleBackColor = true;
            this.btnGenerateRGB.Click += new System.EventHandler(this.Button_Click);
            // 
            // lblR
            // 
            this.lblR.AutoSize = true;
            this.lblR.Location = new System.Drawing.Point(4, 34);
            this.lblR.Name = "lblR";
            this.lblR.Size = new System.Drawing.Size(17, 12);
            this.lblR.TabIndex = 4;
            this.lblR.Text = "R:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "G:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "B:";
            // 
            // trackBarR
            // 
            this.trackBarR.Location = new System.Drawing.Point(27, 29);
            this.trackBarR.Name = "trackBarR";
            this.trackBarR.Size = new System.Drawing.Size(313, 45);
            this.trackBarR.TabIndex = 11;
            this.trackBarR.Tag = "R";
            this.trackBarR.Scroll += new System.EventHandler(this.TrackBar_Scroll);
            this.trackBarR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TrackBar_KeyPress);
            // 
            // trackBarG
            // 
            this.trackBarG.Location = new System.Drawing.Point(26, 62);
            this.trackBarG.Name = "trackBarG";
            this.trackBarG.Size = new System.Drawing.Size(313, 45);
            this.trackBarG.TabIndex = 12;
            this.trackBarG.Tag = "G";
            this.trackBarG.Scroll += new System.EventHandler(this.TrackBar_Scroll);
            this.trackBarG.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TrackBar_KeyPress);
            // 
            // trackBarB
            // 
            this.trackBarB.Location = new System.Drawing.Point(26, 102);
            this.trackBarB.Name = "trackBarB";
            this.trackBarB.Size = new System.Drawing.Size(313, 45);
            this.trackBarB.TabIndex = 13;
            this.trackBarB.Tag = "B";
            this.trackBarB.Scroll += new System.EventHandler(this.TrackBar_Scroll);
            this.trackBarB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TrackBar_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "R:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(124, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "G:";
            // 
            // txtG
            // 
            this.txtG.Location = new System.Drawing.Point(138, 4);
            this.txtG.MaxLength = 2;
            this.txtG.Name = "txtG";
            this.txtG.Size = new System.Drawing.Size(36, 21);
            this.txtG.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(180, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "B:";
            // 
            // txtB
            // 
            this.txtB.Location = new System.Drawing.Point(194, 4);
            this.txtB.MaxLength = 2;
            this.txtB.Name = "txtB";
            this.txtB.Size = new System.Drawing.Size(36, 21);
            this.txtB.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(246, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "RGB:";
            // 
            // txtRGB
            // 
            this.txtRGB.Enabled = false;
            this.txtRGB.Location = new System.Drawing.Point(271, 3);
            this.txtRGB.Name = "txtRGB";
            this.txtRGB.Size = new System.Drawing.Size(59, 21);
            this.txtRGB.TabIndex = 20;
            // 
            // panelRGB
            // 
            this.panelRGB.BackColor = System.Drawing.Color.CadetBlue;
            this.panelRGB.Location = new System.Drawing.Point(6, 153);
            this.panelRGB.Name = "panelRGB";
            this.panelRGB.Size = new System.Drawing.Size(282, 27);
            this.panelRGB.TabIndex = 2;
            // 
            // ColorPickUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtRGB);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtG);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarB);
            this.Controls.Add(this.trackBarG);
            this.Controls.Add(this.trackBarR);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblR);
            this.Controls.Add(this.btnGenerateRGB);
            this.Controls.Add(this.panelRGB);
            this.Controls.Add(this.txtR);
            this.Controls.Add(this.lblRGB);
            this.Name = "ColorPickUp";
            this.Size = new System.Drawing.Size(355, 193);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRGB;
        private System.Windows.Forms.TextBox txtR;
        private System.Windows.Forms.Button btnGenerateRGB;
        private System.Windows.Forms.Label lblR;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBarR;
        private System.Windows.Forms.TrackBar trackBarG;
        private System.Windows.Forms.TrackBar trackBarB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtG;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRGB;
        private System.Windows.Forms.Panel panelRGB;
    }
}
