create table UinRemark
(
	Id uniqueidentifier primary key,
	Uin varchar(15) not null,
	Remark nvarchar(500) not null,--�����Ϣ
	RemarkType tinyint,--���÷����ǵ�����
	CreateTime datetime default(getdate()),--���ñ�ǵ�ʱ��
	Remarker varchar(20)--��������ǵ���	
)
go
 