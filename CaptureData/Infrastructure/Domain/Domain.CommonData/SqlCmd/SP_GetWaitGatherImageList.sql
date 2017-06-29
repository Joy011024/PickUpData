if(object_id('SP_GetWaitGatherImageList','p') is not null)
	drop procedure SP_GetWaitGatherImageList
go
create procedure SP_GetWaitGatherImageList
as
select top 200 ID,Uin,HeadImageUrl from TecentQQData 
where GatherImageTime is null 
and HeadImageUrl not in(--ͷ�������Ѷ�ṩ��ϵͳͷ��
    select distinct( ImageUrl) from IgnoreImage
)
and HeadImageUrl not in
(--��ͷ�����Ѿ��ɼ����ã���ֹͬһ��qq��û���޸�ͷ��ʱ��βɼ�ͷ��
select distinct( HeadImageUrl) from TecentQQData where GatherImageTime is not null
)
order by createtime