USE [VehiclesMonitoring]
GO

INSERT INTO [dbo].[Customer]([Name],[Address]) VALUES ('Kalles Grustransporter AB' ,'Cementvägen 8, 111 11 Södertälje')
INSERT INTO [dbo].[Customer]([Name],[Address]) VALUES ('Johans Bulk AB' ,'Balkvägen 12, 222 22 Stockholm')
INSERT INTO [dbo].[Customer]([Name],[Address]) VALUES ('Haralds Värdetransporter AB' ,'Budgetvägen 1, 333 33 Uppsala')

GO

INSERT INTO [dbo].[Vehicle]([VIN],[RegNr] ,[CustomerId] )  VALUES    ('YS2R4X20005399401'  ,'ABC123'  ,1   )
INSERT INTO [dbo].[Vehicle]([VIN],[RegNr] ,[CustomerId] )  VALUES    ('VLUR4X20009093588'  ,'DEF456'  ,1   )
INSERT INTO [dbo].[Vehicle]([VIN],[RegNr] ,[CustomerId] )  VALUES    ('VLUR4X20009048066'  ,'GHI789'  ,1   )


INSERT INTO [dbo].[Vehicle]([VIN],[RegNr] ,[CustomerId] )  VALUES    ('YS2R4X20005388011'  ,'JKL012'  ,2   )
INSERT INTO [dbo].[Vehicle]([VIN],[RegNr] ,[CustomerId] )  VALUES    ('YS2R4X20005387949'  ,'MNO345'  ,2   )


INSERT INTO [dbo].[Vehicle]([VIN],[RegNr] ,[CustomerId] )  VALUES    ('YS2R4X20005387765'  ,'PQR678'  ,3   )
INSERT INTO [dbo].[Vehicle]([VIN],[RegNr] ,[CustomerId] )  VALUES    ('YS2R4X20005387055'  ,'STU901'  ,3   )


GO






