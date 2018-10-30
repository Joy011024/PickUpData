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
using System.Threading;
using SelfControlForm;
using DataHelpWinform;
using Infrastructure.ExtService;
using Infrastructure.EFSQLite;
namespace CaptureWebData
{
    public partial class TecentDataFrm : Form
    {
        /*
            规定： 存储城市数据的json串日志文件或者Redis存储项命名规则 Name= "CategoryGroup.CategoryData."+Id
         
         */
        string redisItemOrFileNameFormat(bool isJsonString=true) 
        {
            if (isJsonString)
            {
                return typeof(CategoryGroup).Name + ".Json=";
            }
            return typeof(CategoryGroup).Name + ".Objcet=";
        }
        QuartzJob job = new QuartzJob();
        RedisCacheManage rcm = new RedisCacheManage(SystemConfig.RedisIp, SystemConfig.RedisPsw, SystemConfig.RedisPort);
        CategoryData noLimitAddress = new CategoryData() { Name = "不限" };
        RedisCacheService redis;
        int currentIndex = 1;
        string Uin;//当前进行爬虫时使用到的账户信息
        int intervalSec = 3;
        int executeNum = 0;
        enum ForachCallEvent
        { 
            [Description("采集QQ数据")]
            PickUpUin=1,
            [Description("同步qq数据到总库")]
            SyncUinToCodeDB=2
        }
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
        CategoryData targetCountry = null;
        bool GatherFirstUin = true;
        BackgroundWorker backRun = new BackgroundWorker();//开启的后台进程
        Dictionary<string, DelegateData> BackGroundCallRunEvent = new Dictionary<string, DelegateData>();//后台进程触发事件

        public TecentDataFrm()
        {
            InitializeComponent();
            backRun.DoWork += new DoWorkEventHandler(BackGroundDoWork);

        }
        private void TecentDataFrm_Load(object sender, EventArgs e)
        {
            try
            {
                InitBaseConfig();
                Init();
            }
            catch (Exception ex)
            { /*
                未能加载文件或程序集“EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089”或它的某一个依赖项。系统找不到指定的文件。
                 */
                LogHelperExt.WriteLog(ex.ToString());
            }
        }
        void ReadCountryCity() 
        {
        
        }
        void InitSQLite() 
        {
            string sql = @"";
            UinDataService uis = new UinDataService();
            //uis.Excute(sql);
        }
        void InitBaseConfig()
        {

            cityList = new List<CategoryData>();
            cityList.Add(noLimitAddress);
            CategoryDataService cs = new CategoryDataService(new ConfigurationItems().TecentDA);
            string json = FileHelper.ReadFile(@"..\..\DB\City.log");
            IEnumerable<CategoryData> list = new List<CategoryData>();
            if (ConfigurationItems.OpenSQLServer)
                list = cs.QueryCityCategory();
            else
            {
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CategoryData>>(
                   json
                   );
            }
            SyncDataHelper.SyncCategory(list.ToList());
            //开启数据同步
            //SyncDataHelper.SyncCategory(list.ToList());
            CategoryData obj = SystemConfig.UsingDBSaveBaseData ?
                list.Where(c => c.Code == "1" && c.ParentCode == null).FirstOrDefault() :
                null;//没有数据时考虑读取文件json串
            if (obj == null)
            {
               obj= ForeachCityInFile();
            }
            targetCountry = obj;//目标国家数据
            string defaultCountryNode = GetCagetoryDataFileNameOrRedisItem(obj, redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat));
            if (SystemConfig.OpenRedis) 
            {
                redis = new RedisCacheService(SystemConfig.RedisIp, SystemConfig.RedisPort, SystemConfig.RedisPsw);
               // citys = redis.GetRedisCacheItem<CategoryGroup>(defaultCountryNode);
                //GetRedisCacheItem
               // CategoryGroup r = redis.GetRedisCacheItem<CategoryGroup>(defaultCountryNode);
                //数据存储形式：json字符串或者实体对象字节流
                if (!SystemConfig.RedisValueIsJsonFormat)
                {
                    List<CategoryGroup> objectItems = redis.GetRedisCacheItem<List<CategoryGroup>>(defaultCountryNode);
                    //string redisItem = redis.GetRedisItemString(defaultCountryNode);//读取出来含有json串格式形式[ \"key\":\"value\" ]
                    //List<CategoryGroup> objectItems = redisItem.ConvertObject<List<CategoryGroup>>();
                    if (objectItems != null)
                        cityList.AddRange(objectItems.Select(s => s.Root).OrderBy(s => s.Code));
                }
                else
                {
                    CategoryGroup group = rcm.GetCacheItem<CategoryGroup>(defaultCountryNode);
                }
            }
            if (cityList.Count==1)
            {//没有缓存数据，此时将数据库中的城市地址数据进行读取写入到redis
                NotRedisCacheCase();
                //读取节点项
                //数据项是否已经缓存
            }
            if (SystemConfig.OpenAutoQuertyDBTotal) 
            {//是否开启定时自动查询数据库采集的数据量信息
                DelegateData.BaseDelegate del = IntervalDisplay;
                job.CreateJobWithParam<JobDelegate<Common.Data.EISOSex>>(new object[] { del, null }, DateTime.Now.AddSeconds(5), 30, 0);//
            }
        }
       
