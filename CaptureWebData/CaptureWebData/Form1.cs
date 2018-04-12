using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataHelp;
using Common.Data;
using DataHelpWinform;
using CommonHelperEntity;
using Domain.CommonData;
namespace CaptureWebData
{
    public partial class HttpFrm : Form
    {
        enum ButtonActionTag
        {
            PickUpData = 1,
            ClearData = 2
        }
        public HttpFrm()
        {
            InitializeComponent();
            InitPageData();
        }
        public void InitPageData()
        {
            cmbRequestMethod.BindDataSource(RequestMethod.GET);
            if (cmbRequestMethod.Items.Count > 0)
            {
                cmbRequestMethod.SelectedIndex = 0;
            }
        }
        private UrlData PickUpUrlData()
        {
            PageDataHelp data = new PageDataHelp();
            UrlData url = data.GetClassFromControl<UrlData>(this.Controls, new UrlData());
            CommonFormat cf = new CommonFormat();
            string param = cf.DateTimeIntFormatString;
            if (string.IsNullOrEmpty(url.UrlKey))
            {
                url.UrlKey = param;
            }
            if (!string.IsNullOrEmpty(url.WebName) && string.IsNullOrEmpty(url.WebKey))
            {
                url.WebKey = url.WebName.ConvertSpell();
            }
            if (!string.IsNullOrEmpty(url.ParamList)) 
            {
                //string[] items = url.ParamList.Split(new string[] { "\n" },StringSplitOptions.None);
                //string ps = string.Empty;
                //foreach (string item in items)
                //{
                //    if (string.IsNullOrEmpty(item)) { continue; }
                //    string p = item.Split(':')[0];
                //    ps += p + ":@" + p+"\n";
                //}
                //url.ParamList = ps;
            }
            return url;
        }
        private void Button_Clicke(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag as string;
            ButtonActionTag bt;
            Enum.TryParse(tag, out bt);
            switch (bt)
            {
                case ButtonActionTag.PickUpData:
                    UrlData url = PickUpUrlData();
                    string fields=(new ConfigurationItems()).ValidateUrlField;
                    string[] validate = string.IsNullOrEmpty(fields) ? null : fields.Split(',');
                    bool  empty= url.ValidateEmptyField(validate);
                    break;
            }
        }
    }
}
