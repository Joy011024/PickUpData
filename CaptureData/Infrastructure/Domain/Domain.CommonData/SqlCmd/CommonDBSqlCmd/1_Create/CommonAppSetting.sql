use CommonAppSetting
go
create  table Grid
(
	Id uniqueidentifier primary key,
	GridName varchar(16) not null,
	GridDesc nvarchar(1024),
	IsDelete bit not null
);
create table GridCell
(
	Id uniqueidentifier primary key,
	Cell varchar(16) not null,
	[Text] nvarchar(32) not null,
	Remark nvarchar(1024) not null,
	Display bit not  null,
	Sort smallint not null,
	GridId uniqueidentifier not null
);
alter table GridCell add constraint FK_GridId foreign key (GridId) references Grid(Id);