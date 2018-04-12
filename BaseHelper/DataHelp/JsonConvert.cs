using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
namespace DataHelp
{
    public static class JsonConvert
    {
        public static bool Result { get; private set; }
        public static string Message { get; private set; }
        private static void Init() 
        {
            Result = false;
            Message = string.Empty;
        }
        /// <summary>
        /// 对象转换为json字符串
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="entity">实体对象的内容</param>
        /// <returns>json字符串</returns>
        public static string ConvertJson<T>(this T entity)
        {
            Init();
            string json = string.Empty;
            try
            {
                DataContractJsonSerializer djs = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                djs.WriteObject(ms, entity);
                json = Encoding.UTF8.GetString(ms.ToArray());
                Result = true;
            }
            catch (Exception ex) 
            {
                Message = ex.ToString();
            }
            return json;
        }
        public static T ConvertObject<T>(this string json) 
        {
            Init();
            T entity = default(T);
            try
            {
                DataContractJsonSerializer djs = new DataContractJsonSerializer(typeof(T));
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                MemoryStream ms = new MemoryStream(bytes);
                entity=(T) djs.ReadObject(ms);
                Result = true;
            }
            catch (Exception ex) 
            {
                Message = ex.ToString();
            }
            return entity;
        }
        private static void  GetPropertyItems(this string jsonString) 
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(object));
            Stream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            object obj= serializer.ReadObject(mStream);
        }
    }
}
