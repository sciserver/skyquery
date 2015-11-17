USE [SkyNode_Test]
GO

---------------------------------------------------------------

IF (OBJECT_ID('[dbo].[SDSSDR7PhotoObjAll]') IS NOT NULL)
DROP TABLE [dbo].[SDSSDR7PhotoObjAll]

GO

CREATE TABLE [dbo].[SDSSDR7PhotoObjAll]
(
	--/ <summary>Photometric object identifier</summary>
	--/ <class>meta.id, meta.main</class>
	[objId] [bigint] NOT NULL,

	--/ <class>meta.version</class>
	[skyVersion] [tinyint] NOT NULL,

	[run] [smallint] NOT NULL,

	[rerun] [smallint] NOT NULL,

	[camcol] [tinyint] NOT NULL,

	[field] [smallint] NOT NULL,

	[obj] [smallint] NOT NULL,

	[mode] [tinyint] NOT NULL,

	[type] [smallint] NOT NULL,

	--/ <class>pos.eq.ra, pos.frame=FK5</class>
	--/ <unit>deg</unit>
	[ra] [float] NOT NULL,

	--/ <class>pos.eq.dec, pos.frame=FK5</class>
	--/ <unit>deg</unit>
	[dec] [float] NOT NULL,

	--/ <class>stat.error, pos.eq.ra, pos.frame=FK5</class>
	--/ <unit>deg</unit>
	[raErr] [float] NOT NULL,
	
	--/ <class>stat.error, pos.eq.dec, pos.frame=FK5</class>
	--/ <unit>arc</unit>
	[decErr] [float] NOT NULL,

	--/ <class>pos.cartesian.x, pos.frame=FK5</class>
	[cx] [float] NOT NULL,

	--/ <class>pos.cartesian.y, pos.frame=FK5</class>
	[cy] [float] NOT NULL,

	--/ <class>pos.cartesian.z, pos.frame=FK5</class>
	[cz] [float] NOT NULL,

	--/ <class>pos.eq.htm, pos.frame=FK5</class>
	[htmId] [bigint] NOT NULL,

	--/ <class>pos.eq.zone, pos.frame=FK5</class>
	[zoneId] [bigint] NOT NULL,

	--/ <class>phot.mag, em.opt.u</class>
	--/ <unit>mag</unit>
	[u] [real] NOT NULL,
	
	--/ <class>phot.mag, em.opt.g</class>	
	--/ <unit>mag</unit>
	[g] [real] NOT NULL,

	--/ <class>phot.mag, em.opt.r</class>
	--/ <unit>mag</unit>
	[r] [real] NOT NULL,

	--/ <class>phot.mag, em.opt.i</class>
	--/ <unit>mag</unit>
	[i] [real] NOT NULL,

	--/ <class>phot.mag, em.opt.z</class>
	--/ <unit>mag</unit>
	[z] [real] NOT NULL,

	CONSTRAINT PK_SDSSDR7PhotoObjAll PRIMARY KEY 
	(
		objID
	)
)

GO

IF (OBJECT_ID('[dbo].[SDSSDR7PhotoPrimary]') IS NOT NULL)
DROP VIEW [dbo].[SDSSDR7PhotoPrimary]

GO

CREATE VIEW [dbo].[SDSSDR7PhotoPrimary]
AS
SELECT * FROM [SDSSDR7PhotoObjAll] WITH(NOLOCK)
    WHERE mode=1

GO

IF (OBJECT_ID('[dbo].[SDSSDR7Star]') IS NOT NULL)
DROP VIEW [dbo].[SDSSDR7Star]

GO

CREATE VIEW [dbo].[SDSSDR7Star]
AS
SELECT * 
    FROM [SDSSDR7PhotoPrimary]
    WHERE type = 6

GO

IF (OBJECT_ID('[dbo].[SDSSDR7Galaxy]') IS NOT NULL)
DROP VIEW [dbo].[SDSSDR7Galaxy]

GO

CREATE VIEW [dbo].[SDSSDR7Galaxy]
AS
SELECT * 
    FROM [SDSSDR7PhotoPrimary]
    WHERE type = 3

GO

-- Populate

DECLARE @H float = 4.0 / 3600.0

INSERT [dbo].[SDSSDR7PhotoObjAll] WITH (TABLOCKX)
SELECT [objId], [skyVersion], [run], [rerun], [camcol], [field], [obj], [mode], [type],
	[ra], [dec], [raErr], [decErr], [cx], [cy], [cz], [htmId], 
	CONVERT(INT,FLOOR((dec + 90.0) / @H)) AS [zoneID],
	p.[u], p.[g], p.[r], p.[i], [z]
FROM [SkyNode_SDSSDR7]..[PhotoObjAll] p
INNER JOIN SkyQuery_Code.htm.CoverCircleEq(0, 0, 240) htm
	ON p.htmID BETWEEN htm.htmIDStart AND htm.htmIDEnd

-- Create indexes

CREATE INDEX [IX_SDSSDR7PhotoObjAll_HtmID] ON [SDSSDR7PhotoObjAll]
(
	htmID
)
INCLUDE
(
	[mode], [type],
	[ra], [dec], [cx], [cy], [cz]
)

