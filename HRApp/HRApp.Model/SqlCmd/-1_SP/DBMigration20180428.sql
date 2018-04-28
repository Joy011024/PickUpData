USE [HrApp]
GO  
  
 insert into ReportEnumRec ( [Id]
      ,[ReportEnum]
      ,[CreateTime]
      ,[Reporter]
      ,[BeenReporterId]
      ,[IsDelete])
 select * from HrAppBU.dbo.ReportEnumRec;
 insert into ReportNote ([Id]
      ,[CreateTime]
      ,[UINote]
      ,[IsDelete])
 select * from HrAppBU.dbo.ReportNote;
 insert into    ReporterAndNote( [Id]
      ,[ReportId]
      ,[ReportNoteId]
      ,[IsDelete]
      ,[CreateTime])
 select * from HrAppBU.dbo.ReporterAndNote