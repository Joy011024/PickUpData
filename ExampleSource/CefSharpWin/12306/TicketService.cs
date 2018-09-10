using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace CefSharpWin
{
    public class TicketService
    { }
    public static class DataConvertService
    {
        public static T ConvertData<T>(this string json) where T : class
        {
           return  Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
    /// <summary>
    /// 12306 联系人返回实体
    /// </summary>
    public class Ticket12306Resonse
    {
        public bool status { get; set; }
        public int httpstatus { get; set; }
        public string validateMessagesShowId { get; set; }
        public Passengers data { get; set; }
        public string[] messages { get; set; }
        public ValidateMessages validateMessages { get; set; }
    }
    /// <summary>
    /// 乘车人列表
    /// </summary>
    public class Passengers
    {
        public List<Rider> normal_passengers { get; set; }
        public bool isExist { get; set; }
        public string exMsg { get; set; }
        public string[] two_isOpenClick { get; set; }
        public string[] other_isOpenClick { get; set; }
        public string[] dj_passengers { get; set; }

    }
    /// <summary>
    /// 乘车人
    /// </summary>
    public class Rider
    {
        public string code { get; set; }
        /// <summary>
        /// 乘车人姓名
        /// </summary>
        public string passenger_name { get; set; }
        public string sex_code { get; set; }
        public string sex_name { get; set; }
    }
    public class ValidateMessages
    {
        
    }
    /*
    {
    "validateMessagesShowId":"_validatorMessage",
    "status":true,
    "httpstatus":200,
    "data":{
        "isExist":true,
        "exMsg":"",
        "two_isOpenClick":["93","95","97","99"],
        "other_isOpenClick":["91","93","98","99","95","97"],
        "normal_passengers":[
            {
                "code":"5",
                "passenger_name":"张三",
                "sex_code":"M",
                "sex_name":"男",
                "born_date":"1992-03-01 00:00:00",
                "country_code":"CN",
                "passenger_id_type_code":"1",
                "passenger_id_type_name":"中国居民身份证",
                "passenger_id_no":"362320199303011391",
                "passenger_type":"1",
                "passenger_flag":"0",
                "passenger_type_name":"成人",
                "mobile_no":"18280826195",
                "phone_no":"",
                "email":"158055983@qq.com",
                "address":"",
                "postalcode":"",
                "first_letter":"",
                "recordCount":"15",
                "total_times":"99",
                "index_id":"0",
                "gat_born_date":"",
                "gat_valid_date_start":"",
                "gat_valid_date_end":"",
                "gat_version":""
               }
             ],
            "dj_passengers":[]
            },"messages":[],"validateMessages":{}}

    */
}
