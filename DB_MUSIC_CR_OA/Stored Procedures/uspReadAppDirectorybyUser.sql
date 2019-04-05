-- ========================================================================================
-- Name:  [adm].[uspReadAppDirectorybyUser]
-- Desc:  Despliega el menu principal que va a tener el usuario
-- Auth:  Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date:  03/29/2019
------------------------------------------------
-- Change History
------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		----------------------------------------------
-- ========================================================================================
CREATE PROCEDURE [adm].[uspReadAppDirectorybyUser]
		@MainAppName	VARCHAR(50),
		@UserName		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON
    BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
			-- ========================================================================================
				SELECT	RAR.[RARProfileID]
						,AD.[MainClass]
						,AD.[AppName]
						,AD.[Controller]
						,AD.[ViewPage]
						,AD.[Parameter]
				FROM	[adm].[utbAppDirectory] AD
						INNER JOIN [adm].[utbRolesApplicationRights] RAR ON RAR.[ApplicationID] = AD.[ApplicationID] AND RAR.[ActiveFlag] = 1
						INNER JOIN [adm].[utbRoles] R ON R.[RoleID] = RAR.[RoleID] AND R.[ActiveFlag] = 1
						INNER JOIN [adm].[utbUsers] U ON U.[RoleID] = R.[RoleID] AND U.[ActiveFlag] = 1
				WHERE	AD.[ActiveFlag] = 1
						AND U.[UserName] = @UserName
						AND AD.[MainAppName] = @MainAppName
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
