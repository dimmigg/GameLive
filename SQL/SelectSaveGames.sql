CREATE PROCEDURE [dbo].[SelectSaveGames]
    @ID_GAME int out

AS
	SELECT
		X,
		Y,
		ALIVE
	FROM
		MAP_GAMES
	WHERE
	ID_GAME = @ID_GAME
GO

