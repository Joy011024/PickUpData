USE [TecentDataUinDA]
GO 
ALTER procedure [dbo].[SP_QueryAccount] 
(
	@beginDayInt int,
	@endDayInt int,
	@beginRow int,
	@endRow int,
	@total int output
)
as
select @total=count(uin) from [dbo].[TecentQQData] acc where DayInt>=@beginDayInt and dayInt<=@endDayInt and [ImgType]>-1   
and not exists (select distinct(beenReporterID) from  hrapp.dbo.ReportEnumRec r where r.beenReporterID =acc.ID and reporter='-1' )
select [ID] ,[age],[city],[country],[distance],[face],[gender],[nick],[province],[stat],[uin],[HeadImageUrl] as Url,[CreateTime],[IsGatherImage],[GatherImageTime],[LastErrorGatherImage],[GatherImageErrorNum],[ImageRelativePath],[DayInt],[ImgType] 
from
(
SELECT   [ID] ,
row_number() over(order by DayInt desc) as tag
      ,[age],[city],[country],[distance],[face],[gender],[nick],[province],[stat],[uin],[HeadImageUrl],[CreateTime],[IsGatherImage],[GatherImageTime],[LastErrorGatherImage],[GatherImageErrorNum],[ImageRelativePath],[DayInt],[ImgType]
  FROM  [dbo].[TecentQQData] acc where DayInt>=@beginDayInt and dayInt<=@endDayInt and [ImgType]>-1
  and   not exists (select distinct(beenReporterID) from  hrapp.dbo.ReportEnumRec r where r.beenReporterID =acc.ID and reporter='-1' )
)t where tag>=@beginRow and tag<=@endRow
