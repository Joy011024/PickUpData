using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.RedisRespority;
using ApplicationService.IDataService;
using Domain.CommonData;
using Infrastructure.ExtService;
namespace AppService.RedisService
{
    public class RedisService : ICategroyService
    {
        public string ConnString { get; set; }

        public IEnumerable<CategoryData> QueryCityCategory(string key)
        {
            List<CategoryGroup> objectItems = redis.GetRedisCacheItem<List<CategoryGroup>>(key);
            return new List<CategoryData>();
        }
        RedisCacheDBContext redis;
        public RedisService(string connString)
        {
            ConnString=connString;
            redis = new RedisCacheDBContext(connString);
            //string defaultCountryNode = GetCagetoryDataFileNameOrRedisItem(obj, redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat));

            //redis.SetRedisItem(GetCagetoryDataFileNameOrRedisItem(item.Root,
            //               redisItemOrFileNameFormat(SystemConfig.RedisValueIsJsonFormat)), jsonNode);
        }
        
        
    }
}
