using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Jhu.Graywulf.Format;
using Jhu.SkyQuery.Format.VOTable;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapDataReader : FileDataReader
    {
        private TapCommand command;

        public TapDataReader(TapCommand command, VOTable votable)
            : base(votable)
        {
        }
    }
}
