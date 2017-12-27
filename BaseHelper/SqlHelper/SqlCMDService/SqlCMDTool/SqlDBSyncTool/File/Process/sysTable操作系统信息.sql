--使用系统内置功能操作表信息
--选择数据库
declare @table varchar(100)
set @table='T_UserEnterExitLog20170506'
select a.name columnName
from sys.columns a left join sys.objects b on a.object_id= b.object_id
left join sys.types c on a.system_type_id=c.system_type_id 
where b.name=@table

--将单行的某一列数据合并

select   
	stuff(
		(
			select ',' + LTRIM(RTRIM( t.columnName)) 
			from (
				select a.name columnName
				from sys.columns a left join sys.objects b on a.object_id= b.object_id
				left join sys.types c on a.system_type_id=c.system_type_id 
				where b.name=@table
			)  as t
			for xml path('')
		),1,1,''
	) 
	
	
--纯字符串

--找出表中的列信息

--declare @linkService varchar(100)
--set @linkService='LKSV_0_GameLogDB_0_0.GameLogDB'
--declare @table varchar(100)
--set @table='T_UserEnterExitLog20170505'
--declare @sql nvarchar(max)
--declare @columnsOrgin nvarchar(max)
--set @sql='select  @columnsOrgin= 
--			stuff(
--				(
--					select '','' + LTRIM(RTRIM( t.columnName)) 
--					from (
--						select a.name columnName
--						from '+@linkService+'.sys.columns a left join '+@linkService+'.sys.objects b on a.object_id= b.object_id
--						left join '+@linkService+'.sys.types c on a.system_type_id=c.system_type_id 
--						where b.name='''+@table+'''
--					)  as t
--					for xml path('''')
--				),1,1,''''
--			) '
--print @sql
--exec sp_executesql @sql,N'@columnsOrgin nvarchar(max) output' ,@columnsOrgin out
--print @columnsOrgin
--if(nullif(@columnsOrgin,'') is null)
--	begin
--		print ('not found the table')
--		return
--	end
