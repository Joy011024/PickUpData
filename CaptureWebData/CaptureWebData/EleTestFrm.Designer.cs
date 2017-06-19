namespace CaptureWebData
{
    partial class EleTestFrm
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
            this.webKitBrowserData1 = new SelfControlForm.WebKitBrowserData();
            this.SuspendLayout();
            // 
            // webKitBrowserData1
            // 
            this.webKitBrowserData1.Location = new System.Drawing.Point(12, -8);
            this.webKitBrowserData1.Name = "webKitBrowserData1";
            this.webKitBrowserData1.Size = new System.Drawing.Size(782, 620);
            this.webKitBrowserData1.TabIndex = 0;
            // 
            // EleTestFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 626);
            this.Controls.Add(this.webKitBrowserData1);
            this.Name = "EleTestFrm";
            this.Text = "EleTestFrm";
            this.ResumeLayout(false);

        }

        #endregion

        private SelfControlForm.WebKitBrowserData webKitBrowserData1;

    }
}