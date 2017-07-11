namespace CodeInitMenuWin
{
    partial class MainFrm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.tsMenu = new System.Windows.Forms.ToolStrip();
            this.tsbContact = new System.Windows.Forms.ToolStripButton();
            this.tssbUl = new System.Windows.Forms.ToolStripSplitButton();
            this.fToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMenu
            // 
            this.tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbContact,
            this.tssbUl});
            this.tsMenu.Location = new System.Drawing.Point(0, 0);
            this.tsMenu.Name = "tsMenu";
            this.tsMenu.Size = new System.Drawing.Size(713, 25);
            this.tsMenu.TabIndex = 0;
            this.tsMenu.Text = "toolStrip1";
            // 
            // tsbContact
            // 
            this.tsbContact.Image = ((System.Drawing.Image)(resources.GetObject("tsbContact.Image")));
            this.tsbContact.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbContact.Name = "tsbContact";
            this.tsbContact.Size = new System.Drawing.Size(52, 22);
            this.tsbContact.Text = "联系";
            // 
            // tssbUl
            // 
            this.tssbUl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fToolStripMenuItem,
            this.sToolStripMenuItem});
            this.tssbUl.Image = ((System.Drawing.Image)(resources.GetObject("tssbUl.Image")));
            this.tssbUl.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbUl.Name = "tssbUl";
            this.tssbUl.Size = new System.Drawing.Size(64, 22);
            this.tssbUl.Text = "菜单";
            // 
            // fToolStripMenuItem
            // 
            this.fToolStripMenuItem.Name = "fToolStripMenuItem";
            this.fToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fToolStripMenuItem.Text = "f";
            // 
            // sToolStripMenuItem
            // 
            this.sToolStripMenuItem.Name = "sToolStripMenuItem";
            this.sToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sToolStripMenuItem.Text = "s";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 450);
            this.Controls.Add(this.tsMenu);
            this.Name = "MainFrm";
            this.Text = "主页";
            this.tsMenu.ResumeLayout(false);
            this.tsMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMenu;
        private System.Windows.Forms.ToolStripButton tsbContact;
        private System.Windows.Forms.ToolStripSplitButton tssbUl;
        private System.Windows.Forms.ToolStripMenuItem fToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sToolStripMenuItem;
    }
}

