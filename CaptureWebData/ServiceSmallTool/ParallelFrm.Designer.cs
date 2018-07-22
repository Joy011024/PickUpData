namespace ServiceSmallTool
{
    partial class ParallelFrm
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
            this.lblParallelNum = new System.Windows.Forms.Label();
            this.txtParallelNum = new System.Windows.Forms.TextBox();
            this.btnRunParallel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblParallelNum
            // 
            this.lblParallelNum.AutoSize = true;
            this.lblParallelNum.Location = new System.Drawing.Point(36, 17);
            this.lblParallelNum.Name = "lblParallelNum";
            this.lblParallelNum.Size = new System.Drawing.Size(53, 12);
            this.lblParallelNum.TabIndex = 0;
            this.lblParallelNum.Text = "并发数目";
            // 
            // txtParallelNum
            // 
            this.txtParallelNum.Location = new System.Drawing.Point(126, 17);
            this.txtParallelNum.Name = "txtParallelNum";
            this.txtParallelNum.Size = new System.Drawing.Size(100, 21);
            this.txtParallelNum.TabIndex = 1;
            // 
            // btnRunParallel
            // 
            this.btnRunParallel.Location = new System.Drawing.Point(260, 15);
            this.btnRunParallel.Name = "btnRunParallel";
            this.btnRunParallel.Size = new System.Drawing.Size(75, 23);
            this.btnRunParallel.TabIndex = 2;
            this.btnRunParallel.Text = "执行";
            this.btnRunParallel.UseVisualStyleBackColor = true;
            // 
            // ParallelFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 63);
            this.Controls.Add(this.btnRunParallel);
            this.Controls.Add(this.txtParallelNum);
            this.Controls.Add(this.lblParallelNum);
            this.Name = "ParallelFrm";
            this.Text = "高并发工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblParallelNum;
        private System.Windows.Forms.TextBox txtParallelNum;
        private System.Windows.Forms.Button btnRunParallel;
    }
}