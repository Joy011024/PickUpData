using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using Infrastructure.EFSQLite;
using Domain.CommonData;
namespace CaptureWebData
{
    public class UinDataService
    {
        /// <summary>
        /// 使用System.Data.SQLite 访问sqlite数据库
        /// </summary>
        public void GetCityDatas() 
        {
            try
            {
                ConfigurationItems cfg = new ConfigurationItems();
                SQLiteConnection conn = new SQLiteConnection(cfg.SqliteDbConnString);
                conn.Open();
                string sql = "select * from CategoryData";
                SQLiteCommand comq = new SQLiteCommand(sql, conn);
                SQLiteDataAdapter dap = new SQLiteDataAdapter();
                dap.SelectCommand = comq;
                DataSet ds = new DataSet();
                dap.Fill(ds);
                conn.Clone();
            }
            catch (Exception ex)
            {

            }
        }
        public void QueryCityDataByExt()
        {
           
            try
            {
                ConfigurationItems cfg = new ConfigurationItems();
                DBReporistory<Domain.CommonData.CategoryData> cds = new DBReporistory<CategoryData>("TecentDASQLite");
                List< Domain.CommonData.CategoryData> ds= cds.DoQuery< Domain.CommonData.CategoryData>().ToList();

            }
            catch (Exception ex)
            {

            }
        }
    }
    public class SyncDataHelper 
    {
        public static void SyncCategory(List<CategoryData> list) 
        {
            try
            {
                //string json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                /*
                 Unable to complete operation. The supplied SqlConnection does not specify an initial catalog or AttachDBFileName.
                 */

                DBReporistory<CategoryData> md = new DBReporistory<CategoryData>(new ConfigurationItems().SqliteDbConnString);
                List<CategoryData> dstas=  md.DoQuery<CategoryData>().ToList();
                //一次操作数据量过大需要分批次
                List<CategoryData> group = new List<CategoryData>();
                for (int i = 0; i < list.Count; i++)
                {
                    group.Add(list[i]);
                    if (i % 2==0)
                    {
                        md.AddList(group.ToArray());
                        group.Clear();
                    }
                }
            }
            catch (Exception ex)
            { 
            
            }
        }
    }
  
}
