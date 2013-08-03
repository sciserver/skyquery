USE [SkyNode_DLS]
GO

CREATE TABLE [dbo].[PhotoObj](
--/ <summary> The main PhotoObj table for the Deep Lens Survey catalog </summary>
--/ <remarks> The main PhotoObj table for the DLS catalog </remarks>
	[objid] [bigint] NOT NULL, --/ <column>primary key</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">master right ascension</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">master declination</column>
	[band] [char](1) NOT NULL, --/ <column>master coordinate origin</column>
	[AlphaB] [float] NULL, --/ <column unit="deg">RA in B</column>
	[DeltaB] [float] NULL, --/ <column unit="deg">DEC in B</column>
	[XB] [float] NULL, --/ <column unit="pix">position on CCD in B</column>
	[YB] [float] NULL, --/ <column unit="pix">position on CCD in B</column>
	[AB] [real] NULL, --/ <column unit="pix">Semi-major axis in B</column>
	[BB] [real] NULL, --/ <column unit="pix">Semi-minor axis in B</column>
	[THETAB] [real] NULL, --/ <column unit="deg">Position angle</column>
	[FLAGSB] [int] NULL, --/ <column>Flags from Sextractor</column>
	[MAG_APERB] [real] NULL, --/ <column unit="mag">Aperture magnitude in B</column>
	[MAGERR_APERB] [real] NULL, --/ <column unit="mag">error in aperture magnitude in B</column>
	[MAGB] [real] NULL, --/ <column unit="mag">Best magnitude in B</column>
	[MAGERRB] [real] NULL, --/ <column unit="mag">Error in best magnitude in B</column>
	[MAG_ISOB] [real] NULL, --/ <column unit="mag">Isophotal magnitude in B</column>
	[MAGERR_ISOB] [real] NULL, --/ <column unit="mag">Error in isophotal magnitude in B</column>
	[ISOAREAB] [real] NULL, --/ <column unit="pix^2">Isophotal area</column>
	[CLASS_STARB] [real] NULL, --/ <column>Fuzzy classifier</column>
	[AlphaV] [float] NULL, --/ <column unit="deg">RA in V</column>
	[DeltaV] [float] NULL, --/ <column unit="deg">DEC in V</column>
	[XV] [float] NULL, --/ <column unit="pix">position on CCD in V</column>
	[YV] [float] NULL, --/ <column unit="pix">position on CCD in V</column>
	[AV] [real] NULL, --/ <column unit="pix">Semi-major axis in V</column>
	[BV] [real] NULL, --/ <column unit="pix">Semi-minor axis in V</column>
	[THETAV] [real] NULL, --/ <column unit="deg">Position angle in V</column>
	[FLAGSV] [int] NULL, --/ <column>Flags from Sextractor</column>
	[MAG_APERV] [real] NULL, --/ <column unit="mag">Aperture magnitude in V</column>
	[MAGERR_APERV] [real] NULL, --/ <column unit="mag">error in aperture magnitude in V</column>
	[MAGV] [real] NULL, --/ <column unit="mag">Best magnitude in V</column>
	[MAGERRV] [real] NULL, --/ <column unit="mag">Error in best magnitude in V</column>
	[MAG_ISOV] [real] NULL, --/ <column unit="mag">Isophotal magnitude in V</column>
	[MAGERR_ISOV] [real] NULL, --/ <column unit="mag">Error in isophotal magnitude in V</column>
	[ISOAREAV] [real] NULL, --/ <column unit="pix^2">Isophotal area</column>
	[CLASS_STARV] [real] NULL, --/ <column>Fuzzy classifier</column>
	[AlphaR] [float] NULL, --/ <column unit="deg">RA in R</column>
	[DeltaR] [float] NULL, --/ <column unit="deg">DEC in R</column>
	[XR] [float] NULL, --/ <column unit="pix">position on CCD in R</column>
	[YR] [float] NULL, --/ <column unit="pix">position on CCD in R</column>
	[AR] [real] NULL, --/ <column unit="pix">Semi-major axis in R</column>
	[BR] [real] NULL, --/ <column unit="pix">Semi-minor axis in R</column>
	[THETAR] [real] NULL, --/ <column unit="deg">Position angle in R</column>
	[FLAGSR] [int] NULL, --/ <column>Flags from Sextractor</column>
	[MAG_APERR] [real] NULL, --/ <column unit="mag">Aperture magnitude in R</column>
	[MAGERR_APERR] [real] NULL, --/ <column unit="mag">error in aperture magnitude in R</column>
	[MAGR] [real] NULL, --/ <column unit="mag">Best magnitude in R</column>
	[MAGERRR] [real] NULL, --/ <column unit="mag">Error in best magnitude in R</column>
	[MAG_ISOR] [real] NULL, --/ <column unit="mag">Isophotal magnitude in R</column>
	[MAGERR_ISOR] [real] NULL, --/ <column unit="mag">Error in isophotal magnitude in R</column>
	[ISOAREAR] [real] NULL, --/ <column unit="pix^2">Isophotal area</column>
	[CLASS_STARR] [real] NULL, --/ <column>Fuzzy classifier</column>
	[Alphaz] [float] NULL, --/ <column unit="deg">RA in z</column>
	[Deltaz] [float] NULL, --/ <column unit="deg">DEC in z</column>
	[Xz] [float] NULL, --/ <column unit="pix">position on CCD in z</column>
	[Yz] [float] NULL, --/ <column unit="pix">position on CCD in z</column>
	[Az] [real] NULL, --/ <column unit="pix">Semi-major axis in z</column>
	[Bz] [real] NULL, --/ <column unit="pix">Semi-minor axis in z</column>
	[THETAz] [real] NULL, --/ <column unit="deg">Position angle in z</column>
	[FLAGSz] [int] NULL, --/ <column>Flags from Sextractor</column>
	[MAG_APERz] [real] NULL, --/ <column unit="mag">Aperture magnitude in z</column>
	[MAGERR_APERz] [real] NULL, --/ <column unit="mag">error in aperture magnitude in z</column>
	[MAGz] [real] NULL, --/ <column unit="mag">Best magnitude in z</column>
	[MAGERRz] [real] NULL, --/ <column unit="mag">Error in best magnitude in z</column>
	[MAG_ISOz] [real] NULL, --/ <column unit="mag">Isophotal magnitude in z</column>
	[MAGERR_ISOz] [real] NULL, --/ <column unit="mag">Error in isophotal magnitude in z</column>
	[ISOAREAz] [real] NULL, --/ <column unit="pix^2">Isophotal area</column>
	[CLASS_STARz] [real] NULL, --/ <column>Fuzzy classifier</column>
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

/*
CREATE TABLE [dbo].[Frame](
--/ <summary> Table to store precomputed JPEG images </summary>
--/ <remarks> Images are stored with their WCS.   </remarks>
	[fieldid] [bigint] NOT NULL, --/ <column>unique identifier of the field</column>
	[name] [varchar](16) NOT NULL, --/ <column>field name</column>
	[raCen] [float] NOT NULL, --/ <column unit="deg">ra of center</column>
	[decCen] [float] NOT NULL, --/ <column unit="deg">dec of center</column>
	[crpix1] [float] NOT NULL, --/ <column unit="pix">pixel location of center</column>
	[crpix2] [float] NOT NULL, --/ <column unit="pix">pixel location of center</column>
	[cd1_1] [float] NOT NULL, --/ <column unit="deg/pix">element of WCS CD matrix</column>
	[cd1_2] [float] NOT NULL, --/ <column unit="deg/pix">element of WCS CD matrix</column>
	[cd2_1] [float] NOT NULL, --/ <column unit="deg/pix">element of WCS CD matrix</column>
	[cd2_2] [float] NOT NULL, --/ <column unit="deg/pix">element of WCS CD matrix</column>
	[zoom] [int] NOT NULL, --/ <column>log2 of zoom factor</column>
	[width] [int] NOT NULL, --/ <column unit="pix">width of image</column>
	[height] [int] NOT NULL, --/ <column unit="pix">height of image</column>
	[scale] [float] NOT NULL, --/ <column unit="arcsec/pix">pixel scale</column>
	[htmid] [bigint] NOT NULL, --/ <column>htmid for spatial searches</column>
	[cx] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
	[cy] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
	[cz] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
	[img] [image] NOT NULL --/ <column>the binary blob of the image</column>
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
*/