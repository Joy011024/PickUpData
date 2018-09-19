 -- ip proxy
CREATE TABLE Proxy (
    Id VARCHAR (32) PRIMARY KEY,
    IP varchar(15) not null,
    Port int not null,
    CreateTime date,
    Desc varchar(1024),
    Status int default(0)
);
Create table RunProxy
(
    ProxyId varchar(32),
    Id varchar(32) primary key,
    Result bit not null,
    CreateTime date not null
);