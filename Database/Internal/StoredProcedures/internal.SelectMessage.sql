CREATE OR ALTER PROCEDURE internal.SelectMessage
AS
BEGIN
	SELECT 
		Id, 
		Identifier, 
		EndPoint
	FROM internal.Message
	ORDER BY Id
END
GO

INSERT INTO internal.ChangeTracker (ChangeDescription) VALUES ('CREATE OR ALTER PROCEDURE internal.SelectMessage')
GO