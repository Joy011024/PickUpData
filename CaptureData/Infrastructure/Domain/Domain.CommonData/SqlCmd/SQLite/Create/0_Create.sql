create table CategoryData
(
	[ID] [int] NOT NULL primary key,
	[Name] [nvarchar](20) NULL,
	[ParentID] [int] NULL,
	[ParentCode] [varchar](10) NULL,
	[Code] [varchar](10) NULL,
	[Sort] [int] NULL,
	[ItemType] [varchar](20) NULL,
	[IsDelete] [bit] NULL,
	[CreateTime] [datetime] NULL,
	[NodeLevel] [int] NULL
);
CREATE TABLE TecentQQData (
    ID                   VARCHAR (36)  PRIMARY KEY,
    PickUpWhereId        VARCHAR (36),
    age                  INT,
    city                 VARCHAR (50),
    country              VARCHAR (50),
    distance             INT,
    face                 INT,
    gender               char(3) not null,
    nick                 VARCHAR (50),
    province             VARCHAR (50),
    stat                 INT,
    uin                  VARCHAR (15),
    HeadImageUrl         VARCHAR (500),
    CreateTime           DATETIME,
    IsGatherImage        BLOB          DEFAULT (0),
    GatherImageTime      DATETIME,
    LastErrorGatherImage DATETIME,
    GatherImageErrorNum  INT           DEFAULT (0),
    ImageRelativePath    VARCHAR (200),
    DayInt               INT           DEFAULT (0),
    ImgType              INT           DEFAULT (0) 
);
CREATE TABLE AppSettingData (
    Id         INT            PRIMARY KEY,
    Name       VARCHAR (32)   NOT NULL,
    Value      VARCHAR (128)  NOT NULL,
    CreateTime DATETIME       NOT NULL,
    Statues    INT            NOT NULL,
    [Desc]     VARCHAR (1024) 
);
CREATE TABLE IdCard (
    Id         VARCHAR (18)   PRIMARY KEY,
    Name       VARCHAR (32)   NOT NULL,
    CreateTime DATE           NOT NULL,
    [Desc]     VARCHAR (1024) 
);
CREATE TABLE Names (
    CreateTime DATE         NOT NULL,
    Name       VARCHAR (32) NOT NULL,
    InDbDay    INT          NOT NULL
);
CREATE TABLE ProxyIP (
    Id         VARCHAR (32)   PRIMARY KEY,
    IP         VARCHAR (15)   NOT NULL,
    Port       INT            NOT NULL,
    CreateTime DATE,
	DownloadTime  DATE,
    [Desc]     VARCHAR (1024),
	Cryptonym  VARCHAR (32)  ,
	IPHttpType VARCHAR (32)  ,
	IPAddress VARCHAR (32)  ,
	IPResponseSpleed  VARCHAR (32)  ,
	IPPoolUpdateTimeDesc VARCHAR (32)  ,
    Status     INT            DEFAULT (0) 
);
CREATE TABLE RunProxy (
    ProxyId    VARCHAR (32),
    Id         VARCHAR (32) PRIMARY KEY,
    Result     BIT          NOT NULL,
    CreateTime DATE         NOT NULL
);
CREATE TABLE CategoryData (
    Id         INT            PRIMARY KEY,
    ParentId  int,
    ItemType varchar(128),
    IsDelete bit not null,
    CreateTime DATETIME       NOT NULL,
    Sort       int not null,
    ParentCode varchar(128)  ,
    NodeLevel  int not null,
    Name       VARCHAR (32)   NOT NULL,
    Code  varchar(32) not null
);


go

select 
( SELECT  count(Id)   FROM TecentQQData)  as DBTotal,
(select count( distinct(uin))    FROM TecentQQData) as DBPrimaryTotal,
(select count(Id)   from TecentQQData where dayint=cast( strftime('%Y%m%d', 'now')  as int)) as IdTotal ,
(select count(distinct(uin)) from TecentQQData where dayint=cast( strftime('%Y%m%d', 'now')  as int))  as Total,
(select cast( strftime('%Y%m%d', 'now')  as int) ) as StaticDay
 