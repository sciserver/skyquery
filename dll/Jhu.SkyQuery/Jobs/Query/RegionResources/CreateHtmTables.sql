-- *** RegionResources/CreateHtmTables.sql *** ---

DECLARE [$regionudtname] dbo.Region = [$regionparname]

-- Generate HTM ranges

CREATE TABLE [$htm_inner]
(
	htmIDStart bigint NOT NULL,
	htmIDEnd bigint NOT NULL
);

INSERT [$htm_inner] WITH(TABLOCKX)
SELECT htmIDStart, htmIDEnd
FROM htm.Cover([$regionudtname]) AS htm
WHERE partial = 0;

CREATE TABLE [$htm_partial]
(
	htmIDStart bigint NOT NULL,
	htmIDEnd bigint NOT NULL
);

INSERT [$htm_partial] WITH(TABLOCKX)
SELECT htmIDStart, htmIDEnd
FROM htm.Cover([$regionudtname]) AS htm
WHERE partial = 1;