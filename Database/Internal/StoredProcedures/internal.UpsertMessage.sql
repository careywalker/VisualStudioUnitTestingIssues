CREATE OR ALTER PROCEDURE internal.UpsertMessage
	@Id int = 0,
	@Identifier varchar(32) = null,
	@EndPoint varchar(100) = null
AS
BEGIN
	MERGE internal.Message AS target
	USING (SELECT @Id, @Identifier, @EndPoint) AS source (Id, Identifier, EndPoint)
	ON (target.Id = source.Id)
	WHEN NOT MATCHED THEN
		INSERT 
			(
				Identifier,
				EndPoint
			)
		VALUES
			(
				@Identifier,
				@EndPoint
			);

		SELECT @@IDENTITY AS 'Id';
END
GO

INSERT INTO internal.ChangeTracker (ChangeDescription) VALUES ('CREATE OR ALTER PROCEDURE internal.InsertMessage');
GO