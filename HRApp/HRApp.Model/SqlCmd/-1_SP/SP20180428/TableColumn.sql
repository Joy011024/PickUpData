use HrApp
go
alter table RelyTable add  IsPrimaryKey bit not null;
alter table RelyTable add  ColumnName varchar(32) not null;
go
insert   [dbo].[RelyTable] (TableName,ColumnName,CreateTime,IsDelete,IsPrimaryKey)
select tab.name, cell.name,GETDATE(),0,0 from sys.tables  tab ,sys.columns  cell
where  cell.object_id=object_id(tab.name) and tab.name not in (select TableName from RelyTable)
and cell.name not in  (select  ColumnName from RelyTable)
