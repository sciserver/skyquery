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
	SELECT skyquery.BayesFactorCalcPosition(
	           __t1.[Cx], __t1.[Cy], __t1.[Cz], 
	           __t1.[a], __t1.[l], __t1.[q], __t1.[logBF],
	           __t2.[Cx], __t2.[Cy], __t2.[Cz],
	           __t2.[a]) AS __calc
	       [$columnlist1]
	FROM [$pairtable] AS __pair
	INNER JOIN __t1 ON [$tablejoin1]
	INNER JOIN __t2 ON [$tablejoin2]
)
INSERT [$matchtable] WITH (TABLOCKX)
	([RA], [Dec], [Cx], [Cy], [Cz], [a], [l], [q], [logBF], [ZoneID]
	[$columnlist2])
SELECT
	__calc.Ra, __calc.Dec,
	__calc.Cx, __calc.Cy, __calc.Cz,
	__calc.A,
	__calc.L,
	__calc.Q,
	__calc.LogBF,
	CONVERT(INT,FLOOR((__calc.Dec + 90.0) / @H)) as [ZoneID]
	[$columnlist2]
FROM __match
WHERE __calc.[Q] < 2 * (@factor + __calc.[L] + @lmin - LOG(__calc.[A] + @amax) - @limit)