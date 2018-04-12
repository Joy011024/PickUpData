if(object_id('SP_ErrorGatherImageList','p')is not null)
	drop procedure SP_ErrorGatherImageList
go
create procedure SP_ErrorGatherImageList
(
	@ID uniqueidentifier
)
as 
update TecentQQData set IsGatherImage=0,LastErrorGatherImage=getdate()
,GatherImageErrorNum=GatherImageErrorNum+1
where ID=@ID