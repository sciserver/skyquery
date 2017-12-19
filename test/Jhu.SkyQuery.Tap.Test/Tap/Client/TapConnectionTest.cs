using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Tap.Client
{
    public abstract class TapConnectionTest
    {
        protected abstract string ConnectionString { get; }

        protected void OpenConnectionTestHelper()
        {
            using (var cn = new TapConnection(ConnectionString))
            {
                cn.Open();
            }
        }
    }
}
