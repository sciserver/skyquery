-- *** XMatchResources/PopulateZoneTable.sql *** ---

INSERT [$zonetablename] WITH (TABLOCKX)
SELECT CONVERT(INT,FLOOR((([$dec]) + 90.0) / @H)) as [ZoneID],
		[$ra] AS [RA],
		[$dec] AS [Dec],
		[$cx] AS [Cx],
		[$cy] AS [Cy],
		[$cz] AS [Cz],
		[$selectcolumnlist]
FROM [$tablename]
[$where]