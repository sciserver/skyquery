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
	FROM [SkyNode_First].[dbo].[PhotoObj] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_First_Mini].[dbo].[PhotoObj] WITH (TABLOCKX)
	([objID], [ra], [dec], [w], [fpeak], [fint], [rms], [maj], [min], [pa], [fmaj], [fmin], [fpa], [fieldname], [htmid], [cx], [cy], [cz])
 SELECT sourcetablealias.[objID], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[w], sourcetablealias.[fpeak], sourcetablealias.[fint], sourcetablealias.[rms], sourcetablealias.[maj], sourcetablealias.[min], sourcetablealias.[pa], sourcetablealias.[fmaj], sourcetablealias.[fmin], sourcetablealias.[fpa], sourcetablealias.[fieldname], sourcetablealias.[htmid], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz]
 FROM   [SkyNode_First].[dbo].[PhotoObj] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objID = sourcetablealias.objID
	;


GO

