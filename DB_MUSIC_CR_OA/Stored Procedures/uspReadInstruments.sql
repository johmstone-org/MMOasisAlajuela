﻿-- ======================================================================
-- Name: [usr].[uspReadInstruments]
-- Desc: Permite mostrar todos los instrumentos
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 4/15/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [usr].[uspReadInstruments]
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[InstrumentID]
						,[Instrument]
				FROM	[usr].[utbInstruments]
				WHERE	[ActiveFlag] = 1
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
