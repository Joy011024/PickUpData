USE [TecentDataDA]
GO

/****** Object:  Table [dbo].[DataBaseNPKVersion]    Script Date: 06/18/2017 20:49:39 ******/
SET ANSI_NULLS ON
GO
if(object_id('DataBaseNPKVersion','u') is not null)
	drop table DataBaseNPKVersion
go
SET QUOTED_IDENTIFIER ON
GO

if(object_id('DataBaseNPKVersion','u') is not null)
	drop procedure DataBaseNPKVersion
go
SET ANSI_PADDING ON
GO
Create table DataBaseNPKVersion
(
	Id int identity(1,1) primary key,
	NPKEffectVersion varchar(20) not null,--补丁作用版本
	GenerateNPKVersion varchar(20),--补丁新生成安装包的版本号（可能记录前还没有为该补丁生成安装包）
	NPKSvn varchar(10),--补丁提交记录
	NPKSubmitTime datetime not null,--修改日期
	NPKPath nvarchar(200),--补丁存放路径（对于修改内容较多）
	DBStructCmd nvarchar(500),--当数据库结构发生改变时必须把升级命令进行存储
	NPKType int ,--补丁修改分类 1 表结构修改 2 业务数据变动 3 存储过程变动 4 函数变动
	Note nvarchar(100),--补丁备注
	InDBTime datetime default(getdate()),--补丁入库时间
	IsDelete bit ,--是否删除该补丁
	IsRelease bit ,--补丁是否发布
	NPKAuthor nvarchar(20),--本次补丁操作人
	NpkSubmiter nvarchar(20),--补丁提交人
	NPKDataBase varchar(30) --本次补丁针对的数据库
)