--create database TicketDB
--car information
--����վ����Ϣ�����ô�����д洢��ֱ�Ӵ洢����
Create table TicketDetailData
(
	Id uniqueidentifier primary key,
	TranNo varchar(16) not null,
	Trips varchar(16) not null,
	FromStation nvarchar(32) not null,
	ToStation nvarchar(32) not null,
	StartStation nvarchar(32) not null,
	EndStation nvarchar(32) not null,
	GoTime varchar(5) not null,
	ToTime varchar(5) not null,
	TimeConsuming varchar(5) not null,
	CreateTime datetime default(getdate())
)
create table CarInfo
(
	Id uniqueidentifier primary key,
	TranNo varchar(16) not null,
	CarNo varchar(16) not null,--�г������Ϣ
	BeginStation varchar(32) not null,--�������ڼ�¼��ѯ�����ǵĳ˿�Ԥ����վ
	ToStation varchar(32) not null,--Ԥ����վ
	StartStation varchar(32) not null ,--�г�ʼ��վ
	EndStation varchar(32) not null,--�г��ص�վ
	CreateTime Datetime default(getdate()),--���ݴ洢����
	
)
Create table TicketSeatData
(
	Id uniqueidentifier primary key,
	CarFKId uniqueidentifier not null,
	TicketId uniqueidentifier not null,--����Ʊ�����͵����α��
	CreateTime datetime not null,--���ݲɼ�ʱ��
	TicketNum int default(-1),--��Ʊ���������ɼ�����ƱΪ���ǣ���ֵ����Ϊ-1������Ʊʱ����0
	TicketNumDesc nvarchar(2),--��Ʊ��������,[��Ʊ���ھ������Ŀʱ��ֵΪ��ֵ��������д��������]
	TicketPrice float ,--Ʊ��
	TicketType varchar(8)--��Ʊ����
)
alter table  TicketSeatData add constraint FK_TrainNo foreign key (CarFKId) references CarInfo(Id)
 