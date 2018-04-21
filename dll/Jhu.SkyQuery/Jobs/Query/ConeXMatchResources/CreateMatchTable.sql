-- *** ConeXMatchResources/CreateMatchTable.sql *** ---

CREATE TABLE [$tablename]
(
	   [MatchID] [bigint] NOT NULL IDENTITY ([$idseed], 1),
	   [RA] [float],
	   [Dec] [float],
	   [Cx] [float],
	   [Cy] [float],
	   [Cz] [float],
	   [ZoneID] [int]
	   [$columnlist]
)

ALTER TABLE [$tablename] ADD CONSTRAINT [$indexname] PRIMARY KEY ( [MatchID] )