/*

CREATE PARTITION FUNCTION PF_PhotoObjAll ( bigint )
AS RANGE RIGHT
FOR VALUES ( 
1237651735200924423,
1237655175474905492,
1237657874324914248,
1237660670886544392,
1237662341618598877,
1237663542610101540,
1237666091124851262,
1237667172921508069,
1237668660060622206,
1237671991344956035,
1237676678221070961,
1237680263459766494
) 



CREATE PARTITION SCHEME PS_PhotoObjAll
AS PARTITION PF_PhotoObjAll
ALL TO (PHOTOOBJ)


*/

CREATE TABLE PhotoObjAll (
-------------------------------------------------------------------------------
--/H The full photometric catalog quantities for SDSS imaging.
-------------------------------------------------------------------------------
--/T This table contains one entry per detection, with the associated 
--/T photometric parameters measured by PHOTO, and astrometrically 
--/T and photometrically calibrated. 
--/T <p>
--/T The table has the following  views:
--/T <ul>
--/T <li> <b>PhotoObj</b>: all primary and secondary objects; essentially this is the view you should use unless you want a specific type of object.
--/T <li> <b>PhotoPrimary</b>: all photo objects that are primary (the best version of the object).
--/T <ul><li> <b>Star</b>: Primary objects that are classified as stars.
--/T     <li> <b>Galaxy</b>: Primary objects that are classified as galaxies.
--/T	   <li> <b>Sky</b>:Primary objects which are sky samples.
--/T     <li> <b>Unknown</b>:Primary objects which are no0ne of the above</ul>
--/T     <li> <b>PhotoSecondary</b>: all photo objects that are secondary (secondary detections)
--/T     <li> <b>PhotoFamily</b>: all photo objects which are neither primary nor secondary (blended)
--/T </ul>
--/T <p> The table has indices that cover the popular columns.
-------------------------------------------------------------------------------
	objID            bigint NOT NULL,     --/D Unique SDSS identifier composed from [skyVersion,rerun,run,camcol,field,obj].
	skyVersion       tinyint NOT NULL,    --/D Layer of catalog (currently only one layer, 0; 0-15 available)
	run              smallint NOT NULL,   --/D Run number
	rerun            smallint NOT NULL,   --/D Rerun number
	camcol           tinyint NOT NULL,    --/D Camera column
	field            smallint NOT NULL,   --/D Field number
	obj              smallint NOT NULL,   --/D The object id within a field. Usually changes between reruns of the same field --/F id
	mode             tinyint NOT NULL,    --/D 1: primary, 2: secondary, 3: other --/R PhotoMode 
	nChild           smallint NOT NULL,   --/D Number of children if this is a composite object that has been deblended. BRIGHT (in a flags sense) objects also have nchild == 1, the non-BRIGHT sibling.
	type             smallint NOT NULL,   --/D Type classification of the object (star, galaxy, cosmic ray, etc.)  --/R PhotoType --/F objc_type
	clean            [int] NOT NULL,        --/D Clean photometry flag (1=clean, 0=unclean).
	probPSF          real NOT NULL,       --/D Probability that the object is a star. Currently 0 if type == 3 (galaxy), 1 if type == 6 (star).  --/F objc_prob_psf
	insideMask       tinyint NOT NULL,    --/D Flag to indicate whether object is inside a mask and why --/R InsideMask --/F NOFITS
	flags            bigint NOT NULL,     --/D Photo Object Attribute Flags --/R PhotoFlags --/F objc_flags
	/*rowc             real NOT NULL,       --/U pix --/D Row center position (r-band coordinates) --/F objc_rowc
	rowcErr          real NOT NULL,       --/U pix --/D Row center position error (r-band coordinates) --/F objc_rowcerr
	colc             real NOT NULL,       --/U pix --/D Column center position (r-band coordinates) --/F objc_colc
	colcErr          real NOT NULL,       --/U pix --/D Column center position error (r-band coordinates) --/F objc_colcerr
	rowv             real NOT NULL,       --/U deg/day --/D  Row-component of object's velocity --/F rowvdeg
	rowvErr          real NOT NULL,       --/U deg/day --/D Row-component of object's velocity error --/F rowvdegerr
	colv             real NOT NULL,       --/U deg/day --/D Column-component of object's velocity	--/F colvdeg
	colvErr          real NOT NULL,       --/U deg/day --/D Column-component of object's velocity error --/F colvdegerr
	rowc_u           real NOT NULL,       --/U pix --/D Row center, u-band
	rowc_g           real NOT NULL,       --/U pix --/D Row center, g-band
	rowc_r           real NOT NULL,       --/U pix --/D Row center, r-band
	rowc_i           real NOT NULL,       --/U pix --/D Row center, i-band
	rowc_z           real NOT NULL,       --/U pix --/D Row center, z-band
	rowcErr_u        real NOT NULL,       --/U pix --/D ERROR Row center error, u-band
	rowcErr_g        real NOT NULL,       --/U pix --/D ERROR Row center error, g-band
	rowcErr_r        real NOT NULL,       --/U pix --/D ERROR Row center error, r-band
	rowcErr_i        real NOT NULL,       --/U pix --/D ERROR Row center error, i-band
	rowcErr_z        real NOT NULL,       --/U pix --/D ERROR Row center error, z-band
	colc_u           real NOT NULL,       --/U pix --/D Column center, u-band
	colc_g           real NOT NULL,       --/U pix --/D Column center, g-band
	colc_r           real NOT NULL,       --/U pix --/D Column center, r-band
	colc_i           real NOT NULL,       --/U pix --/D Column center, i-band
	colc_z           real NOT NULL,       --/U pix --/D Column center, z-band
	colcErr_u        real NOT NULL,       --/U pix --/D Column center error, u-band
	colcErr_g        real NOT NULL,       --/U pix --/D Column center error, g-band
	colcErr_r        real NOT NULL,       --/U pix --/D Column center error, r-band
	colcErr_i        real NOT NULL,       --/U pix --/D Column center error, i-band
	colcErr_z        real NOT NULL,       --/U pix --/D Column center error, z-band*/
	sky_u            real NOT NULL,       --/U nanomaggies/arcsec^2  --/D Sky flux at the center of object (allowing for siblings if blended). --/F skyflux
	sky_g            real NOT NULL,       --/U nanomaggies/arcsec^2  --/D Sky flux at the center of object (allowing for siblings if blended). --/F skyflux
	sky_r            real NOT NULL,       --/U nanomaggies/arcsec^2  --/D Sky flux at the center of object (allowing for siblings if blended). --/F skyflux
	sky_i            real NOT NULL,       --/U nanomaggies/arcsec^2  --/D Sky flux at the center of object (allowing for siblings if blended). --/F skyflux
	sky_z            real NOT NULL,       --/U nanomaggies/arcsec^2  --/D Sky flux at the center of object (allowing for siblings if blended). --/F skyflux
	skyIvar_u         real NOT NULL,       --/U nanomaggies/arcsec^2  --/D Sky flux inverse variance --/F skyflux_ivar
	skyIvar_g         real NOT NULL,       --/U nanomaggies/arcsec^2  --/D Sky flux inverse variance --/F skyflux_ivar
	skyIvar_r         real NOT NULL,       --/U nanomaggies/arcsec^2  --/D Sky flux inverse variance --/F skyflux_ivar
	skyIvar_i         real NOT NULL,       --/U nanomaggies/arcsec^2  --/D Sky flux inverse variance --/F skyflux_ivar
	skyIvar_z         real NOT NULL,       --/U nanomaggies/arcsec^2  --/D Sky flux inverse variance --/F skyflux_ivar
	psfMag_u         real NOT NULL,       --/U mag --/D PSF magnitude
	psfMag_g         real NOT NULL,       --/U mag --/D PSF magnitude
	psfMag_r         real NOT NULL,       --/U mag --/D PSF magnitude
	psfMag_i         real NOT NULL,       --/U mag --/D PSF magnitude
	psfMag_z         real NOT NULL,       --/U mag --/D PSF magnitude
	psfMagErr_u      real NOT NULL,       --/U mag --/D PSF magnitude error
	psfMagErr_g      real NOT NULL,       --/U mag --/D PSF magnitude error
	psfMagErr_r      real NOT NULL,       --/U mag --/D PSF magnitude error
	psfMagErr_i      real NOT NULL,       --/U mag --/D PSF magnitude error
	psfMagErr_z      real NOT NULL,       --/U mag --/D PSF magnitude error
	fiberMag_u       real NOT NULL,       --/U mag --/D Magnitude in 3 arcsec diameter fiber radius
	fiberMag_g       real NOT NULL,       --/U mag --/D Magnitude in 3 arcsec diameter fiber radius
	fiberMag_r       real NOT NULL,       --/U mag --/D Magnitude in 3 arcsec diameter fiber radius
	fiberMag_i       real NOT NULL,       --/U mag --/D Magnitude in 3 arcsec diameter fiber radius
	fiberMag_z       real NOT NULL,       --/U mag --/D Magnitude in 3 arcsec diameter fiber radius
	fiberMagErr_u    real NOT NULL,     --/U mag --/D Error in magnitude in 3 arcsec diameter fiber radius 
	fiberMagErr_g    real NOT NULL,     --/U mag --/D Error in magnitude in 3 arcsec diameter fiber radius 
	fiberMagErr_r    real NOT NULL,     --/U mag --/D Error in magnitude in 3 arcsec diameter fiber radius 
	fiberMagErr_i    real NOT NULL,     --/U mag --/D Error in magnitude in 3 arcsec diameter fiber radius 
	fiberMagErr_z    real NOT NULL,     --/U mag --/D Error in magnitude in 3 arcsec diameter fiber radius 
	fiber2Mag_u      real NOT NULL,       --/U mag --/D Magnitude in 2 arcsec diameter fiber radius
	fiber2Mag_g      real NOT NULL,       --/U mag --/D Magnitude in 2 arcsec diameter fiber radius
	fiber2Mag_r      real NOT NULL,       --/U mag --/D Magnitude in 2 arcsec diameter fiber radius
	fiber2Mag_i      real NOT NULL,       --/U mag --/D Magnitude in 2 arcsec diameter fiber radius
	fiber2Mag_z      real NOT NULL,       --/U mag --/D Magnitude in 2 arcsec diameter fiber radius
	fiber2MagErr_u   real NOT NULL,     --/U mag --/D Error in magnitude in 2 arcsec diameter fiber radius 
	fiber2MagErr_g   real NOT NULL,     --/U mag --/D Error in magnitude in 2 arcsec diameter fiber radius 
	fiber2MagErr_r   real NOT NULL,     --/U mag --/D Error in magnitude in 2 arcsec diameter fiber radius 
	fiber2MagErr_i   real NOT NULL,     --/U mag --/D Error in magnitude in 2 arcsec diameter fiber radius 
	fiber2MagErr_z   real NOT NULL,     --/U mag --/D Error in magnitude in 2 arcsec diameter fiber radius 
	petroMag_u       real NOT NULL,       --/U mag --/D Petrosian magnitude
	petroMag_g       real NOT NULL,       --/U mag --/D Petrosian magnitude
	petroMag_r       real NOT NULL,       --/U mag --/D Petrosian magnitude
	petroMag_i       real NOT NULL,       --/U mag --/D Petrosian magnitude
	petroMag_z       real NOT NULL,       --/U mag --/D Petrosian magnitude
	petroMagErr_u    real NOT NULL,      --/U mag --/D Petrosian magnitude error
	petroMagErr_g    real NOT NULL,      --/U mag --/D Petrosian magnitude error
	petroMagErr_r    real NOT NULL,      --/U mag --/D Petrosian magnitude error
	petroMagErr_i    real NOT NULL,      --/U mag --/D Petrosian magnitude error
	petroMagErr_z    real NOT NULL,      --/U mag --/D Petrosian magnitude error
	psfFlux_u        real NOT NULL,      --/U nanomaggies --/D PSF flux 
	psfFlux_g        real NOT NULL,      --/U nanomaggies --/D PSF flux
	psfFlux_r        real NOT NULL,      --/U nanomaggies --/D PSF flux
	psfFlux_i        real NOT NULL,      --/U nanomaggies --/D PSF flux
	psfFlux_z        real NOT NULL,      --/U nanomaggies --/D PSF flux
	psfFluxIvar_u    real NOT NULL,      --/U nanomaggies^{-2}  --/D PSF flux inverse variance
	psfFluxIvar_g    real NOT NULL,      --/U nanomaggies^{-2}  --/D PSF flux inverse variance
	psfFluxIvar_r    real NOT NULL,      --/U nanomaggies^{-2}  --/D PSF flux inverse variance
	psfFluxIvar_i    real NOT NULL,      --/U nanomaggies^{-2}  --/D PSF flux inverse variance
	psfFluxIvar_z    real NOT NULL,      --/U nanomaggies^{-2}  --/D PSF flux inverse variance
	fiberFlux_u      real NOT NULL,   --/U nanomaggies --/D Flux in 3 arcsec diameter fiber radius
	fiberFlux_g      real NOT NULL,   --/U nanomaggies --/D Flux in 3 arcsec diameter fiber radius
	fiberFlux_r      real NOT NULL,   --/U nanomaggies --/D Flux in 3 arcsec diameter fiber radius
	fiberFlux_i      real NOT NULL,   --/U nanomaggies --/D Flux in 3 arcsec diameter fiber radius
	fiberFlux_z      real NOT NULL,   --/U nanomaggies --/D Flux in 3 arcsec diameter fiber radius
	fiberFluxIvar_u  real NOT NULL,   --/U nanomaggies^{-2}  --/D Inverse variance in flux in 3 arcsec diameter fiber radius
	fiberFluxIvar_g  real NOT NULL,   --/U nanomaggies^{-2}  --/D Inverse variance in flux in 3 arcsec diameter fiber radius
	fiberFluxIvar_r  real NOT NULL,   --/U nanomaggies^{-2}  --/D Inverse variance in flux in 3 arcsec diameter fiber radius
	fiberFluxIvar_i  real NOT NULL,   --/U nanomaggies^{-2}  --/D Inverse variance in flux in 3 arcsec diameter fiber radius
	fiberFluxIvar_z  real NOT NULL,   --/U nanomaggies^{-2}  --/D Inverse variance in flux in 3 arcsec diameter fiber radius
	fiber2Flux_u     real NOT NULL,   --/U nanomaggies --/D Flux in 2 arcsec diameter fiber radius
	fiber2Flux_g     real NOT NULL,   --/U nanomaggies --/D Flux in 2 arcsec diameter fiber radius
	fiber2Flux_r     real NOT NULL,   --/U nanomaggies --/D Flux in 2 arcsec diameter fiber radius
	fiber2Flux_i     real NOT NULL,   --/U nanomaggies --/D Flux in 2 arcsec diameter fiber radius
	fiber2Flux_z     real NOT NULL,   --/U nanomaggies --/D Flux in 2 arcsec diameter fiber radius
	fiber2FluxIvar_u real NOT NULL,   --/U nanomaggies^{-2}  --/D Inverse variance in flux in 2 arcsec diameter fiber radius 
	fiber2FluxIvar_g real NOT NULL,   --/U nanomaggies^{-2}  --/D Inverse variance in flux in 2 arcsec diameter fiber radius 
	fiber2FluxIvar_r real NOT NULL,   --/U nanomaggies^{-2}  --/D Inverse variance in flux in 2 arcsec diameter fiber radius
	fiber2FluxIvar_i real NOT NULL,   --/U nanomaggies^{-2}  --/D Inverse variance in flux in 2 arcsec diameter fiber radius
	fiber2FluxIvar_z real NOT NULL,   --/U nanomaggies^{-2}  --/D Inverse variance in flux in 2 arcsec diameter fiber radius
	petroFlux_u      real NOT NULL,   --/U nanomaggies --/D Petrosian flux
	petroFlux_g      real NOT NULL,   --/U nanomaggies --/D Petrosian flux
	petroFlux_r      real NOT NULL,   --/U nanomaggies --/D Petrosian flux
	petroFlux_i      real NOT NULL,   --/U nanomaggies --/D Petrosian flux
	petroFlux_z      real NOT NULL,   --/U nanomaggies --/D Petrosian flux
	petroFluxIvar_u  real NOT NULL,   --/U nanomaggies^{-2}  --/D Petrosian flux inverse variance
	petroFluxIvar_g  real NOT NULL,   --/U nanomaggies^{-2}  --/D Petrosian flux inverse variance
	petroFluxIvar_r  real NOT NULL,   --/U nanomaggies^{-2}  --/D Petrosian flux inverse variance
	petroFluxIvar_i  real NOT NULL,   --/U nanomaggies^{-2}  --/D Petrosian flux inverse variance
	petroFluxIvar_z  real NOT NULL,   --/U nanomaggies^{-2}  --/D Petrosian flux inverse variance
	petroRad_u       real NOT NULL,   --/U arcsec  --/D Petrosian radius --/F petrotheta
	petroRad_g       real NOT NULL,   --/U arcsec  --/D Petrosian radius --/F petrotheta
	petroRad_r       real NOT NULL,   --/U arcsec  --/D Petrosian radius --/F petrotheta
	petroRad_i       real NOT NULL,   --/U arcsec  --/D Petrosian radius --/F petrotheta
	petroRad_z       real NOT NULL,   --/U arcsec  --/D Petrosian radius --/F petrotheta
	petroRadErr_u    real NOT NULL,   --/U arcsec  --/D Petrosian radius error --/F petrothetaerr
	petroRadErr_g    real NOT NULL,   --/U arcsec  --/D Petrosian radius error --/F petrothetaerr
	petroRadErr_r    real NOT NULL,   --/U arcsec  --/D Petrosian radius error --/F petrothetaerr
	petroRadErr_i    real NOT NULL,   --/U arcsec  --/D Petrosian radius error --/F petrothetaerr
	petroRadErr_z    real NOT NULL,   --/U arcsec  --/D Petrosian radius error --/F petrothetaerr
	petroR50_u       real NOT NULL,   --/U arcsec  --/D Radius containing 50% of Petrosian flux --/F petroth50
	petroR50_g       real NOT NULL,   --/U arcsec  --/D Radius containing 50% of Petrosian flux --/F petroth50
	petroR50_r       real NOT NULL,   --/U arcsec  --/D Radius containing 50% of Petrosian flux --/F petroth50
	petroR50_i       real NOT NULL,   --/U arcsec  --/D Radius containing 50% of Petrosian flux --/F petroth50
	petroR50_z       real NOT NULL,   --/U arcsec  --/D Radius containing 50% of Petrosian flux --/F petroth50
	petroR50Err_u    real NOT NULL,   --/U arcsec  --/D Error in radius with 50% of Petrosian flux error --/F petroth50err
	petroR50Err_g    real NOT NULL,   --/U arcsec  --/D Error in radius with 50% of Petrosian flux error --/F petroth50err
	petroR50Err_r    real NOT NULL,   --/U arcsec  --/D Error in radius with 50% of Petrosian flux error --/F petroth50err
	petroR50Err_i    real NOT NULL,   --/U arcsec  --/D Error in radius with 50% of Petrosian flux error --/F petroth50err
	petroR50Err_z    real NOT NULL,   --/U arcsec  --/D Error in radius with 50% of Petrosian flux error --/F petroth50err
	petroR90_u       real NOT NULL,   --/U arcsec  --/D Radius containing 90% of Petrosian flux --/F petroth90
	petroR90_g       real NOT NULL,   --/U arcsec  --/D Radius containing 90% of Petrosian flux --/F petroth90
	petroR90_r       real NOT NULL,   --/U arcsec  --/D Radius containing 90% of Petrosian flux --/F petroth90
	petroR90_i       real NOT NULL,   --/U arcsec  --/D Radius containing 90% of Petrosian flux --/F petroth90
	petroR90_z       real NOT NULL,   --/U arcsec  --/D Radius containing 90% of Petrosian flux --/F petroth90
	petroR90Err_u    real NOT NULL,   --/U arcsec  --/D Error in radius with 90% of Petrosian flux error --/F petroth90err
	petroR90Err_g    real NOT NULL,   --/U arcsec  --/D Error in radius with 90% of Petrosian flux error --/F petroth90err
	petroR90Err_r    real NOT NULL,   --/U arcsec  --/D Error in radius with 90% of Petrosian flux error --/F petroth90err
	petroR90Err_i    real NOT NULL,   --/U arcsec  --/D Error in radius with 90% of Petrosian flux error --/F petroth90err
	petroR90Err_z    real NOT NULL,   --/U arcsec  --/D Error in radius with 90% of Petrosian flux error --/F petroth90err
	/*q_u              real NOT NULL,   --/D Stokes Q parameter
	q_g              real NOT NULL,   --/D Stokes Q parameter
	q_r              real NOT NULL,   --/D Stokes Q parameter
	q_i              real NOT NULL,   --/D Stokes Q parameter
	q_z              real NOT NULL,   --/D Stokes Q parameter
	qErr_u           real NOT NULL,   --/D Stokes Q parameter error
	qErr_g           real NOT NULL,   --/D Stokes Q parameter error
	qErr_r           real NOT NULL,   --/D Stokes Q parameter error
	qErr_i           real NOT NULL,   --/D Stokes Q parameter error
	qErr_z           real NOT NULL,   --/D Stokes Q parameter error
	u_u              real NOT NULL,   --/D Stokes U parameter
	u_g              real NOT NULL,   --/D Stokes U parameter
	u_r              real NOT NULL,   --/D Stokes U parameter
	u_i              real NOT NULL,   --/D Stokes U parameter
	u_z              real NOT NULL,   --/D Stokes U parameter
	uErr_u           real NOT NULL,   --/D Stokes U parameter error
	uErr_g           real NOT NULL,   --/D Stokes U parameter error
	uErr_r           real NOT NULL,   --/D Stokes U parameter error
	uErr_i           real NOT NULL,   --/D Stokes U parameter error
	uErr_z           real NOT NULL,   --/D Stokes U parameter error
	mE1_u            real NOT NULL,   --/D Adaptive E1 shape measure (pixel coordinates)
	mE1_g            real NOT NULL,   --/D Adaptive E1 shape measure (pixel coordinates)
	mE1_r            real NOT NULL,   --/D Adaptive E1 shape measure (pixel coordinates)
	mE1_i            real NOT NULL,   --/D Adaptive E1 shape measure (pixel coordinates)
	mE1_z            real NOT NULL,   --/D Adaptive E1 shape measure (pixel coordinates)
	mE2_u            real NOT NULL,   --/D Adaptive E2 shape measure (pixel coordinates)
	mE2_g            real NOT NULL,   --/D Adaptive E2 shape measure (pixel coordinates)
	mE2_r            real NOT NULL,   --/D Adaptive E2 shape measure (pixel coordinates)
	mE2_i            real NOT NULL,   --/D Adaptive E2 shape measure (pixel coordinates)
	mE2_z            real NOT NULL,   --/D Adaptive E2 shape measure (pixel coordinates)
	mE1E1Err_u       real NOT NULL,   --/D Covariance in E1/E1 shape measure (pixel coordinates)
	mE1E1Err_g       real NOT NULL,   --/D Covariance in E1/E1 shape measure (pixel coordinates)
	mE1E1Err_r       real NOT NULL,   --/D Covariance in E1/E1 shape measure (pixel coordinates)
	mE1E1Err_i       real NOT NULL,   --/D Covariance in E1/E1 shape measure (pixel coordinates)
	mE1E1Err_z       real NOT NULL,   --/D Covariance in E1/E1 shape measure (pixel coordinates)
	mE1E2Err_u       real NOT NULL,   --/D Covariance in E1/E2 shape measure (pixel coordinates)
	mE1E2Err_g       real NOT NULL,   --/D Covariance in E1/E2 shape measure (pixel coordinates)
	mE1E2Err_r       real NOT NULL,   --/D Covariance in E1/E2 shape measure (pixel coordinates)
	mE1E2Err_i       real NOT NULL,   --/D Covariance in E1/E2 shape measure (pixel coordinates)
	mE1E2Err_z       real NOT NULL,   --/D Covariance in E1/E2 shape measure (pixel coordinates)
	mE2E2Err_u       real NOT NULL,   --/D Covariance in E2/E2 shape measure (pixel coordinates)
	mE2E2Err_g       real NOT NULL,   --/D Covariance in E2/E2 shape measure (pixel coordinates)
	mE2E2Err_r       real NOT NULL,   --/D Covariance in E2/E2 shape measure (pixel coordinates)
	mE2E2Err_i       real NOT NULL,   --/D Covariance in E2/E2 shape measure (pixel coordinates)
	mE2E2Err_z       real NOT NULL,   --/D Covariance in E2/E2 shape measure (pixel coordinates)
	mRrCc_u          real NOT NULL,   --/D Adaptive ( + ) (pixel coordinates)
	mRrCc_g          real NOT NULL,   --/D Adaptive ( + ) (pixel coordinates)
	mRrCc_r          real NOT NULL,   --/D Adaptive ( + ) (pixel coordinates)
	mRrCc_i          real NOT NULL,   --/D Adaptive ( + ) (pixel coordinates)
	mRrCc_z          real NOT NULL,   --/D Adaptive ( + ) (pixel coordinates)
	mRrCcErr_u       real NOT NULL,   --/D Error in adaptive ( + ) (pixel coordinates)
	mRrCcErr_g       real NOT NULL,   --/D Error in adaptive ( + ) (pixel coordinates)
	mRrCcErr_r       real NOT NULL,   --/D Error in adaptive ( + ) (pixel coordinates)
	mRrCcErr_i       real NOT NULL,   --/D Error in adaptive ( + ) (pixel coordinates)
	mRrCcErr_z       real NOT NULL,   --/D Error in adaptive ( + ) (pixel coordinates)
	mCr4_u           real NOT NULL,   --/D Adaptive fourth moment of object (pixel coordinates)
	mCr4_g           real NOT NULL,   --/D Adaptive fourth moment of object (pixel coordinates)
	mCr4_r           real NOT NULL,   --/D Adaptive fourth moment of object (pixel coordinates)
	mCr4_i           real NOT NULL,   --/D Adaptive fourth moment of object (pixel coordinates)
	mCr4_z           real NOT NULL,   --/D Adaptive fourth moment of object (pixel coordinates)
	mE1PSF_u         real NOT NULL,   --/D Adaptive E1 for PSF (pixel coordinates)
	mE1PSF_g         real NOT NULL,   --/D Adaptive E1 for PSF (pixel coordinates)
	mE1PSF_r         real NOT NULL,   --/D Adaptive E1 for PSF (pixel coordinates)
	mE1PSF_i         real NOT NULL,   --/D Adaptive E1 for PSF (pixel coordinates)
	mE1PSF_z         real NOT NULL,   --/D Adaptive E1 for PSF (pixel coordinates)
	mE2PSF_u         real NOT NULL,   --/D Adaptive E2 for PSF (pixel coordinates)
	mE2PSF_g         real NOT NULL,   --/D Adaptive E2 for PSF (pixel coordinates)
	mE2PSF_r         real NOT NULL,   --/D Adaptive E2 for PSF (pixel coordinates)
	mE2PSF_i         real NOT NULL,   --/D Adaptive E2 for PSF (pixel coordinates)
	mE2PSF_z         real NOT NULL,   --/D Adaptive E2 for PSF (pixel coordinates)
	mRrCcPSF_u       real NOT NULL,   --/D Adaptive ( + ) for PSF (pixel coordinates)
	mRrCcPSF_g       real NOT NULL,   --/D Adaptive ( + ) for PSF (pixel coordinates)
	mRrCcPSF_r       real NOT NULL,   --/D Adaptive ( + ) for PSF (pixel coordinates)
	mRrCcPSF_i       real NOT NULL,   --/D Adaptive ( + ) for PSF (pixel coordinates)
	mRrCcPSF_z       real NOT NULL,   --/D Adaptive ( + ) for PSF (pixel coordinates)
	mCr4PSF_u        real NOT NULL,   --/D Adaptive fourth moment for PSF (pixel coordinates)
	mCr4PSF_g        real NOT NULL,   --/D Adaptive fourth moment for PSF (pixel coordinates)
	mCr4PSF_r        real NOT NULL,   --/D Adaptive fourth moment for PSF (pixel coordinates)
	mCr4PSF_i        real NOT NULL,   --/D Adaptive fourth moment for PSF (pixel coordinates)
	mCr4PSF_z        real NOT NULL,   --/D Adaptive fourth moment for PSF (pixel coordinates)
	deVRad_u         real NOT NULL,   --/U arcsec  --/D de Vaucouleurs fit scale radius, here defined to be the same as the half-light radius, also called the effective radius.  --/F theta_dev
	deVRad_g         real NOT NULL,   --/U arcsec  --/D de Vaucouleurs fit scale radius, here defined to be the same as the half-light radius, also called the effective radius.  --/F theta_dev
	deVRad_r         real NOT NULL,   --/U arcsec  --/D de Vaucouleurs fit scale radius, here defined to be the same as the half-light radius, also called the effective radius.  --/F theta_dev
	deVRad_i         real NOT NULL,   --/U arcsec  --/D de Vaucouleurs fit scale radius, here defined to be the same as the half-light radius, also called the effective radius.  --/F theta_dev
	deVRad_z         real NOT NULL,   --/U arcsec  --/D de Vaucouleurs fit scale radius, here defined to be the same as the half-light radius, also called the effective radius.  --/F theta_dev
	deVRadErr_u      real NOT NULL,   --/U arcsec  --/D Error in de Vaucouleurs fit scale radius error  --/F theta_deverr
	deVRadErr_g      real NOT NULL,   --/U arcsec  --/D Error in de Vaucouleurs fit scale radius error  --/F theta_deverr
	deVRadErr_r      real NOT NULL,   --/U arcsec  --/D Error in de Vaucouleurs fit scale radius error  --/F theta_deverr
	deVRadErr_i      real NOT NULL,   --/U arcsec  --/D Error in de Vaucouleurs fit scale radius error  --/F theta_deverr
	deVRadErr_z      real NOT NULL,   --/U arcsec  --/D Error in de Vaucouleurs fit scale radius error  --/F theta_deverr
	deVAB_u          real NOT NULL,   --/D de Vaucouleurs fit b/a --/F ab_dev
	deVAB_g          real NOT NULL,   --/D de Vaucouleurs fit b/a --/F ab_dev
	deVAB_r          real NOT NULL,   --/D de Vaucouleurs fit b/a --/F ab_dev
	deVAB_i          real NOT NULL,   --/D de Vaucouleurs fit b/a --/F ab_dev
	deVAB_z          real NOT NULL,   --/D de Vaucouleurs fit b/a --/F ab_dev
	deVABErr_u       real NOT NULL,   --/D de Vaucouleurs fit b/a error --/F ab_deverr
	deVABErr_g       real NOT NULL,   --/D de Vaucouleurs fit b/a error --/F ab_deverr
	deVABErr_r       real NOT NULL,   --/D de Vaucouleurs fit b/a error --/F ab_deverr
	deVABErr_i       real NOT NULL,   --/D de Vaucouleurs fit b/a error --/F ab_deverr
	deVABErr_z       real NOT NULL,   --/D de Vaucouleurs fit b/a error --/F ab_deverr
	deVPhi_u         real NOT NULL,   --/U deg --/D de Vaucouleurs fit position angle (+N thru E)  --/F phi_dev_deg
	deVPhi_g         real NOT NULL,   --/U deg --/D de Vaucouleurs fit position angle (+N thru E)  --/F phi_dev_deg
	deVPhi_r         real NOT NULL,   --/U deg --/D de Vaucouleurs fit position angle (+N thru E)  --/F phi_dev_deg
	deVPhi_i         real NOT NULL,   --/U deg --/D de Vaucouleurs fit position angle (+N thru E)  --/F phi_dev_deg
	deVPhi_z         real NOT NULL,   --/U deg --/D de Vaucouleurs fit position angle (+N thru E)  --/F phi_dev_deg
	deVMag_u         real NOT NULL,   --/U mag --/D de Vaucouleurs magnitude fit
	deVMag_g         real NOT NULL,   --/U mag --/D de Vaucouleurs magnitude fit
	deVMag_r         real NOT NULL,   --/U mag --/D de Vaucouleurs magnitude fit
	deVMag_i         real NOT NULL,   --/U mag --/D de Vaucouleurs magnitude fit
	deVMag_z         real NOT NULL,   --/U mag --/D de Vaucouleurs magnitude fit
	deVMagErr_u      real NOT NULL,   --/U mag --/D de Vaucouleurs magnitude fit error
	deVMagErr_g      real NOT NULL,   --/U mag --/D de Vaucouleurs magnitude fit error
	deVMagErr_r      real NOT NULL,   --/U mag --/D de Vaucouleurs magnitude fit error
	deVMagErr_i      real NOT NULL,   --/U mag --/D de Vaucouleurs magnitude fit error
	deVMagErr_z      real NOT NULL,   --/U mag --/D de Vaucouleurs magnitude fit error
	deVFlux_u        real NOT NULL,   --/U nanomaggies --/D de Vaucouleurs magnitude fit
	deVFlux_g        real NOT NULL,   --/U nanomaggies --/D de Vaucouleurs magnitude fit
	deVFlux_r        real NOT NULL,   --/U nanomaggies --/D de Vaucouleurs magnitude fit
	deVFlux_i        real NOT NULL,   --/U nanomaggies --/D de Vaucouleurs magnitude fit
	deVFlux_z        real NOT NULL,   --/U nanomaggies --/D de Vaucouleurs magnitude fit
	deVFluxIvar_u    real NOT NULL,   --/U nanomaggies^{-2} --/D de Vaucouleurs magnitude fit inverse variance
	deVFluxIvar_g    real NOT NULL,   --/U nanomaggies^{-2} --/D de Vaucouleurs magnitude fit inverse variance
	deVFluxIvar_r    real NOT NULL,   --/U nanomaggies^{-2} --/D de Vaucouleurs magnitude fit inverse variance
	deVFluxIvar_i    real NOT NULL,   --/U nanomaggies^{-2} --/D de Vaucouleurs magnitude fit inverse variance
	deVFluxIvar_z    real NOT NULL,   --/U nanomaggies^{-2} --/D de Vaucouleurs magnitude fit inverse variance
	expRad_u         real NOT NULL,   --/U arcsec --/D Exponential fit scale radius, here defined to be the same as the half-light radius, also called the effective radius. --/F theta_exp
	expRad_g         real NOT NULL,   --/U arcsec --/D Exponential fit scale radius, here defined to be the same as the half-light radius, also called the effective radius. --/F theta_exp
	expRad_r         real NOT NULL,   --/U arcsec --/D Exponential fit scale radius, here defined to be the same as the half-light radius, also called the effective radius. --/F theta_exp
	expRad_i         real NOT NULL,   --/U arcsec --/D Exponential fit scale radius, here defined to be the same as the half-light radius, also called the effective radius. --/F theta_exp
	expRad_z         real NOT NULL,   --/U arcsec --/D Exponential fit scale radius, here defined to be the same as the half-light radius, also called the effective radius. --/F theta_exp
	expRadErr_u      real NOT NULL,   --/U arcsec --/D Exponential fit scale radius error --/F theta_experr
	expRadErr_g      real NOT NULL,   --/U arcsec --/D Exponential fit scale radius error --/F theta_experr
	expRadErr_r      real NOT NULL,   --/U arcsec --/D Exponential fit scale radius error --/F theta_experr
	expRadErr_i      real NOT NULL,   --/U arcsec --/D Exponential fit scale radius error --/F theta_experr
	expRadErr_z      real NOT NULL,   --/U arcsec --/D Exponential fit scale radius error --/F theta_experr
	expAB_u          real NOT NULL,	--/D Exponential fit b/a --/F ab_exp
	expAB_g          real NOT NULL,	--/D Exponential fit b/a --/F ab_exp
	expAB_r          real NOT NULL,	--/D Exponential fit b/a --/F ab_exp
	expAB_i          real NOT NULL,	--/D Exponential fit b/a --/F ab_exp
	expAB_z          real NOT NULL,	--/D Exponential fit b/a --/F ab_exp
	expABErr_u       real NOT NULL,	--/D Exponential fit b/a --/F ab_experr
	expABErr_g       real NOT NULL,	--/D Exponential fit b/a --/F ab_experr
	expABErr_r       real NOT NULL,	--/D Exponential fit b/a --/F ab_experr
	expABErr_i       real NOT NULL,	--/D Exponential fit b/a --/F ab_experr
	expABErr_z       real NOT NULL,	--/D Exponential fit b/a --/F ab_experr
	expPhi_u         real NOT NULL, --/U deg --/D Exponential fit position angle (+N thru E)  --/F phi_exp_deg
	expPhi_g         real NOT NULL, --/U deg --/D Exponential fit position angle (+N thru E)  --/F phi_exp_deg
	expPhi_r         real NOT NULL, --/U deg --/D Exponential fit position angle (+N thru E)  --/F phi_exp_deg
	expPhi_i         real NOT NULL, --/U deg --/D Exponential fit position angle (+N thru E)  --/F phi_exp_deg
	expPhi_z         real NOT NULL, --/U deg --/D Exponential fit position angle (+N thru E)  --/F phi_exp_deg
	expMag_u         real NOT NULL, --/U mag --/D Exponential fit magnitude
	expMag_g         real NOT NULL, --/U mag --/D Exponential fit magnitude
	expMag_r         real NOT NULL, --/U mag --/D Exponential fit magnitude
	expMag_i         real NOT NULL, --/U mag --/D Exponential fit magnitude
	expMag_z         real NOT NULL, --/U mag --/D Exponential fit magnitude
	expMagErr_u      real NOT NULL, --/U mag --/D Exponential fit magnitude error
	expMagErr_g      real NOT NULL, --/U mag --/D Exponential fit magnitude error
	expMagErr_r      real NOT NULL, --/U mag --/D Exponential fit magnitude error
	expMagErr_i      real NOT NULL, --/U mag --/D Exponential fit magnitude error
	expMagErr_z      real NOT NULL, --/U mag --/D Exponential fit magnitude error
	modelMag_u       real NOT NULL, --/U mag --/D better of DeV/Exp magnitude fit
	modelMag_g       real NOT NULL, --/U mag --/D better of DeV/Exp magnitude fit
	modelMag_r       real NOT NULL, --/U mag --/D better of DeV/Exp magnitude fit
	modelMag_i       real NOT NULL, --/U mag --/D better of DeV/Exp magnitude fit
	modelMag_z       real NOT NULL, --/U mag --/D better of DeV/Exp magnitude fit
	modelMagErr_u    real NOT NULL, --/U mag --/D Error in better of DeV/Exp magnitude fit 
	modelMagErr_g    real NOT NULL, --/U mag --/D Error in better of DeV/Exp magnitude fit 
	modelMagErr_r    real NOT NULL, --/U mag --/D Error in better of DeV/Exp magnitude fit 
	modelMagErr_i    real NOT NULL, --/U mag --/D Error in better of DeV/Exp magnitude fit 
	modelMagErr_z    real NOT NULL, --/U mag --/D Error in better of DeV/Exp magnitude fit 
	cModelMag_u      real NOT NULL, --/U mag --/D DeV+Exp magnitude 
	cModelMag_g      real NOT NULL, --/U mag --/D DeV+Exp magnitude 
	cModelMag_r      real NOT NULL, --/U mag --/D DeV+Exp magnitude 
	cModelMag_i      real NOT NULL, --/U mag --/D DeV+Exp magnitude 
	cModelMag_z      real NOT NULL, --/U mag --/D DeV+Exp magnitude 
	cModelMagErr_u   real NOT NULL, --/U mag --/D DeV+Exp magnitude error
	cModelMagErr_g   real NOT NULL, --/U mag --/D DeV+Exp magnitude error
	cModelMagErr_r   real NOT NULL, --/U mag --/D DeV+Exp magnitude error
	cModelMagErr_i   real NOT NULL, --/U mag --/D DeV+Exp magnitude error
	cModelMagErr_z   real NOT NULL, --/U mag --/D DeV+Exp magnitude error
	expFlux_u        real NOT NULL, --/U nanomaggies --/D Exponential fit flux
	expFlux_g        real NOT NULL, --/U nanomaggies --/D Exponential fit flux
	expFlux_r        real NOT NULL, --/U nanomaggies --/D Exponential fit flux
	expFlux_i        real NOT NULL, --/U nanomaggies --/D Exponential fit flux
	expFlux_z        real NOT NULL, --/U nanomaggies --/D Exponential fit flux
	expFluxIvar_u    real NOT NULL, --/U nanomaggies^{-2} --/D Exponential fit flux inverse variance
	expFluxIvar_g    real NOT NULL, --/U nanomaggies^{-2} --/D Exponential fit flux inverse variance
	expFluxIvar_r    real NOT NULL, --/U nanomaggies^{-2} --/D Exponential fit flux inverse variance
	expFluxIvar_i    real NOT NULL, --/U nanomaggies^{-2} --/D Exponential fit flux inverse variance
	expFluxIvar_z    real NOT NULL, --/U nanomaggies^{-2} --/D Exponential fit flux inverse variance
	modelFlux_u      real NOT NULL, --/U nanomaggies --/D better of DeV/Exp flux fit
	modelFlux_g      real NOT NULL, --/U nanomaggies --/D better of DeV/Exp flux fit
	modelFlux_r      real NOT NULL, --/U nanomaggies --/D better of DeV/Exp flux fit
	modelFlux_i      real NOT NULL, --/U nanomaggies --/D better of DeV/Exp flux fit
	modelFlux_z      real NOT NULL, --/U nanomaggies --/D better of DeV/Exp flux fit
	modelFluxIvar_u  real NOT NULL, --/U nanomaggies^{-2}  --/D Inverse variance in better of DeV/Exp flux fit 
	modelFluxIvar_g  real NOT NULL, --/U nanomaggies^{-2}  --/D Inverse variance in better of DeV/Exp flux fit 
	modelFluxIvar_r  real NOT NULL, --/U nanomaggies^{-2}  --/D Inverse variance in better of DeV/Exp flux fit 
	modelFluxIvar_i  real NOT NULL, --/U nanomaggies^{-2}  --/D Inverse variance in better of DeV/Exp flux fit 
	modelFluxIvar_z  real NOT NULL, --/U nanomaggies^{-2}  --/D Inverse variance in better of DeV/Exp flux fit 
	cModelFlux_u     real NOT NULL, --/U nanomaggies --/D better of DeV+Exp flux 
	cModelFlux_g     real NOT NULL, --/U nanomaggies --/D better of DeV+Exp flux 
	cModelFlux_r     real NOT NULL, --/U nanomaggies --/D better of DeV+Exp flux 
	cModelFlux_i     real NOT NULL, --/U nanomaggies --/D better of DeV+Exp flux 
	cModelFlux_z     real NOT NULL, --/U nanomaggies --/D better of DeV+Exp flux 
	cModelFluxIvar_u real NOT NULL, --/U nanomaggies^{-2}  --/D Inverse variance in DeV+Exp flux fit 
	cModelFluxIvar_g real NOT NULL, --/U nanomaggies^{-2}  --/D Inverse variance in DeV+Exp flux fit 
	cModelFluxIvar_r real NOT NULL, --/U nanomaggies^{-2}  --/D Inverse variance in DeV+Exp flux fit 
	cModelFluxIvar_i real NOT NULL, --/U nanomaggies^{-2}  --/D Inverse variance in DeV+Exp flux fit 
	cModelFluxIvar_z real NOT NULL, --/U nanomaggies^{-2}  --/D Inverse variance in DeV+Exp flux fit 
	aperFlux7_u      real NOT NULL, --/U nanomaggies --/D Aperture flux within 7.3 arcsec --/F aperflux 6
	aperFlux7_g      real NOT NULL, --/U nanomaggies --/D Aperture flux within 7.3 arcsec --/F aperflux 14
	aperFlux7_r      real NOT NULL, --/U nanomaggies --/D Aperture flux within 7.3 arcsec --/F aperflux 22
	aperFlux7_i      real NOT NULL, --/U nanomaggies --/D Aperture flux within 7.3 arcsec --/F aperflux 30
	aperFlux7_z      real NOT NULL, --/U nanomaggies --/D Aperture flux within 7.3 arcsec --/F aperflux 38
	aperFlux7Ivar_u  real NOT NULL, --/U nanomaggies^{-2} --/D Inverse variance of aperture flux within 7.3 arcsec --/F aperflux_ivar 6 
	aperFlux7Ivar_g  real NOT NULL, --/U nanomaggies^{-2} --/D Inverse variance of aperture flux within 7.3 arcsec --/F aperflux_ivar 14
	aperFlux7Ivar_r  real NOT NULL, --/U nanomaggies^{-2} --/D Inverse variance of aperture flux within 7.3 arcsec --/F aperflux_ivar 22
	aperFlux7Ivar_i  real NOT NULL, --/U nanomaggies^{-2} --/D Inverse variance of aperture flux within 7.3 arcsec --/F aperflux_ivar 30 
	aperFlux7Ivar_z  real NOT NULL, --/U nanomaggies^{-2} --/D Inverse variance of aperture flux within 7.3 arcsec --/F aperflux_ivar 38*/
	lnLStar_u        real NOT NULL,	--/D Star ln(likelihood) --/F star_lnl
	lnLStar_g        real NOT NULL,	--/D Star ln(likelihood) --/F star_lnl
	lnLStar_r        real NOT NULL,	--/D Star ln(likelihood) --/F star_lnl
	lnLStar_i        real NOT NULL,	--/D Star ln(likelihood) --/F star_lnl
	lnLStar_z        real NOT NULL,	--/D Star ln(likelihood) --/F star_lnl
	lnLExp_u         real NOT NULL,	--/D Exponential disk fit ln(likelihood) --/F exp_lnl
	lnLExp_g         real NOT NULL,	--/D Exponential disk fit ln(likelihood)  --/F exp_lnl
	lnLExp_r         real NOT NULL,	--/D Exponential disk fit ln(likelihood)  --/F exp_lnl
	lnLExp_i         real NOT NULL,	--/D Exponential disk fit ln(likelihood)  --/F exp_lnl
	lnLExp_z         real NOT NULL,	--/D Exponential disk fit ln(likelihood)  --/F exp_lnl
	lnLDeV_u         real NOT NULL,	--/D de Vaucouleurs fit ln(likelihood) --/F dev_lnl
	lnLDeV_g         real NOT NULL,	--/D de Vaucouleurs fit ln(likelihood) --/F dev_lnl
	lnLDeV_r         real NOT NULL,	--/D de Vaucouleurs fit ln(likelihood) --/F dev_lnl
	lnLDeV_i         real NOT NULL,	--/D de Vaucouleurs fit ln(likelihood) --/F dev_lnl
	lnLDeV_z         real NOT NULL,	--/D de Vaucouleurs fit ln(likelihood) --/F dev_lnl
	fracDeV_u        real NOT NULL,	--/D Weight of deV component in deV+Exp model
	fracDeV_g        real NOT NULL,	--/D Weight of deV component in deV+Exp model
	fracDeV_r        real NOT NULL,	--/D Weight of deV component in deV+Exp model
	fracDeV_i        real NOT NULL,	--/D Weight of deV component in deV+Exp model
	fracDeV_z        real NOT NULL, --/D Weight of deV component in deV+Exp model
	/*flags_u          bigint NOT NULL, --/D Object detection flags per band 
	flags_g          bigint NOT NULL, --/D Object detection flags per band 
	flags_r          bigint NOT NULL, --/D Object detection flags per band 
	flags_i          bigint NOT NULL, --/D Object detection flags per band 
	flags_z          bigint NOT NULL, --/D Object detection flags per band 
	type_u           [int] NOT NULL, --/D Object type classification per band
	type_g           [int] NOT NULL, --/D Object type classification per band
	type_r           [int] NOT NULL, --/D Object type classification per band
	type_i           [int] NOT NULL, --/D Object type classification per band
	type_z           [int] NOT NULL, --/D Object type classification per band
	probPSF_u        real NOT NULL, --/D  Probablity object is a star in each filter.
	probPSF_g        real NOT NULL, --/D  Probablity object is a star in each filter.
	probPSF_r        real NOT NULL, --/D  Probablity object is a star in each filter.
	probPSF_i        real NOT NULL, --/D  Probablity object is a star in each filter.
	probPSF_z        real NOT NULL, --/D  Probablity object is a star in each filter.*/
	ra               float NOT NULL, --/U deg --/D J2000 Right Ascension (r-band)
	dec              float NOT NULL, --/U deg --/D J2000 Declination (r-band)
	cx               float NOT NULL, --/D unit vector for ra+dec
	cy               float NOT NULL, --/D unit vector for ra+dec
	cz               float NOT NULL, --/D unit vector for ra+dec
	raErr            float NOT NULL, --/U arcsec --/D Error in RA (* cos(Dec), that is, proper units)
	decErr           float NOT NULL, --/U arcsec  --/D Error in Dec
	b                float NOT NULL, --/U deg --/D Galactic latitude
	l                float NOT NULL, --/U deg --/D Galactic longitude
	/*offsetRa_u       real NOT NULL, --/U arcsec --/D filter position RA minus final RA (* cos(Dec), that is, proper units)
	offsetRa_g       real NOT NULL, --/U arcsec --/D filter position RA minus final RA (* cos(Dec), that is, proper units)
	offsetRa_r       real NOT NULL, --/U arcsec --/D filter position RA minus final RA (* cos(Dec), that is, proper units)
	offsetRa_i       real NOT NULL, --/U arcsec --/D filter position RA minus final RA (* cos(Dec), that is, proper units)
	offsetRa_z       real NOT NULL, --/U arcsec --/D filter position RA minus final RA (* cos(Dec), that is, proper units)
	offsetDec_u      real NOT NULL, --/U arcsec --/D filter position Dec minus final Dec
	offsetDec_g      real NOT NULL, --/U arcsec --/D filter position Dec minus final Dec
	offsetDec_r      real NOT NULL, --/U arcsec --/D filter position Dec minus final Dec
	offsetDec_i      real NOT NULL, --/U arcsec --/D filter position Dec minus final Dec
	offsetDec_z      real NOT NULL, --/U arcsec --/D filter position Dec minus final Dec*/
	extinction_u     real NOT NULL, --/U mag --/D Extinction in u-band
	extinction_g     real NOT NULL, --/U mag --/D Extinction in g-band
	extinction_r     real NOT NULL, --/U mag --/D Extinction in r-band
	extinction_i     real NOT NULL, --/U mag --/D Extinction in i-band
	extinction_z     real NOT NULL, --/U mag --/D Extinction in z-band
	/*psffwhm_u        real NOT NULL, --/U arcsec --/D FWHM in u-band
	psffwhm_g        real NOT NULL, --/U arcsec --/D FWHM in g-band
	psffwhm_r        real NOT NULL, --/U arcsec --/D FWHM in r-band
	psffwhm_i        real NOT NULL, --/U arcsec --/D FWHM in i-band
	psffwhm_z        real NOT NULL, --/U arcsec --/D FWHM in z-band*/
	mjd              [int] NOT NULL, --/U days --/D Date of observation
	airmass_u        real NOT NULL, --/D Airmass at time of observation in u-band
	airmass_g        real NOT NULL, --/D Airmass at time of observation in g-band
	airmass_r        real NOT NULL, --/D Airmass at time of observation in r-band
	airmass_i        real NOT NULL, --/D Airmass at time of observation in i-band
	airmass_z        real NOT NULL, --/D Airmass at time of observation in z-band
	/*phioffset_u      real NOT NULL, --/U deg --/D Degrees to add to CCD-aligned angle to convert to E of N
	phioffset_g      real NOT NULL, --/U deg --/D Degrees to add to CCD-aligned angle to convert to E of N
	phioffset_r      real NOT NULL, --/U deg --/D Degrees to add to CCD-aligned angle to convert to E of N
	phioffset_i      real NOT NULL, --/U deg --/D Degrees to add to CCD-aligned angle to convert to E of N
	phioffset_z      real NOT NULL, --/U deg --/D Degrees to add to CCD-aligned angle to convert to E of N
	nProf_u          [int] NOT NULL, --/D Number of Profile Bins
	nProf_g          [int] NOT NULL, --/D Number of Profile Bins
	nProf_r          [int] NOT NULL, --/D Number of Profile Bins
	nProf_i          [int] NOT NULL, --/D Number of Profile Bins
	nProf_z          [int] NOT NULL, --/D Number of Profile Bins*/
	loadVersion      [int] NOT NULL, --/D Load Version  --/F NOFITS
	htmID            bigint NOT NULL, --/D 20-deep hierarchical trangular mesh ID of this object --/F NOFITS
	fieldID          bigint NOT NULL, --/D Link to the field this object is in
	parentID         bigint NOT NULL DEFAULT 0, --/D Pointer to parent (if object deblended) or BRIGHT detection (if object has one), else 0
	specObjID        bigint NOT NULL DEFAULT 0, --/D Pointer to the spectrum of object, if exists, else 0 --/F NOFITS
	u                real NOT NULL, --/U mag --/D Shorthand alias for modelMag --/F modelmag
	g                real NOT NULL, --/U mag --/D Shorthand alias for modelMag --/F modelmag
	r                real NOT NULL, --/U mag --/D Shorthand alias for modelMag --/F modelmag
	i                real NOT NULL, --/U mag --/D Shorthand alias for modelMag --/F modelmag
	z                real NOT NULL, --/U mag --/D Shorthand alias for modelMag --/F modelmag
	err_u            real NOT NULL, --/U mag --/D Error in modelMag alias --/F modelmagerr
	err_g            real NOT NULL, --/U mag --/D Error in modelMag alias --/F modelmagerr
	err_r            real NOT NULL, --/U mag --/D Error in modelMag alias --/F modelmagerr
	err_i            real NOT NULL, --/U mag --/D Error in modelMag alias --/F modelmagerr
	err_z            real NOT NULL, --/U mag --/D Error in modelMag alias --/F modelmagerr
	dered_u          real NOT NULL, --/U mag --/D Simplified mag, corrected for extinction: modelMag-extinction --/F NOFITS
	dered_g          real NOT NULL, --/U mag --/D Simplified mag, corrected for extinction: modelMag-extinction --/F NOFITS
	dered_r          real NOT NULL, --/U mag --/D Simplified mag, corrected for extinction: modelMag-extinction --/F NOFITS
	dered_i          real NOT NULL, --/U mag --/D Simplified mag, corrected for extinction: modelMag-extinction --/F NOFITS
	dered_z          real NOT NULL, --/U mag --/D Simplified mag, corrected for extinction: modelMag-extinction --/F NOFITS
	/*cloudCam_u       [int] NOT NULL, --/D Cloud camera status for observation in u-band
	cloudCam_g       [int] NOT NULL, --/D Cloud camera status for observation in g-band
	cloudCam_r       [int] NOT NULL, --/D Cloud camera status for observation in r-band
	cloudCam_i       [int] NOT NULL, --/D Cloud camera status for observation in i-band
	cloudCam_z       [int] NOT NULL, --/D Cloud camera status for observation in z-band*/
	resolveStatus    [int] NOT NULL, --/D Resolve status of object --/R ResolveStatus
	thingId          [int] NOT NULL, --/D Unique identifier from global resolve
	balkanId         [int] NOT NULL, --/D What balkan object is in from window
	nObserve         [int] NOT NULL, --/D Number of observations of this object
	nDetect          [int] NOT NULL, --/D Number of detections of this object
	nEdge            [int] NOT NULL, --/D Number of observations of this object near an edge
	score            real NOT NULL, --/D Quality of field (0-1)
	calibStatus_u    [int] NOT NULL, --/D Calibration status in u-band --/R CalibStatus
	calibStatus_g    [int] NOT NULL, --/D Calibration status in g-band --/R CalibStatus
	calibStatus_r    [int] NOT NULL, --/D Calibration status in r-band --/R CalibStatus
	calibStatus_i    [int] NOT NULL, --/D Calibration status in i-band --/R CalibStatus
	calibStatus_z    [int] NOT NULL, --/D Calibration status in z-band --/R CalibStatus
	/*nMgyPerCount_u   real NOT NULL, --/U nmgy/count --/D nanomaggies per count in u-band
	nMgyPerCount_g   real NOT NULL, --/U nmgy/count --/D nanomaggies per count in g-band
	nMgyPerCount_r   real NOT NULL, --/U nmgy/count --/D nanomaggies per count in r-band
	nMgyPerCount_i   real NOT NULL, --/U nmgy/count --/D nanomaggies per count in i-band
	nMgyPerCount_z   real NOT NULL, --/U nmgy/count --/D nanomaggies per count in z-band*/
	TAI_u            float NOT NULL, --/U sec --/D time of observation (TAI) in each filter
	TAI_g            float NOT NULL, --/U sec --/D time of observation (TAI) in each filter
	TAI_r            float NOT NULL, --/U sec --/D time of observation (TAI) in each filter
	TAI_i            float NOT NULL, --/U sec --/D time of observation (TAI) in each filter
	TAI_z            float NOT NULL, --/U sec --/D time of observation (TAI) in each filter,

	CONSTRAINT PK_PhotoObjAll PRIMARY KEY (objID)
) 
ON PS_PhotoObjAll(objid)
--ON PHOTOOBJ
WITH (DATA_COMPRESSION = PAGE)
GO