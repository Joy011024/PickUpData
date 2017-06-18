create table PickUpTecentQQWhere
(
	ID uniqueidentifier primary key,
	keyword NVARCHAR(50),
	sessionid  int,
	agerg int,
	sex int,
	firston int,
	video int,
	country int,
	province int,
	city int,
	district int,
	hcountry int,
	hprovince int,
	hcity int,
	hdistrict int,
	online int
)
Create table TecentQQData
(
	ID uniqueidentifier primary key,
	PickUpWhereId uniqueidentifier, --数据采集时使用到的条件
	age int ,
	city nvarchar(50),
	country nvarchar(50),
	distance int,
	face int,
	gender int ,
	nick nvarchar(100),
	province nvarchar(50),
	stat int ,--QQ账户状态
	uin nvarchar(15),
	HeadImageUrl varchar(500),
	CreateTime Datetime default(getdate())
)