 CREATE TABLE [dbo].[CategoryItems](
	[ID] [int] NOT NULL primary key identity(1,1),--主键自增长
	[Name] [nvarchar](32) NULL,
	[ParentID] [int] NULL,
	[ParentCode] [nvarchar](64) ,
	[Code] [nvarchar](64) not NULL,--UI层次没有设定的话通过文本转拼音进行设置
	[Sort] [int] NULL,
	[IsDelete] [bit] NULL,
	ItemUsingSize int default(1),--改项被引用的次数【进行删除时的判断校验】
	[CreateTime] [datetime] NULL,
	[NodeLevel] [int] default(0),
	ItemDesc nvarchar(1024)
)  