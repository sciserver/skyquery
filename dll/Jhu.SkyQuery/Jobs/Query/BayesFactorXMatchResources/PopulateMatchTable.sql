-- *** BayesFactorXMatchResources/PopulateMatchTable.sql *** ---

INSERT [$newtablename] WITH (TABLOCKX)
	([RA], [Dec], [Cx], [Cy], [Cz], [a], [l], [q], [logBF], [$insertcolumnlist])
SELECT
	t.calc.Ra, t.calc.Dec,
	t.calc.Cx, t.calc.Cy, t.calc.Cz,
	t.calc.A,
	t.calc.L,
	q + t.calc.dQ,
	t.calc.LogBF,
	[$selectcolumnlist2]
FROM
(
	SELECT  [tableA].[q] AS q,
			[SkyQuery_Code].dbo.BayesFactorCalcPosition([tableA].[Cx], [tableA].[Cy], [tableA].[Cz],  [tableA].[a], [tableA].[l], [tableA].[logBF], [$weight], [pairtable].[Dx], [pairtable].[Dy], [pairtable].[Dz]) AS [calc],
		    [$selectcolumnlist]
	FROM [$pairtable] AS [pairtable]
	INNER JOIN [$matchtable] AS [tableA]
		   ON [tableA].[MatchID] = [pairtable].[$matchidcolumn]
	INNER JOIN [$table] AS [tableB] -- this should be the 'table'
		   ON [$tablejoinconditions]
) AS t
WHERE
	[Q] + [calc].[dQ] < 2 * (@factor + [calc].[L] + @lmin - LOG([calc].[A] + @amax) - @limit)