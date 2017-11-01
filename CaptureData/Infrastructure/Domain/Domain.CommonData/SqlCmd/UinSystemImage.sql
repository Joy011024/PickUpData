USE [TecentDataDA]
GO
if(object_id('UinSystemImage','u') is not null)
	drop table UinSystemImage
go
--uin change image but also uin sytstem image
CREATE TABLE [dbo].[UinSystemImage](
	[Id] [uniqueidentifier] NOT NULL,
	[ImageUrl] [varchar](500) NULL,
	Uin varchar(15) null,
	[CreateTime] [datetime] default(getdate()),
	[IsDelete] [bit] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
if(object_id('SP_AddUinSystemImage','p') is not null)
	drop procedure SP_AddUinSystemImage
go
create procedure SP_AddUinSystemImage
(
	@uin varchar(15),
	@id uniqueidentifier,
	@imageUrl varchar(500),
	@createtime datetime=null,
	@isSystemImage bit
)
as
begin
	if(nullif(@createtime,'') is null)
		set @createtime=getdate()
	if(@isSystemImage=1)
		begin
			INSERT INTO [dbo].[UinSystemImage]
			  ([Id] ,[ImageUrl] ,[Uin] ,[CreateTime] ,[IsDelete])
			VALUES (@id,@imageUrl,@uin ,@createtime,0)
		end
	else
		begin
			INSERT INTO  [dbo].UserImage
			  ([Id] ,[ImageUrl] ,[Uin] ,[CreateTime] ,[IsDelete])
			VALUES (@id,@imageUrl,@uin ,@createtime,0)
		end
end
CREATE TABLE [dbo].[UserImage](
	[Id] [uniqueidentifier] NOT NULL,
	[ImageUrl] [varchar](500) NULL,
	Uin varchar(15) null,
	[CreateTime] [datetime] default(getdate()),
	[IsDelete] [bit] NULL
) ON [PRIMARY]
