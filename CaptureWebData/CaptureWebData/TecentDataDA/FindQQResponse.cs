using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
namespace CaptureWebData
{
    public class FindQQ
    {
        public int age { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public int distance { get; set; }
        public int face { get; set; }
        public string gender { get; set; }
        public string nick { get; set; }
        public string province { get; set; }
        public int stat { get; set; }
        public string uin { get; set; }
        public string url { get; set; }
    }
    public class FindQQData 
    {
        public List<FindQQ> info_list { get; set; }
        public int count { get; set; }
        public int endflag { get; set; }
        public int online { get; set; }
        public int page { get; set; }
        public int sessionid { get; set; }
        public int totalnum { get; set; }

    }
    public class FindQQResult 
    {
        public FindQQData buddy { get; set; }
        public int sret { get; set; }
    }
    public class FindQQResponse 
    {
        public FindQQResult result { get; set; }
        public int retcode { get; set; }
    }
    public class UinGrounpItem 
    {
        public int app_privilege_flag { get; set; }
        public int bitmap { get; set; }
        public string certificate_name { get; set; }
        public int certificate_type { get; set; }
        public int cityid { get; set; }
        public int _class { get; set; }//这个列对应json串中的列class 
        public string class_text { get; set; }
        public string code { get; set; }
        public string dist { get; set; }
        public string face { get; set; }
        public string flag { get; set; }
        public string flag_ext { get; set; }
        public string[] gcate { get; set; }
        public string geo { get; set; }
        public string gid { get; set; }//qq群号
        public string group_label { get; set; }
        public List<UinGroupTag> labels { get; set; }
        public string latitude { get; set; }//qq 群经纬度
        public string longitude { get; set; }
        public int level { get; set; }
        public int max_member_num { get; set; }//可容纳群员数目
        public int member_num { get; set; }//目前群员数目
        public string memo { get; set; }
        public string name { get; set; }
        public int option { get; set; }
        public string owner_uin { get; set; }//群主qq号
        public string richfingermemo { get; set; }
        public string url { get; set; }//群头像
        public string[] qaddr { get; set; }//群地址??

    }
    public class UinGroupTag 
    {
        public string label { get; set; }//白羊
        public string tagid { get; set; }//0840074857f86b600000212f
        public string time { get; set; }//js形式毫秒级数据 1475898208
    }
    public class UinGroupData 
    {
        public int ec { get; set; }
        public int endflag { get; set; }
        public int exact { get; set; }
        /// <summary>
        /// 符合查询结果的群数目
        /// </summary>
        public int gTotal { get; set; }
        public List<UinGrounpItem> group_list { get; set; }
        public string[] redwords { get; set; }
        public int usr_cityid { get; set; }
    }
    public class UinGroupDataRequestParam 
    {
        public string k;
        public int n;
        public int st;
        public int iso;
        public int src;
        public int v;
        public string bkn;
        public bool isRecommend { get; set; }//是否推荐群
        /// <summary>
        /// 城市编码
        /// </summary>
        public int city_id { get; set; }
        public int from;
        public bool newSearch;
        /// <summary>
        /// 检索qq群使用的关键字
        /// </summary>
        public string keyword;
        public int sort;
        public int wantnum;
        public int page;
        public string ldw;//数据来源 js形式 var  t= +(new Date)  等同于 (new Date()).valueOf() 当前毫秒的时间戳
        /// <summary>
        /// 设置js生成的参数ldw，bkn
        /// </summary>
        /// <param name="cookie"></param>
        public void CalculateUinJsParam(string cookie) 
        {
            Regex reg = new Regex("(^| )skey=([^;]*)(;|$)");// 
            Match m= reg.Match(cookie);
            GroupCollection gc = m.Groups;
            if (gc.Count < 3)
            {
                return;
            }
            string e = m.Groups[2].Value;
            //此处需要对于item可能为16进制的字符进行转义  十六进制转义序列将被它们表示的字符替换。
            long n = 5381;//n=3241243604;//这样的数据是否能取到
            foreach (char item in e)
            {//此处不会出现中文
                int code = (int)item;
                n += (n << 5) + code;
            }
            n = n & 2147483647;
            bkn = n.ToString();
            var ldwValue = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;//日期转毫秒 <>毫秒转日期
            long min = DateTime.MinValue.ToUniversalTime().Ticks;  //  0001-01-01 0:00:00  min=0
            ldw = ldwValue.ToString();
            //毫秒转日期
        }
        //从cookie中提取 skey 
        /*
      
        //计算bkn
           var e = t.get("skey");
          // t.get 获取数据的原理
           //var t= new RegExp("(^| )" + e + "=([^;]*)(;|$)");
           //var e=decodeURIComponent(t[2]) ;
                  , n = 5381;
                for (var r = 0, i = e.length; r < i; ++r)
                    n += (n << 5) + e.charAt(r).charCodeAt();
             BKn= n & 2147483647
         */
    }
}
