CREATE PROCEDURE [dbo].[DeleteMapGame]
@ID_GAME INT

AS
    DELETE 
	FROM MAP_GAMES
	WHERE 
	ID_GAME = @ID_GAME 
GO