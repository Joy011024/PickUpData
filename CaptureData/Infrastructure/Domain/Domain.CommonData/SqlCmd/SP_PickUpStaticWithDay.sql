if(object_id('SP_PickUpStaticWithDay','p') is not null)
	drop procedure SP_PickUpStaticWithDay
go
create procedure SP_PickUpStaticWithDay
(
	@day datetime
)
as
declare @di int
set @di=convert(int,@day,112)

declare @count int
declare @primaryCount int
select @count=count(uin), @primaryCount=count(distinct(uin)) from TecentQQData where convert(int,Createtime,112)=@di
declare @DBTotal int
declare @DBPrimaryTotal int
select @DBTotal=count(uin),@DBPrimaryTotal=count(distinct(uin))  from TecentQQData

select @DBTotal as DBTotal,@DBPrimaryTotal as DBPrimaryTotal, @count as Total,@primaryCount as IdTotal,
convert(varchar(10),@day,120) StaticDay