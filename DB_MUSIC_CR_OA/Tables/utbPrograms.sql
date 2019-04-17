CREATE TABLE [usr].[utbPrograms]
(
	[ProgramID]			INT				IDENTITY(1,1) NOT NULL,
	[ProgramDate]		DATETIME		NOT NULL,
	[ProgramSchedule]	VARCHAR (20)	NOT NULL,
	[CompleteFlag]		BIT				CONSTRAINT [utbProgramsDefaultCompleteFlagFalse] DEFAULT ((0)) NOT NULL,
	[ActiveFlag]		BIT				CONSTRAINT [utbProgramsDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [CreationDate]		DATETIME		CONSTRAINT [utbProgramsDefaultCreationDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser]		VARCHAR (100)	CONSTRAINT [utbProgramsDefaultCreationUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbProgramsDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbProgramsDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbProgramID] PRIMARY KEY CLUSTERED ([ProgramID] ASC)
);

GO
CREATE TRIGGER [usr].[utrLogPrograms] ON [usr].[utbPrograms]
FOR INSERT,UPDATE
AS
    BEGIN
        DECLARE @INSERTUPDATE VARCHAR(30)
        DECLARE @StartValues    XML = (SELECT * FROM Deleted [Values] FOR XML AUTO, ELEMENTS XSINIL)
        DECLARE @EndValues      XML = (SELECT * FROM Inserted [Values] FOR XML AUTO, ELEMENTS XSINIL)
 
        CREATE TABLE #DBCC (EventType VARCHAR(50), Parameters VARCHAR(50), EventInfo NVARCHAR(MAX))
 
        INSERT INTO #DBCC
        EXEC ('DBCC INPUTBUFFER(@@SPID)')
 
        --Assume it is an insert
        SET @INSERTUPDATE ='INSERT'
        --If there's data in deleted, it's an update
        IF EXISTS(SELECT * FROM Deleted)
          SET @INSERTUPDATE='UPDATE'
 
        INSERT INTO [adm].[utbLogActivities] ([ActivityType],[TargetTable],[SQLStatement],[StartValues],[EndValues],[User],[RoleAtTime],[LogActivityDate])
        SELECT  @INSERTUPDATE
                ,'[usr].[utbPrograms]'
                ,(SELECT EventInfo FROM #DBCC)
                ,@StartValues
                ,@EndValues
                ,[LastModifyUser]
                ,[Role] = CASE WHEN R.[RoleName] IS NOT NULL THEN R.[RoleName] ELSE SYSTEM_USER END
                ,GETDATE()
        FROM Inserted I
        OUTER APPLY (SELECT RR.[RoleName]
                     FROM   [adm].[utbUsers] U
                            LEFT JOIN [adm].utbRoles RR ON RR.[RoleID] = U.RoleID
                     WHERE  U.[UserName] = SUBSTRING(I.[LastModifyUser],CHARINDEX('\',I.[LastModifyUser])+1,LEN(I.[LastModifyUser]))) R
    END
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Esta tabla contiene toda la información referente a los diferentes programas.', @level0type = N'SCHEMA', @level0name = N'usr', @level1type = N'TABLE', @level1name = N'utbPrograms';
 
 
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID de identidad del programa.', @level0type = N'SCHEMA', @level0name = N'usr', @level1type = N'TABLE', @level1name = N'utbPrograms', @level2type = N'COLUMN', @level2name = N'ProgramID';
 
 
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha del programa.', @level0type = N'SCHEMA', @level0name = N'usr', @level1type = N'TABLE', @level1name = N'utbPrograms', @level2type = N'COLUMN', @level2name = N'ProgramDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Horario del programa.', @level0type = N'SCHEMA', @level0name = N'usr', @level1type = N'TABLE', @level1name = N'utbPrograms', @level2type = N'COLUMN', @level2name = N'ProgramSchedule';
 

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Este flag indica si el programa ya fue completado.', @level0type = N'SCHEMA', @level0name = N'usr', @level1type = N'TABLE', @level1name = N'utbPrograms', @level2type = N'COLUMN', @level2name = N'CompleteFlag';
 
 

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Este flag indica si el programa esta activo o no.', @level0type = N'SCHEMA', @level0name = N'usr', @level1type = N'TABLE', @level1name = N'utbPrograms', @level2type = N'COLUMN', @level2name = N'ActiveFlag';
 
 
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creacion del programa.', @level0type = N'SCHEMA', @level0name = N'usr', @level1type = N'TABLE', @level1name = N'utbPrograms', @level2type = N'COLUMN', @level2name = N'CreationDate';
 
 
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Usuario que creo el programa.', @level0type = N'SCHEMA', @level0name = N'usr', @level1type = N'TABLE', @level1name = N'utbPrograms', @level2type = N'COLUMN', @level2name = N'CreationUser';
 
 
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de la ultima modificación realizada sobre el programa.', @level0type = N'SCHEMA', @level0name = N'usr', @level1type = N'TABLE', @level1name = N'utbPrograms', @level2type = N'COLUMN', @level2name = N'LastModifyDate';
 
 
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre del usuario que realizo la ultima modificación sobre el programa.', @level0type = N'SCHEMA', @level0name = N'usr', @level1type = N'TABLE', @level1name = N'utbPrograms', @level2type = N'COLUMN', @level2name = N'LastModifyUser';