        void Init()
        {
            Dictionary<string, object> fields = EGender.Men.GetEnumFieldAttributeDict("DescriptionAttribute", "Description");
            BindComboBoxDict(fields, cmbGender);
            pickUpIEWebCookie.CallBack = CallBack;
            InitProvinceData();
            gbPollingType.Enabled = false;
            BindProvince();//绑定省会的数据源
            GetProcessPath();
            rbtWorkPanel.Click += new EventHandler(RadioButton_Click);
            rbtWorkPanel.Tag = workPanel.Name;
            rbtWebPanel.Click += new EventHandler(RadioButton_Click);
            rbtWebPanel.Tag = pickUpIEWebCookie.Name;
            this.FormClosing += new FormClosingEventHandler(Form_FormBeforeClosed);
            QueryTodayPickUp();
        }
        void InitProvinceData() 
        {
            if (cityList.Count > 0)
            {
                return;
            }
           
        }
        /// <summary>
        /// Redis缓存依赖的文件路径
        /// </summary>
        /// <returns></returns>
        string GetRedisRelyFileDir()
        {
            AssemblyDataExt ass = new AssemblyDataExt();
            string debugDir = ass.GetAssemblyDir();
            return debugDir + "/" + SystemConfig.RedisCacheFromFileReleative + "/" + typeof(CategoryGroup).Name + "/" + GetCagetoryDataFileNameOrRedisItem(targetCountry, 
                redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat));
        }
        /// <summary>
        /// redis 中没有匹配的数据时基础数据加载形式
        /// </summary>
        void NotRedisCacheCase() 
        {
            string dir = GetRedisRelyFileDir();
            string cityFile = GetCagetoryDataFileNameOrRedisItem(targetCountry, redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat)) + ".txt";
            if (SystemConfig.CfgFileExistsIsDoReplace|| !File.Exists(dir + "/" + cityFile))
            {//没有数据文件时先从数据库中进行读取，在写入到文件中，最后写入到redis中
                CategoryDataService cds = new CategoryDataService(new ConfigurationItems().TecentDA);
                List<CategoryData> city = cds.QueryCityCategory().ToList();
                //省会，城市，区域
                AnalyCity(city, targetCountry);//创建相关文件
            }
            //将数据写入redis 
            #region --这里需要增加一个判断是否将数据缓存到Redis缓存库
            //在Redis数据库中增加一个版本号来记录当前存储的城市数据版本
            if (SystemConfig.OpenRedis)
            {//启用Redis功能(数据写入到Redis缓存中)
                rcm.SetPropertyValue("A_"+SystemConfig.DateTimeIntFormat, DateTime.Now.ToString(SystemConfig.DateTimeIntFormat));
                rcm.SetCityCacheFromFile(dir, rcm, SystemConfig.RedisValueIsJsonFormat);
            }
            #endregion 
            //将文件中的数据
            string text = FileHelper.ReadFile(dir+ "/" + cityFile);
            CategoryGroup group = text.ConvertObject<CategoryGroup>();

            //转换为城市数据列表
            List<CategoryData> cities = GetCityDatas(group);
            try
            {
                DBReporistory<CategoryData> db = new DBReporistory<CategoryData>("TecentDASQLite");
               // db.AddList(cities.ToArray());
            }
            catch (Exception ex)
            {

            }
            cityList.AddRange(group.Childrens.Select(s => s.Root).OrderBy(s=>s.Code).ToArray());
        }

        private List<CategoryData> GetCityDatas(CategoryGroup ct)
        {
            List<CategoryGroup> cg = ct.Childrens;
            List<CategoryData> cities = new List<CategoryData>();
            cities.Add(ct.Root);
            foreach (CategoryGroup item in cg)
            {
                cities.AddRange( GetCityDatas(item));
            }
            return cities;
        }
        /// <summary>
        /// 对于从数据库中读取的城市数据进行处理写入到文本文件中作为基础数据使用
        /// </summary>
        /// <param name="data"></param>
        /// <param name="root"></param>
        void AnalyCity(List<CategoryData> data, CategoryData root)
        {
            AssemblyDataExt ass = new AssemblyDataExt();
            string debugDir = ass.GetAssemblyDir() + "/" + SystemConfig.RedisCacheFromFileReleative;
            CategoryGourpHelper helper = new CategoryGourpHelper();
            CategoryGroup result = helper.DataGroup(data); ;
            CategoryGroup nodes = new CategoryGroup();// (CategoryGroup)result;//提取国家列表 
            string cityDir = debugDir + "/" + typeof(CategoryGroup).Name;
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
            CategoryGroup provinceGroup = new CategoryGroup();
            CategoryGroup china = result.Childrens.Where(s => s.Root.Name == root.Name).FirstOrDefault();
            provinceGroup.Root = china.Root;
            foreach (CategoryGroup item in china.Childrens)
            {//查询省会，过滤到市、区
                provinceGroup.Childrens.Add(new CategoryGroup() { Root = item.Root });
                //item.Childrens = new List<CategoryGroup>();
            }
            string province = provinceGroup.ConvertJson();
            Logger.CreateNewAppData(province, GetNodeItemFileName(china,
                redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat)),
                GetRedisRelyFileDir());
            CategoryGroup cgJson = province.ConvertObject<CategoryGroup>();
            //各个省会的市区列表
            foreach (var item in china.Childrens)
            {
                LoggerWriter.CreateLogFile(item.Root.Name, SystemConfig.ExeDir + ELogType.DebugData.ToString(),
                 ELogType.DebugData, DateTime.Now.ToString(Common.Data.CommonFormat.DateToHourIntFormat) + ".log", true);
                //提取省会列表到市
                CategoryGroup level = new CategoryGroup()
                {
                    Childrens = item.Childrens.Select(s => new CategoryGroup() { Root = s.Root }).ToList()
                };
                string jsonNode = level.ConvertJson();
                //string provinceJson=item.ConvertJson();
                if (!string.IsNullOrEmpty(jsonNode))
                {
                    Logger.CreateNewAppData(jsonNode, GetNodeItemFileName(item,
                        redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat)),
                        GetRedisRelyFileDir());
                    if (SystemConfig.OpenRedis)
                    {
                        redis.SetRedisItem(GetCagetoryDataFileNameOrRedisItem(item.Root,
                            redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat)), jsonNode);
                    }
                }
                CategoryGroup cg = new CategoryGroup()
                {
                    Root = item.Root,
                    Childrens = item.Childrens
                };
                LoggerWriter.CreateLogFile(cg.Root.Name, SystemConfig.ExeDir + ELogType.DebugData.ToString(),
                 ELogType.DebugData, DateTime.Now.ToString(Common.Data.CommonFormat.DateToHourIntFormat) + ".log", true);
                foreach (CategoryGroup c in item.Childrens)
                {//省直辖市的子节点
                    CategoryGroup cn = new CategoryGroup()
                    {
                        Root = c.Root,
                        Childrens = c.Childrens
                    };
                    string nodeJson = string.Empty;
                    if (c.Childrens.Count > 0)
                    {
                        nodeJson = cn.ConvertJson();
                    }
                    LoggerWriter.CreateLogFile(cn.Root.Name, SystemConfig.ExeDir + ELogType.DebugData.ToString(),
                  ELogType.DebugData, DateTime.Now.ToString(Common.Data.CommonFormat.DateToHourIntFormat) + ".log", true);
                    Logger.CreateNewAppData(nodeJson, GetNodeItemFileName(c,
                        redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat)), GetRedisRelyFileDir());
                }
            }
        }
        string GetNodeItemName(CategoryGroup item,string nameFormat) 
        {
            return "/"+nameFormat + item.Root.Id ;
        }
        /// <summary>
        /// 组装文件或者Redis缓存项的名称
        /// </summary>
        /// <param name="cate"></param>
        /// <param name="nameFormat"></param>
        /// <returns></returns>
        string GetCagetoryDataFileNameOrRedisItem(CategoryData cate,string nameFormat) 
        {
            return nameFormat +cate.Name.TextConvertChar(true)+ cate.Id;
        }
        string GetNodeItemFileName(CategoryGroup item, string nameFormat) 
        {
            return nameFormat +item.Root.Name.TextConvertChar(true)+ item.Root.Id + ".txt";
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
            if (string.IsNullOrEmpty(Cookie))
            {
                rtbTip.Text = "please  login,and get cookie,and continue";
                return;
            }
            // 
            int interval = 0;
            string inter = txtTimeSpan.Text;
            int.TryParse(inter, out interval);
            if (interval > 0)
            {
                intervalSec = interval;
            }
            else 
            {
                interval = intervalSec;
            }
            int repeact = 0;
            string rep = txtRepeact.Text;
            int.TryParse(rep, out repeact);
            QQDataDA das=new QQDataDA();
            LoggerWriter.CreateLogFile(Cookie, (new ConfigurationItems()).LogPath + das.GeneratePathTimeSpan(Cookie), ELogType.SessionOrCookieLog);
            Uin = das.GetUinFromCookie(Cookie);//当前登录的账户
            //useralias  这是提取账户名称的元素
            QueryQQParam param = GetBaseQueryParam();
            ParameterizedThreadStart pth;
            if (!ckStartQuartz.Checked)
            {//不进行轮询
                JustQuery(param);
            }
            else if (!ckBackGroundCall.Checked)
            {
                #region 进行的是quartz.net轮询调度
                if (ckStartQuartz.Checked && rbGuid.Checked)
                {//开启随机轮询
                    DelegateData.BaseDelegate del = QuartzGuidForach;
                    QuartzJobParam p = new QuartzJobParam()
                    {
                        JobExecutionContextJobDataMap = new object[] { del, param, null },
                        StartTime = DateTime.Now.AddSeconds(interval),
                        TriggerRepeat = repeact,
                        TrigggerInterval = interval
                    };
                    pth = new ParameterizedThreadStart(BackstageRun<JobDelegateFunction>);
                    Thread th = new Thread(pth);
                    th.Start(p);
                    // job.CreateJobWithParam<JobDelegateFunction>(new object[] { del, param,null }, DateTime.Now.AddSeconds(interval), interval, repeact);
                }
                else if (ckStartQuartz.Checked && rbDepth.Checked)
                {//该查询结果页轮询
                    DelegateData.BaseDelegate del = QuartzForeachPage;
                    QuartzJobParam p = new QuartzJobParam()
                    {
                        JobExecutionContextJobDataMap = new object[] { del, null, null },
                        StartTime = DateTime.Now.AddSeconds(interval),
                        TriggerRepeat = repeact,
                        TrigggerInterval = interval
                    };
                    pth = new ParameterizedThreadStart(BackstageRun<JobDelegateFunction>);
                    Thread th = new Thread(pth);
                    th.Start(p);
                    //job.CreateJobWithParam<JobDelegateFunction>(new object[] { del, null,null }, DateTime.Now.AddSeconds(interval), interval, repeact);
                }
                else if (ckStartQuartz.Checked)
                {
                    DelegateData.BaseDelegate del = QuartzCallBack;
                    QuartzJobParam p = new QuartzJobParam()
                    {
                        JobExecutionContextJobDataMap = new object[] { Cookie, param, del },
                        StartTime = DateTime.Now.AddSeconds(interval),
                        TriggerRepeat = repeact,
                        TrigggerInterval = interval
                    };
                    pth = new ParameterizedThreadStart(BackstageRun<JobAction<QQDataDA>>);
                    Thread th = new Thread(pth);
                    th.Start(p);
                    // job.CreateJobWithParam<JobAction<QQDataDA>>(new object[] { Cookie, param, del }, DateTime.Now, interval, repeact);
                }
                else
                {
                    JustQuery(param);
                }
                #endregion
            }
            else if (ckBackGroundCall.Checked&& ckStartQuartz.Checked)
            {//轮询但是使用的是后台进程 
                #region 使用的是后台进程
                BackGrounForeachCallType(param);
                #endregion
            }
            #region 数据同步到核心库
            if (ckSyncUin.Checked)
            { //同步数据
                string key=ForachCallEvent.SyncUinToCodeDB.ToString();
                if (BackGroundCallRunEvent.ContainsKey(key))
                {
                    BackGroundCallRunEvent.Remove(key);
                }
                DelegateData del=new DelegateData(){ BaseDel=BackGrounSyncUinToCoreDB,BaseDelegateParam=null};
                BackGroundCallRunEvent.Add(key, del);
            }
            #endregion
            if (!backRun.IsBusy)
            {
                backRun.RunWorkerAsync();
            }
        }
        private void JustQuery(QueryQQParam param) 
        {
            QQDataDA da = new QQDataDA();
            da.QueryParam = param;
            PickUpQQDoResponse response = da.QueryQQData(Cookie);
            if (GatherFirstUin || !SystemConfig.OpenAutoQuertyDBTotal)
            {//这里要改成在页面初始化时查询当前库数据量，其他情形交给另一线程查询
                QueryTodayPickUp();
                GatherFirstUin = false;
            }
            QuartzCallBack(response);
        }
        void BackstageRun<T>(object param) where T:Quartz.IJob
        {
            QuartzJobParam p = param as QuartzJobParam;
            job.CreateJobWithParam<T>(p.JobExecutionContextJobDataMap, p.StartTime, p.TrigggerInterval,p.TriggerRepeat);
        }
        private void webBrowserData_Load(object sender, EventArgs e)
        {

        }
        void BindProvince()
        {//绑定中国的全部省会，自治区到控件上
            BindComboBox(new CategoryData() { Code="1"}, cmbProvince, 2);
        }

        private void cmbProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            //是否为已选中
            string name = cmb.Name;
            bool selectAfter = cmb.Focused;//是否在选中之后进行触发
            CategoryData node = (CategoryData)cmb.SelectedItem;
            BindComboBox(node, cmbCity, 3);
        }
        void BindComboBox(CategoryData parentCode, ComboBox cmb, int level)
        {
            List<CategoryData> nodes = new List<CategoryData>();
            try
            {
                if (parentCode.Id == 0 && level == 2)
                {//该控件来自于省会自治区
                    nodes = cityList;
                }
                else if (parentCode.Id > 0)
                {
                    nodes.Add(noLimitAddress);
                    //如果没有启用Redis功能，则该数据从文本文件中读取
                    CategoryGroup objs = null;
                    //如果是文本文件 需要读取上层节点项，如果是Redis缓存项，则只需读取当前节点对id组装缓存项名称
                    if (SystemConfig.OpenRedis)
                    {
                        string itemName = GetCagetoryDataFileNameOrRedisItem(parentCode,
                            redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat));
                        // typeof(CategoryGroup).Name + ".Objcet=" + parentCode.Id;
                        if (!SystemConfig.RedisValueIsJsonFormat)
                        {//json  or object
                            List<CategoryGroup> items = redis.GetRedisCacheItem<List<CategoryGroup>>(itemName);
                            if (items != null)
                                objs = new CategoryGroup() { Childrens = items };
                        }
                        else
                        {
                            objs = redis.GetRedisCacheItem<CategoryGroup>(itemName);
                        }
                    }
                    else
                    {
                        string dir = GetRedisRelyFileDir();
                        string fileJson = FileHelper.ReadFile(dir + "/" + GetNodeItemFileName(new CategoryGroup() { Root = parentCode },
                            redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat)));
                        if (fileJson != null)
                        {
                            objs = fileJson.ConvertObject<CategoryGroup>();
                        }
                    }
                    if (objs != null)
                    {//没数据
                        List<CategoryData> items = objs.Childrens.Select(s => s.Root)
                       .OrderBy(t => t.Code).ToList();
                        nodes.AddRange(items.ToArray());
                    }
                }
                else
                {
                    nodes.Add(noLimitAddress);
                }
            }
            catch (Exception ex)
            {
                nodes = new List<CategoryData>();
                nodes.Add(noLimitAddress);
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
            if (res == null) { return; }// 1 no cookie 
            //如果此时检测返回集合为空，但是返回状态码不是错误，需要更改检测条件【腾讯防攻击检测】
            QueryResponseAction(res);
            if(!SystemConfig.OpenAutoQuertyDBTotal)
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
            try
            {
                PickUpStatic pc;
                if (SystemConfig.UsingDBSaveBaseData)
                {
                    pc = (new QQDataDA()).TodayStatic();
                }
                else 
                {
                    pc = new PickUpStatic();
                }
                lsbStatic.Items.Clear();
                lsbStatic.Items.Add("时间戳" + DateTime.Now.ToString());
                lsbStatic.Items.Add("库\t" + pc.DBTotal);
                lsbStatic.Items.Add("库唯一\t" + pc.DBPrimaryTotal);
                lsbStatic.Items.Add("日期\t" + pc.StaticDay);
                lsbStatic.Items.Add("今日总计\t" + pc.Total);
                lsbStatic.Items.Add("今日QQ号\t" + pc.IdTotal);
            }
            catch (Exception ex)
            {
                LogHelperExt.WriteLog("Query pick up number\r\n"+ex.Message);
            }
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
            if (city == null)
            {
                return param;
            }
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
            int page = currentIndex+1;
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
                txtPageIndex.Text = (page+1).ToString();
            }
            else
            {
                page = 1;
                txtPageIndex.Text = page.ToString();
            }
            QueryResponseAction(response);
            if (!SystemConfig.OpenAutoQuertyDBTotal)
                QueryTodayPickUp();
        }
        void GetContainerKeyOfCookie(string  cookie)
        {
            //此处调用cookie
            QQDataDA common = new QQDataDA();
            //ldw,bkn 是js生成的参数，在该函数调用中将根据cookie进行生成
            UinGroupDataRequestParam puin = new UinGroupDataRequestParam()
            {
                k = "交友",
                n = 8,
                st = 1,
                iso = 1,
                src = 1,
                v = 4903,
                bkn = "1053723692",
                isRecommend = false,
                city_id = 10059,
                from = 1,
                newSearch = true,
                keyword = "白羊座",
                sort = 0,
                wantnum = 24,
                page = 0,
                ldw = "1053723692"
            };
            common.QQGroupGather(Cookie, puin);
        }
        void QueryResponseAction(PickUpQQDoResponse res)
        {
            executeNum++;//当前执行次数
          //  GetContainerKeyOfCookie(res.cookie);//查询qq群组数据[qq群数据提取参数待确定]
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
                {//已被腾讯封号【禁用时进行程序关闭，并反馈到邮箱】
                    job.DeleteJob<JobDelegateFunction>();
                    job.DeleteJob<JobAction<QQDataDA>>();
                    rtbTip.Text = "该账户本次检测被禁用";
                    DataLink dl = new DataLink();
                    StringBuilder tip = new StringBuilder();
                    tip.AppendLine("time:\t"+DateTime.Now.ToString(SystemConfig.DateTimeFormat));
                    tip.AppendLine("App:\tCaptureWebData");
                    tip.AppendLine("App Event:Pick up uin data [warm]");
                    tip.AppendLine("account:\t"+Uin);
                    dl.SendDataToOtherPlatform(LanguageItem.Tip_PickUpErrorlockAccount, tip.ToString());//需要知道当前在进行采集的账户
                }
            }
            else
            {
                //返回消息转换为实体对象
                FindQQResponse find = res.responseJson.ConvertObject<FindQQResponse>();
                if (find == null && res.responseJson.Contains(SystemConfig.IIS501))
                {
                    rtbTip.Text = SystemConfig.IIS501;
                   // Application.ExitThread();
                }
                else if (find.retcode != QQRetCode.Forbin.GetHashCode() && find.retcode != QQRetCode.CookieTimeOut.GetHashCode())
                { //错误不是来自于账户不禁用
                    ProtectJob();
                    return;
                }
                else
                {
                    //判断返回数据的情况
                    rtbTip.Text = "Error\r\n" + res.responseJson;
                }
                //一旦出现数据异常（防止被腾讯检测导致封号），停止轮询
                job.DeleteJob<JobDelegateFunction>();
                job.DeleteJob<JobAction<QQDataDA>>();
            }
            if (executeNum% SystemConfig.DivideNum==0)
            {//满足发送邮件的条件
                DataLink dl = new DataLink();
                StringBuilder tip = new StringBuilder();
                tip.AppendLine("time:\t" + DateTime.Now.ToString(SystemConfig.DateTimeFormat));
                tip.AppendLine("App:\tCaptureWebData");
                tip.AppendLine("App Event:Pick up uin data [excute num]"+executeNum);
                tip.AppendLine("account:\t" + Uin);
                dl.SendDataToOtherPlatform(LanguageItem.Tip_PickUpErrorlockAccount, tip.ToString());//需要知道当前在进行采集的账户
                
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            StopRunApp();
        }
        void StopRunApp() 
        {
            Application.ExitThread();
            Application.Exit();
            Environment.Exit(0);//次步骤会将新启动的进程一起进行关闭【符合关闭程序的要求限定】
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
        void BackGrounForeachCallType(QueryQQParam param)
        {//backGroundwork
            string key = ForachCallEvent.PickUpUin.ToString();
            DelegateData delete = new DelegateData() { BaseDelegateParam=param};
            //要实现定时
            if (BackGroundCallRunEvent.ContainsKey(key))
            {
                BackGroundCallRunEvent.Remove(key);
            }
            if (ckStartQuartz.Checked && rbGuid.Checked)
            {//开启随机轮询
                delete.BaseDel = QuartzGuidForach;
                BackGroundCallRunEvent.Add(key, delete);
                //QuartzGuidForach(param);
            }
            else if (ckStartQuartz.Checked && rbDepth.Checked)
            {//该查询结果页轮询
                delete.BaseDel = QuartzForeachPage;
                BackGroundCallRunEvent.Add(key, delete);
                //QuartzForeachPage(param);
            }
            else if (ckStartQuartz.Checked)
            {
                delete.BaseDel = QuartzCallBack;
                BackGroundCallRunEvent.Add(key, delete);
                //QuartzCallBack(param);
            }
            else 
            {
                //只查询一遍
                JustQuery(param);
            }
        }
        private void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            CategoryData node = (CategoryData)cmb.SelectedItem;
            BindComboBox(new CategoryData() { Id = node.Id,Name=node.Name,Code=node.Code }, cmbDistinct, 4);
            return;
        }
        void GetRedisCacheItem() 
        {
            
        }
        private void RadioButton_Click(object sender, EventArgs e)
        {
            RadioButton rbt = sender as RadioButton;
            string panel = rbt.Tag as string;
            if (string.IsNullOrEmpty(panel))
            {
                return;
            }
            //首先查找全部radio控制项【联动控制隐藏】
            List<Control> bindPanels =new PageDataHelp().ForeachPanel(switchPanel,typeof(RadioButton).Name);// switchPanel.Controls;
            foreach (Control item in bindPanels)
            {
                string hide = item.Tag as string;
                Control[] targets = this.Controls.Find(hide, false);
                foreach (Control ch in targets)
                {
                    if (panel == hide)
                    {
                        ch.Visible = true;
                    }
                    else 
                    {
                        ch.Visible = false;
                    }
                }
            }
        }
        public void Form_FormBeforeClosed(object sender,FormClosingEventArgs e)
        {
            Form fm = sender as Form;
            //窗体关闭前，关闭异步线程
            StopOtherThread();
        }
        void StopOtherThread() 
        {
            job.DeleteJob<JobDelegate<Common.Data.EISOSex>>();
        }
        void BackGroundDoWork(object sender,DoWorkEventArgs e) 
        {
            BackgroundWorker bg = sender as BackgroundWorker;
            while (true)
            {
                if (BackGroundCallRunEvent.Count > 0)
                {
                    foreach (KeyValuePair<string,DelegateData> item in BackGroundCallRunEvent)
                    {
                        DelegateData delete = item.Value;
                        delete.BaseDel(delete.BaseDelegateParam);
                    }
                }
                Thread.Sleep(intervalSec * 1000);//20秒执行一次
            }
        }
        void BackGrounSyncUinToCoreDB(object param) 
        {
            UinDataSyncHelp helper = new UinDataSyncHelp();
            helper.DoIntervalSync(ConfigurationItems.GetWaitSyncDBString);
        }
        CategoryData ForeachCityInFile() 
        {
            string file = SystemConfig.ExeDir + @"\Service\CategoryGroup\China.txt";
            string text= FileHelper.ReadFile(file);
            CategoryData cg = text.ConvertObject<CategoryData>();
            return cg;
        }
    }
}
