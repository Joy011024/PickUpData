namespace CaptureManage.AppWin
{
    partial class PicpUpWebHtmlFrm
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
            this.htmlPanel = new System.Windows.Forms.Panel();
            this.LogicPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // htmlPanel
            // 
            this.htmlPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.htmlPanel.Location = new System.Drawing.Point(0, 0);
            this.htmlPanel.Name = "htmlPanel";
            this.htmlPanel.Size = new System.Drawing.Size(696, 493);
            this.htmlPanel.TabIndex = 0;
            // 
            // LogicPanel
            // 
            this.LogicPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.LogicPanel.Location = new System.Drawing.Point(702, 0);
            this.LogicPanel.Name = "LogicPanel";
            this.LogicPanel.Size = new System.Drawing.Size(288, 493);
            this.LogicPanel.TabIndex = 1;
            // 
            // PicpUpWebHtmlFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 493);
            this.Controls.Add(this.LogicPanel);
            this.Controls.Add(this.htmlPanel);
            this.Name = "PicpUpWebHtmlFrm";
            this.Text = "PicpUpWebHtmlFrm";
            this.Load += new System.EventHandler(this.PicpUpWebHtmlFrm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel htmlPanel;
        private System.Windows.Forms.Panel LogicPanel;
    }
}