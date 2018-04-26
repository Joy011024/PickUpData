using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using HRApp.IApplicationService;
namespace HRApp.ApplicationService
{
    public class ReportEnumDataService:IReportEnumDataService
    {
         
        public string SqlConnString
        {
            get;
            set;
        }
        public IReportEnumDataRepository reportRepository { get; set; }
        public ReportEnumDataService(IReportEnumDataRepository dal)
        {
            reportRepository = dal;
        }
        public Common.Data.JsonData Add(ReportParam model)
        {
            //添加举报项
            DateTime time = DateTime.Now;
            ReportEnumDetail detail = new ReportEnumDetail()
            {
                Report = new List<ReportEnumRec>()
            };
            if (string.IsNullOrEmpty(model.Note))
            {
                detail.Note = new ReportNote() { UINote = model.Note, CreateTime = time };
                detail.ReportContainerNote = new List<ReporterAndNote>();
            }
            foreach (Guid item in model.Ids)
            {
                ReportEnumRec rec = new ReportEnumRec() { ReportEnum = model.ReportType,BeenReporterId=item,CreateTime=time };
                detail.Report.Add(rec);
                ReporterAndNote rn = new ReporterAndNote() { CreateTime = time, ReportNoteId = detail.Note.Id };
                detail.ReportContainerNote.Add(rn);
            }
            //举报的对象入库
            //关联举报的对象
           bool success= reportRepository.Add(detail);
           return new  Common.Data.JsonData();
        }

        public List<ReportParam> QueryWhere(ReportParam model)
        {
            throw new NotImplementedException();
        }
    }
}
