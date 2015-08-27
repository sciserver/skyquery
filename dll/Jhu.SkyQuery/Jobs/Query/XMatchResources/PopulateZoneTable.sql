-- *** XMatchResources/PopulateZoneTable.sql *** ---

DECLARE @r dbo.Region = @region;

WITH __t AS
(
[$query]
)
INSERT [$zonetablename] WITH (TABLOCKX)
SELECT __t.[ZoneID],
	   __t.[RA],
	   __t.[Dec],
	   __t.[Cx],
	   __t.[Cy],
	   __t.[Cz],
	   __t.[HtmID]
	   [$selectcolumnlist]
FROM __t