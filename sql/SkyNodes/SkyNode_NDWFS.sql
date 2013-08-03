USE [SkyNode_NDWFS]
GO

CREATE TABLE [dbo].[PhotoObj](
--/ <summary> The main Photo table for the NOAO Deep Field catalog containing the individual band detections </summary>
--/ <remarks> The main Photo table for the NOAO Deep Field catalog containing detections in   the individual bands. The merged catalog  will be located in the PhotoObjAll table </remarks>
	[OBJID] [bigint] NOT NULL, --/ <column>unique object id, hashed from BAND, FIELD and NUMBER</column>
	[BAND] [varchar](4) NOT NULL, --/ <column>Designates filter</column>
	[FIELD] [varchar](16) NOT NULL, --/ <column>Field identifier</column>
	[X_IMAGE] [real] NULL, --/ <column unit="pixel">Object position along x</column>
	[Y_IMAGE] [real] NULL, --/ <column unit="pixel">Object position along y</column>
	[XPEAK_IMAGE] [bigint] NULL, --/ <column unit="pixel">x-coordinate of the brightest pixel</column>
	[YPEAK_IMAGE] [bigint] NULL, --/ <column unit="pixel">y-coordinate of the brightest pixel</column>
	[NUMBER] [int] NOT NULL, --/ <column>Running object number</column>
	[FLUX_ISO] [real] NULL, --/ <column unit="count">Isophotal flux</column>
	[FLUXERR_ISO] [real] NULL, --/ <column unit="count">RMS error for isophotal flux</column>
	[MAG_ISO] [real] NULL, --/ <column unit="mag">Isophotal magnitude</column>
	[MAGERR_ISO] [real] NULL, --/ <column unit="mag">RMS error for isophotal magnitude</column>
	[MAG_ISOCOR] [real] NULL, --/ <column unit="mag">Corrected isophotal magnitude</column>
	[MAGERR_ISOCOR] [real] NULL, --/ <column unit="mag">RMS error for corrected isophotal magnitude</column>
	[FLUX_APER_01] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_02] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_03] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_04] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_05] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_06] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_07] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_08] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_09] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_10] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_15] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_20] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUXERR_APER_01] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_02] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_03] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_04] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_05] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_06] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_07] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_08] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_09] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_10] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_15] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_20] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[MAG_APER_01] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_02] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_03] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_04] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_05] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_06] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_07] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_08] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_09] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_10] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_15] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_20] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAGERR_APER_01] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_02] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_03] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_04] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_05] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_06] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_07] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_08] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_09] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_10] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_15] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_20] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAG_AUTO] [real] NULL, --/ <column unit="mag">Kron-like elliptical aperture magnitude</column>
	[MAGERR_AUTO] [real] NULL, --/ <column unit="mag">RMS error for AUTO magnitude</column>
	[KRON_RADIUS] [real] NULL, --/ <column>Kron apertures in units of A or B</column>
	[BACKGROUND] [real] NULL, --/ <column unit="count">Background at centroid position</column>
	[THRESHOLD] [real] NULL, --/ <column unit="count">Detection threshold above background</column>
	[FLUX_MAX] [real] NULL, --/ <column unit="count">Peak flux above background</column>
	[ISOAREA_IMAGE] [bigint] NULL, --/ <column unit="pixel**2">Isophotal area above Analysis threshold</column>
	[RA] [float] NOT NULL, --/ <column unit="deg">Right ascension of barycenter (J2000)</column>
	[DEC] [float] NOT NULL, --/ <column unit="deg">Declination of barycenter (J2000)</column>
  [cx] [float] NULL, --/ <column>Cartesian coordinate x</column>
	[cy] [float] NULL, --/ <column>Cartesian coordinate y</column>
	[cz] [float] NULL, --/ <column>Cartesian coordinate z</column>
	[htmid] [bigint] NULL, --/ <column>Unique HTM ID</column>
	[ALPHAPEAK_J2000] [float] NULL, --/ <column unit="deg">Right ascension of brightest pix (J2000)</column>
	[DELTAPEAK_J2000] [float] NULL, --/ <column unit="deg">Declination of brightest pix (J2000)</column>
	[X2_IMAGE] [real] NULL, --/ <column unit="pixel**2">Variance along x</column>
	[Y2_IMAGE] [real] NULL, --/ <column unit="pixel**2">Variance along y</column>
	[XY_IMAGE] [real] NULL, --/ <column unit="pixel**2">Covariance between x and y</column>
	[CXX_IMAGE] [real] NULL, --/ <column unit="pixel**(-2)">Cxx object ellipse parameter</column>
	[CYY_IMAGE] [real] NULL, --/ <column unit="pixel**(-2)">Cyy object ellipse parameter</column>
	[CXY_IMAGE] [real] NULL, --/ <column unit="pixel**(-2)">Cxy object ellipse parameter</column>
	[CXX_WORLD] [real] NULL, --/ <column unit="deg**(-2)">Cxx object ellipse parameter (WORLD units)</column>
	[CYY_WORLD] [real] NULL, --/ <column unit="deg**(-2)">Cyy object ellipse parameter (WORLD units)</column>
	[CXY_WORLD] [real] NULL, --/ <column unit="deg**(-2)">Cxy object ellipse parameter (WORLD units)</column>
	[A_IMAGE] [real] NULL, --/ <column unit="pixel">Profile RMS along major axis</column>
	[B_IMAGE] [real] NULL, --/ <column unit="pixel">Profile RMS along minor axis</column>
	[A_WORLD] [real] NULL, --/ <column unit="deg">Profile RMS along major axis (world units)</column>
	[B_WORLD] [real] NULL, --/ <column unit="deg">Profile RMS along minor axis (world units)</column>
	[THETA_IMAGE] [real] NULL, --/ <column unit="deg">Position angle (CCW/x)</column>
	[THETA_WORLD] [real] NULL, --/ <column unit="deg">Position angle (CCW/world-x)</column>
	[ELONGATION] [real] NULL, --/ <column>A_IMAGE/B_IMAGE</column>
	[ELLIPTICITY] [real] NULL, --/ <column>1 - B_IMAGE/A_IMAGE</column>
	[ERRX2_IMAGE] [real] NULL, --/ <column unit="pixel**2">Variance of position along x</column>
	[ERRY2_IMAGE] [real] NULL, --/ <column unit="pixel**2">Variance of position along y</column>
	[ERRXY_IMAGE] [real] NULL, --/ <column unit="pixel**2">Covariance of position between x and y</column>
	[ERRA_IMAGE] [real] NULL, --/ <column unit="pixel">RMS position error along major axis</column>
	[ERRB_IMAGE] [real] NULL, --/ <column unit="pixel">RMS position error along minor axis</column>
	[ERRTHETA_IMAGE] [real] NULL, --/ <column unit="deg">Error ellipse position angle (CCW/x)</column>
	[FWHM_IMAGE] [real] NULL, --/ <column unit="pixel">FWHM assuming a gaussian core</column>
	[FLAGS] [bigint] NULL, --/ <column>Extraction flags</column>
	[IMAFLAGS_ISO] [bigint] NULL, --/ <column>FLAG-image flags OR'ed over the iso. profile</column>
	[CLASS_STAR] [real] NULL --/ <column>S/G classifier output</column>
) ON [PRIMARY]

