--create database TicketDB
--car information
--对于站点信息均采用代码进行存储不直接存储名称
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
	CarNo varchar(16) not null,--列车编号信息
	BeginStation varchar(32) not null,--可以用于记录查询数据是的乘客预出发站
	ToStation varchar(32) not null,--预到达站
	StartStation varchar(32) not null ,--列车始发站
	EndStation varchar(32) not null,--列车重点站
	CreateTime Datetime default(getdate()),--数据存储日期
	
)
Create table TicketSeatData
(
	Id uniqueidentifier primary key,
	CarFKId uniqueidentifier not null,
	TicketId uniqueidentifier not null,--多种票务类型的批次编号
	CreateTime datetime not null,--数据采集时间
	TicketNum int default(-1),--车票数量【当采集到余票为有是，此值设置为-1】，无票时设置0
	TicketNumDesc nvarchar(2),--车票数量描述,[车票存在具体的数目时此值为数值，否则填写描述内容]
	TicketPrice float ,--票价
	TicketType varchar(8)--车票类型
)
alter table  TicketSeatData add constraint FK_TrainNo foreign key (CarFKId) references CarInfo(Id)
 