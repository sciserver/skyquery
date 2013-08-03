-- *** XMatchResources/CreateLinkTable.sql *** ---

CREATE TABLE [$tablename]
(
	[ZoneID1] [bigint] NOT NULL,
	[ZoneID2] [int] NOT NULL,
	[Alpha1] [float] NOT NULL,
	[Alpha2] [float] NOT NULL
)

ALTER TABLE [$tablename] ADD CONSTRAINT [$indexname] PRIMARY KEY ( [ZoneID1], [ZoneID2] )