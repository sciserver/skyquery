-- *** XMatchResources/PopulatePairTable.sql *** ---

DECLARE @r dbo.Region = @region;
DECLARE @dist2 float = 4 * POWER(SIN(RADIANS(@theta/2)), 2);

WITH
__t1 AS
(
[$query1]
),
__t2 AS
(
[$query2]
),
pairs AS
(
	SELECT	
		[$columnlist1],
		[$columnlist2],
		__t1.[RA] AS [RA1],
		__t2.[RA] AS [RA2],
		__link.[Alpha] AS [Alpha]
	FROM [$linktable] AS __link
	INNER LOOP JOIN __t1 ON  __link.[ZoneID1] = __t1.[ZoneID]
	INNER LOOP JOIN __t2 ON  __link.[ZoneID2] = __t2.[ZoneID]
	WHERE (__t1.[Cx] - __t2.[Cx]) * (__t1.[Cx] - __t2.[Cx])
		+ (__t1.[Cy] - __t2.[Cy]) * (__t1.[Cy] - __t2.[Cy])
		+ (__t1.[Cz] - __t2.[Cz]) * (__t1.[Cz] - __t2.[Cz]) < @dist2
),
__wrap AS
(
	SELECT * FROM pairs
	WHERE
		[RA2] BETWEEN [RA1] - [Alpha] AND [RA1] + [Alpha]

	UNION ALL

	-- Take care of negative wrap-around
	SELECT * FROM pairs
	WHERE
		[RA1] BETWEEN 360 - [Alpha] AND 360  AND
		[RA2] BETWEEN 0 AND [RA1] - 360 + [Alpha]

	UNION ALL

	-- Take care of positive wrap-around
	SELECT * FROM pairs
	WHERE
		[RA1] BETWEEN 0 AND [Alpha] AND
		[RA2] BETWEEN [RA1] + 360 - [Alpha] AND 360
)
SELECT 
	[$selectlist1],
	[$selectlist2]
INTO [$pairtable]
FROM __wrap