using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
namespace HRApp.Infrastructure
{
    public class ReportEnumDataRepository:IReportEnumDataRepository
    {
        public bool SaveNote(ReportNote note)
        {
            string sql = note.GetInsertSql();
            return CommonRepository.ExtInsert<ReportNote>(sql, SqlConnString, note);
        }

        public bool SaveReported(List<ReportEnumRec> recs)
        {
            if (recs == null || recs.Count == 0)
            {
                return false;
            }
            string sql = recs[0].GetInsertSql();
            return CommonRepository.ExtBatchInsert<ReportEnumRec>(sql, SqlConnString, recs)==recs.Count;
        }

        public bool SaveReportedAndNote(List<ReporterAndNote> notes)
        {
            if (notes == null || notes.Count == 0) 
            {
                return false;
            }
            string sql = notes[0].GetInsertSql();
            return CommonRepository.ExtBatchInsert<ReporterAndNote>(sql, SqlConnString, notes)==notes.Count;
        }

        public string SqlConnString
        {
            get;
            set;
        }
    }
}
