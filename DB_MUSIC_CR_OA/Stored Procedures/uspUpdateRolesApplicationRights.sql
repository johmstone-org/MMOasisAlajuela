-- ======================================================================
-- Name: [adm].[uspUpdateRolesApplicationRights]
-- Desc: Se utiliza para actualizar derechos de edicion a los diferentes roels
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 3/29/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateRolesApplicationRights]
		@InsertUser		VARCHAR(50),
		@RARProfileID	INT,
		@Status			BIT
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
				UPDATE	[adm].[utbRolesApplicationRights]
				SET		[ActiveFlag]		=	@Status
						,[LastModifyDate]	=	GETDATE()
						,[LastModifyUser]	=	@InsertUser
				WHERE	[RARProfileID]		=	@RARProfileID
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
