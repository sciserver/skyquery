USE [SkyNode_AGC]
GO

CREATE TABLE [dbo].[PhotoObj](
--------------------------------------------------------------------------------
--/ <summary>Arecibo Galaxy Catalog</summary>
--/ <remarks>Objects from the Arecibo Galaxy Catalog, which provide 
--/ HI measurements of about 100,000 bright galaxies. Galaxy redshifts 
--/ are in km/s</remarks>
--------------------------------------------------------------------------------
	[objid] [bigint] NOT NULL, --/ <column>Unique object identifier</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">RA (J2000)</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">Dec (J2000)</column>
	[cx] [float] NOT NULL, --/ <column>Cartesian x</column>
	[cy] [float] NOT NULL, --/ <column>Cartesian y</column>
	[cz] [float] NOT NULL, --/ <column>Cartesian z</column>
	[htmID] [bigint] NOT NULL, --/ <column>the htmid of the object</column>
	[a] [real] NOT NULL, --/ <column unit="arcmin">Major axis</column>
	[b] [real] NOT NULL, --/ <column unit="arcmin">Major axis</column>
	[Bmag] [real] NOT NULL, --/ <column unit="mag">B magnitude</column>
	[angle] [real] NOT NULL, --/ <column unit="deg">Position angle</column>
	[type] [varchar](8) NULL, --/ <column>Morphological type</column>
	[btype] [int] NOT NULL, --/ <column>Burstein morph type (enum)</column>
	[velocity] [real] NOT NULL, --/ <column unit="km/s">Optical velocity</column>
	[velocityError] [real] NOT NULL, --/ <column unit="km/s">Error of optical velocity</column>
	[objname] [varchar](64) NULL, --/ <column>Object name</column>
	[fluxHI] [real] NOT NULL, --/ <column unit="Jy*km/s">HI flux</column>
	[fluxHInoise] [real] NOT NULL, --/ <column unit="Jy*km/s">Noise HI rms</column>
	[centerVelocity] [real] NOT NULL, --/ <column unit="km/s">HI center velocity</column>
	[velocityWidth] [real] NOT NULL, --/ <column unit="km/s">Velocity width</column>
	[velocityWidthError] [real] NOT NULL, --/ <column unit="km/s">Error of velocity width</column>
	[telescopeCode] [char](1) NULL, --/ <column>Telescope code</column>
	[HIdetectionCode] [tinyint] NOT NULL, --/ <column>HI detection code</column>
	[HIsnr] [smallint] NOT NULL, --/ <column>HI signal-to-noise ratio</column>
	[IbandQ] [tinyint] NOT NULL, --/ <column>I band quality index</column>
	[RC3flag] [tinyint] NOT NULL, --/ <column>Indicator of galaxy is part of RC3 catalog</column>
	[IrotCat] [tinyint] NOT NULL, --/ <column>Flag for rotation curve database</column>
PRIMARY KEY CLUSTERED 
(
	[objid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


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