use HrApp
go
drop  table  RelyTable
Create table RelyTable
(--��������ı��˱�����ֻ�ܹ���Ա���в���
	Id int primary key identity(1,1),
	TableName varchar(32) not null,
	ColumnName varchar(32) not null,
	CreateTime Datetime not null ,
    Note nvarchar(1024),
    IsDelete bit not null
)
go
insert   [dbo].[RelyTable] (TableName,ColumnName,CreateTime,IsDelete)
select tab.name, cell.name,GETDATE(),0 from sys.tables  tab ,sys.columns  cell
where  cell.object_id=object_id(tab.name) and tab.name not in (select TableName from RelyTable)
and cell.name not in  (select  ColumnName from RelyTable)
