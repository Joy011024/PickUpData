CREATE TABLE [dbo].[CategoryItems](
	[ID] [int] NOT NULL primary key,
	[Name] [nvarchar](32) NULL,
	[ParentID] [int] NULL,
	[ParentCode] [nvarchar](64) ,
	[Code] [nvarchar](64) not NULL,--UI���û���趨�Ļ�ͨ���ı�תƴ����������
	[Sort] [int] NULL,
	[IsDelete] [bit] NULL,
	ItemUsingSize int default(1),--������õĴ���������ɾ��ʱ���ж�У�顿
	[CreateTime] [datetime] NULL,
	[NodeLevel] [int] default(0),
	ItemDesc nvarchar(1024)
)  