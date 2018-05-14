use hrapp
go
if(object_id('EmailSendHistory','U') is not null)
	drop table EmailSendHistory
drop table AppEmailReceiver
go
create table AppEmailReceiver
(--邮件实际发送情形
	Id uniqueidentifier primary key,
	PrimaryMsgId uniqueidentifier not null,
	IsMailer bit not null,
	SendTo varchar(64) not null,
	SendTime datetime ,
	CreateTime datetime not null,
	IsDelete bit not null,
	DayInt int not null --增加快速比较标志
);
 alter table AppEmailReceiver add constraint fk_EmailSendHistoryMsgID foreign key (PrimaryMsgId) 
 references AppEmail(ID);
 create table EmailSendHistory
 (
	Id uniqueidentifier primary key,
 	SendResult bit not null,
	SendNumber smallint not null,
	SendTime datetime not null,
	PrimaryMsgId uniqueidentifier not null,
 )
 alter table EmailSendHistory add constraint fk_PrimaryMsgId foreign key (PrimaryMsgId) 
 references AppEmail(ID);