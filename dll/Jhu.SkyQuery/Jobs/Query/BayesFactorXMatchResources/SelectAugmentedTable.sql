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
	[$where]