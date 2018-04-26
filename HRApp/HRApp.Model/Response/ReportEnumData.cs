using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.Model.Response
{
    public class ReportEnumData
    {
        [Description("ReportEnumNote.Id")]
        public Guid Id { get; set; }
        [Description("举报项")]
        public string ReportEnumText { get; set; }
        public int ReportEnum { get; set; }
        [Description("被举报者存储的id=ReportEnumRec.Id")]
        public Guid ReportObjectId { get; set; }
        [Description("举报内容的id=ReportNote.Id")]
        public Guid ReportNoteId { get; set; }
        [Description("UI输入的举报内容=UINote")]
        public string ReportInputNote { get; set; }
    }
}
