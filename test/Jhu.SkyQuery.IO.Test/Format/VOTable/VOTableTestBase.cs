using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Util;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.VOTable
{
    public class VOTableTestBase : TestClassBase
    {
        protected void ValidateVOTable(string xml)
        {
            // Read an XML file and validate against the xsd schema

            var settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // TODO: add the XSD
            //Resources.Schema_VoTable_v1_3

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(new StringReader(xml), settings);

            // Parse the file. 
            while (reader.Read()) ;

        }

        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            }
            else
            {
                throw new Exception(args.Message);
            }
        }

        protected FileDataReader OpenSimpleReader(string path)
        {
            path = Path.Combine(GetTestFilePath(@"modules\skyquery\test\files"), path);

            var f = new VOTable(
                UriConverter.FromFilePath(path),
                DataFileMode.Read
                )
            {
                GenerateIdentityColumn = false
            };

            var cmd = new FileCommand(f);

            return cmd.ExecuteReader();
        }
    }
}
