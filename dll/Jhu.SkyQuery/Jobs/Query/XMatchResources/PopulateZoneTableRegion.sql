-- *** BayesFactorXMatchResources/PopulateZoneTableRegion.sql *** ---

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

INSERT [$zonetablename] WITH (TABLOCKX)
SELECT CONVERT(INT,FLOOR(([$dec] + 90.0) / @H)) as [ZoneID],
       [$ra] AS [RA],
       [$dec] AS [Dec],
       [$cx] AS [Cx],
       [$cy] AS [Cy],
       [$cz] AS [Cz],
       [$selectcolumnlist]
FROM [$tablename]
INNER JOIN [$htm_inner] __htm
	ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd
[$where_inner]

UNION ALL

SELECT CONVERT(INT,FLOOR(([$dec] + 90.0) / @H)) as [ZoneID],
       [$ra] AS [RA],
       [$dec] AS [Dec],
       [$cx] AS [Cx],
       [$cy] AS [Cy],
       [$cz] AS [Cz],
       [$selectcolumnlist]
FROM [$tablename]
INNER JOIN [$htm_partial] __htm
	ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd
[$where_partial]
