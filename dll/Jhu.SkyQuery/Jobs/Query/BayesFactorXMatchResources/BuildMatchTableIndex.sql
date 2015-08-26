-- *** BayesFactorXMatchResources/BuildMatchTableIndex.sql *** ---

CREATE INDEX [$indexname]
ON [$tablename] ( ZoneID, Ra )
INCLUDE 
(
	[Dec], [Cx], [Cy], [Cz]
	[$columnlist]
)