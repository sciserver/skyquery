-- *** XMatchResources/PopulateZoneDefTable.sql *** ---

INSERT [$tablename] WITH (TABLOCKX)
SELECT [zd].* FROM [SkyQuery_Code].[dbo].[CalculateZones](@ZoneHeight, @Theta, @PartitionMin, @PartitionMax) AS [zd]