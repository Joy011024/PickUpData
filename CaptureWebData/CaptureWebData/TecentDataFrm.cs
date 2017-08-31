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
using System.IO;
using AppService.RedisService;
namespace CaptureWebData
{
    public partial class TecentDataFrm : Form
    {
        QuartzJob job = new QuartzJob();
        string NoLimit = "不限";
        CategoryData noLimitAddress = new CategoryData() { Name = "不限" };
        RedisCacheService redis;
        enum QQRetCode
        {
            Normal = 0,
            Forbin = 6,//禁用
            CookieTimeOut = 100000
        }
        enum ComboboxItem
        {
            Name = 1,
            Code = 2
        }
        enum ComboboxDictItem
        {
            Key = 1,
            Value = 2
        }
        string Cookie { get; set; }
        List<CategoryData> cityList = new List<CategoryData>();
        bool GatherFirstUin = true;
        public TecentDataFrm()
        {
            InitializeComponent();
            InitRedis();
            Test();
            Init();
        }
        void InitRedis()
        {
            redis = new RedisCacheService(SystemConfig.RedisIp, SystemConfig.RedisPort, SystemConfig.RedisPsw);
            CategoryDataService cs = new CategoryDataService(new ConfigurationItems().TecentDA);
            CategoryData obj = cs.QueryCityCategory().Where(c => c.Code == "1" && c.ParentCode == null).FirstOrDefault();
            List<CategoryGroup> citys = redis.GetRedisCacheItem<List<CategoryGroup>>(typeof(CategoryGroup).Name + "." + typeof(CategoryData).Name + "." + obj.Id);
            cityList.Add(noLimitAddress);
            List<CategoryData> cts = citys.Select(s => s.Root).OrderBy(t => t.Code).ToList();
            cityList.AddRange(cts);
            DelegateData.BaseDelegate del=IntervalDisplay;
            job.CreateJobWithParam <JobDelegate<Common.Data.EISOSex> >(new object[]{del,null},DateTime.Now.AddSeconds(5),30,0);//
        }
        void Test()
        {
            ProcessHelp.ForeachProcess();
        }
        private void TecentDataFrm_Load(object sender, EventArgs e)
        {
         
        }
        void Init()
        {
            Dictionary<string, object> fields = EGender.Men.GetEnumFieldAttributeDict("DescriptionAttribute", "Description");
            BindComboBoxDict(fields, cmbGender);
            pickUpIEWebCookie.CallBack = CallBack;
            InitProvinceData();
            gbPollingType.Enabled = false;
            BindProvince();
            GetProcessPath();
            QueryTodayPickUp();
        }
        void InitProvinceData() 
        {
            if (cityList.Count > 0)
            {
                return;
            }
            AssemblyDataExt ass = new AssemblyDataExt();
            string debugDir = ass.GetAssemblyDir();
            string dir = debugDir + "/Service";
            string cityFile = "City.txt";
            if (!File.Exists(dir + "/" + cityFile))
            {
                //首先判断redis中是否存在配置数据

                string text = redis.GetRedisItemString(typeof(CategoryData).Name);
                if (redis.HavaCacheItem)
                {
                    cityList = text.ConvertObject<List<CategoryData>>();
                }
                else
                {
                    CategoryDataService cds = new CategoryDataService(new ConfigurationItems().TecentDA);
                    IEnumerable<CategoryData> city = cds.QueryCityCategory();
                    cityList = city.ToList();
                    string json = cityList.ConvertJson();
                    json.CreateNewAppData(cityFile);
                    redis.SetRedisItem(typeof(CityData).Name, "redis");
                }
            }
            else
            {
                string json = FileHelper.ReadFile(dir + "/" + cityFile);
                cityList = json.ConvertObject<List<CategoryData>>();
            }
            // AnalyCity();
            cityList.Add(noLimitAddress);
        }
        void AnalyCity() 
        {
            AssemblyDataExt ass = new AssemblyDataExt();
            string debugDir = ass.GetAssemblyDir();
            CategoryGourpHelper helper = new CategoryGourpHelper();
            CategoryGroup result=helper.DataGroup(cityList);;
            CategoryGroup nodes = new CategoryGroup();// (CategoryGroup)result;//提取国家列表 
            string cityDir = ass.ForeachDir(debugDir, 3) + "/" + typeof(CategoryGroup).Name;
            //中国的省会列表
            foreach (CategoryGroup item in result.Childrens)
            {//查询到国家，过滤省市直辖区
                //item.Childrens = new List<CategoryGroup>();
                CategoryGroup temp = new CategoryGroup() { Root = item.Root };
                nodes.Childrens.Add(temp);
            }
            string country = nodes.ConvertJson();
            Logger.CreateNewAppData(country, "Country.txt", cityDir);
            //提取中国的省会列表 
            #region 要追踪显示的索引则取消此块注释
            //int index = 0;//
            //for (int i = 0; i < result.Childrens.Count; i++)
            //{
            //    if (result.Childrens[i].Root.Name == "中国") 
            //    {
            //        index = i;
            //        break;
            //    }
            //}
            #endregion
            CategoryGroup provinceGroup = new CategoryGroup();
            CategoryGroup china= result.Childrens.Where(s => s.Root.Name == "中国").FirstOrDefault();
            provinceGroup.Root = china.Root;
            foreach (CategoryGroup item in china.Childrens)
            {//查询省会，过滤到市、区
                provinceGroup.Childrens.Add(new CategoryGroup() { Root = item.Root });
                //item.Childrens = new List<CategoryGroup>();
            }
            string province = provinceGroup.ConvertJson();
            Logger.CreateNewAppData(province, china.Root.Name +"="+china.Root.Id+ ".txt", cityDir + "/" + china.Root.Name + "=" + china.Root.Id);
            //各个省会的市区列表
            foreach (var item in china.Childrens)
            {
                Logger.CreateNewAppData(item.ConvertJson(), item.Root.Name + ".txt", cityDir + "/" + china.Root.Name + "=" + china.Root.Id);
                CategoryGroup cg = new CategoryGroup() 
                {
                    Root=item.Root,
                    Childrens=item.Childrens
                };
                string city = cg.ConvertJson();
                string pro= cityDir + "/"+china.Root.Name+"=" +china.Root.Id+"/"+item.Root.Name+"="+ item.Root.Id;
                Logger.CreateNewAppData(city, item.Root.Name+"="+item.Root.Id+".txt", pro);
                foreach (CategoryGroup c in item.Childrens)
                {//省直辖市的子节点
                    CategoryGroup cn = cn = new CategoryGroup()
                    {
                        Root = c.Root,
                        Childrens = c.Childrens
                    };
                    string nodeJson = string.Empty;
                    if (c.Childrens.Count>0)
                    {
                        nodeJson = cn.ConvertJson();
                    }
                    string cityNodeDir = pro + "/" + c.Root.Name + "=" + c.Root.Id;
                    Logger.CreateNewAppData(nodeJson, c.Root.Name + "=" + c.Root.Id + ".txt", cityNodeDir);
                }
            }
            //foreach (var item in nodes.Childrens)
            //{
            //    string text = item.ConvertJson();
            //    string filename = item.Root.Name+".txt";
            //    Logger.CreateNewAppData(text, filename, cityDir);
            //    if (item.Childrens.Count == 0)
            //    {
            //        continue;
            //    }
            //    string detailDir = cityDir + "/" + item.Root.Name;
            //    foreach (var pro in item.Childrens)
            //    {
            //        string document = pro.ConvertJson();
            //        filename = pro.Root.Name + ".txt";
            //        Logger.CreateNewAppData(document, filename, detailDir);
            //    }
            //}
        }
        void GetProcessPath()
        {
            ConfigurationItems config = new ConfigurationItems();
            rtbTip.Text = config.GetNowAssembly();//config.GetProcessPath();
            this.Text += config.JoinProcessPath();
        }
        void CallBack(object cookie)
        {
            Cookie = cookie as string;
        }
        void BindComboBoxDict(Dictionary<string, object> fields, ComboBox cmb)
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
            LoggerWriter.CreateLogFile(Cookie, (new ConfigurationItems()).LogPath + (new QQDataDA().GeneratePathTimeSpan(Cookie)), ELogType.SessionOrCookieLog);

