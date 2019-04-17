﻿-- ======================================================================
-- Name: [usr].[uspReadMusicSheetsbySong]
-- Desc: Muestra las partituras de una cancion en especifico
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 4/14/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [usr].[uspReadMusicSheetsbySong]
	@SongID	INT
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
				WHERE	[SongID] = @SongID
						AND [ActiveFlag] = 1
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