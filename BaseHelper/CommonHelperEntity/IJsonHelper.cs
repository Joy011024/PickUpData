using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
namespace CommonHelperEntity
{
    public interface IJsonHelper
    {
    }
    public static class JsonHelperExtend 
    {
        public static string ConvertJsonString(this object helper) 
        {
            if (helper == null) { return null; }
            DataContractJsonSerializer json = new DataContractJsonSerializer(helper.GetType());
            MemoryStream ms = new MemoryStream();
            json.WriteObject(ms, helper);
            string data = Encoding.UTF8.GetString(ms.ToArray());
            return data;
        }
        /// <summary>
        /// 对实体进行json序列化操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string JsonSerialization<T>(this T helper) where T : IJsonHelper 
        {
            if (helper == null) { return null; }
            DataContractJsonSerializer json = new DataContractJsonSerializer(helper.GetType());
            MemoryStream ms = new MemoryStream();
            json.WriteObject(ms, helper);
            string data = Encoding.UTF8.GetString(ms.ToArray());
            return data;
        }
        /// <summary>
        /// 进行json字符串反序列化为对应的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonDeserializetion<T>(this T helper, string jsonString) where T : IJsonHelper 
        {
            if (string.IsNullOrEmpty(jsonString)) { return default(T); }
            try
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
                MemoryStream ms = new MemoryStream(bytes);
                T obj = (T)json.ReadObject(ms);
                return obj;
            }
            catch (Exception ex) 
            {
                return default(T);
            }
        }
    }
}
