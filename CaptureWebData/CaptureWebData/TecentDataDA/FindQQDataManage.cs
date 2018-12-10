using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataHelp;
using Domain.CommonData;
using ApplicationService.DataService;
using ApplicationService.IDataService;
using AppService.RedisService;
using ApplicationService.SQLiteService;
namespace CaptureWebData
{
    public class FindQQDataManage
    {
        public string ConnString { get; set; }
        public FindQQDataManage(string connString) 
        {
            ConnString = connString;
        }
        public FindQQResponse SaveFindQQ(string findQQResponseJson) 
        {
            try 
            {
                FindQQResponse find = findQQResponseJson.ConvertObject<FindQQResponse>();
                List<FindQQ> qqs = find.result.buddy.info_list;
                new DataFromManage().SaveQQData(qqs);
                 
                return find;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
        public int CountTodayPickUp() 
        {
            return 0;// new FindQQDataService(ConnString).CountTodayPickUp();
        }
        public PickUpStatic TodayStatic() 
        {


            return new PickUpStatic();// new FindQQDataService(ConnString).TodayStaticData();
        }
        public void GatherHeadImage(string uin,string imgDir) 
        {
            
        }
    }
    #region 数据库切换
    public class DataFromManage
    {
        /// <summary>
        /// 进行数据库的适配
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        private ICategroyService SwitchBasicDataSource(string dbType)
        {
            ICategroyService cs = null;
            //当前数据库
            switch (dbType)
            { 
                case DBType.MySQL:
                    break;
                case DBType.SQLite:
                    cs = new CategorySQLiteService ("TecentDASQLite");
                    break;
                case DBType.SQLServer:
                    cs = new CategoryDataService(new ConfigurationItems().TecentDA);
                    break;
                case DBType.Redis:
                    cs = new RedisService(string.Format( "IP={0};Port={1};Psw={2}", SystemConfig.RedisIp, SystemConfig.RedisPort, SystemConfig.RedisPsw));
                    break;
                case DBType.IOFile:
                    cs = new CategoryDataServiceInIOFile();
                    break;
            }
            return cs;
        }
        public void SaveQQData(List<FindQQ> datas)
        {
            IQQDataService cs = SwitchQQDataService();
            //当前数据库
            switch (SystemConfig.MainDBType)
            {
                case DBType.MySQL:
                    break;
                case DBType.SQLite:
                    //cs = new SQLiteQQDataService("TecentDASQLite");
                    List<FindQQDataTable> tables = datas.Select(s => s.ConvertMapModel<FindQQ, FindQQDataTable>(true)).ToList();
                    cs.SaveQQ(tables);
                    break;
                case DBType.SQLServer:
                   // cs = new  QQDataService(new ConfigurationItems().TecentDA);
                    List< FindQQDataTable> fins= datas.Select(s => s.ConvertMapModel<FindQQ, FindQQDataTable>(true)).ToList();
                    cs.SaveQQ(fins);
                    break;
            } 
        }
        /// <summary>
        /// 获取城市数据
        /// </summary>
        /// <returns>城市数据列表【null情况出现则是数据库切换没有成功】</returns>
        public List<CategoryData> QueryCities()
        {
            ICategroyService cs = SwitchBasicDataSource(SystemConfig.BasicDBType);
            string key = "City";
            if (cs != null)
                return cs.QueryCityCategory(key).ToList();
            return null;
        }
        Dictionary<string, string> dbTypeSwitchCityKey = new Dictionary<string, string>();
        private void InitCityKey()
        {
            // string file = SystemConfig.ExeDir + @"\Service\CategoryGroup\China.txt";
            dbTypeSwitchCityKey.Add(DBType.IOFile, SystemConfig.ExeDir + @"\Service\CategoryGroup\China.txt");
            dbTypeSwitchCityKey.Add(DBType.SQLite, "City");
            dbTypeSwitchCityKey.Add(DBType.SQLServer, "City");
            dbTypeSwitchCityKey.Add(DBType.Redis, "City");
        }
        public IQQDataService SwitchQQDataService()
        {
            IQQDataService cs = null;
            switch (SystemConfig.MainDBType)
            {
                case DBType.MySQL:
                    break;
                case DBType.SQLite:
                    cs = new SQLiteQQDataService("TecentDASQLite");
                    break;
                case DBType.SQLServer:
                     cs = new  QQDataService(new ConfigurationItems().TecentDA);
                    break;
            }
            return cs;
        }
        public PickUpStatic TodayStatic()
        {
            IQQDataService ds = SwitchQQDataService();
            return ds.TodayStaticData();
        }
    }
    public class DBType
    {
        public const string SQLite = "SQLite";
        public const string SQLServer = "SQLServer";
        public const string MySQL = "MySQL";
        public const string Redis = "Redis";
        public const string IOFile = "IOFile";
    }
    #endregion

}
