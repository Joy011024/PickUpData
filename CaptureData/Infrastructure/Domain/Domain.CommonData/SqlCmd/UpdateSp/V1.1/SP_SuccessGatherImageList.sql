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
--��ͬͷ��qq�Ų�ͬ������ͬһ��qq��������ͷ��û�иı�
update TecentQQData set ImageRelativePath=@ImageRelativePath,IsGatherImage=1,GatherImageTime=@time
where GatherImageTime is null and HeadImageUrl=@url