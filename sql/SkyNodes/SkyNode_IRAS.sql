USE [SkyNode_IRAS]
GO

CREATE TABLE [dbo].[PhotoObj](
--/ <summary>The main PhotoObj table for the IRAS catalog</summary>
--/ <remarks>The main PhotoObj table for the IRAS catalog</remarks>
	[objID] [bigint] IDENTITY(1,1) NOT NULL, --/ <column>unique object identifier</column>
	[name] [varchar](11) NOT NULL, --/ <column>IRAS Source Name</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">J2000 right ascension</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">J2000 declination</column>
	[err_maj] [real] NOT NULL, --/ <column unit="arcsec">Uncertainty ellipse major axis</column>
	[err_min] [real] NOT NULL, --/ <column unit="arcsec">Uncertainty ellipse minor axis</column>
	[err_ang] [smallint] NOT NULL, --/ <column unit="deg">Uncertainty ellipse position angle</column>
	[nhcon] [smallint] NOT NULL, --/ <column>Number of times observed (&lt;25)</column>
	[flux_12] [real] NOT NULL, --/ <column unit="Jansky">Averaged non-color corrected flux density, 12 microns</column>
	[flux_25] [real] NOT NULL, --/ <column unit="Jansky">Averaged non-color corrected flux density, 25 microns</column>
	[flux_60] [real] NOT NULL, --/ <column unit="Jansky">Averaged non-color corrected flux density, 60 microns</column>
	[flux_100] [real] NOT NULL, --/ <column unit="Jansky">Averaged non-color corrected flux density,100 microns</column>
	[fqual_12] [smallint] NOT NULL, --/ <column>flux density quality, 12 microns</column>
	[fqual_25] [smallint] NOT NULL, --/ <column>flux density quality, 25 microns</column>
	[fqual_60] [smallint] NOT NULL, --/ <column>flux density quality, 60 microns</column>
	[fqual_100] [smallint] NOT NULL, --/ <column>flux density quality,100 microns</column>
	[nlrs] [smallint] NOT NULL, --/ <column>Number of significant LRS spectra</column>
	[lrschar] [char](2) NOT NULL, --/ <column>Characterization of averaged LRS spectrum, __ is NULL</column>
	[relunc_12] [smallint] NOT NULL, --/ <column>Percent relative flux density uncertainties, 12 microns</column>
	[relunc_25] [smallint] NOT NULL, --/ <column>Percent relative flux density uncertainties, 25 microns</column>
	[relunc_60] [smallint] NOT NULL, --/ <column>Percent relative flux density uncertainties, 60 microns</column>
	[relunc_100] [smallint] NOT NULL, --/ <column>Percent relative flux density uncertainties,100 microns</column>
	[tsnr_12] [int] NOT NULL, --/ <column>Ten times the minimum signal-to-noise ratio, 12 microns</column>
	[tsnr_25] [int] NOT NULL, --/ <column>Ten times the minimum signal-to-noise ratio, 25 microns</column>
	[tsnr_60] [int] NOT NULL, --/ <column>Ten times the minimum signal-to-noise ratio, 60 microns</column>
	[tsnr_100] [int] NOT NULL, --/ <column>Ten times the minimum signal-to-noise ratio,100 microns</column>
	[cc_12] [char](1) NOT NULL, --/ <column>Point source correlation coefficient,_ is NULL, 12 microns</column>
	[cc_25] [char](1) NOT NULL, --/ <column>Point source correlation coefficient,_ is NULL, 25 microns</column>
	[cc_60] [char](1) NOT NULL, --/ <column>Point source correlation coefficient,_ is NULL, 60 microns</column>
	[cc_100] [char](1) NOT NULL, --/ <column>Point source correlation coefficient,_ is NULL,100 microns</column>
	[lvar] [smallint] NOT NULL, --/ <column>Percent Likelihood of Variability</column>
	[disc] [binary](1) NOT NULL, --/ <column>Discrepant Fluxes flag (hex-encoded)</column>
	[confuse] [binary](1) NOT NULL, --/ <column>Confusion flag (hex-encoded)</column>
	[pnearh] [smallint] NOT NULL, --/ <column>Number of nearby hours-confirmed point sources</column>
	[pnearw] [smallint] NOT NULL, --/ <column>Number of nearby weeks-confirmed point sources</column>
	[ses1_12] [smallint] NOT NULL, --/ <column>Number of seconds-confirmed nearby small extended sources, 12 microns</column>
	[ses1_25] [smallint] NOT NULL, --/ <column>Number of seconds-confirmed nearby small extended sources, 25 microns</column>
	[ses1_60] [smallint] NOT NULL, --/ <column>Number of seconds-confirmed nearby small extended sources, 60 microns</column>
	[ses1_100] [smallint] NOT NULL, --/ <column>Number of seconds-confirmed nearby small extended sources,100 microns</column>
	[ses2_12] [smallint] NOT NULL, --/ <column>Number of nearby weeks-confirmed small extended sources, 12 microns</column>
	[ses2_25] [smallint] NOT NULL, --/ <column>Number of nearby weeks-confirmed small extended sources, 25 microns</column>
	[ses2_60] [smallint] NOT NULL, --/ <column>Number of nearby weeks-confirmed small extended sources, 60 microns</column>
	[ses2_100] [smallint] NOT NULL, --/ <column>Number of nearby weeks-confirmed small extended sources,100 microns</column>
	[hsdflag] [binary](1) NOT NULL, --/ <column>Source is located in high source density bin (hex-encoded)</column>
	[cirr1] [smallint] NOT NULL, --/ <column>Number of nearby 100 micron only WSDB sources</column>
	[cirr2] [smallint] NOT NULL, --/ <column>Spatially filtered 100 micron sky brightness ratio to flux density of point source (see text)</column>
	[cirr3] [smallint] NOT NULL, --/ <column unit="MJy/sr">Total 100 micron sky surface brightness</column>
	[nid] [smallint] NOT NULL, --/ <column>Number of positional associations</column>
	[idtype] [smallint] NOT NULL, --/ <column>Type of Object</column>
	[mhcon] [smallint] NOT NULL, --/ <column>Possible number of HCONs, 99 for NULL</column>
	[fcor_12] [int] NOT NULL, --/ <column>Flux correction factor applied (times 1000),9999 is NULL, 12 microns</column>
	[fcor_25] [int] NOT NULL, --/ <column>Flux correction factor applied (times 1000),9999 is NULL, 25 microns</column>
	[fcor_60] [int] NOT NULL, --/ <column>Flux correction factor applied (times 1000),9999 is NULL, 60 microns</column>
	[fcor_100] [int] NOT NULL, --/ <column>Flux correction factor applied (times 1000),9999 is NULL,100 microns</column>
  [cx] [float] NULL, --/ <column>Cartesian coordinate x</column>
	[cy] [float] NULL, --/ <column>Cartesian coordinate y</column>
	[cz] [float] NULL, --/ <column>Cartesian coordinate z</column>
	[htmid] [bigint] NULL --/ <column>Unique HTM ID</column>
) ON [PRIMARY]

ALTER TABLE [dbo].[PhotoObj] ADD PRIMARY KEY CLUSTERED 
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