use  AccountDB
go
create table Encry
(
	Id smallint primary key identity(1,1),
	Code varchar(8) not null,--加密关键字[录入之后不运行修改]
	Remark nvarchar(128),
	IsNowEncryKey  bit not null --是否现在被使用【必须存在一行数据该值为true】 
);
create table [Role]
(
	Id smallint primary key,
	Name nvarchar(16) not null,
	Code varchar(16) not null,
	IsDelete bit null,
	Createtime datetime not null
);
Create table Enum
(
	Id int primary key identity(1,1),
	Code varchar(64) not null,-- typeof(Enum).Name+"."+Field
	Remark varchar(128) not null,
	HashValue int not null,
	Createtime datetime not null
)
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
	RoleId smallint not null,
	AuthortyId int not null,
	Createtime datetime not null
)
create table Account
(
	UserName varchar(16) not null primary key,
	Nick nvarchar(32) not null,
	CreateTime datetime not null,--注册事件
	Psw varchar(16) not null,
	EncryId smallint not null,--密码加密关键词
	IsActive bit not null--账号是否激活
);
create table AccountActiveCode
(
	UserName varchar(16),
	ActiveCode varchar(8) not null,
	GenerateCodeTime datetime not null,
	CodeValidTime datetime not null--激活码过期时间
)
create table AccountActive
(
	UserName varchar(16) not null,
	ActiveType smallint not null,
	ActiveWay varchar(32) not null,
	CreateTime datetime not null
)