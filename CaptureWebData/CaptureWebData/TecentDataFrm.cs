using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain.CommonData;
using ApplicationService.IPDataService;
using DataHelp;
namespace CaptureWebData
{
    public partial class TecentDataFrm : Form
    {
        string NoLimit="不限";
        enum ComboboxItem 
        {
            Name=1,
            Code=2
        }
        enum ComboboxDictItem
        {
            Key = 1,
            Value = 2
        }
        string Cookie { get; set; }
        List<NodeData> cityList { get; set; }
        public TecentDataFrm()
        {
            InitializeComponent();
        }

        private void TecentDataFrm_Load(object sender, EventArgs e)
        {
            Init();
        }
        void Init()
        {
            Dictionary<string, object> fields = EGender.Men.GetEnumFieldAttributeDict("DescriptionAttribute", "Description");
            BindComboBoxDict(fields,cmbGender);
            pickUpIEWebCookie.CallBack = CallBack;
            CategoryDataService cds = new CategoryDataService(new ConfigurationItems().TecentDA);
            IEnumerable<NodeData> city = cds.QueryCityCategory();
            cityList = city.ToList();
            cityList.Add(new NodeData() { Name = "不限" });
            BindProvince();
            GetProcessPath();
        }
        void GetProcessPath() 
        {
            ConfigurationItems config=new ConfigurationItems();
            rtbTip.Text = config.GetNowAssembly();//config.GetProcessPath();
            this.Text += config.JoinProcessPath();
        }
        void CallBack(object cookie) 
        {
            Cookie =cookie as string ;
        }
        void BindComboBoxDict(Dictionary<string, object> fields,ComboBox cmb) 
        {
            foreach (KeyValuePair<string, object> item in fields)
            {
                cmb.Items.Add(item);
            }
            cmb.DisplayMember = ComboboxDictItem.Value.ToString();
            cmb.ValueMember = ComboboxDictItem.Key.ToString();
            cmb.SelectedIndex = 0;
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
           // 
            int interval = 0;
            string inter = txtTimeSpan.Text;
            int.TryParse(inter, out interval);
            int repeact = 0;
            string rep = txtRepeact.Text;
            int.TryParse(rep, out repeact);
            LoggerWriter.CreateLogFile(Cookie, (new ConfigurationItems()).LogPath+(new QQDataDA().GeneratePathTimeSpan(Cookie)),ELogType.SessionOrCookieLog);
            QuartzJob job = new QuartzJob();
            QueryQQParam param = new QueryQQParam();
            NodeData pro = (NodeData)cmbProvince.SelectedItem;
            param.province = pro.Code;
            NodeData city = (NodeData)cmbCity.SelectedItem;
            param.city = city.Code;
            int limit = 30;
            if (int.TryParse(txtCurrentLimit.Text,out limit)) 
            {
                param.num = limit;
            }
            param.district = string.Empty;
            string obj = ((KeyValuePair<string,object>)cmbGender.SelectedItem).Key as string ;
            EGender gender;
            Enum.TryParse(obj, out gender);
            param.sex = gender.GetHashCode();
            if (ckStartQuartz.Checked)
            {
                DelegateData.BaseDelegate del = QuartzCallBack;
                job.CreateJobWithParam<JobAction<QQDataDA>>(new object[] { Cookie, param, del }, DateTime.Now, interval, repeact);
            }
            else
            {
                QQDataDA da = new QQDataDA();
                da.QueryParam = param;
                da.QueryQQData(Cookie);
            }
        }

        private void webBrowserData_Load(object sender, EventArgs e)
        {

        }
        void BindProvince() 
        {
            BindComboBox("1", cmbProvince);
        }

        private void cmbProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            NodeData node=(NodeData) cmb.SelectedItem;
            BindComboBox(node.Code,cmbCity);
        }
        void BindComboBox(string parentCode,ComboBox cmb) 
        {
            List<NodeData> nodes = cityList.Where(c => c.ParentCode == parentCode || string.IsNullOrEmpty(c.Code)).OrderBy(t => t.Code).ToList();
            nodes.Add(new NodeData() { Name = "不限" });
            cmb.DataSource = nodes;
            cmb.DisplayMember = ComboboxItem.Name.ToString();
            cmb.ValueMember = ComboboxItem.Code.ToString();
        }
       
        private void QuartzCallBack(object data) 
        {
            if (rtbTip.InvokeRequired)
            {//其他进程调度
                DelegateData.BaseDelegate bd = new DelegateData.BaseDelegate(QuartzCallBack);
                this.Invoke(bd,data);
                return;
            }
            PickUpQQDoResponse res = data as PickUpQQDoResponse;
            if (res == null) { return; }
            FindQQResponse qqres = res.responseData;
            if (qqres != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("code\t" + qqres.retcode);
                sb.AppendLine("DB Total\t" + qqres.result.buddy.totalnum);
                sb.AppendLine("now count\t" + qqres.result.buddy.info_list.Count);//info_list 可能出现为null的情况
                rtbTip.Text = sb.ToString();
            }
            else
            {

                rtbTip.Text = "Error\r\n" + res.responseJson;
            }
        }

        private void btnDeleteQuartz_Click(object sender, EventArgs e)
        {
            QuartzJob job = new QuartzJob();
            job.DeleteJob<JobAction<QQDataDA>>();
        }
    }
}
