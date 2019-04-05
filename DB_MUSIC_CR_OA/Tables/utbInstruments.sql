CREATE TABLE [usr].[utbInstruments] (
    [InstrumentID]   INT           IDENTITY (1, 1) NOT NULL,
    [Instrument]     VARCHAR (100) NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbInstrumentsDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [CreationDate]   DATETIME      CONSTRAINT [utbInstrumentsDefaultCreationDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser]   VARCHAR (100) CONSTRAINT [utbInstrumentsDefaultCreationUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbInstrumentsDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbInstrumentsDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbInstrumentID] PRIMARY KEY CLUSTERED ([InstrumentID] ASC)
);


GO
CREATE TRIGGER [usr].[utrLogInstruments] ON [usr].[utbInstruments]
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
				,'[usr].[utbInstruments]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,[Role] = CASE WHEN R.[RoleName] IS NOT NULL THEN R.[RoleName] ELSE SYSTEM_USER END
				,GETDATE()
		FROM Inserted I
		OUTER APPLY (SELECT RR.[RoleName]
					 FROM	[adm].[utbUsers] U
							LEFT JOIN [adm].[utbRoles] RR ON RR.[RoleID] = U.[RoleID]
					 WHERE	U.[UserName] = SUBSTRING(I.[LastModifyUser],CHARINDEX('\',I.[LastModifyUser])+1,LEN(I.[LastModifyUser]))) R
	END;


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Esta tabla contiene toda la información de los instrumentos disponibles.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbInstruments';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'ID del instrumento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbInstruments', 
@level2type = N'COLUMN', @level2name = N'InstrumentID';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Nombre del instrumento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbInstruments', 
@level2type = N'COLUMN', @level2name = N'Instrument';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Este flag indica si el usuario esta activo o no.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbInstruments', 
@level2type = N'COLUMN', @level2name = N'ActiveFlag';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Fecha de creacion el instrumento.', 
@level0type = N'SCHEMA', @level0name = N'usr',
@level1type = N'TABLE', @level1name = N'utbInstruments', 
@level2type = N'COLUMN', @level2name = N'CreationDate';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description',
@value = N'Usuario que creo el instrument.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbInstruments', 
@level2type = N'COLUMN', @level2name = N'CreationUser';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Fecha de la ultima modificación realizada sobre el instrumento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbInstruments', 
@level2type = N'COLUMN', @level2name = N'LastModifyDate';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Nombre del usuario que realizo la ultima modificación sobre el instrumento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbInstruments', 
@level2type = N'COLUMN', @level2name = N'LastModifyUser';

