using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace EFSwitchDBApp
{
    public partial class FrmConnSQLite : Form
    {
         
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
        public FrmConnSQLite()
        {
            InitializeComponent();
            Load += new EventHandler(TecentDataFrm_Load);

        }
        private void TecentDataFrm_Load(object sender, EventArgs e)
        {
            try
            {
                //InitBaseConfig();
                InitSQLite();
                Init();
            }
            catch (Exception ex)
            { /*
                未能加载文件或程序集“EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089”或它的某一个依赖项。系统找不到指定的文件。
                 */
              
            }
        }
        void ReadCountryCity() 
        {
        
        }
        void InitSQLite() 
        {
            UinDataService uis = new UinDataService();
            uis.QueryCityDataByExt();
        }
        
        void Init()
        {
            
        } 
       
    }
}
