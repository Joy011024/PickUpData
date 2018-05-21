use  AccountDB
go
create table Encry
(
	Id smallint primary key identity(1,1),
	Code varchar(8) not null,--���ܹؼ���[¼��֮�������޸�]
	Remark nvarchar(128),
	IsNowEncryKey  bit not null --�Ƿ����ڱ�ʹ�á��������һ�����ݸ�ֵΪtrue�� 
);
Create table Enum
(
	Id int primary key identity(1,1),
	Code varchar(64) not null,-- typeof(Enum).Name+"."+Field
	Remark varchar(128) not null,
	HashValue int not null,
	Createtime datetime not null
);
create table [Role]
(
	Id int primary key,
	Name nvarchar(16) not null,
	Code varchar(16) not null,
	IsDelete bit null,
	Createtime datetime not null
);
create table Authorty
(
	Id int primary key,
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
	EncryId smallint not null,--������ܹؼ���
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
)