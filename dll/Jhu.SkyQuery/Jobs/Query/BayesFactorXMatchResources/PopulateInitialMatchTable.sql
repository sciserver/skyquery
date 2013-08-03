-- *** BayesFactorXMatchResources/PopulateInitialMatchTable.sql *** ---

INSERT [$newtablename] WITH (TABLOCKX)
       ([RA], [Dec], [Cx], [Cy], [Cz], [a], [l], [q], [logBF], [ZoneID], [$insertcolumnlist])
SELECT [$ra] AS [RA],
       [$dec] AS [Dec],
       [$cx] AS [Cx],
       [$cy] AS [Cy],
       [$cz] AS [Cz],
       [$weight] AS [a],
       LOG([$weight]) AS [l],
       0 AS [q],
       ([$n] - 1) * LOG(2) AS [logBF],
       CONVERT(INT,FLOOR(([$dec] + 90.0) / @H)) as [ZoneID],
       [$selectcolumnlist]
FROM [$tablename] AS [$tablealias] WITH (NOLOCK)
[$where]