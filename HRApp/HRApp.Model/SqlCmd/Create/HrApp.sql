 CREATE TABLE [dbo].[CategoryItems](
	[ID] [int] NOT NULL primary key identity(1,1),--主键自增长
	[Name] [nvarchar](32) not NULL,
	[ParentID] [int] not NULL,
	[ParentCode] [nvarchar](64) not null,
	[Code] [nvarchar](64) not NULL unique,--UI层次没有设定的话通过文本转拼音进行设置
	[Sort] [int] NULL,
	[IsDelete] [bit] not NULL,
	ItemValue varchar(1024) not null,--可能存储URL
	ItemUsingSize int default(1),--改项被引用的次数【进行删除时的判断校验】
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
(--外键关联的表，此表数据只能管理员进行操作
	Id int primary key identity(1,1),
	TableName varchar(32) not null,
	KeyColumnName varchar(32) not null,
	CreateTime Datetime not null 
)
go
Create table OptionEvent
(--操作历史 ：每一次操作都把之前这对应表中的这一行数据进行清除【设置IsDelete=true】
	Id uniqueidentifier primary key,
	 RelyTableId int not null,
	 EventId smallint not null,
	 CreateTime datetime not null,
	 RelyTableRowValue varchar(32) not null,
	 IsDelete bit not null
);
create table ContactData
(--联系人信息列表
	Id uniqueidentifier primary key,
	ContactTypeId int not null,
	Value varchar(16) not null,
	ContactName nvarchar(16) not null,
	Belonger nvarchar(16) not null,
	CreateTime DateTime not null
);
alter table ContactData add constraint FK_ContactId foreign key (ContactTypeId) 
references CategoryItems(Id)