--create database TicketDB;
go
create  table AppSettingCfg
(
	Id	uniqueidentifier  primary key,
	CfgName nvarchar(64) not null,--配置项名称,
	CfgCode varchar(64) not null,--配置项代码
	CfgValue nvarchar(1024),
	ParentId uniqueidentifier not null,--父项id 【如果没有父项 则此值设置为 00000000-0000-0000-0000-000000000000】
	CreateTime datetime not null,
	Note nvarchar(1024),--配置项描述
	IsDelete bit, --是否删除配置项
	InDBDayInt int
);
go
--各个地铁站以及换乘关系
create table SubwaySiteData
(
	Id  uniqueidentifier  primary key,
	InDBDayInt  int not null,
	CreateTime datetime not null,
	BelongLine int not null,
	TurnLine int not null,
	SubwaySiteName  nvarchar(32) not null,
	SubwaySiteTimeLong  smallint not null
);
go
create table SubwayCollection
(
	Id uniqueidentifier primary key,
	Name nvarchar(32) not null,--地铁所属城市名
	Code smallint ,
	City nvarchar(32) --城市
);