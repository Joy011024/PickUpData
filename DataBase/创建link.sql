declare @link varchar(50)
declare @ip varchar(50)
declare @port int 
declare @user varchar(10)
declare @psw varchar(10)

set @link='Link131'
set @ip='192.168.241.131'
set @port=1433
set @user='sa'
set @psw='password'

exec sp_addlinkedserver  @link, ' ',  'SQLOLEDB',  @ip,@port
exec sp_addlinkedsrvlogin   @link, 'false', null, @user,  @psw