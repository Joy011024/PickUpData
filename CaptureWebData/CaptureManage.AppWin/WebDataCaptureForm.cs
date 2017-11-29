using HttpClientHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain.CommonData;
using System.IO;
using Infrastructure.ExtService;
using Common.Data;
using AppLanguage;
using System.Resources;
using DataHelp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TicketData.Model;
using TicketData.IManage;
using FactoryService;
using TicketData.Manage;
using System.Net;
using QuartzJobService;
using CaptureManage.AppWin.TicketData.Model.Request;
namespace CaptureManage.AppWin
{
    public partial class WebDataCaptureForm : Form
    {
        enum EleTag
        {
            Icon=1
        }
       
        string AppDir
        {
            get 
            {
                string path = this.GetType().Assembly.Location;
                DirectoryInfo di = new DirectoryInfo(path);
                return di.Parent.FullName;
            }
        }
        string logDir;
        public string LogDir
        {
            get
            {
                if (string.IsNullOrEmpty(logDir))
                {
                    DirectoryInfo di = new DirectoryInfo(AppDir);
                    logDir = di.Parent.FullName;
                }
                return logDir;
            }
        }
        public string ImgDir
        {
            get 
            {
                return NowAppDirHelper.GetNowAppDir(AppCategory.WinApp);
            }
        }
        List<StationName> datas = new List<StationName>();
        QueryTicket ticketWhere = new QueryTicket();
        bool SyncToDB = false;//采集的数据是否同步到数据库
        leftTicketDTO ticketParam = new leftTicketDTO();
        string langTipFix = "Tip_12306_";//语言提示的前缀项
        Dictionary<string, string> configDict = new Dictionary<string, string>();
        /// <summary>
        /// 12306 验证码图片中文本内容的高度
        /// </summary>
        int VerifyCodeTextHeight 
        {
            get 
            {
                string cfg = configDict["VerifyCodeTextHeight"];
                int h = 30;
                if (!string.IsNullOrEmpty(cfg) && int.TryParse(cfg, out h))
                { }
                return h;
            }
        }
        /// <summary>
        /// 点中的图片大小
        /// </summary>
        int[] ClickIconSizeIn12306
        {
            get 
            {
                string cfg = configDict["ClickIconSizeIn12306"];
                string[] size = cfg.Split('x');
                return new int[] { int.Parse(size[0]), int.Parse(size[1]) };
            }
        }
        int avgX=0, avgY=0;
        VerifyCode verifyCodeParam;//进行验证码验证的参数配置
        /// <summary>
        /// 选中的图标
        /// </summary>
        Dictionary<int, Point> selectIcon = new Dictionary<int, Point>();
        /// <summary>
        /// 12306图片验证码模式
        /// </summary>
        int[] smallImgNormalIn12306
        {
            get 
            {
                string cfg = configDict["VerifyCodeTemplate"];
                string[] items = cfg.Split('*');
                return new int[] { int.Parse(items[0]), int.Parse(items[1]) };
            }
        }
        string VerifyCodeReleativeDir
        {
            get
            {
                return configDict["ClickImgReleativePath"];

            }
        }
        public string VerifyCodeDir
        {
            get 
            {
                return AppDir + "/" + configDict["ImageRelativeDirName"];
            }
        }
        public string VerifyCodeName 
        {
            get 
            {
                return configDict["VerifyCodeImgName"];
            }
        }
        public string VerifyCodeImgFullDir
        {
            get 
            {
                return VerifyCodeDir + "/" + VerifyCodeName;
            }
        }
        string IconImgFullDir
        {
            get 
            {
                return AppDir + "/" + VerifyCodeReleativeDir;
            }
        }
        int DefualtRefreshCalTicketTimeSpan
        {
            get 
            {
                string cfg = configDict["DefaultQueryTimeSpan"];
                int span = 2;
                if (int.TryParse(cfg, out span))
                { }
                if (span <= 1)
                {
                    span = 1;
                }
                return span;
            }
        }
        public string CheckVerifyCodeUrl
        {
            get 
            {
                return configDict["CheckVerifyCodeUrl"];
            }
        }
        /// <summary>
        /// 票务系统（12306）逻辑配置问路径
        /// </summary>
        string TickekLogicConfigFullFile
        {
            get 
            {
                return  AppDir+"/"+ configDict["TickekLogicConfigFile"];
            }
        }
        string VerifyCodeImgConfig
        {
            get 
            {
                return configDict["VerifyCodeHttp12306"];
            }
        }
        public void SetLogDir(string dir) 
        {
            logDir = dir;
        }
        
