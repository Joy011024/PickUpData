use hrapp
go
drop table menu
Create table Menu
(
	Id int primary key identity(1,1),
	Name nvarchar(32) not null,
	Code varchar(64) not null,
	Url varchar(256) ,
	Remark nvarchar(1024),
	CreateTime datetime not null,
	ParentId int  not null,--If it is root ,the value is -1
	MenuType SmallInt not null --Ele =1, menu=2,
);
go
SET IDENTITY_INSERT [dbo].[Menu] on
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (1, N'新增菜单', N'XinZengCaiDan', N'/Menu/NewNemu', N'提供直接创建系统菜单的快捷通道', CAST(0x0000A8930171741F AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (2, N'系统配置', N'XiTongPeiZhi', N'/AppSettingManage/AppSettingList', N'系统配置', CAST(0x0000A89301721895 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (3, N'程序管理', N'ChengXuGuanLi', N'/AppSettingManage/AppDataDialog', N'程序名称维护通道', CAST(0x0000A89301726118 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (4, N'生僻字维护', N'ShengPiZiWeiHu', N'/SpellName/SpecialSpellNameDialog', N'录入生僻字的通道', CAST(0x0000A8930172E0B7 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (5, N'上传文件', N'ShangChuanWenJian', N'/FileHelper/FileUpload', N'该路径进行文件批量上传操作', CAST(0x0000A894016CF0E1 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (6, N'新增联系人', N'XinZengLianXiRen', N'/ContactManage/AddContacterDialog', N'新增联系人联系信息的快捷通道', CAST(0x0000A89D01804199 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (7, N'菜单列表', N'CaiDanLieBiao', N'/Menu/MenuList', NULL, CAST(0x0000A8A001178699 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (8, N'qq图片分类处理', N'UinImageGroupService', N'/ContactManage/UinImageGroup', N'采集的qq账号分组管理', CAST(0x0000A8A4017460C1 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (9, N'货物维护管理', N'GoodsManage', N'/OrderManage/GoodsManage', N'商品库存信息管理', CAST(0x0000A8B80107F1B9 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (10, N'发送邮件', N'SendEmailDialog', N'/EmailData/SendEmailDialog', N'打开该对话框选择发送的邮件信息，邮件接收人，邮件内容，邮件定时发送时间', CAST(0x0000A8E10188CFC3 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (11, N'企业列表', N'CompanyList', N'/OrganizationManage/OrganzeListManage', N'公司，组织架构数据列表【这是涉及到系统面向服务的群体】', CAST(0x0000A8E2012B3CA0 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (12, N'账号列表', N'AccountList', N'/AppAccountManage/AccountList', N'系统账号列表', CAST(0x0000A8E2012E0578 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (13, N'枚举集合', N'EnumList', N'/EnumData/EnumList', N'程序逻辑中使用的枚举', CAST(0x0000A8E8009AE97E AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (14, N'日志输出', N'LogDataOutPut', N'/LogData/LogDataList', N'程序运行的日志输出', CAST(0x0000A8E8009B1DF6 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (15, N'菜单维护', N'MenuDataService', N'/Menu/MenuList', N'进行系统菜单维护功能', CAST(0x0000A8FA0183771A AS DateTime), 0, 0)
SET IDENTITY_INSERT [dbo].[Menu] OFF