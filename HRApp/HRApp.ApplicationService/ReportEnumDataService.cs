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
                Report = new List<ReportEnumRec>(),
                ReportContainerNote = new List<ReporterAndNote>()
            };
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            try
            {
                bool writeReportNote = false;
                if (!string.IsNullOrEmpty(model.Note))
                {
                    detail.Note = new ReportNote() { UINote = model.Note, CreateTime = time };
                    reportRepository.SaveNote(detail.Note);
                    writeReportNote = true;
                }
                foreach (Guid item in model.Ids)
                {
                    ReportEnumRec rec = new ReportEnumRec() { ReportEnum = model.ReportType, BeenReporterId = item, CreateTime = time };
                    detail.Report.Add(rec);
                    if (writeReportNote)
                    {//填写了举报信息描述 
                        ReporterAndNote rn = new ReporterAndNote() { CreateTime = time, ReportNoteId = detail.Note.Id, ReportId = rec.Id };
                        detail.ReportContainerNote.Add(rn);
                    }
                }
                //举报的对象入库
                reportRepository.SaveReported(detail.Report);
                if (writeReportNote)
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


        public ReportParam Get(object id)
        {
            throw new NotImplementedException();
        }


        public bool Update(ReportParam entity)
        {
            throw new NotImplementedException();
        }
    }
}
