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
	FROM [SkyNode_TwoDF].[dbo].[PhotoObj] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.1;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_TwoDF_Mini].[dbo].[PhotoObj] WITH (TABLOCKX)
	([objID], [cat], [ra], [dec], [cx], [cy], [cz], [htmid], [bjsel], [prob], [park], [parmu], [igal], [jon], [orient], [eccent], [area], [x_bj], [y_bj], [dx], [dy], [bjg], [rmag], [pmag], [fmag], [smag], [redmag], [ifield], [igfield], [name], [bjg_old], [bjselold], [bjg_100], [bjsel100])
 SELECT sourcetablealias.[objID], sourcetablealias.[cat], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[htmid], sourcetablealias.[bjsel], sourcetablealias.[prob], sourcetablealias.[park], sourcetablealias.[parmu], sourcetablealias.[igal], sourcetablealias.[jon], sourcetablealias.[orient], sourcetablealias.[eccent], sourcetablealias.[area], sourcetablealias.[x_bj], sourcetablealias.[y_bj], sourcetablealias.[dx], sourcetablealias.[dy], sourcetablealias.[bjg], sourcetablealias.[rmag], sourcetablealias.[pmag], sourcetablealias.[fmag], sourcetablealias.[smag], sourcetablealias.[redmag], sourcetablealias.[ifield], sourcetablealias.[igfield], sourcetablealias.[name], sourcetablealias.[bjg_old], sourcetablealias.[bjselold], sourcetablealias.[bjg_100], sourcetablealias.[bjsel100]
 FROM   [SkyNode_TwoDF].[dbo].[PhotoObj] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objID = sourcetablealias.objID
	;


GO

-- SUBSAMPLING TABLE 'SpecObj' ---

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
	FROM [SkyNode_TwoDF].[dbo].[SpecObj] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.1;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_TwoDF_Mini].[dbo].[SpecObj] WITH (TABLOCKX)
	([objID], [cat], [spectra], [name], [UKST], [ra], [dec], [cx], [cy], [cz], [htmid], [bjg], [bjsel], [bjg_old], [bjselold], [galext], [sb_bj], [sr_r], [z], [z_helio], [obsrun], [quality], [abemma], [z_abs], [kbestr], [r_crcor], [z_emi], [nmbest], [snr], [eta_type])
 SELECT sourcetablealias.[objID], sourcetablealias.[cat], sourcetablealias.[spectra], sourcetablealias.[name], sourcetablealias.[UKST], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[htmid], sourcetablealias.[bjg], sourcetablealias.[bjsel], sourcetablealias.[bjg_old], sourcetablealias.[bjselold], sourcetablealias.[galext], sourcetablealias.[sb_bj], sourcetablealias.[sr_r], sourcetablealias.[z], sourcetablealias.[z_helio], sourcetablealias.[obsrun], sourcetablealias.[quality], sourcetablealias.[abemma], sourcetablealias.[z_abs], sourcetablealias.[kbestr], sourcetablealias.[r_crcor], sourcetablealias.[z_emi], sourcetablealias.[nmbest], sourcetablealias.[snr], sourcetablealias.[eta_type]
 FROM   [SkyNode_TwoDF].[dbo].[SpecObj] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objID = sourcetablealias.objID
	;


GO

