namespace CaptureWebData
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tspHeadTool = new System.Windows.Forms.ToolStrip();
            this.tspHeadBtnSplider = new System.Windows.Forms.ToolStripSplitButton();
            this.tspMiUin = new System.Windows.Forms.ToolStripMenuItem();
            this.tspMiMaimai = new System.Windows.Forms.ToolStripMenuItem();
            this.tspSBNews = new System.Windows.Forms.ToolStripSplitButton();
            this.tspHeadTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // tspHeadTool
            // 
            this.tspHeadTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspHeadBtnSplider,
            this.tspSBNews});
            this.tspHeadTool.Location = new System.Drawing.Point(0, 0);
            this.tspHeadTool.Name = "tspHeadTool";
            this.tspHeadTool.Size = new System.Drawing.Size(998, 25);
            this.tspHeadTool.TabIndex = 0;
            this.tspHeadTool.Text = "toolStrip1";
            // 
            // tspHeadBtnSplider
            // 
            this.tspHeadBtnSplider.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tspHeadBtnSplider.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspMiUin,
            this.tspMiMaimai});
            this.tspHeadBtnSplider.Image = ((System.Drawing.Image)(resources.GetObject("tspHeadBtnSplider.Image")));
            this.tspHeadBtnSplider.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tspHeadBtnSplider.Name = "tspHeadBtnSplider";
            this.tspHeadBtnSplider.Size = new System.Drawing.Size(53, 22);
            this.tspHeadBtnSplider.Text = "Main";
            // 
            // tspMiUin
            // 
            this.tspMiUin.Name = "tspMiUin";
            this.tspMiUin.Size = new System.Drawing.Size(152, 22);
            this.tspMiUin.Tag = "CaptureWebData.TecentDataFrm";
            this.tspMiUin.Text = "Uin";
            // 
            // tspMiMaimai
            // 
            this.tspMiMaimai.Name = "tspMiMaimai";
            this.tspMiMaimai.Size = new System.Drawing.Size(152, 22);
            this.tspMiMaimai.Tag = "CaptureWebData.MaimaiFrm";
            this.tspMiMaimai.Text = "Maimai";
            // 
            // tspSBNews
            // 
            this.tspSBNews.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tspSBNews.Image = ((System.Drawing.Image)(resources.GetObject("tspSBNews.Image")));
            this.tspSBNews.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tspSBNews.Name = "tspSBNews";
            this.tspSBNews.Size = new System.Drawing.Size(56, 22);
            this.tspSBNews.Text = "News";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 687);
            this.Controls.Add(this.tspHeadTool);
            this.Name = "Main";
            this.Text = "Main";
            this.tspHeadTool.ResumeLayout(false);
            this.tspHeadTool.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspHeadTool;
        private System.Windows.Forms.ToolStripSplitButton tspHeadBtnSplider;
        private System.Windows.Forms.ToolStripMenuItem tspMiUin;
        private System.Windows.Forms.ToolStripMenuItem tspMiMaimai;
        private System.Windows.Forms.ToolStripSplitButton tspSBNews;
    }
}