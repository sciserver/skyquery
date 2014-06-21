using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Schema;

namespace Jhu.SkyQuery.Format.Fits
{
    public static class Constants
    {
        public const string FileExtensionFits = ".fits";
        public const string MimeTypeFits = "image/fits";

        public const string FitsKeywordSimple = "SIMPLE";
        public const string FitsKeywordExtend = "EXTEND";
        public const string FitsKeywordXtension = "XTENSION";

        public const string FitsKeywordBinTable = "BINTABLE";
        public const string FitsKeywordBitPix = "BITPIX";
        public const string FitsKeywordNAxis = "NAXIS";

        public const string FitsKeywordGCount = "GCOUNT";
        public const string FitsKeywordPCount = "PCOUNT";
        public const string FitsKeywordTFields = "TFIELDS";

        public const string FitsKeywordTForm = "TFORM";
        public const string FitsKeywordTType = "TTYPE";
        public const string FitsKeywordTUnit = "TUNIT";
        public const string FitsKeywordTNull = "TNULL";
        public const string FitsKeywordTScal = "TSCAL";
        public const string FitsKeywordTZero = "TZERO";
        public const string FitsKeywordTDisp = "TDISP";
    }
}
