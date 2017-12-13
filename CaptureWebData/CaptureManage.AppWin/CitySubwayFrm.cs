using Infrastructure.ExtService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataHelp;
using HttpClientHelper;
using Domain.CommonData;
using TicketData.Model;
namespace CaptureManage.AppWin
{
    public partial class CitySubwayFrm : Form
    {
        Dictionary<string, string> configDict = new Dictionary<string, string>();
        Dictionary<string, string> appsettingMapModel = new Dictionary<string, string>();
        public CitySubwayFrm()
        {
            InitializeComponent();
            ReadAppCfg();
            btnQuerySubwayData.Click += new EventHandler(Button_Click);
        }
        private void Button_Click(object sender,EventArgs e) 
        {
            //get请求
            foreach (KeyValuePair<string,string> item in configDict)
            {
                string response= HttpClientExtend.HttpClientGet(item.Value);
                if (string.IsNullOrEmpty(response) || response == "[]")
                {//数据集合为空
                    continue;
                }
                LoggerWriter.CreateLogFile(response, new AssemblyLoggerDir().LogDir + "/" + typeof(CitySubwayFrm).Name, ELogType.HttpResponse, item.Key);
            }
            ReadSql(typeof(SubwaySiteData));
            //启用延时 异步进程 读取xml文件内数据

            //读取各个文件中的配置项

            //特顺处理情形 -> xml文件中含有地体线节点信息 没一条线路 又含有各站点信息的xml解析在到数据存储解析 http://map.bjsubway.com:8080/subwaymap2/public/subwaymap/beijing.xml
        }
        void ReadAppCfg()
        {
            string cfgDir = NowAppDirHelper.GetNowAppDir(AppCategory.WinApp);
            string file = TicketAppConfig.BeijingSubwayCfgReletive;//相对路径名称
            configDict = XmlFileHelper.ReadAppsettingSimulateConfig(cfgDir + "/" + file, "configuration/appSettings", "key", "value");
            Dictionary<string, string> brushCfg = XmlFileHelper.ReadXmlNodeItemInText(cfgDir + "/" + TicketAppConfig.BrushTicketCfg, "ticket");
            appsettingMapModel = XmlFileHelper.ReadAppsettingSimulateConfig(cfgDir + "/" + file, "configuration/appMapModel", "appSettings", "model");
        }
        void ReadSql(Type table) 
        {
            string xml = AppSettingItem.AppDataSqlXml;//xml 文件
            //读取进行操作的SQL语句
            string nodeName = BaseCfgItem.AppCfgXmlNodeFormat + "/" + table.Name;
            //读取节点项
            string addSqlCmd= XmlFileHelper.ReadXmlItemValue(xml, nodeName, "key", "value", "Insert");
        }
    }
}
