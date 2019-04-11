﻿-- ======================================================================
-- Name: [adm].[uspSearchRole]
-- Desc: Permite mostrar la informacion de un rol en especifico
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 4/10/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspSearchRole]
		@RoleID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[RoleID]
						,[RoleName]
				FROM	[adm].[utbRoles]
				WHERE	[RoleID] = @RoleID
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
