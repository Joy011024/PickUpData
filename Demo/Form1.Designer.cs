namespace Demo
{
    partial class Form1
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
            this.lstNic = new System.Windows.Forms.ListView();
            this.a = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.b = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.c = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.d = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lstNic
            // 
            this.lstNic.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.a,
            this.b,
            this.c,
            this.d});
            this.lstNic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNic.Location = new System.Drawing.Point(0, 0);
            this.lstNic.Name = "lstNic";
            this.lstNic.Size = new System.Drawing.Size(467, 253);
            this.lstNic.TabIndex = 0;
            this.lstNic.UseCompatibleStateImageBehavior = false;
            this.lstNic.View = System.Windows.Forms.View.Details;
            // 
            // a
            // 
            this.a.DisplayIndex = 1;
            this.a.Text = "别名";
            this.a.Width = 200;
            // 
            // b
            // 
            this.b.DisplayIndex = 0;
            this.b.Text = "描述";
            this.b.Width = 200;
            // 
            // c
            // 
            this.c.Text = "发送";
            // 
            // d
            // 
            this.d.Text = "接收";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 253);
            this.Controls.Add(this.lstNic);
            this.Name = "Form1";
            this.Text = "C# Monitor NIC Traffic.";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstNic;
        private System.Windows.Forms.ColumnHeader a;
        private System.Windows.Forms.ColumnHeader b;
        private System.Windows.Forms.ColumnHeader c;
        private System.Windows.Forms.ColumnHeader d;
    }
}

