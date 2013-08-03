-- SUBSAMPLING TABLE 'PhotoObj' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 --GO

 CREATE TABLE ##temporaryidlist
 (
	[objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[objid], SkyQuery_Code.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_AGC].[dbo].[PhotoObj] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.1;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_AGC_Mini].[dbo].[PhotoObj] WITH (TABLOCKX)
	([objid], [ra], [dec], [cx], [cy], [cz], [htmID], [a], [b], [Bmag], [angle], [type], [btype], [velocity], [velocityError], [objname], [fluxHI], [fluxHInoise], [centerVelocity], [velocityWidth], [velocityWidthError], [telescopeCode], [HIdetectionCode], [HIsnr], [IbandQ], [RC3flag], [IrotCat])
 SELECT sourcetablealias.[objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[htmID], sourcetablealias.[a], sourcetablealias.[b], sourcetablealias.[Bmag], sourcetablealias.[angle], sourcetablealias.[type], sourcetablealias.[btype], sourcetablealias.[velocity], sourcetablealias.[velocityError], sourcetablealias.[objname], sourcetablealias.[fluxHI], sourcetablealias.[fluxHInoise], sourcetablealias.[centerVelocity], sourcetablealias.[velocityWidth], sourcetablealias.[velocityWidthError], sourcetablealias.[telescopeCode], sourcetablealias.[HIdetectionCode], sourcetablealias.[HIsnr], sourcetablealias.[IbandQ], sourcetablealias.[RC3flag], sourcetablealias.[IrotCat]
 FROM   [SkyNode_AGC].[dbo].[PhotoObj] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objid = sourcetablealias.objid
	;


GO

