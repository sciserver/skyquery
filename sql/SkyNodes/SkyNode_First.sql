USE [SkyNode_First]
GO

CREATE TABLE [dbo].[PhotoObj](
--/ <summary>The main PhotoObj table for the First catalog</summary>
--/ <remarks>The main PhotoObj table for the First catalog</remarks>
	[objID] [bigint] NOT NULL, --/ <column>unique object identifier</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">J2000 right ascension</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">J2000 declination</column>
	[w] [char](1) NOT NULL, --/ <column>W warning flag</column>
	[fpeak] [real] NOT NULL, --/ <column unit="mJy">peak flux density</column>
	[fint] [real] NOT NULL, --/ <column unit="mJy">integrated flux density</column>
	[rms] [real] NOT NULL, --/ <column unit="mJy">local rms noise estimate</column>
	[maj] [real] NOT NULL, --/ <column unit="arcsec">major axis raw</column>
	[min] [real] NOT NULL, --/ <column unit="arcsec">minor axis raw</column>
	[pa] [real] NOT NULL, --/ <column unit="deg">position angle raw</column>
	[fmaj] [real] NOT NULL, --/ <column unit="arcsec">major axis (fitted)</column>
	[fmin] [real] NOT NULL, --/ <column unit="arcsec">minor axis (fitted)</column>
	[fpa] [real] NOT NULL, --/ <column unit="deg">position angle (fitted)</column>
	[fieldname] [varchar](32) NOT NULL, --/ <column>name of the coadded image containing the source</column>
  [htmid] [bigint] NOT NULL, --/ <column>htmid for spatial searches</column>
  [cx] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
	[cy] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
	[cz] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
) ON [PRIMARY]


-- Index to support on the fly zone table creation
CREATE NONCLUSTERED INDEX [IX_PhotoObj_Zone] ON [dbo].[PhotoObj] 
(
	[dec] ASC,
	[ra] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


-- HTM index
CREATE NONCLUSTERED INDEX [IX_PhotoObj_HtmID] ON [dbo].[PhotoObj] 
(
	[htmid] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO