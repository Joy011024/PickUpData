USE [TecentDataDA]
GO

/****** Object:  Table [dbo].[DataBaseNPKVersion]    Script Date: 06/18/2017 20:49:39 ******/
SET ANSI_NULLS ON
GO
if(object_id('DataBaseNPKVersion','u') is not null)
	drop table DataBaseNPKVersion
go
SET QUOTED_IDENTIFIER ON
GO

if(object_id('DataBaseNPKVersion','u') is not null)
	drop procedure DataBaseNPKVersion
go
SET ANSI_PADDING ON
GO
Create table DataBaseNPKVersion
(
	Id int identity(1,1) primary key,
	NPKEffectVersion varchar(20) not null,--�������ð汾
	GenerateNPKVersion varchar(20),--���������ɰ�װ���İ汾�ţ����ܼ�¼ǰ��û��Ϊ�ò������ɰ�װ����
	NPKSvn varchar(10),--�����ύ��¼
	NPKSubmitTime datetime not null,--�޸�����
	NPKPath nvarchar(200),--�������·���������޸����ݽ϶ࣩ
	DBStructCmd nvarchar(500),--�����ݿ�ṹ�����ı�ʱ���������������д洢
	NPKType int ,--�����޸ķ��� 1 ��ṹ�޸� 2 ҵ�����ݱ䶯 3 �洢���̱䶯 4 �����䶯
	Note nvarchar(100),--������ע
	InDBTime datetime default(getdate()),--�������ʱ��
	IsDelete bit ,--�Ƿ�ɾ���ò���
	IsRelease bit ,--�����Ƿ񷢲�
	NPKAuthor nvarchar(20),--���β���������
	NpkSubmiter nvarchar(20),--�����ύ��
	NPKDataBase varchar(30) --���β�����Ե����ݿ�
)