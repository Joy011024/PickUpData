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
        public IReportEnumDataRepository reportRepository;
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
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            try
            {
                if (string.IsNullOrEmpty(model.Note))
                {
                    detail.Note = new ReportNote() { UINote = model.Note, CreateTime = time };
                    reportRepository.SaveNote(detail.Note);
                    detail.ReportContainerNote = new List<ReporterAndNote>();
                }
                foreach (Guid item in model.Ids)
                {
                    ReportEnumRec rec = new ReportEnumRec() { ReportEnum = model.ReportType, BeenReporterId = item, CreateTime = time };
                    detail.Report.Add(rec);
                    ReporterAndNote rn = new ReporterAndNote() { CreateTime = time, ReportNoteId = detail.Note.Id };
                    detail.ReportContainerNote.Add(rn);
                }
                //举报的对象入库
                reportRepository.SaveReported(detail.Report);
                reportRepository.SaveReportedAndNote(detail.ReportContainerNote);
                //关联举报的对象
                json.Data = detail;
                json.Success = true;
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }

        public List<ReportParam> QueryWhere(ReportParam model)
        {
            throw new NotImplementedException();
        }
    }
}
