CREATE TABLE [usr].[utbSongs]
(
	[SongID]		 INT           IDENTITY (1, 1) NOT NULL,
    [SongName]		 VARCHAR (100) NOT NULL,
	[AuthorID]		 INT		   NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbSongsDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbSongsDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbSongsDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbSongsDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbSongsDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbSongID] PRIMARY KEY CLUSTERED ([SongID] ASC),
	CONSTRAINT [fk.usr.utbAuthors.usr.utbSongs.AuthorID] FOREIGN KEY ([AuthorID]) REFERENCES [usr].[utbAuthors] ([AuthorID])
);


GO
CREATE TRIGGER [usr].[utrLogSongs] ON [usr].[utbSongs]
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
				,'[usr].[utbSongs]'
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
@value = N'Esta tabla contiene el detalle de las canciones.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbSongs';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'ID del Tipo de documento.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbSongs', 
@level2type = N'COLUMN', @level2name = N'SongID';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Nombre de la cancion.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbSongs', 
@level2type = N'COLUMN', @level2name = N'SongName';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'ID del autor de la cancion.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbSongs', 
@level2type = N'COLUMN', @level2name = N'AuthorID';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Este flag indica si la cancion esta activa o no.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbSongs', 
@level2type = N'COLUMN', @level2name = N'ActiveFlag';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Fecha de inserción de la canción.', 
@level0type = N'SCHEMA', @level0name = N'usr',
@level1type = N'TABLE', @level1name = N'utbSongs', 
@level2type = N'COLUMN', @level2name = N'InsertDate';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description',
@value = N'Usuario que inserto la canción.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbSongs', 
@level2type = N'COLUMN', @level2name = N'InsertUser';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Fecha de la ultima modificación realizada sobre la canción.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbSongs', 
@level2type = N'COLUMN', @level2name = N'LastModifyDate';


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Nombre del usuario que realizo la ultima modificación sobre la canción.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbSongs', 
@level2type = N'COLUMN', @level2name = N'LastModifyUser';

