namespace DataHelpWinform
{
    partial class ColorPickUpWin
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
            this.colorPickUp = new DataHelpWinform.ColorPickUp();
            this.SuspendLayout();
            // 
            // colorPickUp
            // 
            this.colorPickUp.Location = new System.Drawing.Point(13, 13);
            this.colorPickUp.Name = "colorPickUp";
            this.colorPickUp.Size = new System.Drawing.Size(355, 190);
            this.colorPickUp.TabIndex = 0;
            // 
            // ColorPickUpWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 207);
            this.Controls.Add(this.colorPickUp);
            this.Name = "ColorPickUpWin";
            this.Text = "ColorPickUpWin";
            this.Load += new System.EventHandler(this.ColorPickUpWin_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ColorPickUp colorPickUp;
    }
}