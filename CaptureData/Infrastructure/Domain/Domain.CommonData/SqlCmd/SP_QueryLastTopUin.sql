USE [TecentDataUinDA]
GO

/****** Object:  StoredProcedure [dbo].[SP_QueryLastTopUin]    Script Date: 08/13/2017 15:31:48 ******/
SET ANSI_NULLS ON
GO
if (object_id('SP_QueryLastTopUin','p')is not null)
	drop procedure SP_QueryLastTopUin
go
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[SP_QueryLastTopUin]
(
	@total int output
)
as

select @total=count(uin)
from  dbo.TecentQQData 
select top 1000 ID,HeadImageUrl,nick,gender,uin,country,province,city,CreateTime 
from  dbo.TecentQQData order by CreateTime desc
GO