ALTER TABLE [dbo].[PhotoObj] ADD PRIMARY KEY CLUSTERED 
(
	[OBJID] ASC
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

CREATE TABLE [dbo].[PhotoObjK](
--/ <summary> The main Photo table for the NOAO Deep Field catalog containing the individual band detections </summary>
--/ <remarks> The main Photo table for the NOAO Deep Field catalog containing detections in   the individual bands. The merged catalog  will be located in the PhotoObjAll table </remarks>
	[OBJID] [bigint] NOT NULL, --/ <column>unique object id, hashed from BAND, FIELD and NUMBER</column>
	[BAND] [varchar](4) NOT NULL, --/ <column>Designates filter</column>
	[FIELD] [varchar](16) NOT NULL, --/ <column>Field identifier</column>
	[X_IMAGE] [real] NULL, --/ <column unit="pixel">Object position along x</column>
	[Y_IMAGE] [real] NULL, --/ <column unit="pixel">Object position along y</column>
	[XPEAK_IMAGE] [bigint] NULL, --/ <column unit="pixel">x-coordinate of the brightest pixel</column>
	[YPEAK_IMAGE] [bigint] NULL, --/ <column unit="pixel">y-coordinate of the brightest pixel</column>
	[NUMBER] [int] NOT NULL, --/ <column>Running object number</column>
	[FLUX_ISO] [real] NULL, --/ <column unit="count">Isophotal flux</column>
	[FLUXERR_ISO] [real] NULL, --/ <column unit="count">RMS error for isophotal flux</column>
	[MAG_ISO] [real] NULL, --/ <column unit="mag">Isophotal magnitude</column>
	[MAGERR_ISO] [real] NULL, --/ <column unit="mag">RMS error for isophotal magnitude</column>
	[MAG_ISOCOR] [real] NULL, --/ <column unit="mag">Corrected isophotal magnitude</column>
	[MAGERR_ISOCOR] [real] NULL, --/ <column unit="mag">RMS error for corrected isophotal magnitude</column>
	[FLUX_APER_01] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_02] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_03] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_04] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_05] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_06] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_07] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_08] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_09] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_10] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_15] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUX_APER_20] [real] NULL, --/ <column unit="count">Flux vector within fixed circular aperture(s)</column>
	[FLUXERR_APER_01] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_02] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_03] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_04] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_05] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_06] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_07] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_08] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_09] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_10] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_15] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[FLUXERR_APER_20] [real] NULL, --/ <column unit="count">RMS error vector for aperture flux(es)</column>
	[MAG_APER_01] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_02] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_03] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_04] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_05] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_06] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_07] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_08] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_09] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_10] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_15] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAG_APER_20] [real] NULL, --/ <column unit="mag">Fixed aperture magnitude vector</column>
	[MAGERR_APER_01] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_02] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_03] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_04] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_05] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_06] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_07] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_08] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_09] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_10] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_15] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAGERR_APER_20] [real] NULL, --/ <column unit="mag">RMS error vector for fixed aperture mag.</column>
	[MAG_AUTO] [real] NULL, --/ <column unit="mag">Kron-like elliptical aperture magnitude</column>
	[MAGERR_AUTO] [real] NULL, --/ <column unit="mag">RMS error for AUTO magnitude</column>
	[KRON_RADIUS] [real] NULL, --/ <column>Kron apertures in units of A or B</column>
	[BACKGROUND] [real] NULL, --/ <column unit="count">Background at centroid position</column>
	[THRESHOLD] [real] NULL, --/ <column unit="count">Detection threshold above background</column>
	[FLUX_MAX] [real] NULL, --/ <column unit="count">Peak flux above background</column>
	[ISOAREA_IMAGE] [bigint] NULL, --/ <column unit="pixel**2">Isophotal area above Analysis threshold</column>
	[RA] [float] NOT NULL, --/ <column unit="deg">Right ascension of barycenter (J2000)</column>
	[DEC] [float] NOT NULL, --/ <column unit="deg">Declination of barycenter (J2000)</column>
  [cx] [float] NULL, --/ <column>Cartesian coordinate x</column>
	[cy] [float] NULL, --/ <column>Cartesian coordinate y</column>
	[cz] [float] NULL, --/ <column>Cartesian coordinate z</column>
	[htmid] [bigint] NULL, --/ <column>Unique HTM ID</column>
	[ALPHAPEAK_J2000] [float] NULL, --/ <column unit="deg">Right ascension of brightest pix (J2000)</column>
	[DELTAPEAK_J2000] [float] NULL, --/ <column unit="deg">Declination of brightest pix (J2000)</column>
	[X2_IMAGE] [real] NULL, --/ <column unit="pixel**2">Variance along x</column>
	[Y2_IMAGE] [real] NULL, --/ <column unit="pixel**2">Variance along y</column>
	[XY_IMAGE] [real] NULL, --/ <column unit="pixel**2">Covariance between x and y</column>
	[CXX_IMAGE] [real] NULL, --/ <column unit="pixel**(-2)">Cxx object ellipse parameter</column>
	[CYY_IMAGE] [real] NULL, --/ <column unit="pixel**(-2)">Cyy object ellipse parameter</column>
	[CXY_IMAGE] [real] NULL, --/ <column unit="pixel**(-2)">Cxy object ellipse parameter</column>
	[CXX_WORLD] [real] NULL, --/ <column unit="deg**(-2)">Cxx object ellipse parameter (WORLD units)</column>
	[CYY_WORLD] [real] NULL, --/ <column unit="deg**(-2)">Cyy object ellipse parameter (WORLD units)</column>
	[CXY_WORLD] [real] NULL, --/ <column unit="deg**(-2)">Cxy object ellipse parameter (WORLD units)</column>
	[A_IMAGE] [real] NULL, --/ <column unit="pixel">Profile RMS along major axis</column>
	[B_IMAGE] [real] NULL, --/ <column unit="pixel">Profile RMS along minor axis</column>
	[A_WORLD] [real] NULL, --/ <column unit="deg">Profile RMS along major axis (world units)</column>
	[B_WORLD] [real] NULL, --/ <column unit="deg">Profile RMS along minor axis (world units)</column>
	[THETA_IMAGE] [real] NULL, --/ <column unit="deg">Position angle (CCW/x)</column>
	[THETA_WORLD] [real] NULL, --/ <column unit="deg">Position angle (CCW/world-x)</column>
	[ELONGATION] [real] NULL, --/ <column>A_IMAGE/B_IMAGE</column>
	[ELLIPTICITY] [real] NULL, --/ <column>1 - B_IMAGE/A_IMAGE</column>
	[ERRX2_IMAGE] [real] NULL, --/ <column unit="pixel**2">Variance of position along x</column>
	[ERRY2_IMAGE] [real] NULL, --/ <column unit="pixel**2">Variance of position along y</column>
	[ERRXY_IMAGE] [real] NULL, --/ <column unit="pixel**2">Covariance of position between x and y</column>
	[ERRA_IMAGE] [real] NULL, --/ <column unit="pixel">RMS position error along major axis</column>
	[ERRB_IMAGE] [real] NULL, --/ <column unit="pixel">RMS position error along minor axis</column>
	[ERRTHETA_IMAGE] [real] NULL, --/ <column unit="deg">Error ellipse position angle (CCW/x)</column>
	[FWHM_IMAGE] [real] NULL, --/ <column unit="pixel">FWHM assuming a gaussian core</column>
	[FLAGS] [bigint] NULL, --/ <column>Extraction flags</column>
	[IMAFLAGS_ISO] [bigint] NULL, --/ <column>FLAG-image flags OR'ed over the iso. profile</column>
	[CLASS_STAR] [real] NULL --/ <column>S/G classifier output</column>
) ON [PRIMARY]


