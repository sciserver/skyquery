-- *** RegionResources/ExecuteQuery.sql *** ---

DECLARE @r dbo.Region = @region;

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


[$select]



-- Clean up

DROP TABLE [$htm_inner];
DROP TABLE [$htm_partial];