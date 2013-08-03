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
	FROM [SkyNode_USNOB].[dbo].[PhotoObj] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_USNOB_Mini].[dbo].[PhotoObj] WITH (TABLOCKX)
	([objid], [zone], [seqNo], [cx], [cy], [cz], [htmID], [ra], [dec], [pmRA], [pmDEC], [pmPr], [mcFlag], [e_pmRA], [e_pmDEC], [e_RAfit], [e_DECfit], [Ndet], [difSp], [e_RA], [e_DEC], [Epoch], [ys40], [B1Mag], [B1C], [B1S], [B1F], [B1S_G], [B1Xi], [B1Eta], [R1Mag], [R1C], [R1S], [R1F], [R1S_G], [R1Xi], [R1Eta], [B2Mag], [B2C], [B2S], [B2F], [B2S_G], [B2Xi], [B2Eta], [R2Mag], [R2C], [R2S], [R2F], [R2S_G], [R2Xi], [R2Eta], [NMag], [NC], [NS], [NF], [NS_G], [NXi], [NEta], [lbIdxB1], [lbIdxR1], [lbIdxB2], [lbIdxR2], [lbIdxN])
 SELECT sourcetablealias.[objid], sourcetablealias.[zone], sourcetablealias.[seqNo], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[htmID], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[pmRA], sourcetablealias.[pmDEC], sourcetablealias.[pmPr], sourcetablealias.[mcFlag], sourcetablealias.[e_pmRA], sourcetablealias.[e_pmDEC], sourcetablealias.[e_RAfit], sourcetablealias.[e_DECfit], sourcetablealias.[Ndet], sourcetablealias.[difSp], sourcetablealias.[e_RA], sourcetablealias.[e_DEC], sourcetablealias.[Epoch], sourcetablealias.[ys40], sourcetablealias.[B1Mag], sourcetablealias.[B1C], sourcetablealias.[B1S], sourcetablealias.[B1F], sourcetablealias.[B1S_G], sourcetablealias.[B1Xi], sourcetablealias.[B1Eta], sourcetablealias.[R1Mag], sourcetablealias.[R1C], sourcetablealias.[R1S], sourcetablealias.[R1F], sourcetablealias.[R1S_G], sourcetablealias.[R1Xi], sourcetablealias.[R1Eta], sourcetablealias.[B2Mag], sourcetablealias.[B2C], sourcetablealias.[B2S], sourcetablealias.[B2F], sourcetablealias.[B2S_G], sourcetablealias.[B2Xi], sourcetablealias.[B2Eta], sourcetablealias.[R2Mag], sourcetablealias.[R2C], sourcetablealias.[R2S], sourcetablealias.[R2F], sourcetablealias.[R2S_G], sourcetablealias.[R2Xi], sourcetablealias.[R2Eta], sourcetablealias.[NMag], sourcetablealias.[NC], sourcetablealias.[NS], sourcetablealias.[NF], sourcetablealias.[NS_G], sourcetablealias.[NXi], sourcetablealias.[NEta], sourcetablealias.[lbIdxB1], sourcetablealias.[lbIdxR1], sourcetablealias.[lbIdxB2], sourcetablealias.[lbIdxR2], sourcetablealias.[lbIdxN]
 FROM   [SkyNode_USNOB].[dbo].[PhotoObj] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objid = sourcetablealias.objid
	;


GO

