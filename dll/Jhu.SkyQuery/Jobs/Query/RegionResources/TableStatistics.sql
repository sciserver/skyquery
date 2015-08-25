-- *** RegionResources/TableStatistics.sql *** ---

DECLARE @r dbo.Region = @region

-- Create temp table to store keys

CREATE TABLE [$temptable]
(
	[rn] bigint PRIMARY KEY,
	[key] [$keytype]
);

WITH __t AS
(
	[$query]
)
INSERT [$temptable] WITH(TABLOCKX)
SELECT ROW_NUMBER() OVER (ORDER BY [$keycol]), [$keycol]
FROM __t;

DECLARE @count bigint = @@ROWCOUNT;
DECLARE @step bigint = @count / @bincount;

IF (@step = 0) SET @step = 1;

SELECT [rn], [key]
FROM [$temptable]
WHERE [rn] % @step = 0 OR [rn] = @count
ORDER BY [rn];

DROP TABLE [$temptable]