        enum BtnCategory 
        {
            LogicData=1,
            BtnUpdateStation=2, //更新车站数据
            QueryTicket=3 ,//查询车票信息
            ListenTricket=4, //实时抢票
            RefreshVerifyCode=5,
            ClearTip=6,
            Login12306Account=7 //登录12306账户
        }
        public WebDataCaptureForm()
        {
            InitializeComponent();
            InitCfg();
            InitEle();
        }
        void InitCfg() 
        {
            ReadAppCfg();
            GetVerifyCodeImage();
        }
        void InitEle() 
        {
            btnLoadBaseData.Tag = BtnCategory.LogicData.ToString();
            btnLoadBaseData.Click += new EventHandler(Button_Click);
            //如何实现 Combobox元素在不点击下拉箭头前能输入文本
            cmbBeginStation.AutoCompleteMode = AutoCompleteMode.Append;
            dtpGoTime.Value = DateTime.Now;
            cmbBeginStation.Focus();
            Cursor = Cursors.Default;
            cmbBeginStation.DroppedDown = true;
            cmbBeginStation.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndex);
            cmbToStation.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndex);
            btnTicketQuery.Tag = BtnCategory.QueryTicket.ToString();
            btnTicketQuery.Click += new EventHandler(Button_Click);
            btnRefreshVerifyCode.Tag = BtnCategory.RefreshVerifyCode.ToString();
            btnRefreshVerifyCode.Click += new EventHandler(Button_Click);
            btnJob.Tag = BtnCategory.ListenTricket.ToString();
            btnJob.Click += new EventHandler(Button_Click);
            pbVerifyCodeImg.Click += new EventHandler(PictureBox_Click);
            btnUseRuleStation.Tag = BtnCategory.BtnUpdateStation.ToString();
            btnClearTip.Tag = BtnCategory.ClearTip.ToString();
            btnClearTip.Click += new EventHandler(Button_Click);
            btnLogin.Tag = BtnCategory.Login12306Account.ToString();
            btnLogin.Click += new EventHandler(Button_Click);
            LoadStation();//init station of information
            InitRequestParam();
            //绑定数据源
            foreach (StationName item in datas)
            {
                // items.Add(item.Name, item);
                cmbBeginStation.Items.Add(new KeyValuePair<string, StationName>(item.Name, item));
                cmbToStation.Items.Add(new KeyValuePair<string, StationName>(item.Name, item));
            }
            SetComboBoxDisplay(cmbBeginStation);
            SetComboBoxDisplay(cmbToStation);
        }
        void InitRequestParam()
        {
            HttpProtocolConfig config = new HttpProtocolConfig();
            verifyCodeParam = config.GetVerifyCodeParamStatic(TickekLogicConfigFullFile, VerifyCodeImgConfig);
        }
        void LoadStation() 
        {
            string stationDir=LogDir+"/"+ELogType.LogicLog+"/"+typeof(WebDataCaptureForm).Name+".txt";
            if (!File.Exists(stationDir))
            { //文件不存在,查询实时车站
                Button_Click(btnUseRuleStation, null);
            }
            string station= FileHelper.ReadFile(stationDir);
            GetStationModelFromString(station);
        }
        public void LoadBaseData() 
        {
           
        }
        void LoadStaionData() 
        {
            string url = "https://kyfw.12306.cn/otn/resources/js/framework/station_name.js?station_version=1.9028";
            string response = HttpClientExtend.HttpClientGet(url);//@bjb|北京北|VAP|beijingbei|bjb|0 前6项为一组（并且第六项索引可以舍弃）
            LoggerWriter.CreateLogFile(response, LogDir, ELogType.LogicLog, typeof(WebDataCaptureForm).Name);
            GetStationModelFromString(response);
        }
        void GetStationModelFromString(string response)
        {
            string[] station = response.Split('@');//首先各个车站数据分组,在分组的数据中从第二个数据项中开始查询（剔除空项）
            int arrLen = 6;
            datas = new List<StationName>();
            for (int i = 1; i < station.Length; i++)
            {
                StationName sta = new StationName();
                string[] arr = station[i].Split('|');
                if (arr.Length < arrLen)
                {
                    continue;
                }
                sta.SpellCode = arr[0];
                sta.Name = arr[1];
                sta.Code = arr[2];//查询车票时使用的代码
                sta.FullSpell = arr[3];
                sta.SampleSpell = arr[4];
                sta.Key = station[i];
                datas.Add(sta);
            }
        }
        void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag as string;
            BtnCategory bt;
            if (string.IsNullOrEmpty(tag)||!Enum.TryParse(tag,out bt))
            {
                return;
            }
            switch (bt) 
            {
                case BtnCategory.LogicData:
                    LoadBaseData();
                    break;
                case BtnCategory.BtnUpdateStation:
                    LoadStaionData();
                    break;
                case BtnCategory.QueryTicket://查询车票
                    GetPapeParam();//读取页面中的参数
                    QueryTicket();//根据参数查询数据
                    break;
                case BtnCategory.ListenTricket:
                    AsyncListen();
                    break;
                case BtnCategory.RefreshVerifyCode:
                    GetVerifyCodeImage();
                    break;
                case BtnCategory.ClearTip://清空提示内容的处理
                    ClearTip();
                    break;
                case BtnCategory.Login12306Account:
                    LoginAccountWithVerifyCode(selectIcon, verifyCodeParam, CheckVerifyCodeUrl);
                    break;
                
            }
        }
        void ClearTip() 
        {
            lsbTip.Items.Clear();
            rtbTip.Clear();
        }
        void AsyncListen() 
        {//异步进程进行监听
            int timeSpan = DefualtRefreshCalTicketTimeSpan;//默认轮询间隔
            QuartzJobService.QuartzJob job = new QuartzJob();
            job.CreateJobWithParam<QuartzJobService.JobDelegate<WebDataCaptureForm>>(new object[] { new BaseDelegate(DoJob), null }, DateTime.Now, 2, 0);
                  
        }
        void GetPapeParam() 
        {
            ticketParam = new leftTicketDTO();
            DateTime goTime = dtpGoTime.Value;
            ticketParam.from_station= GetComboBoxSelectItemKey(cmbBeginStation);
            ticketParam.to_station = GetComboBoxSelectItemKey(cmbToStation);
            ticketParam.train_date = goTime.ToString(CommonFormat.DateFormat);
            if (string.IsNullOrEmpty(ticketParam.purpose_codes))
            {
                ticketParam.purpose_codes = "ADULT";
            }
        }
        private string GetComboBoxSelectItemKey(ComboBox cmb) 
        {
            object obj = cmb.SelectedItem;
            if (obj != null)
            {
                KeyValuePair<string, StationName> st = (KeyValuePair<string, StationName>)obj;
                StationName station = st.Value as StationName;
               return station.Code;
            }
            return string.Empty;
        }
        void QueryTicket()
        {
            #region--参数检测
            rtbTip.Text = string.Empty;
            ResourceManager rm = Lang.ResourceManager;
            List<string> nullItem = new List<string>();
            if (string.IsNullOrEmpty(ticketParam.from_station))
            {
                nullItem.Add(rm.GetString(langTipFix + cmbBeginStation.Tag));
            }
            if (string.IsNullOrEmpty(ticketParam.to_station))
            {
                nullItem.Add(rm.GetString(langTipFix + cmbToStation.Tag));
            }
            if (nullItem.Count > 0)
            {
                List<string> requireItem = new List<string>();
                requireItem.Add(rm.GetString(langTipFix + cmbBeginStation.Tag));
                requireItem.Add(rm.GetString(langTipFix + cmbToStation.Tag));
                rtbTip.Text = string.Format(Lang.Tip_12306_QueryTicketIsRequired, string.Join(",", requireItem), string.Join(",", nullItem));
                return;
            }
            #endregion
            string url = InitQueryTicketParam(ticketParam);
            string json = HttpClientExtend.HttpClientGet(url);
            LoggerWriter.CreateLogFile(json, LogDir, ELogType.DataLog, DateTime.Now.ToString(CommonFormat.DateTimeIntFormat) + ".txt");
            leftTicketDTOResponse response = json.ConvertObject<leftTicketDTOResponse>();//不能序列化字典
            JObject jr = JObject.Parse(json);
            JObject jd = (JObject)jr["data"];//字典为data下数据  map
            //由于json序列化时不能将节点下的字典型json串进行逆序列化，次出使用中转形式先读取取字典内容，在填充到对象
            JObject jdic = (JObject)jd["map"];
            Dictionary<string, string> _Data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(jdic.ToString());
            if (response != null && response.data != null)
            {
                response.data.map = _Data;
            }
            //leftTicketDTOResponse responseNew= Newtonsoft.Json.JsonConvert.DeserializeObject<leftTicketDTOResponse>(json);
            /*
             * Newtonsoft
             其他信息: Cannot deserialize the current JSON object (e.g. {"name":"value"}) into type 'System.String[]' because the type requires a JSON array (e.g. [1,2,3]) to deserialize correctly.
             */
            foreach (var item in response.data.result)
            {
                GetCarTicket(item, response);
            }
            //存储列车座位信息
            //反射时构造函数没有提供参数可以进行创建
            ISyncTicketDataManage sync = IocHelper.CreateObjectCtorWithParam<ISyncTicketDataManage>(
                new object[]{AppCategory.WinApp,"Conn" },
                typeof(SyncTicketDataManage).Name,
                "TicketData.Manage",
                "CaptureManage.AppWin",//此为程序集，而不是命名空间
                NowAppDirHelper.GetNowAppDir(AppCategory.WinApp));
             
            //由于实现类中对于构造函数存在参数限定，此处需要特殊参数的传递
            sync.SaveCarWithSeatData(response);
            //读取列车过站信息
            //存储列车过站信息，绘制运动轨迹图

        }
        void GetCarTicket(string tick,leftTicketDTOResponse response) 
        {
            string[] cn=tick.Split('|');
            TicketSeatDataDto cr = new TicketSeatDataDto();
            if (!string.IsNullOrEmpty(cn[0]))
            {//本列车当前可以进行售票 
                cr.CanBuyTicket = true;
            }
            //ct.secretStr = cn[0];//secretStr 有车票供购买时使用的安全关键字
            //ct.buttonTextInfo = cn[1]; //按钮的文本内容//可能出现系统维护情况，直接在界面上显示不进行购票
            cr.train_no = cn[2];
            cr.station_train_code = cn[3];
            cr.start_station_telecode = cn[4];//始发站
            cr.end_station_telecode = cn[5];//终点站
            cr.from_station_telecode = cn[6];//乘客出发站
            cr.to_station_telecode = cn[7];//乘客目的地站
            cr.start_time = cn[8];//乘客随车出发时间
            cr.arrive_time = cn[9];//乘客到达时间
            cr.lishi = cn[10];//耗时
            cr.canWebBuy = cn[11];//IS_TIME_NOT_BUY 这样的字符串标识
            cr.yp_info = cn[12];//随机串
            cr.start_train_date = cn[13];//出发日期
            cr.train_seat_feature = cn[14];
            cr.location_code = cn[15];
            cr.from_station_no = cn[16];
            cr.to_station_no = cn[17];
            cr.is_support_card = cn[18];//可凭二代身份证直接进出站(不用取票)
            cr.controlled_train_flag = cn[19];//是否显示票价（1|2不显示票价）
            cr.gg_num =string.IsNullOrEmpty( cn[20]) ? cn[20] : "--";
            cr.gr_num = string.IsNullOrEmpty(cn[21]) ? cn[21] : "--";//高级软卧
            cr.qt_num = string.IsNullOrEmpty(cn[22]) ? cn[22] : "--";
            cr.rw_num = string.IsNullOrEmpty(cn[23]) ? cn[23] : "--";//软卧
            cr.rz_num = string.IsNullOrEmpty(cn[24]) ? cn[24] : "--";//软座
            cr.tz_num = string.IsNullOrEmpty(cn[25]) ? cn[25] : "--";//特等座
            cr.wz_num = string.IsNullOrEmpty(cn[26]) ? cn[26] : "--";//无座
            cr.yb_num = string.IsNullOrEmpty(cn[27]) ? cn[27] : "--";
            cr.yw_num = string.IsNullOrEmpty(cn[28]) ? cn[28] : "--";//硬卧
            cr.yz_num = string.IsNullOrEmpty(cn[29]) ? cn[29] : "--";//硬座
            cr.ze_num = string.IsNullOrEmpty(cn[30]) ? cn[30] : "--";//二等座
            cr.zy_num = string.IsNullOrEmpty(cn[31]) ? cn[31] : "--";//一等座
            cr.swz_num = string.IsNullOrEmpty(cn[32]) ? cn[32] : "--";//商务座特等座
            cr.srrb_num = string.IsNullOrEmpty(cn[33]) ? cn[33] : "--";//动卧
            //yyrw_num 高级动卧已取消？
            cr.yp_ex = cn[34];
            cr.seat_types = cn[35];
            Dictionary<string, string> station = response.data.map;
            cr.from_station_name = station[cn[6]];
            cr.to_station_name = station[cn[7]];
          //  ct.queryLeftNewDTO = cr;
            response.ticketData.Add(cr);
        }
        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                char c = e.KeyChar;
                // 8 '\b' 删除键  32 ' ' 空格
                if (c == 8 || c == 32)
                {
                    return;
                }
                //查找出符合输入关键字对应的车站
            }
            catch (Exception ex)
            { 
            
            }
        }
        private void CombobBox_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                ComboBox cmb = sender as ComboBox;
                cmb.Items.Clear();
                string text = cmb.Text;
                Dictionary<string, StationName> items = new Dictionary<string, StationName>();
                foreach (StationName item in datas)
                {
                    if (item.Key.Contains(text))
                    {
                        // items.Add(item.Name, item);
                        cmb.Items.Add(new KeyValuePair<string, StationName>(item.Name, item));
                    }
                }
                if (cmb.Items.Count == 0)
                {
                    return;
                }
                SetComboBoxDisplay(cmb);
                cmb.SelectionStart = text.Length;//设置继续输入的文本位置（没有改语句将出现倒叙文本）
                Cursor = Cursors.Default;
                cmb.DroppedDown = true;
            }
            catch (Exception ex)
            { 
            
            }
        }
        void SetComboBoxDisplay(ComboBox cmb) 
        {
            cmb.DisplayMember = "Key";
            cmb.ValueMember = "Value";
        }
        private void ComboBox_SelectedIndex(object sender, EventArgs e)
        {
            try
            {
                ComboBox cmb = sender as ComboBox;
            }
            catch (Exception ex)
            { 
            
            }
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="left"></param>
        private string InitQueryTicketParam(leftTicketDTO left) 
        {
            string url = "https://kyfw.12306.cn/otn/leftTicket/query?leftTicketDTO.train_date={train_date}&leftTicketDTO.from_station={from_station}&leftTicketDTO.to_station={to_station}&purpose_codes={purpose_codes}";
            string[] param = left.GetEntityProperties();
            foreach (string item in param)
            {
               url= url.Replace("{" + item+"}", left.GetPropertyValue(item));
            }
            return url;
        }
        void TrainTooStation() 
        {//列车过站信息
            string url = "https://kyfw.12306.cn/otn/czxx/queryByTrainNo?train_no=330000K5980V&from_station_telecode=BXP&to_station_telecode=GZQ&depart_date=2017-10-25";
        }
        void ReadAppCfg() 
        {
            string cfgDir = NowAppDirHelper.GetNowAppDir(AppCategory.WinApp);
            string file = TicketAppConfig.Ticket12306CfgReletive;//相对路径名称
            configDict = XmlFileHelper.ReadAppsettingSimulateConfig(cfgDir + "/" + file, "configuration/appSettings", "key", "value");
            Dictionary<string, string> brushCfg = XmlFileHelper.ReadXmlNodeItemInText(cfgDir + "/" + TicketAppConfig.BrushTicketCfg, "ticket");
        }
        void DoJob(object obj) 
        {
            if (rtbTip.InvokeRequired)
            {//是否是线程调度
                DoJob(obj);
                return;
            }
            GetVerifyCodeImage();
        }
        void GetVerifyCodeImage()
        {
            string url = configDict["GetVerifyCodeUrl"].Replace("{rand}", TicketDataPrepareManager.GenerateVerifyCodeGuid());
            //读取响应的流
            //将流转换为响应的图片显示到界面
            // WebResponse stream= HttpClientExtend.HttpWebRequestGet(url);
           
            string full = Logger.GenerateTimeOfFileName() + ".jpg";//如果操作的图片间隔在1秒之内会占用相同的文件名称导致异常
            string dir = VerifyCodeDir + "/" + Logger.GetNowDayIndex(); 
            HttpClientExtend.DownloadSmallFile(url, dir, full);
            // string response= HttpClientExtend.HttpClientGet(url);//实际上此处应该读取文件流转换为图片
            
            Image img = new Bitmap(dir + "/" + full);
            //存在bug   GDI+发生一般性错误详解
            img.Save(VerifyCodeImgFullDir);
            int w = img.Width;
            int h = img.Height;
            int x = smallImgNormalIn12306[0];
            int y = smallImgNormalIn12306[1];
            if (pbVerifyCodeImg.Width < w || pbVerifyCodeImg.Height < h)
            {
                pbVerifyCodeImg.Width = w;
                pbVerifyCodeImg.Height = h;
                logicPanel.Height = w + 80;
            }
            //生成图片副本
           // Graphics g = Graphics.FromImage(img); 
            pbVerifyCodeImg.BackgroundImage = img;
            //g.Dispose();
            string relative = configDict["ClickImgReleativePath"];
            string appDir = AppDir;
           
            //进行位置的切割分析
            avgX = w / x;//每个图片平均宽度
            avgY = h / y;//每个图片平均高度
            //验证码行列分析
            return;
            #region  --代码控制图标
            
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Point p = pbVerifyCodeImg.Location;
                    PictureBox pb = new PictureBox();
                    ((System.ComponentModel.ISupportInitialize)(pb)).BeginInit();
                    //logicPanel.Controls.Add(pb);
                    pb.Left = p.X + j * avgX + avgX/2;
                    pb.Top = p.Y + i * avgY + avgY/2;
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.BorderStyle = BorderStyle.FixedSingle;
                    pb.Image = Image.FromFile(appDir + "/" + relative);
                    // pb.Visible = false;
                    pb.Tag = EleTag.Icon.ToString();
                    pb.Name = "Icon" + ((i + 1) * (j + 1)).ToString();
                    pb.Size = new System.Drawing.Size(32, 32);
                    pb.Visible = true;
                    pb.BackColor = Color.Red;
                    pb.TabStop = false;
                    ((System.ComponentModel.ISupportInitialize)(pb)).EndInit();
                    logicPanel.Controls.Add(pb);
                }
            }
            logicPanel.ResumeLayout(false);
            #endregion
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;//鼠标点击
            rtbTip.AppendText("X="+ mouse.X + " Y=" + mouse.Y+"\r\n");
            if (mouse.Y < VerifyCodeTextHeight)
            {
                return;
            }
            int x = mouse.X / avgX;
            int y = mouse.Y / avgY;
            int column = smallImgNormalIn12306[0];//每行显示多少列图片
            int index = x + column * y;
            if (selectIcon.ContainsKey(index))
            {
                selectIcon.Remove(index);
            }
            else 
            {
                Point px = new Point(x * avgX + avgX * 3 / 7, y * avgY + avgY * 3 / 7);
                selectIcon.Add(index, px);
            }
           // lsbTip.Items.Add(index);
            //Control img= this.Controls.Find("Icon" + (x * y), false).FirstOrDefault();
            ControlIconDisplay(selectIcon);
            //
        }
        void ControlIconDisplay(Dictionary<int,Point> icons) 
        {//控制那些图标被选中
            Image img=Bitmap.FromFile(VerifyCodeImgFullDir);
            Graphics g = Graphics.FromImage(img);
            Size iconSize = new Size(ClickIconSizeIn12306[0], ClickIconSizeIn12306[1]);
            foreach (KeyValuePair<int,Point> item in icons)
            {
                Infrastructure.ExtService.ImageHelper.GraphiscImg(g,IconImgFullDir, iconSize, item.Value);
            }
            pbVerifyCodeImg.Image = img;
            g.Dispose();
        }
        void LoginAccountWithVerifyCode(Dictionary<int,Point> selectIcon,VerifyCode param,string url) 
        {//提取选择的验证码图片坐标
            List<string> px = new List<string>();
            foreach (KeyValuePair<int,Point> item in selectIcon)
            {
                px.Add(item.Value.X + "," + item.Value.Y);
            }
            string select = string.Join(",", px);
            param.answer = select;

            string json = param.ConvertJson();
            string head = @"Accept:application/json, text/javascript, */*; q=0.01
Accept-Encoding:gzip, deflate, br
Accept-Language:zh-CN,zh;q=0.8
Connection:keep-alive
Content-Length:51
Content-Type:application/x-www-form-urlencoded; charset=UTF-8
Cookie:_passport_session=47d63662000d48f5ad967765bfb47b8b9382; _passport_ct=bf013d1f23774a43bc968ae3e2675904t0269; _jc_save_fromStation=%u5317%u4EAC%u897F%2CBXP; _jc_save_toStation=%u6DF1%u5733%2CSZQ; _jc_save_fromDate=2017-11-19; _jc_save_toDate=2017-11-19; _jc_save_wfdc_flag=dc; route=c5c62a339e7744272a54643b3be5bf64; BIGipServerotn=217055754.50210.0000; RAIL_EXPIRATION=1512095461498; RAIL_DEVICEID=EzgegZVdgB_T7ZG4zCPom_T9XgfrU2pfqAAuoaXpl8fEVAnnnWmws9yX2apr16Niw-HvJ1a5I7ze8gAfo83_dscQ7wiBfop10YDYOfHbbJH6D6tDj61aHmB1JSCPzFnvpH3TLjHPiVzciX714Ck_MQpNw0QBO3q4; BIGipServerpool_passport=300745226.50215.0000
Host:kyfw.12306.cn
Origin:https://kyfw.12306.cn
Referer:https://kyfw.12306.cn/otn/login/init
User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36
X-Requested-With:XMLHttpRequest";
            string answer= HttpClientExtend.RunPosterContainerHeaderHavaParam(url, head,json,null);
            LoggerWriter.CreateLogFile(answer, LogDir, ELogType.HttpResponse, typeof(WebDataCaptureForm).Name);
        }
    }
    
}
