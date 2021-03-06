USE [TecentDataDA]
GO
/****** Object:  StoredProcedure [dbo].[SP_AddLinkServer]    Script Date: 08/05/2017 21:11:55 ******/
SET ANSI_NULLS ON
GO
if(object_id('SP_AddLinkServer','p')is not null)
	drop procedure SP_AddLinkServer
go
SET QUOTED_IDENTIFIER ON
GO
create Procedure [dbo].[SP_AddLinkServer]
(
	@IP varchar(100)
)
as
declare @linkName nvarchar(50)
declare @user nvarchar(20)
set @user='sa'
declare @psw nvarchar(20)
set @psw='password'

declare @time varchar(25) 
set @time= replace( convert(varchar(55), getdate(),120),'-','')
set @time= replace( @time,':','')
set @time= replace( @time,' ','') 
set @linkName='LinkPC'+@time
DECLARE @DB NVARCHAR(30)
set @DB='TecentDataDA'
--DECLARE @IP NVARCHAR(30)  --远程服务器连接实例
--set @IP='192.168.241.142'
if(exists(select name from sys.servers where name=@linkName))
	exec sp_dropserver @linkName,'droplogins'
--配置数据库连接信息
EXEC master.dbo.sp_addlinkedserver @server = @linkName, @srvproduct=N'SQLSERVER', @provider=N'SQLOLEDB', @datasrc=@IP,@catalog=@DB
--配置登录账户信息
exec sp_addlinkedsrvlogin @linkName,'false',null,@user,@psw

insert into dbo.LinkServer (Name,CreateTime,IsDelete,Statue,DBTypeID,LinkNameRemark)	
values(@linkName,getdate(),0,1,1,'The Link name for Sync other pc gather tecent data')
--2远程服务器操作

	--操作远程连接服务器,进行查询
--	select * from LKSV_0_DigGameDB_0_0.DigGameDB.dbo.T_BaseParamDB
----删除远程连接
--	exec sp_dropserver @linkName,'droplogins' --VIRTUALMACHINE1 为设置的远程服务器连接名
