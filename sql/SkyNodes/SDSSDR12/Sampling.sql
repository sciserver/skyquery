-- SUBSAMPLING TABLE 'apogeeDesign' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[apogeeDesign] WITH (TABLOCKX)
    ([designid], [ra], [dec], [location_id], [radius], [shared], [comments], [short_cohort_version], [medium_cohort_version], [long_cohort_version], [number_of_short_fibers], [number_of_medium_fibers], [number_of_long_fibers], [short_cohort_min_h], [short_cohort_max_h], [medium_cohort_min_h], [medium_cohort_max_h], [long_cohort_min_h], [long_cohort_max_h], [dereddened_min_j_ks_color], [number_of_visits], [number_of_tellurics], [number_of_sky], [number_of_science])
 SELECT sourcetablealias.[designid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[location_id], sourcetablealias.[radius], sourcetablealias.[shared], sourcetablealias.[comments], sourcetablealias.[short_cohort_version], sourcetablealias.[medium_cohort_version], sourcetablealias.[long_cohort_version], sourcetablealias.[number_of_short_fibers], sourcetablealias.[number_of_medium_fibers], sourcetablealias.[number_of_long_fibers], sourcetablealias.[short_cohort_min_h], sourcetablealias.[short_cohort_max_h], sourcetablealias.[medium_cohort_min_h], sourcetablealias.[medium_cohort_max_h], sourcetablealias.[long_cohort_min_h], sourcetablealias.[long_cohort_max_h], sourcetablealias.[dereddened_min_j_ks_color], sourcetablealias.[number_of_visits], sourcetablealias.[number_of_tellurics], sourcetablealias.[number_of_sky], sourcetablealias.[number_of_science]
 FROM   [SkyNode_SDSSDR12].[dbo].[apogeeDesign] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'apogeeField' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[apogeeField] WITH (TABLOCKX)
    ([field_name], [location_id], [ra], [dec], [expected_no_of_visits])
 SELECT sourcetablealias.[field_name], sourcetablealias.[location_id], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[expected_no_of_visits]
 FROM   [SkyNode_SDSSDR12].[dbo].[apogeeField] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'apogeeObject' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[target_id] varchar
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[target_id], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[apogeeObject] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [target_id]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[apogeeObject] WITH (TABLOCKX)
	([apogee_id], [target_id], [alt_id], [location_id], [ra], [dec], [j], [j_err], [h], [src_h], [h_err], [k], [k_err], [irac_3_6], [irac_3_6_err], [irac_4_5], [irac_4_5_err], [src_4_5], [irac_5_8], [irac_5_8_err], [irac_8_0], [irac_8_0_err], [wise_4_5], [wise_4_5_err], [ak_wise], [sfd_ebv], [wash_m], [wash_m_err], [wash_t2], [wash_t2_err], [DDO51], [DDO51_err], [wash_ddo51_giant_flag], [wash_ddo51_star_flag], [targ_4_5], [targ_4_5_err], [ak_targ], [ak_targ_method], [pmra], [pmra_err], [pmdec], [pmdec_err], [pm_src], [tmass_a], [tmass_pxpa], [tmass_prox], [tmass_phqual], [tmass_rdflg], [tmass_ccflg], [tmass_extkey], [tmass_gal_contam])
 SELECT sourcetablealias.[apogee_id], sourcetablealias.[target_id], sourcetablealias.[alt_id], sourcetablealias.[location_id], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[j], sourcetablealias.[j_err], sourcetablealias.[h], sourcetablealias.[src_h], sourcetablealias.[h_err], sourcetablealias.[k], sourcetablealias.[k_err], sourcetablealias.[irac_3_6], sourcetablealias.[irac_3_6_err], sourcetablealias.[irac_4_5], sourcetablealias.[irac_4_5_err], sourcetablealias.[src_4_5], sourcetablealias.[irac_5_8], sourcetablealias.[irac_5_8_err], sourcetablealias.[irac_8_0], sourcetablealias.[irac_8_0_err], sourcetablealias.[wise_4_5], sourcetablealias.[wise_4_5_err], sourcetablealias.[ak_wise], sourcetablealias.[sfd_ebv], sourcetablealias.[wash_m], sourcetablealias.[wash_m_err], sourcetablealias.[wash_t2], sourcetablealias.[wash_t2_err], sourcetablealias.[DDO51], sourcetablealias.[DDO51_err], sourcetablealias.[wash_ddo51_giant_flag], sourcetablealias.[wash_ddo51_star_flag], sourcetablealias.[targ_4_5], sourcetablealias.[targ_4_5_err], sourcetablealias.[ak_targ], sourcetablealias.[ak_targ_method], sourcetablealias.[pmra], sourcetablealias.[pmra_err], sourcetablealias.[pmdec], sourcetablealias.[pmdec_err], sourcetablealias.[pm_src], sourcetablealias.[tmass_a], sourcetablealias.[tmass_pxpa], sourcetablealias.[tmass_prox], sourcetablealias.[tmass_phqual], sourcetablealias.[tmass_rdflg], sourcetablealias.[tmass_ccflg], sourcetablealias.[tmass_extkey], sourcetablealias.[tmass_gal_contam]
 FROM   [SkyNode_SDSSDR12].[dbo].[apogeeObject] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.target_id = sourcetablealias.target_id
	;


GO

-- SUBSAMPLING TABLE 'apogeePlate' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[apogeePlate] WITH (TABLOCKX)
    ([plate_visit_id], [location_id], [plate], [mjd], [apred_version], [name], [racen], [deccen], [radius], [shared], [field_type], [survey], [programname], [platerun], [designid], [nStandard], [nScience], [nSky], [platedesign_version])
 SELECT sourcetablealias.[plate_visit_id], sourcetablealias.[location_id], sourcetablealias.[plate], sourcetablealias.[mjd], sourcetablealias.[apred_version], sourcetablealias.[name], sourcetablealias.[racen], sourcetablealias.[deccen], sourcetablealias.[radius], sourcetablealias.[shared], sourcetablealias.[field_type], sourcetablealias.[survey], sourcetablealias.[programname], sourcetablealias.[platerun], sourcetablealias.[designid], sourcetablealias.[nStandard], sourcetablealias.[nScience], sourcetablealias.[nSky], sourcetablealias.[platedesign_version]
 FROM   [SkyNode_SDSSDR12].[dbo].[apogeePlate] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'apogeeStar' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[apstar_id] varchar
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[apstar_id], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[apogeeStar] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [apstar_id]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[apogeeStar] WITH (TABLOCKX)
	([apstar_id], [target_id], [reduction_id], [file], [apogee_id], [telescope], [location_id], [field], [ra], [dec], [glon], [glat], [apogee_target1], [apogee_target2], [extratarg], [nvisits], [commiss], [snr], [starflag], [andflag], [vhelio_avg], [vscatter], [verr], [verr_med], [synthvhelio_avg], [synthvscatter], [synthverr], [synthverr_med], [rv_teff], [rv_logg], [rv_feh], [rv_ccfwhm], [rv_autofwhm], [synthscatter], [stablerv_chi2], [stablerv_rchi2], [chi2_threshold], [stablerv_chi2_prob], [apstar_version], [htmID])
 SELECT sourcetablealias.[apstar_id], sourcetablealias.[target_id], sourcetablealias.[reduction_id], sourcetablealias.[file], sourcetablealias.[apogee_id], sourcetablealias.[telescope], sourcetablealias.[location_id], sourcetablealias.[field], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[glon], sourcetablealias.[glat], sourcetablealias.[apogee_target1], sourcetablealias.[apogee_target2], sourcetablealias.[extratarg], sourcetablealias.[nvisits], sourcetablealias.[commiss], sourcetablealias.[snr], sourcetablealias.[starflag], sourcetablealias.[andflag], sourcetablealias.[vhelio_avg], sourcetablealias.[vscatter], sourcetablealias.[verr], sourcetablealias.[verr_med], sourcetablealias.[synthvhelio_avg], sourcetablealias.[synthvscatter], sourcetablealias.[synthverr], sourcetablealias.[synthverr_med], sourcetablealias.[rv_teff], sourcetablealias.[rv_logg], sourcetablealias.[rv_feh], sourcetablealias.[rv_ccfwhm], sourcetablealias.[rv_autofwhm], sourcetablealias.[synthscatter], sourcetablealias.[stablerv_chi2], sourcetablealias.[stablerv_rchi2], sourcetablealias.[chi2_threshold], sourcetablealias.[stablerv_chi2_prob], sourcetablealias.[apstar_version], sourcetablealias.[htmID]
 FROM   [SkyNode_SDSSDR12].[dbo].[apogeeStar] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.apstar_id = sourcetablealias.apstar_id
	;


GO

-- SUBSAMPLING TABLE 'apogeeStarAllVisit' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[visit_id] varchar, [apstar_id] varchar
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[visit_id], sourcetablealias.[apstar_id], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[apogeeStarAllVisit] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [visit_id], [apstar_id]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[apogeeStarAllVisit] WITH (TABLOCKX)
	([visit_id], [apstar_id])
 SELECT sourcetablealias.[visit_id], sourcetablealias.[apstar_id]
 FROM   [SkyNode_SDSSDR12].[dbo].[apogeeStarAllVisit] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.visit_id = sourcetablealias.visit_id AND ##temporaryidlist.apstar_id = sourcetablealias.apstar_id
	;


GO

-- SUBSAMPLING TABLE 'apogeeStarVisit' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[visit_id] varchar
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[visit_id], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[apogeeStarVisit] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [visit_id]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[apogeeStarVisit] WITH (TABLOCKX)
	([visit_id], [apstar_id])
 SELECT sourcetablealias.[visit_id], sourcetablealias.[apstar_id]
 FROM   [SkyNode_SDSSDR12].[dbo].[apogeeStarVisit] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.visit_id = sourcetablealias.visit_id
	;


GO

-- SUBSAMPLING TABLE 'apogeeVisit' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[visit_id] varchar
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[visit_id], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[apogeeVisit] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [visit_id]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[apogeeVisit] WITH (TABLOCKX)
	([visit_id], [apred_version], [apogee_id], [target_id], [reduction_id], [file], [telescope], [fiberid], [plate], [mjd], [location_id], [ra], [dec], [glon], [glat], [apogee_target1], [apogee_target2], [extratarg], [snr], [starflag], [dateobs], [jd], [bc], [vtype], [vrel], [vrelerr], [vhelio], [chisq], [rv_feh], [rv_teff], [rv_logg], [rv_alpha], [rv_carb], [synthfile], [estvtype], [estvrel], [estvrelerr], [estvhelio], [synthvrel], [synthvrelerr], [synthvhelio])
 SELECT sourcetablealias.[visit_id], sourcetablealias.[apred_version], sourcetablealias.[apogee_id], sourcetablealias.[target_id], sourcetablealias.[reduction_id], sourcetablealias.[file], sourcetablealias.[telescope], sourcetablealias.[fiberid], sourcetablealias.[plate], sourcetablealias.[mjd], sourcetablealias.[location_id], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[glon], sourcetablealias.[glat], sourcetablealias.[apogee_target1], sourcetablealias.[apogee_target2], sourcetablealias.[extratarg], sourcetablealias.[snr], sourcetablealias.[starflag], sourcetablealias.[dateobs], sourcetablealias.[jd], sourcetablealias.[bc], sourcetablealias.[vtype], sourcetablealias.[vrel], sourcetablealias.[vrelerr], sourcetablealias.[vhelio], sourcetablealias.[chisq], sourcetablealias.[rv_feh], sourcetablealias.[rv_teff], sourcetablealias.[rv_logg], sourcetablealias.[rv_alpha], sourcetablealias.[rv_carb], sourcetablealias.[synthfile], sourcetablealias.[estvtype], sourcetablealias.[estvrel], sourcetablealias.[estvrelerr], sourcetablealias.[estvhelio], sourcetablealias.[synthvrel], sourcetablealias.[synthvrelerr], sourcetablealias.[synthvhelio]
 FROM   [SkyNode_SDSSDR12].[dbo].[apogeeVisit] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.visit_id = sourcetablealias.visit_id
	;


GO

-- SUBSAMPLING TABLE 'aspcapStar' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[aspcap_id] varchar
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[aspcap_id], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[aspcapStar] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [aspcap_id]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[aspcapStar] WITH (TABLOCKX)
	([apstar_id], [target_id], [aspcap_id], [apogee_id], [aspcap_version], [results_version], [teff], [teff_err], [teff_flag], [logg], [logg_err], [logg_flag], [aspcap_chi2], [aspcap_class], [aspcapflag], [fparam_teff], [fparam_logg], [fparam_logvmicro], [fparam_m_h], [fparam_c_m], [fparam_n_m], [fparam_alpha_m], [param_teff], [param_logg], [param_logvmicro], [param_logvmicro_flag], [param_m_h], [param_m_h_err], [param_m_h_flag], [param_c_m], [param_c_m_flag], [param_n_m], [param_n_m_flag], [param_alpha_m], [param_alpha_m_err], [param_alpha_m_flag], [al_h], [al_h_err], [al_h_flag], [c_h], [c_h_err], [c_h_flag], [ca_h], [ca_h_err], [ca_h_flag], [fe_h], [fe_h_err], [fe_h_flag], [k_h], [k_h_err], [k_h_flag], [mg_h], [mg_h_err], [mg_h_flag], [mn_h], [mn_h_err], [mn_h_flag], [na_h], [na_h_err], [na_h_flag], [ni_h], [ni_h_err], [ni_h_flag], [n_h], [n_h_err], [n_h_flag], [o_h], [o_h_err], [o_h_flag], [si_h], [si_h_err], [si_h_flag], [s_h], [s_h_err], [s_h_flag], [ti_h], [ti_h_err], [ti_h_flag], [v_h], [v_h_err], [v_h_flag], [felem_al_h], [felem_al_h_err], [felem_c_m], [felem_c_m_err], [felem_ca_m], [felem_ca_m_err], [felem_fe_h], [felem_fe_h_err], [felem_k_h], [felem_k_h_err], [felem_mg_m], [felem_mg_m_err], [felem_mn_h], [felem_mn_h_err], [felem_na_h], [felem_na_h_err], [felem_ni_h], [felem_ni_h_err], [felem_n_m], [felem_n_m_err], [felem_o_m], [felem_o_m_err], [felem_si_m], [felem_si_m_err], [felem_s_m], [felem_s_m_err], [felem_ti_m], [felem_ti_m_err], [felem_v_h], [felem_v_h_err])
 SELECT sourcetablealias.[apstar_id], sourcetablealias.[target_id], sourcetablealias.[aspcap_id], sourcetablealias.[apogee_id], sourcetablealias.[aspcap_version], sourcetablealias.[results_version], sourcetablealias.[teff], sourcetablealias.[teff_err], sourcetablealias.[teff_flag], sourcetablealias.[logg], sourcetablealias.[logg_err], sourcetablealias.[logg_flag], sourcetablealias.[aspcap_chi2], sourcetablealias.[aspcap_class], sourcetablealias.[aspcapflag], sourcetablealias.[fparam_teff], sourcetablealias.[fparam_logg], sourcetablealias.[fparam_logvmicro], sourcetablealias.[fparam_m_h], sourcetablealias.[fparam_c_m], sourcetablealias.[fparam_n_m], sourcetablealias.[fparam_alpha_m], sourcetablealias.[param_teff], sourcetablealias.[param_logg], sourcetablealias.[param_logvmicro], sourcetablealias.[param_logvmicro_flag], sourcetablealias.[param_m_h], sourcetablealias.[param_m_h_err], sourcetablealias.[param_m_h_flag], sourcetablealias.[param_c_m], sourcetablealias.[param_c_m_flag], sourcetablealias.[param_n_m], sourcetablealias.[param_n_m_flag], sourcetablealias.[param_alpha_m], sourcetablealias.[param_alpha_m_err], sourcetablealias.[param_alpha_m_flag], sourcetablealias.[al_h], sourcetablealias.[al_h_err], sourcetablealias.[al_h_flag], sourcetablealias.[c_h], sourcetablealias.[c_h_err], sourcetablealias.[c_h_flag], sourcetablealias.[ca_h], sourcetablealias.[ca_h_err], sourcetablealias.[ca_h_flag], sourcetablealias.[fe_h], sourcetablealias.[fe_h_err], sourcetablealias.[fe_h_flag], sourcetablealias.[k_h], sourcetablealias.[k_h_err], sourcetablealias.[k_h_flag], sourcetablealias.[mg_h], sourcetablealias.[mg_h_err], sourcetablealias.[mg_h_flag], sourcetablealias.[mn_h], sourcetablealias.[mn_h_err], sourcetablealias.[mn_h_flag], sourcetablealias.[na_h], sourcetablealias.[na_h_err], sourcetablealias.[na_h_flag], sourcetablealias.[ni_h], sourcetablealias.[ni_h_err], sourcetablealias.[ni_h_flag], sourcetablealias.[n_h], sourcetablealias.[n_h_err], sourcetablealias.[n_h_flag], sourcetablealias.[o_h], sourcetablealias.[o_h_err], sourcetablealias.[o_h_flag], sourcetablealias.[si_h], sourcetablealias.[si_h_err], sourcetablealias.[si_h_flag], sourcetablealias.[s_h], sourcetablealias.[s_h_err], sourcetablealias.[s_h_flag], sourcetablealias.[ti_h], sourcetablealias.[ti_h_err], sourcetablealias.[ti_h_flag], sourcetablealias.[v_h], sourcetablealias.[v_h_err], sourcetablealias.[v_h_flag], sourcetablealias.[felem_al_h], sourcetablealias.[felem_al_h_err], sourcetablealias.[felem_c_m], sourcetablealias.[felem_c_m_err], sourcetablealias.[felem_ca_m], sourcetablealias.[felem_ca_m_err], sourcetablealias.[felem_fe_h], sourcetablealias.[felem_fe_h_err], sourcetablealias.[felem_k_h], sourcetablealias.[felem_k_h_err], sourcetablealias.[felem_mg_m], sourcetablealias.[felem_mg_m_err], sourcetablealias.[felem_mn_h], sourcetablealias.[felem_mn_h_err], sourcetablealias.[felem_na_h], sourcetablealias.[felem_na_h_err], sourcetablealias.[felem_ni_h], sourcetablealias.[felem_ni_h_err], sourcetablealias.[felem_n_m], sourcetablealias.[felem_n_m_err], sourcetablealias.[felem_o_m], sourcetablealias.[felem_o_m_err], sourcetablealias.[felem_si_m], sourcetablealias.[felem_si_m_err], sourcetablealias.[felem_s_m], sourcetablealias.[felem_s_m_err], sourcetablealias.[felem_ti_m], sourcetablealias.[felem_ti_m_err], sourcetablealias.[felem_v_h], sourcetablealias.[felem_v_h_err]
 FROM   [SkyNode_SDSSDR12].[dbo].[aspcapStar] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.aspcap_id = sourcetablealias.aspcap_id
	;


GO

-- SUBSAMPLING TABLE 'aspcapStarCovar' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[aspcap_covar_id] varchar, [aspcap_id] varchar
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[aspcap_covar_id], sourcetablealias.[aspcap_id], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[aspcapStarCovar] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [aspcap_covar_id], [aspcap_id]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[aspcapStarCovar] WITH (TABLOCKX)
	([aspcap_covar_id], [aspcap_id], [param_1], [param_2], [covar], [fit_covar])
 SELECT sourcetablealias.[aspcap_covar_id], sourcetablealias.[aspcap_id], sourcetablealias.[param_1], sourcetablealias.[param_2], sourcetablealias.[covar], sourcetablealias.[fit_covar]
 FROM   [SkyNode_SDSSDR12].[dbo].[aspcapStarCovar] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.aspcap_covar_id = sourcetablealias.aspcap_covar_id AND ##temporaryidlist.aspcap_id = sourcetablealias.aspcap_id
	;


GO

-- SUBSAMPLING TABLE 'DataConstants' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[DataConstants] WITH (TABLOCKX)
    ([field], [name], [value], [description])
 SELECT sourcetablealias.[field], sourcetablealias.[name], sourcetablealias.[value], sourcetablealias.[description]
 FROM   [SkyNode_SDSSDR12].[dbo].[DataConstants] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'emissionLinesPort' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[emissionLinesPort] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[emissionLinesPort] WITH (TABLOCKX)
	([specObjID], [plate], [fiberID], [mjd], [ra], [dec], [z], [zErr], [zNum], [velStars], [redshift], [sigmaStars], [sigmaStarsErr], [chisq], [bpt], [ebmv], [ebmv_err], [Error_Warning], [V_HeII_3203], [V_HeII_3203_Err], [Sigma_HeII_3203], [Sigma_HeII_3203_Err], [Amplitude_HeII_3203], [Amplitude_HeII_3203_Err], [Flux_HeII_3203], [Flux_HeII_3203_Err], [EW_HeII_3203], [EW_HeII_3203_Err], [Flux_Cont_HeII_3203], [Flux_Cont_HeII_3203_Err], [Fit_Warning_HeII_3203], [AoN_HeII_3203], [V_NeV_3345], [V_NeV_3345_Err], [Sigma_NeV_3345], [Sigma_NeV_3345_Err], [Amplitude_NeV_3345], [Amplitude_NeV_3345_Err], [Flux_NeV_3345], [Flux_NeV_3345_Err], [EW_NeV_3345], [EW_NeV_3345_Err], [Flux_Cont_NeV_3345], [Flux_Cont_NeV_3345_Err], [Fit_Warning_NeV_3345], [AoN_NeV_3345], [V_NeV_3425], [V_NeV_3425_Err], [Sigma_NeV_3425], [Sigma_NeV_3425_Err], [Amplitude_NeV_3425], [Amplitude_NeV_3425_Err], [Flux_NeV_3425], [Flux_NeV_3425_Err], [EW_NeV_3425], [EW_NeV_3425_Err], [Flux_Cont_NeV_3425], [Flux_Cont_NeV_3425_Err], [Fit_Warning_NeV_3425], [AoN_NeV_3425], [V_OII_3726], [V_OII_3726_Err], [Sigma_OII_3726], [Sigma_OII_3726_Err], [Amplitude_OII_3726], [Amplitude_OII_3726_Err], [Flux_OII_3726], [Flux_OII_3726_Err], [EW_OII_3726], [EW_OII_3726_Err], [Flux_Cont_OII_3726], [Flux_Cont_OII_3726_Err], [Fit_Warning_OII_3726], [AoN_OII_3726], [V_OII_3728], [V_OII_3728_Err], [Sigma_OII_3728], [Sigma_OII_3728_Err], [Amplitude_OII_3728], [Amplitude_OII_3728_Err], [Flux_OII_3728], [Flux_OII_3728_Err], [EW_OII_3728], [EW_OII_3728_Err], [Flux_Cont_OII_3728], [Flux_Cont_OII_3728_Err], [Fit_Warning_OII_3728], [AoN_OII_3728], [V_NeIII_3868], [V_NeIII_3868_Err], [Sigma_NeIII_3868], [Sigma_NeIII_3868_Err], [Amplitude_NeIII_3868], [Amplitude_NeIII_3868_Err], [Flux_NeIII_3868], [Flux_NeIII_3868_Err], [EW_NeIII_3868], [EW_NeIII_3868_Err], [Flux_Cont_NeIII_3868], [Flux_Cont_NeIII_3868_Err], [Fit_Warning_NeIII_3868], [AoN_NeIII_3868], [V_NeIII_3967], [V_NeIII_3967_Err], [Sigma_NeIII_3967], [Sigma_NeIII_3967_Err], [Amplitude_NeIII_3967], [Amplitude_NeIII_3967_Err], [Flux_NeIII_3967], [Flux_NeIII_3967_Err], [EW_NeIII_3967], [EW_NeIII_3967_Err], [Flux_Cont_NeIII_3967], [Flux_Cont_NeIII_3967_Err], [Fit_Warning_NeIII_3967], [AoN_NeIII_3967], [V_H5_3889], [V_H5_3889_Err], [Sigma_H5_3889], [Sigma_H5_3889_Err], [Amplitude_H5_3889], [Amplitude_H5_3889_Err], [Flux_H5_3889], [Flux_H5_3889_Err], [EW_H5_3889], [EW_H5_3889_Err], [Flux_Cont_H5_3889], [Flux_Cont_H5_3889_Err], [Fit_Warning_H5_3889], [AoN_H5_3889], [V_He_3970], [V_He_3970_Err], [Sigma_He_3970], [Sigma_He_3970_Err], [Amplitude_He_3970], [Amplitude_He_3970_Err], [Flux_He_3970], [Flux_He_3970_Err], [EW_He_3970], [EW_He_3970_Err], [Flux_Cont_He_3970], [Flux_Cont_He_3970_Err], [Fit_Warning_He_3970], [AoN_He_3970], [V_Hd_4101], [V_Hd_4101_Err], [Sigma_Hd_4101], [Sigma_Hd_4101_Err], [Amplitude_Hd_4101], [Amplitude_Hd_4101_Err], [Flux_Hd_4101], [Flux_Hd_4101_Err], [EW_Hd_4101], [EW_Hd_4101_Err], [Flux_Cont_Hd_4101], [Flux_Cont_Hd_4101_Err], [Fit_Warning_Hd_4101], [AoN_Hd_4101], [V_Hg_4340], [V_Hg_4340_Err], [Sigma_Hg_4340], [Sigma_Hg_4340_Err], [Amplitude_Hg_4340], [Amplitude_Hg_4340_Err], [Flux_Hg_4340], [Flux_Hg_4340_Err], [EW_Hg_4340], [EW_Hg_4340_Err], [Flux_Cont_Hg_4340], [Flux_Cont_Hg_4340_Err], [Fit_Warning_Hg_4340], [AoN_Hg_4340], [V_OIII_4363], [V_OIII_4363_Err], [Sigma_OIII_4363], [Sigma_OIII_4363_Err], [Amplitude_OIII_4363], [Amplitude_OIII_4363_Err], [Flux_OIII_4363], [Flux_OIII_4363_Err], [EW_OIII_4363], [EW_OIII_4363_Err], [Flux_Cont_OIII_4363], [Flux_Cont_OIII_4363_Err], [Fit_Warning_OIII_4363], [AoN_OIII_4363], [V_HeII_4685], [V_HeII_4685_Err], [Sigma_HeII_4685], [Sigma_HeII_4685_Err], [Amplitude_HeII_4685], [Amplitude_HeII_4685_Err], [Flux_HeII_4685], [Flux_HeII_4685_Err], [EW_HeII_4685], [EW_HeII_4685_Err], [Flux_Cont_HeII_4685], [Flux_Cont_HeII_4685_Err], [Fit_Warning_HeII_4685], [AoN_HeII_4685], [V_ArIV_4711], [V_ArIV_4711_Err], [Sigma_ArIV_4711], [Sigma_ArIV_4711_Err], [Amplitude_ArIV_4711], [Amplitude_ArIV_4711_Err], [Flux_ArIV_4711], [Flux_ArIV_4711_Err], [EW_ArIV_4711], [EW_ArIV_4711_Err], [Flux_Cont_ArIV_4711], [Flux_Cont_ArIV_4711_Err], [Fit_Warning_ArIV_4711], [AoN_ArIV_4711], [V_ArIV_4740], [V_ArIV_4740_Err], [Sigma_ArIV_4740], [Sigma_ArIV_4740_Err], [Amplitude_ArIV_4740], [Amplitude_ArIV_4740_Err], [Flux_ArIV_4740], [Flux_ArIV_4740_Err], [EW_ArIV_4740], [EW_ArIV_4740_Err], [Flux_Cont_ArIV_4740], [Flux_Cont_ArIV_4740_Err], [Fit_Warning_ArIV_4740], [AoN_ArIV_4740], [V_Hb_4861], [V_Hb_4861_Err], [Sigma_Hb_4861], [Sigma_Hb_4861_Err], [Amplitude_Hb_4861], [Amplitude_Hb_4861_Err], [Flux_Hb_4861], [Flux_Hb_4861_Err], [EW_Hb_4861], [EW_Hb_4861_Err], [Flux_Cont_Hb_4861], [Flux_Cont_Hb_4861_Err], [Fit_Warning_Hb_4861], [AoN_Hb_4861], [V_OIII_4958], [V_OIII_4958_Err], [Sigma_OIII_4958], [Sigma_OIII_4958_Err], [Amplitude_OIII_4958], [Amplitude_OIII_4958_Err], [Flux_OIII_4958], [Flux_OIII_4958_Err], [EW_OIII_4958], [EW_OIII_4958_Err], [Flux_Cont_OIII_4958], [Flux_Cont_OIII_4958_Err], [Fit_Warning_OIII_4958], [V_OIII_5006], [V_OIII_5006_Err], [Sigma_OIII_5006], [Sigma_OIII_5006_Err], [Amplitude_OIII_5006], [Amplitude_OIII_5006_Err], [Flux_OIII_5006], [Flux_OIII_5006_Err], [EW_OIII_5006], [EW_OIII_5006_Err], [Flux_Cont_OIII_5006], [Flux_Cont_OIII_5006_Err], [Fit_Warning_OIII_5006], [AoN_OIII_5006], [V_NI_5197], [V_NI_5197_Err], [Sigma_NI_5197], [Sigma_NI_5197_Err], [Amplitude_NI_5197], [Amplitude_NI_5197_Err], [Flux_NI_5197], [Flux_NI_5197_Err], [EW_NI_5197], [EW_NI_5197_Err], [Flux_Cont_NI_5197], [Flux_Cont_NI_5197_Err], [Fit_Warning_NI_5197], [AoN_NI_5197], [V_NI_5200], [V_NI_5200_Err], [Sigma_NI_5200], [Sigma_NI_5200_Err], [Amplitude_NI_5200], [Amplitude_NI_5200_Err], [Flux_NI_5200], [Flux_NI_5200_Err], [EW_NI_5200], [EW_NI_5200_Err], [Flux_Cont_NI_5200], [Flux_Cont_NI_5200_Err], [Fit_Warning_NI_5200], [AoN_NI_5200], [V_HeI_5875], [V_HeI_5875_Err], [Sigma_HeI_5875], [Sigma_HeI_5875_Err], [Amplitude_HeI_5875], [Amplitude_HeI_5875_Err], [Flux_HeI_5875], [Flux_HeI_5875_Err], [EW_HeI_5875], [EW_HeI_5875_Err], [Flux_Cont_HeI_5875], [Flux_Cont_HeI_5875_Err], [Fit_Warning_HeI_5875], [AoN_HeI_5875], [V_OI_6300], [V_OI_6300_Err], [Sigma_OI_6300], [Sigma_OI_6300_Err], [Amplitude_OI_6300], [Amplitude_OI_6300_Err], [Flux_OI_6300], [Flux_OI_6300_Err], [EW_OI_6300], [EW_OI_6300_Err], [Flux_Cont_OI_6300], [Flux_Cont_OI_6300_Err], [Fit_Warning_OI_6300], [AoN_OI_6300], [V_OI_6363], [V_OI_6363_Err], [Sigma_OI_6363], [Sigma_OI_6363_Err], [Amplitude_OI_6363], [Amplitude_OI_6363_Err], [Flux_OI_6363], [Flux_OI_6363_Err], [EW_OI_6363], [EW_OI_6363_Err], [Flux_Cont_OI_6363], [Flux_Cont_OI_6363_Err], [Fit_Warning_OI_6363], [AoN_OI_6363], [V_NII_6547], [V_NII_6547_Err], [Sigma_NII_6547], [Sigma_NII_6547_Err], [Amplitude_NII_6547], [Amplitude_NII_6547_Err], [Flux_NII_6547], [Flux_NII_6547_Err], [EW_NII_6547], [EW_NII_6547_Err], [Flux_Cont_NII_6547], [Flux_Cont_NII_6547_Err], [Fit_Warning_NII_6547], [V_Ha_6562], [V_Ha_6562_Err], [Sigma_Ha_6562], [Sigma_Ha_6562_Err], [Amplitude_Ha_6562], [Amplitude_Ha_6562_Err], [Flux_Ha_6562], [Flux_Ha_6562_Err], [EW_Ha_6562], [EW_Ha_6562_Err], [Flux_Cont_Ha_6562], [Flux_Cont_Ha_6562_Err], [Fit_Warning_Ha_6562], [AoN_Ha_6562], [V_NII_6583], [V_NII_6583_Err], [Sigma_NII_6583], [Sigma_NII_6583_Err], [Amplitude_NII_6583], [Amplitude_NII_6583_Err], [Flux_NII_6583], [Flux_NII_6583_Err], [EW_NII_6583], [EW_NII_6583_Err], [Flux_Cont_NII_6583], [Flux_Cont_NII_6583_Err], [Fit_Warning_NII_6583], [AoN_NII_6583], [V_SII_6716], [V_SII_6716_Err], [Sigma_SII_6716], [Sigma_SII_6716_Err], [Amplitude_SII_6716], [Amplitude_SII_6716_Err], [Flux_SII_6716], [Flux_SII_6716_Err], [EW_SII_6716], [EW_SII_6716_Err], [Flux_Cont_SII_6716], [Flux_Cont_SII_6716_Err], [Fit_Warning_SII_6716], [AoN_SII_6716], [V_SII_6730], [V_SII_6730_Err], [Sigma_SII_6730], [Sigma_SII_6730_Err], [Amplitude_SII_6730], [Amplitude_SII_6730_Err], [Flux_SII_6730], [Flux_SII_6730_Err], [EW_SII_6730], [EW_SII_6730_Err], [Flux_Cont_SII_6730], [Flux_Cont_SII_6730_Err], [Fit_Warning_SII_6730], [AoN_SII_6730])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[plate], sourcetablealias.[fiberID], sourcetablealias.[mjd], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[z], sourcetablealias.[zErr], sourcetablealias.[zNum], sourcetablealias.[velStars], sourcetablealias.[redshift], sourcetablealias.[sigmaStars], sourcetablealias.[sigmaStarsErr], sourcetablealias.[chisq], sourcetablealias.[bpt], sourcetablealias.[ebmv], sourcetablealias.[ebmv_err], sourcetablealias.[Error_Warning], sourcetablealias.[V_HeII_3203], sourcetablealias.[V_HeII_3203_Err], sourcetablealias.[Sigma_HeII_3203], sourcetablealias.[Sigma_HeII_3203_Err], sourcetablealias.[Amplitude_HeII_3203], sourcetablealias.[Amplitude_HeII_3203_Err], sourcetablealias.[Flux_HeII_3203], sourcetablealias.[Flux_HeII_3203_Err], sourcetablealias.[EW_HeII_3203], sourcetablealias.[EW_HeII_3203_Err], sourcetablealias.[Flux_Cont_HeII_3203], sourcetablealias.[Flux_Cont_HeII_3203_Err], sourcetablealias.[Fit_Warning_HeII_3203], sourcetablealias.[AoN_HeII_3203], sourcetablealias.[V_NeV_3345], sourcetablealias.[V_NeV_3345_Err], sourcetablealias.[Sigma_NeV_3345], sourcetablealias.[Sigma_NeV_3345_Err], sourcetablealias.[Amplitude_NeV_3345], sourcetablealias.[Amplitude_NeV_3345_Err], sourcetablealias.[Flux_NeV_3345], sourcetablealias.[Flux_NeV_3345_Err], sourcetablealias.[EW_NeV_3345], sourcetablealias.[EW_NeV_3345_Err], sourcetablealias.[Flux_Cont_NeV_3345], sourcetablealias.[Flux_Cont_NeV_3345_Err], sourcetablealias.[Fit_Warning_NeV_3345], sourcetablealias.[AoN_NeV_3345], sourcetablealias.[V_NeV_3425], sourcetablealias.[V_NeV_3425_Err], sourcetablealias.[Sigma_NeV_3425], sourcetablealias.[Sigma_NeV_3425_Err], sourcetablealias.[Amplitude_NeV_3425], sourcetablealias.[Amplitude_NeV_3425_Err], sourcetablealias.[Flux_NeV_3425], sourcetablealias.[Flux_NeV_3425_Err], sourcetablealias.[EW_NeV_3425], sourcetablealias.[EW_NeV_3425_Err], sourcetablealias.[Flux_Cont_NeV_3425], sourcetablealias.[Flux_Cont_NeV_3425_Err], sourcetablealias.[Fit_Warning_NeV_3425], sourcetablealias.[AoN_NeV_3425], sourcetablealias.[V_OII_3726], sourcetablealias.[V_OII_3726_Err], sourcetablealias.[Sigma_OII_3726], sourcetablealias.[Sigma_OII_3726_Err], sourcetablealias.[Amplitude_OII_3726], sourcetablealias.[Amplitude_OII_3726_Err], sourcetablealias.[Flux_OII_3726], sourcetablealias.[Flux_OII_3726_Err], sourcetablealias.[EW_OII_3726], sourcetablealias.[EW_OII_3726_Err], sourcetablealias.[Flux_Cont_OII_3726], sourcetablealias.[Flux_Cont_OII_3726_Err], sourcetablealias.[Fit_Warning_OII_3726], sourcetablealias.[AoN_OII_3726], sourcetablealias.[V_OII_3728], sourcetablealias.[V_OII_3728_Err], sourcetablealias.[Sigma_OII_3728], sourcetablealias.[Sigma_OII_3728_Err], sourcetablealias.[Amplitude_OII_3728], sourcetablealias.[Amplitude_OII_3728_Err], sourcetablealias.[Flux_OII_3728], sourcetablealias.[Flux_OII_3728_Err], sourcetablealias.[EW_OII_3728], sourcetablealias.[EW_OII_3728_Err], sourcetablealias.[Flux_Cont_OII_3728], sourcetablealias.[Flux_Cont_OII_3728_Err], sourcetablealias.[Fit_Warning_OII_3728], sourcetablealias.[AoN_OII_3728], sourcetablealias.[V_NeIII_3868], sourcetablealias.[V_NeIII_3868_Err], sourcetablealias.[Sigma_NeIII_3868], sourcetablealias.[Sigma_NeIII_3868_Err], sourcetablealias.[Amplitude_NeIII_3868], sourcetablealias.[Amplitude_NeIII_3868_Err], sourcetablealias.[Flux_NeIII_3868], sourcetablealias.[Flux_NeIII_3868_Err], sourcetablealias.[EW_NeIII_3868], sourcetablealias.[EW_NeIII_3868_Err], sourcetablealias.[Flux_Cont_NeIII_3868], sourcetablealias.[Flux_Cont_NeIII_3868_Err], sourcetablealias.[Fit_Warning_NeIII_3868], sourcetablealias.[AoN_NeIII_3868], sourcetablealias.[V_NeIII_3967], sourcetablealias.[V_NeIII_3967_Err], sourcetablealias.[Sigma_NeIII_3967], sourcetablealias.[Sigma_NeIII_3967_Err], sourcetablealias.[Amplitude_NeIII_3967], sourcetablealias.[Amplitude_NeIII_3967_Err], sourcetablealias.[Flux_NeIII_3967], sourcetablealias.[Flux_NeIII_3967_Err], sourcetablealias.[EW_NeIII_3967], sourcetablealias.[EW_NeIII_3967_Err], sourcetablealias.[Flux_Cont_NeIII_3967], sourcetablealias.[Flux_Cont_NeIII_3967_Err], sourcetablealias.[Fit_Warning_NeIII_3967], sourcetablealias.[AoN_NeIII_3967], sourcetablealias.[V_H5_3889], sourcetablealias.[V_H5_3889_Err], sourcetablealias.[Sigma_H5_3889], sourcetablealias.[Sigma_H5_3889_Err], sourcetablealias.[Amplitude_H5_3889], sourcetablealias.[Amplitude_H5_3889_Err], sourcetablealias.[Flux_H5_3889], sourcetablealias.[Flux_H5_3889_Err], sourcetablealias.[EW_H5_3889], sourcetablealias.[EW_H5_3889_Err], sourcetablealias.[Flux_Cont_H5_3889], sourcetablealias.[Flux_Cont_H5_3889_Err], sourcetablealias.[Fit_Warning_H5_3889], sourcetablealias.[AoN_H5_3889], sourcetablealias.[V_He_3970], sourcetablealias.[V_He_3970_Err], sourcetablealias.[Sigma_He_3970], sourcetablealias.[Sigma_He_3970_Err], sourcetablealias.[Amplitude_He_3970], sourcetablealias.[Amplitude_He_3970_Err], sourcetablealias.[Flux_He_3970], sourcetablealias.[Flux_He_3970_Err], sourcetablealias.[EW_He_3970], sourcetablealias.[EW_He_3970_Err], sourcetablealias.[Flux_Cont_He_3970], sourcetablealias.[Flux_Cont_He_3970_Err], sourcetablealias.[Fit_Warning_He_3970], sourcetablealias.[AoN_He_3970], sourcetablealias.[V_Hd_4101], sourcetablealias.[V_Hd_4101_Err], sourcetablealias.[Sigma_Hd_4101], sourcetablealias.[Sigma_Hd_4101_Err], sourcetablealias.[Amplitude_Hd_4101], sourcetablealias.[Amplitude_Hd_4101_Err], sourcetablealias.[Flux_Hd_4101], sourcetablealias.[Flux_Hd_4101_Err], sourcetablealias.[EW_Hd_4101], sourcetablealias.[EW_Hd_4101_Err], sourcetablealias.[Flux_Cont_Hd_4101], sourcetablealias.[Flux_Cont_Hd_4101_Err], sourcetablealias.[Fit_Warning_Hd_4101], sourcetablealias.[AoN_Hd_4101], sourcetablealias.[V_Hg_4340], sourcetablealias.[V_Hg_4340_Err], sourcetablealias.[Sigma_Hg_4340], sourcetablealias.[Sigma_Hg_4340_Err], sourcetablealias.[Amplitude_Hg_4340], sourcetablealias.[Amplitude_Hg_4340_Err], sourcetablealias.[Flux_Hg_4340], sourcetablealias.[Flux_Hg_4340_Err], sourcetablealias.[EW_Hg_4340], sourcetablealias.[EW_Hg_4340_Err], sourcetablealias.[Flux_Cont_Hg_4340], sourcetablealias.[Flux_Cont_Hg_4340_Err], sourcetablealias.[Fit_Warning_Hg_4340], sourcetablealias.[AoN_Hg_4340], sourcetablealias.[V_OIII_4363], sourcetablealias.[V_OIII_4363_Err], sourcetablealias.[Sigma_OIII_4363], sourcetablealias.[Sigma_OIII_4363_Err], sourcetablealias.[Amplitude_OIII_4363], sourcetablealias.[Amplitude_OIII_4363_Err], sourcetablealias.[Flux_OIII_4363], sourcetablealias.[Flux_OIII_4363_Err], sourcetablealias.[EW_OIII_4363], sourcetablealias.[EW_OIII_4363_Err], sourcetablealias.[Flux_Cont_OIII_4363], sourcetablealias.[Flux_Cont_OIII_4363_Err], sourcetablealias.[Fit_Warning_OIII_4363], sourcetablealias.[AoN_OIII_4363], sourcetablealias.[V_HeII_4685], sourcetablealias.[V_HeII_4685_Err], sourcetablealias.[Sigma_HeII_4685], sourcetablealias.[Sigma_HeII_4685_Err], sourcetablealias.[Amplitude_HeII_4685], sourcetablealias.[Amplitude_HeII_4685_Err], sourcetablealias.[Flux_HeII_4685], sourcetablealias.[Flux_HeII_4685_Err], sourcetablealias.[EW_HeII_4685], sourcetablealias.[EW_HeII_4685_Err], sourcetablealias.[Flux_Cont_HeII_4685], sourcetablealias.[Flux_Cont_HeII_4685_Err], sourcetablealias.[Fit_Warning_HeII_4685], sourcetablealias.[AoN_HeII_4685], sourcetablealias.[V_ArIV_4711], sourcetablealias.[V_ArIV_4711_Err], sourcetablealias.[Sigma_ArIV_4711], sourcetablealias.[Sigma_ArIV_4711_Err], sourcetablealias.[Amplitude_ArIV_4711], sourcetablealias.[Amplitude_ArIV_4711_Err], sourcetablealias.[Flux_ArIV_4711], sourcetablealias.[Flux_ArIV_4711_Err], sourcetablealias.[EW_ArIV_4711], sourcetablealias.[EW_ArIV_4711_Err], sourcetablealias.[Flux_Cont_ArIV_4711], sourcetablealias.[Flux_Cont_ArIV_4711_Err], sourcetablealias.[Fit_Warning_ArIV_4711], sourcetablealias.[AoN_ArIV_4711], sourcetablealias.[V_ArIV_4740], sourcetablealias.[V_ArIV_4740_Err], sourcetablealias.[Sigma_ArIV_4740], sourcetablealias.[Sigma_ArIV_4740_Err], sourcetablealias.[Amplitude_ArIV_4740], sourcetablealias.[Amplitude_ArIV_4740_Err], sourcetablealias.[Flux_ArIV_4740], sourcetablealias.[Flux_ArIV_4740_Err], sourcetablealias.[EW_ArIV_4740], sourcetablealias.[EW_ArIV_4740_Err], sourcetablealias.[Flux_Cont_ArIV_4740], sourcetablealias.[Flux_Cont_ArIV_4740_Err], sourcetablealias.[Fit_Warning_ArIV_4740], sourcetablealias.[AoN_ArIV_4740], sourcetablealias.[V_Hb_4861], sourcetablealias.[V_Hb_4861_Err], sourcetablealias.[Sigma_Hb_4861], sourcetablealias.[Sigma_Hb_4861_Err], sourcetablealias.[Amplitude_Hb_4861], sourcetablealias.[Amplitude_Hb_4861_Err], sourcetablealias.[Flux_Hb_4861], sourcetablealias.[Flux_Hb_4861_Err], sourcetablealias.[EW_Hb_4861], sourcetablealias.[EW_Hb_4861_Err], sourcetablealias.[Flux_Cont_Hb_4861], sourcetablealias.[Flux_Cont_Hb_4861_Err], sourcetablealias.[Fit_Warning_Hb_4861], sourcetablealias.[AoN_Hb_4861], sourcetablealias.[V_OIII_4958], sourcetablealias.[V_OIII_4958_Err], sourcetablealias.[Sigma_OIII_4958], sourcetablealias.[Sigma_OIII_4958_Err], sourcetablealias.[Amplitude_OIII_4958], sourcetablealias.[Amplitude_OIII_4958_Err], sourcetablealias.[Flux_OIII_4958], sourcetablealias.[Flux_OIII_4958_Err], sourcetablealias.[EW_OIII_4958], sourcetablealias.[EW_OIII_4958_Err], sourcetablealias.[Flux_Cont_OIII_4958], sourcetablealias.[Flux_Cont_OIII_4958_Err], sourcetablealias.[Fit_Warning_OIII_4958], sourcetablealias.[V_OIII_5006], sourcetablealias.[V_OIII_5006_Err], sourcetablealias.[Sigma_OIII_5006], sourcetablealias.[Sigma_OIII_5006_Err], sourcetablealias.[Amplitude_OIII_5006], sourcetablealias.[Amplitude_OIII_5006_Err], sourcetablealias.[Flux_OIII_5006], sourcetablealias.[Flux_OIII_5006_Err], sourcetablealias.[EW_OIII_5006], sourcetablealias.[EW_OIII_5006_Err], sourcetablealias.[Flux_Cont_OIII_5006], sourcetablealias.[Flux_Cont_OIII_5006_Err], sourcetablealias.[Fit_Warning_OIII_5006], sourcetablealias.[AoN_OIII_5006], sourcetablealias.[V_NI_5197], sourcetablealias.[V_NI_5197_Err], sourcetablealias.[Sigma_NI_5197], sourcetablealias.[Sigma_NI_5197_Err], sourcetablealias.[Amplitude_NI_5197], sourcetablealias.[Amplitude_NI_5197_Err], sourcetablealias.[Flux_NI_5197], sourcetablealias.[Flux_NI_5197_Err], sourcetablealias.[EW_NI_5197], sourcetablealias.[EW_NI_5197_Err], sourcetablealias.[Flux_Cont_NI_5197], sourcetablealias.[Flux_Cont_NI_5197_Err], sourcetablealias.[Fit_Warning_NI_5197], sourcetablealias.[AoN_NI_5197], sourcetablealias.[V_NI_5200], sourcetablealias.[V_NI_5200_Err], sourcetablealias.[Sigma_NI_5200], sourcetablealias.[Sigma_NI_5200_Err], sourcetablealias.[Amplitude_NI_5200], sourcetablealias.[Amplitude_NI_5200_Err], sourcetablealias.[Flux_NI_5200], sourcetablealias.[Flux_NI_5200_Err], sourcetablealias.[EW_NI_5200], sourcetablealias.[EW_NI_5200_Err], sourcetablealias.[Flux_Cont_NI_5200], sourcetablealias.[Flux_Cont_NI_5200_Err], sourcetablealias.[Fit_Warning_NI_5200], sourcetablealias.[AoN_NI_5200], sourcetablealias.[V_HeI_5875], sourcetablealias.[V_HeI_5875_Err], sourcetablealias.[Sigma_HeI_5875], sourcetablealias.[Sigma_HeI_5875_Err], sourcetablealias.[Amplitude_HeI_5875], sourcetablealias.[Amplitude_HeI_5875_Err], sourcetablealias.[Flux_HeI_5875], sourcetablealias.[Flux_HeI_5875_Err], sourcetablealias.[EW_HeI_5875], sourcetablealias.[EW_HeI_5875_Err], sourcetablealias.[Flux_Cont_HeI_5875], sourcetablealias.[Flux_Cont_HeI_5875_Err], sourcetablealias.[Fit_Warning_HeI_5875], sourcetablealias.[AoN_HeI_5875], sourcetablealias.[V_OI_6300], sourcetablealias.[V_OI_6300_Err], sourcetablealias.[Sigma_OI_6300], sourcetablealias.[Sigma_OI_6300_Err], sourcetablealias.[Amplitude_OI_6300], sourcetablealias.[Amplitude_OI_6300_Err], sourcetablealias.[Flux_OI_6300], sourcetablealias.[Flux_OI_6300_Err], sourcetablealias.[EW_OI_6300], sourcetablealias.[EW_OI_6300_Err], sourcetablealias.[Flux_Cont_OI_6300], sourcetablealias.[Flux_Cont_OI_6300_Err], sourcetablealias.[Fit_Warning_OI_6300], sourcetablealias.[AoN_OI_6300], sourcetablealias.[V_OI_6363], sourcetablealias.[V_OI_6363_Err], sourcetablealias.[Sigma_OI_6363], sourcetablealias.[Sigma_OI_6363_Err], sourcetablealias.[Amplitude_OI_6363], sourcetablealias.[Amplitude_OI_6363_Err], sourcetablealias.[Flux_OI_6363], sourcetablealias.[Flux_OI_6363_Err], sourcetablealias.[EW_OI_6363], sourcetablealias.[EW_OI_6363_Err], sourcetablealias.[Flux_Cont_OI_6363], sourcetablealias.[Flux_Cont_OI_6363_Err], sourcetablealias.[Fit_Warning_OI_6363], sourcetablealias.[AoN_OI_6363], sourcetablealias.[V_NII_6547], sourcetablealias.[V_NII_6547_Err], sourcetablealias.[Sigma_NII_6547], sourcetablealias.[Sigma_NII_6547_Err], sourcetablealias.[Amplitude_NII_6547], sourcetablealias.[Amplitude_NII_6547_Err], sourcetablealias.[Flux_NII_6547], sourcetablealias.[Flux_NII_6547_Err], sourcetablealias.[EW_NII_6547], sourcetablealias.[EW_NII_6547_Err], sourcetablealias.[Flux_Cont_NII_6547], sourcetablealias.[Flux_Cont_NII_6547_Err], sourcetablealias.[Fit_Warning_NII_6547], sourcetablealias.[V_Ha_6562], sourcetablealias.[V_Ha_6562_Err], sourcetablealias.[Sigma_Ha_6562], sourcetablealias.[Sigma_Ha_6562_Err], sourcetablealias.[Amplitude_Ha_6562], sourcetablealias.[Amplitude_Ha_6562_Err], sourcetablealias.[Flux_Ha_6562], sourcetablealias.[Flux_Ha_6562_Err], sourcetablealias.[EW_Ha_6562], sourcetablealias.[EW_Ha_6562_Err], sourcetablealias.[Flux_Cont_Ha_6562], sourcetablealias.[Flux_Cont_Ha_6562_Err], sourcetablealias.[Fit_Warning_Ha_6562], sourcetablealias.[AoN_Ha_6562], sourcetablealias.[V_NII_6583], sourcetablealias.[V_NII_6583_Err], sourcetablealias.[Sigma_NII_6583], sourcetablealias.[Sigma_NII_6583_Err], sourcetablealias.[Amplitude_NII_6583], sourcetablealias.[Amplitude_NII_6583_Err], sourcetablealias.[Flux_NII_6583], sourcetablealias.[Flux_NII_6583_Err], sourcetablealias.[EW_NII_6583], sourcetablealias.[EW_NII_6583_Err], sourcetablealias.[Flux_Cont_NII_6583], sourcetablealias.[Flux_Cont_NII_6583_Err], sourcetablealias.[Fit_Warning_NII_6583], sourcetablealias.[AoN_NII_6583], sourcetablealias.[V_SII_6716], sourcetablealias.[V_SII_6716_Err], sourcetablealias.[Sigma_SII_6716], sourcetablealias.[Sigma_SII_6716_Err], sourcetablealias.[Amplitude_SII_6716], sourcetablealias.[Amplitude_SII_6716_Err], sourcetablealias.[Flux_SII_6716], sourcetablealias.[Flux_SII_6716_Err], sourcetablealias.[EW_SII_6716], sourcetablealias.[EW_SII_6716_Err], sourcetablealias.[Flux_Cont_SII_6716], sourcetablealias.[Flux_Cont_SII_6716_Err], sourcetablealias.[Fit_Warning_SII_6716], sourcetablealias.[AoN_SII_6716], sourcetablealias.[V_SII_6730], sourcetablealias.[V_SII_6730_Err], sourcetablealias.[Sigma_SII_6730], sourcetablealias.[Sigma_SII_6730_Err], sourcetablealias.[Amplitude_SII_6730], sourcetablealias.[Amplitude_SII_6730_Err], sourcetablealias.[Flux_SII_6730], sourcetablealias.[Flux_SII_6730_Err], sourcetablealias.[EW_SII_6730], sourcetablealias.[EW_SII_6730_Err], sourcetablealias.[Flux_Cont_SII_6730], sourcetablealias.[Flux_Cont_SII_6730_Err], sourcetablealias.[Fit_Warning_SII_6730], sourcetablealias.[AoN_SII_6730]
 FROM   [SkyNode_SDSSDR12].[dbo].[emissionLinesPort] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'Field' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[fieldID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[fieldID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[Field] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [fieldID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[Field] WITH (TABLOCKX)
	([fieldID], [skyVersion], [run], [rerun], [camcol], [field], [nTotal], [nObjects], [nChild], [nGalaxy], [nStars], [nCR_u], [nCR_g], [nCR_r], [nCR_i], [nCR_z], [nBrightObj_u], [nBrightObj_g], [nBrightObj_r], [nBrightObj_i], [nBrightObj_z], [nFaintObj_u], [nFaintObj_g], [nFaintObj_r], [nFaintObj_i], [nFaintObj_z], [quality], [mjd_u], [mjd_g], [mjd_r], [mjd_i], [mjd_z], [a_u], [a_g], [a_r], [a_i], [a_z], [b_u], [b_g], [b_r], [b_i], [b_z], [c_u], [c_g], [c_r], [c_i], [c_z], [d_u], [d_g], [d_r], [d_i], [d_z], [e_u], [e_g], [e_r], [e_i], [e_z], [f_u], [f_g], [f_r], [f_i], [f_z], [dRow0_u], [dRow0_g], [dRow0_r], [dRow0_i], [dRow0_z], [dRow1_u], [dRow1_g], [dRow1_r], [dRow1_i], [dRow1_z], [dRow2_u], [dRow2_g], [dRow2_r], [dRow2_i], [dRow2_z], [dRow3_u], [dRow3_g], [dRow3_r], [dRow3_i], [dRow3_z], [dCol0_u], [dCol0_g], [dCol0_r], [dCol0_i], [dCol0_z], [dCol1_u], [dCol1_g], [dCol1_r], [dCol1_i], [dCol1_z], [dCol2_u], [dCol2_g], [dCol2_r], [dCol2_i], [dCol2_z], [dCol3_u], [dCol3_g], [dCol3_r], [dCol3_i], [dCol3_z], [csRow_u], [csRow_g], [csRow_r], [csRow_i], [csRow_z], [csCol_u], [csCol_g], [csCol_r], [csCol_i], [csCol_z], [ccRow_u], [ccRow_g], [ccRow_r], [ccRow_i], [ccRow_z], [ccCol_u], [ccCol_g], [ccCol_r], [ccCol_i], [ccCol_z], [riCut_u], [riCut_g], [riCut_r], [riCut_i], [riCut_z], [airmass_u], [airmass_g], [airmass_r], [airmass_i], [airmass_z], [muErr_u], [muErr_g], [muErr_r], [muErr_i], [muErr_z], [nuErr_u], [nuErr_g], [nuErr_r], [nuErr_i], [nuErr_z], [ra], [dec], [raMin], [raMax], [decMin], [decMax], [pixScale], [primaryArea], [photoStatus], [rowOffset_u], [rowOffset_g], [rowOffset_r], [rowOffset_i], [rowOffset_z], [colOffset_u], [colOffset_g], [colOffset_r], [colOffset_i], [colOffset_z], [saturationLevel_u], [saturationLevel_g], [saturationLevel_r], [saturationLevel_i], [saturationLevel_z], [nEffPsf_u], [nEffPsf_g], [nEffPsf_r], [nEffPsf_i], [nEffPsf_z], [skyPsp_u], [skyPsp_g], [skyPsp_r], [skyPsp_i], [skyPsp_z], [skyFrames_u], [skyFrames_g], [skyFrames_r], [skyFrames_i], [skyFrames_z], [skyFramesSub_u], [skyFramesSub_g], [skyFramesSub_r], [skyFramesSub_i], [skyFramesSub_z], [sigPix_u], [sigPix_g], [sigPix_r], [sigPix_i], [sigPix_z], [devApCorrection_u], [devApCorrection_g], [devApCorrection_r], [devApCorrection_i], [devApCorrection_z], [devApCorrectionErr_u], [devApCorrectionErr_g], [devApCorrectionErr_r], [devApCorrectionErr_i], [devApCorrectionErr_z], [expApCorrection_u], [expApCorrection_g], [expApCorrection_r], [expApCorrection_i], [expApCorrection_z], [expApCorrectionErr_u], [expApCorrectionErr_g], [expApCorrectionErr_r], [expApCorrectionErr_i], [expApCorrectionErr_z], [devModelApCorrection_u], [devModelApCorrection_g], [devModelApCorrection_r], [devModelApCorrection_i], [devModelApCorrection_z], [devModelApCorrectionErr_u], [devModelApCorrectionErr_g], [devModelApCorrectionErr_r], [devModelApCorrectionErr_i], [devModelApCorrectionErr_z], [expModelApCorrection_u], [expModelApCorrection_g], [expModelApCorrection_r], [expModelApCorrection_i], [expModelApCorrection_z], [expModelApCorrectionErr_u], [expModelApCorrectionErr_g], [expModelApCorrectionErr_r], [expModelApCorrectionErr_i], [expModelApCorrectionErr_z], [medianFiberColor_u], [medianFiberColor_g], [medianFiberColor_r], [medianFiberColor_i], [medianFiberColor_z], [medianPsfColor_u], [medianPsfColor_g], [medianPsfColor_r], [medianPsfColor_i], [medianPsfColor_z], [q_u], [q_g], [q_r], [q_i], [q_z], [u_u], [u_g], [u_r], [u_i], [u_z], [pspStatus], [sky_u], [sky_g], [sky_r], [sky_i], [sky_z], [skySig_u], [skySig_g], [skySig_r], [skySig_i], [skySig_z], [skyErr_u], [skyErr_g], [skyErr_r], [skyErr_i], [skyErr_z], [skySlope_u], [skySlope_g], [skySlope_r], [skySlope_i], [skySlope_z], [lbias_u], [lbias_g], [lbias_r], [lbias_i], [lbias_z], [rbias_u], [rbias_g], [rbias_r], [rbias_i], [rbias_z], [nProf_u], [nProf_g], [nProf_r], [nProf_i], [nProf_z], [psfNStar_u], [psfNStar_g], [psfNStar_r], [psfNStar_i], [psfNStar_z], [psfApCorrectionErr_u], [psfApCorrectionErr_g], [psfApCorrectionErr_r], [psfApCorrectionErr_i], [psfApCorrectionErr_z], [psfSigma1_u], [psfSigma1_g], [psfSigma1_r], [psfSigma1_i], [psfSigma1_z], [psfSigma2_u], [psfSigma2_g], [psfSigma2_r], [psfSigma2_i], [psfSigma2_z], [psfB_u], [psfB_g], [psfB_r], [psfB_i], [psfB_z], [psfP0_u], [psfP0_g], [psfP0_r], [psfP0_i], [psfP0_z], [psfBeta_u], [psfBeta_g], [psfBeta_r], [psfBeta_i], [psfBeta_z], [psfSigmaP_u], [psfSigmaP_g], [psfSigmaP_r], [psfSigmaP_i], [psfSigmaP_z], [psfWidth_u], [psfWidth_g], [psfWidth_r], [psfWidth_i], [psfWidth_z], [psfPsfCounts_u], [psfPsfCounts_g], [psfPsfCounts_r], [psfPsfCounts_i], [psfPsfCounts_z], [psf2GSigma1_u], [psf2GSigma1_g], [psf2GSigma1_r], [psf2GSigma1_i], [psf2GSigma1_z], [psf2GSigma2_u], [psf2GSigma2_g], [psf2GSigma2_r], [psf2GSigma2_i], [psf2GSigma2_z], [psf2GB_u], [psf2GB_g], [psf2GB_r], [psf2GB_i], [psf2GB_z], [psfCounts_u], [psfCounts_g], [psfCounts_r], [psfCounts_i], [psfCounts_z], [gain_u], [gain_g], [gain_r], [gain_i], [gain_z], [darkVariance_u], [darkVariance_g], [darkVariance_r], [darkVariance_i], [darkVariance_z], [score], [aterm_u], [aterm_g], [aterm_r], [aterm_i], [aterm_z], [kterm_u], [kterm_g], [kterm_r], [kterm_i], [kterm_z], [kdot_u], [kdot_g], [kdot_r], [kdot_i], [kdot_z], [reftai_u], [reftai_g], [reftai_r], [reftai_i], [reftai_z], [tai_u], [tai_g], [tai_r], [tai_i], [tai_z], [nStarsOffset_u], [nStarsOffset_g], [nStarsOffset_r], [nStarsOffset_i], [nStarsOffset_z], [fieldOffset_u], [fieldOffset_g], [fieldOffset_r], [fieldOffset_i], [fieldOffset_z], [calibStatus_u], [calibStatus_g], [calibStatus_r], [calibStatus_i], [calibStatus_z], [imageStatus_u], [imageStatus_g], [imageStatus_r], [imageStatus_i], [imageStatus_z], [nMgyPerCount_u], [nMgyPerCount_g], [nMgyPerCount_r], [nMgyPerCount_i], [nMgyPerCount_z], [nMgyPerCountIvar_u], [nMgyPerCountIvar_g], [nMgyPerCountIvar_r], [nMgyPerCountIvar_i], [nMgyPerCountIvar_z], [ifield], [muStart], [muEnd], [nuStart], [nuEnd], [ifindx], [nBalkans], [loadVersion])
 SELECT sourcetablealias.[fieldID], sourcetablealias.[skyVersion], sourcetablealias.[run], sourcetablealias.[rerun], sourcetablealias.[camcol], sourcetablealias.[field], sourcetablealias.[nTotal], sourcetablealias.[nObjects], sourcetablealias.[nChild], sourcetablealias.[nGalaxy], sourcetablealias.[nStars], sourcetablealias.[nCR_u], sourcetablealias.[nCR_g], sourcetablealias.[nCR_r], sourcetablealias.[nCR_i], sourcetablealias.[nCR_z], sourcetablealias.[nBrightObj_u], sourcetablealias.[nBrightObj_g], sourcetablealias.[nBrightObj_r], sourcetablealias.[nBrightObj_i], sourcetablealias.[nBrightObj_z], sourcetablealias.[nFaintObj_u], sourcetablealias.[nFaintObj_g], sourcetablealias.[nFaintObj_r], sourcetablealias.[nFaintObj_i], sourcetablealias.[nFaintObj_z], sourcetablealias.[quality], sourcetablealias.[mjd_u], sourcetablealias.[mjd_g], sourcetablealias.[mjd_r], sourcetablealias.[mjd_i], sourcetablealias.[mjd_z], sourcetablealias.[a_u], sourcetablealias.[a_g], sourcetablealias.[a_r], sourcetablealias.[a_i], sourcetablealias.[a_z], sourcetablealias.[b_u], sourcetablealias.[b_g], sourcetablealias.[b_r], sourcetablealias.[b_i], sourcetablealias.[b_z], sourcetablealias.[c_u], sourcetablealias.[c_g], sourcetablealias.[c_r], sourcetablealias.[c_i], sourcetablealias.[c_z], sourcetablealias.[d_u], sourcetablealias.[d_g], sourcetablealias.[d_r], sourcetablealias.[d_i], sourcetablealias.[d_z], sourcetablealias.[e_u], sourcetablealias.[e_g], sourcetablealias.[e_r], sourcetablealias.[e_i], sourcetablealias.[e_z], sourcetablealias.[f_u], sourcetablealias.[f_g], sourcetablealias.[f_r], sourcetablealias.[f_i], sourcetablealias.[f_z], sourcetablealias.[dRow0_u], sourcetablealias.[dRow0_g], sourcetablealias.[dRow0_r], sourcetablealias.[dRow0_i], sourcetablealias.[dRow0_z], sourcetablealias.[dRow1_u], sourcetablealias.[dRow1_g], sourcetablealias.[dRow1_r], sourcetablealias.[dRow1_i], sourcetablealias.[dRow1_z], sourcetablealias.[dRow2_u], sourcetablealias.[dRow2_g], sourcetablealias.[dRow2_r], sourcetablealias.[dRow2_i], sourcetablealias.[dRow2_z], sourcetablealias.[dRow3_u], sourcetablealias.[dRow3_g], sourcetablealias.[dRow3_r], sourcetablealias.[dRow3_i], sourcetablealias.[dRow3_z], sourcetablealias.[dCol0_u], sourcetablealias.[dCol0_g], sourcetablealias.[dCol0_r], sourcetablealias.[dCol0_i], sourcetablealias.[dCol0_z], sourcetablealias.[dCol1_u], sourcetablealias.[dCol1_g], sourcetablealias.[dCol1_r], sourcetablealias.[dCol1_i], sourcetablealias.[dCol1_z], sourcetablealias.[dCol2_u], sourcetablealias.[dCol2_g], sourcetablealias.[dCol2_r], sourcetablealias.[dCol2_i], sourcetablealias.[dCol2_z], sourcetablealias.[dCol3_u], sourcetablealias.[dCol3_g], sourcetablealias.[dCol3_r], sourcetablealias.[dCol3_i], sourcetablealias.[dCol3_z], sourcetablealias.[csRow_u], sourcetablealias.[csRow_g], sourcetablealias.[csRow_r], sourcetablealias.[csRow_i], sourcetablealias.[csRow_z], sourcetablealias.[csCol_u], sourcetablealias.[csCol_g], sourcetablealias.[csCol_r], sourcetablealias.[csCol_i], sourcetablealias.[csCol_z], sourcetablealias.[ccRow_u], sourcetablealias.[ccRow_g], sourcetablealias.[ccRow_r], sourcetablealias.[ccRow_i], sourcetablealias.[ccRow_z], sourcetablealias.[ccCol_u], sourcetablealias.[ccCol_g], sourcetablealias.[ccCol_r], sourcetablealias.[ccCol_i], sourcetablealias.[ccCol_z], sourcetablealias.[riCut_u], sourcetablealias.[riCut_g], sourcetablealias.[riCut_r], sourcetablealias.[riCut_i], sourcetablealias.[riCut_z], sourcetablealias.[airmass_u], sourcetablealias.[airmass_g], sourcetablealias.[airmass_r], sourcetablealias.[airmass_i], sourcetablealias.[airmass_z], sourcetablealias.[muErr_u], sourcetablealias.[muErr_g], sourcetablealias.[muErr_r], sourcetablealias.[muErr_i], sourcetablealias.[muErr_z], sourcetablealias.[nuErr_u], sourcetablealias.[nuErr_g], sourcetablealias.[nuErr_r], sourcetablealias.[nuErr_i], sourcetablealias.[nuErr_z], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[raMin], sourcetablealias.[raMax], sourcetablealias.[decMin], sourcetablealias.[decMax], sourcetablealias.[pixScale], sourcetablealias.[primaryArea], sourcetablealias.[photoStatus], sourcetablealias.[rowOffset_u], sourcetablealias.[rowOffset_g], sourcetablealias.[rowOffset_r], sourcetablealias.[rowOffset_i], sourcetablealias.[rowOffset_z], sourcetablealias.[colOffset_u], sourcetablealias.[colOffset_g], sourcetablealias.[colOffset_r], sourcetablealias.[colOffset_i], sourcetablealias.[colOffset_z], sourcetablealias.[saturationLevel_u], sourcetablealias.[saturationLevel_g], sourcetablealias.[saturationLevel_r], sourcetablealias.[saturationLevel_i], sourcetablealias.[saturationLevel_z], sourcetablealias.[nEffPsf_u], sourcetablealias.[nEffPsf_g], sourcetablealias.[nEffPsf_r], sourcetablealias.[nEffPsf_i], sourcetablealias.[nEffPsf_z], sourcetablealias.[skyPsp_u], sourcetablealias.[skyPsp_g], sourcetablealias.[skyPsp_r], sourcetablealias.[skyPsp_i], sourcetablealias.[skyPsp_z], sourcetablealias.[skyFrames_u], sourcetablealias.[skyFrames_g], sourcetablealias.[skyFrames_r], sourcetablealias.[skyFrames_i], sourcetablealias.[skyFrames_z], sourcetablealias.[skyFramesSub_u], sourcetablealias.[skyFramesSub_g], sourcetablealias.[skyFramesSub_r], sourcetablealias.[skyFramesSub_i], sourcetablealias.[skyFramesSub_z], sourcetablealias.[sigPix_u], sourcetablealias.[sigPix_g], sourcetablealias.[sigPix_r], sourcetablealias.[sigPix_i], sourcetablealias.[sigPix_z], sourcetablealias.[devApCorrection_u], sourcetablealias.[devApCorrection_g], sourcetablealias.[devApCorrection_r], sourcetablealias.[devApCorrection_i], sourcetablealias.[devApCorrection_z], sourcetablealias.[devApCorrectionErr_u], sourcetablealias.[devApCorrectionErr_g], sourcetablealias.[devApCorrectionErr_r], sourcetablealias.[devApCorrectionErr_i], sourcetablealias.[devApCorrectionErr_z], sourcetablealias.[expApCorrection_u], sourcetablealias.[expApCorrection_g], sourcetablealias.[expApCorrection_r], sourcetablealias.[expApCorrection_i], sourcetablealias.[expApCorrection_z], sourcetablealias.[expApCorrectionErr_u], sourcetablealias.[expApCorrectionErr_g], sourcetablealias.[expApCorrectionErr_r], sourcetablealias.[expApCorrectionErr_i], sourcetablealias.[expApCorrectionErr_z], sourcetablealias.[devModelApCorrection_u], sourcetablealias.[devModelApCorrection_g], sourcetablealias.[devModelApCorrection_r], sourcetablealias.[devModelApCorrection_i], sourcetablealias.[devModelApCorrection_z], sourcetablealias.[devModelApCorrectionErr_u], sourcetablealias.[devModelApCorrectionErr_g], sourcetablealias.[devModelApCorrectionErr_r], sourcetablealias.[devModelApCorrectionErr_i], sourcetablealias.[devModelApCorrectionErr_z], sourcetablealias.[expModelApCorrection_u], sourcetablealias.[expModelApCorrection_g], sourcetablealias.[expModelApCorrection_r], sourcetablealias.[expModelApCorrection_i], sourcetablealias.[expModelApCorrection_z], sourcetablealias.[expModelApCorrectionErr_u], sourcetablealias.[expModelApCorrectionErr_g], sourcetablealias.[expModelApCorrectionErr_r], sourcetablealias.[expModelApCorrectionErr_i], sourcetablealias.[expModelApCorrectionErr_z], sourcetablealias.[medianFiberColor_u], sourcetablealias.[medianFiberColor_g], sourcetablealias.[medianFiberColor_r], sourcetablealias.[medianFiberColor_i], sourcetablealias.[medianFiberColor_z], sourcetablealias.[medianPsfColor_u], sourcetablealias.[medianPsfColor_g], sourcetablealias.[medianPsfColor_r], sourcetablealias.[medianPsfColor_i], sourcetablealias.[medianPsfColor_z], sourcetablealias.[q_u], sourcetablealias.[q_g], sourcetablealias.[q_r], sourcetablealias.[q_i], sourcetablealias.[q_z], sourcetablealias.[u_u], sourcetablealias.[u_g], sourcetablealias.[u_r], sourcetablealias.[u_i], sourcetablealias.[u_z], sourcetablealias.[pspStatus], sourcetablealias.[sky_u], sourcetablealias.[sky_g], sourcetablealias.[sky_r], sourcetablealias.[sky_i], sourcetablealias.[sky_z], sourcetablealias.[skySig_u], sourcetablealias.[skySig_g], sourcetablealias.[skySig_r], sourcetablealias.[skySig_i], sourcetablealias.[skySig_z], sourcetablealias.[skyErr_u], sourcetablealias.[skyErr_g], sourcetablealias.[skyErr_r], sourcetablealias.[skyErr_i], sourcetablealias.[skyErr_z], sourcetablealias.[skySlope_u], sourcetablealias.[skySlope_g], sourcetablealias.[skySlope_r], sourcetablealias.[skySlope_i], sourcetablealias.[skySlope_z], sourcetablealias.[lbias_u], sourcetablealias.[lbias_g], sourcetablealias.[lbias_r], sourcetablealias.[lbias_i], sourcetablealias.[lbias_z], sourcetablealias.[rbias_u], sourcetablealias.[rbias_g], sourcetablealias.[rbias_r], sourcetablealias.[rbias_i], sourcetablealias.[rbias_z], sourcetablealias.[nProf_u], sourcetablealias.[nProf_g], sourcetablealias.[nProf_r], sourcetablealias.[nProf_i], sourcetablealias.[nProf_z], sourcetablealias.[psfNStar_u], sourcetablealias.[psfNStar_g], sourcetablealias.[psfNStar_r], sourcetablealias.[psfNStar_i], sourcetablealias.[psfNStar_z], sourcetablealias.[psfApCorrectionErr_u], sourcetablealias.[psfApCorrectionErr_g], sourcetablealias.[psfApCorrectionErr_r], sourcetablealias.[psfApCorrectionErr_i], sourcetablealias.[psfApCorrectionErr_z], sourcetablealias.[psfSigma1_u], sourcetablealias.[psfSigma1_g], sourcetablealias.[psfSigma1_r], sourcetablealias.[psfSigma1_i], sourcetablealias.[psfSigma1_z], sourcetablealias.[psfSigma2_u], sourcetablealias.[psfSigma2_g], sourcetablealias.[psfSigma2_r], sourcetablealias.[psfSigma2_i], sourcetablealias.[psfSigma2_z], sourcetablealias.[psfB_u], sourcetablealias.[psfB_g], sourcetablealias.[psfB_r], sourcetablealias.[psfB_i], sourcetablealias.[psfB_z], sourcetablealias.[psfP0_u], sourcetablealias.[psfP0_g], sourcetablealias.[psfP0_r], sourcetablealias.[psfP0_i], sourcetablealias.[psfP0_z], sourcetablealias.[psfBeta_u], sourcetablealias.[psfBeta_g], sourcetablealias.[psfBeta_r], sourcetablealias.[psfBeta_i], sourcetablealias.[psfBeta_z], sourcetablealias.[psfSigmaP_u], sourcetablealias.[psfSigmaP_g], sourcetablealias.[psfSigmaP_r], sourcetablealias.[psfSigmaP_i], sourcetablealias.[psfSigmaP_z], sourcetablealias.[psfWidth_u], sourcetablealias.[psfWidth_g], sourcetablealias.[psfWidth_r], sourcetablealias.[psfWidth_i], sourcetablealias.[psfWidth_z], sourcetablealias.[psfPsfCounts_u], sourcetablealias.[psfPsfCounts_g], sourcetablealias.[psfPsfCounts_r], sourcetablealias.[psfPsfCounts_i], sourcetablealias.[psfPsfCounts_z], sourcetablealias.[psf2GSigma1_u], sourcetablealias.[psf2GSigma1_g], sourcetablealias.[psf2GSigma1_r], sourcetablealias.[psf2GSigma1_i], sourcetablealias.[psf2GSigma1_z], sourcetablealias.[psf2GSigma2_u], sourcetablealias.[psf2GSigma2_g], sourcetablealias.[psf2GSigma2_r], sourcetablealias.[psf2GSigma2_i], sourcetablealias.[psf2GSigma2_z], sourcetablealias.[psf2GB_u], sourcetablealias.[psf2GB_g], sourcetablealias.[psf2GB_r], sourcetablealias.[psf2GB_i], sourcetablealias.[psf2GB_z], sourcetablealias.[psfCounts_u], sourcetablealias.[psfCounts_g], sourcetablealias.[psfCounts_r], sourcetablealias.[psfCounts_i], sourcetablealias.[psfCounts_z], sourcetablealias.[gain_u], sourcetablealias.[gain_g], sourcetablealias.[gain_r], sourcetablealias.[gain_i], sourcetablealias.[gain_z], sourcetablealias.[darkVariance_u], sourcetablealias.[darkVariance_g], sourcetablealias.[darkVariance_r], sourcetablealias.[darkVariance_i], sourcetablealias.[darkVariance_z], sourcetablealias.[score], sourcetablealias.[aterm_u], sourcetablealias.[aterm_g], sourcetablealias.[aterm_r], sourcetablealias.[aterm_i], sourcetablealias.[aterm_z], sourcetablealias.[kterm_u], sourcetablealias.[kterm_g], sourcetablealias.[kterm_r], sourcetablealias.[kterm_i], sourcetablealias.[kterm_z], sourcetablealias.[kdot_u], sourcetablealias.[kdot_g], sourcetablealias.[kdot_r], sourcetablealias.[kdot_i], sourcetablealias.[kdot_z], sourcetablealias.[reftai_u], sourcetablealias.[reftai_g], sourcetablealias.[reftai_r], sourcetablealias.[reftai_i], sourcetablealias.[reftai_z], sourcetablealias.[tai_u], sourcetablealias.[tai_g], sourcetablealias.[tai_r], sourcetablealias.[tai_i], sourcetablealias.[tai_z], sourcetablealias.[nStarsOffset_u], sourcetablealias.[nStarsOffset_g], sourcetablealias.[nStarsOffset_r], sourcetablealias.[nStarsOffset_i], sourcetablealias.[nStarsOffset_z], sourcetablealias.[fieldOffset_u], sourcetablealias.[fieldOffset_g], sourcetablealias.[fieldOffset_r], sourcetablealias.[fieldOffset_i], sourcetablealias.[fieldOffset_z], sourcetablealias.[calibStatus_u], sourcetablealias.[calibStatus_g], sourcetablealias.[calibStatus_r], sourcetablealias.[calibStatus_i], sourcetablealias.[calibStatus_z], sourcetablealias.[imageStatus_u], sourcetablealias.[imageStatus_g], sourcetablealias.[imageStatus_r], sourcetablealias.[imageStatus_i], sourcetablealias.[imageStatus_z], sourcetablealias.[nMgyPerCount_u], sourcetablealias.[nMgyPerCount_g], sourcetablealias.[nMgyPerCount_r], sourcetablealias.[nMgyPerCount_i], sourcetablealias.[nMgyPerCount_z], sourcetablealias.[nMgyPerCountIvar_u], sourcetablealias.[nMgyPerCountIvar_g], sourcetablealias.[nMgyPerCountIvar_r], sourcetablealias.[nMgyPerCountIvar_i], sourcetablealias.[nMgyPerCountIvar_z], sourcetablealias.[ifield], sourcetablealias.[muStart], sourcetablealias.[muEnd], sourcetablealias.[nuStart], sourcetablealias.[nuEnd], sourcetablealias.[ifindx], sourcetablealias.[nBalkans], sourcetablealias.[loadVersion]
 FROM   [SkyNode_SDSSDR12].[dbo].[Field] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.fieldID = sourcetablealias.fieldID
	;


GO

-- SUBSAMPLING TABLE 'FieldProfile' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[fieldID] bigint, [bin] tinyint, [band] tinyint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[fieldID], sourcetablealias.[bin], sourcetablealias.[band], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[FieldProfile] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [fieldID], [bin], [band]
 FROM temporaryidlistquery
 WHERE randomnumber < 1;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[FieldProfile] WITH (TABLOCKX)
	([bin], [band], [profMean], [profMed], [profSig], [fieldID])
 SELECT sourcetablealias.[bin], sourcetablealias.[band], sourcetablealias.[profMean], sourcetablealias.[profMed], sourcetablealias.[profSig], sourcetablealias.[fieldID]
 FROM   [SkyNode_SDSSDR12].[dbo].[FieldProfile] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.fieldID = sourcetablealias.fieldID AND ##temporaryidlist.bin = sourcetablealias.bin AND ##temporaryidlist.band = sourcetablealias.band
	;


GO

-- SUBSAMPLING TABLE 'Frame' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[fieldID] bigint, [zoom] int
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[fieldID], sourcetablealias.[zoom], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[Frame] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [fieldID], [zoom]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[Frame] WITH (TABLOCKX)
	([fieldID], [zoom], [run], [rerun], [camcol], [field], [stripe], [strip], [a], [b], [c], [d], [e], [f], [node], [incl], [raMin], [raMax], [decMin], [decMax], [mu], [nu], [ra], [dec], [cx], [cy], [cz], [htmID])
 SELECT sourcetablealias.[fieldID], sourcetablealias.[zoom], sourcetablealias.[run], sourcetablealias.[rerun], sourcetablealias.[camcol], sourcetablealias.[field], sourcetablealias.[stripe], sourcetablealias.[strip], sourcetablealias.[a], sourcetablealias.[b], sourcetablealias.[c], sourcetablealias.[d], sourcetablealias.[e], sourcetablealias.[f], sourcetablealias.[node], sourcetablealias.[incl], sourcetablealias.[raMin], sourcetablealias.[raMax], sourcetablealias.[decMin], sourcetablealias.[decMax], sourcetablealias.[mu], sourcetablealias.[nu], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[htmID]
 FROM   [SkyNode_SDSSDR12].[dbo].[Frame] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.fieldID = sourcetablealias.fieldID AND ##temporaryidlist.zoom = sourcetablealias.zoom
	;


GO

-- SUBSAMPLING TABLE 'galSpecExtra' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[galSpecExtra] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[galSpecExtra] WITH (TABLOCKX)
	([specObjID], [bptclass], [oh_p2p5], [oh_p16], [oh_p50], [oh_p84], [oh_p97p5], [oh_entropy], [lgm_tot_p2p5], [lgm_tot_p16], [lgm_tot_p50], [lgm_tot_p84], [lgm_tot_p97p5], [lgm_fib_p2p5], [lgm_fib_p16], [lgm_fib_p50], [lgm_fib_p84], [lgm_fib_p97p5], [sfr_tot_p2p5], [sfr_tot_p16], [sfr_tot_p50], [sfr_tot_p84], [sfr_tot_p97p5], [sfr_tot_entropy], [sfr_fib_p2p5], [sfr_fib_p16], [sfr_fib_p50], [sfr_fib_p84], [sfr_fib_p97p5], [sfr_fib_entropy], [specsfr_tot_p2p5], [specsfr_tot_p16], [specsfr_tot_p50], [specsfr_tot_p84], [specsfr_tot_p97p5], [specsfr_tot_entropy], [specsfr_fib_p2p5], [specsfr_fib_p16], [specsfr_fib_p50], [specsfr_fib_p84], [specsfr_fib_p97p5], [specsfr_fib_entropy])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[bptclass], sourcetablealias.[oh_p2p5], sourcetablealias.[oh_p16], sourcetablealias.[oh_p50], sourcetablealias.[oh_p84], sourcetablealias.[oh_p97p5], sourcetablealias.[oh_entropy], sourcetablealias.[lgm_tot_p2p5], sourcetablealias.[lgm_tot_p16], sourcetablealias.[lgm_tot_p50], sourcetablealias.[lgm_tot_p84], sourcetablealias.[lgm_tot_p97p5], sourcetablealias.[lgm_fib_p2p5], sourcetablealias.[lgm_fib_p16], sourcetablealias.[lgm_fib_p50], sourcetablealias.[lgm_fib_p84], sourcetablealias.[lgm_fib_p97p5], sourcetablealias.[sfr_tot_p2p5], sourcetablealias.[sfr_tot_p16], sourcetablealias.[sfr_tot_p50], sourcetablealias.[sfr_tot_p84], sourcetablealias.[sfr_tot_p97p5], sourcetablealias.[sfr_tot_entropy], sourcetablealias.[sfr_fib_p2p5], sourcetablealias.[sfr_fib_p16], sourcetablealias.[sfr_fib_p50], sourcetablealias.[sfr_fib_p84], sourcetablealias.[sfr_fib_p97p5], sourcetablealias.[sfr_fib_entropy], sourcetablealias.[specsfr_tot_p2p5], sourcetablealias.[specsfr_tot_p16], sourcetablealias.[specsfr_tot_p50], sourcetablealias.[specsfr_tot_p84], sourcetablealias.[specsfr_tot_p97p5], sourcetablealias.[specsfr_tot_entropy], sourcetablealias.[specsfr_fib_p2p5], sourcetablealias.[specsfr_fib_p16], sourcetablealias.[specsfr_fib_p50], sourcetablealias.[specsfr_fib_p84], sourcetablealias.[specsfr_fib_p97p5], sourcetablealias.[specsfr_fib_entropy]
 FROM   [SkyNode_SDSSDR12].[dbo].[galSpecExtra] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'galSpecIndx' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[galSpecIndx] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[galSpecIndx] WITH (TABLOCKX)
	([specObjID], [lick_cn1], [lick_cn1_err], [lick_cn1_model], [lick_cn1_sub], [lick_cn1_sub_err], [lick_cn2], [lick_cn2_err], [lick_cn2_model], [lick_cn2_sub], [lick_cn2_sub_err], [lick_ca4227], [lick_ca4227_err], [lick_ca4227_model], [lick_ca4227_sub], [lick_ca4227_sub_err], [lick_g4300], [lick_g4300_err], [lick_g4300_model], [lick_g4300_sub], [lick_g4300_sub_err], [lick_fe4383], [lick_fe4383_err], [lick_fe4383_model], [lick_fe4383_sub], [lick_fe4383_sub_err], [lick_ca4455], [lick_ca4455_err], [lick_ca4455_model], [lick_ca4455_sub], [lick_ca4455_sub_err], [lick_fe4531], [lick_fe4531_err], [lick_fe4531_model], [lick_fe4531_sub], [lick_fe4531_sub_err], [lick_c4668], [lick_c4668_err], [lick_c4668_model], [lick_c4668_sub], [lick_c4668_sub_err], [lick_hb], [lick_hb_err], [lick_hb_model], [lick_hb_sub], [lick_hb_sub_err], [lick_fe5015], [lick_fe5015_err], [lick_fe5015_model], [lick_fe5015_sub], [lick_fe5015_sub_err], [lick_mg1], [lick_mg1_err], [lick_mg1_model], [lick_mg1_sub], [lick_mg1_sub_err], [lick_mg2], [lick_mg2_err], [lick_mg2_model], [lick_mg2_sub], [lick_mg2_sub_err], [lick_mgb], [lick_mgb_err], [lick_mgb_model], [lick_mgb_sub], [lick_mgb_sub_err], [lick_fe5270], [lick_fe5270_err], [lick_fe5270_model], [lick_fe5270_sub], [lick_fe5270_sub_err], [lick_fe5335], [lick_fe5335_err], [lick_fe5335_model], [lick_fe5335_sub], [lick_fe5335_sub_err], [lick_fe5406], [lick_fe5406_err], [lick_fe5406_model], [lick_fe5406_sub], [lick_fe5406_sub_err], [lick_fe5709], [lick_fe5709_err], [lick_fe5709_model], [lick_fe5709_sub], [lick_fe5709_sub_err], [lick_fe5782], [lick_fe5782_err], [lick_fe5782_model], [lick_fe5782_sub], [lick_fe5782_sub_err], [lick_nad], [lick_nad_err], [lick_nad_model], [lick_nad_sub], [lick_nad_sub_err], [lick_tio1], [lick_tio1_err], [lick_tio1_model], [lick_tio1_sub], [lick_tio1_sub_err], [lick_tio2], [lick_tio2_err], [lick_tio2_model], [lick_tio2_sub], [lick_tio2_sub_err], [lick_hd_a], [lick_hd_a_err], [lick_hd_a_model], [lick_hd_a_sub], [lick_hd_a_sub_err], [lick_hg_a], [lick_hg_a_err], [lick_hg_a_model], [lick_hg_a_sub], [lick_hg_a_sub_err], [lick_hd_f], [lick_hd_f_err], [lick_hd_f_model], [lick_hd_f_sub], [lick_hd_f_sub_err], [lick_hg_f], [lick_hg_f_err], [lick_hg_f_model], [lick_hg_f_sub], [lick_hg_f_sub_err], [dtt_caii8498], [dtt_caii8498_err], [dtt_caii8498_model], [dtt_caii8498_sub], [dtt_caii8498_sub_err], [dtt_caii8542], [dtt_caii8542_err], [dtt_caii8542_model], [dtt_caii8542_sub], [dtt_caii8542_sub_err], [dtt_caii8662], [dtt_caii8662_err], [dtt_caii8662_model], [dtt_caii8662_sub], [dtt_caii8662_sub_err], [dtt_mgi8807], [dtt_mgi8807_err], [dtt_mgi8807_model], [dtt_mgi8807_sub], [dtt_mgi8807_sub_err], [bh_cnb], [bh_cnb_err], [bh_cnb_model], [bh_cnb_sub], [bh_cnb_sub_err], [bh_hk], [bh_hk_err], [bh_hk_model], [bh_hk_sub], [bh_hk_sub_err], [bh_cai], [bh_cai_err], [bh_cai_model], [bh_cai_sub], [bh_cai_sub_err], [bh_g], [bh_g_err], [bh_g_model], [bh_g_sub], [bh_g_sub_err], [bh_hb], [bh_hb_err], [bh_hb_model], [bh_hb_sub], [bh_hb_sub_err], [bh_mgg], [bh_mgg_err], [bh_mgg_model], [bh_mgg_sub], [bh_mgg_sub_err], [bh_mh], [bh_mh_err], [bh_mh_model], [bh_mh_sub], [bh_mh_sub_err], [bh_fc], [bh_fc_err], [bh_fc_model], [bh_fc_sub], [bh_fc_sub_err], [bh_nad], [bh_nad_err], [bh_nad_model], [bh_nad_sub], [bh_nad_sub_err], [d4000], [d4000_err], [d4000_model], [d4000_sub], [d4000_sub_err], [d4000_n], [d4000_n_err], [d4000_n_model], [d4000_n_sub], [d4000_n_sub_err], [d4000_red], [d4000_blue], [d4000_n_red], [d4000_n_blue], [d4000_sub_red], [d4000_sub_blue], [d4000_n_sub_red], [d4000_n_sub_blue], [tauv_model_040], [model_coef_040], [model_chisq_040], [tauv_model_080], [model_coef_080], [model_chisq_080], [tauv_model_170], [model_coef_170], [model_chisq_170], [tauv_model_400], [model_coef_400], [model_chisq_400], [best_model_z], [tauv_cont], [model_coef], [model_chisq])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[lick_cn1], sourcetablealias.[lick_cn1_err], sourcetablealias.[lick_cn1_model], sourcetablealias.[lick_cn1_sub], sourcetablealias.[lick_cn1_sub_err], sourcetablealias.[lick_cn2], sourcetablealias.[lick_cn2_err], sourcetablealias.[lick_cn2_model], sourcetablealias.[lick_cn2_sub], sourcetablealias.[lick_cn2_sub_err], sourcetablealias.[lick_ca4227], sourcetablealias.[lick_ca4227_err], sourcetablealias.[lick_ca4227_model], sourcetablealias.[lick_ca4227_sub], sourcetablealias.[lick_ca4227_sub_err], sourcetablealias.[lick_g4300], sourcetablealias.[lick_g4300_err], sourcetablealias.[lick_g4300_model], sourcetablealias.[lick_g4300_sub], sourcetablealias.[lick_g4300_sub_err], sourcetablealias.[lick_fe4383], sourcetablealias.[lick_fe4383_err], sourcetablealias.[lick_fe4383_model], sourcetablealias.[lick_fe4383_sub], sourcetablealias.[lick_fe4383_sub_err], sourcetablealias.[lick_ca4455], sourcetablealias.[lick_ca4455_err], sourcetablealias.[lick_ca4455_model], sourcetablealias.[lick_ca4455_sub], sourcetablealias.[lick_ca4455_sub_err], sourcetablealias.[lick_fe4531], sourcetablealias.[lick_fe4531_err], sourcetablealias.[lick_fe4531_model], sourcetablealias.[lick_fe4531_sub], sourcetablealias.[lick_fe4531_sub_err], sourcetablealias.[lick_c4668], sourcetablealias.[lick_c4668_err], sourcetablealias.[lick_c4668_model], sourcetablealias.[lick_c4668_sub], sourcetablealias.[lick_c4668_sub_err], sourcetablealias.[lick_hb], sourcetablealias.[lick_hb_err], sourcetablealias.[lick_hb_model], sourcetablealias.[lick_hb_sub], sourcetablealias.[lick_hb_sub_err], sourcetablealias.[lick_fe5015], sourcetablealias.[lick_fe5015_err], sourcetablealias.[lick_fe5015_model], sourcetablealias.[lick_fe5015_sub], sourcetablealias.[lick_fe5015_sub_err], sourcetablealias.[lick_mg1], sourcetablealias.[lick_mg1_err], sourcetablealias.[lick_mg1_model], sourcetablealias.[lick_mg1_sub], sourcetablealias.[lick_mg1_sub_err], sourcetablealias.[lick_mg2], sourcetablealias.[lick_mg2_err], sourcetablealias.[lick_mg2_model], sourcetablealias.[lick_mg2_sub], sourcetablealias.[lick_mg2_sub_err], sourcetablealias.[lick_mgb], sourcetablealias.[lick_mgb_err], sourcetablealias.[lick_mgb_model], sourcetablealias.[lick_mgb_sub], sourcetablealias.[lick_mgb_sub_err], sourcetablealias.[lick_fe5270], sourcetablealias.[lick_fe5270_err], sourcetablealias.[lick_fe5270_model], sourcetablealias.[lick_fe5270_sub], sourcetablealias.[lick_fe5270_sub_err], sourcetablealias.[lick_fe5335], sourcetablealias.[lick_fe5335_err], sourcetablealias.[lick_fe5335_model], sourcetablealias.[lick_fe5335_sub], sourcetablealias.[lick_fe5335_sub_err], sourcetablealias.[lick_fe5406], sourcetablealias.[lick_fe5406_err], sourcetablealias.[lick_fe5406_model], sourcetablealias.[lick_fe5406_sub], sourcetablealias.[lick_fe5406_sub_err], sourcetablealias.[lick_fe5709], sourcetablealias.[lick_fe5709_err], sourcetablealias.[lick_fe5709_model], sourcetablealias.[lick_fe5709_sub], sourcetablealias.[lick_fe5709_sub_err], sourcetablealias.[lick_fe5782], sourcetablealias.[lick_fe5782_err], sourcetablealias.[lick_fe5782_model], sourcetablealias.[lick_fe5782_sub], sourcetablealias.[lick_fe5782_sub_err], sourcetablealias.[lick_nad], sourcetablealias.[lick_nad_err], sourcetablealias.[lick_nad_model], sourcetablealias.[lick_nad_sub], sourcetablealias.[lick_nad_sub_err], sourcetablealias.[lick_tio1], sourcetablealias.[lick_tio1_err], sourcetablealias.[lick_tio1_model], sourcetablealias.[lick_tio1_sub], sourcetablealias.[lick_tio1_sub_err], sourcetablealias.[lick_tio2], sourcetablealias.[lick_tio2_err], sourcetablealias.[lick_tio2_model], sourcetablealias.[lick_tio2_sub], sourcetablealias.[lick_tio2_sub_err], sourcetablealias.[lick_hd_a], sourcetablealias.[lick_hd_a_err], sourcetablealias.[lick_hd_a_model], sourcetablealias.[lick_hd_a_sub], sourcetablealias.[lick_hd_a_sub_err], sourcetablealias.[lick_hg_a], sourcetablealias.[lick_hg_a_err], sourcetablealias.[lick_hg_a_model], sourcetablealias.[lick_hg_a_sub], sourcetablealias.[lick_hg_a_sub_err], sourcetablealias.[lick_hd_f], sourcetablealias.[lick_hd_f_err], sourcetablealias.[lick_hd_f_model], sourcetablealias.[lick_hd_f_sub], sourcetablealias.[lick_hd_f_sub_err], sourcetablealias.[lick_hg_f], sourcetablealias.[lick_hg_f_err], sourcetablealias.[lick_hg_f_model], sourcetablealias.[lick_hg_f_sub], sourcetablealias.[lick_hg_f_sub_err], sourcetablealias.[dtt_caii8498], sourcetablealias.[dtt_caii8498_err], sourcetablealias.[dtt_caii8498_model], sourcetablealias.[dtt_caii8498_sub], sourcetablealias.[dtt_caii8498_sub_err], sourcetablealias.[dtt_caii8542], sourcetablealias.[dtt_caii8542_err], sourcetablealias.[dtt_caii8542_model], sourcetablealias.[dtt_caii8542_sub], sourcetablealias.[dtt_caii8542_sub_err], sourcetablealias.[dtt_caii8662], sourcetablealias.[dtt_caii8662_err], sourcetablealias.[dtt_caii8662_model], sourcetablealias.[dtt_caii8662_sub], sourcetablealias.[dtt_caii8662_sub_err], sourcetablealias.[dtt_mgi8807], sourcetablealias.[dtt_mgi8807_err], sourcetablealias.[dtt_mgi8807_model], sourcetablealias.[dtt_mgi8807_sub], sourcetablealias.[dtt_mgi8807_sub_err], sourcetablealias.[bh_cnb], sourcetablealias.[bh_cnb_err], sourcetablealias.[bh_cnb_model], sourcetablealias.[bh_cnb_sub], sourcetablealias.[bh_cnb_sub_err], sourcetablealias.[bh_hk], sourcetablealias.[bh_hk_err], sourcetablealias.[bh_hk_model], sourcetablealias.[bh_hk_sub], sourcetablealias.[bh_hk_sub_err], sourcetablealias.[bh_cai], sourcetablealias.[bh_cai_err], sourcetablealias.[bh_cai_model], sourcetablealias.[bh_cai_sub], sourcetablealias.[bh_cai_sub_err], sourcetablealias.[bh_g], sourcetablealias.[bh_g_err], sourcetablealias.[bh_g_model], sourcetablealias.[bh_g_sub], sourcetablealias.[bh_g_sub_err], sourcetablealias.[bh_hb], sourcetablealias.[bh_hb_err], sourcetablealias.[bh_hb_model], sourcetablealias.[bh_hb_sub], sourcetablealias.[bh_hb_sub_err], sourcetablealias.[bh_mgg], sourcetablealias.[bh_mgg_err], sourcetablealias.[bh_mgg_model], sourcetablealias.[bh_mgg_sub], sourcetablealias.[bh_mgg_sub_err], sourcetablealias.[bh_mh], sourcetablealias.[bh_mh_err], sourcetablealias.[bh_mh_model], sourcetablealias.[bh_mh_sub], sourcetablealias.[bh_mh_sub_err], sourcetablealias.[bh_fc], sourcetablealias.[bh_fc_err], sourcetablealias.[bh_fc_model], sourcetablealias.[bh_fc_sub], sourcetablealias.[bh_fc_sub_err], sourcetablealias.[bh_nad], sourcetablealias.[bh_nad_err], sourcetablealias.[bh_nad_model], sourcetablealias.[bh_nad_sub], sourcetablealias.[bh_nad_sub_err], sourcetablealias.[d4000], sourcetablealias.[d4000_err], sourcetablealias.[d4000_model], sourcetablealias.[d4000_sub], sourcetablealias.[d4000_sub_err], sourcetablealias.[d4000_n], sourcetablealias.[d4000_n_err], sourcetablealias.[d4000_n_model], sourcetablealias.[d4000_n_sub], sourcetablealias.[d4000_n_sub_err], sourcetablealias.[d4000_red], sourcetablealias.[d4000_blue], sourcetablealias.[d4000_n_red], sourcetablealias.[d4000_n_blue], sourcetablealias.[d4000_sub_red], sourcetablealias.[d4000_sub_blue], sourcetablealias.[d4000_n_sub_red], sourcetablealias.[d4000_n_sub_blue], sourcetablealias.[tauv_model_040], sourcetablealias.[model_coef_040], sourcetablealias.[model_chisq_040], sourcetablealias.[tauv_model_080], sourcetablealias.[model_coef_080], sourcetablealias.[model_chisq_080], sourcetablealias.[tauv_model_170], sourcetablealias.[model_coef_170], sourcetablealias.[model_chisq_170], sourcetablealias.[tauv_model_400], sourcetablealias.[model_coef_400], sourcetablealias.[model_chisq_400], sourcetablealias.[best_model_z], sourcetablealias.[tauv_cont], sourcetablealias.[model_coef], sourcetablealias.[model_chisq]
 FROM   [SkyNode_SDSSDR12].[dbo].[galSpecIndx] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'galSpecInfo' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[galSpecInfo] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[galSpecInfo] WITH (TABLOCKX)
	([specObjID], [plateid], [mjd], [fiberid], [ra], [dec], [primtarget], [sectarget], [targettype], [spectrotype], [subclass], [z], [z_err], [z_warning], [v_disp], [v_disp_err], [sn_median], [e_bv_sfd], [release], [reliable])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[plateid], sourcetablealias.[mjd], sourcetablealias.[fiberid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[primtarget], sourcetablealias.[sectarget], sourcetablealias.[targettype], sourcetablealias.[spectrotype], sourcetablealias.[subclass], sourcetablealias.[z], sourcetablealias.[z_err], sourcetablealias.[z_warning], sourcetablealias.[v_disp], sourcetablealias.[v_disp_err], sourcetablealias.[sn_median], sourcetablealias.[e_bv_sfd], sourcetablealias.[release], sourcetablealias.[reliable]
 FROM   [SkyNode_SDSSDR12].[dbo].[galSpecInfo] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'galSpecLine' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[galSpecLine] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[galSpecLine] WITH (TABLOCKX)
	([specObjID], [sigma_balmer], [sigma_balmer_err], [sigma_forbidden], [sigma_forbidden_err], [v_off_balmer], [v_off_balmer_err], [v_off_forbidden], [v_off_forbidden_err], [oii_3726_cont], [oii_3726_cont_err], [oii_3726_reqw], [oii_3726_reqw_err], [oii_3726_eqw], [oii_3726_eqw_err], [oii_3726_flux], [oii_3726_flux_err], [oii_3726_inst_res], [oii_3726_chisq], [oii_3729_cont], [oii_3729_cont_err], [oii_3729_reqw], [oii_3729_reqw_err], [oii_3729_eqw], [oii_3729_eqw_err], [oii_3729_flux], [oii_3729_flux_err], [oii_3729_inst_res], [oii_3729_chisq], [neiii_3869_cont], [neiii_3869_cont_err], [neiii_3869_reqw], [neiii_3869_reqw_err], [neiii_3869_eqw], [neiii_3869_eqw_err], [neiii_3869_flux], [neiii_3869_flux_err], [neiii_3869_inst_res], [neiii_3869_chisq], [h_delta_cont], [h_delta_cont_err], [h_delta_reqw], [h_delta_reqw_err], [h_delta_eqw], [h_delta_eqw_err], [h_delta_flux], [h_delta_flux_err], [h_delta_inst_res], [h_delta_chisq], [h_gamma_cont], [h_gamma_cont_err], [h_gamma_reqw], [h_gamma_reqw_err], [h_gamma_eqw], [h_gamma_eqw_err], [h_gamma_flux], [h_gamma_flux_err], [h_gamma_inst_res], [h_gamma_chisq], [oiii_4363_cont], [oiii_4363_cont_err], [oiii_4363_reqw], [oiii_4363_reqw_err], [oiii_4363_eqw], [oiii_4363_eqw_err], [oiii_4363_flux], [oiii_4363_flux_err], [oiii_4363_inst_res], [oiii_4363_chisq], [h_beta_cont], [h_beta_cont_err], [h_beta_reqw], [h_beta_reqw_err], [h_beta_eqw], [h_beta_eqw_err], [h_beta_flux], [h_beta_flux_err], [h_beta_inst_res], [h_beta_chisq], [oiii_4959_cont], [oiii_4959_cont_err], [oiii_4959_reqw], [oiii_4959_reqw_err], [oiii_4959_eqw], [oiii_4959_eqw_err], [oiii_4959_flux], [oiii_4959_flux_err], [oiii_4959_inst_res], [oiii_4959_chisq], [oiii_5007_cont], [oiii_5007_cont_err], [oiii_5007_reqw], [oiii_5007_reqw_err], [oiii_5007_eqw], [oiii_5007_eqw_err], [oiii_5007_flux], [oiii_5007_flux_err], [oiii_5007_inst_res], [oiii_5007_chisq], [hei_5876_cont], [hei_5876_cont_err], [hei_5876_reqw], [hei_5876_reqw_err], [hei_5876_eqw], [hei_5876_eqw_err], [hei_5876_flux], [hei_5876_flux_err], [hei_5876_inst_res], [hei_5876_chisq], [oi_6300_cont], [oi_6300_cont_err], [oi_6300_reqw], [oi_6300_reqw_err], [oi_6300_eqw], [oi_6300_eqw_err], [oi_6300_flux], [oi_6300_flux_err], [oi_6300_inst_res], [oi_6300_chisq], [nii_6548_cont], [nii_6548_cont_err], [nii_6548_reqw], [nii_6548_reqw_err], [nii_6548_eqw], [nii_6548_eqw_err], [nii_6548_flux], [nii_6548_flux_err], [nii_6548_inst_res], [nii_6548_chisq], [h_alpha_cont], [h_alpha_cont_err], [h_alpha_reqw], [h_alpha_reqw_err], [h_alpha_eqw], [h_alpha_eqw_err], [h_alpha_flux], [h_alpha_flux_err], [h_alpha_inst_res], [h_alpha_chisq], [nii_6584_cont], [nii_6584_cont_err], [nii_6584_reqw], [nii_6584_reqw_err], [nii_6584_eqw], [nii_6584_eqw_err], [nii_6584_flux], [nii_6584_flux_err], [nii_6584_inst_res], [nii_6584_chisq], [sii_6717_cont], [sii_6717_cont_err], [sii_6717_reqw], [sii_6717_reqw_err], [sii_6717_eqw], [sii_6717_eqw_err], [sii_6717_flux], [sii_6717_flux_err], [sii_6717_inst_res], [sii_6717_chisq], [sii_6731_cont], [sii_6731_cont_err], [sii_6731_reqw], [sii_6731_reqw_err], [sii_6731_eqw], [sii_6731_eqw_err], [sii_6731_flux], [sii_6731_flux_err], [sii_6731_inst_res], [sii_6731_chisq], [ariii7135_cont], [ariii7135_cont_err], [ariii7135_reqw], [ariii7135_reqw_err], [ariii7135_eqw], [ariii7135_eqw_err], [ariii7135_flux], [ariii7135_flux_err], [ariii7135_inst_res], [ariii7135_chisq], [oii_sigma], [oii_flux], [oii_flux_err], [oii_voff], [oii_chi2], [oiii_sigma], [oiii_flux], [oiii_flux_err], [oiii_voff], [oiii_chi2], [spectofiber])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[sigma_balmer], sourcetablealias.[sigma_balmer_err], sourcetablealias.[sigma_forbidden], sourcetablealias.[sigma_forbidden_err], sourcetablealias.[v_off_balmer], sourcetablealias.[v_off_balmer_err], sourcetablealias.[v_off_forbidden], sourcetablealias.[v_off_forbidden_err], sourcetablealias.[oii_3726_cont], sourcetablealias.[oii_3726_cont_err], sourcetablealias.[oii_3726_reqw], sourcetablealias.[oii_3726_reqw_err], sourcetablealias.[oii_3726_eqw], sourcetablealias.[oii_3726_eqw_err], sourcetablealias.[oii_3726_flux], sourcetablealias.[oii_3726_flux_err], sourcetablealias.[oii_3726_inst_res], sourcetablealias.[oii_3726_chisq], sourcetablealias.[oii_3729_cont], sourcetablealias.[oii_3729_cont_err], sourcetablealias.[oii_3729_reqw], sourcetablealias.[oii_3729_reqw_err], sourcetablealias.[oii_3729_eqw], sourcetablealias.[oii_3729_eqw_err], sourcetablealias.[oii_3729_flux], sourcetablealias.[oii_3729_flux_err], sourcetablealias.[oii_3729_inst_res], sourcetablealias.[oii_3729_chisq], sourcetablealias.[neiii_3869_cont], sourcetablealias.[neiii_3869_cont_err], sourcetablealias.[neiii_3869_reqw], sourcetablealias.[neiii_3869_reqw_err], sourcetablealias.[neiii_3869_eqw], sourcetablealias.[neiii_3869_eqw_err], sourcetablealias.[neiii_3869_flux], sourcetablealias.[neiii_3869_flux_err], sourcetablealias.[neiii_3869_inst_res], sourcetablealias.[neiii_3869_chisq], sourcetablealias.[h_delta_cont], sourcetablealias.[h_delta_cont_err], sourcetablealias.[h_delta_reqw], sourcetablealias.[h_delta_reqw_err], sourcetablealias.[h_delta_eqw], sourcetablealias.[h_delta_eqw_err], sourcetablealias.[h_delta_flux], sourcetablealias.[h_delta_flux_err], sourcetablealias.[h_delta_inst_res], sourcetablealias.[h_delta_chisq], sourcetablealias.[h_gamma_cont], sourcetablealias.[h_gamma_cont_err], sourcetablealias.[h_gamma_reqw], sourcetablealias.[h_gamma_reqw_err], sourcetablealias.[h_gamma_eqw], sourcetablealias.[h_gamma_eqw_err], sourcetablealias.[h_gamma_flux], sourcetablealias.[h_gamma_flux_err], sourcetablealias.[h_gamma_inst_res], sourcetablealias.[h_gamma_chisq], sourcetablealias.[oiii_4363_cont], sourcetablealias.[oiii_4363_cont_err], sourcetablealias.[oiii_4363_reqw], sourcetablealias.[oiii_4363_reqw_err], sourcetablealias.[oiii_4363_eqw], sourcetablealias.[oiii_4363_eqw_err], sourcetablealias.[oiii_4363_flux], sourcetablealias.[oiii_4363_flux_err], sourcetablealias.[oiii_4363_inst_res], sourcetablealias.[oiii_4363_chisq], sourcetablealias.[h_beta_cont], sourcetablealias.[h_beta_cont_err], sourcetablealias.[h_beta_reqw], sourcetablealias.[h_beta_reqw_err], sourcetablealias.[h_beta_eqw], sourcetablealias.[h_beta_eqw_err], sourcetablealias.[h_beta_flux], sourcetablealias.[h_beta_flux_err], sourcetablealias.[h_beta_inst_res], sourcetablealias.[h_beta_chisq], sourcetablealias.[oiii_4959_cont], sourcetablealias.[oiii_4959_cont_err], sourcetablealias.[oiii_4959_reqw], sourcetablealias.[oiii_4959_reqw_err], sourcetablealias.[oiii_4959_eqw], sourcetablealias.[oiii_4959_eqw_err], sourcetablealias.[oiii_4959_flux], sourcetablealias.[oiii_4959_flux_err], sourcetablealias.[oiii_4959_inst_res], sourcetablealias.[oiii_4959_chisq], sourcetablealias.[oiii_5007_cont], sourcetablealias.[oiii_5007_cont_err], sourcetablealias.[oiii_5007_reqw], sourcetablealias.[oiii_5007_reqw_err], sourcetablealias.[oiii_5007_eqw], sourcetablealias.[oiii_5007_eqw_err], sourcetablealias.[oiii_5007_flux], sourcetablealias.[oiii_5007_flux_err], sourcetablealias.[oiii_5007_inst_res], sourcetablealias.[oiii_5007_chisq], sourcetablealias.[hei_5876_cont], sourcetablealias.[hei_5876_cont_err], sourcetablealias.[hei_5876_reqw], sourcetablealias.[hei_5876_reqw_err], sourcetablealias.[hei_5876_eqw], sourcetablealias.[hei_5876_eqw_err], sourcetablealias.[hei_5876_flux], sourcetablealias.[hei_5876_flux_err], sourcetablealias.[hei_5876_inst_res], sourcetablealias.[hei_5876_chisq], sourcetablealias.[oi_6300_cont], sourcetablealias.[oi_6300_cont_err], sourcetablealias.[oi_6300_reqw], sourcetablealias.[oi_6300_reqw_err], sourcetablealias.[oi_6300_eqw], sourcetablealias.[oi_6300_eqw_err], sourcetablealias.[oi_6300_flux], sourcetablealias.[oi_6300_flux_err], sourcetablealias.[oi_6300_inst_res], sourcetablealias.[oi_6300_chisq], sourcetablealias.[nii_6548_cont], sourcetablealias.[nii_6548_cont_err], sourcetablealias.[nii_6548_reqw], sourcetablealias.[nii_6548_reqw_err], sourcetablealias.[nii_6548_eqw], sourcetablealias.[nii_6548_eqw_err], sourcetablealias.[nii_6548_flux], sourcetablealias.[nii_6548_flux_err], sourcetablealias.[nii_6548_inst_res], sourcetablealias.[nii_6548_chisq], sourcetablealias.[h_alpha_cont], sourcetablealias.[h_alpha_cont_err], sourcetablealias.[h_alpha_reqw], sourcetablealias.[h_alpha_reqw_err], sourcetablealias.[h_alpha_eqw], sourcetablealias.[h_alpha_eqw_err], sourcetablealias.[h_alpha_flux], sourcetablealias.[h_alpha_flux_err], sourcetablealias.[h_alpha_inst_res], sourcetablealias.[h_alpha_chisq], sourcetablealias.[nii_6584_cont], sourcetablealias.[nii_6584_cont_err], sourcetablealias.[nii_6584_reqw], sourcetablealias.[nii_6584_reqw_err], sourcetablealias.[nii_6584_eqw], sourcetablealias.[nii_6584_eqw_err], sourcetablealias.[nii_6584_flux], sourcetablealias.[nii_6584_flux_err], sourcetablealias.[nii_6584_inst_res], sourcetablealias.[nii_6584_chisq], sourcetablealias.[sii_6717_cont], sourcetablealias.[sii_6717_cont_err], sourcetablealias.[sii_6717_reqw], sourcetablealias.[sii_6717_reqw_err], sourcetablealias.[sii_6717_eqw], sourcetablealias.[sii_6717_eqw_err], sourcetablealias.[sii_6717_flux], sourcetablealias.[sii_6717_flux_err], sourcetablealias.[sii_6717_inst_res], sourcetablealias.[sii_6717_chisq], sourcetablealias.[sii_6731_cont], sourcetablealias.[sii_6731_cont_err], sourcetablealias.[sii_6731_reqw], sourcetablealias.[sii_6731_reqw_err], sourcetablealias.[sii_6731_eqw], sourcetablealias.[sii_6731_eqw_err], sourcetablealias.[sii_6731_flux], sourcetablealias.[sii_6731_flux_err], sourcetablealias.[sii_6731_inst_res], sourcetablealias.[sii_6731_chisq], sourcetablealias.[ariii7135_cont], sourcetablealias.[ariii7135_cont_err], sourcetablealias.[ariii7135_reqw], sourcetablealias.[ariii7135_reqw_err], sourcetablealias.[ariii7135_eqw], sourcetablealias.[ariii7135_eqw_err], sourcetablealias.[ariii7135_flux], sourcetablealias.[ariii7135_flux_err], sourcetablealias.[ariii7135_inst_res], sourcetablealias.[ariii7135_chisq], sourcetablealias.[oii_sigma], sourcetablealias.[oii_flux], sourcetablealias.[oii_flux_err], sourcetablealias.[oii_voff], sourcetablealias.[oii_chi2], sourcetablealias.[oiii_sigma], sourcetablealias.[oiii_flux], sourcetablealias.[oiii_flux_err], sourcetablealias.[oiii_voff], sourcetablealias.[oiii_chi2], sourcetablealias.[spectofiber]
 FROM   [SkyNode_SDSSDR12].[dbo].[galSpecLine] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'HalfSpace' ---

 --Setting Identity Column
 SET IDENTITY_INSERT [SkyNode_SDSSDR12_Mini].[dbo].[HalfSpace] ON;
  -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[constraintid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[constraintid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[HalfSpace] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [constraintid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[HalfSpace] WITH (TABLOCKX)
	([constraintid], [regionid], [convexid], [x], [y], [z], [c])
 SELECT sourcetablealias.[constraintid], sourcetablealias.[regionid], sourcetablealias.[convexid], sourcetablealias.[x], sourcetablealias.[y], sourcetablealias.[z], sourcetablealias.[c]
 FROM   [SkyNode_SDSSDR12].[dbo].[HalfSpace] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.constraintid = sourcetablealias.constraintid
	;
 --Setting Identity Column
 SET IDENTITY_INSERT [SkyNode_SDSSDR12_Mini].[dbo].[HalfSpace] OFF;
 

GO

-- SUBSAMPLING TABLE 'marvelsStar' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[marvelsStar] WITH (TABLOCKX)
    ([STARNAME], [TWOMASS_NAME], [Plate], [GSC_Name], [TYC_Name], [HIP_Name], [RA_Final], [DEC_Final], [GSC_B], [GSC_V], [TWOMASS_J], [TWOMASS_H], [TWOMASS_K], [SP1], [SP2], [RPM_LOG_g], [Teff], [log_g], [FeH], [GSC_B_E], [GSC_V_E], [TWOMASS_J_E], [TWOMASS_H_E], [TWOMASS_K_E], [Teff_E], [log_g_E], [FeH_E], [Epoch_0], [RA_0], [DEC_0], [RA_TWOMASS], [DEC_TWOMASS], [GSC_PM_RA], [GSC_PM_DEC], [GSC_PM_RA_E], [GSC_PM_DEC_E], [TYC_B], [TYC_B_E], [TYC_V], [TYC_V_E], [HIP_PLX], [HIP_PLX_E], [HIP_SPTYPE])
 SELECT sourcetablealias.[STARNAME], sourcetablealias.[TWOMASS_NAME], sourcetablealias.[Plate], sourcetablealias.[GSC_Name], sourcetablealias.[TYC_Name], sourcetablealias.[HIP_Name], sourcetablealias.[RA_Final], sourcetablealias.[DEC_Final], sourcetablealias.[GSC_B], sourcetablealias.[GSC_V], sourcetablealias.[TWOMASS_J], sourcetablealias.[TWOMASS_H], sourcetablealias.[TWOMASS_K], sourcetablealias.[SP1], sourcetablealias.[SP2], sourcetablealias.[RPM_LOG_g], sourcetablealias.[Teff], sourcetablealias.[log_g], sourcetablealias.[FeH], sourcetablealias.[GSC_B_E], sourcetablealias.[GSC_V_E], sourcetablealias.[TWOMASS_J_E], sourcetablealias.[TWOMASS_H_E], sourcetablealias.[TWOMASS_K_E], sourcetablealias.[Teff_E], sourcetablealias.[log_g_E], sourcetablealias.[FeH_E], sourcetablealias.[Epoch_0], sourcetablealias.[RA_0], sourcetablealias.[DEC_0], sourcetablealias.[RA_TWOMASS], sourcetablealias.[DEC_TWOMASS], sourcetablealias.[GSC_PM_RA], sourcetablealias.[GSC_PM_DEC], sourcetablealias.[GSC_PM_RA_E], sourcetablealias.[GSC_PM_DEC_E], sourcetablealias.[TYC_B], sourcetablealias.[TYC_B_E], sourcetablealias.[TYC_V], sourcetablealias.[TYC_V_E], sourcetablealias.[HIP_PLX], sourcetablealias.[HIP_PLX_E], sourcetablealias.[HIP_SPTYPE]
 FROM   [SkyNode_SDSSDR12].[dbo].[marvelsStar] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'marvelsVelocityCurveUF1D' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[STARNAME] varchar, [BEAM] tinyint, [RADECID] varchar, [FCJD] float, [LST-OBS] varchar
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[STARNAME], sourcetablealias.[BEAM], sourcetablealias.[RADECID], sourcetablealias.[FCJD], sourcetablealias.[LST-OBS], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[marvelsVelocityCurveUF1D] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [STARNAME], [BEAM], [RADECID], [FCJD], [LST-OBS]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[marvelsVelocityCurveUF1D] WITH (TABLOCKX)
	([STARNAME], [FCJD], [RV], [PHOTONERR], [STATERROR], [OFFSETERROR], [Keep], [TOTALPHOTONS], [BARYCENTRICVEL], [SPECNO], [EPOCHFILE], [TEMPLATEFILE], [RADECID], [OBJECT], [EXPTYPE], [PLATEID], [CARTID], [EXPTIME], [DATE-OBS], [TIME], [UTC-OBS], [LST-OBS], [JD], [FCJD_IMG], [MJD], [RA], [DEC], [EPOCH], [ALT], [AZ], [PMTAVG], [PMTRMS], [PMTMIN], [PMTMAX], [OBSFLAG], [IMGAVG], [IMGMAX], [IMGMIN], [SNRMAX], [SNRMEDN], [SNRMIN], [SNRAVG], [SNRSTDEV], [SEEING], [CCDTEMP], [CCDPRES], [P1], [P1RMS], [P2], [P2RMS], [P3], [P3RMS], [T1], [T1RMS], [T2], [T2RMS], [T3], [T3RMS], [T4], [T4RMS], [T5], [T5RMS], [T6], [T6RMS], [T7], [T7RMS], [T8], [T8RMS], [T9], [T9RMS], [T10], [T10RMS], [T11], [T11RMS], [T12], [T12RMS], [T13], [T13RMS], [T14], [T14RMS], [T15], [T15RMS], [T16], [T16RMS], [BEAM], [SURVEY])
 SELECT sourcetablealias.[STARNAME], sourcetablealias.[FCJD], sourcetablealias.[RV], sourcetablealias.[PHOTONERR], sourcetablealias.[STATERROR], sourcetablealias.[OFFSETERROR], sourcetablealias.[Keep], sourcetablealias.[TOTALPHOTONS], sourcetablealias.[BARYCENTRICVEL], sourcetablealias.[SPECNO], sourcetablealias.[EPOCHFILE], sourcetablealias.[TEMPLATEFILE], sourcetablealias.[RADECID], sourcetablealias.[OBJECT], sourcetablealias.[EXPTYPE], sourcetablealias.[PLATEID], sourcetablealias.[CARTID], sourcetablealias.[EXPTIME], sourcetablealias.[DATE-OBS], sourcetablealias.[TIME], sourcetablealias.[UTC-OBS], sourcetablealias.[LST-OBS], sourcetablealias.[JD], sourcetablealias.[FCJD_IMG], sourcetablealias.[MJD], sourcetablealias.[RA], sourcetablealias.[DEC], sourcetablealias.[EPOCH], sourcetablealias.[ALT], sourcetablealias.[AZ], sourcetablealias.[PMTAVG], sourcetablealias.[PMTRMS], sourcetablealias.[PMTMIN], sourcetablealias.[PMTMAX], sourcetablealias.[OBSFLAG], sourcetablealias.[IMGAVG], sourcetablealias.[IMGMAX], sourcetablealias.[IMGMIN], sourcetablealias.[SNRMAX], sourcetablealias.[SNRMEDN], sourcetablealias.[SNRMIN], sourcetablealias.[SNRAVG], sourcetablealias.[SNRSTDEV], sourcetablealias.[SEEING], sourcetablealias.[CCDTEMP], sourcetablealias.[CCDPRES], sourcetablealias.[P1], sourcetablealias.[P1RMS], sourcetablealias.[P2], sourcetablealias.[P2RMS], sourcetablealias.[P3], sourcetablealias.[P3RMS], sourcetablealias.[T1], sourcetablealias.[T1RMS], sourcetablealias.[T2], sourcetablealias.[T2RMS], sourcetablealias.[T3], sourcetablealias.[T3RMS], sourcetablealias.[T4], sourcetablealias.[T4RMS], sourcetablealias.[T5], sourcetablealias.[T5RMS], sourcetablealias.[T6], sourcetablealias.[T6RMS], sourcetablealias.[T7], sourcetablealias.[T7RMS], sourcetablealias.[T8], sourcetablealias.[T8RMS], sourcetablealias.[T9], sourcetablealias.[T9RMS], sourcetablealias.[T10], sourcetablealias.[T10RMS], sourcetablealias.[T11], sourcetablealias.[T11RMS], sourcetablealias.[T12], sourcetablealias.[T12RMS], sourcetablealias.[T13], sourcetablealias.[T13RMS], sourcetablealias.[T14], sourcetablealias.[T14RMS], sourcetablealias.[T15], sourcetablealias.[T15RMS], sourcetablealias.[T16], sourcetablealias.[T16RMS], sourcetablealias.[BEAM], sourcetablealias.[SURVEY]
 FROM   [SkyNode_SDSSDR12].[dbo].[marvelsVelocityCurveUF1D] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.STARNAME = sourcetablealias.STARNAME AND ##temporaryidlist.BEAM = sourcetablealias.BEAM AND ##temporaryidlist.RADECID = sourcetablealias.RADECID AND ##temporaryidlist.FCJD = sourcetablealias.FCJD AND ##temporaryidlist.LST-OBS = sourcetablealias.LST-OBS
	;


GO

-- SUBSAMPLING TABLE 'Mask' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[maskID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[maskID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[Mask] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [maskID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[Mask] WITH (TABLOCKX)
	([maskID], [run], [rerun], [camcol], [field], [mask], [filter], [ra], [dec], [radius], [area], [type], [seeing], [cx], [cy], [cz], [htmID])
 SELECT sourcetablealias.[maskID], sourcetablealias.[run], sourcetablealias.[rerun], sourcetablealias.[camcol], sourcetablealias.[field], sourcetablealias.[mask], sourcetablealias.[filter], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[radius], sourcetablealias.[area], sourcetablealias.[type], sourcetablealias.[seeing], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[htmID]
 FROM   [SkyNode_SDSSDR12].[dbo].[Mask] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.maskID = sourcetablealias.maskID
	;


GO

-- SUBSAMPLING TABLE 'MaskedObject' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[objid] bigint, [maskID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[objid], sourcetablealias.[maskID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[MaskedObject] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objid], [maskID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[MaskedObject] WITH (TABLOCKX)
	([objid], [maskID], [maskType])
 SELECT sourcetablealias.[objid], sourcetablealias.[maskID], sourcetablealias.[maskType]
 FROM   [SkyNode_SDSSDR12].[dbo].[MaskedObject] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objid = sourcetablealias.objid AND ##temporaryidlist.maskID = sourcetablealias.maskID
	;


GO

-- SUBSAMPLING TABLE 'PhotoObjAll' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[objID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[objID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[PhotoObjAll] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[PhotoObjAll] WITH (TABLOCKX)
	([objID], [skyVersion], [run], [rerun], [camcol], [field], [obj], [mode], [nChild], [type], [clean], [probPSF], [insideMask], [flags], [sky_u], [sky_g], [sky_r], [sky_i], [sky_z], [skyIvar_u], [skyIvar_g], [skyIvar_r], [skyIvar_i], [skyIvar_z], [psfMag_u], [psfMag_g], [psfMag_r], [psfMag_i], [psfMag_z], [psfMagErr_u], [psfMagErr_g], [psfMagErr_r], [psfMagErr_i], [psfMagErr_z], [fiberMag_u], [fiberMag_g], [fiberMag_r], [fiberMag_i], [fiberMag_z], [fiberMagErr_u], [fiberMagErr_g], [fiberMagErr_r], [fiberMagErr_i], [fiberMagErr_z], [fiber2Mag_u], [fiber2Mag_g], [fiber2Mag_r], [fiber2Mag_i], [fiber2Mag_z], [fiber2MagErr_u], [fiber2MagErr_g], [fiber2MagErr_r], [fiber2MagErr_i], [fiber2MagErr_z], [petroMag_u], [petroMag_g], [petroMag_r], [petroMag_i], [petroMag_z], [petroMagErr_u], [petroMagErr_g], [petroMagErr_r], [petroMagErr_i], [petroMagErr_z], [psfFlux_u], [psfFlux_g], [psfFlux_r], [psfFlux_i], [psfFlux_z], [psfFluxIvar_u], [psfFluxIvar_g], [psfFluxIvar_r], [psfFluxIvar_i], [psfFluxIvar_z], [fiberFlux_u], [fiberFlux_g], [fiberFlux_r], [fiberFlux_i], [fiberFlux_z], [fiberFluxIvar_u], [fiberFluxIvar_g], [fiberFluxIvar_r], [fiberFluxIvar_i], [fiberFluxIvar_z], [fiber2Flux_u], [fiber2Flux_g], [fiber2Flux_r], [fiber2Flux_i], [fiber2Flux_z], [fiber2FluxIvar_u], [fiber2FluxIvar_g], [fiber2FluxIvar_r], [fiber2FluxIvar_i], [fiber2FluxIvar_z], [petroFlux_u], [petroFlux_g], [petroFlux_r], [petroFlux_i], [petroFlux_z], [petroFluxIvar_u], [petroFluxIvar_g], [petroFluxIvar_r], [petroFluxIvar_i], [petroFluxIvar_z], [petroRad_u], [petroRad_g], [petroRad_r], [petroRad_i], [petroRad_z], [petroRadErr_u], [petroRadErr_g], [petroRadErr_r], [petroRadErr_i], [petroRadErr_z], [petroR50_u], [petroR50_g], [petroR50_r], [petroR50_i], [petroR50_z], [petroR50Err_u], [petroR50Err_g], [petroR50Err_r], [petroR50Err_i], [petroR50Err_z], [petroR90_u], [petroR90_g], [petroR90_r], [petroR90_i], [petroR90_z], [petroR90Err_u], [petroR90Err_g], [petroR90Err_r], [petroR90Err_i], [petroR90Err_z], [lnLStar_u], [lnLStar_g], [lnLStar_r], [lnLStar_i], [lnLStar_z], [lnLExp_u], [lnLExp_g], [lnLExp_r], [lnLExp_i], [lnLExp_z], [lnLDeV_u], [lnLDeV_g], [lnLDeV_r], [lnLDeV_i], [lnLDeV_z], [fracDeV_u], [fracDeV_g], [fracDeV_r], [fracDeV_i], [fracDeV_z], [ra], [dec], [cx], [cy], [cz], [raErr], [decErr], [b], [l], [extinction_u], [extinction_g], [extinction_r], [extinction_i], [extinction_z], [mjd], [airmass_u], [airmass_g], [airmass_r], [airmass_i], [airmass_z], [loadVersion], [htmID], [fieldID], [parentID], [specObjID], [u], [g], [r], [i], [z], [err_u], [err_g], [err_r], [err_i], [err_z], [dered_u], [dered_g], [dered_r], [dered_i], [dered_z], [resolveStatus], [thingId], [balkanId], [nObserve], [nDetect], [nEdge], [score], [calibStatus_u], [calibStatus_g], [calibStatus_r], [calibStatus_i], [calibStatus_z], [TAI_u], [TAI_g], [TAI_r], [TAI_i], [TAI_z])
 SELECT sourcetablealias.[objID], sourcetablealias.[skyVersion], sourcetablealias.[run], sourcetablealias.[rerun], sourcetablealias.[camcol], sourcetablealias.[field], sourcetablealias.[obj], sourcetablealias.[mode], sourcetablealias.[nChild], sourcetablealias.[type], sourcetablealias.[clean], sourcetablealias.[probPSF], sourcetablealias.[insideMask], sourcetablealias.[flags], sourcetablealias.[sky_u], sourcetablealias.[sky_g], sourcetablealias.[sky_r], sourcetablealias.[sky_i], sourcetablealias.[sky_z], sourcetablealias.[skyIvar_u], sourcetablealias.[skyIvar_g], sourcetablealias.[skyIvar_r], sourcetablealias.[skyIvar_i], sourcetablealias.[skyIvar_z], sourcetablealias.[psfMag_u], sourcetablealias.[psfMag_g], sourcetablealias.[psfMag_r], sourcetablealias.[psfMag_i], sourcetablealias.[psfMag_z], sourcetablealias.[psfMagErr_u], sourcetablealias.[psfMagErr_g], sourcetablealias.[psfMagErr_r], sourcetablealias.[psfMagErr_i], sourcetablealias.[psfMagErr_z], sourcetablealias.[fiberMag_u], sourcetablealias.[fiberMag_g], sourcetablealias.[fiberMag_r], sourcetablealias.[fiberMag_i], sourcetablealias.[fiberMag_z], sourcetablealias.[fiberMagErr_u], sourcetablealias.[fiberMagErr_g], sourcetablealias.[fiberMagErr_r], sourcetablealias.[fiberMagErr_i], sourcetablealias.[fiberMagErr_z], sourcetablealias.[fiber2Mag_u], sourcetablealias.[fiber2Mag_g], sourcetablealias.[fiber2Mag_r], sourcetablealias.[fiber2Mag_i], sourcetablealias.[fiber2Mag_z], sourcetablealias.[fiber2MagErr_u], sourcetablealias.[fiber2MagErr_g], sourcetablealias.[fiber2MagErr_r], sourcetablealias.[fiber2MagErr_i], sourcetablealias.[fiber2MagErr_z], sourcetablealias.[petroMag_u], sourcetablealias.[petroMag_g], sourcetablealias.[petroMag_r], sourcetablealias.[petroMag_i], sourcetablealias.[petroMag_z], sourcetablealias.[petroMagErr_u], sourcetablealias.[petroMagErr_g], sourcetablealias.[petroMagErr_r], sourcetablealias.[petroMagErr_i], sourcetablealias.[petroMagErr_z], sourcetablealias.[psfFlux_u], sourcetablealias.[psfFlux_g], sourcetablealias.[psfFlux_r], sourcetablealias.[psfFlux_i], sourcetablealias.[psfFlux_z], sourcetablealias.[psfFluxIvar_u], sourcetablealias.[psfFluxIvar_g], sourcetablealias.[psfFluxIvar_r], sourcetablealias.[psfFluxIvar_i], sourcetablealias.[psfFluxIvar_z], sourcetablealias.[fiberFlux_u], sourcetablealias.[fiberFlux_g], sourcetablealias.[fiberFlux_r], sourcetablealias.[fiberFlux_i], sourcetablealias.[fiberFlux_z], sourcetablealias.[fiberFluxIvar_u], sourcetablealias.[fiberFluxIvar_g], sourcetablealias.[fiberFluxIvar_r], sourcetablealias.[fiberFluxIvar_i], sourcetablealias.[fiberFluxIvar_z], sourcetablealias.[fiber2Flux_u], sourcetablealias.[fiber2Flux_g], sourcetablealias.[fiber2Flux_r], sourcetablealias.[fiber2Flux_i], sourcetablealias.[fiber2Flux_z], sourcetablealias.[fiber2FluxIvar_u], sourcetablealias.[fiber2FluxIvar_g], sourcetablealias.[fiber2FluxIvar_r], sourcetablealias.[fiber2FluxIvar_i], sourcetablealias.[fiber2FluxIvar_z], sourcetablealias.[petroFlux_u], sourcetablealias.[petroFlux_g], sourcetablealias.[petroFlux_r], sourcetablealias.[petroFlux_i], sourcetablealias.[petroFlux_z], sourcetablealias.[petroFluxIvar_u], sourcetablealias.[petroFluxIvar_g], sourcetablealias.[petroFluxIvar_r], sourcetablealias.[petroFluxIvar_i], sourcetablealias.[petroFluxIvar_z], sourcetablealias.[petroRad_u], sourcetablealias.[petroRad_g], sourcetablealias.[petroRad_r], sourcetablealias.[petroRad_i], sourcetablealias.[petroRad_z], sourcetablealias.[petroRadErr_u], sourcetablealias.[petroRadErr_g], sourcetablealias.[petroRadErr_r], sourcetablealias.[petroRadErr_i], sourcetablealias.[petroRadErr_z], sourcetablealias.[petroR50_u], sourcetablealias.[petroR50_g], sourcetablealias.[petroR50_r], sourcetablealias.[petroR50_i], sourcetablealias.[petroR50_z], sourcetablealias.[petroR50Err_u], sourcetablealias.[petroR50Err_g], sourcetablealias.[petroR50Err_r], sourcetablealias.[petroR50Err_i], sourcetablealias.[petroR50Err_z], sourcetablealias.[petroR90_u], sourcetablealias.[petroR90_g], sourcetablealias.[petroR90_r], sourcetablealias.[petroR90_i], sourcetablealias.[petroR90_z], sourcetablealias.[petroR90Err_u], sourcetablealias.[petroR90Err_g], sourcetablealias.[petroR90Err_r], sourcetablealias.[petroR90Err_i], sourcetablealias.[petroR90Err_z], sourcetablealias.[lnLStar_u], sourcetablealias.[lnLStar_g], sourcetablealias.[lnLStar_r], sourcetablealias.[lnLStar_i], sourcetablealias.[lnLStar_z], sourcetablealias.[lnLExp_u], sourcetablealias.[lnLExp_g], sourcetablealias.[lnLExp_r], sourcetablealias.[lnLExp_i], sourcetablealias.[lnLExp_z], sourcetablealias.[lnLDeV_u], sourcetablealias.[lnLDeV_g], sourcetablealias.[lnLDeV_r], sourcetablealias.[lnLDeV_i], sourcetablealias.[lnLDeV_z], sourcetablealias.[fracDeV_u], sourcetablealias.[fracDeV_g], sourcetablealias.[fracDeV_r], sourcetablealias.[fracDeV_i], sourcetablealias.[fracDeV_z], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[raErr], sourcetablealias.[decErr], sourcetablealias.[b], sourcetablealias.[l], sourcetablealias.[extinction_u], sourcetablealias.[extinction_g], sourcetablealias.[extinction_r], sourcetablealias.[extinction_i], sourcetablealias.[extinction_z], sourcetablealias.[mjd], sourcetablealias.[airmass_u], sourcetablealias.[airmass_g], sourcetablealias.[airmass_r], sourcetablealias.[airmass_i], sourcetablealias.[airmass_z], sourcetablealias.[loadVersion], sourcetablealias.[htmID], sourcetablealias.[fieldID], sourcetablealias.[parentID], sourcetablealias.[specObjID], sourcetablealias.[u], sourcetablealias.[g], sourcetablealias.[r], sourcetablealias.[i], sourcetablealias.[z], sourcetablealias.[err_u], sourcetablealias.[err_g], sourcetablealias.[err_r], sourcetablealias.[err_i], sourcetablealias.[err_z], sourcetablealias.[dered_u], sourcetablealias.[dered_g], sourcetablealias.[dered_r], sourcetablealias.[dered_i], sourcetablealias.[dered_z], sourcetablealias.[resolveStatus], sourcetablealias.[thingId], sourcetablealias.[balkanId], sourcetablealias.[nObserve], sourcetablealias.[nDetect], sourcetablealias.[nEdge], sourcetablealias.[score], sourcetablealias.[calibStatus_u], sourcetablealias.[calibStatus_g], sourcetablealias.[calibStatus_r], sourcetablealias.[calibStatus_i], sourcetablealias.[calibStatus_z], sourcetablealias.[TAI_u], sourcetablealias.[TAI_g], sourcetablealias.[TAI_r], sourcetablealias.[TAI_i], sourcetablealias.[TAI_z]
 FROM   [SkyNode_SDSSDR12].[dbo].[PhotoObjAll] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objID = sourcetablealias.objID
	;


GO

-- SUBSAMPLING TABLE 'PhotoObjDR7' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[dr8objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[dr8objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[PhotoObjDR7] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [dr8objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[PhotoObjDR7] WITH (TABLOCKX)
	([dr7objid], [dr8objid], [distance], [modeDR7], [modeDR8], [skyVersion], [run], [rerun], [camcol], [field], [obj], [nChild], [type], [probPSF], [insideMask], [flags], [psfMag_u], [psfMag_g], [psfMag_r], [psfMag_i], [psfMag_z], [psfMagErr_u], [psfMagErr_g], [psfMagErr_r], [psfMagErr_i], [psfMagErr_z], [petroMag_u], [petroMag_g], [petroMag_r], [petroMag_i], [petroMag_z], [petroMagErr_u], [petroMagErr_g], [petroMagErr_r], [petroMagErr_i], [petroMagErr_z], [petroR50_r], [petroR90_r], [modelMag_u], [modelMag_g], [modelMag_r], [modelMag_i], [modelMag_z], [modelMagErr_u], [modelMagErr_g], [modelMagErr_r], [modelMagErr_i], [modelMagErr_z], [mRrCc_r], [mRrCcErr_r], [lnLStar_r], [lnLExp_r], [lnLDeV_r], [status], [ra], [dec], [cx], [cy], [cz], [primTarget], [secTarget], [extinction_u], [extinction_g], [extinction_r], [extinction_i], [extinction_z], [htmID], [fieldID], [SpecObjID], [size])
 SELECT sourcetablealias.[dr7objid], sourcetablealias.[dr8objid], sourcetablealias.[distance], sourcetablealias.[modeDR7], sourcetablealias.[modeDR8], sourcetablealias.[skyVersion], sourcetablealias.[run], sourcetablealias.[rerun], sourcetablealias.[camcol], sourcetablealias.[field], sourcetablealias.[obj], sourcetablealias.[nChild], sourcetablealias.[type], sourcetablealias.[probPSF], sourcetablealias.[insideMask], sourcetablealias.[flags], sourcetablealias.[psfMag_u], sourcetablealias.[psfMag_g], sourcetablealias.[psfMag_r], sourcetablealias.[psfMag_i], sourcetablealias.[psfMag_z], sourcetablealias.[psfMagErr_u], sourcetablealias.[psfMagErr_g], sourcetablealias.[psfMagErr_r], sourcetablealias.[psfMagErr_i], sourcetablealias.[psfMagErr_z], sourcetablealias.[petroMag_u], sourcetablealias.[petroMag_g], sourcetablealias.[petroMag_r], sourcetablealias.[petroMag_i], sourcetablealias.[petroMag_z], sourcetablealias.[petroMagErr_u], sourcetablealias.[petroMagErr_g], sourcetablealias.[petroMagErr_r], sourcetablealias.[petroMagErr_i], sourcetablealias.[petroMagErr_z], sourcetablealias.[petroR50_r], sourcetablealias.[petroR90_r], sourcetablealias.[modelMag_u], sourcetablealias.[modelMag_g], sourcetablealias.[modelMag_r], sourcetablealias.[modelMag_i], sourcetablealias.[modelMag_z], sourcetablealias.[modelMagErr_u], sourcetablealias.[modelMagErr_g], sourcetablealias.[modelMagErr_r], sourcetablealias.[modelMagErr_i], sourcetablealias.[modelMagErr_z], sourcetablealias.[mRrCc_r], sourcetablealias.[mRrCcErr_r], sourcetablealias.[lnLStar_r], sourcetablealias.[lnLExp_r], sourcetablealias.[lnLDeV_r], sourcetablealias.[status], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[primTarget], sourcetablealias.[secTarget], sourcetablealias.[extinction_u], sourcetablealias.[extinction_g], sourcetablealias.[extinction_r], sourcetablealias.[extinction_i], sourcetablealias.[extinction_z], sourcetablealias.[htmID], sourcetablealias.[fieldID], sourcetablealias.[SpecObjID], sourcetablealias.[size]
 FROM   [SkyNode_SDSSDR12].[dbo].[PhotoObjDR7] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.dr8objid = sourcetablealias.dr8objid
	;


GO

-- SUBSAMPLING TABLE 'PhotoPrimaryDR7' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[dr8objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[dr8objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[PhotoPrimaryDR7] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [dr8objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[PhotoPrimaryDR7] WITH (TABLOCKX)
	([dr7objid], [dr8objid], [distance], [skyVersion], [run], [rerun], [camcol], [field], [obj], [nChild], [type], [probPSF], [insideMask], [flags], [psfMag_u], [psfMag_g], [psfMag_r], [psfMag_i], [psfMag_z], [psfMagErr_u], [psfMagErr_g], [psfMagErr_r], [psfMagErr_i], [psfMagErr_z], [petroMag_u], [petroMag_g], [petroMag_r], [petroMag_i], [petroMag_z], [petroMagErr_u], [petroMagErr_g], [petroMagErr_r], [petroMagErr_i], [petroMagErr_z], [petroR50_r], [petroR90_r], [modelMag_u], [modelMag_g], [modelMag_r], [modelMag_i], [modelMag_z], [modelMagErr_u], [modelMagErr_g], [modelMagErr_r], [modelMagErr_i], [modelMagErr_z], [mRrCc_r], [mRrCcErr_r], [lnLStar_r], [lnLExp_r], [lnLDeV_r], [status], [ra], [dec], [cx], [cy], [cz], [primTarget], [secTarget], [extinction_u], [extinction_g], [extinction_r], [extinction_i], [extinction_z], [htmID], [fieldID], [SpecObjID], [size])
 SELECT sourcetablealias.[dr7objid], sourcetablealias.[dr8objid], sourcetablealias.[distance], sourcetablealias.[skyVersion], sourcetablealias.[run], sourcetablealias.[rerun], sourcetablealias.[camcol], sourcetablealias.[field], sourcetablealias.[obj], sourcetablealias.[nChild], sourcetablealias.[type], sourcetablealias.[probPSF], sourcetablealias.[insideMask], sourcetablealias.[flags], sourcetablealias.[psfMag_u], sourcetablealias.[psfMag_g], sourcetablealias.[psfMag_r], sourcetablealias.[psfMag_i], sourcetablealias.[psfMag_z], sourcetablealias.[psfMagErr_u], sourcetablealias.[psfMagErr_g], sourcetablealias.[psfMagErr_r], sourcetablealias.[psfMagErr_i], sourcetablealias.[psfMagErr_z], sourcetablealias.[petroMag_u], sourcetablealias.[petroMag_g], sourcetablealias.[petroMag_r], sourcetablealias.[petroMag_i], sourcetablealias.[petroMag_z], sourcetablealias.[petroMagErr_u], sourcetablealias.[petroMagErr_g], sourcetablealias.[petroMagErr_r], sourcetablealias.[petroMagErr_i], sourcetablealias.[petroMagErr_z], sourcetablealias.[petroR50_r], sourcetablealias.[petroR90_r], sourcetablealias.[modelMag_u], sourcetablealias.[modelMag_g], sourcetablealias.[modelMag_r], sourcetablealias.[modelMag_i], sourcetablealias.[modelMag_z], sourcetablealias.[modelMagErr_u], sourcetablealias.[modelMagErr_g], sourcetablealias.[modelMagErr_r], sourcetablealias.[modelMagErr_i], sourcetablealias.[modelMagErr_z], sourcetablealias.[mRrCc_r], sourcetablealias.[mRrCcErr_r], sourcetablealias.[lnLStar_r], sourcetablealias.[lnLExp_r], sourcetablealias.[lnLDeV_r], sourcetablealias.[status], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[primTarget], sourcetablealias.[secTarget], sourcetablealias.[extinction_u], sourcetablealias.[extinction_g], sourcetablealias.[extinction_r], sourcetablealias.[extinction_i], sourcetablealias.[extinction_z], sourcetablealias.[htmID], sourcetablealias.[fieldID], sourcetablealias.[SpecObjID], sourcetablealias.[size]
 FROM   [SkyNode_SDSSDR12].[dbo].[PhotoPrimaryDR7] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.dr8objid = sourcetablealias.dr8objid
	;


GO

-- SUBSAMPLING TABLE 'Photoz' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[objID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[objID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[Photoz] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[Photoz] WITH (TABLOCKX)
	([objID], [z], [zErr], [nnCount], [nnVol], [nnIsInside], [nnObjID], [nnSpecz], [nnFarObjID], [nnAvgZ], [distMod], [lumDist], [chisq], [rnorm], [nTemplates], [synthU], [synthG], [synthR], [synthI], [synthZ], [kcorrU], [kcorrG], [kcorrR], [kcorrI], [kcorrZ], [kcorrU01], [kcorrG01], [kcorrR01], [kcorrI01], [kcorrZ01], [absMagU], [absMagG], [absMagR], [absMagI], [absMagZ])
 SELECT sourcetablealias.[objID], sourcetablealias.[z], sourcetablealias.[zErr], sourcetablealias.[nnCount], sourcetablealias.[nnVol], sourcetablealias.[nnIsInside], sourcetablealias.[nnObjID], sourcetablealias.[nnSpecz], sourcetablealias.[nnFarObjID], sourcetablealias.[nnAvgZ], sourcetablealias.[distMod], sourcetablealias.[lumDist], sourcetablealias.[chisq], sourcetablealias.[rnorm], sourcetablealias.[nTemplates], sourcetablealias.[synthU], sourcetablealias.[synthG], sourcetablealias.[synthR], sourcetablealias.[synthI], sourcetablealias.[synthZ], sourcetablealias.[kcorrU], sourcetablealias.[kcorrG], sourcetablealias.[kcorrR], sourcetablealias.[kcorrI], sourcetablealias.[kcorrZ], sourcetablealias.[kcorrU01], sourcetablealias.[kcorrG01], sourcetablealias.[kcorrR01], sourcetablealias.[kcorrI01], sourcetablealias.[kcorrZ01], sourcetablealias.[absMagU], sourcetablealias.[absMagG], sourcetablealias.[absMagR], sourcetablealias.[absMagI], sourcetablealias.[absMagZ]
 FROM   [SkyNode_SDSSDR12].[dbo].[Photoz] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objID = sourcetablealias.objID
	;


GO

-- SUBSAMPLING TABLE 'PhotozRF' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[objID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[objID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[PhotozRF] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[PhotozRF] WITH (TABLOCKX)
	([objID], [z], [zErr], [distMod], [lumDist], [chisq], [rnorm], [nTemplates], [synthU], [synthG], [synthR], [synthI], [synthZ], [kcorrU], [kcorrG], [kcorrR], [kcorrI], [kcorrZ], [kcorrU01], [kcorrG01], [kcorrR01], [kcorrI01], [kcorrZ01], [absMagU], [absMagG], [absMagR], [absMagI], [absMagZ])
 SELECT sourcetablealias.[objID], sourcetablealias.[z], sourcetablealias.[zErr], sourcetablealias.[distMod], sourcetablealias.[lumDist], sourcetablealias.[chisq], sourcetablealias.[rnorm], sourcetablealias.[nTemplates], sourcetablealias.[synthU], sourcetablealias.[synthG], sourcetablealias.[synthR], sourcetablealias.[synthI], sourcetablealias.[synthZ], sourcetablealias.[kcorrU], sourcetablealias.[kcorrG], sourcetablealias.[kcorrR], sourcetablealias.[kcorrI], sourcetablealias.[kcorrZ], sourcetablealias.[kcorrU01], sourcetablealias.[kcorrG01], sourcetablealias.[kcorrR01], sourcetablealias.[kcorrI01], sourcetablealias.[kcorrZ01], sourcetablealias.[absMagU], sourcetablealias.[absMagG], sourcetablealias.[absMagR], sourcetablealias.[absMagI], sourcetablealias.[absMagZ]
 FROM   [SkyNode_SDSSDR12].[dbo].[PhotozRF] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objID = sourcetablealias.objID
	;


GO

-- SUBSAMPLING TABLE 'PhotozRFTemplateCoeff' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[objID] bigint, [templateID] int
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[objID], sourcetablealias.[templateID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[PhotozRFTemplateCoeff] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objID], [templateID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[PhotozRFTemplateCoeff] WITH (TABLOCKX)
	([objID], [templateID], [coeff])
 SELECT sourcetablealias.[objID], sourcetablealias.[templateID], sourcetablealias.[coeff]
 FROM   [SkyNode_SDSSDR12].[dbo].[PhotozRFTemplateCoeff] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objID = sourcetablealias.objID AND ##temporaryidlist.templateID = sourcetablealias.templateID
	;


GO

-- SUBSAMPLING TABLE 'PhotozTemplateCoeff' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[objID] bigint, [templateID] int
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[objID], sourcetablealias.[templateID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[PhotozTemplateCoeff] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objID], [templateID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[PhotozTemplateCoeff] WITH (TABLOCKX)
	([objID], [templateID], [coeff])
 SELECT sourcetablealias.[objID], sourcetablealias.[templateID], sourcetablealias.[coeff]
 FROM   [SkyNode_SDSSDR12].[dbo].[PhotozTemplateCoeff] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objID = sourcetablealias.objID AND ##temporaryidlist.templateID = sourcetablealias.templateID
	;


GO

-- SUBSAMPLING TABLE 'Plate2Target' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT , master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[Plate2Target] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT 
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[Plate2Target] WITH (TABLOCKX)
	([plate2targetid], [plate], [plateid], [objid], [loadVersion])
 SELECT sourcetablealias.[plate2targetid], sourcetablealias.[plate], sourcetablealias.[plateid], sourcetablealias.[objid], sourcetablealias.[loadVersion]
 FROM   [SkyNode_SDSSDR12].[dbo].[Plate2Target] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON [$PrimaryKeyJoinCondition]
	;


GO

-- SUBSAMPLING TABLE 'PlateX' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[PlateX] WITH (TABLOCKX)
    ([plateID], [firstRelease], [plate], [mjd], [mjdList], [survey], [programName], [instrument], [chunk], [plateRun], [designComments], [plateQuality], [qualityComments], [plateSN2], [deredSN2], [ra], [dec], [run2d], [run1d], [runsspp], [tile], [designID], [locationID], [iopVersion], [camVersion], [taiHMS], [dateObs], [timeSys], [cx], [cy], [cz], [cartridgeID], [tai], [taiBegin], [taiEnd], [airmass], [mapMjd], [mapName], [plugFile], [expTime], [expTimeB1], [expTimeB2], [expTimeR1], [expTimeR2], [vers2d], [verscomb], [vers1d], [snturnoff], [nturnoff], [nExp], [nExpB1], [nExpB2], [nExpR1], [nExpR2], [sn1_g], [sn1_r], [sn1_i], [sn2_g], [sn2_r], [sn2_i], [dered_sn1_g], [dered_sn1_r], [dered_sn1_i], [dered_sn2_g], [dered_sn2_r], [dered_sn2_i], [helioRV], [gOffStd], [gRMSStd], [rOffStd], [rRMSStd], [iOffStd], [iRMSStd], [grOffStd], [grRMSStd], [riOffStd], [riRMSStd], [gOffGal], [gRMSGal], [rOffGal], [rRMSGal], [iOffGal], [iRMSGal], [grOffGal], [grRMSGal], [riOffGal], [riRMSGal], [nGuide], [seeing20], [seeing50], [seeing80], [rmsoff20], [rmsoff50], [rmsoff80], [airtemp], [sfd_used], [xSigma], [xSigMin], [xSigMax], [wSigma], [wSigMin], [wSigMax], [xChi2], [xChi2Min], [xChi2Max], [skyChi2], [skyChi2Min], [skyChi2Max], [fBadPix], [fBadPix1], [fBadPix2], [status2d], [statuscombine], [status1d], [nTotal], [nGalaxy], [nQSO], [nStar], [nSky], [nUnknown], [isBest], [isPrimary], [isTile], [ha], [mjdDesign], [theta], [fscanVersion], [fmapVersion], [fscanMode], [fscanSpeed], [htmID], [loadVersion])
 SELECT sourcetablealias.[plateID], sourcetablealias.[firstRelease], sourcetablealias.[plate], sourcetablealias.[mjd], sourcetablealias.[mjdList], sourcetablealias.[survey], sourcetablealias.[programName], sourcetablealias.[instrument], sourcetablealias.[chunk], sourcetablealias.[plateRun], sourcetablealias.[designComments], sourcetablealias.[plateQuality], sourcetablealias.[qualityComments], sourcetablealias.[plateSN2], sourcetablealias.[deredSN2], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[run2d], sourcetablealias.[run1d], sourcetablealias.[runsspp], sourcetablealias.[tile], sourcetablealias.[designID], sourcetablealias.[locationID], sourcetablealias.[iopVersion], sourcetablealias.[camVersion], sourcetablealias.[taiHMS], sourcetablealias.[dateObs], sourcetablealias.[timeSys], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[cartridgeID], sourcetablealias.[tai], sourcetablealias.[taiBegin], sourcetablealias.[taiEnd], sourcetablealias.[airmass], sourcetablealias.[mapMjd], sourcetablealias.[mapName], sourcetablealias.[plugFile], sourcetablealias.[expTime], sourcetablealias.[expTimeB1], sourcetablealias.[expTimeB2], sourcetablealias.[expTimeR1], sourcetablealias.[expTimeR2], sourcetablealias.[vers2d], sourcetablealias.[verscomb], sourcetablealias.[vers1d], sourcetablealias.[snturnoff], sourcetablealias.[nturnoff], sourcetablealias.[nExp], sourcetablealias.[nExpB1], sourcetablealias.[nExpB2], sourcetablealias.[nExpR1], sourcetablealias.[nExpR2], sourcetablealias.[sn1_g], sourcetablealias.[sn1_r], sourcetablealias.[sn1_i], sourcetablealias.[sn2_g], sourcetablealias.[sn2_r], sourcetablealias.[sn2_i], sourcetablealias.[dered_sn1_g], sourcetablealias.[dered_sn1_r], sourcetablealias.[dered_sn1_i], sourcetablealias.[dered_sn2_g], sourcetablealias.[dered_sn2_r], sourcetablealias.[dered_sn2_i], sourcetablealias.[helioRV], sourcetablealias.[gOffStd], sourcetablealias.[gRMSStd], sourcetablealias.[rOffStd], sourcetablealias.[rRMSStd], sourcetablealias.[iOffStd], sourcetablealias.[iRMSStd], sourcetablealias.[grOffStd], sourcetablealias.[grRMSStd], sourcetablealias.[riOffStd], sourcetablealias.[riRMSStd], sourcetablealias.[gOffGal], sourcetablealias.[gRMSGal], sourcetablealias.[rOffGal], sourcetablealias.[rRMSGal], sourcetablealias.[iOffGal], sourcetablealias.[iRMSGal], sourcetablealias.[grOffGal], sourcetablealias.[grRMSGal], sourcetablealias.[riOffGal], sourcetablealias.[riRMSGal], sourcetablealias.[nGuide], sourcetablealias.[seeing20], sourcetablealias.[seeing50], sourcetablealias.[seeing80], sourcetablealias.[rmsoff20], sourcetablealias.[rmsoff50], sourcetablealias.[rmsoff80], sourcetablealias.[airtemp], sourcetablealias.[sfd_used], sourcetablealias.[xSigma], sourcetablealias.[xSigMin], sourcetablealias.[xSigMax], sourcetablealias.[wSigma], sourcetablealias.[wSigMin], sourcetablealias.[wSigMax], sourcetablealias.[xChi2], sourcetablealias.[xChi2Min], sourcetablealias.[xChi2Max], sourcetablealias.[skyChi2], sourcetablealias.[skyChi2Min], sourcetablealias.[skyChi2Max], sourcetablealias.[fBadPix], sourcetablealias.[fBadPix1], sourcetablealias.[fBadPix2], sourcetablealias.[status2d], sourcetablealias.[statuscombine], sourcetablealias.[status1d], sourcetablealias.[nTotal], sourcetablealias.[nGalaxy], sourcetablealias.[nQSO], sourcetablealias.[nStar], sourcetablealias.[nSky], sourcetablealias.[nUnknown], sourcetablealias.[isBest], sourcetablealias.[isPrimary], sourcetablealias.[isTile], sourcetablealias.[ha], sourcetablealias.[mjdDesign], sourcetablealias.[theta], sourcetablealias.[fscanVersion], sourcetablealias.[fmapVersion], sourcetablealias.[fscanMode], sourcetablealias.[fscanSpeed], sourcetablealias.[htmID], sourcetablealias.[loadVersion]
 FROM   [SkyNode_SDSSDR12].[dbo].[PlateX] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'ProperMotions' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[ProperMotions] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[ProperMotions] WITH (TABLOCKX)
	([delta], [match], [pmL], [pmB], [pmRa], [pmDec], [pmRaErr], [pmDecErr], [sigRa], [sigDec], [nFit], [O], [E], [J], [F], [N], [dist20], [dist22], [objid])
 SELECT sourcetablealias.[delta], sourcetablealias.[match], sourcetablealias.[pmL], sourcetablealias.[pmB], sourcetablealias.[pmRa], sourcetablealias.[pmDec], sourcetablealias.[pmRaErr], sourcetablealias.[pmDecErr], sourcetablealias.[sigRa], sourcetablealias.[sigDec], sourcetablealias.[nFit], sourcetablealias.[O], sourcetablealias.[E], sourcetablealias.[J], sourcetablealias.[F], sourcetablealias.[N], sourcetablealias.[dist20], sourcetablealias.[dist22], sourcetablealias.[objid]
 FROM   [SkyNode_SDSSDR12].[dbo].[ProperMotions] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objid = sourcetablealias.objid
	;


GO

-- SUBSAMPLING TABLE 'RegionTypes' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[RegionTypes] WITH (TABLOCKX)
    ([type], [radius])
 SELECT sourcetablealias.[type], sourcetablealias.[radius]
 FROM   [SkyNode_SDSSDR12].[dbo].[RegionTypes] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'Rmatrix' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[Rmatrix] WITH (TABLOCKX)
    ([mode], [row], [x], [y], [z])
 SELECT sourcetablealias.[mode], sourcetablealias.[row], sourcetablealias.[x], sourcetablealias.[y], sourcetablealias.[z]
 FROM   [SkyNode_SDSSDR12].[dbo].[Rmatrix] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'Run' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[Run] WITH (TABLOCKX)
    ([skyVersion], [run], [rerun], [mjd], [datestring], [stripe], [strip], [xBore], [fieldRef], [lastField], [flavor], [xBin], [yBin], [nRow], [mjdRef], [muRef], [lineStart], [tracking], [node], [incl], [comments], [qterm], [maxMuResid], [maxNuResid], [startField], [endField], [photoVersion], [dervishVersion], [astromVersion], [sasVersion])
 SELECT sourcetablealias.[skyVersion], sourcetablealias.[run], sourcetablealias.[rerun], sourcetablealias.[mjd], sourcetablealias.[datestring], sourcetablealias.[stripe], sourcetablealias.[strip], sourcetablealias.[xBore], sourcetablealias.[fieldRef], sourcetablealias.[lastField], sourcetablealias.[flavor], sourcetablealias.[xBin], sourcetablealias.[yBin], sourcetablealias.[nRow], sourcetablealias.[mjdRef], sourcetablealias.[muRef], sourcetablealias.[lineStart], sourcetablealias.[tracking], sourcetablealias.[node], sourcetablealias.[incl], sourcetablealias.[comments], sourcetablealias.[qterm], sourcetablealias.[maxMuResid], sourcetablealias.[maxNuResid], sourcetablealias.[startField], sourcetablealias.[endField], sourcetablealias.[photoVersion], sourcetablealias.[dervishVersion], sourcetablealias.[astromVersion], sourcetablealias.[sasVersion]
 FROM   [SkyNode_SDSSDR12].[dbo].[Run] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'RunShift' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[RunShift] WITH (TABLOCKX)
    ([run], [shift])
 SELECT sourcetablealias.[run], sourcetablealias.[shift]
 FROM   [SkyNode_SDSSDR12].[dbo].[RunShift] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'SDSSConstants' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[SDSSConstants] WITH (TABLOCKX)
    ([name], [value], [unit], [description])
 SELECT sourcetablealias.[name], sourcetablealias.[value], sourcetablealias.[unit], sourcetablealias.[description]
 FROM   [SkyNode_SDSSDR12].[dbo].[SDSSConstants] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'sdssTargetParam' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[sdssTargetParam] WITH (TABLOCKX)
    ([targetVersion], [paramFile], [name], [value])
 SELECT sourcetablealias.[targetVersion], sourcetablealias.[paramFile], sourcetablealias.[name], sourcetablealias.[value]
 FROM   [SkyNode_SDSSDR12].[dbo].[sdssTargetParam] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'sdssTileAll' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[sdssTileAll] WITH (TABLOCKX)
    ([tile], [tileRun], [raCen], [decCen], [htmID], [cx], [cy], [cz], [untiled], [nTargets], [loadVersion])
 SELECT sourcetablealias.[tile], sourcetablealias.[tileRun], sourcetablealias.[raCen], sourcetablealias.[decCen], sourcetablealias.[htmID], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[untiled], sourcetablealias.[nTargets], sourcetablealias.[loadVersion]
 FROM   [SkyNode_SDSSDR12].[dbo].[sdssTileAll] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'sdssTilingGeometry' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[sdssTilingGeometry] WITH (TABLOCKX)
    ([tilingGeometryID], [tileRun], [stripe], [nsbx], [isMask], [coordType], [lambdamu_0], [lambdamu_1], [etanu_0], [etanu_1], [lambdaLimit_0], [lambdaLimit_1], [targetVersion], [firstArea], [loadVersion])
 SELECT sourcetablealias.[tilingGeometryID], sourcetablealias.[tileRun], sourcetablealias.[stripe], sourcetablealias.[nsbx], sourcetablealias.[isMask], sourcetablealias.[coordType], sourcetablealias.[lambdamu_0], sourcetablealias.[lambdamu_1], sourcetablealias.[etanu_0], sourcetablealias.[etanu_1], sourcetablealias.[lambdaLimit_0], sourcetablealias.[lambdaLimit_1], sourcetablealias.[targetVersion], sourcetablealias.[firstArea], sourcetablealias.[loadVersion]
 FROM   [SkyNode_SDSSDR12].[dbo].[sdssTilingGeometry] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'sdssTilingRun' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[sdssTilingRun] WITH (TABLOCKX)
    ([tileRun], [ctileVersion], [tilepId], [programName], [primTargetMask], [secTargetMask], [loadVersion])
 SELECT sourcetablealias.[tileRun], sourcetablealias.[ctileVersion], sourcetablealias.[tilepId], sourcetablealias.[programName], sourcetablealias.[primTargetMask], sourcetablealias.[secTargetMask], sourcetablealias.[loadVersion]
 FROM   [SkyNode_SDSSDR12].[dbo].[sdssTilingRun] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'segueTargetAll' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[segueTargetAll] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[segueTargetAll] WITH (TABLOCKX)
	([objid], [segue1_target1], [segue1_target2], [segue2_target1], [segue2_target2], [lcolor], [scolor], [p1s], [totalpm], [hg], [Mi], [disti], [Hr], [vmi_trans1], [vmi_trans2], [vmag_trans], [Mv_trans], [distv_kpc], [vtrans_galrest], [mutrans_galradrest], [murad_galradrest], [vtot_galradrest], [mg_tohv], [vtrans_tohv], [pm1sigma_tohv], [v1sigmaerr_tohv])
 SELECT sourcetablealias.[objid], sourcetablealias.[segue1_target1], sourcetablealias.[segue1_target2], sourcetablealias.[segue2_target1], sourcetablealias.[segue2_target2], sourcetablealias.[lcolor], sourcetablealias.[scolor], sourcetablealias.[p1s], sourcetablealias.[totalpm], sourcetablealias.[hg], sourcetablealias.[Mi], sourcetablealias.[disti], sourcetablealias.[Hr], sourcetablealias.[vmi_trans1], sourcetablealias.[vmi_trans2], sourcetablealias.[vmag_trans], sourcetablealias.[Mv_trans], sourcetablealias.[distv_kpc], sourcetablealias.[vtrans_galrest], sourcetablealias.[mutrans_galradrest], sourcetablealias.[murad_galradrest], sourcetablealias.[vtot_galradrest], sourcetablealias.[mg_tohv], sourcetablealias.[vtrans_tohv], sourcetablealias.[pm1sigma_tohv], sourcetablealias.[v1sigmaerr_tohv]
 FROM   [SkyNode_SDSSDR12].[dbo].[segueTargetAll] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.objid = sourcetablealias.objid
	;


GO

-- SUBSAMPLING TABLE 'SpecDR7' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[SpecDR7] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[SpecDR7] WITH (TABLOCKX)
	([specObjID], [dr7ObjID], [ra], [dec], [cx], [cy], [cz], [skyVersion], [run], [rerun], [camcol], [field], [obj], [nChild], [type], [probPSF], [insideMask], [flags], [psfMag_u], [psfMag_g], [psfMag_r], [psfMag_i], [psfMag_z], [psfMagErr_u], [psfMagErr_g], [psfMagErr_r], [psfMagErr_i], [psfMagErr_z], [petroMag_u], [petroMag_g], [petroMag_r], [petroMag_i], [petroMag_z], [petroMagErr_u], [petroMagErr_g], [petroMagErr_r], [petroMagErr_i], [petroMagErr_z], [petroR50_r], [petroR90_r], [modelMag_u], [modelMag_g], [modelMag_r], [modelMag_i], [modelMag_z], [modelMagErr_u], [modelMagErr_g], [modelMagErr_r], [modelMagErr_i], [modelMagErr_z], [mRrCc_r], [mRrCcErr_r], [lnLStar_r], [lnLExp_r], [lnLDeV_r], [status], [primTarget], [secTarget], [extinction_u], [extinction_g], [extinction_r], [extinction_i], [extinction_z], [htmID], [fieldID], [size], [pmRa], [pmDec], [pmL], [pmB], [pmRaErr], [pmDecErr], [delta], [match])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[dr7ObjID], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[skyVersion], sourcetablealias.[run], sourcetablealias.[rerun], sourcetablealias.[camcol], sourcetablealias.[field], sourcetablealias.[obj], sourcetablealias.[nChild], sourcetablealias.[type], sourcetablealias.[probPSF], sourcetablealias.[insideMask], sourcetablealias.[flags], sourcetablealias.[psfMag_u], sourcetablealias.[psfMag_g], sourcetablealias.[psfMag_r], sourcetablealias.[psfMag_i], sourcetablealias.[psfMag_z], sourcetablealias.[psfMagErr_u], sourcetablealias.[psfMagErr_g], sourcetablealias.[psfMagErr_r], sourcetablealias.[psfMagErr_i], sourcetablealias.[psfMagErr_z], sourcetablealias.[petroMag_u], sourcetablealias.[petroMag_g], sourcetablealias.[petroMag_r], sourcetablealias.[petroMag_i], sourcetablealias.[petroMag_z], sourcetablealias.[petroMagErr_u], sourcetablealias.[petroMagErr_g], sourcetablealias.[petroMagErr_r], sourcetablealias.[petroMagErr_i], sourcetablealias.[petroMagErr_z], sourcetablealias.[petroR50_r], sourcetablealias.[petroR90_r], sourcetablealias.[modelMag_u], sourcetablealias.[modelMag_g], sourcetablealias.[modelMag_r], sourcetablealias.[modelMag_i], sourcetablealias.[modelMag_z], sourcetablealias.[modelMagErr_u], sourcetablealias.[modelMagErr_g], sourcetablealias.[modelMagErr_r], sourcetablealias.[modelMagErr_i], sourcetablealias.[modelMagErr_z], sourcetablealias.[mRrCc_r], sourcetablealias.[mRrCcErr_r], sourcetablealias.[lnLStar_r], sourcetablealias.[lnLExp_r], sourcetablealias.[lnLDeV_r], sourcetablealias.[status], sourcetablealias.[primTarget], sourcetablealias.[secTarget], sourcetablealias.[extinction_u], sourcetablealias.[extinction_g], sourcetablealias.[extinction_r], sourcetablealias.[extinction_i], sourcetablealias.[extinction_z], sourcetablealias.[htmID], sourcetablealias.[fieldID], sourcetablealias.[size], sourcetablealias.[pmRa], sourcetablealias.[pmDec], sourcetablealias.[pmL], sourcetablealias.[pmB], sourcetablealias.[pmRaErr], sourcetablealias.[pmDecErr], sourcetablealias.[delta], sourcetablealias.[match]
 FROM   [SkyNode_SDSSDR12].[dbo].[SpecDR7] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'SpecObjAll' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[SpecObjAll] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[SpecObjAll] WITH (TABLOCKX)
	([specObjID], [bestObjID], [fluxObjID], [targetObjID], [plateID], [sciencePrimary], [legacyPrimary], [seguePrimary], [segue1Primary], [segue2Primary], [bossPrimary], [sdssPrimary], [bossSpecObjID], [firstRelease], [survey], [instrument], [programname], [chunk], [platerun], [mjd], [plate], [fiberID], [run1d], [run2d], [tile], [designID], [legacy_target1], [legacy_target2], [special_target1], [special_target2], [segue1_target1], [segue1_target2], [segue2_target1], [segue2_target2], [boss_target1], [eboss_target0], [ancillary_target1], [ancillary_target2], [primTarget], [secTarget], [spectrographID], [sourceType], [targetType], [ra], [dec], [cx], [cy], [cz], [xFocal], [yFocal], [lambdaEff], [blueFiber], [zOffset], [z], [zErr], [zWarning], [class], [subClass], [rChi2], [DOF], [rChi2Diff], [z_noqso], [zErr_noqso], [zWarning_noqso], [class_noqso], [subClass_noqso], [rChi2Diff_noqso], [z_person], [class_person], [comments_person], [tFile], [tColumn_0], [tColumn_1], [tColumn_2], [tColumn_3], [tColumn_4], [tColumn_5], [tColumn_6], [tColumn_7], [tColumn_8], [tColumn_9], [nPoly], [theta_0], [theta_1], [theta_2], [theta_3], [theta_4], [theta_5], [theta_6], [theta_7], [theta_8], [theta_9], [velDisp], [velDispErr], [velDispZ], [velDispZErr], [velDispChi2], [velDispNPix], [velDispDOF], [waveMin], [waveMax], [wCoverage], [snMedian_u], [snMedian_g], [snMedian_r], [snMedian_i], [snMedian_z], [snMedian], [chi68p], [fracNSigma_1], [fracNSigma_2], [fracNSigma_3], [fracNSigma_4], [fracNSigma_5], [fracNSigma_6], [fracNSigma_7], [fracNSigma_8], [fracNSigma_9], [fracNSigma_10], [fracNSigHi_1], [fracNSigHi_2], [fracNSigHi_3], [fracNSigHi_4], [fracNSigHi_5], [fracNSigHi_6], [fracNSigHi_7], [fracNSigHi_8], [fracNSigHi_9], [fracNSigHi_10], [fracNSigLo_1], [fracNSigLo_2], [fracNSigLo_3], [fracNSigLo_4], [fracNSigLo_5], [fracNSigLo_6], [fracNSigLo_7], [fracNSigLo_8], [fracNSigLo_9], [fracNSigLo_10], [spectroFlux_u], [spectroFlux_g], [spectroFlux_r], [spectroFlux_i], [spectroFlux_z], [spectroSynFlux_u], [spectroSynFlux_g], [spectroSynFlux_r], [spectroSynFlux_i], [spectroSynFlux_z], [spectroFluxIvar_u], [spectroFluxIvar_g], [spectroFluxIvar_r], [spectroFluxIvar_i], [spectroFluxIvar_z], [spectroSynFluxIvar_u], [spectroSynFluxIvar_g], [spectroSynFluxIvar_r], [spectroSynFluxIvar_i], [spectroSynFluxIvar_z], [spectroSkyFlux_u], [spectroSkyFlux_g], [spectroSkyFlux_r], [spectroSkyFlux_i], [spectroSkyFlux_z], [anyAndMask], [anyOrMask], [plateSN2], [deredSN2], [snTurnoff], [sn1_g], [sn1_r], [sn1_i], [sn2_g], [sn2_r], [sn2_i], [elodieFileName], [elodieObject], [elodieSpType], [elodieBV], [elodieTEff], [elodieLogG], [elodieFeH], [elodieZ], [elodieZErr], [elodieZModelErr], [elodieRChi2], [elodieDOF], [htmID], [loadVersion])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[bestObjID], sourcetablealias.[fluxObjID], sourcetablealias.[targetObjID], sourcetablealias.[plateID], sourcetablealias.[sciencePrimary], sourcetablealias.[legacyPrimary], sourcetablealias.[seguePrimary], sourcetablealias.[segue1Primary], sourcetablealias.[segue2Primary], sourcetablealias.[bossPrimary], sourcetablealias.[sdssPrimary], sourcetablealias.[bossSpecObjID], sourcetablealias.[firstRelease], sourcetablealias.[survey], sourcetablealias.[instrument], sourcetablealias.[programname], sourcetablealias.[chunk], sourcetablealias.[platerun], sourcetablealias.[mjd], sourcetablealias.[plate], sourcetablealias.[fiberID], sourcetablealias.[run1d], sourcetablealias.[run2d], sourcetablealias.[tile], sourcetablealias.[designID], sourcetablealias.[legacy_target1], sourcetablealias.[legacy_target2], sourcetablealias.[special_target1], sourcetablealias.[special_target2], sourcetablealias.[segue1_target1], sourcetablealias.[segue1_target2], sourcetablealias.[segue2_target1], sourcetablealias.[segue2_target2], sourcetablealias.[boss_target1], sourcetablealias.[eboss_target0], sourcetablealias.[ancillary_target1], sourcetablealias.[ancillary_target2], sourcetablealias.[primTarget], sourcetablealias.[secTarget], sourcetablealias.[spectrographID], sourcetablealias.[sourceType], sourcetablealias.[targetType], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[xFocal], sourcetablealias.[yFocal], sourcetablealias.[lambdaEff], sourcetablealias.[blueFiber], sourcetablealias.[zOffset], sourcetablealias.[z], sourcetablealias.[zErr], sourcetablealias.[zWarning], sourcetablealias.[class], sourcetablealias.[subClass], sourcetablealias.[rChi2], sourcetablealias.[DOF], sourcetablealias.[rChi2Diff], sourcetablealias.[z_noqso], sourcetablealias.[zErr_noqso], sourcetablealias.[zWarning_noqso], sourcetablealias.[class_noqso], sourcetablealias.[subClass_noqso], sourcetablealias.[rChi2Diff_noqso], sourcetablealias.[z_person], sourcetablealias.[class_person], sourcetablealias.[comments_person], sourcetablealias.[tFile], sourcetablealias.[tColumn_0], sourcetablealias.[tColumn_1], sourcetablealias.[tColumn_2], sourcetablealias.[tColumn_3], sourcetablealias.[tColumn_4], sourcetablealias.[tColumn_5], sourcetablealias.[tColumn_6], sourcetablealias.[tColumn_7], sourcetablealias.[tColumn_8], sourcetablealias.[tColumn_9], sourcetablealias.[nPoly], sourcetablealias.[theta_0], sourcetablealias.[theta_1], sourcetablealias.[theta_2], sourcetablealias.[theta_3], sourcetablealias.[theta_4], sourcetablealias.[theta_5], sourcetablealias.[theta_6], sourcetablealias.[theta_7], sourcetablealias.[theta_8], sourcetablealias.[theta_9], sourcetablealias.[velDisp], sourcetablealias.[velDispErr], sourcetablealias.[velDispZ], sourcetablealias.[velDispZErr], sourcetablealias.[velDispChi2], sourcetablealias.[velDispNPix], sourcetablealias.[velDispDOF], sourcetablealias.[waveMin], sourcetablealias.[waveMax], sourcetablealias.[wCoverage], sourcetablealias.[snMedian_u], sourcetablealias.[snMedian_g], sourcetablealias.[snMedian_r], sourcetablealias.[snMedian_i], sourcetablealias.[snMedian_z], sourcetablealias.[snMedian], sourcetablealias.[chi68p], sourcetablealias.[fracNSigma_1], sourcetablealias.[fracNSigma_2], sourcetablealias.[fracNSigma_3], sourcetablealias.[fracNSigma_4], sourcetablealias.[fracNSigma_5], sourcetablealias.[fracNSigma_6], sourcetablealias.[fracNSigma_7], sourcetablealias.[fracNSigma_8], sourcetablealias.[fracNSigma_9], sourcetablealias.[fracNSigma_10], sourcetablealias.[fracNSigHi_1], sourcetablealias.[fracNSigHi_2], sourcetablealias.[fracNSigHi_3], sourcetablealias.[fracNSigHi_4], sourcetablealias.[fracNSigHi_5], sourcetablealias.[fracNSigHi_6], sourcetablealias.[fracNSigHi_7], sourcetablealias.[fracNSigHi_8], sourcetablealias.[fracNSigHi_9], sourcetablealias.[fracNSigHi_10], sourcetablealias.[fracNSigLo_1], sourcetablealias.[fracNSigLo_2], sourcetablealias.[fracNSigLo_3], sourcetablealias.[fracNSigLo_4], sourcetablealias.[fracNSigLo_5], sourcetablealias.[fracNSigLo_6], sourcetablealias.[fracNSigLo_7], sourcetablealias.[fracNSigLo_8], sourcetablealias.[fracNSigLo_9], sourcetablealias.[fracNSigLo_10], sourcetablealias.[spectroFlux_u], sourcetablealias.[spectroFlux_g], sourcetablealias.[spectroFlux_r], sourcetablealias.[spectroFlux_i], sourcetablealias.[spectroFlux_z], sourcetablealias.[spectroSynFlux_u], sourcetablealias.[spectroSynFlux_g], sourcetablealias.[spectroSynFlux_r], sourcetablealias.[spectroSynFlux_i], sourcetablealias.[spectroSynFlux_z], sourcetablealias.[spectroFluxIvar_u], sourcetablealias.[spectroFluxIvar_g], sourcetablealias.[spectroFluxIvar_r], sourcetablealias.[spectroFluxIvar_i], sourcetablealias.[spectroFluxIvar_z], sourcetablealias.[spectroSynFluxIvar_u], sourcetablealias.[spectroSynFluxIvar_g], sourcetablealias.[spectroSynFluxIvar_r], sourcetablealias.[spectroSynFluxIvar_i], sourcetablealias.[spectroSynFluxIvar_z], sourcetablealias.[spectroSkyFlux_u], sourcetablealias.[spectroSkyFlux_g], sourcetablealias.[spectroSkyFlux_r], sourcetablealias.[spectroSkyFlux_i], sourcetablealias.[spectroSkyFlux_z], sourcetablealias.[anyAndMask], sourcetablealias.[anyOrMask], sourcetablealias.[plateSN2], sourcetablealias.[deredSN2], sourcetablealias.[snTurnoff], sourcetablealias.[sn1_g], sourcetablealias.[sn1_r], sourcetablealias.[sn1_i], sourcetablealias.[sn2_g], sourcetablealias.[sn2_r], sourcetablealias.[sn2_i], sourcetablealias.[elodieFileName], sourcetablealias.[elodieObject], sourcetablealias.[elodieSpType], sourcetablealias.[elodieBV], sourcetablealias.[elodieTEff], sourcetablealias.[elodieLogG], sourcetablealias.[elodieFeH], sourcetablealias.[elodieZ], sourcetablealias.[elodieZErr], sourcetablealias.[elodieZModelErr], sourcetablealias.[elodieRChi2], sourcetablealias.[elodieDOF], sourcetablealias.[htmID], sourcetablealias.[loadVersion]
 FROM   [SkyNode_SDSSDR12].[dbo].[SpecObjAll] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'SpecPhotoAll' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[SpecPhotoAll] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[SpecPhotoAll] WITH (TABLOCKX)
	([specObjID], [mjd], [plate], [tile], [fiberID], [z], [zErr], [class], [subClass], [zWarning], [ra], [dec], [cx], [cy], [cz], [htmID], [sciencePrimary], [legacyPrimary], [seguePrimary], [segue1Primary], [segue2Primary], [bossPrimary], [sdssPrimary], [survey], [programname], [legacy_target1], [legacy_target2], [special_target1], [special_target2], [segue1_target1], [segue1_target2], [segue2_target1], [segue2_target2], [boss_target1], [ancillary_target1], [ancillary_target2], [plateID], [sourceType], [targetObjID], [objID], [skyVersion], [run], [rerun], [camcol], [field], [obj], [mode], [nChild], [type], [flags], [psfMag_u], [psfMag_g], [psfMag_r], [psfMag_i], [psfMag_z], [psfMagErr_u], [psfMagErr_g], [psfMagErr_r], [psfMagErr_i], [psfMagErr_z], [fiberMag_u], [fiberMag_g], [fiberMag_r], [fiberMag_i], [fiberMag_z], [fiberMagErr_u], [fiberMagErr_g], [fiberMagErr_r], [fiberMagErr_i], [fiberMagErr_z], [petroMag_u], [petroMag_g], [petroMag_r], [petroMag_i], [petroMag_z], [petroMagErr_u], [petroMagErr_g], [petroMagErr_r], [petroMagErr_i], [petroMagErr_z], [modelMag_u], [modelMag_g], [modelMag_r], [modelMag_i], [modelMag_z], [modelMagErr_u], [modelMagErr_g], [modelMagErr_r], [modelMagErr_i], [modelMagErr_z], [cModelMag_u], [cModelMag_g], [cModelMag_r], [cModelMag_i], [cModelMag_z], [cModelMagErr_u], [cModelMagErr_g], [cModelMagErr_r], [cModelMagErr_i], [cModelMagErr_z], [mRrCc_r], [mRrCcErr_r], [score], [resolveStatus], [calibStatus_u], [calibStatus_g], [calibStatus_r], [calibStatus_i], [calibStatus_z], [photoRa], [photoDec], [extinction_u], [extinction_g], [extinction_r], [extinction_i], [extinction_z], [fieldID], [dered_u], [dered_g], [dered_r], [dered_i], [dered_z])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[mjd], sourcetablealias.[plate], sourcetablealias.[tile], sourcetablealias.[fiberID], sourcetablealias.[z], sourcetablealias.[zErr], sourcetablealias.[class], sourcetablealias.[subClass], sourcetablealias.[zWarning], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[htmID], sourcetablealias.[sciencePrimary], sourcetablealias.[legacyPrimary], sourcetablealias.[seguePrimary], sourcetablealias.[segue1Primary], sourcetablealias.[segue2Primary], sourcetablealias.[bossPrimary], sourcetablealias.[sdssPrimary], sourcetablealias.[survey], sourcetablealias.[programname], sourcetablealias.[legacy_target1], sourcetablealias.[legacy_target2], sourcetablealias.[special_target1], sourcetablealias.[special_target2], sourcetablealias.[segue1_target1], sourcetablealias.[segue1_target2], sourcetablealias.[segue2_target1], sourcetablealias.[segue2_target2], sourcetablealias.[boss_target1], sourcetablealias.[ancillary_target1], sourcetablealias.[ancillary_target2], sourcetablealias.[plateID], sourcetablealias.[sourceType], sourcetablealias.[targetObjID], sourcetablealias.[objID], sourcetablealias.[skyVersion], sourcetablealias.[run], sourcetablealias.[rerun], sourcetablealias.[camcol], sourcetablealias.[field], sourcetablealias.[obj], sourcetablealias.[mode], sourcetablealias.[nChild], sourcetablealias.[type], sourcetablealias.[flags], sourcetablealias.[psfMag_u], sourcetablealias.[psfMag_g], sourcetablealias.[psfMag_r], sourcetablealias.[psfMag_i], sourcetablealias.[psfMag_z], sourcetablealias.[psfMagErr_u], sourcetablealias.[psfMagErr_g], sourcetablealias.[psfMagErr_r], sourcetablealias.[psfMagErr_i], sourcetablealias.[psfMagErr_z], sourcetablealias.[fiberMag_u], sourcetablealias.[fiberMag_g], sourcetablealias.[fiberMag_r], sourcetablealias.[fiberMag_i], sourcetablealias.[fiberMag_z], sourcetablealias.[fiberMagErr_u], sourcetablealias.[fiberMagErr_g], sourcetablealias.[fiberMagErr_r], sourcetablealias.[fiberMagErr_i], sourcetablealias.[fiberMagErr_z], sourcetablealias.[petroMag_u], sourcetablealias.[petroMag_g], sourcetablealias.[petroMag_r], sourcetablealias.[petroMag_i], sourcetablealias.[petroMag_z], sourcetablealias.[petroMagErr_u], sourcetablealias.[petroMagErr_g], sourcetablealias.[petroMagErr_r], sourcetablealias.[petroMagErr_i], sourcetablealias.[petroMagErr_z], sourcetablealias.[modelMag_u], sourcetablealias.[modelMag_g], sourcetablealias.[modelMag_r], sourcetablealias.[modelMag_i], sourcetablealias.[modelMag_z], sourcetablealias.[modelMagErr_u], sourcetablealias.[modelMagErr_g], sourcetablealias.[modelMagErr_r], sourcetablealias.[modelMagErr_i], sourcetablealias.[modelMagErr_z], sourcetablealias.[cModelMag_u], sourcetablealias.[cModelMag_g], sourcetablealias.[cModelMag_r], sourcetablealias.[cModelMag_i], sourcetablealias.[cModelMag_z], sourcetablealias.[cModelMagErr_u], sourcetablealias.[cModelMagErr_g], sourcetablealias.[cModelMagErr_r], sourcetablealias.[cModelMagErr_i], sourcetablealias.[cModelMagErr_z], sourcetablealias.[mRrCc_r], sourcetablealias.[mRrCcErr_r], sourcetablealias.[score], sourcetablealias.[resolveStatus], sourcetablealias.[calibStatus_u], sourcetablealias.[calibStatus_g], sourcetablealias.[calibStatus_r], sourcetablealias.[calibStatus_i], sourcetablealias.[calibStatus_z], sourcetablealias.[photoRa], sourcetablealias.[photoDec], sourcetablealias.[extinction_u], sourcetablealias.[extinction_g], sourcetablealias.[extinction_r], sourcetablealias.[extinction_i], sourcetablealias.[extinction_z], sourcetablealias.[fieldID], sourcetablealias.[dered_u], sourcetablealias.[dered_g], sourcetablealias.[dered_r], sourcetablealias.[dered_i], sourcetablealias.[dered_z]
 FROM   [SkyNode_SDSSDR12].[dbo].[SpecPhotoAll] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'sppLines' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[SPECOBJID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[SPECOBJID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[sppLines] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [SPECOBJID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[sppLines] WITH (TABLOCKX)
	([SPECOBJID], [bestObjID], [TARGETOBJID], [sciencePrimary], [legacyPrimary], [seguePrimary], [PLATE], [MJD], [FIBER], [RUN2D], [RUN1D], [RUNSSPP], [H83side], [H83cont], [H83err], [H83mask], [H812side], [H812cont], [H812err], [H812mask], [H824side], [H824cont], [H824err], [H824mask], [H848side], [H848cont], [H848err], [H848mask], [KP12side], [KP12cont], [KP12err], [KP12mask], [KP18side], [KP18cont], [KP18err], [KP18mask], [KP6side], [KP6cont], [KP6err], [KP6mask], [CaIIKside], [CaIIKcont], [CaIIKerr], [CaIIKmask], [CaIIHKside], [CaIIHKcont], [CaIIHKerr], [CaIIHKmask], [Hepsside], [Hepscont], [Hepserr], [Hepsmask], [KP16side], [KP16cont], [KP16err], [KP16mask], [SrII1side], [SrII1cont], [SrII1err], [SrII1mask], [HeI121side], [HeI121cont], [HeI121err], [HeI121mask], [Hdelta12side], [Hdelta12cont], [Hdelta12err], [Hdelta12mask], [Hdelta24side], [Hdelta24cont], [Hdelta24err], [Hdelta24mask], [Hdelta48side], [Hdelta48cont], [Hdelta48err], [Hdelta48mask], [Hdeltaside], [Hdeltacont], [Hdeltaerr], [Hdeltamask], [CaI4side], [CaI4cont], [CaI4err], [CaI4mask], [CaI12side], [CaI12cont], [CaI12err], [CaI12mask], [CaI24side], [CaI24cont], [CaI24err], [CaI24mask], [CaI6side], [CaI6cont], [CaI6err], [CaI6mask], [Gside], [Gcont], [Gerr], [Gmask], [Hgamma12side], [Hgamma12cont], [Hgamma12err], [Hgamma12mask], [Hgamma24side], [Hgamma24cont], [Hgamma24err], [Hgamma24mask], [Hgamma48side], [Hgamma48cont], [Hgamma48err], [Hgamma48mask], [Hgammaside], [Hgammacont], [Hgammaerr], [Hgammamask], [HeI122side], [HeI122cont], [HeI122err], [HeI122mask], [Gblueside], [Gbluecont], [Gblueerr], [Gbluemask], [Gwholeside], [Gwholecont], [Gwholeerr], [Gwholemask], [Baside], [Bacont], [Baerr], [Bamask], [C12C13side], [C12C13cont], [C12C13err], [C12C13mask], [CC12side], [CC12cont], [CC12err], [CC12mask], [metal1side], [metal1cont], [metal1err], [metal1mask], [Hbeta12side], [Hbeta12cont], [Hbeta12err], [Hbeta12mask], [Hbeta24side], [Hbeta24cont], [Hbeta24err], [Hbeta24mask], [Hbeta48side], [Hbeta48cont], [Hbeta48err], [Hbeta48mask], [Hbetaside], [Hbetacont], [Hbetaerr], [Hbetamask], [C2side], [C2cont], [C2err], [C2mask], [C2MgIside], [C2MgIcont], [C2MgIerr], [C2MgImask], [MgHMgIC2side], [MgHMgIC2cont], [MgHMgIC2err], [MgHMgIC2mask], [MgHMgIside], [MgHMgIcont], [MgHMgIerr], [MgHMgImask], [MgHside], [MgHcont], [MgHerr], [MgHmask], [CrIside], [CrIcont], [CrIerr], [CrImask], [MgIFeIIside], [MgIFeIIcont], [MgIFeIIerr], [MgIFeIImask], [MgI2side], [MgI2cont], [MgI2err], [MgI2mask], [MgI121side], [MgI121cont], [MgI121err], [MgI121mask], [MgI24side], [MgI24cont], [MgI24err], [MgI24mask], [MgI122side], [MgI122cont], [MgI122err], [MgI122mask], [NaI20side], [NaI20cont], [NaI20err], [NaI20mask], [Na12side], [Na12cont], [Na12err], [Na12mask], [Na24side], [Na24cont], [Na24err], [Na24mask], [Halpha12side], [Halpha12cont], [Halpha12err], [Halpha12mask], [Halpha24side], [Halpha24cont], [Halpha24err], [Halpha24mask], [Halpha48side], [Halpha48cont], [Halpha48err], [Halpha48mask], [Halpha70side], [Halpha70cont], [Halpha70err], [Halpha70mask], [CaHside], [CaHcont], [CaHerr], [CaHmask], [TiOside], [TiOcont], [TiOerr], [TiOmask], [CNside], [CNcont], [CNerr], [CNmask], [OItripside], [OItripcont], [OItriperr], [OItripmask], [KI34side], [KI34cont], [KI34err], [KI34mask], [KI95side], [KI95cont], [KI95err], [KI95mask], [NaI15side], [NaI15cont], [NaI15err], [NaI15mask], [NaIredside], [NaIredcont], [NaIrederr], [NaIredmask], [CaII26side], [CaII26cont], [CaII26err], [CaII26mask], [Paschen13side], [Paschen13cont], [Paschen13err], [Paschen13mask], [CaII29side], [CaII29cont], [CaII29err], [CaII29mask], [CaII401side], [CaII401cont], [CaII401err], [CaII401mask], [CaII161side], [CaII161cont], [CaII161err], [CaII161mask], [Paschen421side], [Paschen421cont], [Paschen421err], [Paschen421mask], [CaII162side], [CaII162cont], [CaII162err], [CaII162mask], [CaII402side], [CaII402cont], [CaII402err], [CaII402mask], [Paschen422side], [Paschen422cont], [Paschen422err], [Paschen422mask], [TiO5side], [TiO5cont], [TiO5err], [TiO5mask], [TiO8side], [TiO8cont], [TiO8err], [TiO8mask], [CaH1side], [CaH1cont], [CaH1err], [CaH1mask], [CaH2side], [CaH2cont], [CaH2err], [CaH2mask], [CaH3side], [CaH3cont], [CaH3err], [CaH3mask], [UVCNside], [UVCNcont], [UVCNerr], [UVCNmask], [BLCNside], [BLCNcont], [BLCNerr], [BLCNmask], [FEI1side], [FEI1cont], [FEI1err], [FEI1mask], [FEI2side], [FEI2cont], [FEI2err], [FEI2mask], [FEI3side], [FEI3cont], [FEI3err], [FEI3mask], [SRII2side], [SRII2cont], [SRII2err], [SRII2mask], [FEI4side], [FEI4cont], [FEI4err], [FEI4mask], [FEI5side], [FEI5cont], [FEI5err], [FEI5mask])
 SELECT sourcetablealias.[SPECOBJID], sourcetablealias.[bestObjID], sourcetablealias.[TARGETOBJID], sourcetablealias.[sciencePrimary], sourcetablealias.[legacyPrimary], sourcetablealias.[seguePrimary], sourcetablealias.[PLATE], sourcetablealias.[MJD], sourcetablealias.[FIBER], sourcetablealias.[RUN2D], sourcetablealias.[RUN1D], sourcetablealias.[RUNSSPP], sourcetablealias.[H83side], sourcetablealias.[H83cont], sourcetablealias.[H83err], sourcetablealias.[H83mask], sourcetablealias.[H812side], sourcetablealias.[H812cont], sourcetablealias.[H812err], sourcetablealias.[H812mask], sourcetablealias.[H824side], sourcetablealias.[H824cont], sourcetablealias.[H824err], sourcetablealias.[H824mask], sourcetablealias.[H848side], sourcetablealias.[H848cont], sourcetablealias.[H848err], sourcetablealias.[H848mask], sourcetablealias.[KP12side], sourcetablealias.[KP12cont], sourcetablealias.[KP12err], sourcetablealias.[KP12mask], sourcetablealias.[KP18side], sourcetablealias.[KP18cont], sourcetablealias.[KP18err], sourcetablealias.[KP18mask], sourcetablealias.[KP6side], sourcetablealias.[KP6cont], sourcetablealias.[KP6err], sourcetablealias.[KP6mask], sourcetablealias.[CaIIKside], sourcetablealias.[CaIIKcont], sourcetablealias.[CaIIKerr], sourcetablealias.[CaIIKmask], sourcetablealias.[CaIIHKside], sourcetablealias.[CaIIHKcont], sourcetablealias.[CaIIHKerr], sourcetablealias.[CaIIHKmask], sourcetablealias.[Hepsside], sourcetablealias.[Hepscont], sourcetablealias.[Hepserr], sourcetablealias.[Hepsmask], sourcetablealias.[KP16side], sourcetablealias.[KP16cont], sourcetablealias.[KP16err], sourcetablealias.[KP16mask], sourcetablealias.[SrII1side], sourcetablealias.[SrII1cont], sourcetablealias.[SrII1err], sourcetablealias.[SrII1mask], sourcetablealias.[HeI121side], sourcetablealias.[HeI121cont], sourcetablealias.[HeI121err], sourcetablealias.[HeI121mask], sourcetablealias.[Hdelta12side], sourcetablealias.[Hdelta12cont], sourcetablealias.[Hdelta12err], sourcetablealias.[Hdelta12mask], sourcetablealias.[Hdelta24side], sourcetablealias.[Hdelta24cont], sourcetablealias.[Hdelta24err], sourcetablealias.[Hdelta24mask], sourcetablealias.[Hdelta48side], sourcetablealias.[Hdelta48cont], sourcetablealias.[Hdelta48err], sourcetablealias.[Hdelta48mask], sourcetablealias.[Hdeltaside], sourcetablealias.[Hdeltacont], sourcetablealias.[Hdeltaerr], sourcetablealias.[Hdeltamask], sourcetablealias.[CaI4side], sourcetablealias.[CaI4cont], sourcetablealias.[CaI4err], sourcetablealias.[CaI4mask], sourcetablealias.[CaI12side], sourcetablealias.[CaI12cont], sourcetablealias.[CaI12err], sourcetablealias.[CaI12mask], sourcetablealias.[CaI24side], sourcetablealias.[CaI24cont], sourcetablealias.[CaI24err], sourcetablealias.[CaI24mask], sourcetablealias.[CaI6side], sourcetablealias.[CaI6cont], sourcetablealias.[CaI6err], sourcetablealias.[CaI6mask], sourcetablealias.[Gside], sourcetablealias.[Gcont], sourcetablealias.[Gerr], sourcetablealias.[Gmask], sourcetablealias.[Hgamma12side], sourcetablealias.[Hgamma12cont], sourcetablealias.[Hgamma12err], sourcetablealias.[Hgamma12mask], sourcetablealias.[Hgamma24side], sourcetablealias.[Hgamma24cont], sourcetablealias.[Hgamma24err], sourcetablealias.[Hgamma24mask], sourcetablealias.[Hgamma48side], sourcetablealias.[Hgamma48cont], sourcetablealias.[Hgamma48err], sourcetablealias.[Hgamma48mask], sourcetablealias.[Hgammaside], sourcetablealias.[Hgammacont], sourcetablealias.[Hgammaerr], sourcetablealias.[Hgammamask], sourcetablealias.[HeI122side], sourcetablealias.[HeI122cont], sourcetablealias.[HeI122err], sourcetablealias.[HeI122mask], sourcetablealias.[Gblueside], sourcetablealias.[Gbluecont], sourcetablealias.[Gblueerr], sourcetablealias.[Gbluemask], sourcetablealias.[Gwholeside], sourcetablealias.[Gwholecont], sourcetablealias.[Gwholeerr], sourcetablealias.[Gwholemask], sourcetablealias.[Baside], sourcetablealias.[Bacont], sourcetablealias.[Baerr], sourcetablealias.[Bamask], sourcetablealias.[C12C13side], sourcetablealias.[C12C13cont], sourcetablealias.[C12C13err], sourcetablealias.[C12C13mask], sourcetablealias.[CC12side], sourcetablealias.[CC12cont], sourcetablealias.[CC12err], sourcetablealias.[CC12mask], sourcetablealias.[metal1side], sourcetablealias.[metal1cont], sourcetablealias.[metal1err], sourcetablealias.[metal1mask], sourcetablealias.[Hbeta12side], sourcetablealias.[Hbeta12cont], sourcetablealias.[Hbeta12err], sourcetablealias.[Hbeta12mask], sourcetablealias.[Hbeta24side], sourcetablealias.[Hbeta24cont], sourcetablealias.[Hbeta24err], sourcetablealias.[Hbeta24mask], sourcetablealias.[Hbeta48side], sourcetablealias.[Hbeta48cont], sourcetablealias.[Hbeta48err], sourcetablealias.[Hbeta48mask], sourcetablealias.[Hbetaside], sourcetablealias.[Hbetacont], sourcetablealias.[Hbetaerr], sourcetablealias.[Hbetamask], sourcetablealias.[C2side], sourcetablealias.[C2cont], sourcetablealias.[C2err], sourcetablealias.[C2mask], sourcetablealias.[C2MgIside], sourcetablealias.[C2MgIcont], sourcetablealias.[C2MgIerr], sourcetablealias.[C2MgImask], sourcetablealias.[MgHMgIC2side], sourcetablealias.[MgHMgIC2cont], sourcetablealias.[MgHMgIC2err], sourcetablealias.[MgHMgIC2mask], sourcetablealias.[MgHMgIside], sourcetablealias.[MgHMgIcont], sourcetablealias.[MgHMgIerr], sourcetablealias.[MgHMgImask], sourcetablealias.[MgHside], sourcetablealias.[MgHcont], sourcetablealias.[MgHerr], sourcetablealias.[MgHmask], sourcetablealias.[CrIside], sourcetablealias.[CrIcont], sourcetablealias.[CrIerr], sourcetablealias.[CrImask], sourcetablealias.[MgIFeIIside], sourcetablealias.[MgIFeIIcont], sourcetablealias.[MgIFeIIerr], sourcetablealias.[MgIFeIImask], sourcetablealias.[MgI2side], sourcetablealias.[MgI2cont], sourcetablealias.[MgI2err], sourcetablealias.[MgI2mask], sourcetablealias.[MgI121side], sourcetablealias.[MgI121cont], sourcetablealias.[MgI121err], sourcetablealias.[MgI121mask], sourcetablealias.[MgI24side], sourcetablealias.[MgI24cont], sourcetablealias.[MgI24err], sourcetablealias.[MgI24mask], sourcetablealias.[MgI122side], sourcetablealias.[MgI122cont], sourcetablealias.[MgI122err], sourcetablealias.[MgI122mask], sourcetablealias.[NaI20side], sourcetablealias.[NaI20cont], sourcetablealias.[NaI20err], sourcetablealias.[NaI20mask], sourcetablealias.[Na12side], sourcetablealias.[Na12cont], sourcetablealias.[Na12err], sourcetablealias.[Na12mask], sourcetablealias.[Na24side], sourcetablealias.[Na24cont], sourcetablealias.[Na24err], sourcetablealias.[Na24mask], sourcetablealias.[Halpha12side], sourcetablealias.[Halpha12cont], sourcetablealias.[Halpha12err], sourcetablealias.[Halpha12mask], sourcetablealias.[Halpha24side], sourcetablealias.[Halpha24cont], sourcetablealias.[Halpha24err], sourcetablealias.[Halpha24mask], sourcetablealias.[Halpha48side], sourcetablealias.[Halpha48cont], sourcetablealias.[Halpha48err], sourcetablealias.[Halpha48mask], sourcetablealias.[Halpha70side], sourcetablealias.[Halpha70cont], sourcetablealias.[Halpha70err], sourcetablealias.[Halpha70mask], sourcetablealias.[CaHside], sourcetablealias.[CaHcont], sourcetablealias.[CaHerr], sourcetablealias.[CaHmask], sourcetablealias.[TiOside], sourcetablealias.[TiOcont], sourcetablealias.[TiOerr], sourcetablealias.[TiOmask], sourcetablealias.[CNside], sourcetablealias.[CNcont], sourcetablealias.[CNerr], sourcetablealias.[CNmask], sourcetablealias.[OItripside], sourcetablealias.[OItripcont], sourcetablealias.[OItriperr], sourcetablealias.[OItripmask], sourcetablealias.[KI34side], sourcetablealias.[KI34cont], sourcetablealias.[KI34err], sourcetablealias.[KI34mask], sourcetablealias.[KI95side], sourcetablealias.[KI95cont], sourcetablealias.[KI95err], sourcetablealias.[KI95mask], sourcetablealias.[NaI15side], sourcetablealias.[NaI15cont], sourcetablealias.[NaI15err], sourcetablealias.[NaI15mask], sourcetablealias.[NaIredside], sourcetablealias.[NaIredcont], sourcetablealias.[NaIrederr], sourcetablealias.[NaIredmask], sourcetablealias.[CaII26side], sourcetablealias.[CaII26cont], sourcetablealias.[CaII26err], sourcetablealias.[CaII26mask], sourcetablealias.[Paschen13side], sourcetablealias.[Paschen13cont], sourcetablealias.[Paschen13err], sourcetablealias.[Paschen13mask], sourcetablealias.[CaII29side], sourcetablealias.[CaII29cont], sourcetablealias.[CaII29err], sourcetablealias.[CaII29mask], sourcetablealias.[CaII401side], sourcetablealias.[CaII401cont], sourcetablealias.[CaII401err], sourcetablealias.[CaII401mask], sourcetablealias.[CaII161side], sourcetablealias.[CaII161cont], sourcetablealias.[CaII161err], sourcetablealias.[CaII161mask], sourcetablealias.[Paschen421side], sourcetablealias.[Paschen421cont], sourcetablealias.[Paschen421err], sourcetablealias.[Paschen421mask], sourcetablealias.[CaII162side], sourcetablealias.[CaII162cont], sourcetablealias.[CaII162err], sourcetablealias.[CaII162mask], sourcetablealias.[CaII402side], sourcetablealias.[CaII402cont], sourcetablealias.[CaII402err], sourcetablealias.[CaII402mask], sourcetablealias.[Paschen422side], sourcetablealias.[Paschen422cont], sourcetablealias.[Paschen422err], sourcetablealias.[Paschen422mask], sourcetablealias.[TiO5side], sourcetablealias.[TiO5cont], sourcetablealias.[TiO5err], sourcetablealias.[TiO5mask], sourcetablealias.[TiO8side], sourcetablealias.[TiO8cont], sourcetablealias.[TiO8err], sourcetablealias.[TiO8mask], sourcetablealias.[CaH1side], sourcetablealias.[CaH1cont], sourcetablealias.[CaH1err], sourcetablealias.[CaH1mask], sourcetablealias.[CaH2side], sourcetablealias.[CaH2cont], sourcetablealias.[CaH2err], sourcetablealias.[CaH2mask], sourcetablealias.[CaH3side], sourcetablealias.[CaH3cont], sourcetablealias.[CaH3err], sourcetablealias.[CaH3mask], sourcetablealias.[UVCNside], sourcetablealias.[UVCNcont], sourcetablealias.[UVCNerr], sourcetablealias.[UVCNmask], sourcetablealias.[BLCNside], sourcetablealias.[BLCNcont], sourcetablealias.[BLCNerr], sourcetablealias.[BLCNmask], sourcetablealias.[FEI1side], sourcetablealias.[FEI1cont], sourcetablealias.[FEI1err], sourcetablealias.[FEI1mask], sourcetablealias.[FEI2side], sourcetablealias.[FEI2cont], sourcetablealias.[FEI2err], sourcetablealias.[FEI2mask], sourcetablealias.[FEI3side], sourcetablealias.[FEI3cont], sourcetablealias.[FEI3err], sourcetablealias.[FEI3mask], sourcetablealias.[SRII2side], sourcetablealias.[SRII2cont], sourcetablealias.[SRII2err], sourcetablealias.[SRII2mask], sourcetablealias.[FEI4side], sourcetablealias.[FEI4cont], sourcetablealias.[FEI4err], sourcetablealias.[FEI4mask], sourcetablealias.[FEI5side], sourcetablealias.[FEI5cont], sourcetablealias.[FEI5err], sourcetablealias.[FEI5mask]
 FROM   [SkyNode_SDSSDR12].[dbo].[sppLines] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.SPECOBJID = sourcetablealias.SPECOBJID
	;


GO

-- SUBSAMPLING TABLE 'sppParams' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[SPECOBJID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[SPECOBJID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[sppParams] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [SPECOBJID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[sppParams] WITH (TABLOCKX)
	([SPECOBJID], [BESTOBJID], [FLUXOBJID], [TARGETOBJID], [PLATEID], [sciencePrimary], [legacyPrimary], [seguePrimary], [FIRSTRELEASE], [SURVEY], [PROGRAMNAME], [CHUNK], [PLATERUN], [MJD], [PLATE], [FIBERID], [RUN2D], [RUN1D], [RUNSSPP], [TARGETSTRING], [PRIMTARGET], [SECTARGET], [LEGACY_TARGET1], [LEGACY_TARGET2], [SPECIAL_TARGET1], [SPECIAL_TARGET2], [SEGUE1_TARGET1], [SEGUE1_TARGET2], [SEGUE2_TARGET1], [SEGUE2_TARGET2], [SPECTYPEHAMMER], [SPECTYPESUBCLASS], [FLAG], [TEFFADOP], [TEFFADOPN], [TEFFADOPUNC], [TEFFHA24], [TEFFHD24], [TEFFNGS1], [TEFFANNSR], [TEFFANNRR], [TEFFWBG], [TEFFK24], [TEFFKI13], [TEFFHA24IND], [TEFFHD24IND], [TEFFNGS1IND], [TEFFANNSRIND], [TEFFANNRRIND], [TEFFWBGIND], [TEFFK24IND], [TEFFKI13IND], [TEFFHA24UNC], [TEFFHD24UNC], [TEFFNGS1UNC], [TEFFANNSRUNC], [TEFFANNRRUNC], [TEFFWBGUNC], [TEFFK24UNC], [TEFFKI13UNC], [LOGGADOP], [LOGGADOPN], [LOGGADOPUNC], [LOGGNGS2], [LOGGNGS1], [LOGGANNSR], [LOGGANNRR], [LOGGCAI1], [LOGGWBG], [LOGGK24], [LOGGKI13], [LOGGNGS2IND], [LOGGNGS1IND], [LOGGANNSRIND], [LOGGANNRRIND], [LOGGCAI1IND], [LOGGWBGIND], [LOGGK24IND], [LOGGKI13IND], [LOGGNGS2UNC], [LOGGNGS1UNC], [LOGGANNSRUNC], [LOGGANNRRUNC], [LOGGCAI1UNC], [LOGGWBGUNC], [LOGGK24UNC], [LOGGKI13UNC], [FEHADOP], [FEHADOPN], [FEHADOPUNC], [FEHNGS2], [FEHNGS1], [FEHANNSR], [FEHANNRR], [FEHCAIIK1], [FEHCAIIK2], [FEHCAIIK3], [FEHWBG], [FEHK24], [FEHKI13], [FEHNGS2IND], [FEHNGS1IND], [FEHANNSRIND], [FEHANNRRIND], [FEHCAIIK1IND], [FEHCAIIK2IND], [FEHCAIIK3IND], [FEHWBGIND], [FEHK24IND], [FEHKI13IND], [FEHNGS2UNC], [FEHNGS1UNC], [FEHANNSRUNC], [FEHANNRRUNC], [FEHCAIIK1UNC], [FEHCAIIK2UNC], [FEHCAIIK3UNC], [FEHWBGUNC], [FEHK24UNC], [FEHKI13UNC], [SNR], [QA], [CCCAHK], [CCMGH], [TEFFSPEC], [TEFFSPECN], [TEFFSPECUNC], [LOGGSPEC], [LOGGSPECN], [LOGGSPECUNC], [FEHSPEC], [FEHSPECN], [FEHSPECUNC], [ACF1], [ACF1SNR], [ACF2], [ACF2SNR], [INSPECT], [ELODIERVFINAL], [ELODIERVFINALERR], [ZWARNING], [TEFFIRFM], [TEFFIRFMIND], [TEFFIRFMUNC], [LOGGNGS1IRFM], [LOGGNGS1IRFMIND], [LOGGNGS1IRFMUNC], [FEHNGS1IRFM], [FEHNGS1IRFMIND], [FEHNGS1IRFMUNC], [LOGGCAI1IRFM], [LOGGCAI1IRFMIND], [LOGGCAI1IRFMUNC], [FEHCAIIK1IRFM], [FEHCAIIK1IRFMIND], [FEHCAIIK1IRFMUNC])
 SELECT sourcetablealias.[SPECOBJID], sourcetablealias.[BESTOBJID], sourcetablealias.[FLUXOBJID], sourcetablealias.[TARGETOBJID], sourcetablealias.[PLATEID], sourcetablealias.[sciencePrimary], sourcetablealias.[legacyPrimary], sourcetablealias.[seguePrimary], sourcetablealias.[FIRSTRELEASE], sourcetablealias.[SURVEY], sourcetablealias.[PROGRAMNAME], sourcetablealias.[CHUNK], sourcetablealias.[PLATERUN], sourcetablealias.[MJD], sourcetablealias.[PLATE], sourcetablealias.[FIBERID], sourcetablealias.[RUN2D], sourcetablealias.[RUN1D], sourcetablealias.[RUNSSPP], sourcetablealias.[TARGETSTRING], sourcetablealias.[PRIMTARGET], sourcetablealias.[SECTARGET], sourcetablealias.[LEGACY_TARGET1], sourcetablealias.[LEGACY_TARGET2], sourcetablealias.[SPECIAL_TARGET1], sourcetablealias.[SPECIAL_TARGET2], sourcetablealias.[SEGUE1_TARGET1], sourcetablealias.[SEGUE1_TARGET2], sourcetablealias.[SEGUE2_TARGET1], sourcetablealias.[SEGUE2_TARGET2], sourcetablealias.[SPECTYPEHAMMER], sourcetablealias.[SPECTYPESUBCLASS], sourcetablealias.[FLAG], sourcetablealias.[TEFFADOP], sourcetablealias.[TEFFADOPN], sourcetablealias.[TEFFADOPUNC], sourcetablealias.[TEFFHA24], sourcetablealias.[TEFFHD24], sourcetablealias.[TEFFNGS1], sourcetablealias.[TEFFANNSR], sourcetablealias.[TEFFANNRR], sourcetablealias.[TEFFWBG], sourcetablealias.[TEFFK24], sourcetablealias.[TEFFKI13], sourcetablealias.[TEFFHA24IND], sourcetablealias.[TEFFHD24IND], sourcetablealias.[TEFFNGS1IND], sourcetablealias.[TEFFANNSRIND], sourcetablealias.[TEFFANNRRIND], sourcetablealias.[TEFFWBGIND], sourcetablealias.[TEFFK24IND], sourcetablealias.[TEFFKI13IND], sourcetablealias.[TEFFHA24UNC], sourcetablealias.[TEFFHD24UNC], sourcetablealias.[TEFFNGS1UNC], sourcetablealias.[TEFFANNSRUNC], sourcetablealias.[TEFFANNRRUNC], sourcetablealias.[TEFFWBGUNC], sourcetablealias.[TEFFK24UNC], sourcetablealias.[TEFFKI13UNC], sourcetablealias.[LOGGADOP], sourcetablealias.[LOGGADOPN], sourcetablealias.[LOGGADOPUNC], sourcetablealias.[LOGGNGS2], sourcetablealias.[LOGGNGS1], sourcetablealias.[LOGGANNSR], sourcetablealias.[LOGGANNRR], sourcetablealias.[LOGGCAI1], sourcetablealias.[LOGGWBG], sourcetablealias.[LOGGK24], sourcetablealias.[LOGGKI13], sourcetablealias.[LOGGNGS2IND], sourcetablealias.[LOGGNGS1IND], sourcetablealias.[LOGGANNSRIND], sourcetablealias.[LOGGANNRRIND], sourcetablealias.[LOGGCAI1IND], sourcetablealias.[LOGGWBGIND], sourcetablealias.[LOGGK24IND], sourcetablealias.[LOGGKI13IND], sourcetablealias.[LOGGNGS2UNC], sourcetablealias.[LOGGNGS1UNC], sourcetablealias.[LOGGANNSRUNC], sourcetablealias.[LOGGANNRRUNC], sourcetablealias.[LOGGCAI1UNC], sourcetablealias.[LOGGWBGUNC], sourcetablealias.[LOGGK24UNC], sourcetablealias.[LOGGKI13UNC], sourcetablealias.[FEHADOP], sourcetablealias.[FEHADOPN], sourcetablealias.[FEHADOPUNC], sourcetablealias.[FEHNGS2], sourcetablealias.[FEHNGS1], sourcetablealias.[FEHANNSR], sourcetablealias.[FEHANNRR], sourcetablealias.[FEHCAIIK1], sourcetablealias.[FEHCAIIK2], sourcetablealias.[FEHCAIIK3], sourcetablealias.[FEHWBG], sourcetablealias.[FEHK24], sourcetablealias.[FEHKI13], sourcetablealias.[FEHNGS2IND], sourcetablealias.[FEHNGS1IND], sourcetablealias.[FEHANNSRIND], sourcetablealias.[FEHANNRRIND], sourcetablealias.[FEHCAIIK1IND], sourcetablealias.[FEHCAIIK2IND], sourcetablealias.[FEHCAIIK3IND], sourcetablealias.[FEHWBGIND], sourcetablealias.[FEHK24IND], sourcetablealias.[FEHKI13IND], sourcetablealias.[FEHNGS2UNC], sourcetablealias.[FEHNGS1UNC], sourcetablealias.[FEHANNSRUNC], sourcetablealias.[FEHANNRRUNC], sourcetablealias.[FEHCAIIK1UNC], sourcetablealias.[FEHCAIIK2UNC], sourcetablealias.[FEHCAIIK3UNC], sourcetablealias.[FEHWBGUNC], sourcetablealias.[FEHK24UNC], sourcetablealias.[FEHKI13UNC], sourcetablealias.[SNR], sourcetablealias.[QA], sourcetablealias.[CCCAHK], sourcetablealias.[CCMGH], sourcetablealias.[TEFFSPEC], sourcetablealias.[TEFFSPECN], sourcetablealias.[TEFFSPECUNC], sourcetablealias.[LOGGSPEC], sourcetablealias.[LOGGSPECN], sourcetablealias.[LOGGSPECUNC], sourcetablealias.[FEHSPEC], sourcetablealias.[FEHSPECN], sourcetablealias.[FEHSPECUNC], sourcetablealias.[ACF1], sourcetablealias.[ACF1SNR], sourcetablealias.[ACF2], sourcetablealias.[ACF2SNR], sourcetablealias.[INSPECT], sourcetablealias.[ELODIERVFINAL], sourcetablealias.[ELODIERVFINALERR], sourcetablealias.[ZWARNING], sourcetablealias.[TEFFIRFM], sourcetablealias.[TEFFIRFMIND], sourcetablealias.[TEFFIRFMUNC], sourcetablealias.[LOGGNGS1IRFM], sourcetablealias.[LOGGNGS1IRFMIND], sourcetablealias.[LOGGNGS1IRFMUNC], sourcetablealias.[FEHNGS1IRFM], sourcetablealias.[FEHNGS1IRFMIND], sourcetablealias.[FEHNGS1IRFMUNC], sourcetablealias.[LOGGCAI1IRFM], sourcetablealias.[LOGGCAI1IRFMIND], sourcetablealias.[LOGGCAI1IRFMUNC], sourcetablealias.[FEHCAIIK1IRFM], sourcetablealias.[FEHCAIIK1IRFMIND], sourcetablealias.[FEHCAIIK1IRFMUNC]
 FROM   [SkyNode_SDSSDR12].[dbo].[sppParams] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.SPECOBJID = sourcetablealias.SPECOBJID
	;


GO

-- SUBSAMPLING TABLE 'stellarMassFSPSGranEarlyDust' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[stellarMassFSPSGranEarlyDust] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[stellarMassFSPSGranEarlyDust] WITH (TABLOCKX)
	([specObjID], [plate], [fiberID], [MJD], [ra], [dec], [z], [z_err], [ke_u], [ke_g], [ke_r], [ke_i], [ke_z], [cModelAbsMag_u], [cModelAbsMag_g], [cModelAbsMag_r], [cModelAbsMag_i], [cModelAbsMag_z], [m2l_u], [m2l_g], [m2l_r], [m2l_i], [m2l_z], [m2l_median_u], [m2l_median_g], [m2l_median_r], [m2l_median_i], [m2l_median_z], [m2l_err_u], [m2l_err_g], [m2l_err_r], [m2l_err_i], [m2l_err_z], [m2l_min_u], [m2l_min_g], [m2l_min_r], [m2l_min_i], [m2l_min_z], [m2l_max_u], [m2l_max_g], [m2l_max_r], [m2l_max_i], [m2l_max_z], [logMass], [logMass_median], [logMass_err], [logMass_min], [logMass_max], [chi2], [nFilter], [t_age], [t_age_mean], [t_age_err], [t_age_min], [t_age_max], [metallicity], [metallicity_mean], [metallicity_err], [metallicity_min], [metallicity_max], [dust1], [dust1_mean], [dust1_err], [dust1_min], [dust1_max], [dust2], [dust2_mean], [dust2_err], [dust2_min], [dust2_max], [tau], [tau_mean], [tau_err], [tau_min], [tau_max], [age], [age_mean], [age_min], [age_max], [ssfr], [ssfr_mean], [ssfr_min], [ssfr_max])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[plate], sourcetablealias.[fiberID], sourcetablealias.[MJD], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[z], sourcetablealias.[z_err], sourcetablealias.[ke_u], sourcetablealias.[ke_g], sourcetablealias.[ke_r], sourcetablealias.[ke_i], sourcetablealias.[ke_z], sourcetablealias.[cModelAbsMag_u], sourcetablealias.[cModelAbsMag_g], sourcetablealias.[cModelAbsMag_r], sourcetablealias.[cModelAbsMag_i], sourcetablealias.[cModelAbsMag_z], sourcetablealias.[m2l_u], sourcetablealias.[m2l_g], sourcetablealias.[m2l_r], sourcetablealias.[m2l_i], sourcetablealias.[m2l_z], sourcetablealias.[m2l_median_u], sourcetablealias.[m2l_median_g], sourcetablealias.[m2l_median_r], sourcetablealias.[m2l_median_i], sourcetablealias.[m2l_median_z], sourcetablealias.[m2l_err_u], sourcetablealias.[m2l_err_g], sourcetablealias.[m2l_err_r], sourcetablealias.[m2l_err_i], sourcetablealias.[m2l_err_z], sourcetablealias.[m2l_min_u], sourcetablealias.[m2l_min_g], sourcetablealias.[m2l_min_r], sourcetablealias.[m2l_min_i], sourcetablealias.[m2l_min_z], sourcetablealias.[m2l_max_u], sourcetablealias.[m2l_max_g], sourcetablealias.[m2l_max_r], sourcetablealias.[m2l_max_i], sourcetablealias.[m2l_max_z], sourcetablealias.[logMass], sourcetablealias.[logMass_median], sourcetablealias.[logMass_err], sourcetablealias.[logMass_min], sourcetablealias.[logMass_max], sourcetablealias.[chi2], sourcetablealias.[nFilter], sourcetablealias.[t_age], sourcetablealias.[t_age_mean], sourcetablealias.[t_age_err], sourcetablealias.[t_age_min], sourcetablealias.[t_age_max], sourcetablealias.[metallicity], sourcetablealias.[metallicity_mean], sourcetablealias.[metallicity_err], sourcetablealias.[metallicity_min], sourcetablealias.[metallicity_max], sourcetablealias.[dust1], sourcetablealias.[dust1_mean], sourcetablealias.[dust1_err], sourcetablealias.[dust1_min], sourcetablealias.[dust1_max], sourcetablealias.[dust2], sourcetablealias.[dust2_mean], sourcetablealias.[dust2_err], sourcetablealias.[dust2_min], sourcetablealias.[dust2_max], sourcetablealias.[tau], sourcetablealias.[tau_mean], sourcetablealias.[tau_err], sourcetablealias.[tau_min], sourcetablealias.[tau_max], sourcetablealias.[age], sourcetablealias.[age_mean], sourcetablealias.[age_min], sourcetablealias.[age_max], sourcetablealias.[ssfr], sourcetablealias.[ssfr_mean], sourcetablealias.[ssfr_min], sourcetablealias.[ssfr_max]
 FROM   [SkyNode_SDSSDR12].[dbo].[stellarMassFSPSGranEarlyDust] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'stellarMassFSPSGranEarlyNoDust' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[stellarMassFSPSGranEarlyNoDust] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[stellarMassFSPSGranEarlyNoDust] WITH (TABLOCKX)
	([specObjID], [plate], [fiberID], [MJD], [ra], [dec], [z], [z_err], [ke_u], [ke_g], [ke_r], [ke_i], [ke_z], [cModelAbsMag_u], [cModelAbsMag_g], [cModelAbsMag_r], [cModelAbsMag_i], [cModelAbsMag_z], [m2l_u], [m2l_g], [m2l_r], [m2l_i], [m2l_z], [m2l_median_u], [m2l_median_g], [m2l_median_r], [m2l_median_i], [m2l_median_z], [m2l_err_u], [m2l_err_g], [m2l_err_r], [m2l_err_i], [m2l_err_z], [m2l_min_u], [m2l_min_g], [m2l_min_r], [m2l_min_i], [m2l_min_z], [m2l_max_u], [m2l_max_g], [m2l_max_r], [m2l_max_i], [m2l_max_z], [logMass], [logMass_median], [logMass_err], [logMass_min], [logMass_max], [chi2], [nFilter], [t_age], [t_age_mean], [t_age_err], [t_age_min], [t_age_max], [metallicity], [metallicity_mean], [metallicity_err], [metallicity_min], [metallicity_max], [dust1], [dust1_mean], [dust1_err], [dust1_min], [dust1_max], [dust2], [dust2_mean], [dust2_err], [dust2_min], [dust2_max], [tau], [tau_mean], [tau_err], [tau_min], [tau_max], [age], [age_mean], [age_min], [age_max], [ssfr], [ssfr_mean], [ssfr_min], [ssfr_max])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[plate], sourcetablealias.[fiberID], sourcetablealias.[MJD], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[z], sourcetablealias.[z_err], sourcetablealias.[ke_u], sourcetablealias.[ke_g], sourcetablealias.[ke_r], sourcetablealias.[ke_i], sourcetablealias.[ke_z], sourcetablealias.[cModelAbsMag_u], sourcetablealias.[cModelAbsMag_g], sourcetablealias.[cModelAbsMag_r], sourcetablealias.[cModelAbsMag_i], sourcetablealias.[cModelAbsMag_z], sourcetablealias.[m2l_u], sourcetablealias.[m2l_g], sourcetablealias.[m2l_r], sourcetablealias.[m2l_i], sourcetablealias.[m2l_z], sourcetablealias.[m2l_median_u], sourcetablealias.[m2l_median_g], sourcetablealias.[m2l_median_r], sourcetablealias.[m2l_median_i], sourcetablealias.[m2l_median_z], sourcetablealias.[m2l_err_u], sourcetablealias.[m2l_err_g], sourcetablealias.[m2l_err_r], sourcetablealias.[m2l_err_i], sourcetablealias.[m2l_err_z], sourcetablealias.[m2l_min_u], sourcetablealias.[m2l_min_g], sourcetablealias.[m2l_min_r], sourcetablealias.[m2l_min_i], sourcetablealias.[m2l_min_z], sourcetablealias.[m2l_max_u], sourcetablealias.[m2l_max_g], sourcetablealias.[m2l_max_r], sourcetablealias.[m2l_max_i], sourcetablealias.[m2l_max_z], sourcetablealias.[logMass], sourcetablealias.[logMass_median], sourcetablealias.[logMass_err], sourcetablealias.[logMass_min], sourcetablealias.[logMass_max], sourcetablealias.[chi2], sourcetablealias.[nFilter], sourcetablealias.[t_age], sourcetablealias.[t_age_mean], sourcetablealias.[t_age_err], sourcetablealias.[t_age_min], sourcetablealias.[t_age_max], sourcetablealias.[metallicity], sourcetablealias.[metallicity_mean], sourcetablealias.[metallicity_err], sourcetablealias.[metallicity_min], sourcetablealias.[metallicity_max], sourcetablealias.[dust1], sourcetablealias.[dust1_mean], sourcetablealias.[dust1_err], sourcetablealias.[dust1_min], sourcetablealias.[dust1_max], sourcetablealias.[dust2], sourcetablealias.[dust2_mean], sourcetablealias.[dust2_err], sourcetablealias.[dust2_min], sourcetablealias.[dust2_max], sourcetablealias.[tau], sourcetablealias.[tau_mean], sourcetablealias.[tau_err], sourcetablealias.[tau_min], sourcetablealias.[tau_max], sourcetablealias.[age], sourcetablealias.[age_mean], sourcetablealias.[age_min], sourcetablealias.[age_max], sourcetablealias.[ssfr], sourcetablealias.[ssfr_mean], sourcetablealias.[ssfr_min], sourcetablealias.[ssfr_max]
 FROM   [SkyNode_SDSSDR12].[dbo].[stellarMassFSPSGranEarlyNoDust] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'stellarMassFSPSGranWideDust' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[stellarMassFSPSGranWideDust] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[stellarMassFSPSGranWideDust] WITH (TABLOCKX)
	([specObjID], [plate], [fiberID], [MJD], [ra], [dec], [z], [z_err], [ke_u], [ke_g], [ke_r], [ke_i], [ke_z], [cModelAbsMag_u], [cModelAbsMag_g], [cModelAbsMag_r], [cModelAbsMag_i], [cModelAbsMag_z], [m2l_u], [m2l_g], [m2l_r], [m2l_i], [m2l_z], [m2l_median_u], [m2l_median_g], [m2l_median_r], [m2l_median_i], [m2l_median_z], [m2l_err_u], [m2l_err_g], [m2l_err_r], [m2l_err_i], [m2l_err_z], [m2l_min_u], [m2l_min_g], [m2l_min_r], [m2l_min_i], [m2l_min_z], [m2l_max_u], [m2l_max_g], [m2l_max_r], [m2l_max_i], [m2l_max_z], [logMass], [logMass_median], [logMass_err], [logMass_min], [logMass_max], [chi2], [nFilter], [t_age], [t_age_mean], [t_age_err], [t_age_min], [t_age_max], [metallicity], [metallicity_mean], [metallicity_err], [metallicity_min], [metallicity_max], [dust1], [dust1_mean], [dust1_err], [dust1_min], [dust1_max], [dust2], [dust2_mean], [dust2_err], [dust2_min], [dust2_max], [tau], [tau_mean], [tau_err], [tau_min], [tau_max], [age], [age_mean], [age_min], [age_max], [ssfr], [ssfr_mean], [ssfr_min], [ssfr_max])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[plate], sourcetablealias.[fiberID], sourcetablealias.[MJD], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[z], sourcetablealias.[z_err], sourcetablealias.[ke_u], sourcetablealias.[ke_g], sourcetablealias.[ke_r], sourcetablealias.[ke_i], sourcetablealias.[ke_z], sourcetablealias.[cModelAbsMag_u], sourcetablealias.[cModelAbsMag_g], sourcetablealias.[cModelAbsMag_r], sourcetablealias.[cModelAbsMag_i], sourcetablealias.[cModelAbsMag_z], sourcetablealias.[m2l_u], sourcetablealias.[m2l_g], sourcetablealias.[m2l_r], sourcetablealias.[m2l_i], sourcetablealias.[m2l_z], sourcetablealias.[m2l_median_u], sourcetablealias.[m2l_median_g], sourcetablealias.[m2l_median_r], sourcetablealias.[m2l_median_i], sourcetablealias.[m2l_median_z], sourcetablealias.[m2l_err_u], sourcetablealias.[m2l_err_g], sourcetablealias.[m2l_err_r], sourcetablealias.[m2l_err_i], sourcetablealias.[m2l_err_z], sourcetablealias.[m2l_min_u], sourcetablealias.[m2l_min_g], sourcetablealias.[m2l_min_r], sourcetablealias.[m2l_min_i], sourcetablealias.[m2l_min_z], sourcetablealias.[m2l_max_u], sourcetablealias.[m2l_max_g], sourcetablealias.[m2l_max_r], sourcetablealias.[m2l_max_i], sourcetablealias.[m2l_max_z], sourcetablealias.[logMass], sourcetablealias.[logMass_median], sourcetablealias.[logMass_err], sourcetablealias.[logMass_min], sourcetablealias.[logMass_max], sourcetablealias.[chi2], sourcetablealias.[nFilter], sourcetablealias.[t_age], sourcetablealias.[t_age_mean], sourcetablealias.[t_age_err], sourcetablealias.[t_age_min], sourcetablealias.[t_age_max], sourcetablealias.[metallicity], sourcetablealias.[metallicity_mean], sourcetablealias.[metallicity_err], sourcetablealias.[metallicity_min], sourcetablealias.[metallicity_max], sourcetablealias.[dust1], sourcetablealias.[dust1_mean], sourcetablealias.[dust1_err], sourcetablealias.[dust1_min], sourcetablealias.[dust1_max], sourcetablealias.[dust2], sourcetablealias.[dust2_mean], sourcetablealias.[dust2_err], sourcetablealias.[dust2_min], sourcetablealias.[dust2_max], sourcetablealias.[tau], sourcetablealias.[tau_mean], sourcetablealias.[tau_err], sourcetablealias.[tau_min], sourcetablealias.[tau_max], sourcetablealias.[age], sourcetablealias.[age_mean], sourcetablealias.[age_min], sourcetablealias.[age_max], sourcetablealias.[ssfr], sourcetablealias.[ssfr_mean], sourcetablealias.[ssfr_min], sourcetablealias.[ssfr_max]
 FROM   [SkyNode_SDSSDR12].[dbo].[stellarMassFSPSGranWideDust] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'stellarMassFSPSGranWideNoDust' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[stellarMassFSPSGranWideNoDust] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[stellarMassFSPSGranWideNoDust] WITH (TABLOCKX)
	([specObjID], [plate], [fiberID], [MJD], [ra], [dec], [z], [z_err], [ke_u], [ke_g], [ke_r], [ke_i], [ke_z], [cModelAbsMag_u], [cModelAbsMag_g], [cModelAbsMag_r], [cModelAbsMag_i], [cModelAbsMag_z], [m2l_u], [m2l_g], [m2l_r], [m2l_i], [m2l_z], [m2l_median_u], [m2l_median_g], [m2l_median_r], [m2l_median_i], [m2l_median_z], [m2l_err_u], [m2l_err_g], [m2l_err_r], [m2l_err_i], [m2l_err_z], [m2l_min_u], [m2l_min_g], [m2l_min_r], [m2l_min_i], [m2l_min_z], [m2l_max_u], [m2l_max_g], [m2l_max_r], [m2l_max_i], [m2l_max_z], [logMass], [logMass_median], [logMass_err], [logMass_min], [logMass_max], [chi2], [nFilter], [t_age], [t_age_mean], [t_age_err], [t_age_min], [t_age_max], [metallicity], [metallicity_mean], [metallicity_err], [metallicity_min], [metallicity_max], [dust1], [dust1_mean], [dust1_err], [dust1_min], [dust1_max], [dust2], [dust2_mean], [dust2_err], [dust2_min], [dust2_max], [tau], [tau_mean], [tau_err], [tau_min], [tau_max], [age], [age_mean], [age_min], [age_max], [ssfr], [ssfr_mean], [ssfr_min], [ssfr_max])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[plate], sourcetablealias.[fiberID], sourcetablealias.[MJD], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[z], sourcetablealias.[z_err], sourcetablealias.[ke_u], sourcetablealias.[ke_g], sourcetablealias.[ke_r], sourcetablealias.[ke_i], sourcetablealias.[ke_z], sourcetablealias.[cModelAbsMag_u], sourcetablealias.[cModelAbsMag_g], sourcetablealias.[cModelAbsMag_r], sourcetablealias.[cModelAbsMag_i], sourcetablealias.[cModelAbsMag_z], sourcetablealias.[m2l_u], sourcetablealias.[m2l_g], sourcetablealias.[m2l_r], sourcetablealias.[m2l_i], sourcetablealias.[m2l_z], sourcetablealias.[m2l_median_u], sourcetablealias.[m2l_median_g], sourcetablealias.[m2l_median_r], sourcetablealias.[m2l_median_i], sourcetablealias.[m2l_median_z], sourcetablealias.[m2l_err_u], sourcetablealias.[m2l_err_g], sourcetablealias.[m2l_err_r], sourcetablealias.[m2l_err_i], sourcetablealias.[m2l_err_z], sourcetablealias.[m2l_min_u], sourcetablealias.[m2l_min_g], sourcetablealias.[m2l_min_r], sourcetablealias.[m2l_min_i], sourcetablealias.[m2l_min_z], sourcetablealias.[m2l_max_u], sourcetablealias.[m2l_max_g], sourcetablealias.[m2l_max_r], sourcetablealias.[m2l_max_i], sourcetablealias.[m2l_max_z], sourcetablealias.[logMass], sourcetablealias.[logMass_median], sourcetablealias.[logMass_err], sourcetablealias.[logMass_min], sourcetablealias.[logMass_max], sourcetablealias.[chi2], sourcetablealias.[nFilter], sourcetablealias.[t_age], sourcetablealias.[t_age_mean], sourcetablealias.[t_age_err], sourcetablealias.[t_age_min], sourcetablealias.[t_age_max], sourcetablealias.[metallicity], sourcetablealias.[metallicity_mean], sourcetablealias.[metallicity_err], sourcetablealias.[metallicity_min], sourcetablealias.[metallicity_max], sourcetablealias.[dust1], sourcetablealias.[dust1_mean], sourcetablealias.[dust1_err], sourcetablealias.[dust1_min], sourcetablealias.[dust1_max], sourcetablealias.[dust2], sourcetablealias.[dust2_mean], sourcetablealias.[dust2_err], sourcetablealias.[dust2_min], sourcetablealias.[dust2_max], sourcetablealias.[tau], sourcetablealias.[tau_mean], sourcetablealias.[tau_err], sourcetablealias.[tau_min], sourcetablealias.[tau_max], sourcetablealias.[age], sourcetablealias.[age_mean], sourcetablealias.[age_min], sourcetablealias.[age_max], sourcetablealias.[ssfr], sourcetablealias.[ssfr_mean], sourcetablealias.[ssfr_min], sourcetablealias.[ssfr_max]
 FROM   [SkyNode_SDSSDR12].[dbo].[stellarMassFSPSGranWideNoDust] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'stellarMassPassivePort' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[stellarMassPassivePort] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[stellarMassPassivePort] WITH (TABLOCKX)
	([specObjID], [plate], [fiberID], [mjd], [ra], [dec], [z], [zErr], [logMass], [minLogMass], [maxLogMass], [medianPDF], [PDF16], [PDF84], [peakPDF], [logMass_noMassLoss], [minLogMass_noMassLoss], [maxLogMass_noMassLoss], [medianPDF_noMassLoss], [PDF16_noMassLoss], [PDF84_noMassLoss], [peakPDF_noMassLoss], [reducedChi2], [age], [minAge], [maxAge], [SFR], [minSFR], [maxSFR], [absMagK], [SFH], [Metallicity], [reddeninglaw], [nFilter])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[plate], sourcetablealias.[fiberID], sourcetablealias.[mjd], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[z], sourcetablealias.[zErr], sourcetablealias.[logMass], sourcetablealias.[minLogMass], sourcetablealias.[maxLogMass], sourcetablealias.[medianPDF], sourcetablealias.[PDF16], sourcetablealias.[PDF84], sourcetablealias.[peakPDF], sourcetablealias.[logMass_noMassLoss], sourcetablealias.[minLogMass_noMassLoss], sourcetablealias.[maxLogMass_noMassLoss], sourcetablealias.[medianPDF_noMassLoss], sourcetablealias.[PDF16_noMassLoss], sourcetablealias.[PDF84_noMassLoss], sourcetablealias.[peakPDF_noMassLoss], sourcetablealias.[reducedChi2], sourcetablealias.[age], sourcetablealias.[minAge], sourcetablealias.[maxAge], sourcetablealias.[SFR], sourcetablealias.[minSFR], sourcetablealias.[maxSFR], sourcetablealias.[absMagK], sourcetablealias.[SFH], sourcetablealias.[Metallicity], sourcetablealias.[reddeninglaw], sourcetablealias.[nFilter]
 FROM   [SkyNode_SDSSDR12].[dbo].[stellarMassPassivePort] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'stellarMassPCAWiscBC03' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[stellarMassPCAWiscBC03] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[stellarMassPCAWiscBC03] WITH (TABLOCKX)
	([specObjID], [plate], [fiberID], [mjd], [ra], [dec], [z], [zErr], [zNum], [mstellar_median], [mstellar_err], [mstellar_p2p5], [mstellar_p16], [mstellar_p84], [mstellar_p97p5], [vdisp_median], [vdisp_err], [vdisp_p2p5], [vdisp_p16], [vdisp_p84], [vdisp_p97p5], [calpha_0], [calpha_1], [calpha_2], [calpha_3], [calpha_4], [calpha_5], [calpha_6], [calpha_norm], [warning])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[plate], sourcetablealias.[fiberID], sourcetablealias.[mjd], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[z], sourcetablealias.[zErr], sourcetablealias.[zNum], sourcetablealias.[mstellar_median], sourcetablealias.[mstellar_err], sourcetablealias.[mstellar_p2p5], sourcetablealias.[mstellar_p16], sourcetablealias.[mstellar_p84], sourcetablealias.[mstellar_p97p5], sourcetablealias.[vdisp_median], sourcetablealias.[vdisp_err], sourcetablealias.[vdisp_p2p5], sourcetablealias.[vdisp_p16], sourcetablealias.[vdisp_p84], sourcetablealias.[vdisp_p97p5], sourcetablealias.[calpha_0], sourcetablealias.[calpha_1], sourcetablealias.[calpha_2], sourcetablealias.[calpha_3], sourcetablealias.[calpha_4], sourcetablealias.[calpha_5], sourcetablealias.[calpha_6], sourcetablealias.[calpha_norm], sourcetablealias.[warning]
 FROM   [SkyNode_SDSSDR12].[dbo].[stellarMassPCAWiscBC03] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'stellarMassPCAWiscM11' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[stellarMassPCAWiscM11] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[stellarMassPCAWiscM11] WITH (TABLOCKX)
	([specObjID], [plate], [fiberID], [mjd], [ra], [dec], [z], [zErr], [zNum], [mstellar_median], [mstellar_err], [mstellar_p2p5], [mstellar_p16], [mstellar_p84], [mstellar_p97p5], [vdisp_median], [vdisp_err], [vdisp_p2p5], [vdisp_p16], [vdisp_p84], [vdisp_p97p5], [calpha_0], [calpha_1], [calpha_2], [calpha_3], [calpha_4], [calpha_5], [calpha_6], [calpha_norm], [warning])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[plate], sourcetablealias.[fiberID], sourcetablealias.[mjd], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[z], sourcetablealias.[zErr], sourcetablealias.[zNum], sourcetablealias.[mstellar_median], sourcetablealias.[mstellar_err], sourcetablealias.[mstellar_p2p5], sourcetablealias.[mstellar_p16], sourcetablealias.[mstellar_p84], sourcetablealias.[mstellar_p97p5], sourcetablealias.[vdisp_median], sourcetablealias.[vdisp_err], sourcetablealias.[vdisp_p2p5], sourcetablealias.[vdisp_p16], sourcetablealias.[vdisp_p84], sourcetablealias.[vdisp_p97p5], sourcetablealias.[calpha_0], sourcetablealias.[calpha_1], sourcetablealias.[calpha_2], sourcetablealias.[calpha_3], sourcetablealias.[calpha_4], sourcetablealias.[calpha_5], sourcetablealias.[calpha_6], sourcetablealias.[calpha_norm], sourcetablealias.[warning]
 FROM   [SkyNode_SDSSDR12].[dbo].[stellarMassPCAWiscM11] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'stellarMassStarformingPort' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specObjID] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specObjID], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[stellarMassStarformingPort] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specObjID]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[stellarMassStarformingPort] WITH (TABLOCKX)
	([specObjID], [plate], [fiberID], [mjd], [ra], [dec], [z], [zErr], [logMass], [minLogMass], [maxLogMass], [medianPDF], [PDF16], [PDF84], [peakPDF], [logMass_noMassLoss], [minLogMass_noMassLoss], [maxLogMass_noMassLoss], [medianPDF_noMassLoss], [PDF16_noMassLoss], [PDF84_noMassLoss], [peakPDF_noMassLoss], [reducedChi2], [age], [minAge], [maxAge], [SFR], [minSFR], [maxSFR], [absMagK], [SFH], [Metallicity], [reddeninglaw], [nFilter])
 SELECT sourcetablealias.[specObjID], sourcetablealias.[plate], sourcetablealias.[fiberID], sourcetablealias.[mjd], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[z], sourcetablealias.[zErr], sourcetablealias.[logMass], sourcetablealias.[minLogMass], sourcetablealias.[maxLogMass], sourcetablealias.[medianPDF], sourcetablealias.[PDF16], sourcetablealias.[PDF84], sourcetablealias.[peakPDF], sourcetablealias.[logMass_noMassLoss], sourcetablealias.[minLogMass_noMassLoss], sourcetablealias.[maxLogMass_noMassLoss], sourcetablealias.[medianPDF_noMassLoss], sourcetablealias.[PDF16_noMassLoss], sourcetablealias.[PDF84_noMassLoss], sourcetablealias.[peakPDF_noMassLoss], sourcetablealias.[reducedChi2], sourcetablealias.[age], sourcetablealias.[minAge], sourcetablealias.[maxAge], sourcetablealias.[SFR], sourcetablealias.[minSFR], sourcetablealias.[maxSFR], sourcetablealias.[absMagK], sourcetablealias.[SFH], sourcetablealias.[Metallicity], sourcetablealias.[reddeninglaw], sourcetablealias.[nFilter]
 FROM   [SkyNode_SDSSDR12].[dbo].[stellarMassStarformingPort] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specObjID = sourcetablealias.specObjID
	;


GO

-- SUBSAMPLING TABLE 'StripeDefs' ---

 -- Insert subset into destination table
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[StripeDefs] WITH (TABLOCKX)
    ([stripe], [hemisphere], [eta], [lambdaMin], [lambdaMax], [htmArea])
 SELECT sourcetablealias.[stripe], sourcetablealias.[hemisphere], sourcetablealias.[eta], sourcetablealias.[lambdaMin], sourcetablealias.[lambdaMax], sourcetablealias.[htmArea]
 FROM   [SkyNode_SDSSDR12].[dbo].[StripeDefs] sourcetablealias WITH (NOLOCK)
	;
 

GO

-- SUBSAMPLING TABLE 'thingIndex' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[thingId] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[thingId], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[thingIndex] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [thingId]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.001;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[thingIndex] WITH (TABLOCKX)
	([thingId], [sdssPolygonID], [fieldID], [objID], [isPrimary], [nDetect], [nEdge], [ra], [dec], [loadVersion])
 SELECT sourcetablealias.[thingId], sourcetablealias.[sdssPolygonID], sourcetablealias.[fieldID], sourcetablealias.[objID], sourcetablealias.[isPrimary], sourcetablealias.[nDetect], sourcetablealias.[nEdge], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[loadVersion]
 FROM   [SkyNode_SDSSDR12].[dbo].[thingIndex] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.thingId = sourcetablealias.thingId
	;


GO

-- SUBSAMPLING TABLE 'zoo2MainPhotoz' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[dr7objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[dr7objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zoo2MainPhotoz] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [dr7objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zoo2MainPhotoz] WITH (TABLOCKX)
	([dr8objid], [dr7objid], [ra], [dec], [rastring], [decstring], [sample], [total_classifications], [total_votes], [t01_smooth_or_features_a01_smooth_count], [t01_smooth_or_features_a01_smooth_weight], [t01_smooth_or_features_a01_smooth_fraction], [t01_smooth_or_features_a01_smooth_weighted_fraction], [t01_smooth_or_features_a01_smooth_debiased], [t01_smooth_or_features_a01_smooth_flag], [t01_smooth_or_features_a02_features_or_disk_count], [t01_smooth_or_features_a02_features_or_disk_weight], [t01_smooth_or_features_a02_features_or_disk_fraction], [t01_smooth_or_features_a02_features_or_disk_weighted_fraction], [t01_smooth_or_features_a02_features_or_disk_debiased], [t01_smooth_or_features_a02_features_or_disk_flag], [t01_smooth_or_features_a03_star_or_artifact_count], [t01_smooth_or_features_a03_star_or_artifact_weight], [t01_smooth_or_features_a03_star_or_artifact_fraction], [t01_smooth_or_features_a03_star_or_artifact_weighted_fraction], [t01_smooth_or_features_a03_star_or_artifact_debiased], [t01_smooth_or_features_a03_star_or_artifact_flag], [t02_edgeon_a04_yes_count], [t02_edgeon_a04_yes_weight], [t02_edgeon_a04_yes_fraction], [t02_edgeon_a04_yes_weighted_fraction], [t02_edgeon_a04_yes_debiased], [t02_edgeon_a04_yes_flag], [t02_edgeon_a05_no_count], [t02_edgeon_a05_no_weight], [t02_edgeon_a05_no_fraction], [t02_edgeon_a05_no_weighted_fraction], [t02_edgeon_a05_no_debiased], [t02_edgeon_a05_no_flag], [t03_bar_a06_bar_count], [t03_bar_a06_bar_weight], [t03_bar_a06_bar_fraction], [t03_bar_a06_bar_weighted_fraction], [t03_bar_a06_bar_debiased], [t03_bar_a06_bar_flag], [t03_bar_a07_no_bar_count], [t03_bar_a07_no_bar_weight], [t03_bar_a07_no_bar_fraction], [t03_bar_a07_no_bar_weighted_fraction], [t03_bar_a07_no_bar_debiased], [t03_bar_a07_no_bar_flag], [t04_spiral_a08_spiral_count], [t04_spiral_a08_spiral_weight], [t04_spiral_a08_spiral_fraction], [t04_spiral_a08_spiral_weighted_fraction], [t04_spiral_a08_spiral_debiased], [t04_spiral_a08_spiral_flag], [t04_spiral_a09_no_spiral_count], [t04_spiral_a09_no_spiral_weight], [t04_spiral_a09_no_spiral_fraction], [t04_spiral_a09_no_spiral_weighted_fraction], [t04_spiral_a09_no_spiral_debiased], [t04_spiral_a09_no_spiral_flag], [t05_bulge_prominence_a10_no_bulge_count], [t05_bulge_prominence_a10_no_bulge_weight], [t05_bulge_prominence_a10_no_bulge_fraction], [t05_bulge_prominence_a10_no_bulge_weighted_fraction], [t05_bulge_prominence_a10_no_bulge_debiased], [t05_bulge_prominence_a10_no_bulge_flag], [t05_bulge_prominence_a11_just_noticeable_count], [t05_bulge_prominence_a11_just_noticeable_weight], [t05_bulge_prominence_a11_just_noticeable_fraction], [t05_bulge_prominence_a11_just_noticeable_weighted_fraction], [t05_bulge_prominence_a11_just_noticeable_debiased], [t05_bulge_prominence_a11_just_noticeable_flag], [t05_bulge_prominence_a12_obvious_count], [t05_bulge_prominence_a12_obvious_weight], [t05_bulge_prominence_a12_obvious_fraction], [t05_bulge_prominence_a12_obvious_weighted_fraction], [t05_bulge_prominence_a12_obvious_debiased], [t05_bulge_prominence_a12_obvious_flag], [t05_bulge_prominence_a13_dominant_count], [t05_bulge_prominence_a13_dominant_weight], [t05_bulge_prominence_a13_dominant_fraction], [t05_bulge_prominence_a13_dominant_weighted_fraction], [t05_bulge_prominence_a13_dominant_debiased], [t05_bulge_prominence_a13_dominant_flag], [t06_odd_a14_yes_count], [t06_odd_a14_yes_weight], [t06_odd_a14_yes_fraction], [t06_odd_a14_yes_weighted_fraction], [t06_odd_a14_yes_debiased], [t06_odd_a14_yes_flag], [t06_odd_a15_no_count], [t06_odd_a15_no_weight], [t06_odd_a15_no_fraction], [t06_odd_a15_no_weighted_fraction], [t06_odd_a15_no_debiased], [t06_odd_a15_no_flag], [t07_rounded_a16_completely_round_count], [t07_rounded_a16_completely_round_weight], [t07_rounded_a16_completely_round_fraction], [t07_rounded_a16_completely_round_weighted_fraction], [t07_rounded_a16_completely_round_debiased], [t07_rounded_a16_completely_round_flag], [t07_rounded_a17_in_between_count], [t07_rounded_a17_in_between_weight], [t07_rounded_a17_in_between_fraction], [t07_rounded_a17_in_between_weighted_fraction], [t07_rounded_a17_in_between_debiased], [t07_rounded_a17_in_between_flag], [t07_rounded_a18_cigar_shaped_count], [t07_rounded_a18_cigar_shaped_weight], [t07_rounded_a18_cigar_shaped_fraction], [t07_rounded_a18_cigar_shaped_weighted_fraction], [t07_rounded_a18_cigar_shaped_debiased], [t07_rounded_a18_cigar_shaped_flag], [t08_odd_feature_a19_ring_count], [t08_odd_feature_a19_ring_weight], [t08_odd_feature_a19_ring_fraction], [t08_odd_feature_a19_ring_weighted_fraction], [t08_odd_feature_a19_ring_debiased], [t08_odd_feature_a19_ring_flag], [t08_odd_feature_a20_lens_or_arc_count], [t08_odd_feature_a20_lens_or_arc_weight], [t08_odd_feature_a20_lens_or_arc_fraction], [t08_odd_feature_a20_lens_or_arc_weighted_fraction], [t08_odd_feature_a20_lens_or_arc_debiased], [t08_odd_feature_a20_lens_or_arc_flag], [t08_odd_feature_a21_disturbed_count], [t08_odd_feature_a21_disturbed_weight], [t08_odd_feature_a21_disturbed_fraction], [t08_odd_feature_a21_disturbed_weighted_fraction], [t08_odd_feature_a21_disturbed_debiased], [t08_odd_feature_a21_disturbed_flag], [t08_odd_feature_a22_irregular_count], [t08_odd_feature_a22_irregular_weight], [t08_odd_feature_a22_irregular_fraction], [t08_odd_feature_a22_irregular_weighted_fraction], [t08_odd_feature_a22_irregular_debiased], [t08_odd_feature_a22_irregular_flag], [t08_odd_feature_a23_other_count], [t08_odd_feature_a23_other_weight], [t08_odd_feature_a23_other_fraction], [t08_odd_feature_a23_other_weighted_fraction], [t08_odd_feature_a23_other_debiased], [t08_odd_feature_a23_other_flag], [t08_odd_feature_a24_merger_count], [t08_odd_feature_a24_merger_weight], [t08_odd_feature_a24_merger_fraction], [t08_odd_feature_a24_merger_weighted_fraction], [t08_odd_feature_a24_merger_debiased], [t08_odd_feature_a24_merger_flag], [t08_odd_feature_a38_dust_lane_count], [t08_odd_feature_a38_dust_lane_weight], [t08_odd_feature_a38_dust_lane_fraction], [t08_odd_feature_a38_dust_lane_weighted_fraction], [t08_odd_feature_a38_dust_lane_debiased], [t08_odd_feature_a38_dust_lane_flag], [t09_bulge_shape_a25_rounded_count], [t09_bulge_shape_a25_rounded_weight], [t09_bulge_shape_a25_rounded_fraction], [t09_bulge_shape_a25_rounded_weighted_fraction], [t09_bulge_shape_a25_rounded_debiased], [t09_bulge_shape_a25_rounded_flag], [t09_bulge_shape_a26_boxy_count], [t09_bulge_shape_a26_boxy_weight], [t09_bulge_shape_a26_boxy_fraction], [t09_bulge_shape_a26_boxy_weighted_fraction], [t09_bulge_shape_a26_boxy_debiased], [t09_bulge_shape_a26_boxy_flag], [t09_bulge_shape_a27_no_bulge_count], [t09_bulge_shape_a27_no_bulge_weight], [t09_bulge_shape_a27_no_bulge_fraction], [t09_bulge_shape_a27_no_bulge_weighted_fraction], [t09_bulge_shape_a27_no_bulge_debiased], [t09_bulge_shape_a27_no_bulge_flag], [t10_arms_winding_a28_tight_count], [t10_arms_winding_a28_tight_weight], [t10_arms_winding_a28_tight_fraction], [t10_arms_winding_a28_tight_weighted_fraction], [t10_arms_winding_a28_tight_debiased], [t10_arms_winding_a28_tight_flag], [t10_arms_winding_a29_medium_count], [t10_arms_winding_a29_medium_weight], [t10_arms_winding_a29_medium_fraction], [t10_arms_winding_a29_medium_weighted_fraction], [t10_arms_winding_a29_medium_debiased], [t10_arms_winding_a29_medium_flag], [t10_arms_winding_a30_loose_count], [t10_arms_winding_a30_loose_weight], [t10_arms_winding_a30_loose_fraction], [t10_arms_winding_a30_loose_weighted_fraction], [t10_arms_winding_a30_loose_debiased], [t10_arms_winding_a30_loose_flag], [t11_arms_number_a31_1_count], [t11_arms_number_a31_1_weight], [t11_arms_number_a31_1_fraction], [t11_arms_number_a31_1_weighted_fraction], [t11_arms_number_a31_1_debiased], [t11_arms_number_a31_1_flag], [t11_arms_number_a32_2_count], [t11_arms_number_a32_2_weight], [t11_arms_number_a32_2_fraction], [t11_arms_number_a32_2_weighted_fraction], [t11_arms_number_a32_2_debiased], [t11_arms_number_a32_2_flag], [t11_arms_number_a33_3_count], [t11_arms_number_a33_3_weight], [t11_arms_number_a33_3_fraction], [t11_arms_number_a33_3_weighted_fraction], [t11_arms_number_a33_3_debiased], [t11_arms_number_a33_3_flag], [t11_arms_number_a34_4_count], [t11_arms_number_a34_4_weight], [t11_arms_number_a34_4_fraction], [t11_arms_number_a34_4_weighted_fraction], [t11_arms_number_a34_4_debiased], [t11_arms_number_a34_4_flag], [t11_arms_number_a36_more_than_4_count], [t11_arms_number_a36_more_than_4_weight], [t11_arms_number_a36_more_than_4_fraction], [t11_arms_number_a36_more_than_4_weighted_fraction], [t11_arms_number_a36_more_than_4_debiased], [t11_arms_number_a36_more_than_4_flag], [t11_arms_number_a37_cant_tell_count], [t11_arms_number_a37_cant_tell_weight], [t11_arms_number_a37_cant_tell_fraction], [t11_arms_number_a37_cant_tell_weighted_fraction], [t11_arms_number_a37_cant_tell_debiased], [t11_arms_number_a37_cant_tell_flag])
 SELECT sourcetablealias.[dr8objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[decstring], sourcetablealias.[sample], sourcetablealias.[total_classifications], sourcetablealias.[total_votes], sourcetablealias.[t01_smooth_or_features_a01_smooth_count], sourcetablealias.[t01_smooth_or_features_a01_smooth_weight], sourcetablealias.[t01_smooth_or_features_a01_smooth_fraction], sourcetablealias.[t01_smooth_or_features_a01_smooth_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a01_smooth_debiased], sourcetablealias.[t01_smooth_or_features_a01_smooth_flag], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_count], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_weight], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_fraction], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_debiased], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_flag], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_count], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_weight], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_fraction], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_debiased], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_flag], sourcetablealias.[t02_edgeon_a04_yes_count], sourcetablealias.[t02_edgeon_a04_yes_weight], sourcetablealias.[t02_edgeon_a04_yes_fraction], sourcetablealias.[t02_edgeon_a04_yes_weighted_fraction], sourcetablealias.[t02_edgeon_a04_yes_debiased], sourcetablealias.[t02_edgeon_a04_yes_flag], sourcetablealias.[t02_edgeon_a05_no_count], sourcetablealias.[t02_edgeon_a05_no_weight], sourcetablealias.[t02_edgeon_a05_no_fraction], sourcetablealias.[t02_edgeon_a05_no_weighted_fraction], sourcetablealias.[t02_edgeon_a05_no_debiased], sourcetablealias.[t02_edgeon_a05_no_flag], sourcetablealias.[t03_bar_a06_bar_count], sourcetablealias.[t03_bar_a06_bar_weight], sourcetablealias.[t03_bar_a06_bar_fraction], sourcetablealias.[t03_bar_a06_bar_weighted_fraction], sourcetablealias.[t03_bar_a06_bar_debiased], sourcetablealias.[t03_bar_a06_bar_flag], sourcetablealias.[t03_bar_a07_no_bar_count], sourcetablealias.[t03_bar_a07_no_bar_weight], sourcetablealias.[t03_bar_a07_no_bar_fraction], sourcetablealias.[t03_bar_a07_no_bar_weighted_fraction], sourcetablealias.[t03_bar_a07_no_bar_debiased], sourcetablealias.[t03_bar_a07_no_bar_flag], sourcetablealias.[t04_spiral_a08_spiral_count], sourcetablealias.[t04_spiral_a08_spiral_weight], sourcetablealias.[t04_spiral_a08_spiral_fraction], sourcetablealias.[t04_spiral_a08_spiral_weighted_fraction], sourcetablealias.[t04_spiral_a08_spiral_debiased], sourcetablealias.[t04_spiral_a08_spiral_flag], sourcetablealias.[t04_spiral_a09_no_spiral_count], sourcetablealias.[t04_spiral_a09_no_spiral_weight], sourcetablealias.[t04_spiral_a09_no_spiral_fraction], sourcetablealias.[t04_spiral_a09_no_spiral_weighted_fraction], sourcetablealias.[t04_spiral_a09_no_spiral_debiased], sourcetablealias.[t04_spiral_a09_no_spiral_flag], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_count], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_weight], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_fraction], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_debiased], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_flag], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_count], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_weight], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_fraction], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_debiased], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_flag], sourcetablealias.[t05_bulge_prominence_a12_obvious_count], sourcetablealias.[t05_bulge_prominence_a12_obvious_weight], sourcetablealias.[t05_bulge_prominence_a12_obvious_fraction], sourcetablealias.[t05_bulge_prominence_a12_obvious_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a12_obvious_debiased], sourcetablealias.[t05_bulge_prominence_a12_obvious_flag], sourcetablealias.[t05_bulge_prominence_a13_dominant_count], sourcetablealias.[t05_bulge_prominence_a13_dominant_weight], sourcetablealias.[t05_bulge_prominence_a13_dominant_fraction], sourcetablealias.[t05_bulge_prominence_a13_dominant_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a13_dominant_debiased], sourcetablealias.[t05_bulge_prominence_a13_dominant_flag], sourcetablealias.[t06_odd_a14_yes_count], sourcetablealias.[t06_odd_a14_yes_weight], sourcetablealias.[t06_odd_a14_yes_fraction], sourcetablealias.[t06_odd_a14_yes_weighted_fraction], sourcetablealias.[t06_odd_a14_yes_debiased], sourcetablealias.[t06_odd_a14_yes_flag], sourcetablealias.[t06_odd_a15_no_count], sourcetablealias.[t06_odd_a15_no_weight], sourcetablealias.[t06_odd_a15_no_fraction], sourcetablealias.[t06_odd_a15_no_weighted_fraction], sourcetablealias.[t06_odd_a15_no_debiased], sourcetablealias.[t06_odd_a15_no_flag], sourcetablealias.[t07_rounded_a16_completely_round_count], sourcetablealias.[t07_rounded_a16_completely_round_weight], sourcetablealias.[t07_rounded_a16_completely_round_fraction], sourcetablealias.[t07_rounded_a16_completely_round_weighted_fraction], sourcetablealias.[t07_rounded_a16_completely_round_debiased], sourcetablealias.[t07_rounded_a16_completely_round_flag], sourcetablealias.[t07_rounded_a17_in_between_count], sourcetablealias.[t07_rounded_a17_in_between_weight], sourcetablealias.[t07_rounded_a17_in_between_fraction], sourcetablealias.[t07_rounded_a17_in_between_weighted_fraction], sourcetablealias.[t07_rounded_a17_in_between_debiased], sourcetablealias.[t07_rounded_a17_in_between_flag], sourcetablealias.[t07_rounded_a18_cigar_shaped_count], sourcetablealias.[t07_rounded_a18_cigar_shaped_weight], sourcetablealias.[t07_rounded_a18_cigar_shaped_fraction], sourcetablealias.[t07_rounded_a18_cigar_shaped_weighted_fraction], sourcetablealias.[t07_rounded_a18_cigar_shaped_debiased], sourcetablealias.[t07_rounded_a18_cigar_shaped_flag], sourcetablealias.[t08_odd_feature_a19_ring_count], sourcetablealias.[t08_odd_feature_a19_ring_weight], sourcetablealias.[t08_odd_feature_a19_ring_fraction], sourcetablealias.[t08_odd_feature_a19_ring_weighted_fraction], sourcetablealias.[t08_odd_feature_a19_ring_debiased], sourcetablealias.[t08_odd_feature_a19_ring_flag], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_count], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_weight], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_fraction], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_weighted_fraction], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_debiased], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_flag], sourcetablealias.[t08_odd_feature_a21_disturbed_count], sourcetablealias.[t08_odd_feature_a21_disturbed_weight], sourcetablealias.[t08_odd_feature_a21_disturbed_fraction], sourcetablealias.[t08_odd_feature_a21_disturbed_weighted_fraction], sourcetablealias.[t08_odd_feature_a21_disturbed_debiased], sourcetablealias.[t08_odd_feature_a21_disturbed_flag], sourcetablealias.[t08_odd_feature_a22_irregular_count], sourcetablealias.[t08_odd_feature_a22_irregular_weight], sourcetablealias.[t08_odd_feature_a22_irregular_fraction], sourcetablealias.[t08_odd_feature_a22_irregular_weighted_fraction], sourcetablealias.[t08_odd_feature_a22_irregular_debiased], sourcetablealias.[t08_odd_feature_a22_irregular_flag], sourcetablealias.[t08_odd_feature_a23_other_count], sourcetablealias.[t08_odd_feature_a23_other_weight], sourcetablealias.[t08_odd_feature_a23_other_fraction], sourcetablealias.[t08_odd_feature_a23_other_weighted_fraction], sourcetablealias.[t08_odd_feature_a23_other_debiased], sourcetablealias.[t08_odd_feature_a23_other_flag], sourcetablealias.[t08_odd_feature_a24_merger_count], sourcetablealias.[t08_odd_feature_a24_merger_weight], sourcetablealias.[t08_odd_feature_a24_merger_fraction], sourcetablealias.[t08_odd_feature_a24_merger_weighted_fraction], sourcetablealias.[t08_odd_feature_a24_merger_debiased], sourcetablealias.[t08_odd_feature_a24_merger_flag], sourcetablealias.[t08_odd_feature_a38_dust_lane_count], sourcetablealias.[t08_odd_feature_a38_dust_lane_weight], sourcetablealias.[t08_odd_feature_a38_dust_lane_fraction], sourcetablealias.[t08_odd_feature_a38_dust_lane_weighted_fraction], sourcetablealias.[t08_odd_feature_a38_dust_lane_debiased], sourcetablealias.[t08_odd_feature_a38_dust_lane_flag], sourcetablealias.[t09_bulge_shape_a25_rounded_count], sourcetablealias.[t09_bulge_shape_a25_rounded_weight], sourcetablealias.[t09_bulge_shape_a25_rounded_fraction], sourcetablealias.[t09_bulge_shape_a25_rounded_weighted_fraction], sourcetablealias.[t09_bulge_shape_a25_rounded_debiased], sourcetablealias.[t09_bulge_shape_a25_rounded_flag], sourcetablealias.[t09_bulge_shape_a26_boxy_count], sourcetablealias.[t09_bulge_shape_a26_boxy_weight], sourcetablealias.[t09_bulge_shape_a26_boxy_fraction], sourcetablealias.[t09_bulge_shape_a26_boxy_weighted_fraction], sourcetablealias.[t09_bulge_shape_a26_boxy_debiased], sourcetablealias.[t09_bulge_shape_a26_boxy_flag], sourcetablealias.[t09_bulge_shape_a27_no_bulge_count], sourcetablealias.[t09_bulge_shape_a27_no_bulge_weight], sourcetablealias.[t09_bulge_shape_a27_no_bulge_fraction], sourcetablealias.[t09_bulge_shape_a27_no_bulge_weighted_fraction], sourcetablealias.[t09_bulge_shape_a27_no_bulge_debiased], sourcetablealias.[t09_bulge_shape_a27_no_bulge_flag], sourcetablealias.[t10_arms_winding_a28_tight_count], sourcetablealias.[t10_arms_winding_a28_tight_weight], sourcetablealias.[t10_arms_winding_a28_tight_fraction], sourcetablealias.[t10_arms_winding_a28_tight_weighted_fraction], sourcetablealias.[t10_arms_winding_a28_tight_debiased], sourcetablealias.[t10_arms_winding_a28_tight_flag], sourcetablealias.[t10_arms_winding_a29_medium_count], sourcetablealias.[t10_arms_winding_a29_medium_weight], sourcetablealias.[t10_arms_winding_a29_medium_fraction], sourcetablealias.[t10_arms_winding_a29_medium_weighted_fraction], sourcetablealias.[t10_arms_winding_a29_medium_debiased], sourcetablealias.[t10_arms_winding_a29_medium_flag], sourcetablealias.[t10_arms_winding_a30_loose_count], sourcetablealias.[t10_arms_winding_a30_loose_weight], sourcetablealias.[t10_arms_winding_a30_loose_fraction], sourcetablealias.[t10_arms_winding_a30_loose_weighted_fraction], sourcetablealias.[t10_arms_winding_a30_loose_debiased], sourcetablealias.[t10_arms_winding_a30_loose_flag], sourcetablealias.[t11_arms_number_a31_1_count], sourcetablealias.[t11_arms_number_a31_1_weight], sourcetablealias.[t11_arms_number_a31_1_fraction], sourcetablealias.[t11_arms_number_a31_1_weighted_fraction], sourcetablealias.[t11_arms_number_a31_1_debiased], sourcetablealias.[t11_arms_number_a31_1_flag], sourcetablealias.[t11_arms_number_a32_2_count], sourcetablealias.[t11_arms_number_a32_2_weight], sourcetablealias.[t11_arms_number_a32_2_fraction], sourcetablealias.[t11_arms_number_a32_2_weighted_fraction], sourcetablealias.[t11_arms_number_a32_2_debiased], sourcetablealias.[t11_arms_number_a32_2_flag], sourcetablealias.[t11_arms_number_a33_3_count], sourcetablealias.[t11_arms_number_a33_3_weight], sourcetablealias.[t11_arms_number_a33_3_fraction], sourcetablealias.[t11_arms_number_a33_3_weighted_fraction], sourcetablealias.[t11_arms_number_a33_3_debiased], sourcetablealias.[t11_arms_number_a33_3_flag], sourcetablealias.[t11_arms_number_a34_4_count], sourcetablealias.[t11_arms_number_a34_4_weight], sourcetablealias.[t11_arms_number_a34_4_fraction], sourcetablealias.[t11_arms_number_a34_4_weighted_fraction], sourcetablealias.[t11_arms_number_a34_4_debiased], sourcetablealias.[t11_arms_number_a34_4_flag], sourcetablealias.[t11_arms_number_a36_more_than_4_count], sourcetablealias.[t11_arms_number_a36_more_than_4_weight], sourcetablealias.[t11_arms_number_a36_more_than_4_fraction], sourcetablealias.[t11_arms_number_a36_more_than_4_weighted_fraction], sourcetablealias.[t11_arms_number_a36_more_than_4_debiased], sourcetablealias.[t11_arms_number_a36_more_than_4_flag], sourcetablealias.[t11_arms_number_a37_cant_tell_count], sourcetablealias.[t11_arms_number_a37_cant_tell_weight], sourcetablealias.[t11_arms_number_a37_cant_tell_fraction], sourcetablealias.[t11_arms_number_a37_cant_tell_weighted_fraction], sourcetablealias.[t11_arms_number_a37_cant_tell_debiased], sourcetablealias.[t11_arms_number_a37_cant_tell_flag]
 FROM   [SkyNode_SDSSDR12].[dbo].[zoo2MainPhotoz] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.dr7objid = sourcetablealias.dr7objid
	;


GO

-- SUBSAMPLING TABLE 'zoo2MainSpecz' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[dr7objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[dr7objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zoo2MainSpecz] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [dr7objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zoo2MainSpecz] WITH (TABLOCKX)
	([specobjid], [dr8objid], [dr7objid], [ra], [dec], [rastring], [decstring], [sample], [total_classifications], [total_votes], [t01_smooth_or_features_a01_smooth_count], [t01_smooth_or_features_a01_smooth_weight], [t01_smooth_or_features_a01_smooth_fraction], [t01_smooth_or_features_a01_smooth_weighted_fraction], [t01_smooth_or_features_a01_smooth_debiased], [t01_smooth_or_features_a01_smooth_flag], [t01_smooth_or_features_a02_features_or_disk_count], [t01_smooth_or_features_a02_features_or_disk_weight], [t01_smooth_or_features_a02_features_or_disk_fraction], [t01_smooth_or_features_a02_features_or_disk_weighted_fraction], [t01_smooth_or_features_a02_features_or_disk_debiased], [t01_smooth_or_features_a02_features_or_disk_flag], [t01_smooth_or_features_a03_star_or_artifact_count], [t01_smooth_or_features_a03_star_or_artifact_weight], [t01_smooth_or_features_a03_star_or_artifact_fraction], [t01_smooth_or_features_a03_star_or_artifact_weighted_fraction], [t01_smooth_or_features_a03_star_or_artifact_debiased], [t01_smooth_or_features_a03_star_or_artifact_flag], [t02_edgeon_a04_yes_count], [t02_edgeon_a04_yes_weight], [t02_edgeon_a04_yes_fraction], [t02_edgeon_a04_yes_weighted_fraction], [t02_edgeon_a04_yes_debiased], [t02_edgeon_a04_yes_flag], [t02_edgeon_a05_no_count], [t02_edgeon_a05_no_weight], [t02_edgeon_a05_no_fraction], [t02_edgeon_a05_no_weighted_fraction], [t02_edgeon_a05_no_debiased], [t02_edgeon_a05_no_flag], [t03_bar_a06_bar_count], [t03_bar_a06_bar_weight], [t03_bar_a06_bar_fraction], [t03_bar_a06_bar_weighted_fraction], [t03_bar_a06_bar_debiased], [t03_bar_a06_bar_flag], [t03_bar_a07_no_bar_count], [t03_bar_a07_no_bar_weight], [t03_bar_a07_no_bar_fraction], [t03_bar_a07_no_bar_weighted_fraction], [t03_bar_a07_no_bar_debiased], [t03_bar_a07_no_bar_flag], [t04_spiral_a08_spiral_count], [t04_spiral_a08_spiral_weight], [t04_spiral_a08_spiral_fraction], [t04_spiral_a08_spiral_weighted_fraction], [t04_spiral_a08_spiral_debiased], [t04_spiral_a08_spiral_flag], [t04_spiral_a09_no_spiral_count], [t04_spiral_a09_no_spiral_weight], [t04_spiral_a09_no_spiral_fraction], [t04_spiral_a09_no_spiral_weighted_fraction], [t04_spiral_a09_no_spiral_debiased], [t04_spiral_a09_no_spiral_flag], [t05_bulge_prominence_a10_no_bulge_count], [t05_bulge_prominence_a10_no_bulge_weight], [t05_bulge_prominence_a10_no_bulge_fraction], [t05_bulge_prominence_a10_no_bulge_weighted_fraction], [t05_bulge_prominence_a10_no_bulge_debiased], [t05_bulge_prominence_a10_no_bulge_flag], [t05_bulge_prominence_a11_just_noticeable_count], [t05_bulge_prominence_a11_just_noticeable_weight], [t05_bulge_prominence_a11_just_noticeable_fraction], [t05_bulge_prominence_a11_just_noticeable_weighted_fraction], [t05_bulge_prominence_a11_just_noticeable_debiased], [t05_bulge_prominence_a11_just_noticeable_flag], [t05_bulge_prominence_a12_obvious_count], [t05_bulge_prominence_a12_obvious_weight], [t05_bulge_prominence_a12_obvious_fraction], [t05_bulge_prominence_a12_obvious_weighted_fraction], [t05_bulge_prominence_a12_obvious_debiased], [t05_bulge_prominence_a12_obvious_flag], [t05_bulge_prominence_a13_dominant_count], [t05_bulge_prominence_a13_dominant_weight], [t05_bulge_prominence_a13_dominant_fraction], [t05_bulge_prominence_a13_dominant_weighted_fraction], [t05_bulge_prominence_a13_dominant_debiased], [t05_bulge_prominence_a13_dominant_flag], [t06_odd_a14_yes_count], [t06_odd_a14_yes_weight], [t06_odd_a14_yes_fraction], [t06_odd_a14_yes_weighted_fraction], [t06_odd_a14_yes_debiased], [t06_odd_a14_yes_flag], [t06_odd_a15_no_count], [t06_odd_a15_no_weight], [t06_odd_a15_no_fraction], [t06_odd_a15_no_weighted_fraction], [t06_odd_a15_no_debiased], [t06_odd_a15_no_flag], [t07_rounded_a16_completely_round_count], [t07_rounded_a16_completely_round_weight], [t07_rounded_a16_completely_round_fraction], [t07_rounded_a16_completely_round_weighted_fraction], [t07_rounded_a16_completely_round_debiased], [t07_rounded_a16_completely_round_flag], [t07_rounded_a17_in_between_count], [t07_rounded_a17_in_between_weight], [t07_rounded_a17_in_between_fraction], [t07_rounded_a17_in_between_weighted_fraction], [t07_rounded_a17_in_between_debiased], [t07_rounded_a17_in_between_flag], [t07_rounded_a18_cigar_shaped_count], [t07_rounded_a18_cigar_shaped_weight], [t07_rounded_a18_cigar_shaped_fraction], [t07_rounded_a18_cigar_shaped_weighted_fraction], [t07_rounded_a18_cigar_shaped_debiased], [t07_rounded_a18_cigar_shaped_flag], [t08_odd_feature_a19_ring_count], [t08_odd_feature_a19_ring_weight], [t08_odd_feature_a19_ring_fraction], [t08_odd_feature_a19_ring_weighted_fraction], [t08_odd_feature_a19_ring_debiased], [t08_odd_feature_a19_ring_flag], [t08_odd_feature_a20_lens_or_arc_count], [t08_odd_feature_a20_lens_or_arc_weight], [t08_odd_feature_a20_lens_or_arc_fraction], [t08_odd_feature_a20_lens_or_arc_weighted_fraction], [t08_odd_feature_a20_lens_or_arc_debiased], [t08_odd_feature_a20_lens_or_arc_flag], [t08_odd_feature_a21_disturbed_count], [t08_odd_feature_a21_disturbed_weight], [t08_odd_feature_a21_disturbed_fraction], [t08_odd_feature_a21_disturbed_weighted_fraction], [t08_odd_feature_a21_disturbed_debiased], [t08_odd_feature_a21_disturbed_flag], [t08_odd_feature_a22_irregular_count], [t08_odd_feature_a22_irregular_weight], [t08_odd_feature_a22_irregular_fraction], [t08_odd_feature_a22_irregular_weighted_fraction], [t08_odd_feature_a22_irregular_debiased], [t08_odd_feature_a22_irregular_flag], [t08_odd_feature_a23_other_count], [t08_odd_feature_a23_other_weight], [t08_odd_feature_a23_other_fraction], [t08_odd_feature_a23_other_weighted_fraction], [t08_odd_feature_a23_other_debiased], [t08_odd_feature_a23_other_flag], [t08_odd_feature_a24_merger_count], [t08_odd_feature_a24_merger_weight], [t08_odd_feature_a24_merger_fraction], [t08_odd_feature_a24_merger_weighted_fraction], [t08_odd_feature_a24_merger_debiased], [t08_odd_feature_a24_merger_flag], [t08_odd_feature_a38_dust_lane_count], [t08_odd_feature_a38_dust_lane_weight], [t08_odd_feature_a38_dust_lane_fraction], [t08_odd_feature_a38_dust_lane_weighted_fraction], [t08_odd_feature_a38_dust_lane_debiased], [t08_odd_feature_a38_dust_lane_flag], [t09_bulge_shape_a25_rounded_count], [t09_bulge_shape_a25_rounded_weight], [t09_bulge_shape_a25_rounded_fraction], [t09_bulge_shape_a25_rounded_weighted_fraction], [t09_bulge_shape_a25_rounded_debiased], [t09_bulge_shape_a25_rounded_flag], [t09_bulge_shape_a26_boxy_count], [t09_bulge_shape_a26_boxy_weight], [t09_bulge_shape_a26_boxy_fraction], [t09_bulge_shape_a26_boxy_weighted_fraction], [t09_bulge_shape_a26_boxy_debiased], [t09_bulge_shape_a26_boxy_flag], [t09_bulge_shape_a27_no_bulge_count], [t09_bulge_shape_a27_no_bulge_weight], [t09_bulge_shape_a27_no_bulge_fraction], [t09_bulge_shape_a27_no_bulge_weighted_fraction], [t09_bulge_shape_a27_no_bulge_debiased], [t09_bulge_shape_a27_no_bulge_flag], [t10_arms_winding_a28_tight_count], [t10_arms_winding_a28_tight_weight], [t10_arms_winding_a28_tight_fraction], [t10_arms_winding_a28_tight_weighted_fraction], [t10_arms_winding_a28_tight_debiased], [t10_arms_winding_a28_tight_flag], [t10_arms_winding_a29_medium_count], [t10_arms_winding_a29_medium_weight], [t10_arms_winding_a29_medium_fraction], [t10_arms_winding_a29_medium_weighted_fraction], [t10_arms_winding_a29_medium_debiased], [t10_arms_winding_a29_medium_flag], [t10_arms_winding_a30_loose_count], [t10_arms_winding_a30_loose_weight], [t10_arms_winding_a30_loose_fraction], [t10_arms_winding_a30_loose_weighted_fraction], [t10_arms_winding_a30_loose_debiased], [t10_arms_winding_a30_loose_flag], [t11_arms_number_a31_1_count], [t11_arms_number_a31_1_weight], [t11_arms_number_a31_1_fraction], [t11_arms_number_a31_1_weighted_fraction], [t11_arms_number_a31_1_debiased], [t11_arms_number_a31_1_flag], [t11_arms_number_a32_2_count], [t11_arms_number_a32_2_weight], [t11_arms_number_a32_2_fraction], [t11_arms_number_a32_2_weighted_fraction], [t11_arms_number_a32_2_debiased], [t11_arms_number_a32_2_flag], [t11_arms_number_a33_3_count], [t11_arms_number_a33_3_weight], [t11_arms_number_a33_3_fraction], [t11_arms_number_a33_3_weighted_fraction], [t11_arms_number_a33_3_debiased], [t11_arms_number_a33_3_flag], [t11_arms_number_a34_4_count], [t11_arms_number_a34_4_weight], [t11_arms_number_a34_4_fraction], [t11_arms_number_a34_4_weighted_fraction], [t11_arms_number_a34_4_debiased], [t11_arms_number_a34_4_flag], [t11_arms_number_a36_more_than_4_count], [t11_arms_number_a36_more_than_4_weight], [t11_arms_number_a36_more_than_4_fraction], [t11_arms_number_a36_more_than_4_weighted_fraction], [t11_arms_number_a36_more_than_4_debiased], [t11_arms_number_a36_more_than_4_flag], [t11_arms_number_a37_cant_tell_count], [t11_arms_number_a37_cant_tell_weight], [t11_arms_number_a37_cant_tell_fraction], [t11_arms_number_a37_cant_tell_weighted_fraction], [t11_arms_number_a37_cant_tell_debiased], [t11_arms_number_a37_cant_tell_flag])
 SELECT sourcetablealias.[specobjid], sourcetablealias.[dr8objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[decstring], sourcetablealias.[sample], sourcetablealias.[total_classifications], sourcetablealias.[total_votes], sourcetablealias.[t01_smooth_or_features_a01_smooth_count], sourcetablealias.[t01_smooth_or_features_a01_smooth_weight], sourcetablealias.[t01_smooth_or_features_a01_smooth_fraction], sourcetablealias.[t01_smooth_or_features_a01_smooth_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a01_smooth_debiased], sourcetablealias.[t01_smooth_or_features_a01_smooth_flag], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_count], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_weight], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_fraction], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_debiased], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_flag], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_count], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_weight], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_fraction], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_debiased], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_flag], sourcetablealias.[t02_edgeon_a04_yes_count], sourcetablealias.[t02_edgeon_a04_yes_weight], sourcetablealias.[t02_edgeon_a04_yes_fraction], sourcetablealias.[t02_edgeon_a04_yes_weighted_fraction], sourcetablealias.[t02_edgeon_a04_yes_debiased], sourcetablealias.[t02_edgeon_a04_yes_flag], sourcetablealias.[t02_edgeon_a05_no_count], sourcetablealias.[t02_edgeon_a05_no_weight], sourcetablealias.[t02_edgeon_a05_no_fraction], sourcetablealias.[t02_edgeon_a05_no_weighted_fraction], sourcetablealias.[t02_edgeon_a05_no_debiased], sourcetablealias.[t02_edgeon_a05_no_flag], sourcetablealias.[t03_bar_a06_bar_count], sourcetablealias.[t03_bar_a06_bar_weight], sourcetablealias.[t03_bar_a06_bar_fraction], sourcetablealias.[t03_bar_a06_bar_weighted_fraction], sourcetablealias.[t03_bar_a06_bar_debiased], sourcetablealias.[t03_bar_a06_bar_flag], sourcetablealias.[t03_bar_a07_no_bar_count], sourcetablealias.[t03_bar_a07_no_bar_weight], sourcetablealias.[t03_bar_a07_no_bar_fraction], sourcetablealias.[t03_bar_a07_no_bar_weighted_fraction], sourcetablealias.[t03_bar_a07_no_bar_debiased], sourcetablealias.[t03_bar_a07_no_bar_flag], sourcetablealias.[t04_spiral_a08_spiral_count], sourcetablealias.[t04_spiral_a08_spiral_weight], sourcetablealias.[t04_spiral_a08_spiral_fraction], sourcetablealias.[t04_spiral_a08_spiral_weighted_fraction], sourcetablealias.[t04_spiral_a08_spiral_debiased], sourcetablealias.[t04_spiral_a08_spiral_flag], sourcetablealias.[t04_spiral_a09_no_spiral_count], sourcetablealias.[t04_spiral_a09_no_spiral_weight], sourcetablealias.[t04_spiral_a09_no_spiral_fraction], sourcetablealias.[t04_spiral_a09_no_spiral_weighted_fraction], sourcetablealias.[t04_spiral_a09_no_spiral_debiased], sourcetablealias.[t04_spiral_a09_no_spiral_flag], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_count], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_weight], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_fraction], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_debiased], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_flag], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_count], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_weight], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_fraction], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_debiased], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_flag], sourcetablealias.[t05_bulge_prominence_a12_obvious_count], sourcetablealias.[t05_bulge_prominence_a12_obvious_weight], sourcetablealias.[t05_bulge_prominence_a12_obvious_fraction], sourcetablealias.[t05_bulge_prominence_a12_obvious_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a12_obvious_debiased], sourcetablealias.[t05_bulge_prominence_a12_obvious_flag], sourcetablealias.[t05_bulge_prominence_a13_dominant_count], sourcetablealias.[t05_bulge_prominence_a13_dominant_weight], sourcetablealias.[t05_bulge_prominence_a13_dominant_fraction], sourcetablealias.[t05_bulge_prominence_a13_dominant_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a13_dominant_debiased], sourcetablealias.[t05_bulge_prominence_a13_dominant_flag], sourcetablealias.[t06_odd_a14_yes_count], sourcetablealias.[t06_odd_a14_yes_weight], sourcetablealias.[t06_odd_a14_yes_fraction], sourcetablealias.[t06_odd_a14_yes_weighted_fraction], sourcetablealias.[t06_odd_a14_yes_debiased], sourcetablealias.[t06_odd_a14_yes_flag], sourcetablealias.[t06_odd_a15_no_count], sourcetablealias.[t06_odd_a15_no_weight], sourcetablealias.[t06_odd_a15_no_fraction], sourcetablealias.[t06_odd_a15_no_weighted_fraction], sourcetablealias.[t06_odd_a15_no_debiased], sourcetablealias.[t06_odd_a15_no_flag], sourcetablealias.[t07_rounded_a16_completely_round_count], sourcetablealias.[t07_rounded_a16_completely_round_weight], sourcetablealias.[t07_rounded_a16_completely_round_fraction], sourcetablealias.[t07_rounded_a16_completely_round_weighted_fraction], sourcetablealias.[t07_rounded_a16_completely_round_debiased], sourcetablealias.[t07_rounded_a16_completely_round_flag], sourcetablealias.[t07_rounded_a17_in_between_count], sourcetablealias.[t07_rounded_a17_in_between_weight], sourcetablealias.[t07_rounded_a17_in_between_fraction], sourcetablealias.[t07_rounded_a17_in_between_weighted_fraction], sourcetablealias.[t07_rounded_a17_in_between_debiased], sourcetablealias.[t07_rounded_a17_in_between_flag], sourcetablealias.[t07_rounded_a18_cigar_shaped_count], sourcetablealias.[t07_rounded_a18_cigar_shaped_weight], sourcetablealias.[t07_rounded_a18_cigar_shaped_fraction], sourcetablealias.[t07_rounded_a18_cigar_shaped_weighted_fraction], sourcetablealias.[t07_rounded_a18_cigar_shaped_debiased], sourcetablealias.[t07_rounded_a18_cigar_shaped_flag], sourcetablealias.[t08_odd_feature_a19_ring_count], sourcetablealias.[t08_odd_feature_a19_ring_weight], sourcetablealias.[t08_odd_feature_a19_ring_fraction], sourcetablealias.[t08_odd_feature_a19_ring_weighted_fraction], sourcetablealias.[t08_odd_feature_a19_ring_debiased], sourcetablealias.[t08_odd_feature_a19_ring_flag], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_count], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_weight], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_fraction], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_weighted_fraction], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_debiased], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_flag], sourcetablealias.[t08_odd_feature_a21_disturbed_count], sourcetablealias.[t08_odd_feature_a21_disturbed_weight], sourcetablealias.[t08_odd_feature_a21_disturbed_fraction], sourcetablealias.[t08_odd_feature_a21_disturbed_weighted_fraction], sourcetablealias.[t08_odd_feature_a21_disturbed_debiased], sourcetablealias.[t08_odd_feature_a21_disturbed_flag], sourcetablealias.[t08_odd_feature_a22_irregular_count], sourcetablealias.[t08_odd_feature_a22_irregular_weight], sourcetablealias.[t08_odd_feature_a22_irregular_fraction], sourcetablealias.[t08_odd_feature_a22_irregular_weighted_fraction], sourcetablealias.[t08_odd_feature_a22_irregular_debiased], sourcetablealias.[t08_odd_feature_a22_irregular_flag], sourcetablealias.[t08_odd_feature_a23_other_count], sourcetablealias.[t08_odd_feature_a23_other_weight], sourcetablealias.[t08_odd_feature_a23_other_fraction], sourcetablealias.[t08_odd_feature_a23_other_weighted_fraction], sourcetablealias.[t08_odd_feature_a23_other_debiased], sourcetablealias.[t08_odd_feature_a23_other_flag], sourcetablealias.[t08_odd_feature_a24_merger_count], sourcetablealias.[t08_odd_feature_a24_merger_weight], sourcetablealias.[t08_odd_feature_a24_merger_fraction], sourcetablealias.[t08_odd_feature_a24_merger_weighted_fraction], sourcetablealias.[t08_odd_feature_a24_merger_debiased], sourcetablealias.[t08_odd_feature_a24_merger_flag], sourcetablealias.[t08_odd_feature_a38_dust_lane_count], sourcetablealias.[t08_odd_feature_a38_dust_lane_weight], sourcetablealias.[t08_odd_feature_a38_dust_lane_fraction], sourcetablealias.[t08_odd_feature_a38_dust_lane_weighted_fraction], sourcetablealias.[t08_odd_feature_a38_dust_lane_debiased], sourcetablealias.[t08_odd_feature_a38_dust_lane_flag], sourcetablealias.[t09_bulge_shape_a25_rounded_count], sourcetablealias.[t09_bulge_shape_a25_rounded_weight], sourcetablealias.[t09_bulge_shape_a25_rounded_fraction], sourcetablealias.[t09_bulge_shape_a25_rounded_weighted_fraction], sourcetablealias.[t09_bulge_shape_a25_rounded_debiased], sourcetablealias.[t09_bulge_shape_a25_rounded_flag], sourcetablealias.[t09_bulge_shape_a26_boxy_count], sourcetablealias.[t09_bulge_shape_a26_boxy_weight], sourcetablealias.[t09_bulge_shape_a26_boxy_fraction], sourcetablealias.[t09_bulge_shape_a26_boxy_weighted_fraction], sourcetablealias.[t09_bulge_shape_a26_boxy_debiased], sourcetablealias.[t09_bulge_shape_a26_boxy_flag], sourcetablealias.[t09_bulge_shape_a27_no_bulge_count], sourcetablealias.[t09_bulge_shape_a27_no_bulge_weight], sourcetablealias.[t09_bulge_shape_a27_no_bulge_fraction], sourcetablealias.[t09_bulge_shape_a27_no_bulge_weighted_fraction], sourcetablealias.[t09_bulge_shape_a27_no_bulge_debiased], sourcetablealias.[t09_bulge_shape_a27_no_bulge_flag], sourcetablealias.[t10_arms_winding_a28_tight_count], sourcetablealias.[t10_arms_winding_a28_tight_weight], sourcetablealias.[t10_arms_winding_a28_tight_fraction], sourcetablealias.[t10_arms_winding_a28_tight_weighted_fraction], sourcetablealias.[t10_arms_winding_a28_tight_debiased], sourcetablealias.[t10_arms_winding_a28_tight_flag], sourcetablealias.[t10_arms_winding_a29_medium_count], sourcetablealias.[t10_arms_winding_a29_medium_weight], sourcetablealias.[t10_arms_winding_a29_medium_fraction], sourcetablealias.[t10_arms_winding_a29_medium_weighted_fraction], sourcetablealias.[t10_arms_winding_a29_medium_debiased], sourcetablealias.[t10_arms_winding_a29_medium_flag], sourcetablealias.[t10_arms_winding_a30_loose_count], sourcetablealias.[t10_arms_winding_a30_loose_weight], sourcetablealias.[t10_arms_winding_a30_loose_fraction], sourcetablealias.[t10_arms_winding_a30_loose_weighted_fraction], sourcetablealias.[t10_arms_winding_a30_loose_debiased], sourcetablealias.[t10_arms_winding_a30_loose_flag], sourcetablealias.[t11_arms_number_a31_1_count], sourcetablealias.[t11_arms_number_a31_1_weight], sourcetablealias.[t11_arms_number_a31_1_fraction], sourcetablealias.[t11_arms_number_a31_1_weighted_fraction], sourcetablealias.[t11_arms_number_a31_1_debiased], sourcetablealias.[t11_arms_number_a31_1_flag], sourcetablealias.[t11_arms_number_a32_2_count], sourcetablealias.[t11_arms_number_a32_2_weight], sourcetablealias.[t11_arms_number_a32_2_fraction], sourcetablealias.[t11_arms_number_a32_2_weighted_fraction], sourcetablealias.[t11_arms_number_a32_2_debiased], sourcetablealias.[t11_arms_number_a32_2_flag], sourcetablealias.[t11_arms_number_a33_3_count], sourcetablealias.[t11_arms_number_a33_3_weight], sourcetablealias.[t11_arms_number_a33_3_fraction], sourcetablealias.[t11_arms_number_a33_3_weighted_fraction], sourcetablealias.[t11_arms_number_a33_3_debiased], sourcetablealias.[t11_arms_number_a33_3_flag], sourcetablealias.[t11_arms_number_a34_4_count], sourcetablealias.[t11_arms_number_a34_4_weight], sourcetablealias.[t11_arms_number_a34_4_fraction], sourcetablealias.[t11_arms_number_a34_4_weighted_fraction], sourcetablealias.[t11_arms_number_a34_4_debiased], sourcetablealias.[t11_arms_number_a34_4_flag], sourcetablealias.[t11_arms_number_a36_more_than_4_count], sourcetablealias.[t11_arms_number_a36_more_than_4_weight], sourcetablealias.[t11_arms_number_a36_more_than_4_fraction], sourcetablealias.[t11_arms_number_a36_more_than_4_weighted_fraction], sourcetablealias.[t11_arms_number_a36_more_than_4_debiased], sourcetablealias.[t11_arms_number_a36_more_than_4_flag], sourcetablealias.[t11_arms_number_a37_cant_tell_count], sourcetablealias.[t11_arms_number_a37_cant_tell_weight], sourcetablealias.[t11_arms_number_a37_cant_tell_fraction], sourcetablealias.[t11_arms_number_a37_cant_tell_weighted_fraction], sourcetablealias.[t11_arms_number_a37_cant_tell_debiased], sourcetablealias.[t11_arms_number_a37_cant_tell_flag]
 FROM   [SkyNode_SDSSDR12].[dbo].[zoo2MainSpecz] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.dr7objid = sourcetablealias.dr7objid
	;


GO

-- SUBSAMPLING TABLE 'zoo2Stripe82Coadd1' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[stripe82objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[stripe82objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zoo2Stripe82Coadd1] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [stripe82objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zoo2Stripe82Coadd1] WITH (TABLOCKX)
	([specobjid], [stripe82objid], [dr8objid], [dr7objid], [ra], [dec], [rastring], [decstring], [sample], [total_classifications], [total_votes], [t01_smooth_or_features_a01_smooth_count], [t01_smooth_or_features_a01_smooth_weight], [t01_smooth_or_features_a01_smooth_fraction], [t01_smooth_or_features_a01_smooth_weighted_fraction], [t01_smooth_or_features_a01_smooth_debiased], [t01_smooth_or_features_a01_smooth_flag], [t01_smooth_or_features_a02_features_or_disk_count], [t01_smooth_or_features_a02_features_or_disk_weight], [t01_smooth_or_features_a02_features_or_disk_fraction], [t01_smooth_or_features_a02_features_or_disk_weighted_fraction], [t01_smooth_or_features_a02_features_or_disk_debiased], [t01_smooth_or_features_a02_features_or_disk_flag], [t01_smooth_or_features_a03_star_or_artifact_count], [t01_smooth_or_features_a03_star_or_artifact_weight], [t01_smooth_or_features_a03_star_or_artifact_fraction], [t01_smooth_or_features_a03_star_or_artifact_weighted_fraction], [t01_smooth_or_features_a03_star_or_artifact_debiased], [t01_smooth_or_features_a03_star_or_artifact_flag], [t02_edgeon_a04_yes_count], [t02_edgeon_a04_yes_weight], [t02_edgeon_a04_yes_fraction], [t02_edgeon_a04_yes_weighted_fraction], [t02_edgeon_a04_yes_debiased], [t02_edgeon_a04_yes_flag], [t02_edgeon_a05_no_count], [t02_edgeon_a05_no_weight], [t02_edgeon_a05_no_fraction], [t02_edgeon_a05_no_weighted_fraction], [t02_edgeon_a05_no_debiased], [t02_edgeon_a05_no_flag], [t03_bar_a06_bar_count], [t03_bar_a06_bar_weight], [t03_bar_a06_bar_fraction], [t03_bar_a06_bar_weighted_fraction], [t03_bar_a06_bar_debiased], [t03_bar_a06_bar_flag], [t03_bar_a07_no_bar_count], [t03_bar_a07_no_bar_weight], [t03_bar_a07_no_bar_fraction], [t03_bar_a07_no_bar_weighted_fraction], [t03_bar_a07_no_bar_debiased], [t03_bar_a07_no_bar_flag], [t04_spiral_a08_spiral_count], [t04_spiral_a08_spiral_weight], [t04_spiral_a08_spiral_fraction], [t04_spiral_a08_spiral_weighted_fraction], [t04_spiral_a08_spiral_debiased], [t04_spiral_a08_spiral_flag], [t04_spiral_a09_no_spiral_count], [t04_spiral_a09_no_spiral_weight], [t04_spiral_a09_no_spiral_fraction], [t04_spiral_a09_no_spiral_weighted_fraction], [t04_spiral_a09_no_spiral_debiased], [t04_spiral_a09_no_spiral_flag], [t05_bulge_prominence_a10_no_bulge_count], [t05_bulge_prominence_a10_no_bulge_weight], [t05_bulge_prominence_a10_no_bulge_fraction], [t05_bulge_prominence_a10_no_bulge_weighted_fraction], [t05_bulge_prominence_a10_no_bulge_debiased], [t05_bulge_prominence_a10_no_bulge_flag], [t05_bulge_prominence_a11_just_noticeable_count], [t05_bulge_prominence_a11_just_noticeable_weight], [t05_bulge_prominence_a11_just_noticeable_fraction], [t05_bulge_prominence_a11_just_noticeable_weighted_fraction], [t05_bulge_prominence_a11_just_noticeable_debiased], [t05_bulge_prominence_a11_just_noticeable_flag], [t05_bulge_prominence_a12_obvious_count], [t05_bulge_prominence_a12_obvious_weight], [t05_bulge_prominence_a12_obvious_fraction], [t05_bulge_prominence_a12_obvious_weighted_fraction], [t05_bulge_prominence_a12_obvious_debiased], [t05_bulge_prominence_a12_obvious_flag], [t05_bulge_prominence_a13_dominant_count], [t05_bulge_prominence_a13_dominant_weight], [t05_bulge_prominence_a13_dominant_fraction], [t05_bulge_prominence_a13_dominant_weighted_fraction], [t05_bulge_prominence_a13_dominant_debiased], [t05_bulge_prominence_a13_dominant_flag], [t06_odd_a14_yes_count], [t06_odd_a14_yes_weight], [t06_odd_a14_yes_fraction], [t06_odd_a14_yes_weighted_fraction], [t06_odd_a14_yes_debiased], [t06_odd_a14_yes_flag], [t06_odd_a15_no_count], [t06_odd_a15_no_weight], [t06_odd_a15_no_fraction], [t06_odd_a15_no_weighted_fraction], [t06_odd_a15_no_debiased], [t06_odd_a15_no_flag], [t07_rounded_a16_completely_round_count], [t07_rounded_a16_completely_round_weight], [t07_rounded_a16_completely_round_fraction], [t07_rounded_a16_completely_round_weighted_fraction], [t07_rounded_a16_completely_round_debiased], [t07_rounded_a16_completely_round_flag], [t07_rounded_a17_in_between_count], [t07_rounded_a17_in_between_weight], [t07_rounded_a17_in_between_fraction], [t07_rounded_a17_in_between_weighted_fraction], [t07_rounded_a17_in_between_debiased], [t07_rounded_a17_in_between_flag], [t07_rounded_a18_cigar_shaped_count], [t07_rounded_a18_cigar_shaped_weight], [t07_rounded_a18_cigar_shaped_fraction], [t07_rounded_a18_cigar_shaped_weighted_fraction], [t07_rounded_a18_cigar_shaped_debiased], [t07_rounded_a18_cigar_shaped_flag], [t08_odd_feature_a19_ring_count], [t08_odd_feature_a19_ring_weight], [t08_odd_feature_a19_ring_fraction], [t08_odd_feature_a19_ring_weighted_fraction], [t08_odd_feature_a19_ring_debiased], [t08_odd_feature_a19_ring_flag], [t08_odd_feature_a20_lens_or_arc_count], [t08_odd_feature_a20_lens_or_arc_weight], [t08_odd_feature_a20_lens_or_arc_fraction], [t08_odd_feature_a20_lens_or_arc_weighted_fraction], [t08_odd_feature_a20_lens_or_arc_debiased], [t08_odd_feature_a20_lens_or_arc_flag], [t08_odd_feature_a21_disturbed_count], [t08_odd_feature_a21_disturbed_weight], [t08_odd_feature_a21_disturbed_fraction], [t08_odd_feature_a21_disturbed_weighted_fraction], [t08_odd_feature_a21_disturbed_debiased], [t08_odd_feature_a21_disturbed_flag], [t08_odd_feature_a22_irregular_count], [t08_odd_feature_a22_irregular_weight], [t08_odd_feature_a22_irregular_fraction], [t08_odd_feature_a22_irregular_weighted_fraction], [t08_odd_feature_a22_irregular_debiased], [t08_odd_feature_a22_irregular_flag], [t08_odd_feature_a23_other_count], [t08_odd_feature_a23_other_weight], [t08_odd_feature_a23_other_fraction], [t08_odd_feature_a23_other_weighted_fraction], [t08_odd_feature_a23_other_debiased], [t08_odd_feature_a23_other_flag], [t08_odd_feature_a24_merger_count], [t08_odd_feature_a24_merger_weight], [t08_odd_feature_a24_merger_fraction], [t08_odd_feature_a24_merger_weighted_fraction], [t08_odd_feature_a24_merger_debiased], [t08_odd_feature_a24_merger_flag], [t08_odd_feature_a38_dust_lane_count], [t08_odd_feature_a38_dust_lane_weight], [t08_odd_feature_a38_dust_lane_fraction], [t08_odd_feature_a38_dust_lane_weighted_fraction], [t08_odd_feature_a38_dust_lane_debiased], [t08_odd_feature_a38_dust_lane_flag], [t09_bulge_shape_a25_rounded_count], [t09_bulge_shape_a25_rounded_weight], [t09_bulge_shape_a25_rounded_fraction], [t09_bulge_shape_a25_rounded_weighted_fraction], [t09_bulge_shape_a25_rounded_debiased], [t09_bulge_shape_a25_rounded_flag], [t09_bulge_shape_a26_boxy_count], [t09_bulge_shape_a26_boxy_weight], [t09_bulge_shape_a26_boxy_fraction], [t09_bulge_shape_a26_boxy_weighted_fraction], [t09_bulge_shape_a26_boxy_debiased], [t09_bulge_shape_a26_boxy_flag], [t09_bulge_shape_a27_no_bulge_count], [t09_bulge_shape_a27_no_bulge_weight], [t09_bulge_shape_a27_no_bulge_fraction], [t09_bulge_shape_a27_no_bulge_weighted_fraction], [t09_bulge_shape_a27_no_bulge_debiased], [t09_bulge_shape_a27_no_bulge_flag], [t10_arms_winding_a28_tight_count], [t10_arms_winding_a28_tight_weight], [t10_arms_winding_a28_tight_fraction], [t10_arms_winding_a28_tight_weighted_fraction], [t10_arms_winding_a28_tight_debiased], [t10_arms_winding_a28_tight_flag], [t10_arms_winding_a29_medium_count], [t10_arms_winding_a29_medium_weight], [t10_arms_winding_a29_medium_fraction], [t10_arms_winding_a29_medium_weighted_fraction], [t10_arms_winding_a29_medium_debiased], [t10_arms_winding_a29_medium_flag], [t10_arms_winding_a30_loose_count], [t10_arms_winding_a30_loose_weight], [t10_arms_winding_a30_loose_fraction], [t10_arms_winding_a30_loose_weighted_fraction], [t10_arms_winding_a30_loose_debiased], [t10_arms_winding_a30_loose_flag], [t11_arms_number_a31_1_count], [t11_arms_number_a31_1_weight], [t11_arms_number_a31_1_fraction], [t11_arms_number_a31_1_weighted_fraction], [t11_arms_number_a31_1_debiased], [t11_arms_number_a31_1_flag], [t11_arms_number_a32_2_count], [t11_arms_number_a32_2_weight], [t11_arms_number_a32_2_fraction], [t11_arms_number_a32_2_weighted_fraction], [t11_arms_number_a32_2_debiased], [t11_arms_number_a32_2_flag], [t11_arms_number_a33_3_count], [t11_arms_number_a33_3_weight], [t11_arms_number_a33_3_fraction], [t11_arms_number_a33_3_weighted_fraction], [t11_arms_number_a33_3_debiased], [t11_arms_number_a33_3_flag], [t11_arms_number_a34_4_count], [t11_arms_number_a34_4_weight], [t11_arms_number_a34_4_fraction], [t11_arms_number_a34_4_weighted_fraction], [t11_arms_number_a34_4_debiased], [t11_arms_number_a34_4_flag], [t11_arms_number_a36_more_than_4_count], [t11_arms_number_a36_more_than_4_weight], [t11_arms_number_a36_more_than_4_fraction], [t11_arms_number_a36_more_than_4_weighted_fraction], [t11_arms_number_a36_more_than_4_debiased], [t11_arms_number_a36_more_than_4_flag], [t11_arms_number_a37_cant_tell_count], [t11_arms_number_a37_cant_tell_weight], [t11_arms_number_a37_cant_tell_fraction], [t11_arms_number_a37_cant_tell_weighted_fraction], [t11_arms_number_a37_cant_tell_debiased], [t11_arms_number_a37_cant_tell_flag])
 SELECT sourcetablealias.[specobjid], sourcetablealias.[stripe82objid], sourcetablealias.[dr8objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[decstring], sourcetablealias.[sample], sourcetablealias.[total_classifications], sourcetablealias.[total_votes], sourcetablealias.[t01_smooth_or_features_a01_smooth_count], sourcetablealias.[t01_smooth_or_features_a01_smooth_weight], sourcetablealias.[t01_smooth_or_features_a01_smooth_fraction], sourcetablealias.[t01_smooth_or_features_a01_smooth_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a01_smooth_debiased], sourcetablealias.[t01_smooth_or_features_a01_smooth_flag], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_count], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_weight], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_fraction], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_debiased], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_flag], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_count], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_weight], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_fraction], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_debiased], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_flag], sourcetablealias.[t02_edgeon_a04_yes_count], sourcetablealias.[t02_edgeon_a04_yes_weight], sourcetablealias.[t02_edgeon_a04_yes_fraction], sourcetablealias.[t02_edgeon_a04_yes_weighted_fraction], sourcetablealias.[t02_edgeon_a04_yes_debiased], sourcetablealias.[t02_edgeon_a04_yes_flag], sourcetablealias.[t02_edgeon_a05_no_count], sourcetablealias.[t02_edgeon_a05_no_weight], sourcetablealias.[t02_edgeon_a05_no_fraction], sourcetablealias.[t02_edgeon_a05_no_weighted_fraction], sourcetablealias.[t02_edgeon_a05_no_debiased], sourcetablealias.[t02_edgeon_a05_no_flag], sourcetablealias.[t03_bar_a06_bar_count], sourcetablealias.[t03_bar_a06_bar_weight], sourcetablealias.[t03_bar_a06_bar_fraction], sourcetablealias.[t03_bar_a06_bar_weighted_fraction], sourcetablealias.[t03_bar_a06_bar_debiased], sourcetablealias.[t03_bar_a06_bar_flag], sourcetablealias.[t03_bar_a07_no_bar_count], sourcetablealias.[t03_bar_a07_no_bar_weight], sourcetablealias.[t03_bar_a07_no_bar_fraction], sourcetablealias.[t03_bar_a07_no_bar_weighted_fraction], sourcetablealias.[t03_bar_a07_no_bar_debiased], sourcetablealias.[t03_bar_a07_no_bar_flag], sourcetablealias.[t04_spiral_a08_spiral_count], sourcetablealias.[t04_spiral_a08_spiral_weight], sourcetablealias.[t04_spiral_a08_spiral_fraction], sourcetablealias.[t04_spiral_a08_spiral_weighted_fraction], sourcetablealias.[t04_spiral_a08_spiral_debiased], sourcetablealias.[t04_spiral_a08_spiral_flag], sourcetablealias.[t04_spiral_a09_no_spiral_count], sourcetablealias.[t04_spiral_a09_no_spiral_weight], sourcetablealias.[t04_spiral_a09_no_spiral_fraction], sourcetablealias.[t04_spiral_a09_no_spiral_weighted_fraction], sourcetablealias.[t04_spiral_a09_no_spiral_debiased], sourcetablealias.[t04_spiral_a09_no_spiral_flag], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_count], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_weight], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_fraction], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_debiased], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_flag], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_count], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_weight], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_fraction], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_debiased], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_flag], sourcetablealias.[t05_bulge_prominence_a12_obvious_count], sourcetablealias.[t05_bulge_prominence_a12_obvious_weight], sourcetablealias.[t05_bulge_prominence_a12_obvious_fraction], sourcetablealias.[t05_bulge_prominence_a12_obvious_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a12_obvious_debiased], sourcetablealias.[t05_bulge_prominence_a12_obvious_flag], sourcetablealias.[t05_bulge_prominence_a13_dominant_count], sourcetablealias.[t05_bulge_prominence_a13_dominant_weight], sourcetablealias.[t05_bulge_prominence_a13_dominant_fraction], sourcetablealias.[t05_bulge_prominence_a13_dominant_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a13_dominant_debiased], sourcetablealias.[t05_bulge_prominence_a13_dominant_flag], sourcetablealias.[t06_odd_a14_yes_count], sourcetablealias.[t06_odd_a14_yes_weight], sourcetablealias.[t06_odd_a14_yes_fraction], sourcetablealias.[t06_odd_a14_yes_weighted_fraction], sourcetablealias.[t06_odd_a14_yes_debiased], sourcetablealias.[t06_odd_a14_yes_flag], sourcetablealias.[t06_odd_a15_no_count], sourcetablealias.[t06_odd_a15_no_weight], sourcetablealias.[t06_odd_a15_no_fraction], sourcetablealias.[t06_odd_a15_no_weighted_fraction], sourcetablealias.[t06_odd_a15_no_debiased], sourcetablealias.[t06_odd_a15_no_flag], sourcetablealias.[t07_rounded_a16_completely_round_count], sourcetablealias.[t07_rounded_a16_completely_round_weight], sourcetablealias.[t07_rounded_a16_completely_round_fraction], sourcetablealias.[t07_rounded_a16_completely_round_weighted_fraction], sourcetablealias.[t07_rounded_a16_completely_round_debiased], sourcetablealias.[t07_rounded_a16_completely_round_flag], sourcetablealias.[t07_rounded_a17_in_between_count], sourcetablealias.[t07_rounded_a17_in_between_weight], sourcetablealias.[t07_rounded_a17_in_between_fraction], sourcetablealias.[t07_rounded_a17_in_between_weighted_fraction], sourcetablealias.[t07_rounded_a17_in_between_debiased], sourcetablealias.[t07_rounded_a17_in_between_flag], sourcetablealias.[t07_rounded_a18_cigar_shaped_count], sourcetablealias.[t07_rounded_a18_cigar_shaped_weight], sourcetablealias.[t07_rounded_a18_cigar_shaped_fraction], sourcetablealias.[t07_rounded_a18_cigar_shaped_weighted_fraction], sourcetablealias.[t07_rounded_a18_cigar_shaped_debiased], sourcetablealias.[t07_rounded_a18_cigar_shaped_flag], sourcetablealias.[t08_odd_feature_a19_ring_count], sourcetablealias.[t08_odd_feature_a19_ring_weight], sourcetablealias.[t08_odd_feature_a19_ring_fraction], sourcetablealias.[t08_odd_feature_a19_ring_weighted_fraction], sourcetablealias.[t08_odd_feature_a19_ring_debiased], sourcetablealias.[t08_odd_feature_a19_ring_flag], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_count], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_weight], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_fraction], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_weighted_fraction], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_debiased], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_flag], sourcetablealias.[t08_odd_feature_a21_disturbed_count], sourcetablealias.[t08_odd_feature_a21_disturbed_weight], sourcetablealias.[t08_odd_feature_a21_disturbed_fraction], sourcetablealias.[t08_odd_feature_a21_disturbed_weighted_fraction], sourcetablealias.[t08_odd_feature_a21_disturbed_debiased], sourcetablealias.[t08_odd_feature_a21_disturbed_flag], sourcetablealias.[t08_odd_feature_a22_irregular_count], sourcetablealias.[t08_odd_feature_a22_irregular_weight], sourcetablealias.[t08_odd_feature_a22_irregular_fraction], sourcetablealias.[t08_odd_feature_a22_irregular_weighted_fraction], sourcetablealias.[t08_odd_feature_a22_irregular_debiased], sourcetablealias.[t08_odd_feature_a22_irregular_flag], sourcetablealias.[t08_odd_feature_a23_other_count], sourcetablealias.[t08_odd_feature_a23_other_weight], sourcetablealias.[t08_odd_feature_a23_other_fraction], sourcetablealias.[t08_odd_feature_a23_other_weighted_fraction], sourcetablealias.[t08_odd_feature_a23_other_debiased], sourcetablealias.[t08_odd_feature_a23_other_flag], sourcetablealias.[t08_odd_feature_a24_merger_count], sourcetablealias.[t08_odd_feature_a24_merger_weight], sourcetablealias.[t08_odd_feature_a24_merger_fraction], sourcetablealias.[t08_odd_feature_a24_merger_weighted_fraction], sourcetablealias.[t08_odd_feature_a24_merger_debiased], sourcetablealias.[t08_odd_feature_a24_merger_flag], sourcetablealias.[t08_odd_feature_a38_dust_lane_count], sourcetablealias.[t08_odd_feature_a38_dust_lane_weight], sourcetablealias.[t08_odd_feature_a38_dust_lane_fraction], sourcetablealias.[t08_odd_feature_a38_dust_lane_weighted_fraction], sourcetablealias.[t08_odd_feature_a38_dust_lane_debiased], sourcetablealias.[t08_odd_feature_a38_dust_lane_flag], sourcetablealias.[t09_bulge_shape_a25_rounded_count], sourcetablealias.[t09_bulge_shape_a25_rounded_weight], sourcetablealias.[t09_bulge_shape_a25_rounded_fraction], sourcetablealias.[t09_bulge_shape_a25_rounded_weighted_fraction], sourcetablealias.[t09_bulge_shape_a25_rounded_debiased], sourcetablealias.[t09_bulge_shape_a25_rounded_flag], sourcetablealias.[t09_bulge_shape_a26_boxy_count], sourcetablealias.[t09_bulge_shape_a26_boxy_weight], sourcetablealias.[t09_bulge_shape_a26_boxy_fraction], sourcetablealias.[t09_bulge_shape_a26_boxy_weighted_fraction], sourcetablealias.[t09_bulge_shape_a26_boxy_debiased], sourcetablealias.[t09_bulge_shape_a26_boxy_flag], sourcetablealias.[t09_bulge_shape_a27_no_bulge_count], sourcetablealias.[t09_bulge_shape_a27_no_bulge_weight], sourcetablealias.[t09_bulge_shape_a27_no_bulge_fraction], sourcetablealias.[t09_bulge_shape_a27_no_bulge_weighted_fraction], sourcetablealias.[t09_bulge_shape_a27_no_bulge_debiased], sourcetablealias.[t09_bulge_shape_a27_no_bulge_flag], sourcetablealias.[t10_arms_winding_a28_tight_count], sourcetablealias.[t10_arms_winding_a28_tight_weight], sourcetablealias.[t10_arms_winding_a28_tight_fraction], sourcetablealias.[t10_arms_winding_a28_tight_weighted_fraction], sourcetablealias.[t10_arms_winding_a28_tight_debiased], sourcetablealias.[t10_arms_winding_a28_tight_flag], sourcetablealias.[t10_arms_winding_a29_medium_count], sourcetablealias.[t10_arms_winding_a29_medium_weight], sourcetablealias.[t10_arms_winding_a29_medium_fraction], sourcetablealias.[t10_arms_winding_a29_medium_weighted_fraction], sourcetablealias.[t10_arms_winding_a29_medium_debiased], sourcetablealias.[t10_arms_winding_a29_medium_flag], sourcetablealias.[t10_arms_winding_a30_loose_count], sourcetablealias.[t10_arms_winding_a30_loose_weight], sourcetablealias.[t10_arms_winding_a30_loose_fraction], sourcetablealias.[t10_arms_winding_a30_loose_weighted_fraction], sourcetablealias.[t10_arms_winding_a30_loose_debiased], sourcetablealias.[t10_arms_winding_a30_loose_flag], sourcetablealias.[t11_arms_number_a31_1_count], sourcetablealias.[t11_arms_number_a31_1_weight], sourcetablealias.[t11_arms_number_a31_1_fraction], sourcetablealias.[t11_arms_number_a31_1_weighted_fraction], sourcetablealias.[t11_arms_number_a31_1_debiased], sourcetablealias.[t11_arms_number_a31_1_flag], sourcetablealias.[t11_arms_number_a32_2_count], sourcetablealias.[t11_arms_number_a32_2_weight], sourcetablealias.[t11_arms_number_a32_2_fraction], sourcetablealias.[t11_arms_number_a32_2_weighted_fraction], sourcetablealias.[t11_arms_number_a32_2_debiased], sourcetablealias.[t11_arms_number_a32_2_flag], sourcetablealias.[t11_arms_number_a33_3_count], sourcetablealias.[t11_arms_number_a33_3_weight], sourcetablealias.[t11_arms_number_a33_3_fraction], sourcetablealias.[t11_arms_number_a33_3_weighted_fraction], sourcetablealias.[t11_arms_number_a33_3_debiased], sourcetablealias.[t11_arms_number_a33_3_flag], sourcetablealias.[t11_arms_number_a34_4_count], sourcetablealias.[t11_arms_number_a34_4_weight], sourcetablealias.[t11_arms_number_a34_4_fraction], sourcetablealias.[t11_arms_number_a34_4_weighted_fraction], sourcetablealias.[t11_arms_number_a34_4_debiased], sourcetablealias.[t11_arms_number_a34_4_flag], sourcetablealias.[t11_arms_number_a36_more_than_4_count], sourcetablealias.[t11_arms_number_a36_more_than_4_weight], sourcetablealias.[t11_arms_number_a36_more_than_4_fraction], sourcetablealias.[t11_arms_number_a36_more_than_4_weighted_fraction], sourcetablealias.[t11_arms_number_a36_more_than_4_debiased], sourcetablealias.[t11_arms_number_a36_more_than_4_flag], sourcetablealias.[t11_arms_number_a37_cant_tell_count], sourcetablealias.[t11_arms_number_a37_cant_tell_weight], sourcetablealias.[t11_arms_number_a37_cant_tell_fraction], sourcetablealias.[t11_arms_number_a37_cant_tell_weighted_fraction], sourcetablealias.[t11_arms_number_a37_cant_tell_debiased], sourcetablealias.[t11_arms_number_a37_cant_tell_flag]
 FROM   [SkyNode_SDSSDR12].[dbo].[zoo2Stripe82Coadd1] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.stripe82objid = sourcetablealias.stripe82objid
	;


GO

-- SUBSAMPLING TABLE 'zoo2Stripe82Coadd2' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[stripe82objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[stripe82objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zoo2Stripe82Coadd2] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [stripe82objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zoo2Stripe82Coadd2] WITH (TABLOCKX)
	([specobjid], [stripe82objid], [dr8objid], [dr7objid], [ra], [dec], [rastring], [decstring], [sample], [total_classifications], [total_votes], [t01_smooth_or_features_a01_smooth_count], [t01_smooth_or_features_a01_smooth_weight], [t01_smooth_or_features_a01_smooth_fraction], [t01_smooth_or_features_a01_smooth_weighted_fraction], [t01_smooth_or_features_a01_smooth_debiased], [t01_smooth_or_features_a01_smooth_flag], [t01_smooth_or_features_a02_features_or_disk_count], [t01_smooth_or_features_a02_features_or_disk_weight], [t01_smooth_or_features_a02_features_or_disk_fraction], [t01_smooth_or_features_a02_features_or_disk_weighted_fraction], [t01_smooth_or_features_a02_features_or_disk_debiased], [t01_smooth_or_features_a02_features_or_disk_flag], [t01_smooth_or_features_a03_star_or_artifact_count], [t01_smooth_or_features_a03_star_or_artifact_weight], [t01_smooth_or_features_a03_star_or_artifact_fraction], [t01_smooth_or_features_a03_star_or_artifact_weighted_fraction], [t01_smooth_or_features_a03_star_or_artifact_debiased], [t01_smooth_or_features_a03_star_or_artifact_flag], [t02_edgeon_a04_yes_count], [t02_edgeon_a04_yes_weight], [t02_edgeon_a04_yes_fraction], [t02_edgeon_a04_yes_weighted_fraction], [t02_edgeon_a04_yes_debiased], [t02_edgeon_a04_yes_flag], [t02_edgeon_a05_no_count], [t02_edgeon_a05_no_weight], [t02_edgeon_a05_no_fraction], [t02_edgeon_a05_no_weighted_fraction], [t02_edgeon_a05_no_debiased], [t02_edgeon_a05_no_flag], [t03_bar_a06_bar_count], [t03_bar_a06_bar_weight], [t03_bar_a06_bar_fraction], [t03_bar_a06_bar_weighted_fraction], [t03_bar_a06_bar_debiased], [t03_bar_a06_bar_flag], [t03_bar_a07_no_bar_count], [t03_bar_a07_no_bar_weight], [t03_bar_a07_no_bar_fraction], [t03_bar_a07_no_bar_weighted_fraction], [t03_bar_a07_no_bar_debiased], [t03_bar_a07_no_bar_flag], [t04_spiral_a08_spiral_count], [t04_spiral_a08_spiral_weight], [t04_spiral_a08_spiral_fraction], [t04_spiral_a08_spiral_weighted_fraction], [t04_spiral_a08_spiral_debiased], [t04_spiral_a08_spiral_flag], [t04_spiral_a09_no_spiral_count], [t04_spiral_a09_no_spiral_weight], [t04_spiral_a09_no_spiral_fraction], [t04_spiral_a09_no_spiral_weighted_fraction], [t04_spiral_a09_no_spiral_debiased], [t04_spiral_a09_no_spiral_flag], [t05_bulge_prominence_a10_no_bulge_count], [t05_bulge_prominence_a10_no_bulge_weight], [t05_bulge_prominence_a10_no_bulge_fraction], [t05_bulge_prominence_a10_no_bulge_weighted_fraction], [t05_bulge_prominence_a10_no_bulge_debiased], [t05_bulge_prominence_a10_no_bulge_flag], [t05_bulge_prominence_a11_just_noticeable_count], [t05_bulge_prominence_a11_just_noticeable_weight], [t05_bulge_prominence_a11_just_noticeable_fraction], [t05_bulge_prominence_a11_just_noticeable_weighted_fraction], [t05_bulge_prominence_a11_just_noticeable_debiased], [t05_bulge_prominence_a11_just_noticeable_flag], [t05_bulge_prominence_a12_obvious_count], [t05_bulge_prominence_a12_obvious_weight], [t05_bulge_prominence_a12_obvious_fraction], [t05_bulge_prominence_a12_obvious_weighted_fraction], [t05_bulge_prominence_a12_obvious_debiased], [t05_bulge_prominence_a12_obvious_flag], [t05_bulge_prominence_a13_dominant_count], [t05_bulge_prominence_a13_dominant_weight], [t05_bulge_prominence_a13_dominant_fraction], [t05_bulge_prominence_a13_dominant_weighted_fraction], [t05_bulge_prominence_a13_dominant_debiased], [t05_bulge_prominence_a13_dominant_flag], [t06_odd_a14_yes_count], [t06_odd_a14_yes_weight], [t06_odd_a14_yes_fraction], [t06_odd_a14_yes_weighted_fraction], [t06_odd_a14_yes_debiased], [t06_odd_a14_yes_flag], [t06_odd_a15_no_count], [t06_odd_a15_no_weight], [t06_odd_a15_no_fraction], [t06_odd_a15_no_weighted_fraction], [t06_odd_a15_no_debiased], [t06_odd_a15_no_flag], [t07_rounded_a16_completely_round_count], [t07_rounded_a16_completely_round_weight], [t07_rounded_a16_completely_round_fraction], [t07_rounded_a16_completely_round_weighted_fraction], [t07_rounded_a16_completely_round_debiased], [t07_rounded_a16_completely_round_flag], [t07_rounded_a17_in_between_count], [t07_rounded_a17_in_between_weight], [t07_rounded_a17_in_between_fraction], [t07_rounded_a17_in_between_weighted_fraction], [t07_rounded_a17_in_between_debiased], [t07_rounded_a17_in_between_flag], [t07_rounded_a18_cigar_shaped_count], [t07_rounded_a18_cigar_shaped_weight], [t07_rounded_a18_cigar_shaped_fraction], [t07_rounded_a18_cigar_shaped_weighted_fraction], [t07_rounded_a18_cigar_shaped_debiased], [t07_rounded_a18_cigar_shaped_flag], [t08_odd_feature_a19_ring_count], [t08_odd_feature_a19_ring_weight], [t08_odd_feature_a19_ring_fraction], [t08_odd_feature_a19_ring_weighted_fraction], [t08_odd_feature_a19_ring_debiased], [t08_odd_feature_a19_ring_flag], [t08_odd_feature_a20_lens_or_arc_count], [t08_odd_feature_a20_lens_or_arc_weight], [t08_odd_feature_a20_lens_or_arc_fraction], [t08_odd_feature_a20_lens_or_arc_weighted_fraction], [t08_odd_feature_a20_lens_or_arc_debiased], [t08_odd_feature_a20_lens_or_arc_flag], [t08_odd_feature_a21_disturbed_count], [t08_odd_feature_a21_disturbed_weight], [t08_odd_feature_a21_disturbed_fraction], [t08_odd_feature_a21_disturbed_weighted_fraction], [t08_odd_feature_a21_disturbed_debiased], [t08_odd_feature_a21_disturbed_flag], [t08_odd_feature_a22_irregular_count], [t08_odd_feature_a22_irregular_weight], [t08_odd_feature_a22_irregular_fraction], [t08_odd_feature_a22_irregular_weighted_fraction], [t08_odd_feature_a22_irregular_debiased], [t08_odd_feature_a22_irregular_flag], [t08_odd_feature_a23_other_count], [t08_odd_feature_a23_other_weight], [t08_odd_feature_a23_other_fraction], [t08_odd_feature_a23_other_weighted_fraction], [t08_odd_feature_a23_other_debiased], [t08_odd_feature_a23_other_flag], [t08_odd_feature_a24_merger_count], [t08_odd_feature_a24_merger_weight], [t08_odd_feature_a24_merger_fraction], [t08_odd_feature_a24_merger_weighted_fraction], [t08_odd_feature_a24_merger_debiased], [t08_odd_feature_a24_merger_flag], [t08_odd_feature_a38_dust_lane_count], [t08_odd_feature_a38_dust_lane_weight], [t08_odd_feature_a38_dust_lane_fraction], [t08_odd_feature_a38_dust_lane_weighted_fraction], [t08_odd_feature_a38_dust_lane_debiased], [t08_odd_feature_a38_dust_lane_flag], [t09_bulge_shape_a25_rounded_count], [t09_bulge_shape_a25_rounded_weight], [t09_bulge_shape_a25_rounded_fraction], [t09_bulge_shape_a25_rounded_weighted_fraction], [t09_bulge_shape_a25_rounded_debiased], [t09_bulge_shape_a25_rounded_flag], [t09_bulge_shape_a26_boxy_count], [t09_bulge_shape_a26_boxy_weight], [t09_bulge_shape_a26_boxy_fraction], [t09_bulge_shape_a26_boxy_weighted_fraction], [t09_bulge_shape_a26_boxy_debiased], [t09_bulge_shape_a26_boxy_flag], [t09_bulge_shape_a27_no_bulge_count], [t09_bulge_shape_a27_no_bulge_weight], [t09_bulge_shape_a27_no_bulge_fraction], [t09_bulge_shape_a27_no_bulge_weighted_fraction], [t09_bulge_shape_a27_no_bulge_debiased], [t09_bulge_shape_a27_no_bulge_flag], [t10_arms_winding_a28_tight_count], [t10_arms_winding_a28_tight_weight], [t10_arms_winding_a28_tight_fraction], [t10_arms_winding_a28_tight_weighted_fraction], [t10_arms_winding_a28_tight_debiased], [t10_arms_winding_a28_tight_flag], [t10_arms_winding_a29_medium_count], [t10_arms_winding_a29_medium_weight], [t10_arms_winding_a29_medium_fraction], [t10_arms_winding_a29_medium_weighted_fraction], [t10_arms_winding_a29_medium_debiased], [t10_arms_winding_a29_medium_flag], [t10_arms_winding_a30_loose_count], [t10_arms_winding_a30_loose_weight], [t10_arms_winding_a30_loose_fraction], [t10_arms_winding_a30_loose_weighted_fraction], [t10_arms_winding_a30_loose_debiased], [t10_arms_winding_a30_loose_flag], [t11_arms_number_a31_1_count], [t11_arms_number_a31_1_weight], [t11_arms_number_a31_1_fraction], [t11_arms_number_a31_1_weighted_fraction], [t11_arms_number_a31_1_debiased], [t11_arms_number_a31_1_flag], [t11_arms_number_a32_2_count], [t11_arms_number_a32_2_weight], [t11_arms_number_a32_2_fraction], [t11_arms_number_a32_2_weighted_fraction], [t11_arms_number_a32_2_debiased], [t11_arms_number_a32_2_flag], [t11_arms_number_a33_3_count], [t11_arms_number_a33_3_weight], [t11_arms_number_a33_3_fraction], [t11_arms_number_a33_3_weighted_fraction], [t11_arms_number_a33_3_debiased], [t11_arms_number_a33_3_flag], [t11_arms_number_a34_4_count], [t11_arms_number_a34_4_weight], [t11_arms_number_a34_4_fraction], [t11_arms_number_a34_4_weighted_fraction], [t11_arms_number_a34_4_debiased], [t11_arms_number_a34_4_flag], [t11_arms_number_a36_more_than_4_count], [t11_arms_number_a36_more_than_4_weight], [t11_arms_number_a36_more_than_4_fraction], [t11_arms_number_a36_more_than_4_weighted_fraction], [t11_arms_number_a36_more_than_4_debiased], [t11_arms_number_a36_more_than_4_flag], [t11_arms_number_a37_cant_tell_count], [t11_arms_number_a37_cant_tell_weight], [t11_arms_number_a37_cant_tell_fraction], [t11_arms_number_a37_cant_tell_weighted_fraction], [t11_arms_number_a37_cant_tell_debiased], [t11_arms_number_a37_cant_tell_flag])
 SELECT sourcetablealias.[specobjid], sourcetablealias.[stripe82objid], sourcetablealias.[dr8objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[decstring], sourcetablealias.[sample], sourcetablealias.[total_classifications], sourcetablealias.[total_votes], sourcetablealias.[t01_smooth_or_features_a01_smooth_count], sourcetablealias.[t01_smooth_or_features_a01_smooth_weight], sourcetablealias.[t01_smooth_or_features_a01_smooth_fraction], sourcetablealias.[t01_smooth_or_features_a01_smooth_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a01_smooth_debiased], sourcetablealias.[t01_smooth_or_features_a01_smooth_flag], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_count], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_weight], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_fraction], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_debiased], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_flag], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_count], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_weight], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_fraction], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_debiased], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_flag], sourcetablealias.[t02_edgeon_a04_yes_count], sourcetablealias.[t02_edgeon_a04_yes_weight], sourcetablealias.[t02_edgeon_a04_yes_fraction], sourcetablealias.[t02_edgeon_a04_yes_weighted_fraction], sourcetablealias.[t02_edgeon_a04_yes_debiased], sourcetablealias.[t02_edgeon_a04_yes_flag], sourcetablealias.[t02_edgeon_a05_no_count], sourcetablealias.[t02_edgeon_a05_no_weight], sourcetablealias.[t02_edgeon_a05_no_fraction], sourcetablealias.[t02_edgeon_a05_no_weighted_fraction], sourcetablealias.[t02_edgeon_a05_no_debiased], sourcetablealias.[t02_edgeon_a05_no_flag], sourcetablealias.[t03_bar_a06_bar_count], sourcetablealias.[t03_bar_a06_bar_weight], sourcetablealias.[t03_bar_a06_bar_fraction], sourcetablealias.[t03_bar_a06_bar_weighted_fraction], sourcetablealias.[t03_bar_a06_bar_debiased], sourcetablealias.[t03_bar_a06_bar_flag], sourcetablealias.[t03_bar_a07_no_bar_count], sourcetablealias.[t03_bar_a07_no_bar_weight], sourcetablealias.[t03_bar_a07_no_bar_fraction], sourcetablealias.[t03_bar_a07_no_bar_weighted_fraction], sourcetablealias.[t03_bar_a07_no_bar_debiased], sourcetablealias.[t03_bar_a07_no_bar_flag], sourcetablealias.[t04_spiral_a08_spiral_count], sourcetablealias.[t04_spiral_a08_spiral_weight], sourcetablealias.[t04_spiral_a08_spiral_fraction], sourcetablealias.[t04_spiral_a08_spiral_weighted_fraction], sourcetablealias.[t04_spiral_a08_spiral_debiased], sourcetablealias.[t04_spiral_a08_spiral_flag], sourcetablealias.[t04_spiral_a09_no_spiral_count], sourcetablealias.[t04_spiral_a09_no_spiral_weight], sourcetablealias.[t04_spiral_a09_no_spiral_fraction], sourcetablealias.[t04_spiral_a09_no_spiral_weighted_fraction], sourcetablealias.[t04_spiral_a09_no_spiral_debiased], sourcetablealias.[t04_spiral_a09_no_spiral_flag], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_count], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_weight], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_fraction], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_debiased], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_flag], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_count], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_weight], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_fraction], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_debiased], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_flag], sourcetablealias.[t05_bulge_prominence_a12_obvious_count], sourcetablealias.[t05_bulge_prominence_a12_obvious_weight], sourcetablealias.[t05_bulge_prominence_a12_obvious_fraction], sourcetablealias.[t05_bulge_prominence_a12_obvious_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a12_obvious_debiased], sourcetablealias.[t05_bulge_prominence_a12_obvious_flag], sourcetablealias.[t05_bulge_prominence_a13_dominant_count], sourcetablealias.[t05_bulge_prominence_a13_dominant_weight], sourcetablealias.[t05_bulge_prominence_a13_dominant_fraction], sourcetablealias.[t05_bulge_prominence_a13_dominant_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a13_dominant_debiased], sourcetablealias.[t05_bulge_prominence_a13_dominant_flag], sourcetablealias.[t06_odd_a14_yes_count], sourcetablealias.[t06_odd_a14_yes_weight], sourcetablealias.[t06_odd_a14_yes_fraction], sourcetablealias.[t06_odd_a14_yes_weighted_fraction], sourcetablealias.[t06_odd_a14_yes_debiased], sourcetablealias.[t06_odd_a14_yes_flag], sourcetablealias.[t06_odd_a15_no_count], sourcetablealias.[t06_odd_a15_no_weight], sourcetablealias.[t06_odd_a15_no_fraction], sourcetablealias.[t06_odd_a15_no_weighted_fraction], sourcetablealias.[t06_odd_a15_no_debiased], sourcetablealias.[t06_odd_a15_no_flag], sourcetablealias.[t07_rounded_a16_completely_round_count], sourcetablealias.[t07_rounded_a16_completely_round_weight], sourcetablealias.[t07_rounded_a16_completely_round_fraction], sourcetablealias.[t07_rounded_a16_completely_round_weighted_fraction], sourcetablealias.[t07_rounded_a16_completely_round_debiased], sourcetablealias.[t07_rounded_a16_completely_round_flag], sourcetablealias.[t07_rounded_a17_in_between_count], sourcetablealias.[t07_rounded_a17_in_between_weight], sourcetablealias.[t07_rounded_a17_in_between_fraction], sourcetablealias.[t07_rounded_a17_in_between_weighted_fraction], sourcetablealias.[t07_rounded_a17_in_between_debiased], sourcetablealias.[t07_rounded_a17_in_between_flag], sourcetablealias.[t07_rounded_a18_cigar_shaped_count], sourcetablealias.[t07_rounded_a18_cigar_shaped_weight], sourcetablealias.[t07_rounded_a18_cigar_shaped_fraction], sourcetablealias.[t07_rounded_a18_cigar_shaped_weighted_fraction], sourcetablealias.[t07_rounded_a18_cigar_shaped_debiased], sourcetablealias.[t07_rounded_a18_cigar_shaped_flag], sourcetablealias.[t08_odd_feature_a19_ring_count], sourcetablealias.[t08_odd_feature_a19_ring_weight], sourcetablealias.[t08_odd_feature_a19_ring_fraction], sourcetablealias.[t08_odd_feature_a19_ring_weighted_fraction], sourcetablealias.[t08_odd_feature_a19_ring_debiased], sourcetablealias.[t08_odd_feature_a19_ring_flag], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_count], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_weight], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_fraction], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_weighted_fraction], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_debiased], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_flag], sourcetablealias.[t08_odd_feature_a21_disturbed_count], sourcetablealias.[t08_odd_feature_a21_disturbed_weight], sourcetablealias.[t08_odd_feature_a21_disturbed_fraction], sourcetablealias.[t08_odd_feature_a21_disturbed_weighted_fraction], sourcetablealias.[t08_odd_feature_a21_disturbed_debiased], sourcetablealias.[t08_odd_feature_a21_disturbed_flag], sourcetablealias.[t08_odd_feature_a22_irregular_count], sourcetablealias.[t08_odd_feature_a22_irregular_weight], sourcetablealias.[t08_odd_feature_a22_irregular_fraction], sourcetablealias.[t08_odd_feature_a22_irregular_weighted_fraction], sourcetablealias.[t08_odd_feature_a22_irregular_debiased], sourcetablealias.[t08_odd_feature_a22_irregular_flag], sourcetablealias.[t08_odd_feature_a23_other_count], sourcetablealias.[t08_odd_feature_a23_other_weight], sourcetablealias.[t08_odd_feature_a23_other_fraction], sourcetablealias.[t08_odd_feature_a23_other_weighted_fraction], sourcetablealias.[t08_odd_feature_a23_other_debiased], sourcetablealias.[t08_odd_feature_a23_other_flag], sourcetablealias.[t08_odd_feature_a24_merger_count], sourcetablealias.[t08_odd_feature_a24_merger_weight], sourcetablealias.[t08_odd_feature_a24_merger_fraction], sourcetablealias.[t08_odd_feature_a24_merger_weighted_fraction], sourcetablealias.[t08_odd_feature_a24_merger_debiased], sourcetablealias.[t08_odd_feature_a24_merger_flag], sourcetablealias.[t08_odd_feature_a38_dust_lane_count], sourcetablealias.[t08_odd_feature_a38_dust_lane_weight], sourcetablealias.[t08_odd_feature_a38_dust_lane_fraction], sourcetablealias.[t08_odd_feature_a38_dust_lane_weighted_fraction], sourcetablealias.[t08_odd_feature_a38_dust_lane_debiased], sourcetablealias.[t08_odd_feature_a38_dust_lane_flag], sourcetablealias.[t09_bulge_shape_a25_rounded_count], sourcetablealias.[t09_bulge_shape_a25_rounded_weight], sourcetablealias.[t09_bulge_shape_a25_rounded_fraction], sourcetablealias.[t09_bulge_shape_a25_rounded_weighted_fraction], sourcetablealias.[t09_bulge_shape_a25_rounded_debiased], sourcetablealias.[t09_bulge_shape_a25_rounded_flag], sourcetablealias.[t09_bulge_shape_a26_boxy_count], sourcetablealias.[t09_bulge_shape_a26_boxy_weight], sourcetablealias.[t09_bulge_shape_a26_boxy_fraction], sourcetablealias.[t09_bulge_shape_a26_boxy_weighted_fraction], sourcetablealias.[t09_bulge_shape_a26_boxy_debiased], sourcetablealias.[t09_bulge_shape_a26_boxy_flag], sourcetablealias.[t09_bulge_shape_a27_no_bulge_count], sourcetablealias.[t09_bulge_shape_a27_no_bulge_weight], sourcetablealias.[t09_bulge_shape_a27_no_bulge_fraction], sourcetablealias.[t09_bulge_shape_a27_no_bulge_weighted_fraction], sourcetablealias.[t09_bulge_shape_a27_no_bulge_debiased], sourcetablealias.[t09_bulge_shape_a27_no_bulge_flag], sourcetablealias.[t10_arms_winding_a28_tight_count], sourcetablealias.[t10_arms_winding_a28_tight_weight], sourcetablealias.[t10_arms_winding_a28_tight_fraction], sourcetablealias.[t10_arms_winding_a28_tight_weighted_fraction], sourcetablealias.[t10_arms_winding_a28_tight_debiased], sourcetablealias.[t10_arms_winding_a28_tight_flag], sourcetablealias.[t10_arms_winding_a29_medium_count], sourcetablealias.[t10_arms_winding_a29_medium_weight], sourcetablealias.[t10_arms_winding_a29_medium_fraction], sourcetablealias.[t10_arms_winding_a29_medium_weighted_fraction], sourcetablealias.[t10_arms_winding_a29_medium_debiased], sourcetablealias.[t10_arms_winding_a29_medium_flag], sourcetablealias.[t10_arms_winding_a30_loose_count], sourcetablealias.[t10_arms_winding_a30_loose_weight], sourcetablealias.[t10_arms_winding_a30_loose_fraction], sourcetablealias.[t10_arms_winding_a30_loose_weighted_fraction], sourcetablealias.[t10_arms_winding_a30_loose_debiased], sourcetablealias.[t10_arms_winding_a30_loose_flag], sourcetablealias.[t11_arms_number_a31_1_count], sourcetablealias.[t11_arms_number_a31_1_weight], sourcetablealias.[t11_arms_number_a31_1_fraction], sourcetablealias.[t11_arms_number_a31_1_weighted_fraction], sourcetablealias.[t11_arms_number_a31_1_debiased], sourcetablealias.[t11_arms_number_a31_1_flag], sourcetablealias.[t11_arms_number_a32_2_count], sourcetablealias.[t11_arms_number_a32_2_weight], sourcetablealias.[t11_arms_number_a32_2_fraction], sourcetablealias.[t11_arms_number_a32_2_weighted_fraction], sourcetablealias.[t11_arms_number_a32_2_debiased], sourcetablealias.[t11_arms_number_a32_2_flag], sourcetablealias.[t11_arms_number_a33_3_count], sourcetablealias.[t11_arms_number_a33_3_weight], sourcetablealias.[t11_arms_number_a33_3_fraction], sourcetablealias.[t11_arms_number_a33_3_weighted_fraction], sourcetablealias.[t11_arms_number_a33_3_debiased], sourcetablealias.[t11_arms_number_a33_3_flag], sourcetablealias.[t11_arms_number_a34_4_count], sourcetablealias.[t11_arms_number_a34_4_weight], sourcetablealias.[t11_arms_number_a34_4_fraction], sourcetablealias.[t11_arms_number_a34_4_weighted_fraction], sourcetablealias.[t11_arms_number_a34_4_debiased], sourcetablealias.[t11_arms_number_a34_4_flag], sourcetablealias.[t11_arms_number_a36_more_than_4_count], sourcetablealias.[t11_arms_number_a36_more_than_4_weight], sourcetablealias.[t11_arms_number_a36_more_than_4_fraction], sourcetablealias.[t11_arms_number_a36_more_than_4_weighted_fraction], sourcetablealias.[t11_arms_number_a36_more_than_4_debiased], sourcetablealias.[t11_arms_number_a36_more_than_4_flag], sourcetablealias.[t11_arms_number_a37_cant_tell_count], sourcetablealias.[t11_arms_number_a37_cant_tell_weight], sourcetablealias.[t11_arms_number_a37_cant_tell_fraction], sourcetablealias.[t11_arms_number_a37_cant_tell_weighted_fraction], sourcetablealias.[t11_arms_number_a37_cant_tell_debiased], sourcetablealias.[t11_arms_number_a37_cant_tell_flag]
 FROM   [SkyNode_SDSSDR12].[dbo].[zoo2Stripe82Coadd2] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.stripe82objid = sourcetablealias.stripe82objid
	;


GO

-- SUBSAMPLING TABLE 'zoo2Stripe82Normal' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[dr7objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[dr7objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zoo2Stripe82Normal] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [dr7objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zoo2Stripe82Normal] WITH (TABLOCKX)
	([specobjid], [dr8objid], [dr7objid], [ra], [dec], [rastring], [decstring], [sample], [total_classifications], [total_votes], [t01_smooth_or_features_a01_smooth_count], [t01_smooth_or_features_a01_smooth_weight], [t01_smooth_or_features_a01_smooth_fraction], [t01_smooth_or_features_a01_smooth_weighted_fraction], [t01_smooth_or_features_a01_smooth_debiased], [t01_smooth_or_features_a01_smooth_flag], [t01_smooth_or_features_a02_features_or_disk_count], [t01_smooth_or_features_a02_features_or_disk_weight], [t01_smooth_or_features_a02_features_or_disk_fraction], [t01_smooth_or_features_a02_features_or_disk_weighted_fraction], [t01_smooth_or_features_a02_features_or_disk_debiased], [t01_smooth_or_features_a02_features_or_disk_flag], [t01_smooth_or_features_a03_star_or_artifact_count], [t01_smooth_or_features_a03_star_or_artifact_weight], [t01_smooth_or_features_a03_star_or_artifact_fraction], [t01_smooth_or_features_a03_star_or_artifact_weighted_fraction], [t01_smooth_or_features_a03_star_or_artifact_debiased], [t01_smooth_or_features_a03_star_or_artifact_flag], [t02_edgeon_a04_yes_count], [t02_edgeon_a04_yes_weight], [t02_edgeon_a04_yes_fraction], [t02_edgeon_a04_yes_weighted_fraction], [t02_edgeon_a04_yes_debiased], [t02_edgeon_a04_yes_flag], [t02_edgeon_a05_no_count], [t02_edgeon_a05_no_weight], [t02_edgeon_a05_no_fraction], [t02_edgeon_a05_no_weighted_fraction], [t02_edgeon_a05_no_debiased], [t02_edgeon_a05_no_flag], [t03_bar_a06_bar_count], [t03_bar_a06_bar_weight], [t03_bar_a06_bar_fraction], [t03_bar_a06_bar_weighted_fraction], [t03_bar_a06_bar_debiased], [t03_bar_a06_bar_flag], [t03_bar_a07_no_bar_count], [t03_bar_a07_no_bar_weight], [t03_bar_a07_no_bar_fraction], [t03_bar_a07_no_bar_weighted_fraction], [t03_bar_a07_no_bar_debiased], [t03_bar_a07_no_bar_flag], [t04_spiral_a08_spiral_count], [t04_spiral_a08_spiral_weight], [t04_spiral_a08_spiral_fraction], [t04_spiral_a08_spiral_weighted_fraction], [t04_spiral_a08_spiral_debiased], [t04_spiral_a08_spiral_flag], [t04_spiral_a09_no_spiral_count], [t04_spiral_a09_no_spiral_weight], [t04_spiral_a09_no_spiral_fraction], [t04_spiral_a09_no_spiral_weighted_fraction], [t04_spiral_a09_no_spiral_debiased], [t04_spiral_a09_no_spiral_flag], [t05_bulge_prominence_a10_no_bulge_count], [t05_bulge_prominence_a10_no_bulge_weight], [t05_bulge_prominence_a10_no_bulge_fraction], [t05_bulge_prominence_a10_no_bulge_weighted_fraction], [t05_bulge_prominence_a10_no_bulge_debiased], [t05_bulge_prominence_a10_no_bulge_flag], [t05_bulge_prominence_a11_just_noticeable_count], [t05_bulge_prominence_a11_just_noticeable_weight], [t05_bulge_prominence_a11_just_noticeable_fraction], [t05_bulge_prominence_a11_just_noticeable_weighted_fraction], [t05_bulge_prominence_a11_just_noticeable_debiased], [t05_bulge_prominence_a11_just_noticeable_flag], [t05_bulge_prominence_a12_obvious_count], [t05_bulge_prominence_a12_obvious_weight], [t05_bulge_prominence_a12_obvious_fraction], [t05_bulge_prominence_a12_obvious_weighted_fraction], [t05_bulge_prominence_a12_obvious_debiased], [t05_bulge_prominence_a12_obvious_flag], [t05_bulge_prominence_a13_dominant_count], [t05_bulge_prominence_a13_dominant_weight], [t05_bulge_prominence_a13_dominant_fraction], [t05_bulge_prominence_a13_dominant_weighted_fraction], [t05_bulge_prominence_a13_dominant_debiased], [t05_bulge_prominence_a13_dominant_flag], [t06_odd_a14_yes_count], [t06_odd_a14_yes_weight], [t06_odd_a14_yes_fraction], [t06_odd_a14_yes_weighted_fraction], [t06_odd_a14_yes_debiased], [t06_odd_a14_yes_flag], [t06_odd_a15_no_count], [t06_odd_a15_no_weight], [t06_odd_a15_no_fraction], [t06_odd_a15_no_weighted_fraction], [t06_odd_a15_no_debiased], [t06_odd_a15_no_flag], [t07_rounded_a16_completely_round_count], [t07_rounded_a16_completely_round_weight], [t07_rounded_a16_completely_round_fraction], [t07_rounded_a16_completely_round_weighted_fraction], [t07_rounded_a16_completely_round_debiased], [t07_rounded_a16_completely_round_flag], [t07_rounded_a17_in_between_count], [t07_rounded_a17_in_between_weight], [t07_rounded_a17_in_between_fraction], [t07_rounded_a17_in_between_weighted_fraction], [t07_rounded_a17_in_between_debiased], [t07_rounded_a17_in_between_flag], [t07_rounded_a18_cigar_shaped_count], [t07_rounded_a18_cigar_shaped_weight], [t07_rounded_a18_cigar_shaped_fraction], [t07_rounded_a18_cigar_shaped_weighted_fraction], [t07_rounded_a18_cigar_shaped_debiased], [t07_rounded_a18_cigar_shaped_flag], [t08_odd_feature_a19_ring_count], [t08_odd_feature_a19_ring_weight], [t08_odd_feature_a19_ring_fraction], [t08_odd_feature_a19_ring_weighted_fraction], [t08_odd_feature_a19_ring_debiased], [t08_odd_feature_a19_ring_flag], [t08_odd_feature_a20_lens_or_arc_count], [t08_odd_feature_a20_lens_or_arc_weight], [t08_odd_feature_a20_lens_or_arc_fraction], [t08_odd_feature_a20_lens_or_arc_weighted_fraction], [t08_odd_feature_a20_lens_or_arc_debiased], [t08_odd_feature_a20_lens_or_arc_flag], [t08_odd_feature_a21_disturbed_count], [t08_odd_feature_a21_disturbed_weight], [t08_odd_feature_a21_disturbed_fraction], [t08_odd_feature_a21_disturbed_weighted_fraction], [t08_odd_feature_a21_disturbed_debiased], [t08_odd_feature_a21_disturbed_flag], [t08_odd_feature_a22_irregular_count], [t08_odd_feature_a22_irregular_weight], [t08_odd_feature_a22_irregular_fraction], [t08_odd_feature_a22_irregular_weighted_fraction], [t08_odd_feature_a22_irregular_debiased], [t08_odd_feature_a22_irregular_flag], [t08_odd_feature_a23_other_count], [t08_odd_feature_a23_other_weight], [t08_odd_feature_a23_other_fraction], [t08_odd_feature_a23_other_weighted_fraction], [t08_odd_feature_a23_other_debiased], [t08_odd_feature_a23_other_flag], [t08_odd_feature_a24_merger_count], [t08_odd_feature_a24_merger_weight], [t08_odd_feature_a24_merger_fraction], [t08_odd_feature_a24_merger_weighted_fraction], [t08_odd_feature_a24_merger_debiased], [t08_odd_feature_a24_merger_flag], [t08_odd_feature_a38_dust_lane_count], [t08_odd_feature_a38_dust_lane_weight], [t08_odd_feature_a38_dust_lane_fraction], [t08_odd_feature_a38_dust_lane_weighted_fraction], [t08_odd_feature_a38_dust_lane_debiased], [t08_odd_feature_a38_dust_lane_flag], [t09_bulge_shape_a25_rounded_count], [t09_bulge_shape_a25_rounded_weight], [t09_bulge_shape_a25_rounded_fraction], [t09_bulge_shape_a25_rounded_weighted_fraction], [t09_bulge_shape_a25_rounded_debiased], [t09_bulge_shape_a25_rounded_flag], [t09_bulge_shape_a26_boxy_count], [t09_bulge_shape_a26_boxy_weight], [t09_bulge_shape_a26_boxy_fraction], [t09_bulge_shape_a26_boxy_weighted_fraction], [t09_bulge_shape_a26_boxy_debiased], [t09_bulge_shape_a26_boxy_flag], [t09_bulge_shape_a27_no_bulge_count], [t09_bulge_shape_a27_no_bulge_weight], [t09_bulge_shape_a27_no_bulge_fraction], [t09_bulge_shape_a27_no_bulge_weighted_fraction], [t09_bulge_shape_a27_no_bulge_debiased], [t09_bulge_shape_a27_no_bulge_flag], [t10_arms_winding_a28_tight_count], [t10_arms_winding_a28_tight_weight], [t10_arms_winding_a28_tight_fraction], [t10_arms_winding_a28_tight_weighted_fraction], [t10_arms_winding_a28_tight_debiased], [t10_arms_winding_a28_tight_flag], [t10_arms_winding_a29_medium_count], [t10_arms_winding_a29_medium_weight], [t10_arms_winding_a29_medium_fraction], [t10_arms_winding_a29_medium_weighted_fraction], [t10_arms_winding_a29_medium_debiased], [t10_arms_winding_a29_medium_flag], [t10_arms_winding_a30_loose_count], [t10_arms_winding_a30_loose_weight], [t10_arms_winding_a30_loose_fraction], [t10_arms_winding_a30_loose_weighted_fraction], [t10_arms_winding_a30_loose_debiased], [t10_arms_winding_a30_loose_flag], [t11_arms_number_a31_1_count], [t11_arms_number_a31_1_weight], [t11_arms_number_a31_1_fraction], [t11_arms_number_a31_1_weighted_fraction], [t11_arms_number_a31_1_debiased], [t11_arms_number_a31_1_flag], [t11_arms_number_a32_2_count], [t11_arms_number_a32_2_weight], [t11_arms_number_a32_2_fraction], [t11_arms_number_a32_2_weighted_fraction], [t11_arms_number_a32_2_debiased], [t11_arms_number_a32_2_flag], [t11_arms_number_a33_3_count], [t11_arms_number_a33_3_weight], [t11_arms_number_a33_3_fraction], [t11_arms_number_a33_3_weighted_fraction], [t11_arms_number_a33_3_debiased], [t11_arms_number_a33_3_flag], [t11_arms_number_a34_4_count], [t11_arms_number_a34_4_weight], [t11_arms_number_a34_4_fraction], [t11_arms_number_a34_4_weighted_fraction], [t11_arms_number_a34_4_debiased], [t11_arms_number_a34_4_flag], [t11_arms_number_a36_more_than_4_count], [t11_arms_number_a36_more_than_4_weight], [t11_arms_number_a36_more_than_4_fraction], [t11_arms_number_a36_more_than_4_weighted_fraction], [t11_arms_number_a36_more_than_4_debiased], [t11_arms_number_a36_more_than_4_flag], [t11_arms_number_a37_cant_tell_count], [t11_arms_number_a37_cant_tell_weight], [t11_arms_number_a37_cant_tell_fraction], [t11_arms_number_a37_cant_tell_weighted_fraction], [t11_arms_number_a37_cant_tell_debiased], [t11_arms_number_a37_cant_tell_flag])
 SELECT sourcetablealias.[specobjid], sourcetablealias.[dr8objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[decstring], sourcetablealias.[sample], sourcetablealias.[total_classifications], sourcetablealias.[total_votes], sourcetablealias.[t01_smooth_or_features_a01_smooth_count], sourcetablealias.[t01_smooth_or_features_a01_smooth_weight], sourcetablealias.[t01_smooth_or_features_a01_smooth_fraction], sourcetablealias.[t01_smooth_or_features_a01_smooth_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a01_smooth_debiased], sourcetablealias.[t01_smooth_or_features_a01_smooth_flag], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_count], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_weight], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_fraction], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_debiased], sourcetablealias.[t01_smooth_or_features_a02_features_or_disk_flag], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_count], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_weight], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_fraction], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_weighted_fraction], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_debiased], sourcetablealias.[t01_smooth_or_features_a03_star_or_artifact_flag], sourcetablealias.[t02_edgeon_a04_yes_count], sourcetablealias.[t02_edgeon_a04_yes_weight], sourcetablealias.[t02_edgeon_a04_yes_fraction], sourcetablealias.[t02_edgeon_a04_yes_weighted_fraction], sourcetablealias.[t02_edgeon_a04_yes_debiased], sourcetablealias.[t02_edgeon_a04_yes_flag], sourcetablealias.[t02_edgeon_a05_no_count], sourcetablealias.[t02_edgeon_a05_no_weight], sourcetablealias.[t02_edgeon_a05_no_fraction], sourcetablealias.[t02_edgeon_a05_no_weighted_fraction], sourcetablealias.[t02_edgeon_a05_no_debiased], sourcetablealias.[t02_edgeon_a05_no_flag], sourcetablealias.[t03_bar_a06_bar_count], sourcetablealias.[t03_bar_a06_bar_weight], sourcetablealias.[t03_bar_a06_bar_fraction], sourcetablealias.[t03_bar_a06_bar_weighted_fraction], sourcetablealias.[t03_bar_a06_bar_debiased], sourcetablealias.[t03_bar_a06_bar_flag], sourcetablealias.[t03_bar_a07_no_bar_count], sourcetablealias.[t03_bar_a07_no_bar_weight], sourcetablealias.[t03_bar_a07_no_bar_fraction], sourcetablealias.[t03_bar_a07_no_bar_weighted_fraction], sourcetablealias.[t03_bar_a07_no_bar_debiased], sourcetablealias.[t03_bar_a07_no_bar_flag], sourcetablealias.[t04_spiral_a08_spiral_count], sourcetablealias.[t04_spiral_a08_spiral_weight], sourcetablealias.[t04_spiral_a08_spiral_fraction], sourcetablealias.[t04_spiral_a08_spiral_weighted_fraction], sourcetablealias.[t04_spiral_a08_spiral_debiased], sourcetablealias.[t04_spiral_a08_spiral_flag], sourcetablealias.[t04_spiral_a09_no_spiral_count], sourcetablealias.[t04_spiral_a09_no_spiral_weight], sourcetablealias.[t04_spiral_a09_no_spiral_fraction], sourcetablealias.[t04_spiral_a09_no_spiral_weighted_fraction], sourcetablealias.[t04_spiral_a09_no_spiral_debiased], sourcetablealias.[t04_spiral_a09_no_spiral_flag], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_count], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_weight], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_fraction], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_debiased], sourcetablealias.[t05_bulge_prominence_a10_no_bulge_flag], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_count], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_weight], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_fraction], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_debiased], sourcetablealias.[t05_bulge_prominence_a11_just_noticeable_flag], sourcetablealias.[t05_bulge_prominence_a12_obvious_count], sourcetablealias.[t05_bulge_prominence_a12_obvious_weight], sourcetablealias.[t05_bulge_prominence_a12_obvious_fraction], sourcetablealias.[t05_bulge_prominence_a12_obvious_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a12_obvious_debiased], sourcetablealias.[t05_bulge_prominence_a12_obvious_flag], sourcetablealias.[t05_bulge_prominence_a13_dominant_count], sourcetablealias.[t05_bulge_prominence_a13_dominant_weight], sourcetablealias.[t05_bulge_prominence_a13_dominant_fraction], sourcetablealias.[t05_bulge_prominence_a13_dominant_weighted_fraction], sourcetablealias.[t05_bulge_prominence_a13_dominant_debiased], sourcetablealias.[t05_bulge_prominence_a13_dominant_flag], sourcetablealias.[t06_odd_a14_yes_count], sourcetablealias.[t06_odd_a14_yes_weight], sourcetablealias.[t06_odd_a14_yes_fraction], sourcetablealias.[t06_odd_a14_yes_weighted_fraction], sourcetablealias.[t06_odd_a14_yes_debiased], sourcetablealias.[t06_odd_a14_yes_flag], sourcetablealias.[t06_odd_a15_no_count], sourcetablealias.[t06_odd_a15_no_weight], sourcetablealias.[t06_odd_a15_no_fraction], sourcetablealias.[t06_odd_a15_no_weighted_fraction], sourcetablealias.[t06_odd_a15_no_debiased], sourcetablealias.[t06_odd_a15_no_flag], sourcetablealias.[t07_rounded_a16_completely_round_count], sourcetablealias.[t07_rounded_a16_completely_round_weight], sourcetablealias.[t07_rounded_a16_completely_round_fraction], sourcetablealias.[t07_rounded_a16_completely_round_weighted_fraction], sourcetablealias.[t07_rounded_a16_completely_round_debiased], sourcetablealias.[t07_rounded_a16_completely_round_flag], sourcetablealias.[t07_rounded_a17_in_between_count], sourcetablealias.[t07_rounded_a17_in_between_weight], sourcetablealias.[t07_rounded_a17_in_between_fraction], sourcetablealias.[t07_rounded_a17_in_between_weighted_fraction], sourcetablealias.[t07_rounded_a17_in_between_debiased], sourcetablealias.[t07_rounded_a17_in_between_flag], sourcetablealias.[t07_rounded_a18_cigar_shaped_count], sourcetablealias.[t07_rounded_a18_cigar_shaped_weight], sourcetablealias.[t07_rounded_a18_cigar_shaped_fraction], sourcetablealias.[t07_rounded_a18_cigar_shaped_weighted_fraction], sourcetablealias.[t07_rounded_a18_cigar_shaped_debiased], sourcetablealias.[t07_rounded_a18_cigar_shaped_flag], sourcetablealias.[t08_odd_feature_a19_ring_count], sourcetablealias.[t08_odd_feature_a19_ring_weight], sourcetablealias.[t08_odd_feature_a19_ring_fraction], sourcetablealias.[t08_odd_feature_a19_ring_weighted_fraction], sourcetablealias.[t08_odd_feature_a19_ring_debiased], sourcetablealias.[t08_odd_feature_a19_ring_flag], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_count], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_weight], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_fraction], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_weighted_fraction], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_debiased], sourcetablealias.[t08_odd_feature_a20_lens_or_arc_flag], sourcetablealias.[t08_odd_feature_a21_disturbed_count], sourcetablealias.[t08_odd_feature_a21_disturbed_weight], sourcetablealias.[t08_odd_feature_a21_disturbed_fraction], sourcetablealias.[t08_odd_feature_a21_disturbed_weighted_fraction], sourcetablealias.[t08_odd_feature_a21_disturbed_debiased], sourcetablealias.[t08_odd_feature_a21_disturbed_flag], sourcetablealias.[t08_odd_feature_a22_irregular_count], sourcetablealias.[t08_odd_feature_a22_irregular_weight], sourcetablealias.[t08_odd_feature_a22_irregular_fraction], sourcetablealias.[t08_odd_feature_a22_irregular_weighted_fraction], sourcetablealias.[t08_odd_feature_a22_irregular_debiased], sourcetablealias.[t08_odd_feature_a22_irregular_flag], sourcetablealias.[t08_odd_feature_a23_other_count], sourcetablealias.[t08_odd_feature_a23_other_weight], sourcetablealias.[t08_odd_feature_a23_other_fraction], sourcetablealias.[t08_odd_feature_a23_other_weighted_fraction], sourcetablealias.[t08_odd_feature_a23_other_debiased], sourcetablealias.[t08_odd_feature_a23_other_flag], sourcetablealias.[t08_odd_feature_a24_merger_count], sourcetablealias.[t08_odd_feature_a24_merger_weight], sourcetablealias.[t08_odd_feature_a24_merger_fraction], sourcetablealias.[t08_odd_feature_a24_merger_weighted_fraction], sourcetablealias.[t08_odd_feature_a24_merger_debiased], sourcetablealias.[t08_odd_feature_a24_merger_flag], sourcetablealias.[t08_odd_feature_a38_dust_lane_count], sourcetablealias.[t08_odd_feature_a38_dust_lane_weight], sourcetablealias.[t08_odd_feature_a38_dust_lane_fraction], sourcetablealias.[t08_odd_feature_a38_dust_lane_weighted_fraction], sourcetablealias.[t08_odd_feature_a38_dust_lane_debiased], sourcetablealias.[t08_odd_feature_a38_dust_lane_flag], sourcetablealias.[t09_bulge_shape_a25_rounded_count], sourcetablealias.[t09_bulge_shape_a25_rounded_weight], sourcetablealias.[t09_bulge_shape_a25_rounded_fraction], sourcetablealias.[t09_bulge_shape_a25_rounded_weighted_fraction], sourcetablealias.[t09_bulge_shape_a25_rounded_debiased], sourcetablealias.[t09_bulge_shape_a25_rounded_flag], sourcetablealias.[t09_bulge_shape_a26_boxy_count], sourcetablealias.[t09_bulge_shape_a26_boxy_weight], sourcetablealias.[t09_bulge_shape_a26_boxy_fraction], sourcetablealias.[t09_bulge_shape_a26_boxy_weighted_fraction], sourcetablealias.[t09_bulge_shape_a26_boxy_debiased], sourcetablealias.[t09_bulge_shape_a26_boxy_flag], sourcetablealias.[t09_bulge_shape_a27_no_bulge_count], sourcetablealias.[t09_bulge_shape_a27_no_bulge_weight], sourcetablealias.[t09_bulge_shape_a27_no_bulge_fraction], sourcetablealias.[t09_bulge_shape_a27_no_bulge_weighted_fraction], sourcetablealias.[t09_bulge_shape_a27_no_bulge_debiased], sourcetablealias.[t09_bulge_shape_a27_no_bulge_flag], sourcetablealias.[t10_arms_winding_a28_tight_count], sourcetablealias.[t10_arms_winding_a28_tight_weight], sourcetablealias.[t10_arms_winding_a28_tight_fraction], sourcetablealias.[t10_arms_winding_a28_tight_weighted_fraction], sourcetablealias.[t10_arms_winding_a28_tight_debiased], sourcetablealias.[t10_arms_winding_a28_tight_flag], sourcetablealias.[t10_arms_winding_a29_medium_count], sourcetablealias.[t10_arms_winding_a29_medium_weight], sourcetablealias.[t10_arms_winding_a29_medium_fraction], sourcetablealias.[t10_arms_winding_a29_medium_weighted_fraction], sourcetablealias.[t10_arms_winding_a29_medium_debiased], sourcetablealias.[t10_arms_winding_a29_medium_flag], sourcetablealias.[t10_arms_winding_a30_loose_count], sourcetablealias.[t10_arms_winding_a30_loose_weight], sourcetablealias.[t10_arms_winding_a30_loose_fraction], sourcetablealias.[t10_arms_winding_a30_loose_weighted_fraction], sourcetablealias.[t10_arms_winding_a30_loose_debiased], sourcetablealias.[t10_arms_winding_a30_loose_flag], sourcetablealias.[t11_arms_number_a31_1_count], sourcetablealias.[t11_arms_number_a31_1_weight], sourcetablealias.[t11_arms_number_a31_1_fraction], sourcetablealias.[t11_arms_number_a31_1_weighted_fraction], sourcetablealias.[t11_arms_number_a31_1_debiased], sourcetablealias.[t11_arms_number_a31_1_flag], sourcetablealias.[t11_arms_number_a32_2_count], sourcetablealias.[t11_arms_number_a32_2_weight], sourcetablealias.[t11_arms_number_a32_2_fraction], sourcetablealias.[t11_arms_number_a32_2_weighted_fraction], sourcetablealias.[t11_arms_number_a32_2_debiased], sourcetablealias.[t11_arms_number_a32_2_flag], sourcetablealias.[t11_arms_number_a33_3_count], sourcetablealias.[t11_arms_number_a33_3_weight], sourcetablealias.[t11_arms_number_a33_3_fraction], sourcetablealias.[t11_arms_number_a33_3_weighted_fraction], sourcetablealias.[t11_arms_number_a33_3_debiased], sourcetablealias.[t11_arms_number_a33_3_flag], sourcetablealias.[t11_arms_number_a34_4_count], sourcetablealias.[t11_arms_number_a34_4_weight], sourcetablealias.[t11_arms_number_a34_4_fraction], sourcetablealias.[t11_arms_number_a34_4_weighted_fraction], sourcetablealias.[t11_arms_number_a34_4_debiased], sourcetablealias.[t11_arms_number_a34_4_flag], sourcetablealias.[t11_arms_number_a36_more_than_4_count], sourcetablealias.[t11_arms_number_a36_more_than_4_weight], sourcetablealias.[t11_arms_number_a36_more_than_4_fraction], sourcetablealias.[t11_arms_number_a36_more_than_4_weighted_fraction], sourcetablealias.[t11_arms_number_a36_more_than_4_debiased], sourcetablealias.[t11_arms_number_a36_more_than_4_flag], sourcetablealias.[t11_arms_number_a37_cant_tell_count], sourcetablealias.[t11_arms_number_a37_cant_tell_weight], sourcetablealias.[t11_arms_number_a37_cant_tell_fraction], sourcetablealias.[t11_arms_number_a37_cant_tell_weighted_fraction], sourcetablealias.[t11_arms_number_a37_cant_tell_debiased], sourcetablealias.[t11_arms_number_a37_cant_tell_flag]
 FROM   [SkyNode_SDSSDR12].[dbo].[zoo2Stripe82Normal] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.dr7objid = sourcetablealias.dr7objid
	;


GO

-- SUBSAMPLING TABLE 'zooConfidence' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specobjid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specobjid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zooConfidence] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specobjid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zooConfidence] WITH (TABLOCKX)
	([specobjid], [objid], [dr7objid], [ra], [dec], [rastring], [decstring], [f_unclass_clean], [f_misclass_clean], [avcorr_clean], [stdcorr_clean], [f_misclass_greater], [avcorr_greater], [stdcorr_greater])
 SELECT sourcetablealias.[specobjid], sourcetablealias.[objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[decstring], sourcetablealias.[f_unclass_clean], sourcetablealias.[f_misclass_clean], sourcetablealias.[avcorr_clean], sourcetablealias.[stdcorr_clean], sourcetablealias.[f_misclass_greater], sourcetablealias.[avcorr_greater], sourcetablealias.[stdcorr_greater]
 FROM   [SkyNode_SDSSDR12].[dbo].[zooConfidence] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specobjid = sourcetablealias.specobjid
	;


GO

-- SUBSAMPLING TABLE 'zooMirrorBias' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[dr7objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[dr7objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zooMirrorBias] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [dr7objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zooMirrorBias] WITH (TABLOCKX)
	([specobjid], [objid], [dr7objid], [ra], [dec], [rastring], [decstring], [nvote_mr1], [p_el_mr1], [p_cw_mr1], [p_acw_mr1], [p_edge_mr1], [p_dk_mr1], [p_mg_mr1], [p_cs_mr1], [nvote_mr2], [p_el_mr2], [p_cw_mr2], [p_acw_mr2], [p_edge_mr2], [p_dk_mr2], [p_mg_mr2], [p_cs_mr2])
 SELECT sourcetablealias.[specobjid], sourcetablealias.[objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[decstring], sourcetablealias.[nvote_mr1], sourcetablealias.[p_el_mr1], sourcetablealias.[p_cw_mr1], sourcetablealias.[p_acw_mr1], sourcetablealias.[p_edge_mr1], sourcetablealias.[p_dk_mr1], sourcetablealias.[p_mg_mr1], sourcetablealias.[p_cs_mr1], sourcetablealias.[nvote_mr2], sourcetablealias.[p_el_mr2], sourcetablealias.[p_cw_mr2], sourcetablealias.[p_acw_mr2], sourcetablealias.[p_edge_mr2], sourcetablealias.[p_dk_mr2], sourcetablealias.[p_mg_mr2], sourcetablealias.[p_cs_mr2]
 FROM   [SkyNode_SDSSDR12].[dbo].[zooMirrorBias] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.dr7objid = sourcetablealias.dr7objid
	;


GO

-- SUBSAMPLING TABLE 'zooMonochromeBias' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[dr7objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[dr7objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zooMonochromeBias] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [dr7objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zooMonochromeBias] WITH (TABLOCKX)
	([specobjid], [objid], [dr7objid], [ra], [dec], [rastring], [decstring], [nvote_mon], [p_el_mon], [p_cw_mon], [p_acw_mon], [p_edge_mon], [p_dk_mon], [p_mg_mon], [p_cs_mon])
 SELECT sourcetablealias.[specobjid], sourcetablealias.[objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[decstring], sourcetablealias.[nvote_mon], sourcetablealias.[p_el_mon], sourcetablealias.[p_cw_mon], sourcetablealias.[p_acw_mon], sourcetablealias.[p_edge_mon], sourcetablealias.[p_dk_mon], sourcetablealias.[p_mg_mon], sourcetablealias.[p_cs_mon]
 FROM   [SkyNode_SDSSDR12].[dbo].[zooMonochromeBias] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.dr7objid = sourcetablealias.dr7objid
	;


GO

-- SUBSAMPLING TABLE 'zooNoSpec' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[dr7objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[dr7objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zooNoSpec] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [dr7objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zooNoSpec] WITH (TABLOCKX)
	([specobjid], [objid], [dr7objid], [ra], [dec], [rastring], [nvote], [p_el], [p_cw], [p_acw], [p_edge], [p_dk], [p_mg], [p_cs])
 SELECT sourcetablealias.[specobjid], sourcetablealias.[objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[nvote], sourcetablealias.[p_el], sourcetablealias.[p_cw], sourcetablealias.[p_acw], sourcetablealias.[p_edge], sourcetablealias.[p_dk], sourcetablealias.[p_mg], sourcetablealias.[p_cs]
 FROM   [SkyNode_SDSSDR12].[dbo].[zooNoSpec] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.dr7objid = sourcetablealias.dr7objid
	;


GO

-- SUBSAMPLING TABLE 'zooSpec' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[specobjid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[specobjid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zooSpec] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [specobjid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zooSpec] WITH (TABLOCKX)
	([specobjid], [objid], [dr7objid], [ra], [dec], [rastring], [decstring], [nvote], [p_el], [p_cw], [p_acw], [p_edge], [p_dk], [p_mg], [p_cs], [p_el_debiased], [p_cs_debiased], [spiral], [elliptical], [uncertain])
 SELECT sourcetablealias.[specobjid], sourcetablealias.[objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[decstring], sourcetablealias.[nvote], sourcetablealias.[p_el], sourcetablealias.[p_cw], sourcetablealias.[p_acw], sourcetablealias.[p_edge], sourcetablealias.[p_dk], sourcetablealias.[p_mg], sourcetablealias.[p_cs], sourcetablealias.[p_el_debiased], sourcetablealias.[p_cs_debiased], sourcetablealias.[spiral], sourcetablealias.[elliptical], sourcetablealias.[uncertain]
 FROM   [SkyNode_SDSSDR12].[dbo].[zooSpec] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.specobjid = sourcetablealias.specobjid
	;


GO

-- SUBSAMPLING TABLE 'zooVotes' ---

 -- Create temporary table for the random ID list
 IF EXISTS( SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..##temporaryidlist'))
 DROP TABLE ##temporaryidlist;
 GO

 CREATE TABLE ##temporaryidlist
 (
	[dr7objid] bigint
 );
 
 -- Collect IDs
 WITH temporaryidlistquery AS
 (
	SELECT sourcetablealias.[dr7objid], master.dbo.RandomDouble() AS randomnumber
	FROM [SkyNode_SDSSDR12].[dbo].[zooVotes] sourcetablealias
	
 )
 INSERT ##temporaryidlist WITH (TABLOCKX)
 SELECT [dr7objid]
 FROM temporaryidlistquery
 WHERE randomnumber < 0.01;
 
 -- Insert subset into destination table
 
 INSERT [SkyNode_SDSSDR12_Mini].[dbo].[zooVotes] WITH (TABLOCKX)
	([specobjid], [objid], [dr7objid], [ra], [dec], [rastring], [decstring], [nvote_tot], [nvote_std], [nvote_mr1], [nvote_mr2], [nvote_mon], [p_el], [p_cw], [p_acw], [p_edge], [p_dk], [p_mg], [p_cs])
 SELECT sourcetablealias.[specobjid], sourcetablealias.[objid], sourcetablealias.[dr7objid], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[rastring], sourcetablealias.[decstring], sourcetablealias.[nvote_tot], sourcetablealias.[nvote_std], sourcetablealias.[nvote_mr1], sourcetablealias.[nvote_mr2], sourcetablealias.[nvote_mon], sourcetablealias.[p_el], sourcetablealias.[p_cw], sourcetablealias.[p_acw], sourcetablealias.[p_edge], sourcetablealias.[p_dk], sourcetablealias.[p_mg], sourcetablealias.[p_cs]
 FROM   [SkyNode_SDSSDR12].[dbo].[zooVotes] sourcetablealias WITH (NOLOCK)
	INNER JOIN ##temporaryidlist ON ##temporaryidlist.dr7objid = sourcetablealias.dr7objid
	;


GO

