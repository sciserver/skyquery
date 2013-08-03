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
	FROM [SkyNode_DLS].[dbo].[PhotoObj] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_DLS_Mini].[dbo].[PhotoObj] WITH (TABLOCKX)
	([objid], [ra], [dec], [band], [AlphaB], [DeltaB], [XB], [YB], [AB], [BB], [THETAB], [FLAGSB], [MAG_APERB], [MAGERR_APERB], [MAGB], [MAGERRB], [MAG_ISOB], [MAGERR_ISOB], [ISOAREAB], [CLASS_STARB], [AlphaV], [DeltaV], [XV], [YV], [AV], [BV], [THETAV], [FLAGSV], [MAG_APERV], [MAGERR_APERV], [MAGV], [MAGERRV], [MAG_ISOV], [MAGERR_ISOV], [ISOAREAV], [CLASS_STARV], [AlphaR], [DeltaR], [XR], [YR], [AR], [BR], [THETAR], [FLAGSR], [MAG_APERR], [MAGERR_APERR], [MAGR], [MAGERRR], [MAG_ISOR], [MAGERR_ISOR], [ISOAREAR], [CLASS_STARR], [Alphaz], [Deltaz], [Xz], [Yz], [Az], [Bz], [THETAz], [FLAGSz], [MAG_APERz], [MAGERR_APERz], [MAGz], [MAGERRz], [MAG_ISOz], [MAGERR_ISOz], [ISOAREAz], [CLASS_STARz], [htmid], [cx], [cy], [cz])
 SELECT sourcetablealias.[objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[band], sourcetablealias.[AlphaB], sourcetablealias.[DeltaB], sourcetablealias.[XB], sourcetablealias.[YB], sourcetablealias.[AB], sourcetablealias.[BB], sourcetablealias.[THETAB], sourcetablealias.[FLAGSB], sourcetablealias.[MAG_APERB], sourcetablealias.[MAGERR_APERB], sourcetablealias.[MAGB], sourcetablealias.[MAGERRB], sourcetablealias.[MAG_ISOB], sourcetablealias.[MAGERR_ISOB], sourcetablealias.[ISOAREAB], sourcetablealias.[CLASS_STARB], sourcetablealias.[AlphaV], sourcetablealias.[DeltaV], sourcetablealias.[XV], sourcetablealias.[YV], sourcetablealias.[AV], sourcetablealias.[BV], sourcetablealias.[THETAV], sourcetablealias.[FLAGSV], sourcetablealias.[MAG_APERV], sourcetablealias.[MAGERR_APERV], sourcetablealias.[MAGV], sourcetablealias.[MAGERRV], sourcetablealias.[MAG_ISOV], sourcetablealias.[MAGERR_ISOV], sourcetablealias.[ISOAREAV], sourcetablealias.[CLASS_STARV], sourcetablealias.[AlphaR], sourcetablealias.[DeltaR], sourcetablealias.[XR], sourcetablealias.[YR], sourcetablealias.[AR], sourcetablealias.[BR], sourcetablealias.[THETAR], sourcetablealias.[FLAGSR], sourcetablealias.[MAG_APERR], sourcetablealias.[MAGERR_APERR], sourcetablealias.[MAGR], sourcetablealias.[MAGERRR], sourcetablealias.[MAG_ISOR], sourcetablealias.[MAGERR_ISOR], sourcetablealias.[ISOAREAR], sourcetablealias.[CLASS_STARR], sourcetablealias.[Alphaz], sourcetablealias.[Deltaz], sourcetablealias.[Xz], sourcetablealias.[Yz], sourcetablealias.[Az], sourcetablealias.[Bz], sourcetablealias.[THETAz], sourcetablealias.[FLAGSz], sourcetablealias.[MAG_APERz], sourcetablealias.[MAGERR_APERz], sourcetablealias.[MAGz], sourcetablealias.[MAGERRz], sourcetablealias.[MAG_ISOz], sourcetablealias.[MAGERR_ISOz], sourcetablealias.[ISOAREAz], sourcetablealias.[CLASS_STARz], sourcetablealias.[htmid], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz]
 FROM   [SkyNode_DLS].[dbo].[PhotoObj] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objid = sourcetablealias.objid
	;


GO

