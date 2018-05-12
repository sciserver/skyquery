	SELECT [$zoneid] AS [ZoneID],
	       [$ra] AS [RA],
	       [$dec] AS [Dec],
	       [$cx] AS [Cx],
	       [$cy] AS [Cy],
	       [$cz] AS [Cz],
	       [$htmid] AS [HtmID],
		   [$radius] AS [radius]
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
		   [$radius] AS [radius]
	       [$columnlist]
	FROM htm.Cover(@r) __htm
	INNER LOOP JOIN [$tablename]
		ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 1
	[$where_partial]