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
	PickUpWhereId uniqueidentifier, --���ݲɼ�ʱʹ�õ�������
	age int ,
	city nvarchar(50),
	country nvarchar(50),
	distance int,
	face int,
	gender int ,
	nick nvarchar(100),
	province nvarchar(50),
	stat int ,--QQ�˻�״̬
	uin nvarchar(15),
	HeadImageUrl varchar(500),
	CreateTime Datetime default(getdate()),
	IsGatherImage bit ,--�Ƿ��ѳɹ��ɼ�ͼ��
	GatherImageTime datetime,--�ɹ��ɼ�ͷ��ʱ��
	LastErrorGatherImage datetime ,--�ϴβɼ�ͷ��ʧ��ʱ��
	GatherImageErrorNum int,--�ɼ�ͷ��ʧ�ܴ���
	ImageRelativePath varchar(200)
)
--���Բɼ�ͷ���б�
create table IgnoreImage
(
	Id uniqueidentifier primary key,
	ImageUrl  varchar(500),--��Щͷ�񼯺Ͽ��Բ��ɼ�
	CreateTime Datetime,
	IsDelete bit
)