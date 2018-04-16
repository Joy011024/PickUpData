namespace ServiceSmallTool
{
    partial class ExcelCompareToolFrm
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.listView2 = new System.Windows.Forms.ListView();
            this.listView3 = new System.Windows.Forms.ListView();
            this.selectFile1 = new FeatureFrmList.SelectFile();
            this.selectFile2 = new FeatureFrmList.SelectFile();
            this.btnCompare = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(150, 119);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(198, 181);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // listView2
            // 
            this.listView2.Location = new System.Drawing.Point(23, 119);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(89, 181);
            this.listView2.TabIndex = 1;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // listView3
            // 
            this.listView3.Location = new System.Drawing.Point(401, 119);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(84, 181);
            this.listView3.TabIndex = 2;
            this.listView3.UseCompatibleStateImageBehavior = false;
            // 
            // selectFile1
            // 
            this.selectFile1.CallBack = null;
            this.selectFile1.Location = new System.Drawing.Point(118, 12);
            this.selectFile1.Name = "selectFile1";
            this.selectFile1.Size = new System.Drawing.Size(362, 35);
            this.selectFile1.TabIndex = 3;
            // 
            // selectFile2
            // 
            this.selectFile2.CallBack = null;
            this.selectFile2.Location = new System.Drawing.Point(118, 64);
            this.selectFile2.Name = "selectFile2";
            this.selectFile2.Size = new System.Drawing.Size(362, 35);
            this.selectFile2.TabIndex = 4;
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(191, 322);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(75, 23);
            this.btnCompare.TabIndex = 5;
            this.btnCompare.Text = "比较";
            this.btnCompare.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "第一份Excel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "第二份Excel";
            // 
            // ExcelCompareToolFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 373);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.selectFile2);
            this.Controls.Add(this.selectFile1);
            this.Controls.Add(this.listView3);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Name = "ExcelCompareToolFrm";
            this.Text = "ExcelToolFrm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ListView listView3;
        private FeatureFrmList.SelectFile selectFile1;
        private FeatureFrmList.SelectFile selectFile2;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}