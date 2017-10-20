using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapTransaction : DbTransaction
    {
        #region Private member variables

        private TapConnection connection;
        private IsolationLevel isolationLevel;

        #endregion
        #region Properties

        protected override DbConnection DbConnection
        {
            get { return connection; }
        }

        public override IsolationLevel IsolationLevel
        {
            get { return isolationLevel; }
        }

        #endregion
        #region Constructors and initializers

        internal TapTransaction(TapConnection connection, IsolationLevel isolationLevel)
        {
            this.connection = connection;
            this.isolationLevel = isolationLevel;
        }

        #endregion

        public override void Commit()
        {
            // Nothing to do here
        }

        public override void Rollback()
        {
            // Nothing to do here
        }
    }
}
