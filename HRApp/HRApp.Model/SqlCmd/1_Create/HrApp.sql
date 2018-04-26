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
(--���ֿ��ܶ���
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
	[ID] [int] NOT NULL primary key identity(1,1),--����������
	[Name] [nvarchar](32) not NULL,
	[ParentID] [int] not NULL,
	[ParentCode] [nvarchar](64) not null,
	[Code] [nvarchar](64) not NULL unique,--UI���û���趨�Ļ�ͨ���ı�תƴ����������
	[Sort] [int] NULL,
	[IsDelete] [bit] not NULL,
	ItemValue varchar(1024) not null,--���ܴ洢URL
	ItemUsingSize int default(1),--������õĴ���������ɾ��ʱ���ж�У�顿
	[CreateTime] [datetime] not  NULL,
	[NodeLevel] [int] default(0),
	ItemDesc nvarchar(1024)
);
go
Create table Menu
(
	Id int primary key identity(1,1),
	Name nvarchar(32) not null,
	Code varchar(64) not null,
	Url varchar(256) not null,
	Remark nvarchar(1024),
	CreateTime datetime not null
)
go
Create table RelyTable
(--��������ı��˱�����ֻ�ܹ���Ա���в���
	Id int primary key identity(1,1),
	TableName varchar(32) not null,
	KeyColumnName varchar(32) not null,
	CreateTime Datetime not null ,
    Note nvarchar(1024),
    IsDelete bit not null
)
go
Create table OptionEvent
(--������ʷ ��ÿһ�β�������֮ǰ���Ӧ���е���һ�����ݽ������������IsDelete=true��
	Id uniqueidentifier primary key,
	 RelyTableId int not null,
	 EventId smallint not null,
	 CreateTime datetime not null,
	 RelyTableRowValue varchar(32) not null,
	 IsDelete bit not null
);
alter table OptionEvent add constraint fj_OptionEventId foreign key (RelyTableId) 
references RelyTable(Id)
create table ContactData
(--��ϵ����Ϣ�б�
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
 alter table ClickFlag add constraint fj_RelyTableId foreign key (RelyTableId) 
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
	IsDelete bit not null
);
create table ReportEnumRecNote
(
	Id uniqueidentifier primary key,
	ReportId uniqueidentifier not null,
	CreateTime datetime not null,
	Note nvarchar(256) not null,
	IsDelete bit not null
);
alter table ReportEnumRecNote add constraint fj_ReportId foreign key (ReportId) 
 references ReportEnumRec(Id);