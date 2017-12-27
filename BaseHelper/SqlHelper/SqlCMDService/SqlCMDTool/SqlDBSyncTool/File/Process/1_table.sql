USE [LinkServer]
GO

/****** Object:  Table [dbo].[JobTableData]    Script Date: 05/03/2017 14:04:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[JobTableData](
	[Id] [uniqueidentifier] NOT NULL,--随机主键
	[Text] [nvarchar](max) NOT NULL,--job作业查询的数据
	[InsertTime] [datetime] NOT NULL,--job作业插入数据的时间
	[OriginTable] [nvarchar](50) NULL,--job作业的所属表
	[OriginServerIp] [varchar](20) NULL,--job作业的服务器
	[OriginUserId] [nvarchar](20) NULL,--使用job作业的账户
	[OriginPassword] [nvarchar](20) NULL,--使用job作业的账户密码
	[OriginDataBase] [nvarchar](20) NULL --所属数据库
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


