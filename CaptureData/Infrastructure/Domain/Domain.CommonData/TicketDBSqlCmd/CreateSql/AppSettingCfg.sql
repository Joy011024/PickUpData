--create database TicketDB;
go
create  table AppSettingCfg
(
	Id	uniqueidentifier  primary key,
	CfgName nvarchar(64) not null,--����������,
	CfgCode varchar(64) not null,--���������
	CfgValue nvarchar(1024),
	ParentId uniqueidentifier not null,--����id �����û�и��� ���ֵ����Ϊ 00000000-0000-0000-0000-000000000000��
	CreateTime datetime not null,
	Note nvarchar(1024),--����������
	IsDelete bit, --�Ƿ�ɾ��������
	InDBDayInt int
);
go
--��������վ�Լ����˹�ϵ
create table SubwaySiteData
(
	Id  uniqueidentifier  primary key,
	InDBDayInt  int not null,
	CreateTime datetime not null,
	BelongLine int not null,
	TurnLine int not null,
	SubwaySiteName  nvarchar(32) not null,
	SubwaySiteTimeLong  smallint not null
);
go
create table SubwayCollection
(
	Id uniqueidentifier primary key,
	Name nvarchar(32) not null,--��������������
	Code smallint ,
	City nvarchar(32) --����
);