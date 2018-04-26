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
        public string GetInsertSql()
        {
            return @"INSERT INTO  [dbo].[ReportEnumRec]    ([Id],[ReportEnum],[CreateTime],[Reporter],[BeenReporterId],[IsDelete])
VALUES  ({Id},{ReportEnum},{CreateTime},{Reporter},{BeenReporterId},{IsDelete})";
        }
    }
    public class ReportNote : GuidTimeFieldwithDelete
    {
        public string UINote { get; set; }
        public ReportNote() 
        {
            base.Init();
        }
        public string GetInsertSql() 
        {
            return @"INSERT INTO [dbo].[ReportNote] ([Id],[CreateTime],[UINote],[IsDelete]) VALUES ({Id},{CreateTime},{UINote},{IsDelete})";
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
        public string GetInsertSql()
        {
            return @"INSERT INTO  [dbo].[ReporterAndNote] ([Id],[ReportId],[ReportNoteId],[IsDelete],[CreateTime])
     VALUES ({Id},{ReportId},{ReportNoteId},{IsDelete},{CreateTime}) ";
        }
    }
    public class ReportEnumDetail
    {
        public ReportNote Note { get; set; }
        public List<ReporterAndNote> ReportContainerNote { get; set; }
        public List<ReportEnumRec> Report { get; set; }
    }
}
