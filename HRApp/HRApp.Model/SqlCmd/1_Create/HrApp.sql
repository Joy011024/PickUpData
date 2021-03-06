use  HrApp
go
Create table  SpecialSpellName
(--chinese convert english
	Id int identity(1,1) primary key,
	Name nvarchar(2) not null,
	Code varchar(16) not null,
	IsDeleted int default(0),
	IsErrorSpell bit not null
);
create table MaybeSpellName
(--汉字可能读音
	Id uniqueidentifier primary key,
	Name nvarchar(3) not null,
	Code nvarchar(16) not null,
	CreateTime DateTime not null,
	CreateDayInt int not null,
	IsSpecialChinese bit not null 
);
go 
Create table App
(
	Id int identity(1,1) primary key ,
	AppName nvarchar(32) not null,
	AppCode varchar(32) not null,
	IsDelete int default(0)
);
create table AppVer
(
	Id int identity(1,1),
	AppVersion varchar(32)not null,
	AppId int not null ,
	IsNowVersion bit not null,--is now version
	CreateTime DateTime default(getdate()) 
);
alter table AppVer add constraint FK_AppId foreign key (AppId) references App(Id)
Create table KeyWord
(
	Id uniqueidentifier primary key,
	Word nvarchar(64) not null,
	Useage nvarchar(128) not null,
	IsDelete int default(0),
	AppId int not null
);
alter table KeyWord add constraint FK_AppKeyWordId foreign key (AppId) references App(Id);
go
 CREATE TABLE [dbo].[CategoryItems](
	[ID] [int] NOT NULL primary key identity(1,1),--主键自增长
	[Name] [nvarchar](32) not NULL,
	[ParentID] [int] not NULL,
	[ParentCode] [nvarchar](64) not null,
	[Code] [nvarchar](64) not NULL unique,--UI层次没有设定的话通过文本转拼音进行设置
	[IndexSpell] varchar(256) not null,--query of key
	[Sort] [int] NULL,
	[IsDelete] [bit] not NULL,
	ItemValue varchar(1024) not null,--可能存储URL
	ItemUsingSize int default(1),--改项被引用的次数【进行删除时的判断校验】
	[CreateTime] [datetime] not  NULL,
	[NodeLevel] [int] default(0),
	ItemDesc nvarchar(1024)
);
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
create TABLE [dbo].[OptionEvent]
(--操作历史 ：每一次操作都把之前这对应表中的这一行数据进行清除【设置IsDelete=true】
	[Id] [uniqueidentifier] NOT NULL primary key,
	[EventId] [smallint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[RelyTableRowValue] [varchar](36) NOT NULL,
	RowValueType int not null,
	[IsDelete] [bit] NOT NULL,
) ;
Create table RelyTable
(--外键关联的表，此表数据只能管理员进行操作
	Id int primary key identity(1,1),
	TableName varchar(32) not null,
	ColumnName varchar(32) not null,
	CreateTime Datetime not null ,
    Note nvarchar(1024),
	IsPrimaryKey bit not null,
    IsDelete bit not null
)
go
create table ContactData
(--联系人信息列表
	Id uniqueidentifier primary key,
	ContactTypeId int not null,
	Value varchar(16) not null,
	ContactName nvarchar(16) not null,
	Belonger nvarchar(16) not null,
	CreateTime DateTime not null,
	[Desc] nvarchar(128)
);
alter table ContactData add constraint FK_ContactId foreign key (ContactTypeId) 
references CategoryItems(Id)
go
create table Account
(
	Id uniqueidentifier primary key,
	Name nvarchar(16) not null,
	HeadImgUrl varchar(1024),
	HasHeadImage bit not null,
	UserName varchar(16) not null,
	IsThirdAccount bit not null,
	Statue smallint not null,
	CreateTime datetime not null,
	CreateDayInt int not null
);
create table ClickFlag
(
	Id uniqueidentifier primary key,
	Createtime datetime not null,
	RelyTableId int not null,
	TableId uniqueidentifier not null,
	ClickId int not null,
	OptionIP varchar(16)
)
 alter table ClickFlag add constraint fk_RelyTableId foreign key (RelyTableId) 
 references RelyTable(Id);
 create table UserSetting
(
	Id uniqueidentifier primary key,
	Name nvarchar(32) not null,
	Code varchar(32) not null,
	Value varchar(16) not null,
	IsDelete bit not null,
	CreateTime datetime not null,
	Belonger varchar(16)
);
create table ReportEnumRec
(
	Id uniqueidentifier primary key,
	ReportEnum int not null,
	CreateTime datetime not null,
	Reporter varchar(16) not null,--sys flag ,set the value=-1
	BeenReporterId uniqueidentifier not null,
	IsDelete bit not null
);
create table ReportNote
(
	Id uniqueidentifier primary key,
	CreateTime datetime not null,
	UINote nvarchar(256) not null,
	IsDelete bit not null
);
create table ReporterAndNote
(
	Id uniqueidentifier primary key,
	ReportId uniqueidentifier not null,
	ReportNoteId uniqueidentifier not null,
	IsDelete bit not null,
	CreateTime datetime not null
);
alter table ReporterAndNote add constraint fk_ReportId foreign key (ReportId) 
 references ReportEnumRec(Id);
 alter table ReporterAndNote add constraint fk_ReportNoteId foreign key (ReportNoteId) 
 references ReportNote(Id);
create table KeySpellGuid
(
	Id uniqueidentifier primary key,
	SpellWord varchar(256) not null,
	TableId int not null,
	TableRowId uniqueidentifier not null,
	CreateTime datetime not null
)
alter table KeySpellGuid add constraint fk_KeySpellGuid foreign key (TableId) 
 references RelyTable(Id);
create table KeySpellInt
(
	Id int primary key,
	SpellWord varchar(256) not null,
	TableId int not null,
	TableRowId uniqueidentifier not null,
	CreateTime datetime not null
)
alter table KeySpellInt add constraint fk_KeySpellInt foreign key (TableId) 
 references RelyTable(Id);
create table Organze
(
	Id int identity(1,1) primary key,
	Name nvarchar(16) not null,
	Code varchar(32) not null,
	CreateTime datetime not null,
	Scale int not null,--人员规模
	ParentId int not null
);
create table OrganzeMember
(
	Id int identity(1,1) primary key,
	Name nvarchar(16) not null,
	SpellName varchar(16) not null,
	IsLeader bit not null,
	IsBoss bit not null,
	CreateTime datetime not null,
	ServeTime datetime not null,
	OrganzeId int not null,
	Statue smallint not null,
	LastUpdateTime datetime not null
)
 alter table OrganzeMember add constraint fk_OrganzeMemberId foreign key (OrganzeId) 
 references Organze(Id);
create table BaseOrderData
(--
	Id uniqueidentifier primary key,
	Name nvarchar(16) not null,
	CreateTime DateTime not null,
	Remark nvarchar(1028)
);
create table Saller
(
	Id int primary key identity(1,1),
	Name nvarchar(16) not null,
	JoinTime datetime not null,
	CancelTime datetime not null,
	[Level] smallint not null,
	Address nvarchar(128) not null,
	Contact varchar(16) not null
)
create table UserOrderData
(
	Id uniqueidentifier primary key,
	Name nvarchar(16) not null,
	CreateTime DateTime not null,
	Remark nvarchar(1028),
	BelongSallerId int not null,--店家
	DayStock int not null,--库存
	DayPrice float not null--售价
);
 alter table UserOrderData add constraint fk_BelongSallerId foreign key (BelongSallerId) 
 references Saller(Id);
 create table WorkFlow
 (--数据流程节点表
	Id int primary key,
	FlowNodeName nvarchar(32) not null,--流程（节点）名
	Sort smallint not null,--根据这个标志查找下一流程
	UseFlow bit not null,
	Remark nvarchar(1028),
	Code varchar(32) not null,
	CreateTime DateTime not null,
	ParentId int not null,--用于记录是所属流程
	GotoFlowId int not null,--查找流程下一步骤节点，这是流程进度图
	BackId int not null--拒绝时返项流程 
 );
create table ReserveEmailAccount
(--备用邮箱
	Id smallint identity(1,1) primary key,
	Account varchar(32) not null,
	AuthortyCode varchar(32) not null,
	Smtp smallint not null,
	SmtpHost varchar(32) not null,
	CreateTime datetime not null,
	IsDelete bit not null,
	Remark nvarchar(128),
	UsePriority smallint not null,
	IsPrimaryAccount bit not null,
	IsEnable bit not null
);
Create table EveryDayActive
(
	ID uniqueidentifier primary key,
	EmailAccountId smallint not null,
	CreateTime datetime not null,
	IsSuccess bit not null,
	DayActiveNumber int not null,
	DayInt int not null
)
alter table EveryDayActive add constraint fk_EmailAccountId foreign key (EmailAccountId) 
references ReserveEmailAccount(ID);
Create table AppEmail
(
	ID uniqueidentifier primary key,
	Body nvarchar(1024) not null,
	BodyType smallint not null,
	[Subject] nvarchar(128) not null,
	IsDelete bit not null,
	ParentId uniqueidentifier not null,
	SendBy varchar(64) not null,
	CreateTime datetime not null
);
create table AppEmailPlan
(--邮件定时计划
	Id uniqueidentifier primary key,
	PrimaryMsgId uniqueidentifier not null,
	SendTime datetime not null,
	CreateTime datetime not null,
	IsDelete bit not null,
	SendNumber smallint not null
);
alter table AppEmailPlan add constraint fk_PlanPrimaryMsgId foreign key (PrimaryMsgId) 
references AppEmail(ID);
create table AppEmailReceiverPlan
(--邮件接收人计划列表
	Id uniqueidentifier primary key,
	PrimaryMsgId uniqueidentifier not null,
	CreateTime datetime not null,
	IsMailer bit not null,
	IsDelete bit not null,
	SendTo varchar(64) not null
);
alter table AppEmailReceiverPlan add constraint fk_AppEmailReceiverPlanMsgId foreign key (PrimaryMsgId) 
references AppEmail(ID);
create table AppEmailReceiver
(--邮件实际发送情形
	Id uniqueidentifier primary key,
	PrimaryMsgId uniqueidentifier not null,
	IsMailer bit not null,
	SendTo varchar(64) not null,
	SendTime datetime ,
	CreateTime datetime not null,
	IsDelete bit not null,
	DayInt int not null --增加快速比较标志
);
 alter table AppEmailReceiver add constraint fk_EmailSendHistoryMsgID foreign key (PrimaryMsgId) 
 references AppEmail(ID);
 create table EmailSendHistory
 (
	Id uniqueidentifier primary key,
 	SendResult bit not null,
	SendNumber smallint not null,
	SendTime datetime not null,
	PrimaryMsgId uniqueidentifier not null,
 )
 alter table EmailSendHistory add constraint fk_PrimaryMsgId foreign key (PrimaryMsgId) 
 references AppEmail(ID);
create table EventLog
(
	Id uniqueidentifier primary key,
	Category smallint not null,
	Note nvarchar(2048) not null,
	Title nvarchar(128) not null,
	IsError bit not null,
	CreateTime datetime not null,
	DayInt int not null
);
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
if(OBJECT_ID('SP_InitTableColumnPrimaryType','P') is not null)
	drop procedure SP_InitTableColumnPrimaryType
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
go
Create table DataSource
(
	Id int identity(1,1) primary key,
	FromName nvarchar(16) not null,
	Code varchar(16) not null,--便于检索 
	Remark nvarchar(1028),
	IsEnable bit not null
);
go
create table AddressData
(
	Id int identity(1,1) primary key,
	Name nvarchar(32) not null,
	ParentId int not null,
	Code varchar(16) not null,
	ParentCode varchar(16) not null,
	AddressLevel smallint not null,
	Createtime datetime not null,
	DataSourceId int not null
)
alter table AddressData add  constraint FK_DataSourceId foreign key(DataSourceId)
references DataSource(id);
go
create table  ActiveData
(
	Id uniqueidentifier primary key,
	Name nvarchar(32) not null,
	CreateTime datetime not null,
	ActiveBeginTime datetime not null,
	ActiveEndTime datetime not null,
	ActiveAddress nvarchar(64) not null,
	AdressDetail nvarchar(256) not null,
	ActiveOrganzer nvarchar(32) not null,
	ActiveStatue smallint not null,--活动审核状态
	ActiveExecuteStatue smallint not null,--活动进行状态
	ActiveDetail nvarchar(1028) --活动详细信息
);
go
if(object_id('SP_QueryDayLog','P') is not null)
	drop procedure SP_QueryDayLog
go
create procedure SP_QueryDayLog
(
	@day int,
	@beginRow int,
	@endRow int,
	@total int output
) 
as
select   @total=count(Id)  from eventlog where dayint=@day
select [Id],[Category],[Note],[Title],[IsError],[CreateTime],[DayInt]
from
(
	 select  [Id],[Category],[Note],[Title],[IsError],[CreateTime],[DayInt]
	 ,row_number() over(order by dayint)  rn
	 from eventlog where dayint=@day
) t 
where t.rn>=@beginRow and t.rn<=@endRow
 