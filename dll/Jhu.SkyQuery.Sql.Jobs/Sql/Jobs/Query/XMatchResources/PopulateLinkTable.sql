-- *** XMatchResources/PopulateLinkTable.sql *** ---

-- Calculate overlap
DECLARE @nzone int = CONVERT(int, FLOOR(@theta/@H) + 1)

INSERT [$tablename] WITH (TABLOCKX)
SELECT D1.ZoneID,
	   D2.ZoneID, 
	   skyquery.Alpha(@theta, [D2].[DecMin], [D2].[DecMax], @H)
FROM	   [$zonedeftable] AS [D1]
INNER JOIN [$zonedeftable] AS [D2]
	ON [D2].[ZoneID] BETWEEN [D1].[ZoneID] - @nzone AND [D1].[ZoneID] + @nzone
[$where]