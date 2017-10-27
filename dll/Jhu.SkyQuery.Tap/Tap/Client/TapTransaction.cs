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
        enum TapTransactionState
        {
            Open,
            Commited,
            RolledBack,
        }

        #region Private member variables
        
        private TapConnection connection;
        private IsolationLevel isolationLevel;
        private TapTransactionState state;

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
            this.state = TapTransactionState.Open;
        }

        #endregion

        public override void Commit()
        {
            state = TapTransactionState.Commited;
        }

        public override void Rollback()
        {
            state = TapTransactionState.RolledBack;
        }
    }
}
