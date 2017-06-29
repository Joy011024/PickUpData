using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpClientHelper;
namespace CaptureWebData
{
    public class QQSortbyupdatetime
    {
        /// <summary>
        /// 含有好友9项数据的字符串数组
        /// </summary>
        public string[] Member { get; set; }
    }
    public class QQFriendResponse
    {
        public string[] domaingroups { get; set; }
        public string[] ggroup { get; set; }
        public string[] groups { get; set; }
        public string[] qqgroups { get; set; }
        public string[] sortbyupdatetime { get; set; }
        public string[] timeuse { get; set; }
        public string[] tool { get; set; }
    }
    public class PrepareParam
    {
        public string GetFriendUrl = "https://mail.qq.com/cgi-bin/laddr_lastlist";
        public static Dictionary<string, string> PrepareHeader() 
        {
            string head = @":authority:mail.qq.com
:method:GET
:path:/cgi-bin/laddr_lastlist?sid=eFdapxlCqMOMcnsi&encode_type=js&t=addr_datanew&s=AutoComplete&category=hot&resp_charset=UTF8&ef=js&r=0.6020991376261546
:scheme:https
accept:*/*
accept-encoding:gzip, deflate, sdch, br
accept-language:zh-CN,zh;q=0.8
cookie:pgv_pvi=2166054912; RK=5Q8eAGcqdH; tvfe_boss_uuid=4326f9f3a442deb4; o_cookie=158055983; pac_uid=1_158055983; pgv_pvid=3887722460; pgv_si=s6359198720; ptisp=; ptui_loginuin=158055983; ptcz=50381d247d471856795f71bee19f08d2d437f1a1e9ecc4a4d47d90b0744a7ddf; pt2gguin=o0158055983; uin=o0158055983; skey=@1nPjzuiwV; p_uin=o0158055983; p_skey=kTWhcsqmG7KkzjFgyTDc1yZlwAHfQJCzQQpCbcJu0LE_; pt4_token=6XS7kVOMTUszVxbyGpfdjG44YzKdwdnQAFq*LMPfU-c_; wimrefreshrun=0&; qm_antisky=158055983&80dbb4d659204186ce41890411ffba0886f869abe61d68f4c9ef88d9f6eec103; qm_flag=0; qqmail_alias=158055983@qq.com; sid=158055983&6c2ac2e2e9421186966c28e02248ffb3,qa1RXaGNzcW1HN0trempGZ3lURGMxeVpsd0FIZlFKQ3pRUXBDYmNKdTBMRV8.; qm_username=158055983; qm_domain=https://mail.qq.com; qm_ptsk=158055983&@1nPjzuiwV; foxacc=158055983&0; ssl_edition=sail.qq.com; edition=mail.qq.com; qm_loginfrom=158055983&wpt; username=158055983&158055983; CCSHOW=000001; new_mail_num=158055983&142; webp=1
referer:https://mail.qq.com/zh_CN/htmledition/ajax_proxy.html?mail.qq.com&v=140521
user-agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
            Dictionary<string, string> requestHead = new RequestHeaderHelper().PickUpRequestHeader(head);
            Dictionary<string, string> heads = HttpClientHelper.RequestHeaderHelper.InitHeader(head);
            return heads;
        }
        public static string RequestUrl() 
        {
            string param = "?sid={sid}&encode_type=js&t=addr_datanew&s=AutoComplete&s=AutoComplete&resp_charset=UTF8&resp_charset=UTF8&r={r}";
            param= param.Replace("{r}", GenerateJSRandom());
           
            return param;
        }
        /// <summary>
        /// 
        /// 产生查询qq好友所需要的16位随机数r
        /// </summary>
        /// <returns></returns>
        public static string GenerateJSRandom()
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            double d = r.NextDouble();
            string flo = "0.";
            string ruin = (d + "").Replace(flo, "");//虽然产生的浮点数长度为17位但是经过ToString转换之后长度变为了15位
            int limitLen = 16;
            if (ruin.Length > limitLen)
            {
                ruin = ruin.Substring(0, limitLen);
            }
            else if (ruin.Length < limitLen)
            {
                int len = limitLen - ruin.Length;
                string ext = GenerateLimitLengthInt(len);
                ruin = ruin + ext;
            }
            return flo + ruin;
        }
        /// <summary>
        /// 产生指定长度的int字符串
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        static string GenerateLimitLengthInt(int len)
        {
            int result = 10;
            int start = 1;
            while (len > 1)
            {
                result = result * 10;
                len--;
            }
            start = result / 10;
            Random ran = new Random(Guid.NewGuid().GetHashCode());
            return ran.Next(start, result).ToString();
        }
    }
}
