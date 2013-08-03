USE SkyNode_TwoDF

CREATE TABLE [dbo].[PhotoObj](
--/ <summary>The main PhotoObj table for the 2DF catalog</summary>
--/ <remarks>The main PhotoObj table for the 2DF catalog</remarks>
	[objID] [bigint] NOT NULL, --/ <column>unique object identifier, SEQNUM in original catalog</column>
	[cat] [char](1) NOT NULL, --/ <column>Catalogue Type: n for NGP,s for SGP and r for random fiels</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">J2000 right ascension</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">J2000 declination</column>
  [cx] [float] NULL, --/ <column>Cartesian coordinate x</column>
	[cy] [float] NULL, --/ <column>Cartesian coordinate y</column>
	[cz] [float] NULL, --/ <column>Cartesian coordinate z</column>
	[htmid] [bigint] NULL, --/ <column>Unique HTM ID</column>
	[bjsel] [real] NOT NULL, --/ <column unit="mag">Final bj magnitude with extinction correction</column>
	[prob] [real] NOT NULL, --/ <column>psi classification parameter</column>
	[park] [real] NOT NULL, --/ <column>k classification parameter = k / k_star</column>
	[parmu] [real] NOT NULL, --/ <column>mu classification parameter = mu / mu_star</column>
	[igal] [smallint] NOT NULL, --/ <column>Final classification</column>
	[jon] [smallint] NOT NULL, --/ <column>Eyeball classification</column>
	[orient] [real] NOT NULL, --/ <column>Orientation measured in degrees clockwise from E to W</column>
	[eccent] [real] NOT NULL, --/ <column>Eccentricity (e)</column>
	[area] [real] NOT NULL, --/ <column>Isophotal area in pixels</column>
	[x_bj] [real] NOT NULL, --/ <column>Plate x_bj in 8 micron pixels</column>
	[y_bj] [real] NOT NULL, --/ <column>Plate y_bj in 8 micron pixels</column>
	[dx] [real] NOT NULL, --/ <column>Distortion corrected difference (x_bj - x_R)*100</column>
	[dy] [real] NOT NULL, --/ <column>Distortion corrected difference (y_bj - y_R)*100</column>
	[bjg] [real] NOT NULL, --/ <column unit="mag">Final bj magnitude without extinction correction</column>
	[rmag] [real] NOT NULL, --/ <column unit="mag">Unmatched APM 'total' mag</column>
	[pmag] [real] NOT NULL, --/ <column unit="mag">Unmatched raw APM profile integrated mag</column>
	[fmag] [real] NOT NULL, --/ <column unit="mag">Unmatched raw APM 2" profile integrated mag</column>
	[smag] [real] NOT NULL, --/ <column unit="mag">Unmatched raw stellar mag (from APMCAL)</column>
	[redmag] [real] NOT NULL, --/ <column>Not used.</column>
	[ifield] [int] NOT NULL, --/ <column>UKST field</column>
	[igfield] [int] NOT NULL, --/ <column>Galaxy number in UKST field</column>
	[name] [varchar](10) NOT NULL, --/ <column>2dFGRS assigned name</column>
	[bjg_old] [real] NOT NULL, --/ <column unit="mag">Original bj magnitude without extinction correction</column>
	[bjselold] [real] NOT NULL, --/ <column unit="mag">Original bj magnitude with extinction correction</column>
	[bjg_100] [real] NOT NULL, --/ <column unit="mag">100k release bj magnitude without extinction correction</column>
	[bjsel100] [real] NOT NULL --/ <column unit="mag">100k release bj  magnitude with extinction correction</column>
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

CREATE TABLE [dbo].[SpecObj](
--/ <summary>The main SpecObj table for the 2DF catalog</summary>
--/ <remarks>The main SpecObj table for the 2DF catalog</remarks>
	[objID] [bigint] NOT NULL, --/ <column>unique object identifier, SEQNUM in original catalog</column>
	[cat] [char](1) NOT NULL, --/ <column>Catalogue Type: n for NGP,s for SGP and r for random fiels</column>
	[spectra] [smallint] NOT NULL, --/ <column>Number of spectra obtained</column>
	[name] [varchar](10) NOT NULL, --/ <column>2dFGRS name</column>
	[UKST] [varchar](3) NOT NULL, --/ <column>UKST plate (=IFIELD)</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">Right Ascension (J2000)</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">Declination (J2000)</column>
  [cx] [float] NULL, --/ <column>Cartesian coordinate x</column>
	[cy] [float] NULL, --/ <column>Cartesian coordinate y</column>
	[cz] [float] NULL, --/ <column>Cartesian coordinate z</column>
	[htmid] [bigint] NULL, --/ <column>Unique HTM ID</column>
	[bjg] [real] NOT NULL, --/ <column unit="mag">Final bj magnitude without extinction correction</column>
	[bjsel] [real] NOT NULL, --/ <column unit="mag">Final bj magnitude with extinction correction</column>
	[bjg_old] [real] NOT NULL, --/ <column unit="mag">Original bj magnitude without extinction correction</column>
	[bjselold] [real] NOT NULL, --/ <column unit="mag">Original bj magnitude with extinction correction</column>
	[galext] [real] NOT NULL, --/ <column>Galactic extinction value</column>
	[sb_bj] [real] NOT NULL, --/ <column unit="mag">SuperCosmos bj magnitude without extinction correction</column>
	[sr_r] [real] NOT NULL, --/ <column unit="mag">SuperCosmos R magnitude without extinction correction</column>
	[z] [real] NOT NULL, --/ <column>Best redshift (observed)</column>
	[z_helio] [real] NOT NULL, --/ <column>Best redshift (heliocentric)</column>
	[obsrun] [varchar](5) NOT NULL, --/ <column>Observation run of best spectrum</column>
	[quality] [smallint] NOT NULL, --/ <column>Redshift quality parameter for best spectrum; reliable redshifts have >=3</column>
	[abemma] [smallint] NOT NULL, --/ <column>Redshift type (abs=1, emi=2, man=3)</column>
	[z_abs] [real] NOT NULL, --/ <column>Cross-correlation redshift from best spectrum</column>
	[kbestr] [smallint] NOT NULL, --/ <column>Cross-correlation template from best spectrum</column>
	[r_crcor] [real] NOT NULL, --/ <column>Cross-correlation R value from best spectrum</column>
	[z_emi] [real] NOT NULL, --/ <column>Emission redshift from best spectrum</column>
	[nmbest] [smallint] NOT NULL, --/ <column>Number of emission lines for Z_EMI from best spectrum</column>
	[snr] [real] NOT NULL, --/ <column>Median S/N per pixel from best spectrum</column>
	[eta_type] [real] NOT NULL --/ <column>Eta spectral type parameter from best spectrum (-99.9 if none)</column>
) ON [PRIMARY]


ALTER TABLE [dbo].[SpecObj] ADD PRIMARY KEY CLUSTERED 
(
	[objID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

