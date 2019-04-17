-- ======================================================================
-- Name: [usr].[uspReadMusicSheets]
-- Desc: Muestra las partituras que existen
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 4/14/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [usr].[uspReadMusicSheets]
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[MSID]
						,[MSTypeID]
						,[SongID]
						,[Version] = CASE WHEN [Version] IS NULL THEN 'NV' ELSE '"' + [Version] + '"' END
						,[InstrumentID]
						,[Tonality]
						,[FileName]
						,[FileData]
						,[ActiveFlag]
				FROM	[usr].[utbMusicSheets]
				WHERE	[ActiveFlag] = 1
				ORDER BY [SongID],[InstrumentID]
			-- =======================================================

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
