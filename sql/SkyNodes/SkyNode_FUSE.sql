USE SkyNode_FUSE

CREATE TABLE [dbo].[PhotoObj](
--/ <summary>A view of the SpecObj table so SkyNode queries will work</summary>
--/ <remarks>Using the FUSE SkyNode for cross-matching requires it to have a table named PhotoPrimary.  Even though FUSE data is spectroscopic data, a PhotoPrimary view of the main SpecObj table is defined for this purpose.</remarks>
	[objID] [bigint] NOT NULL, --/ <column>Unique ID of each object</column>
	[TELESCOP] [varchar](64) NOT NULL, --/ <column></column>
	[ROOTNAME] [varchar](64) NOT NULL, --/ <column>Rootname of the observation ppppttooeee</column>
	[PRGRM_ID] [varchar](64) NOT NULL, --/ <column>PROGRAM IDENTIFICATION</column>
	[TARG_ID] [float] NOT NULL, --/ <column>Target ID</column>
	[PR_INV_L] [varchar](64) NOT NULL, --/ <column> Last name of principal investigator</column>
	[PR_INV_R] [varchar](64) NOT NULL, --/ <column> First name of principal investigator </column>
	[TARGNAME] [varchar](64) NOT NULL, --/ <column>Target name on proposal</column>
	[RA] [float] NOT NULL, --/ <column unit="deg">Right ascension of the target</column>
	[DEC] [float] NOT NULL, --/ <column unit="deg">Declination of the target</column>
	[APER_PA] [float] NOT NULL, --/ <column unit="deg">Position angle of Y axis</column>
	[ELAT] [float] NOT NULL, --/ <column unit="deg">Ecliptic latitud</column>
	[ELONG] [float] NOT NULL, --/ <column unit="deg">Ecliptic longitud</column>
	[GLAT] [float] NOT NULL, --/ <column unit="deg">Galactic latitud</column>
	[GLONG] [float] NOT NULL, --/ <column unit="deg">Galactic longitud</column>
	[OBJCLASS] [float] NOT NULL, --/ <column>Object class</column>
	[SP_TYPE] [varchar](64) NULL, --/ <column> MK spectral type and luminosity class </column>
	[SRC_TYPE] [varchar](64) NULL, --/ <column>Point or Extended Continuum Emission</column>
	[VMAG] [float] NOT NULL, --/ <column>Target V magnitud</column>
	[EBV] [float] NOT NULL, --/ <column>Color excess</column>
	[Z] [float] NOT NULL, --/ <column>Redshift</column>
	[HIGH_PM] [varchar](64) NOT NULL, --/ <column>High proper motion target</column>
	[MOV_TARG] [varchar](64) NOT NULL, --/ <column>Fixed or Moving target</column>
	[DATEOBS] [varchar](64) NOT NULL, --/ <column>UT date of start of exposure yyyy-mm-dd</column>
	[OBSSTART] [float] NOT NULL, --/ <column>Exposure start time - Modified Julian Date</column>
	[OBSEND] [float] NOT NULL, --/ <column>Exposure end time - Modified Julian Date</column>
	[OBSTIME] [float] NOT NULL, --/ <column unit="s"> Total time after screening </column>
	[RAWTIME] [float] NOT NULL, --/ <column unit="s">Exposure duration of raw data file</column>
	[OBSNIGHT] [float] NOT NULL, --/ <column unit="s">Night time after screening</column>
	[INSTMODE] [varchar](64) NOT NULL, --/ <column>Instrument mode TTAG or HIST</column>
	[APERTURE] [varchar](64) NOT NULL, --/ <column>Planned target aperture</column>
	[CF_VERS] [varchar](64) NOT NULL, --/ <column>CALFUSE pipeline version number</column>
	[BANDWID] [float] NOT NULL, --/ <column unit="Angstroms">Bandwidth of the data</column>
	[CENTRWV] [float] NOT NULL, --/ <column unit="Angstroms">Central wavelenght of the data</column>
	[WAVEMIN] [float] NOT NULL, --/ <column unit="Angstroms">Minimun wavelenght of the data</column>
	[WAVEMAX] [float] NOT NULL, --/ <column unit="Angstroms">Maximun wavelenght of the data</column>
	[cx] [float] NULL, --/ <column>Cartesian coordinate x</column>
	[cy] [float] NULL, --/ <column>Cartesian coordinate y</column>
	[cz] [float] NULL, --/ <column>Cartesian coordinate z</column>
	[htmid] [bigint] NULL --/ <column>Unique HTM ID</column>
) ON [PRIMARY]


