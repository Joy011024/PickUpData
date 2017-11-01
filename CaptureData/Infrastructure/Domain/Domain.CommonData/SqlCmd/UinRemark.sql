create table UinRemark
(
	Id uniqueidentifier primary key,
	Uin varchar(15) not null,
	Remark nvarchar(500) not null,--标记信息
	RemarkType tinyint,--设置分类标记的类型
	CreateTime datetime default(getdate()),--设置标记的时间
	Remarker varchar(20)--本次做标记的人	
)
go
 