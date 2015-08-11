-- *** BayesFactorXMatchResources/ComputeRSquared.sql *** ---

SELECT CAST(ISNULL(MAX( (1 / [$a] + 1 / @weightMin) * (2 * (@factor + [$l] + @lmax - LOG([$a] + @amin) - @limit) - [$q]) ), 0.0000000000) AS float)
FROM [$matchtable]