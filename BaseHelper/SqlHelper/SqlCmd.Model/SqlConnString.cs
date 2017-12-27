using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
namespace SqlCmd.Model
{
    public class SqlConnString
    {
        /// <summary>
        /// 数据库实例名
        /// </summary>
        public string DataSource { get; set; }
        /// <summary>
        /// 数据库名
        /// </summary>
        public string InitialCatalog{get;set;}
        /// <summary>
        /// 用户
        /// </summary>
        public string UserId{get;set;}
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        public int ConnStringEmptyFields { get; private set; }
        /// <summary>
        /// 获取该对象组装的数据库连接字符串，调用后ConnStringEmptyFields记录存在多少个空字段
        /// </summary>
        /// <returns></returns>
        public string GetConnString() 
        {
            string format = "Data Source={0};Initial Catalog={1};User Id={2};Password={3};";
            int existEmpty = 0;
            if ((++existEmpty) > 0 && !string.IsNullOrEmpty(DataSource))
            {
                existEmpty--;
                format=format.Replace("{0}", DataSource);

            }
            if ((++existEmpty) > 0 && !string.IsNullOrEmpty(InitialCatalog))
            {
                existEmpty--;
                format=format.Replace("{1}", InitialCatalog);
            }
            if ((++existEmpty) > 0 && !string.IsNullOrEmpty(UserId))
            {
                existEmpty--;
                format = format.Replace("{2}", UserId);
            }
            if ((++existEmpty) > 0 && !string.IsNullOrEmpty(Password))
            {
                existEmpty--;
                format = format.Replace("{3}", Password);
            }
            ConnStringEmptyFields = existEmpty;
            return format;
        }
    }
}