/****** Object:  Index [PK_PhotoObjK]    Script Date: 07/26/2013 15:26:42 ******/
ALTER TABLE [dbo].[PhotoObjK] ADD  CONSTRAINT [PK_PhotoObjK] PRIMARY KEY CLUSTERED 
(
	[OBJID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

-- Index to support on the fly zone table creation
CREATE NONCLUSTERED INDEX [IX_PhotoObjK_Zone] ON [dbo].[PhotoObjK] 
(
	[dec] ASC,
	[ra] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


-- HTM index
CREATE NONCLUSTERED INDEX [IX_PhotoObjK_HtmID] ON [dbo].[PhotoObjK] 
(
	[htmid] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Neighbors](
--/ <summary> All PhotoObjAll pairs within 2 arcseconds  </summary>
--/ <remarks> SDSS objects within 2 arcsec and their match parameters stored here.   The two halves of the pair come from different bands. The first  half of the pair  </remarks>
	[objID] [bigint] NOT NULL, --/ <column content="ID_CATALOG">The unique objId of the center object</column>
	[neighborObjID] [bigint] NULL, --/ <column content="ID_CATALOG">The objId of the neighbor</column>
	[distance] [float] NOT NULL, --/ <column unit="arcmins" content="POS_ANG_DIST_GENERAL">Distance between center and neighbor</column>
	[band] [varchar](3) NOT NULL, --/ <column content="CLASS_OBJECT">Filter band of the center</column>
	[neighborBand] [varchar](3) NULL --/ <column content="CLASS_OBJECT">Filter band of the neighbor</column>
) ON [PRIMARY]


