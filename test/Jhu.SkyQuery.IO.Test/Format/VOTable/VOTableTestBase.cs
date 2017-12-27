using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Util;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.VoTable
{
    public class VOTableTestBase : TestClassBase
    {
        protected FileDataReader OpenSimpleReader(string path)
        {
            path = Path.Combine(GetTestFilePath(@"modules\skyquery\test\files"), path);

            var f = new VoTableWrapper(
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
