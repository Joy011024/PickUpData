	declare @linkName nvarchar(50)
	declare @user nvarchar(20)
	set @user='sa'
	declare @psw nvarchar(20)
	set @psw='5225qs5a5a#'
	set @linkName='LKSV_1_GameLogDB_0_22'
--1 设置
	--创建远程连接服务器
	exec sp_addlinkedserver @linkName,'','SQLOLEDB','192.168.1.220'
	--设置远程连接服务器的登录账户信息
	exec sp_addlinkedsrvlogin @linkName,'false',null,@user,@psw
	
--2远程服务器操作

	--操作远程连接服务器,进行查询
--	select * from LKSV_0_DigGameDB_0_0.DigGameDB.dbo.T_BaseParamDB
----删除远程连接
--	exec sp_dropserver @linkName,'droplogins' --VIRTUALMACHINE1 为设置的远程服务器连接名