            QueryQQParam param = GetBaseQueryParam();
            if (ckStartQuartz.Checked && rbGuid.Checked)
            {//开启随机轮询
                DelegateData.BaseDelegate del = QuartzGuidForach;
                job.CreateJobWithParam<JobDelegateFunction>(new object[] { del, param }, DateTime.Now.AddSeconds(interval), interval, repeact);
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
                if (GatherFirstUin) 
                {//这里要改成在页面初始化时查询当前库数据量，其他情形交给另一线程查询
                    QueryTodayPickUp();
                    GatherFirstUin = false;
                }
               
                QuartzCallBack(response);
            }
        }

        private void webBrowserData_Load(object sender, EventArgs e)
        {

        }
        void BindProvince()
        {
            BindComboBox(new CategoryData() { Code="1"}, cmbProvince, 2);
        }

        private void cmbProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            CategoryData node = (CategoryData)cmb.SelectedItem;
            BindComboBox(node, cmbCity, 3);
        }
        void BindComboBox(CategoryData parentCode, ComboBox cmb, int level)
        {
            List<CategoryData> nodes = new List<CategoryData>();
            if (parentCode.Id==0&&level==2)
            {//没有选择省/自治区
                nodes = cityList;
            }
            else
            {
                nodes.Add(noLimitAddress);
                List < CategoryGroup> objs = redis.GetRedisCacheItem<List<CategoryGroup>>(typeof(CategoryGroup).Name + ".Objcet=" + parentCode.Id);
                if (objs !=null)
                {//没数据
                    List<CategoryData> items = objs.Select(s => s.Root).ToList()
                   .OrderBy(t => t.Code).ToList();
                    nodes.AddRange(items.ToArray());
                }
                //nodes = cityList.Where(c => (c.ParentCode == parentCode || string.IsNullOrEmpty(c.Code)) && c.NodeLevel == level).
                //    Select(n =>
                //        n.ConvertMapModel<CategoryData, NodeItem>()
                //        ).OrderBy(t => t.Code).ToList();
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
                this.Invoke(bd, data);
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
        private void IntervalDisplay(object quartzParam)
        {
            if (this.InvokeRequired)
            {
                DelegateData.BaseDelegate bd = new DelegateData.BaseDelegate(IntervalDisplay);
                this.Invoke(bd, quartzParam);
                return;
            }
            else {
                QueryTodayPickUp();
            }
        }
        private void QueryTodayPickUp()
        {
            PickUpStatic pc = (new QQDataDA()).TodayStatic();
            lsbStatic.Items.Clear();
            lsbStatic.Items.Add("时间戳" + DateTime.Now.ToString());
            lsbStatic.Items.Add("库\t" + pc.DBTotal);
            lsbStatic.Items.Add("库唯一\t" + pc.DBPrimaryTotal);
            lsbStatic.Items.Add("日期\t" + pc.StaticDay);
            lsbStatic.Items.Add("今日总计\t" + pc.Total);
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
            PickUpQQDoResponse response = da.QueryQQData(Cookie);
           // QueryTodayPickUp();
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
            int limit = 30;
            if (int.TryParse(txtCurrentLimit.Text, out limit))
            {
                param.num = limit;
            }
            EGender gender;
            string obj = ((KeyValuePair<string, object>)cmbGender.SelectedItem).Key as string;
            Enum.TryParse(obj, out gender);
            param.sex = gender.GetHashCode();

            NodeData pro = (NodeData)cmbProvince.SelectedItem;
            if (string.IsNullOrEmpty(pro.Code))
            {
                return param;
            }
            param.province = pro.Code;
            NodeData city = (NodeData)cmbCity.SelectedItem;
            if (string.IsNullOrEmpty(city.Code))
            {
                return param;
            }
            param.city = city.Code;
            NodeData dist = (NodeData)cmbDistinct.SelectedItem;
            if (string.IsNullOrEmpty(dist.Code))
            {
                return param;
            }
            param.district = dist.Code;
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
           // QueryTodayPickUp();
        }
        void QueryResponseAction(PickUpQQDoResponse res)
        {
            if (res != null && res.responseData != null)
            {
                FindQQResponse qqres = res.responseData;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("code\t" + qqres.retcode);
                sb.AppendLine("DB Total\t" + qqres.result.buddy.totalnum);
                sb.AppendLine("now count\t" + qqres.result.buddy.info_list.Count);//info_list 可能出现为null的情况
                rtbTip.Text = sb.ToString();
                if (qqres.retcode == QQRetCode.Normal.GetHashCode() && qqres.result.buddy.info_list.Count > 0)
                {
                    return;
                }
                else if (qqres.retcode == QQRetCode.Normal.GetHashCode())
                {//数据读取正常(但是当前没有查找到数据)
                    //数据读取正常也可能出现腾讯没有提供返回数据（腾讯在进行防作弊检测）
                    ProtectJob();
                    return;
                }
                if (qqres.retcode == QQRetCode.CookieTimeOut.GetHashCode())
                { //该账户已退出登录
                    rtbTip.Text = "该账户已退出";
                    job.DeleteJob<JobDelegateFunction>();
                    job.DeleteJob<JobAction<QQDataDA>>();
                    return;
                }
                if (qqres.retcode != QQRetCode.Forbin.GetHashCode())
                {//延迟轮询的间隔
                    //更改轮询条件
                    ProtectJob();
                }
                else if (qqres.retcode == QQRetCode.Forbin.GetHashCode())
                {//已被腾讯封号
                    job.DeleteJob<JobDelegateFunction>();
                    job.DeleteJob<JobAction<QQDataDA>>();
                    rtbTip.Text = "该账户本次检测被禁用";
                }
            }
            else
            {
                //返回消息转换为实体对象
                FindQQResponse find = res.responseJson.ConvertObject<FindQQResponse>();
                if (find.retcode != QQRetCode.Forbin.GetHashCode() && find.retcode != QQRetCode.CookieTimeOut.GetHashCode())
                { //错误不是来自于账户不禁用
                    ProtectJob();
                    return;
                }
                //判断返回数据的情况
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

        private void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            CategoryData node = (CategoryData)cmb.SelectedItem;
            BindComboBox(new CategoryData() { Id = node.Id }, cmbDistinct, 4);
            return;
            //由于腾讯城市数据县级 的父节点使用
            CategoryData item = cityList.Where(c => c.Id == node.Id).FirstOrDefault();
            List<CategoryData> items = new List<CategoryData>();
            items.Add(noLimitAddress);
            if (item.ParentId.HasValue)
            {
                
                //object dd = cityList.Where(c => c.ParentId == item.Id).ToList();
                //CategoryData[] cts = cityList.Where(c => c.ParentId == item.Id).ToArray();
                //items.AddRange(cts);
            }
            //cmbDistinct.DataSource = items;
            //cmbDistinct.DisplayMember = ComboboxItem.Name.ToString();
            //cmbDistinct.ValueMember = ComboboxItem.Code.ToString();
        }
        void GetRedisCacheItem() 
        {
            
        }
    }
}
