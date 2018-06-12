use CommonAppSetting
go
create  table Grid
(
	Id uniqueidentifier primary key,
	GridName varchar(16) not null,
	GridDesc nvarchar(1024),
	IsDelete bit not null
);
create table GridCell
(
	Id uniqueidentifier primary key,
	Cell varchar(16) not null,
	[Text] nvarchar(32) not null,
	Remark nvarchar(1024) not null,
	Display bit not  null,
	Sort smallint not null,
	GridId uniqueidentifier not null
);
alter table GridCell add constraint FK_GridId foreign key (GridId) references Grid(Id);
if(OBJECT_ID('[OperateEvent]','u') is not null)
	drop table OperateEvent
if(object_id('RowValueType','u') is not null)
	drop table RowValueType
go
create table RowValueType
(
	Id int identity(1,1) primary key,
	ColumnName varchar(32) not null,
	ColumnType varchar(32) not null,
	TableName varchar(32) not null,
	Remark nvarchar(1024) ,
	CreateTime datetime not null,
	CreateTimeDayInt int not null
);
create TABLE [dbo].[OperateEvent]
(--操作历史 ：每一次操作都把之前这对应表中的这一行数据进行清除【设置IsDelete=true】
	[Id] [uniqueidentifier] NOT NULL primary key,
	[EventId] [smallint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[RelyTableRowValue] [varchar](36) NOT NULL,
	RowValueType int not null,
	[IsDelete] [bit] NOT NULL,
) ;
go
if(object_id('SP_MakeOperateEventByColumn','P') is not null)
	drop procedure SP_MakeOperateEventByColumn
go
create  procedure SP_MakeOperateEventByColumn
(
	@rowId varchar(36) ,
	@OpId smallint ,
	@tableName nvarchar(32) ,
	@ColumnName nvarchar(32)
)
as
begin
INSERT INTO  [dbo].[OperateEvent]
           ([Id]
           ,[EventId]
           ,[CreateTime]
           ,[RelyTableRowValue]
           ,[RowValueType]
           ,[IsDelete])
     VALUES
           (NEWID()
           ,@OpId
           ,GETDATE()
           ,@rowId
           , (select top 1 id from dbo.[RowValueType] where ColumnName=@ColumnName and TableName=@TableName)
           ,0)
end
go
if(object_id('SP_MakeOperateEventByColumnId','P') is not null)
	drop procedure SP_MakeOperateEventByColumnId
go
create  procedure SP_MakeOperateEventByColumnId
(
	@rowId varchar(36) ,
	@OpId smallint ,
	@ColumnId nvarchar(32)
)
as
begin
INSERT INTO  [dbo].[OperateEvent]
           ([Id]
           ,[EventId]
           ,[CreateTime]
           ,[RelyTableRowValue]
           ,[RowValueType]
           ,[IsDelete])
     VALUES
           (NEWID()
           ,@OpId
           ,GETDATE()
           ,@rowId
           , (select top 1 id from dbo.[RowValueType] where ID=@ColumnId)
           ,0)
end
go
create procedure SP_InitTableColumnPrimaryType
as
declare @day int
declare @time datetime
set @time=GETDATE()
set @day=CONVERT(int, CONVERT(varchar(10),@time,112))
insert into dbo.RowValueType
([ColumnName] ,[ColumnType] ,[TableName]  ,[Remark],CreateTime,CreateTimeDayInt)
select  c.name,
	case c.system_type_id 
		when 36 then 'uniqueidentifier' 
		when 56 then 'int'
			else 'Unkonw'
		end, t.name,null,@time,@day
 from sys.columns c left join sys.objects o
on c.object_id=o.object_id  
left join sys.tables t on o.object_id=t.object_id 
where o.type='U' and c.name='id' and t.name not in('RowValueType');