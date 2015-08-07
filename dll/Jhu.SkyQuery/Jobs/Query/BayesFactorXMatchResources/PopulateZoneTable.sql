-- *** BayesFactorXMatchResources/PopulateZoneTable.sql *** ---

INSERT [$zonetablename] WITH (TABLOCKX)
SELECT CONVERT(INT,FLOOR(([$dec] + 90.0) / @H)) as [ZoneID],
       [$ra] AS [RA],
       [$dec] AS [Dec],
       [$cx] AS [Cx],
       [$cy] AS [Cy],
       [$cz] AS [Cz],
       [$selectcolumnlist]
FROM [$tablename] AS [$tablealias] WITH (NOLOCK)
[$where]
ORDER BY [ZoneID], [RA]

-- Add wrap-around

INSERT [$zonetablename] WITH (TABLOCKX)
SELECT [t].[ZoneID],
       [t].[RA] - 360,
       [t].[Dec],
       [t].[Cx],
       [t].[Cy],
       [t].[Cz],
       [$insertcolumnlist]
FROM [$zonetablename] AS [t]
INNER JOIN [$zonedeftable] [d] ON [d].[ZoneID] = [t].[ZoneID]
WHERE [t].[RA] + [d].[Alpha] > 360


INSERT [$zonetablename] WITH (TABLOCKX)
SELECT [t].[ZoneID],
       [t].[RA] + 360,
       [t].[Dec],
       [t].[Cx],
       [t].[Cy],
       [t].[Cz],
       [$insertcolumnlist]
FROM [$zonetablename] AS [t]
INNER JOIN [$zonedeftable] [d] ON [d].[ZoneID] = [t].[ZoneID]
WHERE [t].[RA] < [d].[Alpha]