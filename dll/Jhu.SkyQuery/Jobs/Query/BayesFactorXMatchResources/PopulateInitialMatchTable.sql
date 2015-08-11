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
       ([$n] - 1) * 0.69314718055994530941723212145818 AS [logBF],		-- LOG(2)
       CONVERT(INT,FLOOR(([$dec] + 90.0) / @H)) as [ZoneID],
       [$selectcolumnlist]
FROM [$tablename]
[$where]