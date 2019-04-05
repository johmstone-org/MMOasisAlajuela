CREATE TABLE [usr].[utbMusicSheetsHistory]
(
	[MSHistoryID]	 INT			IDENTITY (1,1) NOT NULL,
	[MSID]			 INT			NOT NULL,
	[MSTypeID]		 INT			NOT NULL,
	[SongID]		 INT			NOT NULL,
	[InstrumentID]	 INT			NOT NULL,
	[Tonality]		 VARCHAR(10)	NOT NULL,
	[FileName]		 VARCHAR(100)	NOT NULL,
	[FileData]		 VARBINARY(MAX)	NOT NULL,
    [ActiveFlag]     BIT			NOT NULL,
    [InsertDate]     DATETIME		CONSTRAINT [utbMusicSheetsHistoryDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100)	CONSTRAINT [utbMusicSheetsHistoryDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME		CONSTRAINT [utbMusicSheetsHistoryDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100)	CONSTRAINT [utbMusicSheetsHistoryDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbMSHistoryID] PRIMARY KEY CLUSTERED ([MSHistoryID] ASC),
	CONSTRAINT [fk.usr.utbMusicSheets.usr.utbMusicSheetsHistory.MSID] FOREIGN KEY ([MSID]) REFERENCES [usr].[utbMusicSheets] ([MSID]),
	CONSTRAINT [fk.usr.utbMusicSheetTypes.usr.utbMusicSheetsHistory.MSTypeID] FOREIGN KEY ([MSTypeID]) REFERENCES [usr].[utbMusicSheetTypes] ([MSTypeID]),
	CONSTRAINT [fk.usr.utbSongs.usr.utbMusicSheetsHistory.SongID] FOREIGN KEY ([SongID]) REFERENCES [usr].[utbSongs] ([SongID]),
	CONSTRAINT [fk.usr.utbInstruments.usr.utbMusicSheetsHistory.InstrumentID] FOREIGN KEY ([InstrumentID]) REFERENCES [usr].[utbInstruments] ([InstrumentID])
);


GO
EXECUTE sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Esta tabla contiene toda la data historica de todas las partituras, cifrados, etc.', 
@level0type = N'SCHEMA', @level0name = N'usr', 
@level1type = N'TABLE', @level1name = N'utbMusicSheetsHistory';


