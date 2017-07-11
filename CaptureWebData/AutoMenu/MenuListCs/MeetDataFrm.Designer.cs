namespace MenuListCs
{
    partial class MeetDataFrm
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
            this.selectHeadImage1 = new FeatureFrmList.SelectHeadImage();
            this.userContact1 = new FeatureFrmList.UserContact();
            this.SuspendLayout();
            // 
            // selectHeadImage1
            // 
            this.selectHeadImage1.Location = new System.Drawing.Point(285, 3);
            this.selectHeadImage1.Name = "selectHeadImage1";
            this.selectHeadImage1.Size = new System.Drawing.Size(172, 217);
            this.selectHeadImage1.TabIndex = 1;
            // 
            // userContact1
            // 
            this.userContact1.Location = new System.Drawing.Point(1, 3);
            this.userContact1.Name = "userContact1";
            this.userContact1.Size = new System.Drawing.Size(293, 238);
            this.userContact1.TabIndex = 0;
            // 
            // MeetDataFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 316);
            this.Controls.Add(this.selectHeadImage1);
            this.Controls.Add(this.userContact1);
            this.Name = "MeetDataFrm";
            this.Tag = "Meet";
            this.Text = "MeetDataFrm";
            this.ResumeLayout(false);

        }

        #endregion

        private FeatureFrmList.UserContact userContact1;
        private FeatureFrmList.SelectHeadImage selectHeadImage1;
    }
}