USE [TecentDataDA]
GO
/****** Object:  Table [dbo].[CategoryData]    Script Date: 06/18/2017 12:08:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CategoryData](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](20) NULL,
	[ParentID] [int] NULL,
	[ParentCode] [nvarchar](10) NULL,
	[Code] [nvarchar](10) NULL,
	[Sort] [int] NULL,
	[ItemType] [varchar](20) NULL,
	[IsDelete] [bit] NULL,
	[CreateTime] [datetime] NULL,
	[NodeLevel] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CategoryData] ADD  DEFAULT ((0)) FOR [Sort]
GO
ALTER TABLE [dbo].[CategoryData] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO