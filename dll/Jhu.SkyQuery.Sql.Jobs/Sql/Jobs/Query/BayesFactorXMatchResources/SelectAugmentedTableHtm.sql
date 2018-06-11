	SELECT [$zoneid] AS [ZoneID],
	       [$ra] AS [RA],
	       [$dec] AS [Dec],
	       [$cx] AS [Cx],
	       [$cy] AS [Cy],
	       [$cz] AS [Cz],
	       [$htmid] AS [HtmID],
	       CAST(1 AS smallint) AS [n],
	       [$weight] AS [a],
	       LOG([$weight]) AS [l],
	       CAST(0 AS float) AS [q],
	       CAST(0 AS float) AS [logBF]
	       [$columnlist]
	FROM htm.Cover(@r) __htm
	INNER LOOP JOIN [$tablename]
		ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 0
	[$where_inner]

	UNION ALL

	SELECT [$zoneid] AS [ZoneID],
	       [$ra] AS [RA],
	       [$dec] AS [Dec],
	       [$cx] AS [Cx],
	       [$cy] AS [Cy],
	       [$cz] AS [Cz],
	       [$htmid] AS [HtmID],
	       CAST(1 AS smallint) AS [n],
	       [$weight] AS [a],
	       LOG([$weight]) AS [l],
	       CAST(0 AS float) AS [q],
	       CAST(0 AS float) AS [logBF]
	       [$columnlist]
	FROM htm.Cover(@r) __htm
	INNER LOOP JOIN [$tablename]
		ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 1
	[$where_partial]