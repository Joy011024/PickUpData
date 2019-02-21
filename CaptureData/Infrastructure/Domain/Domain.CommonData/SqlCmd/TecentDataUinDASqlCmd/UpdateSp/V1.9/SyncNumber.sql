create table SyncNumber
(
	DayInt int primary key,
	Number int not null,
	SyncTime Datetime not null,
	SyncIdNumber int not null
)
go
if(object_id('sp_EverydaySyncNumber','p') is not null)
	drop procedure sp_EverydaySyncNumber
go
create procedure sp_EverydaySyncNumber
as
declare @day int 
declare @id int
declare @total int
set @day=  convert(int, convert(varchar, getdate(),112))
select @total =count(id),@id=count(distinct(uin)) from TecentQQData
if(exists(select dayint from SyncNumber where dayint=@day))
	 begin
		update SyncNumber set Number=@total ,SyncIdNumber=@id,SyncTime =getdate() where DayInt=@day
	 end
else 
	 begin 
		insert into SyncNumber ([DayInt]  ,[Number] ,[SyncTime]  ,[SyncIdNumber])
		values (@day,@total,getdate(),@id)
	 end