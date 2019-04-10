-- ========================================================================================
-- Name:  [adm].[uspReadLabelMainMenu]
-- Desc:  Despliega el subtitulo que contiene de cada bloque en el menu principal
-- Auth:  Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date:  04/06/2019
------------------------------------------------
-- Change History
------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		----------------------------------------------
-- ========================================================================================
CREATE PROCEDURE [adm].[uspReadLabelMainMenu]
		@MainAppName	VARCHAR(50),
		@MainClass		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON
    BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
			-- ========================================================================================
				SELECT	STRING_AGG(ISNULL(AppName, ' '), ' - ') AS Label 
				FROM	[adm].[utbAppDirectory]
				WHERE 	[MainAppName] = @MainAppName
						AND [MainClass] = @MainClass
			-- ========================================================================================
	END TRY
        BEGIN CATCH

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
END
    SET NOCOUNT OFF
GO
