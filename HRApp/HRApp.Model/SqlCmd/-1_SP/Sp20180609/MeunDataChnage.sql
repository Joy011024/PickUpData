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
SET IDENTITY_INSERT [dbo].[CategoryItems] OFF
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (1, N'�����˵�', N'XinZengCaiDan', N'/Menu/NewNemu', N'�ṩֱ�Ӵ���ϵͳ�˵��Ŀ��ͨ��', CAST(0x0000A8930171741F AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (2, N'ϵͳ����', N'XiTongPeiZhi', N'/AppSettingManage/AppSettingList', N'ϵͳ����', CAST(0x0000A89301721895 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (3, N'�������', N'ChengXuGuanLi', N'/AppSettingManage/AppDataDialog', N'��������ά��ͨ��', CAST(0x0000A89301726118 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (4, N'��Ƨ��ά��', N'ShengPiZiWeiHu', N'/SpellName/SpecialSpellNameDialog', N'¼����Ƨ�ֵ�ͨ��', CAST(0x0000A8930172E0B7 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (5, N'�ϴ��ļ�', N'ShangChuanWenJian', N'/FileHelper/FileUpload', N'��·�������ļ������ϴ�����', CAST(0x0000A894016CF0E1 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (6, N'������ϵ��', N'XinZengLianXiRen', N'/ContactManage/AddContacterDialog', N'������ϵ����ϵ��Ϣ�Ŀ��ͨ��', CAST(0x0000A89D01804199 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (7, N'�˵��б�', N'CaiDanLieBiao', N'/Menu/MenuList', NULL, CAST(0x0000A8A001178699 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (8, N'qqͼƬ���ദ��', N'UinImageGroupService', N'/ContactManage/UinImageGroup', N'�ɼ���qq�˺ŷ������', CAST(0x0000A8A4017460C1 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (9, N'����ά������', N'GoodsManage', N'/OrderManage/GoodsManage', N'��Ʒ�����Ϣ����', CAST(0x0000A8B80107F1B9 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (10, N'�����ʼ�', N'SendEmailDialog', N'/EmailData/SendEmailDialog', N'�򿪸öԻ���ѡ���͵��ʼ���Ϣ���ʼ������ˣ��ʼ����ݣ��ʼ���ʱ����ʱ��', CAST(0x0000A8E10188CFC3 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (11, N'��ҵ�б�', N'CompanyList', N'/OrganizationManage/OrganzeListManage', N'��˾����֯�ܹ������б������漰��ϵͳ��������Ⱥ�塿', CAST(0x0000A8E2012B3CA0 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (12, N'�˺��б�', N'AccountList', N'/AppAccountManage/AccountList', N'ϵͳ�˺��б�', CAST(0x0000A8E2012E0578 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (13, N'ö�ټ���', N'EnumList', N'/EnumData/EnumList', N'�����߼���ʹ�õ�ö��', CAST(0x0000A8E8009AE97E AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (14, N'��־���', N'LogDataOutPut', N'/LogData/LogDataList', N'�������е���־���', CAST(0x0000A8E8009B1DF6 AS DateTime), -1, 3)
INSERT [dbo].[Menu] ([Id], [Name], [Code], [Url], [Remark], [CreateTime], [ParentId], [MenuType]) VALUES (15, N'�˵�ά��', N'MenuDataService', N'/Menu/MenuList', N'����ϵͳ�˵�ά������', CAST(0x0000A8FA0183771A AS DateTime), 0, 0)
SET IDENTITY_INSERT [dbo].[Menu] OFF