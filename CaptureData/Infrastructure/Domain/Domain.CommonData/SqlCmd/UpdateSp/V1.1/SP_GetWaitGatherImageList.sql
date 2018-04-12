if(object_id('SP_GetWaitGatherImageList','p') is not null)
	drop procedure SP_GetWaitGatherImageList
go
create procedure SP_GetWaitGatherImageList
as
select top 200 ID,Uin,HeadImageUrl,province+'_'+city as LocalCity  from TecentQQData 
where GatherImageTime is null 
and HeadImageUrl not in(--头像提出腾讯提供的系统头像
    select distinct( ImageUrl) from IgnoreImage
)
and HeadImageUrl not in
(--改头像不是已经采集过得，防止同一个qq号没有修改头像时多次采集头像
select distinct( HeadImageUrl) from TecentQQData where GatherImageTime is not null
)
order by createtime