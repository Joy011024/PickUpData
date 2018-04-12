use tecentdatauinda
go
if( not exists(
select c.name from sys.columns c,  sys.objects t
where c.object_id=t.object_id and t.name='TecentQQData' and t.type='u' and c.name='ImgType'  ) )
	begin
		alter table TecentQQData add ImgType int default (0) 
	end
