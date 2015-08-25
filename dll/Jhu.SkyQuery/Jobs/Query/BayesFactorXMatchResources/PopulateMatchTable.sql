-- *** BayesFactorXMatchResources/PopulateMatchTable.sql *** ---

WITH 
__t1 AS
(
	[$query1]
),
__t2 AS
(
	[$query2]
),
__match AS
(
	SELECT  __t1.[q] AS [q],
			skyquery.BayesFactorCalcPosition(
				__t1.[Cx], __t1.[Cy], __t1.[Cz],  __t1.[a], __t1.[l], __t1.[logBF],
				[$weight],
				__pair.[Dx], __pair.[Dy], __pair.[Dz]) AS __calc
		    [$selectcolumnlist1]
			[$selectcolumnlist2]
	FROM [$pairtable] AS __pair
	INNER JOIN __t1
		   ON [$tablejoin1]
	INNER JOIN __t2
		   ON [$tablejoin2]
	WHERE
		[Q] + __calc.[dQ] < 2 * (@factor + __calc.[L] + @lmin - LOG(__calc.[A] + @amax) - @limit)
)
INSERT [$matchtable] WITH (TABLOCKX)
	([RA], [Dec], [Cx], [Cy], [Cz], [a], [l], [q], [logBF], [ZoneID],
	 [$insertcolumnlist])
SELECT
	t.calc.Ra, t.calc.Dec,
	t.calc.Cx, t.calc.Cy, t.calc.Cz,
	t.calc.A,
	t.calc.L,
	q + t.calc.dQ,
	t.calc.LogBF,
	CONVERT(INT,FLOOR((t.calc.Dec + 90.0) / @H)) as [ZoneID],
	[$selectcolumnlist2]
FROM __match