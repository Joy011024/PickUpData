Drop table ProxyIP;
CREATE TABLE ProxyIP (
    Id         bigint   PRIMARY KEY,
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