using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace HRApp.Model
{
    public class ReportEnumRec : GuidTimeFieldwithDelete
    {
        public int ReportEnum { get; set; }
        public string Reporter { get; set; }
        public Guid BeenReporterId { get; set; }
        public ReportEnumRec() 
        {
            Reporter = "-1";//默认操作者是系统
            base.Init();
        }
    }
    public class ReportNote : GuidTimeFieldwithDelete
    {
        public string UINote { get; set; }
        public ReportNote() 
        {
            base.Init();
        }
    }
    public class ReporterAndNote : GuidTimeFieldwithDelete
    {
        public Guid ReportId { get; set; }
        public Guid ReportNoteId { get; set; }
        public ReporterAndNote() 
        {
            base.Init();
        }
    }
    public class ReportEnumDetail
    {
        public ReportNote Note { get; set; }
        public List<ReporterAndNote> ReportContainerNote { get; set; }
        public List<ReportEnumRec> Report { get; set; }
    }
}
