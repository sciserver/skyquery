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
        protected SkyQueryFileFormatFactory()
        {
        }

        protected override IEnumerable<DataFileBase> OnFilesLoading()
        {
            foreach (var file in base.OnFilesLoading())
            {
                yield return file;
            }

            yield return new VOTable.VOTable();
            yield return new Fits.FitsFileWrapper();
        }
    }
}
