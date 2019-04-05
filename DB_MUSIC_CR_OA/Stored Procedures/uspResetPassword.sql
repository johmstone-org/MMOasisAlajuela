﻿-- ======================================================================
-- Name: [usr].[uspResetPassword]
-- Desc: Se utiliza para restablecer contraseñas
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 3/27/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [usr].[uspResetPassword]
	@GUID		VARCHAR(MAX),
	@Password	VARCHAR(50)
AS 
    BEGIN
        SET NOCOUNT ON
        SET XACT_ABORT ON
                           
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
            DECLARE @lLocalTran BIT = 0
                               
            IF @@TRANCOUNT = 0 
                BEGIN
                    BEGIN TRANSACTION
                    SET @lLocalTran = 1
                END

            -- =======================================================
				DECLARE @UserID		INT
				DECLARE	@UserName	VARCHAR(50)
				
				SELECT	@UserID		=	RR.[UserID],
						@UserName	=	U.UserName						
				FROM	[usr].[utbResetPasswords]  RR
						LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = RR.UserID
				WHERE	RR.[GUID] = @GUID
				
				UPDATE	[adm].[utbUsers]
				SET		[PasswordHash]		=	HASHBYTES('SHA2_512',@Password),
						[LastModifyDate]	=	GETDATE(),
						[LastModifyUser]	=	@UserName
				WHERE	[UserID] = @UserID

				UPDATE 	[usr].[utbResetPasswords]
				SET		[ActiveFlag]	=	0,
						[LastModifyDate]	=	GETDATE(),
						[LastModifyUser]	=	@UserName
				WHERE	[GUID] = @GUID
			-- =======================================================

        IF ( @@trancount > 0
                 AND @lLocalTran = 1
               ) 
                BEGIN
                    COMMIT TRANSACTION
                END
        END TRY
        BEGIN CATCH
            IF ( @@trancount > 0
                 AND XACT_STATE() <> 0
               ) 
                BEGIN
                    ROLLBACK TRANSACTION
                END

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END

    SET NOCOUNT OFF
    SET XACT_ABORT OFF
GO
