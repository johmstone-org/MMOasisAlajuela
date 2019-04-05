﻿-- ======================================================================
-- Name: [usr].[uspValidateGUIDResetPassword]
-- Desc: Valida si el GUID de autorización esta activo
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 3/27/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [usr].[uspValidateGUIDResetPassword]
	@GUID VARCHAR(MAX)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[ActiveFlag]
				FROM	[usr].[utbResetPasswords]
				WHERE	[GUID] = @GUID
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
