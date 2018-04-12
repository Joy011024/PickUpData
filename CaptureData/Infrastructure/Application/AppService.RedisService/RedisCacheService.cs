using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
namespace AppService.RedisService
{
    public class RedisCacheService
    {
        string RedisIp;
        int RedisPort;
        string RedisPsw;
        RedisClient Client;
        bool CanConnection;
        public bool HavaCacheItem;
        public RedisCacheService(string ip,int port,string psw) 
        {
            RedisIp = ip;
            RedisPort = port;
            RedisPsw = psw;
            if (string.IsNullOrEmpty(psw))
            {
                Client = new RedisClient(ip, port);
            }
            else
            {
                Client = new RedisClient(ip, port, psw);
            }
            CanConnection = Client.IsSocketConnected();//是否连接成功
        }
        public void SetRedisItem(string cacheItem,object obj) 
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
            byte[] bts= Client.Get(cacheItem);
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
            return  Client.Get<T>(key);
        }
    }
}
