USE [SkyNode_Test]
GO

---------------------------------------------------------------

IF (OBJECT_ID('[dbo].[WisePhotoObj]') IS NOT NULL)
DROP TABLE [dbo].[WisePhotoObj]

GO

CREATE TABLE [dbo].[WisePhotoObj]
(
	[cntr] [bigint] NOT NULL,
	[ra] [float] NOT NULL,
	[dec] [float] NOT NULL,
	[cx] [float] NOT NULL,
	[cy] [float] NOT NULL,
	[cz] [float] NOT NULL,
	[sigra] [float] NOT NULL,
	[sigdec] [float] NOT NULL,
	[sigradec] [float] NOT NULL,
	[htmId] [bigint] NOT NULL,
	[zoneId] [bigint] NOT NULL,
	[w1mpro] [real] NOT NULL,
	[w2mpro] [real] NOT NULL,
	[w3mpro] [real] NOT NULL,
	[w4mpro] [real] NOT NULL,

	CONSTRAINT PK_WisePhotoObj PRIMARY KEY 
	(
		[cntr]
	)
)

GO

-- Populate

DECLARE @H float = 4.0 / 3600.0

INSERT [dbo].[WisePhotoObj] WITH (TABLOCKX)
SELECT 
	[cntr], [ra], [dec], [cx], [cy], [cz], [sigra], [sigdec], [sigradec], [htmid],
	CONVERT(INT,FLOOR((dec + 90.0) / @H)) AS [zoneID],
	[w1mpro], [w2mpro], [w3mpro], [w4mpro]
FROM [SkyNode_WISE]..[PhotoObj] p
INNER JOIN SkyQuery_Code.htm.CoverCircleEq(2, 0, 240) htm
	ON p.htmID BETWEEN htm.htmIDStart AND htm.htmIDEnd

GO

CREATE INDEX [IX_WisePhotoObj_HtmID] ON [WisePhotoObj]
(
	htmID
)
INCLUDE
(
	[ra], [dec], [cx], [cy], [cz], [zoneid]
)

CREATE INDEX [IX_WisePhotoObj_ZoneID] ON [WisePhotoObj]
(
	zoneID, ra
)
INCLUDE
(
	[dec], [cx], [cy], [cz], [htmid]
)