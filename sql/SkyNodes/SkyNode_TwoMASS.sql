USE SkyNode_TwoMASS

CREATE TABLE [dbo].[PhotoObj](
--/ <summary>Point source survey objects</summary>
--/ <remarks>   This is the Point Source Catalog.</remarks>
	[objID] [bigint] NOT NULL, --/ <column>unique identification number for the PSC source</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">right ascension</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">declination</column>
  [cx] [float] NULL, --/ <column>Cartesian coordinate x</column>
	[cy] [float] NULL, --/ <column>Cartesian coordinate y</column>
	[cz] [float] NULL, --/ <column>Cartesian coordinate z</column>
	[htmid] [bigint] NULL, --/ <column>Unique HTM ID</column>
	[err_maj] [real] NOT NULL, --/ <column unit="arcsec">major axis of position error ellipse</column>
	[err_min] [real] NOT NULL, --/ <column unit="arcsec">minor axis of position error ellipse</column>
	[err_ang] [smallint] NOT NULL, --/ <column unit="deg">position angle of error ellipse major axis</column>
	[designation] [varchar](17) NOT NULL, --/ <column>source designation formed from sexigesimal coordinates</column>
	[j_m] [real] NOT NULL, --/ <column unit="mag">J default magnitude</column>
	[j_cmsig] [real] NOT NULL, --/ <column unit="mag">J corrected magnitude uncertainty</column>
	[j_msigcom] [real] NOT NULL, --/ <column unit="mag">J total mag uncertainty</column>
	[j_snr] [real] NOT NULL, --/ <column unit="mag">J band scan signal-to-noise ratio</column>
	[h_m] [real] NOT NULL, --/ <column unit="mag">H default magnitude</column>
	[h_cmsig] [real] NOT NULL, --/ <column unit="mag">H corrected magnitude uncertainty</column>
	[h_msigcom] [real] NOT NULL, --/ <column unit="mag">H total mag uncertainty</column>
	[h_snr] [real] NOT NULL, --/ <column unit="mag">H band scan signal-to-noise ratio</column>
	[k_m] [real] NOT NULL, --/ <column unit="mag">K default magnitude</column>
	[k_cmsig] [real] NOT NULL, --/ <column unit="mag">K corrected magnitude uncertainty</column>
	[k_msigcom] [real] NOT NULL, --/ <column unit="mag">K total mag uncertainty</column>
	[k_snr] [real] NOT NULL, --/ <column unit="mag">K band scan signal-to-noise ratio</column>
	[ph_qual] [varchar](32) NOT NULL, --/ <column>photometric quality flag</column>
	[rd_flg] [varchar](32) NOT NULL, --/ <column>source of JHK default mags (read flag)</column>
	[bl_flg] [varchar](32) NOT NULL, --/ <column>indicates no of JHK components fit to source (each digit 0|1|2)</column>
	[cc_flg] [varchar](32) NOT NULL, --/ <column>indicates artifact contamination and/or confusion</column>
	[ndet] [varchar](32) NOT NULL, --/ <column>frame detection statistics</column>
	[prox] [real] NOT NULL, --/ <column unit="arcsec">distance between this source and its nearest neighbor in the PSC</column>
	[pxpa] [smallint] NOT NULL, --/ <column unit="deg">position angle on the sky of the vector from the source to the nearest neighbor in the PSC</column>
	[pxcntr] [int] NOT NULL, --/ <column>pts_key value of the nearest source in the PSC</column>
	[gal_contam] [smallint] NOT NULL, --/ <column>indicates src associated with or contaminated by an ext. src</column>
	[mp_flg] [smallint] NOT NULL, --/ <column>source is positionally associated with an asteroid or comet</column>
	[hemis] [varchar](32) NOT NULL, --/ <column>hemisphere (N/S) of observation</column>
	[date] [varchar](32) NOT NULL, --/ <column>observation date</column>
	[scan] [smallint] NOT NULL, --/ <column>scan number (unique within date)</column>
	[glon] [real] NOT NULL, --/ <column unit="deg">galactic longitude, rounded to 0.001 deg</column>
	[glat] [real] NOT NULL, --/ <column unit="deg">galactic latitude, rounded to 0.001 deg</column>
	[x_scan] [real] NOT NULL, --/ <column unit="arcsec">mean cross-scan focal plane position of the source in the U-scan coordinate system</column>
	[jdate] [float] NOT NULL, --/ <column unit="day">Julian Date of the source measurement accurate to +-30 seconds</column>
	[j_psfchi] [real] NOT NULL, --/ <column>J band reduced chi-squared value of fit</column>
	[h_psfchi] [real] NOT NULL, --/ <column>H band reduced chi-squared value of fit</column>
	[k_psfchi] [real] NOT NULL, --/ <column>K band reduced chi-squared value of fit</column>
	[j_m_stdap] [real] NOT NULL, --/ <column unit="mag">J standard aperture magnitude or BF aperture-photometry mag</column>
	[j_msig_stdap] [real] NOT NULL, --/ <column unit="mag">J standard ap. mag/BF ap.-photometry mag uncertainty</column>
	[h_m_stdap] [real] NOT NULL, --/ <column unit="mag">H standard aperture magnitude or BF aperture-photometry mag</column>
	[h_msig_stdap] [real] NOT NULL, --/ <column unit="mag">H standard ap. mag/BF ap.-photometry mag uncertainty</column>
	[k_m_stdap] [real] NOT NULL, --/ <column unit="mag">K standard aperture magnitude or BF aperture-photometry mag</column>
	[k_msig_stdap] [real] NOT NULL, --/ <column unit="mag">K standard ap. mag/BF ap.-photometry mag uncertainty</column>
	[dist_edge_ns] [int] NOT NULL, --/ <column unit="arcsec">distance from the source to the nearest North or South scan edge</column>
	[dist_edge_ew] [int] NOT NULL, --/ <column unit="arcsec">distance from the source to the nearest East or West scan edge</column>
	[dist_edge_flg] [varchar](32) NOT NULL, --/ <column>two character flag that specifies to which scan edges a source lies closest</column>
	[dup_src] [smallint] NOT NULL, --/ <column>duplicate source flag</column>
	[use_src] [smallint] NOT NULL, --/ <column>use source flag</column>
	[a] [char](1) NOT NULL, --/ <column>catalog identifier of an optical source from either the Tycho 2 or USNO-A2.0 catalog</column>
	[dist_opt] [real] NOT NULL, --/ <column unit="arcsec">distance in arcsec relative to associated optical source</column>
	[phi_opt] [smallint] NOT NULL, --/ <column unit="deg">position angle relative to assocaited optical source</column>
	[b_m_opt] [real] NOT NULL, --/ <column unit="mag">catalog blue mag of associated optical source</column>
	[vr_m_opt] [real] NOT NULL, --/ <column unit="mag">catalog red magnitude of the associated optical source</column>
	[nopt_mchs] [smallint] NOT NULL, --/ <column>number of optical sources within 5 arcsec of 2MASS src</column>
	[ext_key] [int] NOT NULL, --/ <column>unique identification number of the record in the XSC that corresponds to this point source</column>
	[scan_key] [int] NOT NULL, --/ <column>unique identification number of the record in the Scan Information Table</column>
	[coadd_key] [int] NOT NULL, --/ <column>unique identification number of the record in the Atlas Image Data Table</column>
	[coadd] [smallint] NOT NULL, --/ <column>sequence number of the Atlas Image in which the position of this source falls</column>
	[j_h] [real] NOT NULL, --/ <column unit="mag">default J-H mag color</column>
	[h_k] [real] NOT NULL, --/ <column unit="mag">default H-K mag color</column>
	[j_k] [real] NOT NULL --/ <column unit="mag">default J-K mag color</column>
) ON [PRIMARY]

