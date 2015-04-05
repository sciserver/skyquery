
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_apogeeObject_apogee_id_j_h_k_j]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_apogeeObject_apogee_id_j_h_k_j] ON [dbo].[apogeeObject]
(
	[apogee_id] ASC,
	[j] ASC,
	[h] ASC,
	[k] ASC,
	[j_err] ASC,
	[h_err] ASC,
	[k_err] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_apogeeStar_apogee_id]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_apogeeStar_apogee_id] ON [dbo].[apogeeStar]
(
	[apogee_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_apogeeStar_htmID]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_apogeeStar_htmID] ON [dbo].[apogeeStar]
(
	[htmID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_apogeeVisit_apogee_id]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_apogeeVisit_apogee_id] ON [dbo].[apogeeVisit]
(
	[apogee_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_apogeeVisit_plate_mjd_fiberid]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_apogeeVisit_plate_mjd_fiberid] ON [dbo].[apogeeVisit]
(
	[plate] ASC,
	[mjd] ASC,
	[fiberid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_aspcapStar_apstar_id]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_aspcapStar_apstar_id] ON [dbo].[aspcapStar]
(
	[apstar_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_DataConstants_value]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_DataConstants_value] ON [dbo].[DataConstants]
(
	[value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_detectionIndex_thingID]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_detectionIndex_thingID] ON [dbo].[detectionIndex]
(
	[thingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_Field_field_camcol_run_rerun]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_Field_field_camcol_run_rerun] ON [dbo].[Field]
(
	[field] ASC,
	[camcol] ASC,
	[run] ASC,
	[rerun] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_Field_run_camcol_field_rerun]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_Field_run_camcol_field_rerun] ON [dbo].[Field]
(
	[run] ASC,
	[camcol] ASC,
	[field] ASC,
	[rerun] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_Frame_field_camcol_run_zoom_re]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_Frame_field_camcol_run_zoom_re] ON [dbo].[Frame]
(
	[field] ASC,
	[camcol] ASC,
	[run] ASC,
	[zoom] ASC,
	[rerun] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_Frame_htmID_zoom_cx_cy_cz_a_b_]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_Frame_htmID_zoom_cx_cy_cz_a_b_] ON [dbo].[Frame]
(
	[htmID] ASC,
	[zoom] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC,
	[a] ASC,
	[b] ASC,
	[c] ASC,
	[d] ASC,
	[e] ASC,
	[f] ASC,
	[node] ASC,
	[incl] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_HalfSpace_regionID_convexID_x_]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_HalfSpace_regionID_convexID_x_] ON [dbo].[HalfSpace]
(
	[regionid] ASC,
	[convexid] ASC,
	[x] ASC,
	[y] ASC,
	[z] ASC,
	[c] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_Mask_htmID_ra_dec_cx_cy_cz]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_Mask_htmID_ra_dec_cx_cy_cz] ON [dbo].[Mask]
