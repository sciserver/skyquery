using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Format
{
    class VOTableKeywords
    {
        // Different Key used in VOTable document
        public const string VoTable = "VOTABLE";
        public const string Resource = "RESOURCE";
        public const string Table = "TABLE";
        public const string Field = "FIELD";
        public const string TableData = "TABLEDATA";
        public const string Data = "DATA";
        public const string TR = "TR";
        public const string TD = "TD";
        public const string Name = "name";
        public const string Description = "description";
        public const string UType = "Utype";
        public const string DataType = "datatype";
        public const string Ucd = "UCD";
        public const string Unit = "unit";
        public const string Precision = "precision";
        public const string Width = "width";
        public const string ArraySize = "arraysize";
        
        // Name spaces
        public const String dtdFilename = "http://www.ivoa.net/internal/IVOA/IvoaVOTable/VOTable-1.2.dtd";
        public const String xsdFileName = "http://www.ivoa.net/xml/VOTable/VOTable-1.2.xsd";
        public const String votableNameSpace = "http://www.ivoa.net/xml/VOTable/v1.2";

        public static String[] vodatatypes = new String[] { 
           "boolean", "bit","unsignedbyte","short","int","long","char","unicodechar","float","double","floatcomplex","doublecomplex"
        };
    }

}
