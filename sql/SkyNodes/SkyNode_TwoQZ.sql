USE [SkyNode_TwoQZ]
GO

CREATE TABLE [dbo].[PhotoObj](
--/ <summary>The main PhotoObj table for the 2QZ catalog</summary>
--/ <remarks>The main PhotoObj table for the 2QZ catalog</remarks>
	[objID] [bigint] NOT NULL, --/ <column>unique object identifier</column>
	[name] [varchar](16) NOT NULL, --/ <column>IAU format object name</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">J2000 right ascension</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">J2000 declination</column>
	[cat_num] [int] NOT NULL, --/ <column>Internal catalogue object number</column>
	[cat_name] [varchar](10) NOT NULL, --/ <column>Internal catalogue object name</column>
	[sector] [varchar](25) NOT NULL, --/ <column>Name of the sector this object inhabits</column>
	[raB1950] [float] NOT NULL, --/ <column unit="deg">Right ascension B1950</column>
	[decB1950] [float] NOT NULL, --/ <column unit="deg">Declination B1950</column>
	[UKST] [smallint] NOT NULL, --/ <column>UKST survey field number</column>
	[x_apm] [float] NOT NULL, --/ <column>APM scan X position (~8 micron pixels)</column>
	[y_apm] [float] NOT NULL, --/ <column>APM scan Y position (~8 micron pixels)</column>
	[raBrad] [float] NOT NULL, --/ <column unit="rad"></column>
	[decBrad] [float] NOT NULL, --/ <column unit="rad"></column>
	[bj] [real] NOT NULL, --/ <column unit="mag"></column>
	[u_bj] [real] NOT NULL, --/ <column>u-bj colour</column>
	[bj_r] [real] NOT NULL, --/ <column>bj-r colour [incl. r upper limits as (bj-rlim-10)]</column>
	[n_obs] [smallint] NOT NULL, --/ <column>Number of observations made with 2dF</column>
	[z1] [real] NOT NULL, --/ <column>Redshift (Obs.#1)</column>
	[q1] [smallint] NOT NULL, --/ <column>Identification quality x 10 + redshift quality</column>
	[id1] [varchar](10) NOT NULL, --/ <column>Identification</column>
	[date1] [varchar](8) NOT NULL, --/ <column>Observation date</column>
	[fld1] [smallint] NOT NULL, --/ <column>2dF field number x 10 + spectrograph number</column>
	[fibre1] [smallint] NOT NULL, --/ <column>2dF fibre number (in spectrograph)</column>
	[SN1] [real] NOT NULL, --/ <column>Signal-to-noise ratio in the 4000-5000A band</column>
	[z2] [real] NOT NULL, --/ <column>Redshift (Obs.#2)</column>
	[q2] [smallint] NOT NULL, --/ <column>Identification quality x 10 + redshift quality</column>
	[id2] [varchar](10) NOT NULL, --/ <column>Identification</column>
	[date2] [varchar](8) NOT NULL, --/ <column>Observation date</column>
	[fld2] [smallint] NOT NULL, --/ <column>2dF field number x 10 + spectrograph number</column>
	[fibre2] [smallint] NOT NULL, --/ <column>2dF fibre number (in spectrograph)</column>
	[SN2] [real] NOT NULL, --/ <column>Signal-to-noise ratio in the 4000-5000A band</column>
	[zprev] [real] NOT NULL, --/ <column>Previously known redshift (Veron-Cetty &amp; Veron 2000)</column>
	[radio] [real] NOT NULL, --/ <column unit="mJy">NVSS radio flux</column>
	[Xray] [real] NOT NULL, --/ <column unit="keV">RASS  x-ray flux, 0.2-2.4keV (x10-13 erg s-1 cm-2)</column>
	[dust] [real] NOT NULL, --/ <column>E_{B-V} (Schlegel, Finkbeiner &amp; Davis 1998)</column>
	[comm1] [varchar](20) NOT NULL, --/ <column>Specific comments on observation 1</column>
	[comm2] [varchar](20) NOT NULL, --/ <column>Specific comments on observation 2</column>
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