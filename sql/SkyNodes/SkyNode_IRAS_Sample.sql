-- SUBSAMPLING TABLE 'PhotoObj' ---

 --Setting Identity Column
 SET IDENTITY_INSERT [SkyNode_IRAS_Mini].[dbo].[PhotoObj] ON;
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
	FROM [SkyNode_IRAS].[dbo].[PhotoObj] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.1;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_IRAS_Mini].[dbo].[PhotoObj] WITH (TABLOCKX)
	([objID], [name], [ra], [dec], [err_maj], [err_min], [err_ang], [nhcon], [flux_12], [flux_25], [flux_60], [flux_100], [fqual_12], [fqual_25], [fqual_60], [fqual_100], [nlrs], [lrschar], [relunc_12], [relunc_25], [relunc_60], [relunc_100], [tsnr_12], [tsnr_25], [tsnr_60], [tsnr_100], [cc_12], [cc_25], [cc_60], [cc_100], [lvar], [disc], [confuse], [pnearh], [pnearw], [ses1_12], [ses1_25], [ses1_60], [ses1_100], [ses2_12], [ses2_25], [ses2_60], [ses2_100], [hsdflag], [cirr1], [cirr2], [cirr3], [nid], [idtype], [mhcon], [fcor_12], [fcor_25], [fcor_60], [fcor_100], [cx], [cy], [cz], [htmid])
 SELECT sourcetablealias.[objID], sourcetablealias.[name], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[err_maj], sourcetablealias.[err_min], sourcetablealias.[err_ang], sourcetablealias.[nhcon], sourcetablealias.[flux_12], sourcetablealias.[flux_25], sourcetablealias.[flux_60], sourcetablealias.[flux_100], sourcetablealias.[fqual_12], sourcetablealias.[fqual_25], sourcetablealias.[fqual_60], sourcetablealias.[fqual_100], sourcetablealias.[nlrs], sourcetablealias.[lrschar], sourcetablealias.[relunc_12], sourcetablealias.[relunc_25], sourcetablealias.[relunc_60], sourcetablealias.[relunc_100], sourcetablealias.[tsnr_12], sourcetablealias.[tsnr_25], sourcetablealias.[tsnr_60], sourcetablealias.[tsnr_100], sourcetablealias.[cc_12], sourcetablealias.[cc_25], sourcetablealias.[cc_60], sourcetablealias.[cc_100], sourcetablealias.[lvar], sourcetablealias.[disc], sourcetablealias.[confuse], sourcetablealias.[pnearh], sourcetablealias.[pnearw], sourcetablealias.[ses1_12], sourcetablealias.[ses1_25], sourcetablealias.[ses1_60], sourcetablealias.[ses1_100], sourcetablealias.[ses2_12], sourcetablealias.[ses2_25], sourcetablealias.[ses2_60], sourcetablealias.[ses2_100], sourcetablealias.[hsdflag], sourcetablealias.[cirr1], sourcetablealias.[cirr2], sourcetablealias.[cirr3], sourcetablealias.[nid], sourcetablealias.[idtype], sourcetablealias.[mhcon], sourcetablealias.[fcor_12], sourcetablealias.[fcor_25], sourcetablealias.[fcor_60], sourcetablealias.[fcor_100], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[htmid]
 FROM   [SkyNode_IRAS].[dbo].[PhotoObj] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objID = sourcetablealias.objID
	;
 --Setting Identity Column
 SET IDENTITY_INSERT [SkyNode_IRAS_Mini].[dbo].[PhotoObj] OFF;
 

GO

