using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using Domain.CommonData;
using System.IO;
using Newtonsoft.Json;
namespace CaptureWebData
{
    
    public class RedisCacheManageFun
    {
       
        public void TestSetOrStore(int cacheSize) 
        {
            string format = "yyyy-MM-dd HH:mm:ss ff";
            Console.WriteLine("\tSet start" + DateTime.Now.ToString(format));
            
            DateTime exprise = DateTime.Now.AddDays(-1);
            //Client.Set("CacheKey", cacheSize, exprise);
            //Client.Set("127.0.0.1", cacheSize, exprise);
            for (int i = 0; i < cacheSize; i++)
            {
                if (i < 5999)
                {
                    //Client.Set("SetKey" + i, i, exprise);
                }
                if (i>= 5999) 
                {
                  //  Client.Set("SetKey" + i, i, exprise);
                  
                }
            }
            Console.WriteLine("\tSet end" + DateTime.Now.ToString(format));
            Console.WriteLine("\tstore start" + DateTime.Now.ToString(format));
            for (int i = 0; i < cacheSize; i++)
            {
                //Client.Store(new KeyValuePair<string, int>("KeyValuePair" , i));
            }
            Console.WriteLine("\tstore end" + DateTime.Now.ToString(format));
            List<int> data = new List<int>();
            data.Add(1);
            data.Add(10);
            data.Add(55);
            //Client.Set<List<int>>("arar", data);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("int", typeof(int).Name);
            dic.Add("string", typeof(string).Name);
            //Client.Set < Dictionary<string, string>>("dic", dic);
            //Client.Set<KeyValuePair<string, string>[]>("keyvalue", dic.ToArray());
            //Client.Store<KeyValuePair<string, string>>(new KeyValuePair<string, string>("store", "storeKey"));

            Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();
            dict.Add("table", dic);
            //Client.Store<Dictionary<string, Dictionary<string, string>>>( dict);
            //Client.Store<KeyValuePair<string,int>>(new  KeyValuePair<string,int>("Year",DateTime.Now.Year));
        }
        public class RedisTable 
        {
            public string ColumnName { get; set; }
            public int Index { get; set; }
            public int Value { get; set; }
            public string ColumnText { get; set; }
            public string ColumnImage { get; set; }
        }
        /// <summary>
        /// 从文件中读取城市数据并存储到redis中
        /// </summary>
        /// <param name="dir">文件命名规则（name=id）的路径</param>
        /// <param name="redis">redis数据库配置项</param>
        /// <param name="jsonItemSave">是存储json串还是实体对象作为redis数据项的value -> false :项名称  CategoryGroup.Objcet=id,CategoryGroup.Json=id</param>
        public  void SetCityCacheFromFile(string dir, string  redisConnstring, bool jsonItemSave)
        {
            DirectoryInfo dis = new DirectoryInfo(dir);
            DirectoryInfo[] countrys = dis.GetDirectories();//获取全部的子目录
            FileInfo[] fis = dis.GetFiles(); //提取目录下的文件
            foreach (var file in fis)
            {
                string ext = file.Extension;
                string[] fn = file.Name.Replace(ext, "").Split('=');
                if (fn.Length < 2)
                {
                    continue;
                }
                string id = fn[1];
                string json = FileHelper.ReadFile(file.FullName);
                if (string.IsNullOrEmpty(json))
                {
                    continue;
                }
                 
                
            }
            foreach (var item in countrys)
            {//
                string[] names = item.Name.Split('=');
                if (names.Length < 2)
                {
                    continue;
                }
                SetCityCacheFromFile(item.FullName, redisConnstring, jsonItemSave);
            }
        }
        
    }
}
