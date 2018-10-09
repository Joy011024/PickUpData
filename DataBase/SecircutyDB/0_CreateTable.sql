create table IdCard
(
    Id varchar(18) primary key,
    Name varchar(32) not null,
    CreateTime date not null,
    Desc varchar(1024)
);
Create table Names
(
    CreateTime date not null,
    Name varchar(32) not null,
    InDbDay int not null
)
);
create table AppSettingData
(
    Id int primary key,
    Name varchar(32) not null,
    Value varchar(128) not null,
    CreateTime datetime not null,
    Statues int not null, 
    Desc varchar(1024)
);
CREATE TABLE IdCard (
    Id         VARCHAR (18)   PRIMARY KEY,
    Name       VARCHAR (32)   NOT NULL,
    CreateTime DATE           NOT NULL,
    [Desc]     VARCHAR (1024) 
);
CREATE TABLE Proxy (
    Id         VARCHAR (32)   PRIMARY KEY,
    IP         VARCHAR (15)   NOT NULL,
    Port       INT            NOT NULL,
    CreateTime DATE,
    [Desc]     VARCHAR (1024),
    Status     INT            DEFAULT (0) 
);
CREATE TABLE RunProxy (
    ProxyId    VARCHAR (32),
    Id         VARCHAR (32) PRIMARY KEY,
    Result     BIT          NOT NULL,
    CreateTime DATE         NOT NULL
);