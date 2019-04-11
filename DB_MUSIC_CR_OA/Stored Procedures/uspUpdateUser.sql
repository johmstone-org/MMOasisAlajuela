-- ======================================================================
-- Name: [adm].[uspUpdateUser]
-- Desc: Se utiliza para actualizar la información de un usuario
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 4/11/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateUser]
	@ActionType		VARCHAR(6),
	@InsertUser		VARCHAR(50),
	@UserID			INT,
	@FullName		VARCHAR(50)	=	NULL,
	@RoleID			INT			=	NULL,
	@ActiveFlag		BIT			=	NULL
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
				IF(@ActionType = 'GUDP')		-- GUDP = General Update
					BEGIN
						UPDATE	[adm].[utbUsers]
						SET		[FullName]		=	@FullName,
								[RoleID]		=	@RoleID,
								[ActiveFlag]	=	@ActiveFlag,
								[LastModifyUser]=	@InsertUser,
								[LastModifyDate]=	GETDATE()
						WHERE	[UserID]	=	@UserID
					END
				ELSE
					BEGIN
						IF(@ActionType = 'MS')	-- MS = Modify Status - ActiveFlag
							BEGIN
								UPDATE	[adm].[utbUsers]
								SET		[ActiveFlag]	=	@ActiveFlag,
										[LastModifyUser]=	@InsertUser,
										[LastModifyDate]=	GETDATE()
								WHERE	[UserID]	=	@UserID
							END
						ELSE					-- Authorizations
							BEGIN
								UPDATE	[adm].[utbUsers]
								SET		[RoleID]			=	@RoleID,
										[AuthorizationFlag]	=	1,
										[LastModifyUser]	=	@InsertUser,
										[LastModifyDate]	=	GETDATE()
								WHERE	[UserID]	=	@UserID
							END
					END
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
