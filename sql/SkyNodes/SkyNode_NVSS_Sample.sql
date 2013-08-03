-- SUBSAMPLING TABLE 'PhotoObj' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 --GO

 CREATE TABLE ##temporaryidlist
 (
	[objID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[objID], SkyQuery_Code.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_NVSS].[dbo].[PhotoObj] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_NVSS_Mini].[dbo].[PhotoObj] WITH (TABLOCKX)
	([objID], [ra], [raErr], [dec], [decErr], [flux], [fluxErr], [major], [majorErr], [minor], [minorErr], [pa], [paErr], [res], [resOff], [p_flux], [p_fluxErr], [p_ang], [p_angErr], [field], [x_pix], [y_pix], [htmid], [cx], [cy], [cz])
 SELECT sourcetablealias.[objID], sourcetablealias.[ra], sourcetablealias.[raErr], sourcetablealias.[dec], sourcetablealias.[decErr], sourcetablealias.[flux], sourcetablealias.[fluxErr], sourcetablealias.[major], sourcetablealias.[majorErr], sourcetablealias.[minor], sourcetablealias.[minorErr], sourcetablealias.[pa], sourcetablealias.[paErr], sourcetablealias.[res], sourcetablealias.[resOff], sourcetablealias.[p_flux], sourcetablealias.[p_fluxErr], sourcetablealias.[p_ang], sourcetablealias.[p_angErr], sourcetablealias.[field], sourcetablealias.[x_pix], sourcetablealias.[y_pix], sourcetablealias.[htmid], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz]
 FROM   [SkyNode_NVSS].[dbo].[PhotoObj] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objID = sourcetablealias.objID
	;


GO

