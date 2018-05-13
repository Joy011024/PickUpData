select *  into ReserveEmailAccountBU from ReserveEmailAccount
if(object_id('EveryDayActive','U') is not null)
	drop table EveryDayActive
if(object_id('ReserveEmailAccount','U') is not null)
	drop table ReserveEmailAccount
create table ReserveEmailAccount
(--±∏”√” œ‰
	Id smallint identity(1,1) primary key,
	Account varchar(32) not null,
	AuthortyCode varchar(32) not null,
	Smtp smallint not null,
	SmtpHost varchar(32) not null,
	CreateTime datetime not null,
	IsDelete bit not null,
	Remark nvarchar(128),
	UsePriority smallint not null,
	IsPrimaryAccount bit not null,
	IsEnable bit not null
);
Create table EveryDayActive
(
	ID uniqueidentifier primary key,
	EmailAccountId smallint not null,
	CreateTime datetime not null,
	IsSuccess bit not null,
	DayActiveNumber int not null,
	DayInt int not null
)
alter table EveryDayActive add constraint fk_EmailAccountId foreign key (EmailAccountId) 
references ReserveEmailAccount(ID);
insert into ReserveEmailAccount
select [Account], [AuthortyCode], [Smtp], [SmtpHost], [CreateTime], [IsDelete], [Remark], [UsePriority], [IsPrimaryAccount], [IsEnable]
from ReserveEmailAccountBU;
drop table ReserveEmailAccountBU
