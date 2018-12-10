using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationService.IDataService;
using Domain.CommonData;
using Infrastructure.EFSQLite;
using DataHelp;
namespace ApplicationService.SQLiteService
{
    public class CategorySQLiteService : ICategroyService
    {
        public string ConnString { get; set; }
        public CategorySQLiteService(string connString)
        {
            ConnString = connString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"> "City"</param>
        /// <returns></returns>
        public IEnumerable<CategoryData> QueryCityCategory(string key)
        {
            DBReporistory<CategoryData> main = new DBReporistory<CategoryData>(ConnString);
            IEnumerable<CategoryData> data = main.DoQuery<CategoryData>().Where(t => !t.IsDelete && (!string.IsNullOrEmpty(key)? t.ItemType == key:true) && !string.IsNullOrEmpty(t.Name));
            return data;
        }
    }
    public class SQLiteQQDataService : IQQDataService
    {
        public string ConnString { get; set; }
        public SQLiteQQDataService(string connString)
        {
            ConnString = connString;
        }
        public void SaveQQ (List<FindQQDataTable> data) 
        {
            try
            {
                //由于sqlite不支持guid数据存储：直接存储会变成乱码

                DBReporistory<TecentQQData> main = new DBReporistory<TecentQQData>(ConnString);
                DateTime now = DateTime.Now;
                List<TecentQQData> tcs = new List<TecentQQData>();
                foreach (FindQQDataTable item in data)
                {
                    NoIDQQData noKey = item.ConvertMapModel<FindQQDataTable, NoIDQQData>();
                    TecentQQData tc = noKey.ConvertMapModel<NoIDQQData, TecentQQData>();
                    tc.ID = GenerateId();
                    tc.CreateTime = now;
                    if (string.IsNullOrEmpty(tc.Url))//没有采集到该账户的头像数据
                        tc.ImgType = -1;
                    tc.DayInt = int.Parse(now.ToString("yyyyMMdd"));
                    tcs.Add(tc);
                }
                main.AddList(tcs.ToArray());
            }
            catch (Exception ex)
            {
                /*  原因：sqlite不支持实体列数据类型为Guid的情形
                 One or more validation errors were detected during model generation:

Infrastructure.EFSQLite.TecentQQData: : EntityType 'TecentQQData' has no key defined. Define the key for this EntityType.
Entity: EntityType: EntitySet 'Entity' is based on type 'TecentQQData' that has no keys defined.

                 */
            }
        }
        public string GenerateId()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }
        public class NoIDQQData
        {
            public DateTime CreateTime { get; set; }
            public int Age { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public int Distance { get; set; }
            public int Face { get; set; }
            public string  Gender { get; set; }
            public string Nick { get; set; }
            public string Province { get; set; }
            public int Stat { get; set; }
            public string Uin { get; set; }
            [System.ComponentModel.DataAnnotations.Schema.Column("HeadImageUrl")]
            public string Url { get; set; }
            /// <summary>
            /// 头像类型【默认0 当没有采集到头像数据时设置该值为-1】
            /// </summary>
            public int ImgType { get; set; }
            #region 增加这字段是为了兼容数据库表创建的形式不符合采集数据
            public int GatherImageErrorNum { get; set; }
            public int IsGatherImage { get; set; }
            [DescriptionSort("数据采集日期数值（精确到天），这个字段在数据量大的时候很有用")]
            public int DayInt { get; set; }
            #endregion
        }
        public class TecentQQData : NoIDQQData
        {
            public string ID { get; set; } //使用guid出现乱码
        }

        public PickUpStatic TodayStaticData()
        {
            string sql = @"select 
( SELECT  count(Id)   FROM TecentQQData)  as DBTotal,
(select count( distinct(uin))    FROM TecentQQData) as DBPrimaryTotal,
(select count(Id)   from TecentQQData where dayint=cast( strftime('%Y%m%d', 'now')  as int)) as IdTotal ,
(select count(distinct(uin)) from TecentQQData where dayint=cast( strftime('%Y%m%d', 'now')  as int))  as Total,
(select cast( strftime('%Y%m%d', 'now')  as int) ) as StaticDay";
            DBReporistory<PickUpStatic> sync = new DBReporistory<PickUpStatic>(ConnString);
            PickUpStatic[] data= sync.ExecuteSQL<PickUpStatic>(sql).ToArray();
            PickUpStatic ps = new PickUpStatic();
            /*
             One or more validation errors were detected during model generation:

Infrastructure.EFSQLite.PickUpStatic: : EntityType 'PickUpStatic' has no key defined. Define the key for this EntityType.
Entity: EntityType: EntitySet 'Entity' is based on type 'PickUpStatic' that has no keys defined.

             */
            if (data.Length > 0)
            {
                ps= data[0];
            }
            return ps;
        }
    }
   
}
