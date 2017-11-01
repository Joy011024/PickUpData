using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FeatureFrmList;
using Domain.MainContextData;
using Domain.CommonData;
namespace NPKVersionNote
{
    public partial class NPKManage : Form
    {
        enum ButtonAction 
        {
            Submit=1,
            Reset=2,
            Read=3,
            UserTimeSpan=4
        }
        void Test() 
        {
           
        }
        public NPKManage()
        {
            InitializeComponent();
            InitEleDataSource();
            BindEleTagWithEvent();
            Test();
        }
        void InitEleDataSource()
        {
            if (ConfigItem.NPKCategoryFromDB)
            {

            }
            else
            {
                cmbNpkCategory.BindDataSource(ENPKCategory.DBStruct);
            }
            if (cmbNpkCategory.Items.Count > 0) 
            {
                cmbNpkCategory.SelectedIndex = 0;
            }
        }
        void BindEleTagWithEvent() 
        {
            btnReadNpk.Tag = ButtonAction.Read.ToString();
            btnSubmitNPK.Tag = ButtonAction.Submit.ToString();
            btnResetNPK.Tag = ButtonAction.Reset.ToString();
            btnUserTimeSpan.Tag = ButtonAction.UserTimeSpan.ToString();
            EventHandler e = new EventHandler(Button_Click); 
            btnResetNPK.Click += e;
            btnSubmitNPK.Click += e;
            btnReadNpk.Click += e;
            btnUserTimeSpan.Click += e;
        }
        private void Button_Click(object sender, EventArgs e) 
        {
            Button btn = sender as Button;
            ButtonAction ba;
            string tag = btn.Tag as string;
            Enum.TryParse(tag, out ba);
            switch (ba)
            {
                case ButtonAction.Submit:
                    Upload();
                    break;
                case ButtonAction.Reset:
                    break;
                case ButtonAction.Read:
                    ReadTextFromFile(selectFile1.SelectFileFullName);
                    break;
                case ButtonAction.UserTimeSpan:
                    break;
            }

        }
        private void ReadTextFromFile(string file) 
        {
            string text = FileHelper.ReadFile(file);
            rtbNPKCmd.Text = text;
        }
        public void Upload() 
        {
            string path = selectFile1.SelectFileFullName;
            FileUploadLinkServer ls = new FileUploadLinkServer();
            ls.UploadFile(path, ConfigItem.NpkServiceUrl + ConfigItem.SubmitNpkFileUrl, string.Empty);
        }
    }
}
