if(object_id('SP_StaticCountToday','p') is not null )
	drop procedure SP_StaticCountToday
go
create procedure SP_StaticCountToday
as
declare @today datetime 
declare @day int
set @today=getdate()
set @day=convert(int,@today,112)
--ͳ�Ƶ��������˶���������
select count(uin) from TecentQQData where @day=convert(int,createtime,112)