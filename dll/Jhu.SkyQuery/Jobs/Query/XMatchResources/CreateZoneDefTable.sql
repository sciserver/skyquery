-- *** XMatchResources/CreateZoneDefTable.sql *** ---

CREATE TABLE [$tablename]
(
	ZoneID int NOT NULL PRIMARY KEY,
	DecMin float NOT NULL,
	DecMax float NOT NULL
) 

ALTER TABLE [$tablename] ADD CONSTRAINT [$indexname] PRIMARY KEY ( [ZoneID] )