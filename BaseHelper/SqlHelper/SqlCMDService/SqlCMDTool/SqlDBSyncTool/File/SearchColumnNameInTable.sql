--��ȡ���ݿ����Ϣ�����Ѿ����е��С�
use master
go
if(object_id('{SP_SearchColumnNamesInTable}','p') is not null)
	 begin
		return;
	 end
create rocedure [dbo].[{SP_SearchColumnNamesInTable}]
(
	 @tableNew varchar(100),--�±�����
	 @LinkServerDB varchar(100),--Զ�����ݿ���Ϣ��
	 @targetTableName  varchar(100),--Ŀ�������
	 @columnsOrgin varchar(1000)=null,
	 @columnsNew varchar(1000)=null
)
as
if( nullif(@columnsOrgin,'') is null)
	--�ҳ����е�����Ϣ

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
'

print @tableBackUp
exec  (@tableBackUp)
