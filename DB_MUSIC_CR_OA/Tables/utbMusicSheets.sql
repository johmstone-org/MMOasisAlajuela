﻿CREATE TABLE [usr].[utbMusicSheets]
(
	[MSID]			 INT			IDENTITY (1,1) NOT NULL,
	[MSTypeID]		 INT			NOT NULL,
	[SongID]		 INT			NOT NULL,
	[Version]		 VARCHAR(MAX)   NULL,
	[InstrumentID]	 INT			NOT NULL,
	[Tonality]		 VARCHAR(10)	NOT NULL,
	[FileName]		 VARCHAR(100)	NOT NULL,
	[FileData]		 VARBINARY(MAX)	NOT NULL,
    [ActiveFlag]     BIT			CONSTRAINT [utbMusicSheetsDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME		CONSTRAINT [utbMusicSheetsDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100)	CONSTRAINT [utbMusicSheetsDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME		CONSTRAINT [utbMusicSheetsDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100)	CONSTRAINT [utbMusicSheetsDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbMSID] PRIMARY KEY CLUSTERED ([MSID] ASC),
	CONSTRAINT [fk.usr.utbMusicSheetTypes.usr.utbMusicSheets.MSTypeID] FOREIGN KEY ([MSTypeID]) REFERENCES [usr].[utbMusicSheetTypes] ([MSTypeID]),
	CONSTRAINT [fk.usr.utbSongs.usr.utbMusicSheets.SongID] FOREIGN KEY ([SongID]) REFERENCES [usr].[utbSongs] ([SongID]),
	CONSTRAINT [fk.usr.utbInstruments.usr.utbMusicSheets.InstrumentID] FOREIGN KEY ([InstrumentID]) REFERENCES [usr].[utbInstruments] ([InstrumentID])
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];


GO
CREATE TRIGGER [usr].[utrLogMusicSheets] ON [usr].[utbMusicSheets]
FOR INSERT,UPDATE
AS
	BEGIN
		DECLARE @INSERTUPDATE VARCHAR(30)
		DECLARE @StartValues	XML = (SELECT [MSID],[MSTypeID],[SongID],[Version],[InstrumentID],[Tonality],[FileName],[ActiveFlag],[InsertDate],[InsertUser],[LastModifyDate],[LastModifyUser] FROM Deleted [Values] for xml AUTO, ELEMENTS XSINIL)
		DECLARE @EndValues		XML = (SELECT [MSID],[MSTypeID],[SongID],[Version],[InstrumentID],[Tonality],[FileName],[ActiveFlag],[InsertDate],[InsertUser],[LastModifyDate],[LastModifyUser] FROM Inserted [Values] for xml AUTO, ELEMENTS XSINIL)

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
				,'[usr].[utbMusicSheets]'
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
@value = N'Esta tabla contiene todas las partituras, cifrados, etc.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'ID del documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'MSID';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'ID de la cancion a la que pertenece el documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'SongID';			 


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Version en la cual se baso la partitura o el documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'Version';	


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'ID del instrumento a la que pertenece el documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'InstrumentID';	


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Tonalidad a la que esta el documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'Tonality';	


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Nombre del documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'FileName';	


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Metadata del documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'FileData';	

GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Este flag indica si el tipo de documento esta activo o no.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'ActiveFlag';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Fecha de inserción del documento.', 
@level0type = N'SCHEMA', @level0name = N'usr',
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'InsertDate';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description',
@value = N'Usuario que inserto el documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'InsertUser';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Fecha de la ultima modificación realizada sobre el documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'LastModifyDate';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Nombre del usuario que realizo la ultima modificación sobre el documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheets', 
@level2type = N'COLUMN', @level2name = N'LastModifyUser';

