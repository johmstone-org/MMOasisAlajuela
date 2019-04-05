-- ======================================================================
-- Name: [adm].[uspLogin]
-- Desc: Se utiliza para la validación de los usuarios al logearse en la applicación
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 3/26/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspLogin]
	@UserName	VARCHAR(50),
	@Password	VARCHAR(50)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				DECLARE @RoleID		INT
				DECLARE @AuthFlag	BIT

				IF EXISTS (SELECT TOP 1 [UserID] FROM [adm].[utbUsers] WHERE [UserName] = @UserName)
					BEGIN
						SELECT	@RoleID = [RoleID],
								@AuthFlag = [AuthorizationFlag]
						FROM	[adm].[utbUsers] 
						WHERE	[UserName] = @UserName
								AND [PasswordHash] = HASHBYTES('SHA2_512',@Password)
								AND [ActiveFlag] = 1
					
						IF (@RoleID IS NULL)
							BEGIN
								IF ((CASE WHEN @AuthFlag = 1 OR @AuthFlag IS NULL THEN 1 ELSE 0 END) = 1)
									BEGIN
										SET @RoleID  = -1  -- Password Incorrecto
										
									END
								ELSE
									BEGIN
										SET	@RoleID  = -2  -- Usuario no autorizado aún
									END
							END							
						SELECT @RoleID 
					END
				ELSE
					BEGIN
						SELECT (0)  -- Usuario no registrado
					END

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
