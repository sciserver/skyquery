-- *** XMatchResources/CreateZoneTable.sql *** ---

CREATE TABLE [$tablename]
(
	[ZoneID] int NOT NULL,
	[RA] float NOT NULL,
	[Dec] float NOT NULL,
	[Cx] float NOT NULL,
	[Cy] float NOT NULL,
	[Cz] float NOT NULL,
	[$columnlist]
)

CREATE CLUSTERED INDEX [$indexname] ON [$tablename] ([ZoneID], [RA])