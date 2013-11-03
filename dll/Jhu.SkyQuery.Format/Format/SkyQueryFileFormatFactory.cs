using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format
{
    public class SkyQueryFileFormatFactory : Jhu.Graywulf.Format.FileFormatFactory
    {
        public SkyQueryFileFormatFactory()
        {
        }

        protected override void OnCreateFileFormatDescriptions(HashSet<Type> fileTypes)
        {
            base.OnCreateFileFormatDescriptions(fileTypes);

            fileTypes.Add(typeof(VOTable.VOTable));
            fileTypes.Add(typeof(Fits.Fits));
        }
    }
}
