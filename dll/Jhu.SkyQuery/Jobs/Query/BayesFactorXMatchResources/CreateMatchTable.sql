-- *** BayesFactorXMatchResources/CreateMatchTable.sql *** ---

CREATE TABLE [$tablename]
(
	   [MatchID] [bigint] NOT NULL IDENTITY (1, 1),
	   [RA] [float],
	   [Dec] [float],
	   [Cx] [float],
	   [Cy] [float],
	   [Cz] [float],
	   [n] [smallint],
	   [a] [float],
	   [l] [float],
	   [q] [float],
	   [logBF] [float],
	   [ZoneID] [int]
	   [$columnlist]
)

ALTER TABLE [$tablename] ADD CONSTRAINT [$indexname] PRIMARY KEY ( [MatchID] )