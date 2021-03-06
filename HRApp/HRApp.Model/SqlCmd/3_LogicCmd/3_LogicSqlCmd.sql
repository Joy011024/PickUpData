using HRApp
go
create procedure SP_QueryAppSetByPage
(
	@beginRow int,
	@endRow int,
	@count int out
)
as
select @count=count(ID) from [CategoryItems]
select [ID],[Name],[ParentID],[ParentCode],[Code],[Sort],[IsDelete],[ItemValue],[ItemUsingSize],[CreateTime],[NodeLevel],[ItemDesc]
from
(
	SELECT  row_number() over(order by Createtime)as Tag,  
	[ID],[Name],[ParentID],[ParentCode],[Code],[Sort],[IsDelete],[ItemValue],[ItemUsingSize],[CreateTime],[NodeLevel],[ItemDesc]
	FROM  [dbo].[CategoryItems]
) temp where tag>@beginRow and tag<@endRow