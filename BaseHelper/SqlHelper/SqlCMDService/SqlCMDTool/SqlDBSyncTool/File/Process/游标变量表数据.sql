--����ս����������Զ�������б�
--���û��Զ�����ݿ�����������ʹ�ñ���[������Ҫȷ�����ش��ڻ�������,����ͬ��ʧ��]
print '���û��Զ�����ݿ�����������ʹ�ñ���[������Ҫȷ�����ش��ڻ�������,����ͬ��ʧ��]'



DECLARE @F_ID INT,@F_DBType TINYINT,@F_BigZoneID VARCHAR(5),@F_BattleZoneID VARCHAR(10),@F_DBIP NVARCHAR(30),
@F_DBName NVARCHAR(30),@F_DBUser NVARCHAR(30),@F_DBPSW NVARCHAR(30)
DECLARE SynDBTable_CURSOR CURSOR LOCAL FOR 
	SELECT F_ID,F_DBType,F_BigZoneID,F_BattleZoneID,F_DBIP,F_DBName,F_DBUser,F_DBPSW 
	FROM T_BaseParamDB  WITH(NOLOCK) WHERE F_DBType=5 and F_State=1
OPEN SynDBTable_CURSOR
FETCH NEXT FROM SynDBTable_CURSOR INTO @F_ID,@F_DBType,@F_BigZoneID,@F_BattleZoneID,@F_DBIP,@F_DBName,@F_DBUser,@F_DBPSW
WHILE @@FETCH_STATUS = 0
	begin
		print 'do..'
	
		FETCH NEXT FROM SynDBTable_CURSOR INTO @F_ID,@F_DBType,@F_BigZoneID,@F_BattleZoneID,@F_DBIP,@F_DBName,@F_DBUser,@F_DBPSW
	end
