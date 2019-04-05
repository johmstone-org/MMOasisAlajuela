CREATE TABLE [adm].[utbRolesApplicationRights] (
    [RARProfileID]   INT           IDENTITY (1,1) NOT NULL,
    [RoleID]         INT           NOT NULL,
    [ApplicationID]  INT           NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbRolesApplicationRightsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]	 DATETIME      CONSTRAINT [utbRolesApplicationRightsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbRolesApplicationRightsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbRolesApplicationRightsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbRolesApplicationRightsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbRARProfileID] PRIMARY KEY CLUSTERED ([RARProfileID] ASC),
    CONSTRAINT [fk.adm.utbRolesApplicationRights.adm.utbAppDirectory.ApplicationID] FOREIGN KEY ([ApplicationID]) REFERENCES [adm].[utbAppDirectory] ([ApplicationID]),
    CONSTRAINT [fk.adm.utbRolesApplicationRights.adm.utbRoles.RoleID] FOREIGN KEY ([RoleID]) REFERENCES [adm].[utbRoles] ([RoleID])
);


GO
CREATE TRIGGER [adm].[utrLogRolesApplicationRights] ON [adm].[utbRolesApplicationRights]
FOR INSERT,UPDATE
AS
	BEGIN
		DECLARE @INSERTUPDATE VARCHAR(30)
		DECLARE @StartValues	XML = (SELECT * FROM Deleted [Values] for xml AUTO, ELEMENTS XSINIL)
		DECLARE @EndValues		XML = (SELECT * FROM Inserted [Values] for xml AUTO, ELEMENTS XSINIL)

		CREATE TABLE #DBCC (EventType varchar(50), Parameters varchar(50), EventInfo nvarchar(max))

		INSERT INTO #DBCC
		EXEC ('DBCC INPUTBUFFER(@@SPID)')

		--Assume it is an insert
		SET @INSERTUPDATE ='INSERT'
		--If there's data in deleted, it's an update
		IF EXISTS(SELECT * FROM Deleted)
		  SET @INSERTUPDATE='UPDATE'

		INSERT INTO [adm].[utbLogActivities] ([ActivityType],[TargetTable],[SQLStatement],[StartValues],[EndValues],[User],[RoleAtTime],[LogActivityDate])
		SELECT	@INSERTUPDATE
				,'[adm].[utbRolesApplicationRights]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,[Role] = CASE WHEN R.[RoleName] IS NOT NULL THEN R.[RoleName] ELSE SYSTEM_USER END
				,GETDATE()
		FROM Inserted I
		OUTER APPLY (SELECT RR.[RoleName]
					 FROM	[adm].[utbUsers] U
							LEFT JOIN [adm].utbRoles RR ON RR.[RoleID] = U.RoleID
					 WHERE	U.[UserName] = SUBSTRING(I.[LastModifyUser],CHARINDEX('\',I.[LastModifyUser])+1,LEN(I.[LastModifyUser]))) R
	END


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Esta tabla contiene metadata de los profiles de cada rol dentro de la aplicación.', 
@level0type = N'SCHEMA', @level0name = N'adm', 
@level1type = N'TABLE', @level1name = N'utbRolesApplicationRights';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'ID de identidad.', 
@level0type = N'SCHEMA', @level0name = N'adm', 
@level1type = N'TABLE', @level1name = N'utbRolesApplicationRights', 
@level2type = N'COLUMN', @level2name = N'RARProfileID';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'ID de identidad del rol.', 
@level0type = N'SCHEMA', @level0name = N'adm', 
@level1type = N'TABLE', @level1name = N'utbRolesApplicationRights', 
@level2type = N'COLUMN', @level2name = N'RoleID';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'ID de identidad de la pagina.', 
@level0type = N'SCHEMA', 
@level0name = N'adm', 
@level1type = N'TABLE', @level1name = N'utbRolesApplicationRights', 
@level2type = N'COLUMN', @level2name = N'ApplicationID';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Este flag es usado para identificar si la pagina esta active o no.', 
@level0type = N'SCHEMA', @level0name = N'adm', 
@level1type = N'TABLE', @level1name = N'utbRolesApplicationRights', 
@level2type = N'COLUMN', @level2name = N'ActiveFlag';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Fecha de inserción del registro.', 
@level0type = N'SCHEMA', @level0name = N'adm', 
@level1type = N'TABLE', @level1name = N'utbRolesApplicationRights', 
@level2type = N'COLUMN', @level2name = N'InsertDate';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Usuario que inserto el registro.', 
@level0type = N'SCHEMA', @level0name = N'adm', 
@level1type = N'TABLE', @level1name = N'utbRolesApplicationRights', 
@level2type = N'COLUMN', @level2name = N'InsertUser';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Fecha de la última modificación sobre el registro.', 
@level0type = N'SCHEMA', @level0name = N'adm',
@level1type = N'TABLE', @level1name = N'utbRolesApplicationRights', 
@level2type = N'COLUMN', @level2name = N'LastModifyDate';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Usuario que realizo la última modificación del registro.', 
@level0type = N'SCHEMA', @level0name = N'adm', 
@level1type = N'TABLE', @level1name = N'utbRolesApplicationRights', 
@level2type = N'COLUMN', @level2name = N'LastModifyUser';

