if(object_id('SP_StaticCountToday','p') is not null )
	drop procedure SP_StaticCountToday
go
create procedure SP_StaticCountToday
as
declare @today datetime 
declare @day int
set @today=getdate()
set @day=convert(int,@today,112)
declare @DBTotal int
declare @DBPrimaryTotal int
select @DBTotal=count(uin),@DBPrimaryTotal=count(distinct(uin))  from TecentQQData
--统计当天增加了多少条数据
select @DBTotal as DBTotal,@DBPrimaryTotal as DBPrimaryTotal,count(uin) from TecentQQData where @day=convert(int,createtime,112)