using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.Graywulf.Format
{
    class VOTableKeywords
    {
        /// <summary>
        /// Different Key used in VOTable document
        /// </summary>
        public static String VOTableStart  = "VOTABLE";
        public static String VOResource    = "RESOURCE";
        public static String VOTable       = "TABLE";
        public static String VOField       = "FIELD";
        public static String VOTabledata   = "TABLEDATA";
        public static String VOdata        = "Data";
        public static String TR            = "TR";
        public static String TD            = "TD";
        public static String name          = "name";
        public static String description   = "description";
        public static String utype         = "Utype";
        public static String datatype      = "datatype";
        public static String ucd           = "UCD";        
        public static String unit          = "unit";
        public static String precision     = "precision";
        public static String width         = "width";
        public static String arraysize     = "arraysize";
        /// <summary>
        /// Name spaces
        /// </summary>
        public static String dtdFilename        = "http://www.ivoa.net/internal/IVOA/IvoaVOTable/VOTable-1.2.dtd";
        public static String xsdFileName        = "http://www.ivoa.net/xml/VOTable/VOTable-1.2.xsd";
        public static String votableNameSpace   = "http://www.ivoa.net/xml/VOTable/v1.2";

        public static String[] vodatatypes = new String[] { 
           "boolean", "bit","unsignedbyte","short","int","long","char","unicodechar","float","double","floatcomplex","doublecomplex"
        };
    }
    
}
