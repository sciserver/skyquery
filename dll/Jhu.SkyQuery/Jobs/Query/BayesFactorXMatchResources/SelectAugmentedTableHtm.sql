	SELECT [$ra] AS [RA],
		   [$dec] AS [Dec],
		   [$cx] AS [Cx],
		   [$cy] AS [Cy],
		   [$cz] AS [Cz],
		   [$weight] AS [a],
		   LOG([$weight]) AS [l],
		   0 AS [q],
		   ([$n] - 1) * LOG(2) AS [logBF],
		   [$columnlist]
	FROM [$tablename]
	INNER JOIN [$codedb].htm.Cover(@r) __htm
		ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 0
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
		   [$columnlist]
	FROM [$tablename]
	INNER JOIN [$codedb].htm.Cover(@r) __htm
		ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 1
	[$where_partial]