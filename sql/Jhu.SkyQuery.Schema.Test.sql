USE [SkyQuery_Schema_Test]
GO

CREATE TABLE CatalogA
(
	[objid] bigint PRIMARY KEY,
	[ra] float,
	[dec] float,
	[cx] float,
	[cy] float,
	[cz] float,
	[htmid] bigint,
	[zoneid] int,
	[mag_a] float,
	[mag_b] float
)

GO

CREATE TABLE SimpleTable
(
	[ID] bigint,
	[Data] float,
)

GO