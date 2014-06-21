using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Format.VOTable
{
    class Constants
    {
        public const string FileExtensionVOTable = ".votable";
        public const string MimeTypeVOTable = "application/x-votable+xml";

        public const string VOTableVersion = "1.2";

        // Different Key used in VOTable document
        public const string VOTableKeywordVOTable = "VOTABLE";
        public const string VOTableKeywordVersion = "version";
        public const string VOTableKeywordResource = "RESOURCE";
        public const string VOTableKeywordTable = "TABLE";
        public const string VOTableKeywordField = "FIELD";
        public const string VOTableKeywordTableData = "TABLEDATA";
        public const string VOTableKeywordData = "DATA";
        public const string VOTableKeywordTR = "TR";
        public const string VOTableKeywordTD = "TD";
        public const string VOTableKeywordName = "name";
        public const string VOTableKeywordDescription = "description";
        public const string VOTableKeywordUType = "utype";
        public const string VOTableKeywordDataType = "datatype";
        public const string VOTableKeywordUcd = "ucd";
        public const string VOTableKeywordUnit = "unit";
        public const string VOTableKeywordPrecision = "precision";
        public const string VOTableKeywordWidth = "width";
        public const string VOTableKeywordArraySize = "arraysize";

        // Name spaces
        public const string VOTableXsi = "http://www.w3.org/2001/XMLSchema-instance";
        public const string VOTableNs = "http://www.ivoa.net/xml/VOTable/v1.2";
        public const string StcNs = "http://www.ivoa.net/xml/STC/v1.30";


        //public const String dtdFilename = "http://www.ivoa.net/internal/IVOA/IvoaVOTable/VOTable-1.2.dtd";
        //public const String xsdFileName = "http://www.ivoa.net/xml/VOTable/VOTable-1.2.xsd";
        //public const String votableNameSpace = "http://www.ivoa.net/xml/VOTable/v1.2";

        public const string VOTableTypeBoolean = "boolean";
        public const string VOTableTypeBit = "bit";
        public const string VOTableTypeByte = "unsignedbyte";
        public const string VOTableTypeShort = "short";
        public const string VOTableTypeInt = "int";
        public const string VOTableTypeLong = "long";
        public const string VOTableTypeChar = "char";
        public const string VOTableTypeUnicodeChar = "unicodechar";
        public const string VOTableTypeFloat = "float";
        public const string VOTableTypeDouble = "double";
        public const string VOTableTypeFloatComplex = "floatcomplex";
        public const string VOTableTypeDoubleComplex = "doublecomplex";

        /*public static String[] vodatatypes = new String[] { 
           "boolean", "bit","unsignedbyte","short","int","long","char","unicodechar","float","double","floatcomplex","doublecomplex"
        };*/
    }

}
