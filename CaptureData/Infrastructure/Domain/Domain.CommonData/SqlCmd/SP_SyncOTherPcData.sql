USE [TecentDataDA]
GO
/****** Object:  StoredProcedure [dbo].[SP_SyncOTherPcData]    Script Date: 08/05/2017 22:24:21 ******/
SET ANSI_NULLS ON
GO
if(object_id('SP_SyncOTherPcData','p') is not null)
	drop procedure SP_SyncOTherPcData
go
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_SyncOTherPcData]
as
declare @link varchar(30)
select @link=Name from dbo.LinkServer where DBTypeID=1
declare @sql varchar(2000)
set @sql='
insert into tecentdatada.dbo.tecentqqdata
(ID,PickUpWhereId,age,city,country,distance,face,gender,nick,province,stat,uin,HeadImageUrl,CreateTime)
select 
ID,PickUpWhereId,age,city,country,distance,face,gender,nick,province,stat,uin,HeadImageUrl,CreateTime
 from {Link}.tecentdatada.dbo.tecentqqdata t
 where t.id not in(select id from tecentdatada.dbo.tecentqqdata )'
 set @sql=replace(@sql,'{Link}',@link)
exec (@sql)