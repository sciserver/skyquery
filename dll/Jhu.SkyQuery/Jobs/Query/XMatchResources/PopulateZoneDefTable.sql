-- *** XMatchResources/PopulateZoneDefTable.sql *** ---

-- TODO: add buffer zones to partitions!

INSERT [$tablename] WITH (TABLOCKX)
SELECT [zd].* 
FROM [SkyQuery_Code].[dbo].[CalculateZones](@ZoneHeight, @Theta, @PartitionMin, @PartitionMax) AS [zd]