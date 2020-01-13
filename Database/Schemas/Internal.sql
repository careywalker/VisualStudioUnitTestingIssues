IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = N'internal')
BEGIN
	EXEC('CREATE SCEHMA [internal] AUTHORIZATION [dbo]');
END
GO