USE [SkyNode_Test]
GO

---------------------------------------------------------------

IF (OBJECT_ID('[dbo].[GalexPhotoObjAll]') IS NOT NULL)
DROP TABLE [dbo].[GalexPhotoObjAll]

GO

CREATE TABLE [dbo].[GalexPhotoObjAll]
(
	[objId] [bigint] NOT NULL,
	[mode] [tinyint] NOT NULL,
	[ra] [float] NOT NULL,
	[dec] [float] NOT NULL,
	[cx] [float] NOT NULL,
	[cy] [float] NOT NULL,
	[cz] [float] NOT NULL,
	[htmId] [bigint] NOT NULL,
	[zoneId] [bigint] NOT NULL,
	[nuv_mag] [real] NOT NULL,
	[fuv_mag] [real] NOT NULL,

	CONSTRAINT PK_GalexPhotoObjAll PRIMARY KEY 
	(
		objID
	)
)

GO

-- Populate

DECLARE @H float = 4.0 / 3600.0

INSERT [dbo].[GalexPhotoObjAll] WITH (TABLOCKX)
SELECT [objId], [mode],
	[ra], [dec], [cx], [cy], [cz], [htmId], 
	CONVERT(INT,FLOOR((dec + 90.0) / @H)) AS [zoneID],
	p.[nuv_mag], p.[fuv_mag]
FROM [SkyNode_GALEX]..[PhotoObjAll] p
INNER JOIN SkyQuery_Code.htm.CoverCircleEq(0, 0, 240) htm
	ON p.htmID BETWEEN htm.htmIDStart AND htm.htmIDEnd

GO

-- Create indexes

CREATE INDEX [IX_GalexPhotoObjAll_HtmID] ON [GalexPhotoObjAll]
(
	htmID
)
INCLUDE
(
	[mode],
	[ra], [dec], [cx], [cy], [cz]
)

CREATE INDEX [IX_GalexPhotoObjAll_ZoneID] ON [GalexPhotoObjAll]
(
	zoneID, ra
)
INCLUDE
(
	[mode],
	[dec], [cx], [cy], [cz]
)

GO