(
	[htmID] ASC,
	[ra] ASC,
	[dec] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


/****** Object:  Index [i_PlateX_htmID_ra_dec_cx_cy_cz]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_PlateX_htmID_ra_dec_cx_cy_cz] ON [dbo].[PlateX]
(
	[htmID] ASC,
	[ra] ASC,
	[dec] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_RegionPatch_htmID_ra_dec_x_y_z]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_RegionPatch_htmID_ra_dec_x_y_z] ON [dbo].[RegionPatch]
(
	[htmid] ASC,
	[ra] ASC,
	[dec] ASC,
	[x] ASC,
	[y] ASC,
	[z] ASC,
	[type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_sdssTileAll_htmID_racen_deccen]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_sdssTileAll_htmID_racen_deccen] ON [dbo].[sdssTileAll]
(
	[htmID] ASC,
	[raCen] ASC,
	[decCen] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_sdssTileAll_tileRun_tile]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [i_sdssTileAll_tileRun_tile] ON [dbo].[sdssTileAll]
(
	[tileRun] ASC,
	[tile] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_sdssTiledTargetAll_htmID_ra_de]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_sdssTiledTargetAll_htmID_ra_de] ON [dbo].[sdssTiledTargetAll]
(
	[htmID] ASC,
	[ra] ASC,
	[dec] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_sdssTilingInfo_targetID_tileRu]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [i_sdssTilingInfo_targetID_tileRu] ON [dbo].[sdssTilingInfo]
(
	[targetID] ASC,
	[tileRun] ASC,
	[collisionGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_sdssTilingInfo_tile_collisionG]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_sdssTilingInfo_tile_collisionG] ON [dbo].[sdssTilingInfo]
(
	[tile] ASC,
	[collisionGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_sdssTilingInfo_tileRun_tid_col]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [i_sdssTilingInfo_tileRun_tid_col] ON [dbo].[sdssTilingInfo]
(
	[tileRun] ASC,
	[tid] ASC,
	[collisionGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_segueTargetAll_segue1_target1_]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_segueTargetAll_segue1_target1_] ON [dbo].[segueTargetAll]
(
	[segue1_target1] ASC,
	[segue2_target1] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_SpecObjAll_BestObjID_sourceTyp]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_SpecObjAll_BestObjID_sourceTyp] ON [dbo].[SpecObjAll]
(
	[bestObjID] ASC,
	[sourceType] ASC,
	[sciencePrimary] ASC,
	[class] ASC,
	[htmID] ASC,
	[ra] ASC,
	[dec] ASC,
	[plate] ASC,
	[mjd] ASC,
	[fiberID] ASC,
	[z] ASC,
	[zErr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_SpecObjAll_class_zWarning_z_sc]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_SpecObjAll_class_zWarning_z_sc] ON [dbo].[SpecObjAll]
(
	[class] ASC,
	[zWarning] ASC,
	[z] ASC,
	[sciencePrimary] ASC,
	[plateID] ASC,
	[bestObjID] ASC,
	[targetObjID] ASC,
	[htmID] ASC,
	[ra] ASC,
	[dec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_SpecObjAll_fluxObjID]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_SpecObjAll_fluxObjID] ON [dbo].[SpecObjAll]
(
	[fluxObjID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_SpecObjAll_htmID_ra_dec_cx_cy_]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_SpecObjAll_htmID_ra_dec_cx_cy_] ON [dbo].[SpecObjAll]
(
	[htmID] ASC,
	[ra] ASC,
	[dec] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC,
	[sciencePrimary] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_SpecObjAll_ra_dec_class_plat]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_SpecObjAll_ra_dec_class_plat] ON [dbo].[SpecObjAll]
(
	[ra] ASC,
	[dec] ASC,
	[class] ASC,
	[plate] ASC,
	[tile] ASC,
	[z] ASC,
	[zErr] ASC,
	[sciencePrimary] ASC,
	[plateID] ASC,
	[bestObjID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_SpecObjAll_targetObjID_sourceT]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_SpecObjAll_targetObjID_sourceT] ON [dbo].[SpecObjAll]
(
	[targetObjID] ASC,
	[sourceType] ASC,
	[sciencePrimary] ASC,
	[class] ASC,
	[htmID] ASC,
	[ra] ASC,
	[dec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_SpecPhotoAll_objID_sciencePrim]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_SpecPhotoAll_objID_sciencePrim] ON [dbo].[SpecPhotoAll]
(
	[objID] ASC,
	[sciencePrimary] ASC,
	[class] ASC,
	[z] ASC,
	[targetObjID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_SpecPhotoAll_targetObjID_scien]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_SpecPhotoAll_targetObjID_scien] ON [dbo].[SpecPhotoAll]
(
	[targetObjID] ASC,
	[sciencePrimary] ASC,
	[class] ASC,
	[z] ASC,
	[objID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_thingIndex_objID]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_thingIndex_objID] ON [dbo].[thingIndex]
(
	[objID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_TwoMass_ccflag]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_TwoMass_ccflag] ON [dbo].[TwoMass]
(
	[ccFlag] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_TwoMass_dec]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_TwoMass_dec] ON [dbo].[TwoMass]
(
	[dec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_TwoMass_h]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_TwoMass_h] ON [dbo].[TwoMass]
(
	[h] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_TwoMass_j]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_TwoMass_j] ON [dbo].[TwoMass]
(
	[j] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_TwoMass_k]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_TwoMass_k] ON [dbo].[TwoMass]
(
	[k] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [i_TwoMass_phqual]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_TwoMass_phqual] ON [dbo].[TwoMass]
(
	[phQual] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_TwoMass_ra]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_TwoMass_ra] ON [dbo].[TwoMass]
(
	[ra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_blend_ext_flags]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_blend_ext_flags] ON [dbo].[WISE_allsky]
(
	[blend_ext_flags] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_glat_glon]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_glat_glon] ON [dbo].[WISE_allsky]
(
	[glat] ASC,
	[glon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_h_m_2mass]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_h_m_2mass] ON [dbo].[WISE_allsky]
(
	[h_m_2mass] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_j_m_2mass]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_j_m_2mass] ON [dbo].[WISE_allsky]
(
	[j_m_2mass] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_k_m_2mass]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_k_m_2mass] ON [dbo].[WISE_allsky]
(
	[k_m_2mass] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_n_2mass]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_n_2mass] ON [dbo].[WISE_allsky]
(
	[n_2mass] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_ra_dec]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_ra_dec] ON [dbo].[WISE_allsky]
(
	[ra] ASC,
	[dec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF

GO
/****** Object:  Index [i_WISE_allsky_rjce]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_rjce] ON [dbo].[WISE_allsky]
(
	[rjce] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_tmass_key]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_tmass_key] ON [dbo].[WISE_allsky]
(
	[tmass_key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_w1cc_map]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_w1cc_map] ON [dbo].[WISE_allsky]
(
	[w1cc_map] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_w1mpro]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_w1mpro] ON [dbo].[WISE_allsky]
(
	[w1mpro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_w1rsemi]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_w1rsemi] ON [dbo].[WISE_allsky]
(
	[w1rsemi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_w2cc_map]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_w2cc_map] ON [dbo].[WISE_allsky]
(
	[w2cc_map] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_w2mpro]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_w2mpro] ON [dbo].[WISE_allsky]
(
	[w2mpro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_w3cc_map]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_w3cc_map] ON [dbo].[WISE_allsky]
(
	[w3cc_map] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_w3mpro]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_w3mpro] ON [dbo].[WISE_allsky]
(
	[w3mpro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_w4cc_map]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_w4cc_map] ON [dbo].[WISE_allsky]
(
	[w4cc_map] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_allsky_w4mpro]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_allsky_w4mpro] ON [dbo].[WISE_allsky]
(
	[w4mpro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_WISE_xmatch_wise_cntr]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_WISE_xmatch_wise_cntr] ON [dbo].[WISE_xmatch]
(
	[wise_cntr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_zooConfidence_objID]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_zooConfidence_objID] ON [dbo].[zooConfidence]
(
	[objid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_zooMirrorBias_objID]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_zooMirrorBias_objID] ON [dbo].[zooMirrorBias]
(
	[objid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_zooMonochromeBias_objID]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_zooMonochromeBias_objID] ON [dbo].[zooMonochromeBias]
(
	[objid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_zooNoSpec_objID]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_zooNoSpec_objID] ON [dbo].[zooNoSpec]
(
	[objid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_zooSpec_objID]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_zooSpec_objID] ON [dbo].[zooSpec]
(
	[objid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [i_zooVotes_objID]    Script Date: 4/3/2015 4:53:04 AM ******/
CREATE NONCLUSTERED INDEX [i_zooVotes_objID] ON [dbo].[zooVotes]
(
	[objid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
