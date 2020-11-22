CREATE PROCEDURE [dbo].[InsertSaveGame]
    @ID_GAME int out,
    @DATE datetime,
    @COMMENT VARCHAR(50)

AS
    INSERT INTO SAVE_GAMES (Date, Comment)
    VALUES (@date, @comment)   
    SET @ID_GAME=SCOPE_IDENTITY()
	SELECT SCOPE_IDENTITY()
GO


