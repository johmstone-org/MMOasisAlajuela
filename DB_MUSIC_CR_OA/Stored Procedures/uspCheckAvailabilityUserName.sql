﻿-- ======================================================================
-- Name: [usr].[uspCheckAvailabilityUserName]
-- Desc: Es para validar si un nombre de usuario ya existe en la base de datos
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 3/26/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [usr].[uspCheckAvailabilityUserName]
	@UserName VARCHAR(50)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	 [UserID]
						,[FullName]
						,[UserName]
						,[Email]
				FROM	[adm].[utbUsers]
				WHERE	[UserName] = @UserName
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
