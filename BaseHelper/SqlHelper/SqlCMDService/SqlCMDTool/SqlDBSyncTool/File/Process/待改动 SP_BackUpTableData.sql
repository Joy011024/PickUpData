USE [LinkServer]
GO
/****** Object:  StoredProcedure [dbo].[SP_BackUpTableData]    Script Date: 05/08/2017 09:43:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[SP_BackUpTableData]
(
	 @tableNew varchar(100),--新表名称
	 @LinkServerDB varchar(100),--远程数据库信息】
	 @targetTableName  varchar(100),--目标表名称
	 @columnsOrgin varchar(1000)=null,
	 @columnsNew varchar(1000)=null
)
as

if( nullif(@columnsOrgin,'') is null)
	--找出表中的列信息

	declare @linkService varchar(100)
	set @linkService=@LinkServerDB
	declare @table varchar(100)
	set @table=@targetTableName

	declare @sql nvarchar(max)
	set @sql='select  @columnsOrgin= 
				stuff(
					(
						select '','' + LTRIM(RTRIM( t.columnName)) 
						from (
							select a.name columnName
							from '+@linkService+'.sys.columns a left join '+@linkService+'.sys.objects b on a.object_id= b.object_id
							left join '+@linkService+'.sys.types c on a.system_type_id=c.system_type_id 
							where b.name='''+@table+'''
						)  as t
						for xml path('''')
					),1,1,''''
				) '
	print @sql
	exec sp_executesql @sql,N'@columnsOrgin nvarchar(max) output' ,@columnsOrgin out
	print @columnsOrgin
if(nullif(@columnsOrgin,'') is null)
	begin
		print ('not found the table')
		return
	end
print @columnsOrgin
if(nullif(@columnsOrgin,'') is null)
	begin
		print( 'peovider table not found column Info')
		return
	end
declare @tableBackUp varchar(max)
set @tableBackUp='if(OBJECT_ID('''+@tableNew+''',''table'') is  null)
begin
	select '+@columnsOrgin+' into '+@tableNew+' from '+@LinkServerDB+'.dbo.'+@targetTableName+'
	--where 1=0
end
else
	begin
		 
		print (''exist same name table'');
	end
'

print @tableBackUp
exec  (@tableBackUp)
