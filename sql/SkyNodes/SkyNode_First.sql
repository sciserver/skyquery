USE [SkyNode_First]
GO

--/ <summary>The main PhotoObj table for the First catalog</summary>
--/ <remarks>The main PhotoObj table for the First catalog</remarks>
CREATE TABLE [dbo].[PhotoObj](

	--/ <summary>unique object identifier</summary>
	--/ <quantity>meta.id</quantity>
	--/ <class>id</class>
	[objID] [bigint] NOT NULL, 

	--/ <summary>J2000 right ascension</summary>
	--/ <quantity>pos.eq.ra</quantity>
	--/ <unit>deg</unit>
	--/ <class>point.ra</class>
	[ra] [float] NOT NULL, 

	--/ <summary>J2000 declination</summary>
	--/ <quantity>pos.eq.dec</quantity>
	--/ <unit>deg</unit>
	--/ <class>point.dec</class>
	[dec] [float] NOT NULL, 

	--/ <summary>W warning flag</summary>
	--/ <quantity></quantity>
	[w] [char](1) NOT NULL, 

	--/ <summary>peak flux density</summary>
	--/ <quantity>em.radio, stat.max</quantity>
	--/ <unit>mJy</unit>
	[fpeak] [real] NOT NULL,
	
	--/ <summary>integrated flux density</summary> 
	--/ <quantity>em.radio</quantity>
	--/ <unit>mJy</unit>
	[fint] [real] NOT NULL,
	
	--/ <summary>local rms noise estimate</summary> 
	--/ <quantity>em.radio, stat.error</quantity>
	--/ <unit>mJy</unit>
	[rms] [real] NOT NULL, 

	--/ <summary>major axis raw</summary>
	--/ <quantity>phys.size.smajAxis</quantity>
	--/ <unit>arcsec</unit>
	[maj] [real] NOT NULL, 

	--/ <summary>minor axis raw</summary>
	--/ <quantity>phys.size.sminAxis</quantity>
	--/ <unit>arcsec</unit>
	[min] [real] NOT NULL,
	
	--/ <summary>position angle raw</summary> 
	--/ <quantity>pos.posAng</quantity>
	--/ <unit>deg</unit>
	[pa] [real] NOT NULL, 

	--/ <summary>major axis (fitted)</summary>
	--/ <quantity>phys.size.smajAxis, stat.fit</quantity>
	--/ <unit>arcsec</unit>
	[fmaj] [real] NOT NULL, 

	--/ <summary>minor axis (fitted)</summary>
	--/ <quantity>phys.size.sminAxis, stat.fit</quantity>
	--/ <unit>arcsec</unit>
	[fmin] [real] NOT NULL,
	
	--/ <summary>position angle (fitted)</summary> 
	--/ <quantity>pos.posAng, stat.fit</quantity>
	--/ <unit>arcsec</unit>
	[fpa] [real] NOT NULL, 

	--/ <summary>name of the coadded image containing the source</summary>
	[fieldname] [varchar](32) NOT NULL, 

	--/ <summary>htmid for spatial searches</summary>
	--/ <quantity>pos, meta.id</quantity>
	--/ <class>point.htmid</class>
    [htmid] [bigint] NOT NULL, 

	--/ <summary>cartesian x coordinate</summary>
	--/ <quantity>pos.cartesian.x</quantity>
	--/ <class>point.x</class>
    [cx] [float] NOT NULL, 

	--/ <summary>cartesian x coordinate</summary>
	--/ <quantity>pos.cartesian.y</quantity>
	--/ <class>point.y</class>
	[cy] [float] NOT NULL, 

	--/ <summary>cartesian x coordinate</summary>
	--/ <quantity>pos.cartesian.z</quantity>
	--/ <class>point.z</class>
	[cz] [float] NOT NULL, 
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