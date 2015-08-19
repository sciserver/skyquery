-- *** XMatchResources/PopulateZoneDefTable.sql *** ---

INSERT [$tablename] WITH (TABLOCKX)
SELECT [ZD].* 
FROM skyquery.GetZones(@H, @Theta) AS [ZD]