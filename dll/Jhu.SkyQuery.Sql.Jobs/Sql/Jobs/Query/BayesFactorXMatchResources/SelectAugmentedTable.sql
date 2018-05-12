	SELECT [$zoneid] AS [ZoneID],
	       [$ra] AS [RA],
	       [$dec] AS [Dec],
	       [$cx] AS [Cx],
	       [$cy] AS [Cy],
	       [$cz] AS [Cz],
	       [$htmid] AS [HtmID],
	       CAST(1 AS smallint) AS [n],
	       [$weight] AS [a],				-- 1/sigma^2 of Eq. 12
	       LOG([$weight]) AS [l],			-- 
	       CAST(0 AS float) AS [q],			-- Eq. 39
	       CAST(0 AS float) AS [logBF]		-- ln(N) of Eq. 33
	       [$columnlist]
	FROM [$tablename]
	[$where]