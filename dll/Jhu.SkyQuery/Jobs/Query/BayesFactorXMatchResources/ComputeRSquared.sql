-- *** BayesFactorXMatchResources/ComputeRSquared.sql *** ---

DECLARE @r dbo.Region = @region;

WITH __t AS
(
[$query]
)
SELECT MAX(skyquery.BayesFactorCalcSearchRadius(
	a, l, q, @weightMin, @amin, @lmax, @stepCount, @limit))
FROM __t