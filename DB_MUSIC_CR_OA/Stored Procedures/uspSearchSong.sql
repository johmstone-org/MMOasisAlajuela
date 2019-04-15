﻿-- ======================================================================
-- Name: [usr].[uspSearchSong]
-- Desc: Permite mostrar la informacion de una canción en especifico
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 4/15/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [usr].[uspSearchSong]
		@SongID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[SongID]
						,[SongName]
						,[AuthorID]
				FROM	[usr].[utbSongs]
				WHERE	[SongID] = @SongID
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
