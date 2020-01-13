IF(OBJECTPROPERTY(object_id('[internal].[Message]'), N'IsTable') IS NULL)
BEGIN
	CREATE TABLE [internal].[Message]
	(
		[Id] INT IDENTITY (1, 1) NOT NULL
		,[Identifier] VARCHAR(32)
		,[EndPoint] VARCHAR(100)
	)

	INSERT INTO internal.ChangeTracker (ChangeDescription) VALUES ('Created table: internal.Message')
END
GO