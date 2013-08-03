-- *** BayesFactorXMatchResources/PopulatePairTable.sql *** ---

DECLARE @dist2 float = 4 * POWER(SIN(RADIANS(@theta/2)), 2);
--
INSERT [$pairtable] WITH (TABLOCKX)
SELECT	[$columnlist1],
		[$columnlist2],
		[tableB].[Cx] - [tableA].[Cx],
		[tableB].[Cy] - [tableA].[Cy],
		[tableB].[Cz] - [tableA].[Cz]
FROM [$matchzonetable] AS [tableA]
INNER LOOP JOIN [$linktable] AS [linktable] ON [linktable].ZoneID1 = [tableA].ZoneID
INNER LOOP JOIN [$zonetable] AS [tableB] ON [linktable].ZoneID2 = [tableB].ZoneID
		AND [tableB].[RA] BETWEEN [tableA].[RA] - [linktable].[Alpha2] AND [tableA].[RA] + [linktable].[Alpha2]
		AND [tableB].[Dec] BETWEEN [tableA].[Dec] - @theta AND [tableA].[Dec] + @theta 
		AND ([tableA].[RA] >= 0 OR [tableB].[RA] >= 0 ) -- correcting wraparound 
WHERE ([tableA].[Cx] - [tableB].[Cx]) * ([tableA].[Cx] - [tableB].[Cx])
	+ ([tableA].[Cy] - [tableB].[Cy]) * ([tableA].[Cy] - [tableB].[Cy])
	+ ([tableA].[Cz] - [tableB].[Cz]) * ([tableA].[Cz] - [tableB].[Cz]) < @dist2