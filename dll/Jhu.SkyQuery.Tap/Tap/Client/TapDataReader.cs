﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Jhu.Graywulf.Format;
using Jhu.SkyQuery.Format.VoTable;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapDataReader : FileDataReader
    {
        private TapCommand command;

        public TapDataReader(TapCommand command, VoTableWrapper votable)
            : base(votable)
        {
            this.command = command;
        }
    }
}
