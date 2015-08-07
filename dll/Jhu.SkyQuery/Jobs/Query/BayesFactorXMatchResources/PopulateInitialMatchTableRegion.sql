-- *** BayesFactorXMatchResources/PopulateInitialMatchTableRegion.sql *** ---

-- Generate HTM ranges

CREATE TABLE [$htm_inner]
(
	htmIDStart bigint NOT NULL,
	htmIDEnd bigint NOT NULL
);

INSERT [$htm_inner] WITH(TABLOCKX)
SELECT htmIDStart, htmIDEnd
FROM htm.Cover(@region) AS htm
WHERE partial = 0;

CREATE TABLE [$htm_partial]
(
	htmIDStart bigint NOT NULL,
	htmIDEnd bigint NOT NULL
);

INSERT [$htm_partial] WITH(TABLOCKX)
SELECT htmIDStart, htmIDEnd
FROM htm.Cover(@region) AS htm
WHERE partial = 1;

-- Execute search

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
FROM [$tablename]
INNER JOIN [$htm_inner] __htm
	ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd
[$where_inner]

UNION ALL

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
FROM [$tablename]
INNER JOIN [$htm_partial] __htm
	ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd
[$where_partial]

ORDER BY [ZoneID], [RA]