--������ʱ�����±�
--ִ��Զ�����ݿⱸ��

if(OBJECT_ID('SP_BackUpTableData','p') is not null)
	begin
		print 'drop procedure SP_BackUpTableData'
		drop procedure SP_BackUpTableData
	end
go
create procedure SP_BackUpTableData
(
	 @tableNew varchar(100),--�±�����
	 @tableOrigin varchar(100),--ԭʼ��������Զ�����ݿ�ı�
	 @columnsOrgin varchar(1000),
	 @columnsNew varchar(1000)=null
)
as
declare @tableBackUp varchar(max)
set @tableBackUp='if(OBJECT_ID('''+@tableNew+''',''table'') is  null)
begin
	select '+@columnsOrgin+' into '+@tableNew+' from '+@tableOrigin+'
	--where 1=0
end
else
	print (''exist same name table'');
'

print @tableBackUp
exec  (@tableBackUp)
go