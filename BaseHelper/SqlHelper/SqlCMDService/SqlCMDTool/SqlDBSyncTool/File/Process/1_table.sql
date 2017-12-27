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
	[Id] [uniqueidentifier] NOT NULL,--�������
	[Text] [nvarchar](max) NOT NULL,--job��ҵ��ѯ������
	[InsertTime] [datetime] NOT NULL,--job��ҵ�������ݵ�ʱ��
	[OriginTable] [nvarchar](50) NULL,--job��ҵ��������
	[OriginServerIp] [varchar](20) NULL,--job��ҵ�ķ�����
	[OriginUserId] [nvarchar](20) NULL,--ʹ��job��ҵ���˻�
	[OriginPassword] [nvarchar](20) NULL,--ʹ��job��ҵ���˻�����
	[OriginDataBase] [nvarchar](20) NULL --�������ݿ�
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


