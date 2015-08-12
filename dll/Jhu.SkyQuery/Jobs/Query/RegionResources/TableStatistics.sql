-- *** RegionResources/TableStatistics.sql *** ---

-- Generate HTM ranges

DECLARE @r dbo.Region = @region

CREATE TABLE [$htm]
(
	htmIDStart bigint NOT NULL,
	htmIDEnd bigint NOT NULL
);

INSERT [$htm] WITH(TABLOCKX)
SELECT htmIDStart, htmIDEnd
FROM htm.Cover(@region) AS htm;

-- Compute statistics

CREATE TABLE [$temptable]
(
	[rn] bigint PRIMARY KEY,
	[key] [$keytype]
);

INSERT [$temptable] WITH(TABLOCKX)
SELECT ROW_NUMBER() OVER (ORDER BY [$keycol]), [$keycol]
FROM [$tablename]
INNER JOIN [$htm] __htm
	ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd
[$where];

DECLARE @count bigint = @@ROWCOUNT;
DECLARE @step bigint = @count / @bincount;

IF (@step = 0) SET @step = 1;

SELECT [rn], [key]
FROM [$temptable]
WHERE [rn] % @step = 0 OR [rn] = @count
ORDER BY [rn];

DROP TABLE [$temptable];
DROP TABLE [$htm];