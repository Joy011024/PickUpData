using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
namespace IHRApp.Infrastructure
{
    public interface IReportEnumDataRepository//:IBaseListRepository<ReportEnumDetail>
    {
        bool SaveNote(ReportNote note);
        bool SaveReported(List<ReportEnumRec> recs);
        bool SaveReportedAndNote(List<ReporterAndNote> notes);
        string SqlConnString { set; get; }
    }
}
