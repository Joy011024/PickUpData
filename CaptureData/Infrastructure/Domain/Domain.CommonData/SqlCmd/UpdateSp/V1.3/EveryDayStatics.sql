use tecentdatada
--1 ->add  column for set the statics in  everyday 
alter table tecentqqdata add DayInt int 
alter table tecentqqdata add constraint DefDayInt default 0 for DayInt with values
update tecentqqdata set DayInt=convert(int,convert(varchar(8),createtime,112)) where DayInt=0
go
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_EveryDayCaptureStatics]') AND type in (N'P', N'PC'))
BEGIN
 create procedure SP_EveryDayCaptureStatics
 as
 select DayInt,count(uin) from tecentqqdata group by DayInt
 order by DayInt
end