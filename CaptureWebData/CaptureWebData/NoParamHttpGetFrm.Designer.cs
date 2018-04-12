namespace CaptureWebData
{
    partial class NoParamHttpGetFrm
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
            this.rtbResponse = new System.Windows.Forms.RichTextBox();
            this.btnRequest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbUrl = new System.Windows.Forms.RichTextBox();
            this.btnWrite = new System.Windows.Forms.Button();
            this.rtbPath = new System.Windows.Forms.RichTextBox();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // rtbResponse
            // 
            this.rtbResponse.Location = new System.Drawing.Point(12, 39);
            this.rtbResponse.Name = "rtbResponse";
            this.rtbResponse.Size = new System.Drawing.Size(573, 382);
            this.rtbResponse.TabIndex = 0;
            this.rtbResponse.Text = "";
            // 
            // btnRequest
            // 
            this.btnRequest.Location = new System.Drawing.Point(510, 10);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(75, 23);
            this.btnRequest.TabIndex = 1;
            this.btnRequest.Text = "请求";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Url";
            // 
            // rtbUrl
            // 
            this.rtbUrl.Location = new System.Drawing.Point(60, 10);
            this.rtbUrl.Name = "rtbUrl";
            this.rtbUrl.Size = new System.Drawing.Size(436, 26);
            this.rtbUrl.TabIndex = 3;
            this.rtbUrl.Text = "";
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(12, 427);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 4;
            this.btnWrite.Text = "存储Text";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // rtbPath
            // 
            this.rtbPath.Location = new System.Drawing.Point(102, 427);
            this.rtbPath.Name = "rtbPath";
            this.rtbPath.Size = new System.Drawing.Size(394, 26);
            this.rtbPath.TabIndex = 5;
            this.rtbPath.Text = "";
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Location = new System.Drawing.Point(510, 430);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(75, 23);
            this.btnSelectPath.TabIndex = 6;
            this.btnSelectPath.Text = "选择路径";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // NoParamHttpGetFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 458);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.rtbPath);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.rtbUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRequest);
            this.Controls.Add(this.rtbResponse);
            this.Name = "NoParamHttpGetFrm";
            this.Text = "NoParamHttpGetFrm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbResponse;
        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtbUrl;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.RichTextBox rtbPath;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}