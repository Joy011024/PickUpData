namespace NPKVersionNote
{
    partial class NPKManage
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
            this.lblNpkPath = new System.Windows.Forms.Label();
            this.lbl = new System.Windows.Forms.Label();
            this.rtbNpkDesc = new System.Windows.Forms.RichTextBox();
            this.lblNPKDetail = new System.Windows.Forms.Label();
            this.rtbNPKCmd = new System.Windows.Forms.RichTextBox();
            this.lblNpkCategory = new System.Windows.Forms.Label();
            this.cmbNpkCategory = new System.Windows.Forms.ComboBox();
            this.NpkPanel = new System.Windows.Forms.Panel();
            this.lblEffectVersion = new System.Windows.Forms.Label();
            this.rtbEffectVersion = new System.Windows.Forms.RichTextBox();
            this.lblNpkAuthor = new System.Windows.Forms.Label();
            this.rtbNPKAuthor = new System.Windows.Forms.RichTextBox();
            this.btnSubmitNPK = new System.Windows.Forms.Button();
            this.btnResetNPK = new System.Windows.Forms.Button();
            this.btnReadNpk = new System.Windows.Forms.Button();
            this.selectFile1 = new FeatureFrmList.SelectFile();
            this.btnUserTimeSpan = new System.Windows.Forms.Button();
            this.NpkPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNpkPath
            // 
            this.lblNpkPath.AutoSize = true;
            this.lblNpkPath.Location = new System.Drawing.Point(12, 22);
            this.lblNpkPath.Name = "lblNpkPath";
            this.lblNpkPath.Size = new System.Drawing.Size(53, 12);
            this.lblNpkPath.TabIndex = 1;
            this.lblNpkPath.Text = "补丁路径";
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Location = new System.Drawing.Point(12, 185);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(53, 12);
            this.lbl.TabIndex = 2;
            this.lbl.Text = "补丁说明";
            // 
            // rtbNpkDesc
            // 
            this.rtbNpkDesc.Location = new System.Drawing.Point(84, 141);
            this.rtbNpkDesc.Name = "rtbNpkDesc";
            this.rtbNpkDesc.Size = new System.Drawing.Size(358, 124);
            this.rtbNpkDesc.TabIndex = 3;
            this.rtbNpkDesc.Text = "";
            // 
            // lblNPKDetail
            // 
            this.lblNPKDetail.AutoSize = true;
            this.lblNPKDetail.Location = new System.Drawing.Point(3, 60);
            this.lblNPKDetail.Name = "lblNPKDetail";
            this.lblNPKDetail.Size = new System.Drawing.Size(53, 12);
            this.lblNPKDetail.TabIndex = 4;
            this.lblNPKDetail.Text = "补丁内容";
            // 
            // rtbNPKCmd
            // 
            this.rtbNPKCmd.Location = new System.Drawing.Point(68, 3);
            this.rtbNPKCmd.Name = "rtbNPKCmd";
            this.rtbNPKCmd.Size = new System.Drawing.Size(352, 124);
            this.rtbNPKCmd.TabIndex = 5;
            this.rtbNPKCmd.Text = "";
            // 
            // lblNpkCategory
            // 
            this.lblNpkCategory.AutoSize = true;
            this.lblNpkCategory.Location = new System.Drawing.Point(12, 274);
            this.lblNpkCategory.Name = "lblNpkCategory";
            this.lblNpkCategory.Size = new System.Drawing.Size(53, 12);
            this.lblNpkCategory.TabIndex = 6;
            this.lblNpkCategory.Text = "补丁类型";
            // 
            // cmbNpkCategory
            // 
            this.cmbNpkCategory.FormattingEnabled = true;
            this.cmbNpkCategory.Location = new System.Drawing.Point(82, 271);
            this.cmbNpkCategory.Name = "cmbNpkCategory";
            this.cmbNpkCategory.Size = new System.Drawing.Size(356, 20);
            this.cmbNpkCategory.TabIndex = 7;
            // 
            // NpkPanel
            // 
            this.NpkPanel.Controls.Add(this.lblNPKDetail);
            this.NpkPanel.Controls.Add(this.rtbNPKCmd);
            this.NpkPanel.Location = new System.Drawing.Point(14, 305);
            this.NpkPanel.Name = "NpkPanel";
            this.NpkPanel.Size = new System.Drawing.Size(432, 136);
            this.NpkPanel.TabIndex = 8;
            // 
            // lblEffectVersion
            // 
            this.lblEffectVersion.AutoSize = true;
            this.lblEffectVersion.Location = new System.Drawing.Point(17, 61);
            this.lblEffectVersion.Name = "lblEffectVersion";
            this.lblEffectVersion.Size = new System.Drawing.Size(53, 12);
            this.lblEffectVersion.TabIndex = 9;
            this.lblEffectVersion.Text = "影响版本";
            // 
            // rtbEffectVersion
            // 
            this.rtbEffectVersion.Location = new System.Drawing.Point(84, 58);
            this.rtbEffectVersion.Name = "rtbEffectVersion";
            this.rtbEffectVersion.Size = new System.Drawing.Size(358, 31);
            this.rtbEffectVersion.TabIndex = 10;
            this.rtbEffectVersion.Text = "";
            // 
            // lblNpkAuthor
            // 
            this.lblNpkAuthor.AutoSize = true;
            this.lblNpkAuthor.Location = new System.Drawing.Point(5, 101);
            this.lblNpkAuthor.Name = "lblNpkAuthor";
            this.lblNpkAuthor.Size = new System.Drawing.Size(65, 12);
            this.lblNpkAuthor.TabIndex = 11;
            this.lblNpkAuthor.Text = "补丁建立者";
            // 
            // rtbNPKAuthor
            // 
            this.rtbNPKAuthor.Location = new System.Drawing.Point(84, 98);
            this.rtbNPKAuthor.Name = "rtbNPKAuthor";
            this.rtbNPKAuthor.Size = new System.Drawing.Size(358, 31);
            this.rtbNPKAuthor.TabIndex = 12;
            this.rtbNPKAuthor.Text = "";
            // 
            // btnSubmitNPK
            // 
            this.btnSubmitNPK.Location = new System.Drawing.Point(452, 194);
            this.btnSubmitNPK.Name = "btnSubmitNPK";
            this.btnSubmitNPK.Size = new System.Drawing.Size(45, 58);
            this.btnSubmitNPK.TabIndex = 13;
            this.btnSubmitNPK.Text = "提交补丁";
            this.btnSubmitNPK.UseVisualStyleBackColor = true;
            // 
            // btnResetNPK
            // 
            this.btnResetNPK.Location = new System.Drawing.Point(452, 332);
            this.btnResetNPK.Name = "btnResetNPK";
            this.btnResetNPK.Size = new System.Drawing.Size(45, 58);
            this.btnResetNPK.TabIndex = 14;
            this.btnResetNPK.Text = "重置补丁";
            this.btnResetNPK.UseVisualStyleBackColor = true;
            // 
            // btnReadNpk
            // 
            this.btnReadNpk.Location = new System.Drawing.Point(452, 12);
            this.btnReadNpk.Name = "btnReadNpk";
            this.btnReadNpk.Size = new System.Drawing.Size(45, 44);
            this.btnReadNpk.TabIndex = 15;
            this.btnReadNpk.Text = "读取补丁";
            this.btnReadNpk.UseVisualStyleBackColor = true;
            // 
            // selectFile1
            // 
            this.selectFile1.CallBack = null;
            this.selectFile1.Location = new System.Drawing.Point(84, 12);
            this.selectFile1.Name = "selectFile1";
            this.selectFile1.Size = new System.Drawing.Size(362, 35);
            this.selectFile1.TabIndex = 16;
            // 
            // btnUserTimeSpan
            // 
            this.btnUserTimeSpan.Location = new System.Drawing.Point(452, 58);
            this.btnUserTimeSpan.Name = "btnUserTimeSpan";
            this.btnUserTimeSpan.Size = new System.Drawing.Size(45, 44);
            this.btnUserTimeSpan.TabIndex = 17;
            this.btnUserTimeSpan.Text = "采用日期戳";
            this.btnUserTimeSpan.UseVisualStyleBackColor = true;
            // 
            // NPKManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 453);
            this.Controls.Add(this.btnUserTimeSpan);
            this.Controls.Add(this.selectFile1);
            this.Controls.Add(this.btnReadNpk);
            this.Controls.Add(this.btnResetNPK);
            this.Controls.Add(this.btnSubmitNPK);
            this.Controls.Add(this.rtbNPKAuthor);
            this.Controls.Add(this.lblNpkAuthor);
            this.Controls.Add(this.rtbEffectVersion);
            this.Controls.Add(this.lblEffectVersion);
            this.Controls.Add(this.NpkPanel);
            this.Controls.Add(this.cmbNpkCategory);
            this.Controls.Add(this.lblNpkCategory);
            this.Controls.Add(this.rtbNpkDesc);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.lblNpkPath);
            this.Name = "NPKManage";
            this.Text = "Form1";
            this.NpkPanel.ResumeLayout(false);
            this.NpkPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNpkPath;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.RichTextBox rtbNpkDesc;
        private System.Windows.Forms.Label lblNPKDetail;
        private System.Windows.Forms.RichTextBox rtbNPKCmd;
        private System.Windows.Forms.Label lblNpkCategory;
        private System.Windows.Forms.ComboBox cmbNpkCategory;
        private System.Windows.Forms.Panel NpkPanel;
        private System.Windows.Forms.Label lblEffectVersion;
        private System.Windows.Forms.RichTextBox rtbEffectVersion;
        private System.Windows.Forms.Label lblNpkAuthor;
        private System.Windows.Forms.RichTextBox rtbNPKAuthor;
        private System.Windows.Forms.Button btnSubmitNPK;
        private System.Windows.Forms.Button btnResetNPK;
        private System.Windows.Forms.Button btnReadNpk;
        private FeatureFrmList.SelectFile selectFile1;
        private System.Windows.Forms.Button btnUserTimeSpan;
    }
}

