USE [SkyNode_USNOB]
GO

CREATE TABLE [dbo].[PhotoObj](
--/ <summary> The main photometry table for the USNOB1.0 catalog </summary>
--/ <remarks> We created a bigint objid that is concatenated from  zone and seqNo, with zone in the upper 4 bytes of objid.  The table has been loaded by the ROE group, and has been  genereously shared with the JHU group. </remarks>
	[objid] [bigint] NOT NULL, --/ <column content="ID_MAIN">Main unique object identifier</column>
	[zone] [smallint] NOT NULL, --/ <column content="ID_MAIN:1">The catalog is arranged in zones of 0.1deg in Dec, from 0 in South Pole to 1799 in North Pole</column>
	[seqNo] [int] NOT NULL, --/ <column content="ID_MAIN:2">Sequence number of objects ordered by Right Ascension in the zone.</column>
	[cx] [float] NOT NULL, --/ <column content="POS_EQ_X">unit vector of spherical co-ordinate</column>
	[cy] [float] NOT NULL, --/ <column content="POS_EQ_Y">unit vector of spherical co-ordinate</column>
	[cz] [float] NOT NULL, --/ <column content="POS_EQ_Z">unit vector of spherical co-ordinate</column>
	[htmID] [bigint] NOT NULL, --/ <column content="POS_GENERAL">HTM index, 20 digits, for co-ordinate</column>
	[ra] [float] NOT NULL, --/ <column unit="deg" content="POS_EQ_RA_MAIN">J2000 Celestial Right Ascension</column>
	[dec] [float] NOT NULL, --/ <column unit="deg" content="POS_EQ_DEC_MAIN">J2000 Celestial Declination</column>
	[pmRA] [real] NOT NULL, --/ <column unit="mas/yr" content="POS_EQ_PMRA">Proper motion in RA (relative to YS4.0)</column>
	[pmDEC] [real] NOT NULL, --/ <column unit="mas/yr" content="POS_EQ_PMDEC">Proper motion in DEC (relative to YS4.0)</column>
	[pmPr] [real] NOT NULL, --/ <column content="STAT_PROBABILITY">Total Proper Motion probability</column>
	[mcFlag] [tinyint] NOT NULL, --/ <column content="CODE_MISC">Motion catalogue flag, 0=no, 1=yes</column>
	[e_pmRA] [real] NOT NULL, --/ <column unit="mas/yr" content="ERROR">Mean error on pmRA</column>
	[e_pmDEC] [real] NOT NULL, --/ <column unit="mas/yr" content="ERROR">Mean error on pmDEC</column>
	[e_RAfit] [real] NOT NULL, --/ <column unit="arcsec" content="FIT_ERROR">Mean error on RA fit</column>
	[e_DECfit] [real] NOT NULL, --/ <column unit="arcsec" content="FIT_ERROR">Mean error on DEC fit</column>
	[Ndet] [tinyint] NOT NULL, --/ <column content="NUMBER">Number of detections</column>
	[difSp] [tinyint] NOT NULL, --/ <column content="CODE_MISC">Diffraction spike flag, 0=no, 1=yes</column>
	[e_RA] [real] NOT NULL, --/ <column unit="mas" content="ERROR">Mean error on ra*cos(dec) at Epoch</column>
	[e_DEC] [real] NOT NULL, --/ <column unit="mas" content="ERROR">Mean error on dec at Epoch</column>
	[Epoch] [real] NOT NULL, --/ <column unit="yr" content="TIME_EPOCH">Mean epoch of observation</column>
	[ys40] [tinyint] NOT NULL, --/ <column content="CODE_MISC">YS4.0 correlation flag, 0=no, 1=yes</column>
	[B1Mag] [real] NOT NULL, --/ <column unit="mag" content="PHOT_PHG_B">First blue magnitude</column>
	[B1C] [tinyint] NOT NULL, --/ <column content="INST_CALIB_PARAM">source of photometric calibration</column>
	[B1S] [tinyint] NOT NULL, --/ <column content="ID_SURVEY">Survey number</column>
	[B1F] [smallint] NOT NULL, --/ <column content="ID_FIELD">Field number in survey</column>
	[B1S_G] [tinyint] NOT NULL, --/ <column content="CLASS_STAR/GALAXY">Star-galaxy separation</column>
	[B1Xi] [real] NOT NULL, --/ <column unit="arcsec" content="POS_EQ_RA_OFF">Residual in X direction</column>
	[B1Eta] [real] NOT NULL, --/ <column unit="arcsec" content="POS_EQ_DEC_OFF">Residual in Y direction</column>
	[R1Mag] [real] NOT NULL, --/ <column unit="mag" content="PHOT_PHG_R">First red magnitude</column>
	[R1C] [tinyint] NOT NULL, --/ <column content="INST_CALIB_PARAM">source of photometric calibration</column>
	[R1S] [tinyint] NOT NULL, --/ <column content="ID_SURVEY">Survey number</column>
	[R1F] [smallint] NOT NULL, --/ <column content="ID_FIELD">Field number in survey</column>
	[R1S_G] [tinyint] NOT NULL, --/ <column content="CLASS_STAR/GALAXY">Star-galaxy separation</column>
	[R1Xi] [real] NOT NULL, --/ <column unit="arcsec" content="POS_EQ_RA_OFF">Residual in X direction</column>
	[R1Eta] [real] NOT NULL, --/ <column unit="arcsec" content="POS_EQ_DEC_OFF">Residual in Y direction</column>
	[B2Mag] [real] NOT NULL, --/ <column unit="mag" content="PHOT_PHG_B">Second blue magnitude</column>
	[B2C] [tinyint] NOT NULL, --/ <column content="INST_CALIB_PARAM">source of photometric calibration</column>
	[B2S] [tinyint] NOT NULL, --/ <column content="ID_SURVEY">Survey number</column>
	[B2F] [smallint] NOT NULL, --/ <column content="ID_FIELD">Field number in survey</column>
	[B2S_G] [tinyint] NOT NULL, --/ <column content="CLASS_STAR/GALAXY">Star-galaxy separation</column>
	[B2Xi] [real] NOT NULL, --/ <column unit="arcsec" content="POS_EQ_RA_OFF">Residual in X direction</column>
	[B2Eta] [real] NOT NULL, --/ <column unit="arcsec" content="POS_EQ_DEC_OFF">Residual in Y direction</column>
	[R2Mag] [real] NOT NULL, --/ <column unit="mag" content="PHOT_PHG_R">Second red magnitude</column>
	[R2C] [tinyint] NOT NULL, --/ <column content="INST_CALIB_PARAM">source of photometric calibration</column>
	[R2S] [tinyint] NOT NULL, --/ <column content="ID_SURVEY">Survey number</column>
	[R2F] [smallint] NOT NULL, --/ <column content="ID_FIELD">Field number in survey</column>
	[R2S_G] [tinyint] NOT NULL, --/ <column content="CLASS_STAR/GALAXY">Star-galaxy separation</column>
	[R2Xi] [real] NOT NULL, --/ <column unit="arcsec" content="POS_EQ_RA_OFF">Residual in X direction</column>
	[R2Eta] [real] NOT NULL, --/ <column unit="arcsec" content="POS_EQ_DEC_OFF">Residual in Y direction</column>
	[NMag] [real] NOT NULL, --/ <column unit="mag" content="PHOT_PHG_I">Infrared (N) magnitude</column>
	[NC] [tinyint] NOT NULL, --/ <column content="INST_CALIB_PARAM">source of photometric calibration</column>
	[NS] [tinyint] NOT NULL, --/ <column content="ID_SURVEY">Survey number</column>
	[NF] [smallint] NOT NULL, --/ <column content="ID_FIELD">Field number in survey</column>
	[NS_G] [tinyint] NOT NULL, --/ <column content="CLASS_STAR/GALAXY">Star-galaxy separation</column>
	[NXi] [real] NOT NULL, --/ <column unit="arcses" content="POS_EQ_RA_OFF">Residual in X direction</column>
	[NEta] [real] NOT NULL, --/ <column unit="arcsec" content="POS_EQ_DEC_OFF">Residual in Y direction</column>
	[lbIdxB1] [int] NOT NULL, --/ <column content="CODE_MISC">First blue look-back index into PMM scan file</column>
	[lbIdxR1] [int] NOT NULL, --/ <column content="CODE_MISC">First red look-back index into PMM scan file</column>
	[lbIdxB2] [int] NOT NULL, --/ <column content="CODE_MISC">Second blue look-back index into PMM scan file</column>
	[lbIdxR2] [int] NOT NULL, --/ <column content="CODE_MISC">Second red look-back index into PMM scan file</column>
	[lbIdxN] [int] NOT NULL --/ <column content="CODE_MISC">N look-back index into PMM scan file</column>
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PhotoObj] ADD PRIMARY KEY CLUSTERED 
(
	[objid] ASC
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