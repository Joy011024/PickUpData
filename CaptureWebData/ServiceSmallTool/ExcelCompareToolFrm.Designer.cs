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
            this.lstCompare = new System.Windows.Forms.ListView();
            this.lstLeft = new System.Windows.Forms.ListView();
            this.lstRight = new System.Windows.Forms.ListView();
            this.btnCompare = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbNote = new System.Windows.Forms.RichTextBox();
            this.secondFile = new FeatureFrmList.SelectFile();
            this.firstFile = new FeatureFrmList.SelectFile();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnLoadExcel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstCompare
            // 
            this.lstCompare.Location = new System.Drawing.Point(292, 119);
            this.lstCompare.Name = "lstCompare";
            this.lstCompare.Size = new System.Drawing.Size(381, 181);
            this.lstCompare.TabIndex = 0;
            this.lstCompare.UseCompatibleStateImageBehavior = false;
            // 
            // lstLeft
            // 
            this.lstLeft.Location = new System.Drawing.Point(23, 119);
            this.lstLeft.Name = "lstLeft";
            this.lstLeft.Size = new System.Drawing.Size(211, 181);
            this.lstLeft.TabIndex = 1;
            this.lstLeft.UseCompatibleStateImageBehavior = false;
            // 
            // lstRight
            // 
            this.lstRight.Location = new System.Drawing.Point(718, 119);
            this.lstRight.Name = "lstRight";
            this.lstRight.Size = new System.Drawing.Size(226, 181);
            this.lstRight.TabIndex = 2;
            this.lstRight.UseCompatibleStateImageBehavior = false;
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(420, 323);
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
            this.label2.Location = new System.Drawing.Point(505, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "第二份Excel";
            // 
            // rtbNote
            // 
            this.rtbNote.Location = new System.Drawing.Point(23, 367);
            this.rtbNote.Name = "rtbNote";
            this.rtbNote.Size = new System.Drawing.Size(921, 96);
            this.rtbNote.TabIndex = 8;
            this.rtbNote.Text = "";
            // 
            // secondFile
            // 
            this.secondFile.CallBack = null;
            this.secondFile.Location = new System.Drawing.Point(582, 12);
            this.secondFile.Name = "secondFile";
            this.secondFile.Size = new System.Drawing.Size(362, 35);
            this.secondFile.TabIndex = 4;
            // 
            // firstFile
            // 
            this.firstFile.CallBack = null;
            this.firstFile.Location = new System.Drawing.Point(118, 12);
            this.firstFile.Name = "firstFile";
            this.firstFile.Size = new System.Drawing.Size(362, 35);
            this.firstFile.TabIndex = 3;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(23, 338);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnLoadExcel
            // 
            this.btnLoadExcel.Location = new System.Drawing.Point(420, 70);
            this.btnLoadExcel.Name = "btnLoadExcel";
            this.btnLoadExcel.Size = new System.Drawing.Size(75, 23);
            this.btnLoadExcel.TabIndex = 10;
            this.btnLoadExcel.Text = "加载Excel";
            this.btnLoadExcel.UseVisualStyleBackColor = true;
            // 
            // ExcelCompareToolFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 475);
            this.Controls.Add(this.btnLoadExcel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.rtbNote);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.secondFile);
            this.Controls.Add(this.firstFile);
            this.Controls.Add(this.lstRight);
            this.Controls.Add(this.lstLeft);
            this.Controls.Add(this.lstCompare);
            this.Name = "ExcelCompareToolFrm";
            this.Text = "ExcelToolFrm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstCompare;
        private System.Windows.Forms.ListView lstLeft;
        private System.Windows.Forms.ListView lstRight;
        private FeatureFrmList.SelectFile firstFile;
        private FeatureFrmList.SelectFile secondFile;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtbNote;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnLoadExcel;
    }
}