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
	CreateTime Datetime default(getdate()),
	IsGatherImage bit ,--是否已成功采集图像
	GatherImageTime datetime,--成功采集头像时间
	LastErrorGatherImage datetime ,--上次采集头像失败时间
	GatherImageErrorNum int,--采集头像失败次数
	ImageRelativePath varchar(200)
)
--忽略采集头像列表
create table IgnoreImage
(
	Id uniqueidentifier primary key,
	ImageUrl  varchar(500),--那些头像集合可以不采集
	CreateTime Datetime,
	IsDelete bit
)