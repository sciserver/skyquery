USE [SkyNode_NVSS]
GO

CREATE TABLE [dbo].[PhotoObj](
--/ <summary>The main PhotoObj table for the NVSS catalog</summary>
--/ <remarks>The main PhotoObj table for the NVSS catalog</remarks>
	[objID] [bigint] NOT NULL, --/ <column>unique object identifier</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">J2000 right ascension</column>
	[raErr] [float] NOT NULL, --/ <column unit="deg">Estimate of J2000 right ascension standard deviation</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">J2000 declination</column>
	[decErr] [float] NOT NULL, --/ <column unit="deg">Estimate of J2000 declination standard deviation</column>
	[flux] [real] NOT NULL, --/ <column unit="mJy">Strength of the source</column>
	[fluxErr] [real] NOT NULL, --/ <column unit="mJy">Standard deviation estimate of the flux</column>
	[major] [real] NOT NULL, --/ <column>Major axis size of the source</column>
	[majorErr] [real] NULL, --/ <column>Estimated standard deviation of the major axis size (nulls allowed)</column>
	[minor] [real] NOT NULL, --/ <column>Minor axis size of the source</column>
	[minorErr] [real] NULL, --/ <column>Estimated standard deviation of the minor axis size (nulls allowed)</column>
	[pa] [real] NULL, --/ <column unit="deg">Orientation of the major axis on the sky (from N through E) (nulls allowed)</column>
	[paErr] [real] NULL, --/ <column unit="deg">Standard deviation estimate of Pa</column>
	[res] [varchar](1) NULL, --/ <column>Code indicating structure more complex than can be fitted by the Gaussian model(P=peak, R=RMS, S=integrated) (nulls allowed)</column>
	[resOff] [smallint] NULL, --/ <column unit="100s of mJy">offending value (nulls allowed)</column>
	[p_flux] [real] NULL, --/ <column unit="mJy">Integrated linearly polarized flux density (nulls allowed)</column>
	[p_fluxErr] [real] NULL, --/ <column unit="mJy">Standard deviation estimate of the integrated linearly polarized flux density (nulls allowed)</column>
	[p_ang] [real] NULL, --/ <column unit="deg">Position angle of the "E" vectors on the sky if the source was detected in linear polarization (nulls allowed)</column>
	[p_angErr] [real] NULL, --/ <column unit="deg">Standard deviation estimate of p_ang (nulls allowed)</column>
	[field] [varchar](8) NOT NULL, --/ <column>Name of the original survey image field</column>
	[x_pix] [real] NOT NULL, --/ <column>X(Ra) pixel numbers of the center of the component</column>
	[y_pix] [real] NOT NULL, --/ <column>Y(Dec) pixel numbers of the center of the component</column>
  [htmid] [bigint] NOT NULL, --/ <column>htmid for spatial searches</column>
  [cx] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
	[cy] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
	[cz] [float] NOT NULL, --/ <column>cartesian x coordinate</column>
) ON [PRIMARY]

/****** Object:  Index [pk_PhotoObj]    Script Date: 07/26/2013 15:29:32 ******/
ALTER TABLE [dbo].[PhotoObj] ADD  CONSTRAINT [pk_PhotoObj] PRIMARY KEY CLUSTERED 
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