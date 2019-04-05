-- ========================================================================================
-- Name:  [adm].[uspReadProfilebyRole]
-- Desc:  Despliega las vistas a las que esta autorizado o no a ver dependiendo del rol.
-- Auth:  Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date:  03/28/2019
------------------------------------------------
-- Change History
------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		----------------------------------------------
-- ========================================================================================
CREATE PROCEDURE [adm].[uspReadProfilebyRole]
		@MainAppName	VARCHAR(50),
		@RoleID			INT
AS
BEGIN
	SET NOCOUNT ON
    BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
			-- ========================================================================================
				SELECT	AD.[ApplicationID]
						,AD.[MainClass]
						,AD.[AppName]
						,[ProfileID]	=	ISNULL(P.[RARProfileID],0)
						,[Status]		=	ISNULL(P.[ActiveFlag],0)
				FROM	[adm].[utbAppDirectory] AD
						OUTER APPLY	(SELECT	RAR.[RARProfileID]
											,RAR.[ActiveFlag]
										FROM	[adm].[utbRolesApplicationRights] RAR
										WHERE	RAR.[RoleID] = @RoleID
											AND RAR.[ApplicationID] = AD.[ApplicationID]) P

				WHERE	AD.[ActiveFlag] = 1
						AND AD.MainAppName = @MainAppName
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
