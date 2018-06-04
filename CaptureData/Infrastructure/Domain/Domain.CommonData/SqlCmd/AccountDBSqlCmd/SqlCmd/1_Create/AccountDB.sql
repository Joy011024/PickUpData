use  AccountDB
go
create table Encry
(
	Id smallint primary key identity(1,1),
	Code varchar(16) not null,--���ܹؼ���[¼��֮�������޸�]
	Remark nvarchar(128),
	IsNowEncryKey  bit not null --�Ƿ����ڱ�ʹ�á��������һ�����ݸ�ֵΪtrue�� 
);
Create table Enum
(
	Id int primary key identity(1,1),
	Code varchar(64) not null,-- typeof(Enum).Name+"."+Field
	Name nvarchar(64) not null,
	Remark varchar(128) not null,
	Value int not null,
	ParentId int not null,--enum name =-1
	Createtime datetime not null,
	IsDelete bit not null
);
create table [Role]
(
	Id int primary key  identity(1,1),
	Name nvarchar(16) not null,
	Code varchar(16) not null,
	IsDelete bit null,
	Createtime datetime not null
);
create table Authorty
(
	Id int primary key identity(1,1),
	AuthortyId int not null,
	Code varchar(256) not null,
	Remark nvarchar(128) ,
	ParentId int not null,--null-> -1 // tree ,  button in Ui
	CreateTime datetime not null
)
create table RoleAuthorty
(
	RoleId int not null,
	AuthortyId int not null,
	Createtime datetime not null
);
create table [User]
(
	Id uniqueidentifier primary key,
	UserName varchar(16) not null unique,
	Nick nvarchar(32) not null,
	CreateTime datetime not null,--ע���¼�
	Psw varchar(16) not null,
	Encry varchar(8) not null,--������ܹؼ���
	IsActive bit not null--�˺��Ƿ񼤻�
);
create table UserActiveCode
(
	
	UserId  uniqueidentifier not null,
	ActiveCode varchar(8) not null,
	GenerateCodeTime datetime not null,
	CodeValidTime datetime not null,--���������ʱ��
	DayInt int not null
);
create table UserActive
(
	UserName varchar(16) not null,
	ActiveType smallint not null,
	ToolCode varchar(32) not null,
	CreateTime datetime not null
);
Create table UserRole
(
	Id uniqueidentifier primary key,
	UserId uniqueidentifier not null,
	RoleId int not null ,
	Createtime datetime not null
);
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
