if(object_id('DBTypeDesc','u') is not null)
	drop table DBTypeDesc
go
create table DBTypeDesc
(
	Id smallint identity(1,1) primary key,
	Name varchar(30) not null,
	CreateTime datetime default(getdate()),
	Remark nvarchar(100),
	IsDelete bit
)
if(object_id('LinkServer','u') is not null)
	drop table LinkServer
go
Create table LinkServer
(
	Id int identity(1,1) primary key,
	Name varchar(100) not null,--·þÎñÆ÷
	LinkNameRemark nvarchar(200),
	CreateTime Datetime default(getdate()),
	IsDelete bit,
	Statue smallInt,
	DBTypeID smallint
)