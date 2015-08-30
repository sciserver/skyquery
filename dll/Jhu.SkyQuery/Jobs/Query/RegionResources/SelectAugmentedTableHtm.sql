	SELECT [$columnlist]
	FROM [$codedb].htm.Cover(@r) __htm
	INNER LOOP JOIN [$tablename]
		ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 0
	[$where_inner]

	UNION ALL

	SELECT [$columnlist]
	FROM [$codedb].htm.Cover(@r) __htm
	INNER LOOP JOIN [$tablename]
		ON [$htmid] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 1
	[$where_partial]