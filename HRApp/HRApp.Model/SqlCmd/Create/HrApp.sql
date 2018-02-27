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
	CreateTime Datetime not null 
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
)