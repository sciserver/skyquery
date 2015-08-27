	SELECT [$zoneid] AS [ZoneID],
	       [$ra] AS [RA],
	       [$dec] AS [Dec],
	       [$cx] AS [Cx],
	       [$cy] AS [Cy],
	       [$cz] AS [Cz],
	       [$htmid] AS [HtmID],
	       [$weight] AS [a],
	       LOG([$weight]) AS [l],
	       0 AS [q],
	       ([$n] - 1) * LOG(2) AS [logBF]		-- ln(N) of Eq. 33
	       [$columnlist]
	FROM [$tablename]
	[$where]