CREATE INDEX [IX_SDSSDR7PhotoObjAll_ZoneID] ON [SDSSDR7PhotoObjAll]
(
	zoneID, ra
)
INCLUDE
(
	[mode], [type],
	[dec], [cx], [cy], [cz]
)

GO

---------------------------------------------------------------

IF (OBJECT_ID('[dbo].[SDSSDR7PhotoObjAll_NoZone]') IS NOT NULL)
DROP TABLE [dbo].[SDSSDR7PhotoObjAll_NoZone]

GO

CREATE TABLE [dbo].[SDSSDR7PhotoObjAll_NoZone]
(
	[objId] [bigint] NOT NULL,
	[skyVersion] [tinyint] NOT NULL,
	[run] [smallint] NOT NULL,
	[rerun] [smallint] NOT NULL,
	[camcol] [tinyint] NOT NULL,
	[field] [smallint] NOT NULL,
	[obj] [smallint] NOT NULL,
	[mode] [tinyint] NOT NULL,
	[type] [smallint] NOT NULL,
	[ra] [float] NOT NULL,
	[dec] [float] NOT NULL,
	[raErr] [float] NOT NULL,
	[decErr] [float] NOT NULL,
	[cx] [float] NOT NULL,
	[cy] [float] NOT NULL,
	[cz] [float] NOT NULL,
	[htmId] [bigint] NOT NULL,
	[u] [real] NOT NULL,
	[g] [real] NOT NULL,
	[r] [real] NOT NULL,
	[i] [real] NOT NULL,
	[z] [real] NOT NULL,

	CONSTRAINT PK_SDSSDR7PhotoObjAll_NoZone PRIMARY KEY 
	(
		objID
	)
)

GO

-- Populate

DECLARE @H float = 4.0 / 3600.0

INSERT [dbo].[SDSSDR7PhotoObjAll_NoZone] WITH (TABLOCKX)
SELECT [objId], [skyVersion], [run], [rerun], [camcol], [field], [obj], [mode], [type],
	[ra], [dec], [raErr], [decErr], [cx], [cy], [cz], [htmId], 
	p.[u], p.[g], p.[r], p.[i], [z]
FROM [SkyNode_SDSSDR7]..[PhotoObjAll] p
INNER JOIN SkyQuery_Code.htm.CoverCircleEq(0, 0, 240) htm
	ON p.htmID BETWEEN htm.htmIDStart AND htm.htmIDEnd

-- Create indexes

CREATE INDEX [IX_SDSSDR7PhotoObjAll_NoZone_HtmID] ON [SDSSDR7PhotoObjAll_NoZone]
(
	htmID
)
INCLUDE
(
	[mode], [type],
	[ra], [dec], [cx], [cy], [cz]
)

GO

---------------------------------------------------------------

IF (OBJECT_ID('[dbo].[SDSSDR7PhotoObjAll_NoHTM]') IS NOT NULL)
DROP TABLE [dbo].[SDSSDR7PhotoObjAll_NoHTM]

GO

CREATE TABLE [dbo].[SDSSDR7PhotoObjAll_NoHTM]
(
	[objId] [bigint] NOT NULL,
	[skyVersion] [tinyint] NOT NULL,
	[run] [smallint] NOT NULL,
	[rerun] [smallint] NOT NULL,
	[camcol] [tinyint] NOT NULL,
	[field] [smallint] NOT NULL,
	[obj] [smallint] NOT NULL,
	[mode] [tinyint] NOT NULL,
	[type] [smallint] NOT NULL,
	[ra] [float] NOT NULL,
	[dec] [float] NOT NULL,
	[raErr] [float] NOT NULL,
	[decErr] [float] NOT NULL,
	[cx] [float] NOT NULL,
	[cy] [float] NOT NULL,
	[cz] [float] NOT NULL,
	[zoneId] [bigint] NOT NULL,
	[u] [real] NOT NULL,
	[g] [real] NOT NULL,
	[r] [real] NOT NULL,
	[i] [real] NOT NULL,
	[z] [real] NOT NULL,

	CONSTRAINT PK_SDSSDR7PhotoObjAll_NoHTM PRIMARY KEY 
	(
		objID
	)
)

GO

-- Populate

DECLARE @H float = 4.0 / 3600.0

INSERT [dbo].[SDSSDR7PhotoObjAll_NoHTM] WITH (TABLOCKX)
SELECT [objId], [skyVersion], [run], [rerun], [camcol], [field], [obj], [mode], [type],
	[ra], [dec], [raErr], [decErr], [cx], [cy], [cz],
	CONVERT(INT,FLOOR((dec + 90.0) / @H)) AS [zoneID],
	p.[u], p.[g], p.[r], p.[i], [z]
FROM [SkyNode_SDSSDR7]..[PhotoObjAll] p
INNER JOIN SkyQuery_Code.htm.CoverCircleEq(0, 0, 240) htm
	ON p.htmID BETWEEN htm.htmIDStart AND htm.htmIDEnd

-- Create indexes



CREATE INDEX [IX_SDSSDR7PhotoObjAll_NoHTM_ZoneID] ON [SDSSDR7PhotoObjAll_NoHTM]
(
	zoneID, ra
)
INCLUDE
(
	[mode], [type],
	[dec], [cx], [cy], [cz]
)

GO