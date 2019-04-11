-- ======================================================================
-- Name: [adm].[uspReadUsers]
-- Desc: Muestra la informacion de todos los usuarios existentes
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 4/10/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspReadUsers]
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[UserID]
						,[FullName]
						,[UserName]
						,[Email]
						,[RoleID]
						,[ActiveFlag]
						,[AuthorizationFlag]  
				FROM	[adm].[utbUsers]
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
