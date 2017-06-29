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
        QuartzJob job = new QuartzJob();
        string NoLimit="不限";
        NodeData noLimitAddress = new NodeData() { Name = "不限" };
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
            Test();
        }
        void Test() 
        {
            ProcessHelp.ForeachProcess();   
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
            cityList.Add(noLimitAddress);
            gbPollingType.Enabled = false;
            BindProvince();
            GetProcessPath();
            QueryTodayPickUp();
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

            QueryQQParam param = GetBaseQueryParam();
            if (ckStartQuartz.Checked && rbGuid.Checked) 
            {//开启随机轮询
                DelegateData.BaseDelegate del = QuartzGuidForach;
                job.CreateJobWithParam<JobDelegateFunction>(new object[] { del, param}, DateTime.Now.AddSeconds(interval), interval, repeact);
            }
            else if (ckStartQuartz.Checked && rbDepth.Checked) 
            {//该查询结果页轮询
                DelegateData.BaseDelegate del = QuartzForeachPage;
                job.CreateJobWithParam<JobDelegateFunction>(new object[] { del, null }, DateTime.Now.AddSeconds(interval), interval, repeact);
            }
            else if (ckStartQuartz.Checked)
            {
                DelegateData.BaseDelegate del = QuartzCallBack;
                job.CreateJobWithParam<JobAction<QQDataDA>>(new object[] { Cookie, param, del }, DateTime.Now, interval, repeact);
            }
            else
            {
                QQDataDA da = new QQDataDA();
                da.QueryParam = param;
                PickUpQQDoResponse response = da.QueryQQData(Cookie);
                QueryTodayPickUp();
                QuartzCallBack(response);
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
            List<NodeData> nodes=new List<NodeData>();
            if (string.IsNullOrEmpty(parentCode))
            {//没有选择省/自治区
                nodes.Add(noLimitAddress);
            }
            else
            {
                nodes = cityList.Where(c => c.ParentCode == parentCode || string.IsNullOrEmpty(c.Code)).OrderBy(t => t.Code).ToList();
            }
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
            //如果此时检测返回集合为空，但是返回状态码不是错误，需要更改检测条件【腾讯防攻击检测】

            QueryResponseAction(res);
            QueryTodayPickUp();
        }

        private void btnDeleteQuartz_Click(object sender, EventArgs e)
        {
            QuartzJob job = new QuartzJob();
            job.DeleteJob<JobAction<QQDataDA>>();
        }
        private void QueryTodayPickUp() 
        {
            PickUpStatic pc = (new QQDataDA()).TodayStatic();
            lsbStatic.Items.Clear();
            lsbStatic.Items.Add("时间戳"+DateTime.Now.ToString());
            lsbStatic.Items.Add("库\t" + pc.DBTotal);
            lsbStatic.Items.Add("库唯一\t"+pc.DBPrimaryTotal);
            lsbStatic.Items.Add( "日期\t"+pc.StaticDay);
            lsbStatic.Items.Add("今日总计\t" + pc.Total );
            lsbStatic.Items.Add("今日QQ号\t" + pc.IdTotal);
        }
        void QuartzGuidForach(object quartzParam) 
        {
            if (this.InvokeRequired) 
            {//是否通过其他形式调用
                DelegateData.BaseDelegate bd = new DelegateData.BaseDelegate(QuartzGuidForach);
                this.Invoke(bd, quartzParam);
                return;
            }
            LoggerWriter.CreateLogFile(Cookie, (new ConfigurationItems()).LogPath + (new QQDataDA().GeneratePathTimeSpan(Cookie)), ELogType.SessionOrCookieLog);
            QueryQQParam param = GetBaseQueryParam();           
            QQDataDA da = new QQDataDA();
            da.QueryParam = param;
            PickUpQQDoResponse response= da.QueryQQData(Cookie);
            QueryTodayPickUp();
            //为下一次产生随机参数
            Guid gid = Guid.NewGuid();
            Random ran = new Random(gid.GetHashCode());
            //所在城市联动
            int szd = ran.Next(0, 8);
            int ps = cmbProvince.Items.Count;
            int guidP = ran.Next(0, ps);
            cmbProvince.SelectedIndex = guidP;
            QuartzCallBack(response);
        }
        /// <summary>
        /// 提取查询qq数据使用的参数
        /// </summary>
        /// <returns></returns>
        QueryQQParam GetBaseQueryParam() 
        {
            int interval = 0;
            string inter = txtTimeSpan.Text;
            int.TryParse(inter, out interval);
            int repeact = 0;
            string rep = txtRepeact.Text;
            int.TryParse(rep, out repeact);
            QueryQQParam param = new QueryQQParam();
            NodeData pro = (NodeData)cmbProvince.SelectedItem;
            param.province = pro.Code;
            NodeData city = (NodeData)cmbCity.SelectedItem;
            param.city = city.Code;
            int limit = 30;
            if (int.TryParse(txtCurrentLimit.Text, out limit))
            {
                param.num = limit;
            }
            EGender gender;
            string obj = ((KeyValuePair<string, object>)cmbGender.SelectedItem).Key as string;
            Enum.TryParse(obj, out gender);
            param.sex = gender.GetHashCode();
            return param;
        }
        private void ckStartQuartz_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            if (ck.Checked)
            {
                rbNormal.Checked = true;
                gbPollingType.Enabled = true;
            }
            else 
            {
                rbNormal.Checked = false;
                rbGuid.Checked = false;
                rbDepth.Checked = false;
                gbPollingType.Enabled = false;
            }
        }
        void QuartzForeachPage(object data) 
        {
            if (this.InvokeRequired)
            {//是否通过其他形式调用
                DelegateData.BaseDelegate bd = new DelegateData.BaseDelegate(QuartzForeachPage);
                this.Invoke(bd, data);
                return;
            }
            QueryQQParam param = GetBaseQueryParam();
            int page = 1;
            string pageStr = txtPageIndex.Text;
            int.TryParse(pageStr, out page);
            param.page = page;
            QQDataDA da = new QQDataDA();
            da.QueryParam = param;
            PickUpQQDoResponse response = da.QueryQQData(Cookie);
            if (response == null)
            {
                rtbTip.Text = "No Cookie";
                return;
            }
            FindQQResponse resp = response.responseData;
            if (resp != null && resp.retcode == 0 && resp.result.buddy.totalnum > param.num)
            {
                txtPageIndex.Text = (resp.result.buddy.page + 1).ToString();
            }
            else 
            {
                txtPageIndex.Text = "1";
            }
            QueryResponseAction(response);
            QueryTodayPickUp();
        }
        void QueryResponseAction(PickUpQQDoResponse res)
        {
            if (res != null&&res.responseData!=null)
            {
                FindQQResponse qqres = res.responseData;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("code\t" + qqres.retcode);
                sb.AppendLine("DB Total\t" + qqres.result.buddy.totalnum);
                sb.AppendLine("now count\t" + qqres.result.buddy.info_list.Count);//info_list 可能出现为null的情况
                rtbTip.Text = sb.ToString();
                if (qqres.retcode == 0 && qqres.result.buddy.info_list.Count> 0)
                {
                    return;
                }
                else if (qqres.retcode == 0) 
                {//数据读取正常
                    //数据读取正常也可能出现腾讯没有提供返回数据（腾讯在进行防作弊检测）
                    ProtectJob();
                    return;
                }
                 
                if (qqres.retcode != 0 && qqres.retcode != 6)
                {//延迟轮询的间隔
                    //更改轮询条件
                    ProtectJob();
                }
                else if (res != null && res.responseData.retcode == 6)
                {//已被腾讯封号
                    job.DeleteJob<JobDelegateFunction>();
                    job.DeleteJob<JobAction<QQDataDA>>();
                    rtbTip.Text = "该账户本次检测被禁用";
                }
            }
            else
            {
                rtbTip.Text = "Error\r\n" + res.responseJson;
                //一旦出现数据异常（防止被腾讯检测导致封号），停止轮询
                job.DeleteJob<JobDelegateFunction>();
                job.DeleteJob<JobAction<QQDataDA>>();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        void GatherErrorSendEmail(string responseJson) 
        {
            
        }
        void ProtectJob() 
        {
            //更换查询条件 防止被检测为攻击
            Guid gid = Guid.NewGuid();
            int seed = gid.GetHashCode();
            Random r = new Random(seed);
            int pro = r.Next(0, cmbProvince.Items.Count);
            cmbProvince.SelectedIndex = pro;
            //随机检测是否选择市
            int bindCity = r.Next(0, 2);
            if (bindCity == 1)
            {
                int city = r.Next(0, cmbCity.Items.Count);
                cmbCity.SelectedIndex = city;
            }

        }
    }
}
