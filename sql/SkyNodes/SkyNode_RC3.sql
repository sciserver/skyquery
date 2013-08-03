USE [SkyNode_RC3]
GO

CREATE TABLE [dbo].[PhotoObj](
--/ <summary> The main PhotoObj table for the RC3 catalog </summary>
--/ <remarks> The main PhotoObj table for the RC3 catalog </remarks>
	[name] [varchar](20) NOT NULL, --/ <column content="ID_AREA">Primary name</column>
	[aliases] [varchar](20) NULL, --/ <column>Other names</column>
	[PGC_name] [varchar](10) NULL, --/ <column>PGC (Paturel et al. 1989a,b) designation</column>
	[ra] [float] NOT NULL, --/ <column unit="degrees" content="POS_EQ_RA">Right Ascension (J2000)</column>
	[dec] [float] NOT NULL, --/ <column unit="degrees" content="POS_EQ_DEC">Declination (J2000)</column>
	[good_position] [int] NOT NULL, --/ <column>The values of ra/dec are good to 0.1 sec time, 1 asec (otherwise, 0.1 min time, 1 amin)</column>
	[galactic_l] [float] NULL, --/ <column unit="degrees" content="POS_EQ_RA">Galactic longitude in the IAU 1958 system (Blaauw et al. 1960); good to 0.01 degrees</column>
	[galactic_b] [float] NULL, --/ <column unit="degrees" content="POS_EQ_DEC">Galactic latitude in the IAU 1958 system (Blaauw et al. 1960); good to 0.01 degrees</column>
	[superGalactic_l] [float] NULL, --/ <column unit="degrees" content="POS_EQ_RA">Supergalactic longitude in the RC2 system; good to 0.01 degrees</column>
	[superGalactic_b] [float] NULL, --/ <column unit="degrees" content="POS_EQ_DEC">Supergalactic latitude in the RC2 system; good to 0.01 degrees</column>
	[PosAngle] [float] NULL, --/ <column unit="degrees">position angle, measured in degrees from north through east (all p.a. &lt; 180 degrees), taken when available from UGC, ESO, and ESGC (and in a few cases from HI data).</column>
	[rc2_type] [char](7) NULL, --/ <column>mean revised morphological type in the RC2 system, coded as in RC2</column>
	[rc2_typeSource] [char](6) NULL, --/ <column>Sources of revised morphological type estimate</column>
	[T_type] [float] NULL, --/ <column>Mean numerical index of stage along the Hubble sequence in the RC2 system</column>
	[T_typeErr] [float] NULL, --/ <column>Error in T_type</column>
	[LC_rc2] [float] NULL, --/ <column>Mean numerical luminosity class in the RC2 system</column>
	[LC_rc2Err] [float] NULL, --/ <column>Error in LC_rc2</column>
	[n_L] [float] NULL, --/ <column>Number of luminosity class estimates</column>
	[B_T] [float] NULL, --/ <column unit="magnitudes">Total (asymptotic) magnitude in the B system, derived by extrapolation from photoelectric aperture-magnitude data, B_T^A, and from surface photometry with photoelectric zero point, B_T^S</column>
	[B_TErr] [float] NULL, --/ <column unit="magnitudes">Error in B_T</column>
	[B_Tsource] [char](1) NULL, --/ <column>M when B_T is the weighted mean of B_T^A and B_T^S; V when B_T is a V-band magnitude rather than a B-band magnitude; v when the nucleus of the galaxy is variable; * when deriving B_T^A would have required an extrapolation in excess of 0.75 mag.</column>
	[B_T0] [float] NULL, --/ <column unit="magnitudes">total ``face-on'' magnitude corrected for Galactic and internal extinction, and for redshift</column>
	[M_B] [float] NULL, --/ <column unit="magnitudes">Photographic magnitude from Ames (1930), Shapley and Ames (1932), CGCG, Buta and Corwin (1986), and/or Lauberts and Valentijn (1989) reduced to the B_T system</column>
	[M_BErr] [float] NULL, --/ <column unit="magnitudes">Error in M_B</column>
	[M_FIR] [float] NULL, --/ <column unit="magnitudes">Far-infrared magnitude calculated from M_FIR = - 20.0 - 2.5 log_10(FIR), where FIR is the far infrared continuum flux measured at 60 and 100 microns as listed in the IRAS Point Source Catalog (1987). For galaxies larger than 8 arcmin in RC2 and for the Virgo cluster area, resolved by the IRAS beam, integrated fluxes are taken from Rice et al. (1988) or Helou et al. (1988)</column>
	[M_21cm] [float] NULL, --/ <column unit="magnitudes">21-cm emission line magnitude, and its mean error, defined by m_21 = 21.6 - 2.5 log(S_H), where S_H is the measured neutral hydrogen flux density in units of 10^-24 W/m^2</column>
	[M_21cmErr] [float] NULL, --/ <column unit="magnitudes">Error in M_21cm</column>
	[BV_T] [float] NULL, --/ <column unit="magnitudes">Total (asymptotic) color index in the Johnson B-V system, derived by extrapolation from photoelectric color-aperture data, and/or from surface photometry with photoelectric zero point</column>
	[BV_TErr] [float] NULL, --/ <column unit="magnitudes">Error in BV_T</column>
	[BV_T0] [float] NULL, --/ <column unit="magnitudes">Total B-V color index corrected for Galactic and internal extinction, and for redshift</column>
	[BV_e] [float] NULL, --/ <column unit="magnitudes">Mean B-V color index within the effective aperture A_e, derived by interpolation from photoelectric color-aperture data</column>
	[BV_eErr] [float] NULL, --/ <column unit="magnitudes">Error in BV_e</column>
	[UB_T] [float] NULL, --/ <column unit="magnitudes">Total (asymptotic) color index in the Johnson U-B system, derived by extrapolation from photoelectric color-aperture data, and/or from surface photometry with photoelectric zero point</column>
	[UB_TErr] [float] NULL, --/ <column unit="magnitudes">Error in UB_T</column>
	[UB_T0] [float] NULL, --/ <column unit="magnitudes">Total U-B color index corrected for Galactic and internal extinction, and for redshift</column>
	[UB_e] [float] NULL, --/ <column unit="magnitudes">Mean U-B color index, and its mean error, within the effective aperture A_e, derived by interpolation from photoelectric color-aperture data</column>
	[UB_eErr] [float] NULL, --/ <column unit="magnitudes">Error in UB_e</column>
	[lgD_25] [float] NULL, --/ <column unit="lg(0.1 arcmin)">mean decimal logarithm of the apparent major isophotal diameter measured at or reduced to surface brightness level mu_B = 25.0 B-mag/arcsec^2</column>
	[lgD_25Err] [float] NULL, --/ <column unit="0.1 arcmin">Error in lgD_25</column>
	[lgD_0] [float] NULL, --/ <column unit="lg(0.1 arcmin)?">decimal logarithm of the isophotal major diameter corrected to ``face-on'' (inclination = 0 degrees), and corrected for Galactic extinction to A_g = 0, but not for redshift</column>
	[lgR_25] [float] NULL, --/ <column>Mean decimal logarithm of the ratio of the major isophotal diameter, D_25, to the minor isophotal diameter, d_25, measured at or reduced to the surface brightness level mu_B = 25.0 B-mag/arcsec^2</column>
	[lgR_25Err] [float] NULL, --/ <column>Error in lgR_25</column>
	[logA_e] [float] NULL, --/ <column>Decimal logarithm of the apparent diameter (in 0.1 arcmin) of the ``effective aperture,'' the circle centered on the nucleus within which one-half of the total B-band flux is emitted</column>
	[logA_eErr] [float] NULL, --/ <column>Error in logA_e</column>
	[m_e] [float] NULL, --/ <column unit="magnitudes">mean B-band surface brightness in magnitudes per square arcmin (B-mag/arcmin^2) within the effective aperture A_e, as given by the relation m'_e = B_T + 0.75 + 5logA_e - 5.26.  m'_e is statistically related to the effective mean surface brightness, mu'_e (RC2, p. 31; Olson and de Vaucouleurs 1981), with which it coincides when log R = 0 (i = 0 degrees)</column>
	[m_eErr] [float] NULL, --/ <column unit="magnitudes">Error in m_e</column>
	[m_25] [float] NULL, --/ <column unit="magnitudes/arcmin^2">m'_25 = the mean surface brightness in B-mag/arcmin^2 within the mu_B = 25.0 B-mag/arcsec^2 elliptical isophote of major axis D_25 and axis ratio R_25, defined as in RC2 (Equation 21) by:  m'_25 = B_T + Delta m_25 + 5 logD_25 - 2.5 logR_25 - - 5.26, where Delta m_25 = 2.5 log(L_T/L_D_25) = B_25 - B_T is the magnitude increment contributed by the outer regions of a galaxy fainter than mu_B = 25.0 B-mag/arcsec^2</column>
	[m_25Err] [float] NULL, --/ <column unit="magnitudes/arcmin^2">Error in m_25</column>
	[A_B] [float] NULL, --/ <column unit="magnitudes">Galactic extinction in B-band magnitudes, calculated following Burstein and Heiles (1978a,b, 1982, 1984)</column>
	[A_HI_self] [float] NULL, --/ <column unit="magnitudes">HI line self-absorption in magnitudes (for correction to face-on), calculated from logR and T >= 1</column>
	[A_B_int] [float] NULL, --/ <column unit="magnitudes">internal extinction in B-band magnitudes (for correction to face-on), calculated from logR and T</column>
	[W_20] [float] NULL, --/ <column unit="km/s">Neutral hydrogen line full width measured at the 20% level (I_20/I_max)</column>
	[W_20Err] [float] NULL, --/ <column unit="km/s">Error in W_20</column>
	[W_50] [float] NULL, --/ <column unit="km/s">Neutral hydrogen line full width measured at the 50% level (I_50/I_max)</column>
	[W_50Err] [float] NULL, --/ <column unit="km/s">Error in W_50</column>
	[HI] [float] NULL, --/ <column unit="magnitudes">corrected neutral hydrogen index, which is the difference m_21^0 - B_T^0 between the corrected (face-on) 21-cm emission line magnitude and the similarly corrected magnitude in the B_T system. N.n. Since m_21 and B_T are provided in this table, there is no need to list the uncorrected index.</column>
	[V_21cm] [float] NULL, --/ <column unit="km/s">Mean heliocentric radial velocity, and its mean error, in km/s derived from neutral hydrogen observations</column>
	[V_21cmErr] [float] NULL, --/ <column unit="km/s">Error in V_21cm</column>
	[V_opt] [float] NULL, --/ <column unit="km/s">mean heliocentric radial velocity, and its mean error, in km/s derived from optical observations</column>
	[V_optErr] [float] NULL, --/ <column unit="km/s">Error in V_opt</column>
	[V_GSR] [float] NULL, --/ <column unit="km/s">weighted mean of the neutral hydrogen and optical velocities, corrected to the ``Galactic standard of rest</column>
	[V_3K] [float] NULL, --/ <column unit="km/s">weighted mean velocity corrected to the reference frame defined by the 3 K microwave background radiation</column>
	[objId] [bigint] NOT NULL, --/ <column content="ID_MAIN">the main primary key</column>
  [htmid] [bigint] NOT NULL, --/ <column>htmid for spatial searches</column>
  [cx] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
	[cy] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
	[cz] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
) ON [PRIMARY]


ALTER TABLE [dbo].[PhotoObj] ADD  CONSTRAINT [PK__PhotoObjAll__3A81B327] PRIMARY KEY CLUSTERED 
(
	[objId] ASC
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