ALTER TABLE [dbo].[PhotoObj] ADD PRIMARY KEY CLUSTERED 
(
	[objID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

-- Index to support on the fly zone table creation
CREATE NONCLUSTERED INDEX [IX_PhotoObj_Zone] ON [dbo].[PhotoObj] 
(
	[dec] ASC,
	[ra] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


-- HTM index
CREATE NONCLUSTERED INDEX [IX_PhotoObj_HtmID] ON [dbo].[PhotoObj] 
(
	[htmid] ASC,
	[cx] ASC,
	[cy] ASC,
	[cz] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PhotoXSC](
--/ <summary></summary>
--/ <remarks></remarks>
	[jdate] [float] NOT NULL, --/ <column>Julian Date of the source measurement accurate to +30 seconds. (See 2MASS PSC documentation).</column>
	[designation] [varchar](17) NOT NULL, --/ <column>Sexagesimal, equatorial position-based source name in the form: hhmmssss+ddmmsss[ABC...].</column>
	[ra] [float] NOT NULL, --/ <column></column>
	[dec] [float] NOT NULL, --/ <column></column>
	[sup_ra] [float] NOT NULL, --/ <column>Super-coadd centroid RA (J2000 decimal deg).</column>
	[sup_dec] [float] NOT NULL, --/ <column>Super-coadd centroid Dec (J2000 decimal deg).</column>
	[glon] [real] NOT NULL, --/ <column></column>
	[glat] [real] NOT NULL, --/ <column></column>
	[density] [real] NOT NULL, --/ <column>Coadd log(density) of stars with k&amp;lt;14 mag.</column>
	[r_k20fe] [real] NOT NULL, --/ <column unit="mag">20mag/sq arcsec isophotal K fiducial ell. ap. semi-major axis.</column>
	[j_m_k20fe] [real] NOT NULL, --/ <column unit="mag">J 20mag/sq arcsec isophotal fiducial ell. ap. magnitude.</column>
	[j_msig_k20fe] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in 20mag/sq arcsec iso.fid.ell.mag.</column>
	[j_flg_k20fe] [smallint] NOT NULL, --/ <column>J confusion flag for 20mag/sq arcsec iso. fid. ell. mag.</column>
	[h_m_k20fe] [real] NOT NULL, --/ <column unit="mag">H 20mag/sq arcsec isophotal fiducial ell. ap. magnitude.</column>
	[h_msig_k20fe] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in 20mag/sq arcsec iso.fid.ell.mag.</column>
	[h_flg_k20fe] [smallint] NOT NULL, --/ <column></column>
	[k_m_k20fe] [real] NOT NULL, --/ <column unit="mag">K 20mag/sq arcsec isophotal fiducial ell. ap. magnitude.</column>
	[k_msig_k20fe] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in 20mag/sq arcsec iso.fid.ell.mag.</column>
	[k_flg_k20fe] [smallint] NOT NULL, --/ <column>K confusion flag for 20mag/sq arcsec iso. fid. ell. mag.</column>
	[r_3sig] [real] NOT NULL, --/ <column unit="arcsec">3-sigma K isophotal semi-major axis.</column>
	[j_ba] [real] NOT NULL, --/ <column>J minor/major axis ratio fit to the 3-sigma isophote.</column>
	[j_phi] [smallint] NOT NULL, --/ <column unit="deg">J angle to 3-sigma major axis (E of N).</column>
	[h_ba] [real] NOT NULL, --/ <column></column>
	[h_phi] [smallint] NOT NULL, --/ <column unit="deg">H angle to 3-sigma major axis (E of N).</column>
	[k_ba] [real] NOT NULL, --/ <column>K minor/major axis ratio fit to the 3-sigma isophote.</column>
	[k_phi] [smallint] NOT NULL, --/ <column unit="deg">K angle to 3-sigma major axis (E of N).</column>
	[sup_r_3sig] [real] NOT NULL, --/ <column>Super-coadd minor/major axis ratio fit to the 3-sigma isophote.</column>
	[sup_ba] [real] NOT NULL, --/ <column>K minor/major axis ratio fit to the 3-sigma isophote.</column>
	[sup_phi] [smallint] NOT NULL, --/ <column unit="deg">K angle to 3-sigma major axis (E of N).</column>
	[r_fe] [real] NOT NULL, --/ <column>K fiducial Kron elliptical aperture semi-major axis.</column>
	[j_m_fe] [real] NOT NULL, --/ <column unit="mag">J fiducial Kron elliptical aperture magnitude.</column>
	[j_msig_fe] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in fiducial Kron ell. mag.</column>
	[j_flg_fe] [smallint] NOT NULL, --/ <column>J confusion flag for fiducial Kron ell. mag.</column>
	[h_m_fe] [real] NOT NULL, --/ <column unit="mag">H fiducial Kron elliptical aperture magnitude.</column>
	[h_msig_fe] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in fiducial Kron ell. mag.</column>
	[h_flg_fe] [smallint] NOT NULL, --/ <column></column>
	[k_m_fe] [real] NOT NULL, --/ <column unit="mag">K fiducial Kron elliptical aperture magnitude.</column>
	[k_msig_fe] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in fiducial Kron ell. mag.</column>
	[k_flg_fe] [smallint] NOT NULL, --/ <column>K confusion flag for fiducial Kron ell. mag.</column>
	[r_ext] [real] NOT NULL, --/ <column unit="arcsec">extrapolation/total radius.</column>
	[j_m_ext] [real] NOT NULL, --/ <column unit="mag">J mag from fit extrapolation.</column>
	[j_msig_ext] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in mag from fit extrapolation.</column>
	[j_pchi] [real] NOT NULL, --/ <column>J chi^2 of fit to rad. profile</column>
	[h_m_ext] [real] NOT NULL, --/ <column unit="mag">H mag from fit extrapolation.</column>
	[h_msig_ext] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in mag from fit extrapolation.</column>
	[h_pchi] [real] NOT NULL, --/ <column>H chi^2 of fit to rad. profile</column>
	[k_m_ext] [real] NOT NULL, --/ <column unit="mag">K mag from fit extrapolation.</column>
	[k_msig_ext] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in mag from fit extrapolation.</column>
	[k_pchi] [real] NOT NULL, --/ <column>K chi^2 of fit to rad. profile</column>
	[j_r_eff] [real] NOT NULL, --/ <column unit="arcsec">J half-light (integrated half-flux point) radius.</column>
	[j_mnsurfb_eff] [real] NOT NULL, --/ <column unit="mag/sq. arcsec">J mean surface brightness at the half-light radius.</column>
	[h_r_eff] [real] NOT NULL, --/ <column unit="arcsec">H half-light (integrated half-flux point) radius.</column>
	[h_mnsurfb_eff] [real] NOT NULL, --/ <column unit="mag/sq. arcsec">H mean surface brightness at the half-light radius.</column>
	[k_r_eff] [real] NOT NULL, --/ <column unit="arcsec">K half-light (integrated half-flux point) radius.</column>
	[k_mnsurfb_eff] [real] NOT NULL, --/ <column unit="mag/sq. arcsec">K mean surface brightness at the half-light radius.</column>
	[j_con_indx] [real] NOT NULL, --/ <column>J concentration index r_75%/r_25%.</column>
	[h_con_indx] [real] NOT NULL, --/ <column></column>
	[k_con_indx] [real] NOT NULL, --/ <column>K concentration index r_75%/r_25%.</column>
	[j_peak] [real] NOT NULL, --/ <column unit="mag/sq. arcsec">J peak pixel brightness.</column>
	[h_peak] [real] NOT NULL, --/ <column unit="mag/sq. arcsec">H peak pixel brightness.</column>
	[k_peak] [real] NOT NULL, --/ <column unit="mag/sq. arcsec">K peak pixel brightness.</column>
	[j_5surf] [real] NOT NULL, --/ <column unit="mag/sq. arcsec">J central surface brightness (r&amp;lt;=5).</column>
	[h_5surf] [real] NOT NULL, --/ <column unit="mag/sq. arcsec">H central surface brightness (r&amp;lt;=5).</column>
	[k_5surf] [real] NOT NULL, --/ <column unit="mag/sq. arcsec">K central surface brightness (r&amp;lt;=5).</column>
	[e_score] [real] NOT NULL, --/ <column>extended score: 1(extended) &amp;lt; e_score &amp;lt; 2(point-like).</column>
	[g_score] [real] NOT NULL, --/ <column>galaxy score: 1(extended) &amp;lt; g_score &amp;lt; 2(point-like).</column>
	[vc] [smallint] NOT NULL, --/ <column>visual verification score for source.</column>
	[cc_flg] [char](1) NOT NULL, --/ <column>indicates artifact contamination and/or confusion.</column>
	[im_nx] [smallint] NOT NULL, --/ <column>size of postage stamp image in pixels.</column>
	[r_k20fc] [real] NOT NULL, --/ <column unit="arcsec">20mag/sq arcsec isophotal K fiducial circular ap. radius.</column>
	[j_m_k20fc] [real] NOT NULL, --/ <column unit="mag">J 20mag/sq arcsec isophotal fiducial circ. ap. mag.</column>
	[j_msig_k20fc] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in 20mag/sq arcsec iso.fid.circ. mag.</column>
	[j_flg_k20fc] [smallint] NOT NULL, --/ <column>J confusion flag for 20mag/sq arcsec iso. fid. circ. mag.</column>
	[h_m_k20fc] [real] NOT NULL, --/ <column unit="mag">H 20mag/sq arcsec isophotal fiducial circ. ap. mag.</column>
	[h_msig_k20fc] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in 20mag/sq arcsec iso.fid.circ. mag.</column>
	[h_flg_k20fc] [smallint] NOT NULL, --/ <column></column>
	[k_m_k20fc] [real] NOT NULL, --/ <column unit="mag">K 20mag/sq arcsec isophotal fiducial circ. ap. mag.</column>
	[k_msig_k20fc] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in 20mag/sq arcsec iso.fid.circ. mag.</column>
	[k_flg_k20fc] [smallint] NOT NULL, --/ <column>K confusion flag for 20mag/sq arcsec iso. fid. circ. mag.</column>
	[j_r_e] [real] NOT NULL, --/ <column unit="arcsec">J Kron elliptical aperture semi-major axis.</column>
	[j_m_e] [real] NOT NULL, --/ <column unit="mag">J Kron elliptical aperture magnitude</column>
	[j_msig_e] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in Kron elliptical mag.</column>
	[j_flg_e] [smallint] NOT NULL, --/ <column>J confusion flag for Kron elliptical mag.</column>
	[h_r_e] [real] NOT NULL, --/ <column unit="arcsec">H Kron elliptical aperture semi-major axis.</column>
	[h_m_e] [real] NOT NULL, --/ <column unit="mag">H Kron elliptical aperture magnitude</column>
	[h_msig_e] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in Kron elliptical mag.</column>
	[h_flg_e] [smallint] NOT NULL, --/ <column></column>
	[k_r_e] [real] NOT NULL, --/ <column unit="arcsec">K Kron elliptical aperture semi-major axis.</column>
	[k_m_e] [real] NOT NULL, --/ <column unit="mag">K Kron elliptical aperture magnitude</column>
	[k_msig_e] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in Kron elliptical mag.</column>
	[k_flg_e] [smallint] NOT NULL, --/ <column>K confusion flag for Kron elliptical mag.</column>
	[j_r_c] [real] NOT NULL, --/ <column unit="arcsec">J Kron circular aperture radius.</column>
	[j_m_c] [real] NOT NULL, --/ <column unit="mag">J Kron circular aperture magnitude.</column>
	[j_msig_c] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in Kron circular mag.</column>
	[j_flg_c] [smallint] NOT NULL, --/ <column>J confusion flag for Kron circular mag.</column>
	[h_r_c] [real] NOT NULL, --/ <column unit="arcsec">H Kron circular aperture radius.</column>
	[h_m_c] [real] NOT NULL, --/ <column unit="mag">H Kron circular aperture magnitude.</column>
	[h_msig_c] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in Kron circular mag.</column>
	[h_flg_c] [smallint] NOT NULL, --/ <column></column>
	[k_r_c] [real] NOT NULL, --/ <column unit="arcsec">K Kron circular aperture radius.</column>
	[k_m_c] [real] NOT NULL, --/ <column unit="mag">K Kron circular aperture magnitude.</column>
	[k_msig_c] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in Kron circular mag.</column>
	[k_flg_c] [smallint] NOT NULL, --/ <column>K confusion flag for Kron circular mag.</column>
	[r_fc] [real] NOT NULL, --/ <column unit="arcsec">K fiducial Kron circular aperture radius.</column>
	[j_m_fc] [real] NOT NULL, --/ <column unit="mag">J fiducial Kron circular magnitude.</column>
	[j_msig_fc] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in fiducial Kron circ. mag.</column>
	[j_flg_fc] [smallint] NOT NULL, --/ <column>J confusion flag for Kron circular mag. confusion flag for fiducial Kron circ. mag.</column>
	[h_m_fc] [real] NOT NULL, --/ <column unit="mag">H fiducial Kron circular magnitude.</column>
	[h_msig_fc] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in fiducial Kron circ. mag.</column>
	[h_flg_fc] [smallint] NOT NULL, --/ <column></column>
	[k_m_fc] [real] NOT NULL, --/ <column unit="mag">K fiducial Kron circular magnitude.</column>
	[k_msig_fc] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in fiducial Kron circ. mag.</column>
	[k_flg_fc] [smallint] NOT NULL, --/ <column>K confusion flag for Kron circular mag. confusion flag for fiducial Kron circ. mag.</column>
	[j_r_i20e] [real] NOT NULL, --/ <column unit="arcsec">J 20mag/sq arcsec isophotal elliptical ap. semi-major axis.</column>
	[j_m_i20e] [real] NOT NULL, --/ <column unit="mag">J 20mag/sq arcsec isophotal elliptical ap. magnitude.</column>
	[j_msig_i20e] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in 20mag/sq arcsec iso. ell. mag.</column>
	[j_flg_i20e] [smallint] NOT NULL, --/ <column>J confusion flag for 20mag/sq arcsec iso. ell. mag.</column>
	[h_r_i20e] [real] NOT NULL, --/ <column unit="arcsec">H 20mag/sq arcsec isophotal elliptical ap. semi-major axis.</column>
	[h_m_i20e] [real] NOT NULL, --/ <column unit="mag">H 20mag/sq arcsec isophotal elliptical ap. magnitude.</column>
	[h_msig_i20e] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in 20mag/sq arcsec iso. ell. mag.</column>
	[h_flg_i20e] [smallint] NOT NULL, --/ <column></column>
	[k_r_i20e] [real] NOT NULL, --/ <column unit="arcsec">K 20mag/sq arcsec isophotal elliptical ap. semi-major axis.</column>
	[k_m_i20e] [real] NOT NULL, --/ <column unit="mag">K 20mag/sq arcsec isophotal elliptical ap. magnitude.</column>
	[k_msig_i20e] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in 20mag/sq arcsec iso. ell. mag.</column>
	[k_flg_i20e] [smallint] NOT NULL, --/ <column>K confusion flag for 20mag/sq arcsec iso. ell. mag.</column>
	[j_r_i20c] [real] NOT NULL, --/ <column unit="arcsec">J 20mag/sq arcsec isophotal circular aperture radius.</column>
	[j_m_i20c] [real] NOT NULL, --/ <column unit="mag">J 20mag/sq arcsec isophotal circular ap. magnitude.</column>
	[j_msig_i20c] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in 20mag/sq arcsec iso. circ. mag.</column>
	[j_flg_i20c] [smallint] NOT NULL, --/ <column>J confusion flag for 20mag/sq arcsec iso. circ. mag.</column>
	[h_r_i20c] [real] NOT NULL, --/ <column unit="arcsec">H 20mag/sq arcsec isophotal circular aperture radius.</column>
	[h_m_i20c] [real] NOT NULL, --/ <column unit="mag">H 20mag/sq arcsec isophotal circular ap. magnitude.</column>
	[h_msig_i20c] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in 20mag/sq arcsec iso. circ. mag.</column>
	[h_flg_i20c] [smallint] NOT NULL, --/ <column></column>
	[k_r_i20c] [real] NOT NULL, --/ <column unit="arcsec">K 20mag/sq arcsec isophotal circular aperture radius.</column>
	[k_m_i20c] [real] NOT NULL, --/ <column unit="mag">K 20mag/sq arcsec isophotal circular ap. magnitude.</column>
	[k_msig_i20c] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in 20mag/sq arcsec iso. circ. mag.</column>
	[k_flg_i20c] [smallint] NOT NULL, --/ <column>K confusion flag for 20mag/sq arcsec iso. circ. mag.</column>
	[j_r_i21e] [real] NOT NULL, --/ <column unit="arcsec">J 21mag/sq arcsec isophotal elliptical ap. semi-major axis.</column>
	[j_m_i21e] [real] NOT NULL, --/ <column unit="mag">J 21mag/sq arcsec isophotal elliptical ap. magnitude.</column>
	[j_msig_i21e] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in 21mag/sq arcsec iso. ell. mag.</column>
	[j_flg_i21e] [smallint] NOT NULL, --/ <column>J confusion flag for 21mag/sq arcsec iso. ell. mag.</column>
	[h_r_i21e] [real] NOT NULL, --/ <column unit="arcsec">H 21mag/sq arcsec isophotal elliptical ap. semi-major axis.</column>
	[h_m_i21e] [real] NOT NULL, --/ <column unit="mag">H 21mag/sq arcsec isophotal elliptical ap. magnitude.</column>
	[h_msig_i21e] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in 21mag/sq arcsec iso. ell. mag.</column>
	[h_flg_i21e] [smallint] NOT NULL, --/ <column></column>
	[k_r_i21e] [real] NOT NULL, --/ <column unit="arcsec">K 21mag/sq arcsec isophotal elliptical ap. semi-major axis.</column>
	[k_m_i21e] [real] NOT NULL, --/ <column unit="mag">K 21mag/sq arcsec isophotal elliptical ap. magnitude.</column>
	[k_msig_i21e] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in 21mag/sq arcsec iso. ell. mag.</column>
	[k_flg_i21e] [smallint] NOT NULL, --/ <column>K confusion flag for 21mag/sq arcsec iso. ell. mag.</column>
	[r_j21fe] [real] NOT NULL, --/ <column></column>
	[j_m_j21fe] [real] NOT NULL, --/ <column unit="mag">J 21mag/sq arcsec isophotal fiducial ell. ap. magnitude.</column>
	[j_msig_j21fe] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in 21mag/sq arcsec iso.fid.ell.mag.</column>
	[j_flg_j21fe] [smallint] NOT NULL, --/ <column>J confusion flag for 21mag/sq arcsec iso. fid. ell. mag.</column>
	[h_m_j21fe] [real] NOT NULL, --/ <column unit="mag">H 21mag/sq arcsec isophotal fiducial ell. ap. magnitude.</column>
	[h_msig_j21fe] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in 21mag/sq arcsec iso.fid.ell.mag.</column>
	[h_flg_j21fe] [smallint] NOT NULL, --/ <column></column>
	[k_m_j21fe] [real] NOT NULL, --/ <column unit="mag">K 21mag/sq arcsec isophotal fiducial ell. ap. magnitude.</column>
	[k_msig_j21fe] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in 21mag/sq arcsec iso.fid.ell.mag.</column>
	[k_flg_j21fe] [smallint] NOT NULL, --/ <column>K confusion flag for 21mag/sq arcsec iso. fid. ell. mag.</column>
	[j_r_i21c] [real] NOT NULL, --/ <column unit="arcsec">J 21mag/sq arcsec isophotal circular aperture radius.</column>
	[j_m_i21c] [real] NOT NULL, --/ <column unit="mag">J 21mag/sq arcsec isophotal circular ap. magnitude.</column>
	[j_msig_i21c] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in 21mag/sq arcsec iso. circ. mag.</column>
	[j_flg_i21c] [smallint] NOT NULL, --/ <column>J confusion flag for 21mag/sq arcsec iso. circ. mag.</column>
	[h_r_i21c] [real] NOT NULL, --/ <column unit="arcsec">H 21mag/sq arcsec isophotal circular aperture radius.</column>
	[h_m_i21c] [real] NOT NULL, --/ <column unit="mag">H 21mag/sq arcsec isophotal circular ap. magnitude.</column>
	[h_msig_i21c] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in 21mag/sq arcsec iso. circ. mag.</column>
	[h_flg_i21c] [smallint] NOT NULL, --/ <column></column>
	[k_r_i21c] [real] NOT NULL, --/ <column unit="arcsec">K 21mag/sq arcsec isophotal circular aperture radius.</column>
	[k_m_i21c] [real] NOT NULL, --/ <column unit="mag">K 21mag/sq arcsec isophotal circular ap. magnitude.</column>
	[k_msig_i21c] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in 21mag/sq arcsec iso. circ. mag.</column>
	[k_flg_i21c] [smallint] NOT NULL, --/ <column>K confusion flag for 21mag/sq arcsec iso. circ. mag.</column>
	[r_j21fc] [real] NOT NULL, --/ <column unit="arcsec">21mag/sq arcsec isophotal J fiducial circular ap. radius.</column>
	[j_m_j21fc] [real] NOT NULL, --/ <column unit="mag">J 21mag/sq arcsec isophotal fiducial circ. ap. mag.</column>
	[j_msig_j21fc] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in 21mag/sq arcsec iso.fid.circ.mag.</column>
	[j_flg_j21fc] [smallint] NOT NULL, --/ <column>J confusion flag for 21mag/sq arcsec iso. fid. circ. mag.</column>
	[h_m_j21fc] [real] NOT NULL, --/ <column unit="mag">H 21mag/sq arcsec isophotal fiducial circ. ap. mag.</column>
	[h_msig_j21fc] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in 21mag/sq arcsec iso.fid.circ.mag.</column>
	[h_flg_j21fc] [smallint] NOT NULL, --/ <column></column>
	[k_m_j21fc] [real] NOT NULL, --/ <column unit="mag">K 21mag/sq arcsec isophotal fiducial circ. ap. mag.</column>
	[k_msig_j21fc] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in 21mag/sq arcsec iso.fid.circ.mag.</column>
	[k_flg_j21fc] [smallint] NOT NULL, --/ <column>K confusion flag for 21mag/sq arcsec iso. fid. circ. mag.</column>
	[j_m_5] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (5 arcsec radius)</column>
	[j_msig_5] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (5 arcsec radius)</column>
	[j_flg_5] [smallint] NOT NULL, --/ <column>J confusion flag for 5 arcsec circular ap. mag.</column>
	[h_m_5] [real] NOT NULL, --/ <column></column>
	[h_msig_5] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (5 arcsec radius)</column>
	[h_flg_5] [smallint] NOT NULL, --/ <column></column>
	[k_m_5] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (5 arcsec radius)</column>
	[k_msig_5] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (5 arcsec radius)</column>
	[k_flg_5] [smallint] NOT NULL, --/ <column>K confusion flag for 5 arcsec circular ap. mag.</column>
	[j_m_7] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (7 arcsec radius)</column>
	[j_msig_7] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (7 arcsec radius)</column>
	[j_flg_7] [smallint] NOT NULL, --/ <column>J confusion flag for 7 arcsec circular ap. mag.</column>
	[h_m_7] [real] NOT NULL, --/ <column unit="mag">H-band circular aperture magnitude (7 arcsec radius)</column>
	[h_msig_7] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (7 arcsec radius)</column>
	[h_flg_7] [smallint] NOT NULL, --/ <column></column>
	[k_m_7] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (7 arcsec radius)</column>
	[k_msig_7] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (7 arcsec radius)</column>
	[k_flg_7] [smallint] NOT NULL, --/ <column>K confusion flag for 7 arcsec circular ap. mag.</column>
	[j_m_10] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (10 arcsec radius)</column>
	[j_msig_10] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (10 arcsec radius)</column>
	[j_flg_10] [smallint] NOT NULL, --/ <column>J confusion flag for 10 arcsec circular ap. mag.</column>
	[h_m_10] [real] NOT NULL, --/ <column></column>
	[h_msig_10] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (10 arcsec radius)</column>
	[h_flg_10] [smallint] NOT NULL, --/ <column></column>
	[k_m_10] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (10 arcsec radius)</column>
	[k_msig_10] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (10 arcsec radius)</column>
	[k_flg_10] [smallint] NOT NULL, --/ <column>K confusion flag for 10 arcsec circular ap. mag.</column>
	[j_m_15] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (15 arcsec radius)</column>
	[j_msig_15] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (15 arcsec radius)</column>
	[j_flg_15] [smallint] NOT NULL, --/ <column>J confusion flag for 15 arcsec circular ap. mag.</column>
	[h_m_15] [real] NOT NULL, --/ <column></column>
	[h_msig_15] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (15 arcsec radius)</column>
	[h_flg_15] [smallint] NOT NULL, --/ <column></column>
	[k_m_15] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (15 arcsec radius)</column>
	[k_msig_15] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (15 arcsec radius)</column>
	[k_flg_15] [smallint] NOT NULL, --/ <column>K confusion flag for 15 arcsec circular ap. mag.</column>
	[j_m_20] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (20 arcsec radius)</column>
	[j_msig_20] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (20 arcsec radius)</column>
	[j_flg_20] [smallint] NOT NULL, --/ <column>J confusion flag for 20 arcsec circular ap. mag.</column>
	[h_m_20] [real] NOT NULL, --/ <column></column>
	[h_msig_20] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (20 arcsec radius)</column>
	[h_flg_20] [smallint] NOT NULL, --/ <column></column>
	[k_m_20] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (20 arcsec radius)</column>
	[k_msig_20] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (20 arcsec radius)</column>
	[k_flg_20] [smallint] NOT NULL, --/ <column>K confusion flag for 20 arcsec circular ap. mag.</column>
	[j_m_25] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (25 arcsec radius)</column>
	[j_msig_25] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (25 arcsec radius)</column>
	[j_flg_25] [smallint] NOT NULL, --/ <column>J confusion flag for 25 arcsec circular ap. mag.</column>
	[h_m_25] [real] NOT NULL, --/ <column></column>
	[h_msig_25] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (25 arcsec radius)</column>
	[h_flg_25] [smallint] NOT NULL, --/ <column></column>
	[k_m_25] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (25 arcsec radius)</column>
	[k_msig_25] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (25 arcsec radius)</column>
	[k_flg_25] [smallint] NOT NULL, --/ <column>K confusion flag for 25 arcsec circular ap. mag.</column>
	[j_m_30] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (30 arcsec radius)</column>
	[j_msig_30] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (30 arcsec radius)</column>
	[j_flg_30] [smallint] NOT NULL, --/ <column>J confusion flag for 30 arcsec circular ap. mag.</column>
	[h_m_30] [real] NOT NULL, --/ <column></column>
	[h_msig_30] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (30 arcsec radius)</column>
	[h_flg_30] [smallint] NOT NULL, --/ <column></column>
	[k_m_30] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (30 arcsec radius)</column>
	[k_msig_30] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (30 arcsec radius)</column>
	[k_flg_30] [smallint] NOT NULL, --/ <column>K confusion flag for 30 arcsec circular ap. mag.</column>
	[j_m_40] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (40 arcsec radius)</column>
	[j_msig_40] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (40 arcsec radius)</column>
	[j_flg_40] [smallint] NOT NULL, --/ <column>J confusion flag for 40 arcsec circular ap. mag.</column>
	[h_m_40] [real] NOT NULL, --/ <column></column>
	[h_msig_40] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (40 arcsec radius)</column>
	[h_flg_40] [smallint] NOT NULL, --/ <column></column>
	[k_m_40] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (40 arcsec radius)</column>
	[k_msig_40] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (40 arcsec radius)</column>
	[k_flg_40] [smallint] NOT NULL, --/ <column>K confusion flag for 40 arcsec circular ap. mag.</column>
	[j_m_50] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (50 arcsec radius)</column>
	[j_msig_50] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (50 arcsec radius)</column>
	[j_flg_50] [smallint] NOT NULL, --/ <column>J confusion flag for 50 arcsec circular ap. mag.</column>
	[h_m_50] [real] NOT NULL, --/ <column></column>
	[h_msig_50] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (50 arcsec radius)</column>
	[h_flg_50] [smallint] NOT NULL, --/ <column></column>
	[k_m_50] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (50 arcsec radius)</column>
	[k_msig_50] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (50 arcsec radius)</column>
	[k_flg_50] [smallint] NOT NULL, --/ <column>K confusion flag for 50 arcsec circular ap. mag.</column>
	[j_m_60] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (60 arcsec radius)</column>
	[j_msig_60] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (60 arcsec radius)</column>
	[j_flg_60] [smallint] NOT NULL, --/ <column>J confusion flag for 60 arcsec circular ap. mag.</column>
	[h_m_60] [real] NOT NULL, --/ <column unit="mag">H-band circular aperture magnitude (60 arcsec radius)</column>
	[h_msig_60] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (60 arcsec radius)</column>
	[h_flg_60] [smallint] NOT NULL, --/ <column></column>
	[k_m_60] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (60 arcsec radius)</column>
	[k_msig_60] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (60 arcsec radius)</column>
	[k_flg_60] [smallint] NOT NULL, --/ <column>K confusion flag for 60 arcsec circular ap. mag.</column>
	[j_m_70] [real] NOT NULL, --/ <column unit="mag">J-band circular aperture magnitude (70 arcsec radius)</column>
	[j_msig_70] [real] NOT NULL, --/ <column unit="mag">error in J-band circular aperture magnitude (70 arcsec radius)</column>
	[j_flg_70] [smallint] NOT NULL, --/ <column>J confusion flag for 70 arcsec circular ap. mag.</column>
	[h_m_70] [real] NOT NULL, --/ <column unit="mag">H-band circular aperture magnitude (70 arcsec radius)</column>
	[h_msig_70] [real] NOT NULL, --/ <column unit="mag">error in H-band circular aperture magnitude (70 arcsec radius)</column>
	[h_flg_70] [smallint] NOT NULL, --/ <column></column>
	[k_m_70] [real] NOT NULL, --/ <column unit="mag">K-band circular aperture magnitude (70 arcsec radius)</column>
	[k_msig_70] [real] NOT NULL, --/ <column unit="mag">error in K-band circular aperture magnitude (70 arcsec radius)</column>
	[k_flg_70] [smallint] NOT NULL, --/ <column>K confusion flag for 70 arcsec circular ap. mag.</column>
	[j_m_sys] [real] NOT NULL, --/ <column unit="mag">J system photometry magnitude.</column>
	[j_msig_sys] [real] NOT NULL, --/ <column unit="mag">J 1-sigma uncertainty in system photometry mag.</column>
	[h_m_sys] [real] NOT NULL, --/ <column unit="mag">H system photometry magnitude.</column>
	[h_msig_sys] [real] NOT NULL, --/ <column unit="mag">H 1-sigma uncertainty in system photometry mag.</column>
	[k_m_sys] [real] NOT NULL, --/ <column unit="mag">K system photometry magnitude.</column>
	[k_msig_sys] [real] NOT NULL, --/ <column unit="mag">K 1-sigma uncertainty in system photometry mag.</column>
	[sys_flg] [smallint] NOT NULL, --/ <column>system flag: 0=no system, 1=nearby galaxy flux incl. in mag.</column>
	[contam_flg] [smallint] NOT NULL, --/ <column>contamination flag: 0=no stars, 1=stellar flux incl. in mag.</column>
	[j_5sig_ba] [real] NOT NULL, --/ <column>J minor/major axis ratio fit to the 5-sigma isophote.</column>
	[j_5sig_phi] [smallint] NOT NULL, --/ <column unit="deg">J angle to 5-sigma major axis (E of N).</column>
	[h_5sig_ba] [real] NOT NULL, --/ <column>H minor/major axis ratio fit to the 5-sigma isophote.</column>
	[h_5sig_phi] [smallint] NOT NULL, --/ <column unit="deg">H angle to 5-sigma major axis (E of N).</column>
	[k_5sig_ba] [real] NOT NULL, --/ <column>K minor/major axis ratio fit to the 5-sigma isophote.</column>
	[k_5sig_phi] [smallint] NOT NULL, --/ <column unit="deg">K angle to 5-sigma major axis (E of N).</column>
	[j_d_area] [smallint] NOT NULL, --/ <column unit="sq arcsec">J 5-sigma to 3-sigma differential area.</column>
	[j_perc_darea] [smallint] NOT NULL, --/ <column unit="sq arcsec">J 5-sigma to 3-sigma percent area change.</column>
	[h_d_area] [smallint] NOT NULL, --/ <column></column>
	[h_perc_darea] [smallint] NOT NULL, --/ <column unit="sq arcsec">H 5-sigma to 3-sigma percent area change.</column>
	[k_d_area] [smallint] NOT NULL, --/ <column unit="sq arcsec">K 5-sigma to 3-sigma differential area.</column>
	[k_perc_darea] [smallint] NOT NULL, --/ <column unit="sq arcsec">K 5-sigma to 3-sigma percent area change.</column>
	[j_bisym_rat] [real] NOT NULL, --/ <column>J bi-symmetric flux ratio.</column>
	[j_bisym_chi] [real] NOT NULL, --/ <column>J bi-symmetric cross-correlation chi.</column>
	[h_bisym_rat] [real] NOT NULL, --/ <column></column>
	[h_bisym_chi] [real] NOT NULL, --/ <column></column>
	[k_bisym_rat] [real] NOT NULL, --/ <column>K bi-symmetric flux ratio.</column>
	[k_bisym_chi] [real] NOT NULL, --/ <column>K bi-symmetric cross-correlation chi.</column>
	[j_sh0] [real] NOT NULL, --/ <column>J ridge shape (LCSB: BSNR limit).</column>
	[j_sig_sh0] [real] NOT NULL, --/ <column>J ridge shape sigma (LCSB: B2SNR limit).</column>
	[h_sh0] [real] NOT NULL, --/ <column>H ridge shape (LCSB: BSNR limit).</column>
	[h_sig_sh0] [real] NOT NULL, --/ <column>H ridge shape sigma (LCSB: B2SNR limit).</column>
	[k_sh0] [real] NOT NULL, --/ <column>K ridge shape (LCSB: BSNR limit).</column>
	[k_sig_sh0] [real] NOT NULL, --/ <column>K ridge shape sigma (LCSB: B2SNR limit).</column>
	[j_sc_mxdn] [real] NOT NULL, --/ <column>J mxdn (score) (LCSB: BSNR - block/smoothed SNR)</column>
	[j_sc_sh] [real] NOT NULL, --/ <column>J shape (score).</column>
	[j_sc_wsh] [real] NOT NULL, --/ <column>J wsh (score) (LCSB: PSNR - peak raw SNR).</column>
	[j_sc_r23] [real] NOT NULL, --/ <column>J r23 (score) (LCSB: TSNR - integrated SNR for r=15).</column>
	[j_sc_1mm] [real] NOT NULL, --/ <column>J 1st moment (score) (LCSB: super blk 2,4,8 SNR).</column>
	[j_sc_2mm] [real] NOT NULL, --/ <column>J 2nd moment (score) (LCSB: SNRMAX - super SNR max).</column>
	[j_sc_vint] [real] NOT NULL, --/ <column>J vint (score).</column>
	[j_sc_r1] [real] NOT NULL, --/ <column>J r1 (score).</column>
	[j_sc_msh] [real] NOT NULL, --/ <column>J median shape score.</column>
	[h_sc_mxdn] [real] NOT NULL, --/ <column>H mxdn (score) (LCSB: BSNR - block/smoothed SNR)</column>
	[h_sc_sh] [real] NOT NULL, --/ <column>H shape (score).</column>
	[h_sc_wsh] [real] NOT NULL, --/ <column>H wsh (score) (LCSB: PSNR - peak raw SNR).</column>
	[h_sc_r23] [real] NOT NULL, --/ <column>H r23 (score) (LCSB: TSNR - integrated SNR for r=15).</column>
	[h_sc_1mm] [real] NOT NULL, --/ <column>H 1st moment (score) (LCSB: super blk 2,4,8 SNR).</column>
	[h_sc_2mm] [real] NOT NULL, --/ <column>H 2nd moment (score) (LCSB: SNRMAX - super SNR max).</column>
	[h_sc_vint] [real] NOT NULL, --/ <column>H vint (score).</column>
	[h_sc_r1] [real] NOT NULL, --/ <column>H r1 (score).</column>
	[h_sc_msh] [real] NOT NULL, --/ <column>H median shape score.</column>
	[k_sc_mxdn] [real] NOT NULL, --/ <column>K mxdn (score) (LCSB: BSNR - block/smoothed SNR)</column>
	[k_sc_sh] [real] NOT NULL, --/ <column>K shape (score).</column>
	[k_sc_wsh] [real] NOT NULL, --/ <column>K wsh (score) (LCSB: PSNR - peak raw SNR).</column>
	[k_sc_r23] [real] NOT NULL, --/ <column>K r23 (score) (LCSB: TSNR - integrated SNR for r=15).</column>
	[k_sc_1mm] [real] NOT NULL, --/ <column>K 1st moment (score) (LCSB: super blk 2,4,8 SNR).</column>
	[k_sc_2mm] [real] NOT NULL, --/ <column>K 2nd moment (score) (LCSB: SNRMAX - super SNR max).</column>
	[k_sc_vint] [real] NOT NULL, --/ <column>K vint (score).</column>
	[k_sc_r1] [real] NOT NULL, --/ <column>K r1 (score).</column>
	[k_sc_msh] [real] NOT NULL, --/ <column>K median shape score.</column>
	[j_chif_ellf] [real] NOT NULL, --/ <column>J % chi-fraction for elliptical fit to 3-sig isophote.</column>
	[k_chif_ellf] [real] NOT NULL, --/ <column>K % chi-fraction for elliptical fit to 3-sig isophote.</column>
	[ellfit_flg] [smallint] NOT NULL, --/ <column>ellipse fitting contamination flag.</column>
	[sup_chif_ellf] [real] NOT NULL, --/ <column>super-coadd % chi-fraction for ellip. fit to 3-sig isophote.</column>
	[n_blank] [smallint] NOT NULL, --/ <column>number of blanked source records.</column>
	[n_sub] [smallint] NOT NULL, --/ <column>number of subtracted source records.</column>
	[bl_sub_flg] [smallint] NOT NULL, --/ <column>blanked/subtracted src description flag.</column>
	[id_flg] [smallint] NOT NULL, --/ <column>type/galaxy ID flag (0=non-catalog, 1=catalog, 2=LCSB).</column>
	[id_cat] [varchar](20) NOT NULL, --/ <column>matched galaxy catalog name.</column>
	[fg_flg] [varchar](6) NOT NULL, --/ <column>flux-growth convergence flag.</column>
	[blk_fac] [smallint] NOT NULL, --/ <column>LCSB blocking factor (1, 2, 4, 8).</column>
	[dup_src] [smallint] NOT NULL, --/ <column>Duplicate source flag.</column>
	[use_src] [smallint] NOT NULL, --/ <column>Use source flag.</column>
	[prox] [real] NOT NULL, --/ <column unit="arcsec">Proximity. The distance between this source and its nearest neighbor in the PSC.</column>
	[pxpa] [smallint] NOT NULL, --/ <column unit="deg">The position angle on the sky of the vector from the source to the nearest neighbor in the PSC, East of North.</column>
	[pxcntr] [int] NOT NULL, --/ <column>ext_key value of nearest XSC source.</column>
	[dist_edge_ns] [int] NOT NULL, --/ <column>The distance from the source to the nearest North or South scan edge.</column>
	[dist_edge_ew] [smallint] NOT NULL, --/ <column unit="arcsec">The distance from the source to the nearest East or West scan edge.</column>
	[dist_edge_flg] [varchar](2) NOT NULL, --/ <column>flag indicating which edges ([n|s][e|w]) are nearest to src.</column>
	[pts_key] [bigint] NOT NULL, --/ <column>key to point source data DB record.</column>
	[mp_key] [int] NOT NULL, --/ <column>key to minor planet prediction DB record.</column>
	[night_key] [smallint] NOT NULL, --/ <column>key to night data record in "scan DB".</column>
	[scan_key] [int] NOT NULL, --/ <column>key to scan data record in "scan DB".</column>
	[coadd_key] [int] NOT NULL, --/ <column>key to coadd data record in "scan DB".</column>
	[hemis] [char](1) NOT NULL, --/ <column>hemisphere (N/S) of observation. "n" = North/Mt. Hopkins; "s" = South/CTIO.</column>
	[date] [varchar](32) NOT NULL, --/ <column>The observation reference date for this source expressed in ISO standard format.</column>
	[scan] [smallint] NOT NULL, --/ <column>scan number (unique within date).</column>
	[coadd] [smallint] NOT NULL, --/ <column>3-digit coadd number (unique within scan).</column>
	[id] [int] NOT NULL, --/ <column></column>
	[x_coadd] [real] NOT NULL, --/ <column unit="pix">x (cross-scan) position (coadd coord.).</column>
	[y_coadd] [real] NOT NULL, --/ <column unit="pix">y (in-scan) position (coadd coord.).</column>
	[j_subst2] [real] NOT NULL, --/ <column>J residual background #2 (score).</column>
	[h_subst2] [real] NOT NULL, --/ <column>H residual background #2 (score).</column>
	[k_subst2] [real] NOT NULL, --/ <column>K residual background #2 (score).</column>
	[j_back] [real] NOT NULL, --/ <column unit="mag/sq arcsec">J coadd median background.</column>
	[h_back] [real] NOT NULL, --/ <column></column>
	[k_back] [real] NOT NULL, --/ <column unit="mag/sq arcsec">K coadd median background.</column>
	[j_resid_ann] [real] NOT NULL, --/ <column unit="mag/sq arcsec">J residual annulus background median.</column>
	[h_resid_ann] [real] NOT NULL, --/ <column unit="mag/sq arcsec">H residual annulus background median.</column>
	[k_resid_ann] [real] NOT NULL, --/ <column unit="mag/sq arcsec">K residual annulus background median.</column>
	[j_bndg_per] [int] NOT NULL, --/ <column>J banding Fourier Transf. period on this side of coadd.</column>
	[j_bndg_amp] [real] NOT NULL, --/ <column>J banding maximum FT amplitude on this side of coadd.</column>
	[h_bndg_per] [int] NOT NULL, --/ <column></column>
	[h_bndg_amp] [real] NOT NULL, --/ <column></column>
	[k_bndg_per] [int] NOT NULL, --/ <column>K banding Fourier Transf. period on this side of coadd.</column>
	[k_bndg_amp] [real] NOT NULL, --/ <column>K banding maximum FT amplitude on this side of coadd.</column>
	[j_seetrack] [real] NOT NULL, --/ <column>J band seetracking score.</column>
	[h_seetrack] [real] NOT NULL, --/ <column>H band seetracking score.</column>
	[k_seetrack] [real] NOT NULL, --/ <column>K band seetracking score.</column>
	[ext_key] [int] NOT NULL --/ <column>entry counter (key) number (unique within table).</column>
) ON [PRIMARY]

CREATE TABLE [dbo].[ScanInfo](
--/ <summary></summary>
--/ <remarks></remarks>
	[scan_key] [int] NOT NULL, --/ <column>The unique identification number for this scan</column>
	[hemis] [char](1) NOT NULL, --/ <column>Observatory from which data were obtained (n/s)</column>
	[date] [varchar](32) NOT NULL, --/ <column>The observation reference date for this scan expressed in ISO standard format</column>
	[scan] [smallint] NOT NULL, --/ <column>Scan number (unique within date)</column>
	[tile] [int] NOT NULL, --/ <column>Tile identification number</column>
	[ra] [float] NOT NULL, --/ <column unit="deg">Right ascension of scan center for equinox J2000</column>
	[dec] [float] NOT NULL, --/ <column unit="deg">Declination of scan center for equinox J2000</column>
	[glon] [real] NOT NULL, --/ <column unit="deg">Galactic longitude of scan center, as computed from ra and dec</column>
	[glat] [real] NOT NULL, --/ <column unit="deg">Galactic latitude of scan center, as computed from ra and dec</column>
	[ra_1] [float] NOT NULL, --/ <column unit="deg">J2000 right ascension of the eastern corner at start of scan</column>
	[dec_1] [float] NOT NULL, --/ <column unit="deg">J2000 declination of the eastern corner at start of scan</column>
	[ra_2] [float] NOT NULL, --/ <column unit="deg">J2000 right ascension of the western corner at start of s</column>
	[dec_2] [float] NOT NULL, --/ <column unit="deg">J2000 declination of the western corner at start of scan</column>
	[ra_3] [float] NOT NULL, --/ <column unit="deg">J2000 right ascension of the eastern corner at end of scan</column>
	[dec_3] [float] NOT NULL, --/ <column unit="deg">J2000 declination of the eastern corner at end of scan</column>
	[ra_4] [float] NOT NULL, --/ <column unit="deg">J2000 right ascension of the western corner at end of scan</column>
	[dec_4] [float] NOT NULL, --/ <column unit="deg">J2000 declination of the western corner at end of scan</column>
	[sd] [char](1) NOT NULL, --/ <column>Scanning direction (north-going or south-going)</column>
	[qual] [smallint] NOT NULL, --/ <column>Quality score for scan (10 is highest grade,0 is lowest)</column>
	[hgl] [smallint] NOT NULL, --/ <column>flag indicating whether(1) or not(0) the scan has a single-frame H-band electronic glitch</column>
	[cld] [smallint] NOT NULL, --/ <column>flag indicating whether or not a cloud was found in the scan after comparison (1 indicates clouds)</column>
	[xph] [smallint] NOT NULL, --/ <column>flag indicating whether(1) or not(0) another photometric problem, not obviously cloud related, was found</column>
	[anom] [smallint] NOT NULL, --/ <column>flag indicating whether(1) or not(0) an unusual problem was found in the Atlas Images for this scan</column>
	[ut] [float] NOT NULL, --/ <column unit="hr">Universal Time (UT) at beginning of scan</column>
	[jdate] [float] NOT NULL, --/ <column unit="day">Julian Date at beginning of scan</column>
	[airm] [real] NOT NULL, --/ <column>Airmass at beginning of scan</column>
	[zd] [real] NOT NULL, --/ <column unit="deg">Scan's distance from the zenith at beginning of scan</column>
	[ha] [float] NOT NULL, --/ <column unit="hr">Hour angle at beginning of scan</column>
	[rh] [smallint] NOT NULL, --/ <column unit="%">Relative humidity of telescope enclosure at beginning of scan</column>
	[air_temp] [real] NOT NULL, --/ <column unit="deg C">Air temperature at beginning of scan</column>
	[tel_temp] [real] NOT NULL, --/ <column unit="deg C">Telescope girdle temperature at beginning of scan</column>
	[focus] [smallint] NOT NULL, --/ <column>Focus setting of telescope at beginning of scan</column>
	[hry] [smallint] NOT NULL, --/ <column>Flag indicating the H-band array configuration for the camera (1 indicates the new  configuration)</column>
	[c_strat] [smallint] NOT NULL, --/ <column>Flag indicating the calibration strategy for this night's data</column>
	[j_zp_ap] [real] NOT NULL, --/ <column unit="mag">Photometric zero-point for J-band aperture photometry</column>
	[h_zp_ap] [real] NOT NULL, --/ <column unit="mag">Photometric zero-point for H-band aperture photometry</column>
	[k_zp_ap] [real] NOT NULL, --/ <column unit="mag">Photometric zero-point for Ks-band aperture photometry</column>
	[h_zperr_ap] [real] NOT NULL, --/ <column unit="mag">RMS-error of zero-point for H-band aperture photometry</column>
	[k_zperr_ap] [real] NOT NULL, --/ <column unit="mag">RMS-error of zero-point for Ks-band aperture photometry</column>
	[j_n_snr10] [int] NOT NULL, --/ <column>Number of point sources at J-band with SNR&amp;gt;10 (instrumental mag &amp;lt;=15.8)</column>
	[h_n_snr10] [int] NOT NULL, --/ <column>Number of point sources at H-band with SNR&amp;gt;10 (instrumental mag &amp;lt;=15.1)</column>
	[k_n_snr10] [int] NOT NULL, --/ <column>Number of point sources at Ks-band with SNR&amp;gt;10 (instrumental mag &amp;lt;=14.3)</column>
	[n_ext] [int] NOT NULL, --/ <column>Number of regular extended sources detected in scan</column>
	[j_shape_avg] [real] NOT NULL, --/ <column>J-band average seeing shape for scan</column>
	[h_shape_avg] [real] NOT NULL, --/ <column>H-band average seeing shape for scan</column>
	[k_shape_avg] [real] NOT NULL, --/ <column>Ks-band average seeing shape for scan</column>
	[j_shape_rms] [real] NOT NULL, --/ <column>RMS-error of J-band average seeing shape</column>
	[h_shape_rms] [real] NOT NULL, --/ <column>RMS-error of H-band average seeing shape</column>
	[k_shape_rms] [real] NOT NULL, --/ <column>RMS-error of Ks-band average seeing shape</column>
	[j_2mrat] [real] NOT NULL, --/ <column>J-band average 2nd image moment ratio</column>
	[h_2mrat] [real] NOT NULL, --/ <column>H-band average 2nd image moment ratio</column>
	[k_2mrat] [real] NOT NULL, --/ <column>Ks-band average 2nd image moment ratio</column>
	[j_psp] [real] NOT NULL, --/ <column>J-band photometric sensitivity paramater (PSP).</column>
	[h_psp] [real] NOT NULL, --/ <column>H-band photometric sensitivity parameter (PSP).</column>
	[k_psp] [real] NOT NULL, --/ <column>Ks-band photometric sensitivity parameter (PSP).</column>
	[j_pts_noise] [real] NOT NULL, --/ <column>log10 of the mode of the noise distribution (noise estimated from J-band)</column>
	[h_pts_noise] [real] NOT NULL, --/ <column>log10 of the mode of the noise distribution (noise estimated from H-band)</column>
	[k_pts_noise] [real] NOT NULL, --/ <column>log10 of the mode of the noise distribution (noise estimated from Ks-band)</column>
	[j_msnr10] [real] NOT NULL, --/ <column>The J-band magnitude at which SNR=10 is achieved for this scan</column>
	[h_msnr10] [real] NOT NULL, --/ <column>The H-band magnitude at which SNR=10 is achieved for this scan</column>
	[k_msnr10] [real] NOT NULL, --/ <column>The Ks-band magnitude at which SNR=10 is achieved for this scan</column>
	[rel0] [smallint] NOT NULL, --/ <column>Flag indicating whether the scan is contained in the 2MASS Sampler Release</column>
	[rel1] [smallint] NOT NULL, --/ <column>Flag indicating whether the scan is contained in the 2MASS First Incremental Data Release (IDR1)</column>
	[rel2] [smallint] NOT NULL --/ <column>Flag indicating whether the scan is contained in the 2MASS Second Incremental Data Release (IDR2)</column>
) ON [PRIMARY]


