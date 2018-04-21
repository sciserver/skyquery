-- *** ConeXMatchResources/PopulateMatchTable.sql *** ---

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
	SELECT 
		point.GetAngleXyz(
			__t1.[Cx], __t1.[Cy], __t1.[Cz], 
			__t2.[Cx], __t2.[Cy], __t2.[Cz]) * 60.0 __arc,
		__t1.[RA], __t1.[Dec],
		__t1.[Cx], __t1.[Cy], __t1.[Cz],
		__t1.[radius]
		[$columnlist1]
	FROM [$pairtable] AS __pair
	INNER JOIN __t1 ON [$tablejoin1]
	INNER JOIN __t2 ON [$tablejoin2]
)
INSERT [$matchtable] WITH (TABLOCKX)
	([RA], [Dec], [Cx], [Cy], [Cz], [ZoneID]
	[$columnlist2])
SELECT
	[Ra], [Dec],
	[Cx], [Cy], [Cz],
	CONVERT(INT,FLOOR(([Dec] + 90.0) / @H)) as [ZoneID]
	[$columnlist2]
FROM __match
WHERE __arc <= [radius]