ALTER TABLE [dbo].[PhotoObj] ADD  CONSTRAINT [PK_PhotoObj] PRIMARY KEY CLUSTERED 
(
	[objID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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

CREATE TABLE [dbo].[SpecLine](
--/ <summary>The flux for each spectral line is in this table</summary>
--/ <remarks></remarks>
	[ObjID] [bigint] NOT NULL, --/ <column></column>
	[LineID] [int] IDENTITY(1,1) NOT NULL, --/ <column>Unique ID for each spectral line</column>
	[WAVE] [float] NOT NULL, --/ <column>Wavelength</column>
	[FLUX] [float] NOT NULL, --/ <column>Flux</column>
	[ERROR] [float] NOT NULL --/ <column>Error</column>
) ON [PRIMARY]

ALTER TABLE [dbo].[SpecLine] ADD  CONSTRAINT [PK_SpecLine] PRIMARY KEY CLUSTERED 
(
	[ObjID] ASC,
	[LineID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


CREATE TABLE [dbo].[SpecObj](
--/ <summary>The main table containing the astronomical data</summary>
--/ <remarks></remarks>
	[objID] [bigint] NOT NULL, --/ <column>Unique ID of each object</column>
	[TELESCOP] [varchar](64) NOT NULL, --/ <column></column>
	[ROOTNAME] [varchar](64) NOT NULL, --/ <column>Rootname of the observation ppppttooeee</column>
	[PRGRM_ID] [varchar](64) NOT NULL, --/ <column>PROGRAM IDENTIFICATION</column>
	[TARG_ID] [float] NOT NULL, --/ <column>Target ID</column>
	[PR_INV_L] [varchar](64) NOT NULL, --/ <column> Last name of principal investigator</column>
	[PR_INV_R] [varchar](64) NOT NULL, --/ <column> First name of principal investigator </column>
	[TARGNAME] [varchar](64) NOT NULL, --/ <column>Target name on proposal</column>
	[RA] [float] NOT NULL, --/ <column unit="deg">Right ascension of the target</column>
	[DEC] [float] NOT NULL, --/ <column unit="deg">Declination of the target</column>
	[APER_PA] [float] NOT NULL, --/ <column unit="deg">Position angle of Y axis</column>
	[ELAT] [float] NOT NULL, --/ <column unit="deg">Ecliptic latitud</column>
	[ELONG] [float] NOT NULL, --/ <column unit="deg">Ecliptic longitud</column>
	[GLAT] [float] NOT NULL, --/ <column unit="deg">Galactic latitud</column>
	[GLONG] [float] NOT NULL, --/ <column unit="deg">Galactic longitud</column>
	[OBJCLASS] [float] NOT NULL, --/ <column>Object class</column>
	[SP_TYPE] [varchar](64) NULL, --/ <column> MK spectral type and luminosity class </column>
	[SRC_TYPE] [varchar](64) NULL, --/ <column>Point or Extended Continuum Emission</column>
	[VMAG] [float] NOT NULL, --/ <column>Target V magnitud</column>
	[EBV] [float] NOT NULL, --/ <column>Color excess</column>
	[Z] [float] NOT NULL, --/ <column>Redshift</column>
	[HIGH_PM] [varchar](64) NOT NULL, --/ <column>High proper motion target</column>
	[MOV_TARG] [varchar](64) NOT NULL, --/ <column>Fixed or Moving target</column>
	[DATEOBS] [varchar](64) NOT NULL, --/ <column>UT date of start of exposure yyyy-mm-dd</column>
	[OBSSTART] [float] NOT NULL, --/ <column>Exposure start time - Modified Julian Date</column>
	[OBSEND] [float] NOT NULL, --/ <column>Exposure end time - Modified Julian Date</column>
	[OBSTIME] [float] NOT NULL, --/ <column unit="s"> Total time after screening </column>
	[RAWTIME] [float] NOT NULL, --/ <column unit="s">Exposure duration of raw data file</column>
	[OBSNIGHT] [float] NOT NULL, --/ <column unit="s">Night time after screening</column>
	[INSTMODE] [varchar](64) NOT NULL, --/ <column>Instrument mode TTAG or HIST</column>
	[APERTURE] [varchar](64) NOT NULL, --/ <column>Planned target aperture</column>
	[CF_VERS] [varchar](64) NOT NULL, --/ <column>CALFUSE pipeline version number</column>
	[BANDWID] [float] NOT NULL, --/ <column unit="Angstroms">Bandwidth of the data</column>
	[CENTRWV] [float] NOT NULL, --/ <column unit="Angstroms">Central wavelenght of the data</column>
	[WAVEMIN] [float] NOT NULL, --/ <column unit="Angstroms">Minimun wavelenght of the data</column>
	[WAVEMAX] [float] NOT NULL, --/ <column unit="Angstroms">Maximun wavelenght of the data</column>
	[cx] [float] NULL, --/ <column>Cartesian coordinate x</column>
	[cy] [float] NULL, --/ <column>Cartesian coordinate y</column>
	[cz] [float] NULL, --/ <column>Cartesian coordinate z</column>
	[htmid] [bigint] NULL --/ <column>Unique HTM ID</column>
) ON [PRIMARY]


ALTER TABLE [dbo].[SpecObj] ADD  CONSTRAINT [PK_SpecObj] PRIMARY KEY CLUSTERED 
(
	[objID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


