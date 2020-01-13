IF(OBJECTPROPERTY(object_id('[internal].[ChangeTracker]'), N'IsTable') IS NULL)
BEGIN
       CREATE TABLE [internal].[ChangeTracker]
       (
              [Id] INT IDENTITY (1, 1) NOT NULL
              ,[ChangeDescription] VARCHAR(1000) NOT NULL
              ,[CreatedDateTime] DATETIME NOT NULL DEFAULT (sysdatetime())
              ,[CreatedBy] VARCHAR(50) NOT NULL DEFAULT (suser_name())
       ) 

       INSERT INTO internal.ChangeTracker (ChangeDescription) VALUES ('Created table: internal.ChangeTracker')
END
GO