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

        protected override IEnumerable<FileFormatMapping> OnFileFormatsLoading()
        {
            foreach (var format in base.OnFileFormatsLoading())
            {
                yield return format;
            }

            yield return new FileFormatMapping
            {
                Extension = VOTable.Constants.FileExtensionVOTable,
                MimeType = VOTable.Constants.MimeTypeVOTable,
                Type = typeof(VOTable.VOTable)
            };

            yield return new FileFormatMapping
            {
                Extension = Fits.Constants.FileExtensionFits,
                MimeType = Fits.Constants.MimeTypeFits,
                Type = typeof(Fits.FitsFileWrapper)
            };
        }
    }
}
