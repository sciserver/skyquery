-- *** XMatchResources/CreateLinkTable.sql *** ---

CREATE TABLE [$tablename]
(
	[ZoneID1] [int] NOT NULL,
	[ZoneID2] [int] NOT NULL,
	[Alpha2] [float] NOT NULL
)

ALTER TABLE [$tablename] ADD CONSTRAINT [$indexname] PRIMARY KEY ( [ZoneID1], [ZoneID2] )