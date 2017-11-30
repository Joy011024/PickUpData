namespace CaptureManage.AppWin
{
    partial class CitySubwayFrm
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
            this.btnQuerySubwayData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnQuerySubwayData
            // 
            this.btnQuerySubwayData.Location = new System.Drawing.Point(12, 23);
            this.btnQuerySubwayData.Name = "btnQuerySubwayData";
            this.btnQuerySubwayData.Size = new System.Drawing.Size(105, 23);
            this.btnQuerySubwayData.TabIndex = 0;
            this.btnQuerySubwayData.Text = "地铁数据采集";
            this.btnQuerySubwayData.UseVisualStyleBackColor = true;
            // 
            // CitySubwayFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 251);
            this.Controls.Add(this.btnQuerySubwayData);
            this.Name = "CitySubwayFrm";
            this.Text = "地铁站数据采集";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnQuerySubwayData;
    }
}