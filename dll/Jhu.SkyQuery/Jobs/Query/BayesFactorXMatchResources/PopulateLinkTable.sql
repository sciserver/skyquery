-- *** BayesFactorXMatchResources/PopulateLinkTable.sql *** ---

DECLARE @nzone int = CONVERT(int, FLOOR(@theta/@h) + 1)

INSERT [$tablename] WITH (TABLOCKX)
SELECT [Z1].[ZoneID], [Z2].[ZoneID], 
	SkyQuery_Code.dbo.CalculateAlpha(@theta, [D1].[DecMin], [D1].[DecMax], @H),
	SkyQuery_Code.dbo.CalculateAlpha(@theta, [D2].[DecMin], [D2].[DecMax], @H)
FROM       (SELECT DISTINCT [ZoneID] FROM [$zonetable1]) AS [Z1]
INNER JOIN (SELECT DISTINCT [ZoneID] FROM [$zonetable2]) AS [Z2]
      ON [Z2].[ZoneID] BETWEEN [Z1].[ZoneID] - @nzone AND [Z1].[ZoneID] + @nzone
INNER JOIN [$zonedeftable] AS [D1] ON [D1].[ZoneID] = [Z1].[ZoneID]
INNER JOIN [$zonedeftable] AS [D2] ON [D2].[ZoneID] = [Z2].[ZoneID]
ORDER BY 1, 2