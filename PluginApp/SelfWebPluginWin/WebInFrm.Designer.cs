namespace SelfWebPluginWin
{
    partial class WebInFrm
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
            this.mBodyPanel = new System.Windows.Forms.Panel();
            this.mBtnBrowser = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mUrlPanel = new System.Windows.Forms.Panel();
            this.mUrlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mBodyPanel
            // 
            this.mBodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mBodyPanel.Location = new System.Drawing.Point(0, 0);
            this.mBodyPanel.Name = "mBodyPanel";
            this.mBodyPanel.Size = new System.Drawing.Size(1008, 604);
            this.mBodyPanel.TabIndex = 0;
            // 
            // mBtnBrowser
            // 
            this.mBtnBrowser.Location = new System.Drawing.Point(911, 9);
            this.mBtnBrowser.Name = "mBtnBrowser";
            this.mBtnBrowser.Size = new System.Drawing.Size(75, 23);
            this.mBtnBrowser.TabIndex = 1;
            this.mBtnBrowser.Text = "浏览";
            this.mBtnBrowser.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(61, 9);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(724, 21);
            this.textBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "网址";
            // 
            // mUrlPanel
            // 
            this.mUrlPanel.Controls.Add(this.mBtnBrowser);
            this.mUrlPanel.Controls.Add(this.label1);
            this.mUrlPanel.Controls.Add(this.textBox1);
            this.mUrlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mUrlPanel.Location = new System.Drawing.Point(0, 0);
            this.mUrlPanel.Name = "mUrlPanel";
            this.mUrlPanel.Size = new System.Drawing.Size(1008, 42);
            this.mUrlPanel.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 604);
            this.Controls.Add(this.mUrlPanel);
            this.Controls.Add(this.mBodyPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.mUrlPanel.ResumeLayout(false);
            this.mUrlPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mBodyPanel;
        private System.Windows.Forms.Button mBtnBrowser;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel mUrlPanel;
    }
}

