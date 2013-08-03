USE [SkyNode_PSCz]
GO

CREATE TABLE [dbo].[PhotoObj](
--/ <summary>The main PhotoObj table for the PSCz catalog</summary>
--/ <remarks>The main PhotoObj table for the PSCz catalog</remarks>
	[objID] [bigint] NOT NULL, --/ <column>unique object identifier</column>
	[cname] [varchar](11) NOT NULL, --/ <column>PSCz name e.g. Q12345+4856</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">J2000 right ascension</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">J2000 declination</column>
	[flux_12] [real] NOT NULL, --/ <column unit="Jy">non-color corrected flux, 12 um</column>
	[flux_25] [real] NOT NULL, --/ <column unit="Jy">non-color corrected flux, 25 um</column>
	[flux_60] [real] NOT NULL, --/ <column unit="Jy">non-color corrected flux, 60 um</column>
	[flux_100] [real] NOT NULL, --/ <column unit="Jy">non-color corrected flux, 100 um</column>
	[iq_12] [smallint] NOT NULL, --/ <column>Flux origin and quality flag, 12 um</column>
	[iq_25] [smallint] NOT NULL, --/ <column>Flux origin and quality flag, 25 um</column>
	[iq_60] [smallint] NOT NULL, --/ <column>Flux origin and quality flag, 60 um</column>
	[iq_100] [smallint] NOT NULL, --/ <column>Flux origin and quality flag, 100 um</column>
	[ises_12] [smallint] NOT NULL, --/ <column>SES flag, derived from SES2, 12 um</column>
	[ises_25] [smallint] NOT NULL, --/ <column>SES flag, derived from SES2, 25 um</column>
	[ises_60] [smallint] NOT NULL, --/ <column>SES flag, derived from SES2, 60 um</column>
	[ises_100] [smallint] NOT NULL, --/ <column>SES flag, derived from SES2, 100 um</column>
	[iu_12] [smallint] NOT NULL, --/ <column unit="%">flux uncertainty (%), 12 um</column>
	[iu_25] [smallint] NOT NULL, --/ <column unit="%">flux uncertainty (%), 25 um</column>
	[iu_60] [smallint] NOT NULL, --/ <column unit="%">flux uncertainty (%), 60 um</column>
	[iu_100] [smallint] NOT NULL, --/ <column unit="%">flux uncertainty (%), 100 um</column>
	[mine] [smallint] NOT NULL, --/ <column>Minor axe of 2 sig er ellipse</column>
	[maje] [smallint] NOT NULL, --/ <column>Major axe of 2 sig er ellipse,</column>
	[ipose] [smallint] NOT NULL, --/ <column>PA of 2 sig er ellipse</column>
	[ic1] [smallint] NOT NULL, --/ <column>CIRR1 flag</column>
	[ic2] [smallint] NOT NULL, --/ <column>CIRR2 flag</column>
	[ic3] [smallint] NOT NULL, --/ <column>CIRR3 flag</column>
	[cc_12] [varchar](1) NULL, --/ <column>Correlation Coefficient, 12 um (NULL allowed)</column>
	[cc_25] [varchar](1) NULL, --/ <column>Correlation Coefficient, 25 um (NULL allowed)</column>
	[cc_60] [varchar](1) NULL, --/ <column>Correlation Coefficient, 60 um (NULL allowed)</column>
	[cc_100] [varchar](1) NULL, --/ <column>Correlation Coefficient, 100 um (NULL allowed)</column>
	[conf] [char](1) NULL, --/ <column>Confusion flag (NULL allowed)</column>
	[cdisc] [char](1) NULL, --/ <column>Discrepant flux flag (NULL allowed)</column>
	[chsd] [char](1) NULL, --/ <column>HSD flag (NULL allowed)</column>
	[ipnearh] [smallint] NOT NULL, --/ <column>Nearby confirmed sources</column>
	[ipnearw] [smallint] NOT NULL, --/ <column>Nearby confirmed sources</column>
	[ises1_12] [smallint] NOT NULL, --/ <column>nearby seconds confirmed extended sources, 12 um</column>
	[ises1_25] [smallint] NOT NULL, --/ <column>nearby seconds confirmed extended sources, 25 um</column>
	[ises1_60] [smallint] NOT NULL, --/ <column>nearby seconds confirmed extended sources, 60 um</column>
	[ises1_100] [smallint] NOT NULL, --/ <column>nearby seconds confirmed extended sources, 100 um</column>
	[isnr3] [int] NOT NULL, --/ <column>10* s/n at 60um</column>
	[nhcon] [smallint] NOT NULL, --/ <column>Number of HCONS with source detected</column>
	[mhcon] [smallint] NOT NULL, --/ <column>Number of HCONS covering sourc</column>
	[ifcor3] [smallint] NOT NULL, --/ <column>flux correction factor</column>
	[cplate] [varchar](5) NOT NULL, --/ <column>Plate number (POSS or SRC)</column>
	[cnom] [varchar](7) NOT NULL, --/ <column>Nominal plate centre</column>
	[num] [varchar](3) NOT NULL, --/ <column>Identifier for igal source on this plate</column>
	[iarh] [int] NOT NULL, --/ <column unit="HH">RA hours of best optical id</column>
	[iarm] [int] NOT NULL, --/ <column unit="MIN">RA mins of best optical id</column>
	[iars] [int] NOT NULL, --/ <column unit="SEC">RA secs of best optical id INTEGER PART!</column>
	[ars] [real] NOT NULL, --/ <column unit="SEC">RA secs of best optical id DECIMAL PART!</column>
	[cadsn] [varchar](1) NULL, --/ <column>Dec SIGN (+ if NULL)</column>
	[iadd] [int] NOT NULL, --/ <column unit="DEG">Dec DEG part of best optical id</column>
	[iadm] [int] NOT NULL, --/ <column unit="MIN">Dec MINS part of best optical id</column>
	[iads] [int] NOT NULL, --/ <column unit="SEC">Dec SEC INTEGER part of best optical id</column>
	[ads] [real] NOT NULL, --/ <column unit="SEC">Dec SEC DECIMAL part of best optical id</column>
	[iadx] [smallint] NOT NULL, --/ <column unit="arcsec">Delta X from IRAS posn</column>
	[iady] [smallint] NOT NULL, --/ <column unit="arcsec">Delta Y from IRAS posn</column>
	[alk] [real] NOT NULL, --/ <column>log likelihood</column>
	[irel] [smallint] NOT NULL, --/ <column unit="%">% chance this is right id</column>
	[cand] [varchar](1) NULL, --/ <column>Candidate A,B etc (NULL allowed)</column>
	[iopt] [smallint] NOT NULL, --/ <column>Schmidt plate material</column>
	[amag] [real] NOT NULL, --/ <column unit="mag">APM magnitude</column>
	[iad1] [smallint] NOT NULL, --/ <column unit="arcsec">APM d1</column>
	[iad2] [smallint] NOT NULL, --/ <column unit="arcsec">APM d2</column>
	[iapa] [smallint] NOT NULL, --/ <column>APM PA</column>
	[amu] [real] NOT NULL, --/ <column unit="(mags/arcsecs^2)">surface brightness</column>
	[idtype] [smallint] NOT NULL, --/ <column>ID type</column>
	[coname] [varchar](12) NULL, --/ <column>Optical name (NULL allowed)</column>
	[amzw] [real] NOT NULL, --/ <column unit="mag">Zwicky/De Vauc magnitude</column>
	[d1x] [real] NOT NULL, --/ <column>RC3/UGC/MCG/ESO diameter (X for CATX)</column>
	[iar] [smallint] NOT NULL, --/ <column>100*(d25/D25)</column>
	[ivhel] [int] NOT NULL, --/ <column>Best heliocentric Vel from Literature or PSCz.</column>
	[iverr] [int] NOT NULL, --/ <column>Error</column>
	[ivref] [smallint] NOT NULL, --/ <column>Reference</column>
	[ivhelp] [int] NOT NULL, --/ <column>PSCz heliocentric Vel</column>
	[iverrp] [smallint] NOT NULL, --/ <column>Error</column>
	[ivrefp] [smallint] NOT NULL, --/ <column>Reference</column>
	[it] [smallint] NOT NULL, --/ <column>De Vauc Type, 99 = unknown</column>
	[cpgc] [varchar](9) NULL, --/ <column>PGC name (NULL allowed)</column>
	[rad] [float] NOT NULL, --/ <column unit="deg">RA (equinox B1950, degrees)</column>
	[decd] [float] NOT NULL, --/ <column unit="deg">Dec (equinox B1950, degrees)</column>
	[glon] [float] NOT NULL, --/ <column unit="deg">Galactic longitude</column>
	[glat] [float] NOT NULL, --/ <column unit="deg">Galactic latitude</column>
	[ivgal] [int] NOT NULL, --/ <column>Velocity cz in LG frame (IVHEL + 300 sin(l) cos(b))</column>
	[dist] [real] NOT NULL, --/ <column>Distance (Rowan-Robinson 1988)</column>
	[iclus] [smallint] NOT NULL, --/ <column>Cluster id (Rowan-Robinson 1988)</column>
	[ali4] [real] NOT NULL, --/ <column>log10(100um background in MJy/sr), (see note)</column>
	[ali4b] [real] NOT NULL, --/ <column>log10(100um background in MJy/sr), (see note)</column>
	[nlbin] [int] NOT NULL, --/ <column>lune bin as from P2BIN</column>
	[class] [varchar](2) NOT NULL, --/ <column>optical classification (see note)</column>
	[csec] [varchar](5) NOT NULL, --/ <column>PSC-z sector, e.g. A/123</column>
	[platex] [smallint] NOT NULL, --/ <column unit="mm">posn from bottom left corner of Schmidt plate in mm</column>
	[platey] [smallint] NOT NULL, --/ <column unit="mm">posn from bottom left corner of Schmidt plate in mm</column>
	[cstat] [varchar](1) NULL, --/ <column>optical material E,O,R,J,X,Polaroid,Scan (NULL allowed)</column>
	[carea] [varchar](1) NULL, --/ <column>h=high|b|, p=other PSCz, l=low|b|, c=in coverage gaps (NULL allowed)</column>
	[czstat] [varchar](1) NOT NULL, --/ <column>Redshift status (see note)</column>
	[c12class] [varchar](1) NULL, --/ <column>1.2Jy classification (NULL allowed)</column>
	[f3psc] [real] NOT NULL, --/ <column>Original PSC fluxes</column>
	[f4psc] [real] NOT NULL, --/ <column>Original PSC fluxes</column>
	[abb] [real] NOT NULL, --/ <column>Extinction A_B in lune bin derived from 100 um background, dust temp corrected (see note)</column>
	[cpscz] [varchar](1) NOT NULL, --/ <column>Source category (see note)</column>
	[mhconb] [smallint] NOT NULL, --/ <column>2/3 HCON sky, calculated on lune bin basis</column>
	[cqdot] [varchar](1) NULL, --/ <column>q = in QDOT 1-in-6 sample (NULL allowed)</column>
	[iw20] [smallint] NOT NULL, --/ <column>HI (21cm) 20% vel width (see note)</column>
	[iw20e] [smallint] NOT NULL, --/ <column>error</column>
	[iw50] [smallint] NOT NULL, --/ <column>HI 50% vel width, codes as IW20</column>
	[iw50e] [smallint] NOT NULL, --/ <column>error</column>
	[ihiref] [smallint] NOT NULL, --/ <column>HI reference.</column>
	[fp_12] [real] NOT NULL, --/ <column>Addscan flux using point source template</column>
	[fp_25] [real] NOT NULL, --/ <column>Addscan flux using point source template</column>
	[fp_60] [real] NOT NULL, --/ <column>Addscan flux using point source template</column>
	[fp_100] [real] NOT NULL, --/ <column>Addscan flux using point source template</column>
	[iep_12] [smallint] NOT NULL, --/ <column unit="mJy">Statistical error</column>
	[iep_25] [smallint] NOT NULL, --/ <column unit="mJy">Statistical error</column>
	[iep_60] [smallint] NOT NULL, --/ <column unit="mJy">Statistical error</column>
	[iep_100] [smallint] NOT NULL, --/ <column unit="mJy">Statistical error</column>
	[ibp_12] [smallint] NOT NULL, --/ <column>1 for background fit, 0 for none (extended or bright)</column>
	[ibp_25] [smallint] NOT NULL, --/ <column>1 for background fit, 0 for none (extended or bright)</column>
	[ibp_60] [smallint] NOT NULL, --/ <column>1 for background fit, 0 for none (extended or bright)</column>
	[ibp_100] [smallint] NOT NULL, --/ <column>1 for background fit, 0 for none (extended or bright)</column>
	[fe_12] [real] NOT NULL, --/ <column>Rice or Yahil/Addscan flux for source with size given by WIDTH2</column>
	[fe_25] [real] NOT NULL, --/ <column>Rice or Yahil/Addscan flux for source with size given by WIDTH2</column>
	[fe_60] [real] NOT NULL, --/ <column>Rice or Yahil/Addscan flux for source with size given by WIDTH2</column>
	[fe_100] [real] NOT NULL, --/ <column>Rice or Yahil/Addscan flux for source with size given by WIDTH2</column>
	[iee_12] [smallint] NOT NULL, --/ <column unit="mJy">Statistical error</column>
	[iee_25] [smallint] NOT NULL, --/ <column unit="mJy">Statistical error</column>
	[iee_60] [smallint] NOT NULL, --/ <column unit="mJy">Statistical error</column>
	[iee_100] [smallint] NOT NULL, --/ <column unit="mJy">Statistical error</column>
	[ibe_12] [smallint] NOT NULL, --/ <column>1 for background fit, 0 for none (extended or bright), 2 = Rice</column>
	[ibe_25] [smallint] NOT NULL, --/ <column>1 for background fit, 0 for none (extended or bright), 2 = Rice</column>
	[ibe_60] [smallint] NOT NULL, --/ <column>1 for background fit, 0 for none (extended or bright), 2 = Rice</column>
	[ibe_100] [smallint] NOT NULL, --/ <column>1 for background fit, 0 for none (extended or bright), 2 = Rice</column>
	[width2] [real] NOT NULL, --/ <column unit="arcmin^2">best fit width**2 at 60um</column>
	[ewidth] [real] NOT NULL, --/ <column>error on WIDTH2</column>
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