# -*- coding: utf-8 -*-
"""
Read FIRST Catalog
- Convert RA Dec
- Write to binary file
"""
import numpy as np
import pandas as pd

# function for converting from HHMMSS.s DDMMSS.s

def hmsdms2dd_no(ra,dec):
   i=0
   newRa, newDec = np.array(np.zeros(len(ra))),np.array(np.zeros(len(dec)))
   for item in ra:
      newRa[i] = (int(item[0:2])+ int(item[2:4])/60.+ float(item[4:])/3600.)*15
      i+=1
   i=0
   for item in dec:
      newDec[i] = int(item[0:3]) + int(item[3:5])/60. + float(item[5:]) /3600.
      i+=1
   return newRa.astype(np.float), newDec.astype(np.float)

# CREATE A PANDAS DATA FRAME FROM DATA
# set first 6 cols dtype as str
dtypes = {"RA1":str,"RA2":str,"RA3":str,"Dec1":str,"Dec2":str,"Dec3":str}  

# source folder
src = r"C:\Data\ebanyai\project\FIRST\catalog_14dec17.bin"  

# setting up a new header
header = ["RA1", "RA2", "RA3", "Dec1", "Dec2", "Dec3",  "Ps", "Fpeak", "Fint", "RMS", 
          "Maj", "Min", "PA", "fMaj", "fMin", "fPA", "Field", "num_SDSS", "Sep_SDSS",
          "i_SDSS", "Cl_SDSS", "num_2MASS",  "Sep_2MASS", "K_2MASS", "Mean-yr",
          "Mean-MJD", "rms-MJD"]        
          
# grab the data
data = pd.read_csv(src,dtype=dtypes, sep=" ",skipinitialspace=True,index_col=None,names=header,header=None,comment="#")

# CREATE PROPER FORMAT FOR CONVERTING & CONVERT 
data["RA_hms"] = data.apply(lambda x:'%s%s%s' % (x['RA1'],x['RA2'],x["RA3"]),axis=1)
data["Dec_hms"] = data.apply(lambda x:'%s%s%s' % (x['Dec1'],x['Dec2'],x["Dec3"]),axis=1)

ra,dec= hmsdms2dd_no(data["RA_hms"],data["Dec_hms"])

# DROP UNNECESSARY COLUMNS
data.drop(data.columns[[0,1,2,3,4,5,-1,-2]], axis=1, inplace=True)

# INSERT CONVERTED RA & Dec
data.insert(1,"RA",ra)
data.insert(2,"Dec",dec)


# DEFINE DATA TYPES FOR BINARY FORMAT
dt_data = np.dtype([("index","i8"),
                    ("RA","f8"),
                    ("Dec","f8"),
                    ("Ps","f4"),
                    ("Fpeak","f4"),
                    ("Fint","f4"),
                    ("RMS","f4"),
                    ("Maj","f4"),
                    ("Min","f4"),
                    ("PA","f4"),
                    ("fMaj","f4"),
                    ("fMin","f4"),
                    ("fPA","f4"),
                    ("Field","S12"),
                    ("num_SDSS","i2"),
                    ("Sep_SDSS","f4"),
                    ("i_SDSS","f4"),
                    ("Cl_SDSS","S1"),
                    ("num_2MASS","i2"),
                    ("Sep_2MASS","f4"),
                    ("K_2MASS","f4"),
                    ("Mean-yr","f4"),
                    ("Mean-MJD","f8"),
                    ("rms-MJD","f4")])



# CREATE A NUMPY ARRAY WITH THE PROPER DATA TYPES & WRITE INTO FILE
# set datatypes for binary format
records = np.array(data.to_records(),dtype=dt_data) 

# destination folder
dst = r".\FIRST_binTest.npy" 

# write to file
records.tofile(dst)


