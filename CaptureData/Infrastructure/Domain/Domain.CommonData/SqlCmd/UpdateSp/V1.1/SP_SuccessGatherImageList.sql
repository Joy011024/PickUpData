if(object_id('SP_SuccessGatherImageList','p')is not null)
	drop procedure SP_SuccessGatherImageList
go
create procedure SP_SuccessGatherImageList
(
	@ID uniqueidentifier,
	@ImageRelativePath varchar(200)
)
as 
declare @url varchar(500)
select @url=HeadImageUrl from TecentQQData where ID=@ID
declare @time datetime
set @time=getdate()
update TecentQQData set ImageRelativePath=@ImageRelativePath,IsGatherImage=1,GatherImageTime=@time
where ID=@ID
--相同头像（qq号不同）或者同一个qq号数据且头像没有改变
update TecentQQData set ImageRelativePath=@ImageRelativePath,IsGatherImage=1,GatherImageTime=@time
where GatherImageTime is null and HeadImageUrl=@url