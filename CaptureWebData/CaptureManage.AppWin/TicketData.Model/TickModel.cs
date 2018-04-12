using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketData.Model
{
    public class StationName
    {
        public string SpellCode { get; set; }//拼音代码
        public string Name { get; set; }//名称
        public string Code { get; set; }//英文编码
        public string FullSpell { get; set; }//全拼
        public string SampleSpell { get; set; }// 简写
        public int Index { get; set; }//站点索引序号
        public string Key { get; set; }//检索使用的关键字
    }
    public class QueryTicket
    {
        public string GoTime { get; set; }//出发日期
        public string GoStation { get; set; }//出发车站
        public string ToStation { get; set; }//到达车站

    }
    public class leftTicketDTO
    {
        /// <summary>
        /// 出发日期
        /// </summary>
        public string train_date { get; set; }
        /// <summary>
        /// 出发站
        /// </summary>
        public string from_station { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        public string to_station { get; set; }
        /// <summary>
        /// 车票类型
        /// </summary>
        public string purpose_codes { get; set; }

    }
    public class leftTicketDTOResponse
    {
        public TicketDataResponse data { get; set; }
        public int httpstatus { get; set; }
        public string[] messages { get; set; }
        public bool status { get; set; }
        public string[] validateMessages { get; set; }
        public string validateMessagesShowId { get; set; }
        public List<TicketSeatDataDto> ticketData = new List<TicketSeatDataDto>();

    }
    public class TicketDataResponse
    {
        public string flag { get; set; }//实际上不知道flag会返回什么类型的数据，此处使用字符串
        public string[] result { get; set; }//此处存储的是车辆信息
        public Dictionary<string, string> map { get; set; }//车站名称检索码信息
    }
    /// <summary>
    /// 12306 车票座位详细信息
    /// </summary>
    public class TicketSeatDataDto
    {
        public bool? CanBuyTicket { get; set; }
        //public string secretStr { get; set; }
        //public string buttonTextInfo { get; set; }//备注
        public string train_no { get; set; }
        public string station_train_code { get; set; }
        public string start_station_telecode { get; set; }
        public string end_station_telecode { get; set; }
        /// <summary>
        /// 出发站代码
        /// </summary>
        public string from_station_telecode { get; set; }
        public string to_station_telecode { get; set; }
        public string start_time { get; set; }
        public string arrive_time { get; set; }
        public string lishi { get; set; }
        public string canWebBuy { get; set; }
        public string yp_info { get; set; }
        public string start_train_date { get; set; }
        public string train_seat_feature { get; set; }
        public string location_code { get; set; }
        public string from_station_no { get; set; }
        public string to_station_no { get; set; }
        public string is_support_card { get; set; }
        public string controlled_train_flag { get; set; }
        public string gg_num { get; set; }
        public string gr_num { get; set; }
        public string qt_num { get; set; }
        public string rw_num { get; set; }
        public string rz_num { get; set; }
        public string tz_num { get; set; }
        public string wz_num { get; set; }
        public string yb_num { get; set; }
        public string yw_num { get; set; }
        public string yz_num { get; set; }
        public string ze_num { get; set; }
        public string zy_num { get; set; }
        public string swz_num { get; set; }
        public string srrb_num { get; set; }
        public string yp_ex { get; set; }
        public string seat_types { get; set; }
        public string from_station_name { get; set; }
        /// <summary>
        /// 到达站代码
        /// </summary>
        public string to_station_name { get; set; }
    }
}
