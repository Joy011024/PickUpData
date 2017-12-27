--导出服务器数据到本地
--1 首先需要在本地数据库中配置远程服务器的访问
--2使用配置的远程服务器信息进行数据导入操作
--import menus
insert into T_Menus
	(
	F_MenuID,
	[F_Name]
	,[F_FormName]
	,[F_ParentID]
	,[F_IsUsed]
	,[F_Sort] 
	)
select F_MenuID, [F_Name]
      ,[F_FormName]
      ,[F_ParentID]
      ,[F_IsUsed]
      ,[F_Sort] 
from LKSV_GSS_1_gssdb_0_0.gssdb.dbo.T_Menus m
where m.F_Name not in (select F_Name from T_Menus)
go
--import department
 insert into T_Department 
      ( [F_ParentID]
      ,[F_DepartName]
      ,[F_Note])
select [F_ParentID]
  ,[F_DepartName]
  ,[F_Note] from  LKSV_GSS_1_gssdb_0_0.gssdb.dbo.T_Department d
where d.F_DepartName not in (
     select F_DepartName from T_Department
)
go     
--import roles
insert into GSSDB.dbo.T_Roles
([F_RoleName]
      ,[F_IsUsed]
      ,[F_Power]
)
select [F_RoleName]
      ,[F_IsUsed]
      ,[F_Power] from LKSV_GSS_1_gssdb_0_0.gssdb.dbo.T_roles t
      where  t.F_RoleName not in (select F_RoleName from GSSDB.dbo.T_Roles)
go
--import用户数据
insert into GSSDB.dbo.T_Users
([F_UserName]
      ,[F_PassWord]
      ,[F_DepartID]
      ,[F_RoleID]
      ,[F_RealName]
      ,[F_Sex]
      ,[F_Birthday]
      ,[F_Email]
      ,[F_MobilePhone]
      ,[F_Telphone]
      ,[F_Note]
      ,[F_RegTime]
      ,[F_LastInTime]
      ,[F_IsUsed])
select [F_UserName]
      ,[F_PassWord]
      ,[F_DepartID]
      ,[F_RoleID]
      ,[F_RealName]
      ,[F_Sex]
      ,[F_Birthday]
      ,[F_Email]
      ,[F_MobilePhone]
      ,[F_Telphone]
      ,[F_Note]
      ,[F_RegTime]
      ,[F_LastInTime]
      ,[F_IsUsed] from LKSV_GSS_1_gssdb_0_0.gssdb.dbo.T_users u
   where u.F_UserName not in (select F_UserName from GSSDB.dbo.T_Users)