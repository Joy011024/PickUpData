namespace CaptureWebData
{
    partial class HttpFrm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbRequestMethod = new System.Windows.Forms.ComboBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblRequestMethod = new System.Windows.Forms.Label();
            this.lblUrlDesc = new System.Windows.Forms.Label();
            this.rtbUrlDesc = new System.Windows.Forms.RichTextBox();
            this.lblUrlKey = new System.Windows.Forms.Label();
            this.txtWebKey = new System.Windows.Forms.TextBox();
            this.txtWebName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblParamList = new System.Windows.Forms.Label();
            this.rtbParamList = new System.Windows.Forms.RichTextBox();
            this.lblCookie = new System.Windows.Forms.Label();
            this.rtbCookie = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(106, 466);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Tag = "PickUpData";
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.Button_Clicke);
            // 
            // cmbRequestMethod
            // 
            this.cmbRequestMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRequestMethod.FormattingEnabled = true;
            this.cmbRequestMethod.Location = new System.Drawing.Point(98, 47);
            this.cmbRequestMethod.Name = "cmbRequestMethod";
            this.cmbRequestMethod.Size = new System.Drawing.Size(295, 20);
            this.cmbRequestMethod.TabIndex = 1;
            this.cmbRequestMethod.Tag = "RequestMethod";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(12, 12);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(47, 12);
            this.lblUrl.TabIndex = 2;
            this.lblUrl.Text = "Url地址";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(98, 9);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(295, 21);
            this.txtUrl.TabIndex = 3;
            this.txtUrl.Tag = "Url";
            // 
            // lblRequestMethod
            // 
            this.lblRequestMethod.AutoSize = true;
            this.lblRequestMethod.Location = new System.Drawing.Point(12, 50);
            this.lblRequestMethod.Name = "lblRequestMethod";
            this.lblRequestMethod.Size = new System.Drawing.Size(53, 12);
            this.lblRequestMethod.TabIndex = 4;
            this.lblRequestMethod.Text = "请求方式";
            // 
            // lblUrlDesc
            // 
            this.lblUrlDesc.AutoSize = true;
            this.lblUrlDesc.Location = new System.Drawing.Point(12, 116);
            this.lblUrlDesc.Name = "lblUrlDesc";
            this.lblUrlDesc.Size = new System.Drawing.Size(47, 12);
            this.lblUrlDesc.TabIndex = 5;
            this.lblUrlDesc.Text = "Url用途";
            // 
            // rtbUrlDesc
            // 
            this.rtbUrlDesc.Location = new System.Drawing.Point(98, 87);
            this.rtbUrlDesc.Name = "rtbUrlDesc";
            this.rtbUrlDesc.Size = new System.Drawing.Size(295, 89);
            this.rtbUrlDesc.TabIndex = 6;
            this.rtbUrlDesc.Tag = "UrlDesc";
            this.rtbUrlDesc.Text = "";
            // 
            // lblUrlKey
            // 
            this.lblUrlKey.AutoSize = true;
            this.lblUrlKey.Location = new System.Drawing.Point(12, 394);
            this.lblUrlKey.Name = "lblUrlKey";
            this.lblUrlKey.Size = new System.Drawing.Size(47, 12);
            this.lblUrlKey.TabIndex = 7;
            this.lblUrlKey.Text = "Url标识";
            // 
            // txtWebKey
            // 
            this.txtWebKey.Location = new System.Drawing.Point(92, 391);
            this.txtWebKey.Name = "txtWebKey";
            this.txtWebKey.Size = new System.Drawing.Size(295, 21);
            this.txtWebKey.TabIndex = 8;
            this.txtWebKey.Tag = "UrlKey";
            // 
            // txtWebName
            // 
            this.txtWebName.Location = new System.Drawing.Point(92, 427);
            this.txtWebName.Name = "txtWebName";
            this.txtWebName.Size = new System.Drawing.Size(295, 21);
            this.txtWebName.TabIndex = 10;
            this.txtWebName.Tag = "WebName";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 430);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Url归属网站";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(272, 466);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 11;
            this.btnClear.Tag = "ClearData";
            this.btnClear.Text = "重置";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // lblParamList
            // 
            this.lblParamList.AutoSize = true;
            this.lblParamList.Location = new System.Drawing.Point(12, 213);
            this.lblParamList.Name = "lblParamList";
            this.lblParamList.Size = new System.Drawing.Size(47, 12);
            this.lblParamList.TabIndex = 12;
            this.lblParamList.Text = "Url参数";
            // 
            // rtbParamList
            // 
            this.rtbParamList.Location = new System.Drawing.Point(98, 191);
            this.rtbParamList.Name = "rtbParamList";
            this.rtbParamList.Size = new System.Drawing.Size(295, 89);
            this.rtbParamList.TabIndex = 13;
            this.rtbParamList.Tag = "ParamList";
            this.rtbParamList.Text = "";
            // 
            // lblCookie
            // 
            this.lblCookie.AutoSize = true;
            this.lblCookie.Location = new System.Drawing.Point(18, 318);
            this.lblCookie.Name = "lblCookie";
            this.lblCookie.Size = new System.Drawing.Size(65, 12);
            this.lblCookie.TabIndex = 14;
            this.lblCookie.Text = "附带Cookie";
            // 
            // rtbCookie
            // 
            this.rtbCookie.Location = new System.Drawing.Point(98, 286);
            this.rtbCookie.Name = "rtbCookie";
            this.rtbCookie.Size = new System.Drawing.Size(295, 89);
            this.rtbCookie.TabIndex = 15;
            this.rtbCookie.Tag = "Cookie";
            this.rtbCookie.Text = "";
            // 
            // HttpFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 501);
            this.Controls.Add(this.rtbCookie);
            this.Controls.Add(this.lblCookie);
            this.Controls.Add(this.rtbParamList);
            this.Controls.Add(this.lblParamList);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtWebName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtWebKey);
            this.Controls.Add(this.lblUrlKey);
            this.Controls.Add(this.rtbUrlDesc);
            this.Controls.Add(this.lblUrlDesc);
            this.Controls.Add(this.lblRequestMethod);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.cmbRequestMethod);
            this.Controls.Add(this.btnSave);
            this.Name = "HttpFrm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbRequestMethod;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label lblRequestMethod;
        private System.Windows.Forms.Label lblUrlDesc;
        private System.Windows.Forms.RichTextBox rtbUrlDesc;
        private System.Windows.Forms.Label lblUrlKey;
        private System.Windows.Forms.TextBox txtWebKey;
        private System.Windows.Forms.TextBox txtWebName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblParamList;
        private System.Windows.Forms.RichTextBox rtbParamList;
        private System.Windows.Forms.Label lblCookie;
        private System.Windows.Forms.RichTextBox rtbCookie;
    }
}

