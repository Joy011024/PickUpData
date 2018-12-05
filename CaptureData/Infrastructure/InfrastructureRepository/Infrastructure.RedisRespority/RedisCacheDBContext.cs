using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using Newtonsoft.Json;
namespace Infrastructure.RedisRespority
{
    public class RedisCacheItem
    {
        public string CacheKey { get; set; }//缓存关键字
        public object CacheValue { get; set; }//缓存值
        public TimeSpan? CacheExpires { get; set; }//缓存过期时间
    }
    public class RedisCacheDBContext
    {
        string RedisIp;
        int RedisPort;
        string RedisPsw;
        RedisClient Client;
        bool CanConnection;
        public bool HavaCacheItem;
        public RedisCacheDBContext(string connString)
        {
            string[] info = connString.Split(';');

            RedisIp = info[0].Split('=')[1].Trim();
            RedisPort = int.Parse(info[1].Split('=')[1].Trim());
            if (info.Length<3)
            {
                Client = new RedisClient(RedisIp, RedisPort);
            }
            else
            {
                RedisPsw = info[2].Split('=')[1].Trim();
                Client = new RedisClient(RedisIp, RedisPort, RedisPsw);
            }
            CanConnection = Client.IsSocketConnected();//是否连接成功
        }
        public void AddCacheItem(RedisCacheItem cache)
        {
            if (cache.CacheExpires.HasValue)
            {
                TimeSpan e = cache.CacheExpires.Value;
                DateTime expires = DateTime.Now.AddSeconds(e.TotalMilliseconds);
                Client.Set(cache.CacheKey, cache.CacheValue, expires);
                //  Client.DeleteAll<CategoryGroup>();//设置无效
                // Client.FlushDb();
            }
            else
            {
                Client.Set(cache.CacheKey, cache.CacheValue, DateTime.Now.AddDays(-1));
            }
        }
        public void SetRedisItem(string cacheItem, object obj)
        {
            if (!CanConnection)
            { }
            DateTime exprise = DateTime.Now.AddDays(1);
            Client.Set(cacheItem, obj, exprise);
        }
        public string GetRedisItemString(string cacheItem)
        {
            HavaCacheItem = false;
            if (!CanConnection)
            { }
            byte[] bts = Client.Get(cacheItem);
            if (bts == null)
            { //数据库中没有该项缓存数据
                return null;
            }
            HavaCacheItem = true;
            return Encoding.UTF8.GetString(bts);
        }
        public string GetCacheByKey(string key)
        {
            byte[] bs = Client.Get(key);
            //将存储的数据转换为字符内容
            return string.Empty;
        }
        public T GetRedisCacheItem<T>(string key)
        {
            return Client.Get<T>(key);
        }
        public T GetCacheItem<T>(string cacheKey) where T : class
        {
            byte[] bts = Client.Get(cacheKey);
            if (bts == null)
            {//数据库中缓存已到期
                return System.Activator.CreateInstance<T>();
            }
            string json = Encoding.UTF8.GetString(bts);
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
    }
}
