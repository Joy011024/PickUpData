namespace DataHelpWinform
{
    partial class SetTimeIntrevalWin
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
            this.setTimeInterval = new DataHelpWinform.SetTimeInterval();
            this.SuspendLayout();
            // 
            // setTimeInterval
            // 
            this.setTimeInterval.Location = new System.Drawing.Point(1, -2);
            this.setTimeInterval.Name = "setTimeInterval";
            this.setTimeInterval.SetIntervalCallBack = null;
            this.setTimeInterval.Size = new System.Drawing.Size(247, 104);
            this.setTimeInterval.TabIndex = 0;
            this.setTimeInterval.Load += new System.EventHandler(this.setTimeInterval_Load);
            // 
            // SetTimeIntrevalWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 96);
            this.Controls.Add(this.setTimeInterval);
            this.Name = "SetTimeIntrevalWin";
            this.Text = "SetTimeIntrevalWin";
            this.ResumeLayout(false);

        }

        #endregion

        private SetTimeInterval setTimeInterval;

    }
}