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
namespace CaptureManage.AppWin
{
    public partial class CitySubwayFrm : Form
    {
        Dictionary<string, string> configDict = new Dictionary<string, string>();
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
                LoggerWriter.CreateLogFile(response, new AssemblyLoggerDir().LogDir + "/" + typeof(CitySubwayFrm).Name, ELogType.HttpResponse, item.Key);
            }
        }
        void ReadAppCfg()
        {
            string cfgDir = NowAppDirHelper.GetNowAppDir(AppCategory.WinApp);
            string file = TicketAppConfig.BeijingSubwayCfgReletive;//相对路径名称
            configDict = XmlFileHelper.ReadAppsettingSimulateConfig(cfgDir + "/" + file, "configuration/appSettings", "key", "value");
            Dictionary<string, string> brushCfg = XmlFileHelper.ReadXmlNodeItemInText(cfgDir + "/" + TicketAppConfig.BrushTicketCfg, "ticket");
        }
    }
}
