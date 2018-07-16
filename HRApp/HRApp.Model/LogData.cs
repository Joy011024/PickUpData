using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using Domain.GlobalModel;
namespace HRApp.Model
{
    [TableField(TableName = "EventLog")]
    public class LogData:GuidTimeField
    {
        public short Category { get; set; }
        public string Note { get; set; }
        public string Title { get; set; }
        public bool IsError { get; set; }
        public int DayInt { get; set; }
        /// <summary>
        /// 表数据入库记录内容
        /// </summary>
        /// <returns></returns>
        public static string InsertDbNoteFormat() 
        {
            return "insert into db the  table 【{0}】";
        }
    }
    [DescriptionSort("操作历史/非日志详情")]
    public class OperateEvent : GuidTimeFieldwithDelete
    {
        public short EventId { get; set; }
        public string RelyTableRowValue { get; set; }
        [DescriptionSort("外键关联表RowValueType中主键id")]
        public int RowValueType { get; set; }
    }
    [DescriptionSort("输出参数")]
    public class OutputParam
    {
        public int total { get; set; }
    }
    public class LogQueryParam:OutputParam
    {
        public int day { get; set; }
        public int beginRow { get; set; }
        public int endRow { get; set; }
    }
}
