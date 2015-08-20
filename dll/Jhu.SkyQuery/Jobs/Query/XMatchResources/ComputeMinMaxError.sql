-- *** XMatchResources/ComputeMinMaxError.sql *** ---

SELECT MIN(CAST([$error] AS float)), MAX(CAST([$error] AS float))
FROM [$tablename]
[$where]