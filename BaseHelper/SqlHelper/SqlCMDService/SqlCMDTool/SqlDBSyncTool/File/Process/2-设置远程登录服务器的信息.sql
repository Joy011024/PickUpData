	declare @linkName nvarchar(50)
	declare @user nvarchar(20)
	set @user='sa'
	declare @psw nvarchar(20)
	set @psw='5225qs5a5a#'
	set @linkName='LKSV_1_GameLogDB_0_22'
--1 ����
	--����Զ�����ӷ�����
	exec sp_addlinkedserver @linkName,'','SQLOLEDB','192.168.1.220'
	--����Զ�����ӷ������ĵ�¼�˻���Ϣ
	exec sp_addlinkedsrvlogin @linkName,'false',null,@user,@psw
	
--2Զ�̷���������

	--����Զ�����ӷ�����,���в�ѯ
--	select * from LKSV_0_DigGameDB_0_0.DigGameDB.dbo.T_BaseParamDB
----ɾ��Զ������
--	exec sp_dropserver @linkName,'droplogins' --VIRTUALMACHINE1 Ϊ���õ�Զ�̷�����������