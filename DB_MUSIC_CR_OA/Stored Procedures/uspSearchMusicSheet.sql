﻿-- ======================================================================
-- Name: [usr].[uspSearchMusicSheet]
-- Desc: Permite mostrar la informacion de una partitura en especifico
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 4/16/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [usr].[uspSearchMusicSheet]
		@MSID	INT,
		@User	VARCHAR(50)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				DECLARE	@UserID	INT	=	(SELECT [UserID]
										 FROM	[adm].[utbUsers]
										 WHERE	[UserName] = @User)

				SELECT	MS.[MSID]
						,MS.[MSTypeID]
						,MS.[SongID]
						,[Version]			=	CASE WHEN MS.[Version] IS NULL THEN 'NV' ELSE '"' + MS.[Version] + '"' END
						,MS.[InstrumentID]
						,MS.[Tonality]
						,MS.[ActiveFlag]
						,[Favorite]			=	CASE WHEN F.[MSFavoriteID] IS NOT NULL THEN 1 ELSE 0 END
				FROM	[usr].[utbMusicSheets] MS
						LEFT JOIN [usr].[utbMSFavoritesbyUser] F ON F.[MSID] = MS.[MSID] 
																	AND F.[UserID] = @UserID 
																	AND F.[ActiveFlag] = 1
				WHERE	MS.[MSID] = @MSID
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