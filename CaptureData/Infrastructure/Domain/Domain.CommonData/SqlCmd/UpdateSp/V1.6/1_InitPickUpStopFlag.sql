INSERT INTO [TecentDataUinDA].[dbo].[PickUpStopFlag]
           ([Id]
           ,[CreateTime]
           ,[LastPickUptime]
           ,[PickUpFromPc]
           ,[DataSaveDB]
           ,[DataSaveTable]
           ,[PickUpNum])
     VALUES
           (NEWID()
           ,GETDATE()
           ,( SELECT  MAX(CreateTime) FROM [TecentDataUinDA].[dbo].[TecentQQData] )
           ,'192.168.0.159'
           ,'TecentDataUinDA'
           ,'TecentQQData'
           ,(  SELECT  count(uin) FROM [TecentDataUinDA].[dbo].[TecentQQData] )
     )
GO


