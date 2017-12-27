--检查表是否存在
if(OBJECT_ID('f_TableExists','fn') is not null)
	begin
		print 'drop function f_TableExists'
		drop function f_TableExists
	end
go
create function f_TableExists
(
	@db nvarchar(100)=null,
	@tableName nvarchar(100) --表名称
)
Returns real
as
begin
	if(nullif(@db,'') is not null)
		set @db+='.'
	else set @db=''
	declare @sql nvarchar(max)
	set @sql=' select @num= count(name) from '+@db+'sys.objects where type=''u'' and name='''+@tableName+''''
	declare @ex real 
	exec sp_Executesql @sql,N'@num int output',@ex out
	if( @ex is null)
		set @ex= -1
	Return @ex
end