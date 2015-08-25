	SELECT [$columnlist]
	FROM [$tablename]
	INNER JOIN [$codedb].htm.Cover(@r) __htm
		ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 0
	[$where_inner]

	UNION ALL

	SELECT [$columnlist]
	FROM [$tablename]
	INNER JOIN [$codedb].htm.Cover(@r) __htm
		ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 1
	[$where_partial]