using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapConnection : DbConnection
    {
        #region Private member variables

        private string connectionString;
        private int connectionTimeout;
        private string dataSource;
        private string serverVersion;

        private ConnectionState state;

        #endregion
        #region Properties

        public override string ConnectionString
        {
            get { return connectionString; }
            set
            {
                EnsureConnectionClosed();
                SetConnectionString(connectionString);
            }
        }

        public override int ConnectionTimeout
        {
            get { return connectionTimeout; }
        }

        public override string Database
        {
            get { throw new NotImplementedException(); }
        }

        public override string DataSource
        {
            get { return dataSource; }
        }

        public override string ServerVersion
        {
            get { return serverVersion; }
        }

        public override ConnectionState State
        {
            get { return state; }
        }

        #endregion
        #region Constructors and initializers

        public TapConnection()
        {
            InitializeMembers();
        }

        public TapConnection(string connectionString)
        {
            InitializeMembers();

            SetConnectionString(connectionString);
        }

        private void InitializeMembers()
        {
            this.connectionString = null;
            this.connectionTimeout = Constants.DefaultConnectionTimeout;
            this.dataSource = null;
            this.serverVersion = null;

            this.state = ConnectionState.Closed;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (state != ConnectionState.Closed)
            {
                Close();
            }
        }

        #endregion

        private void EnsureConnectionClosed()
        {
            // TODO: throw exception if not closed
        }

        private void EnsureConnectionOpen()
        {
            // TODO: throw exception if not open
        }

        private void SetConnectionString(string connectionString)
        {
            var csb = new TapConnectionStringBuilder(connectionString);

            this.connectionString = connectionString;
            dataSource = csb.DataSource;
            connectionTimeout = csb.ConnectTimeout;
        }

        public override void Open()
        {
            // Because we don't keep any HTTP connections alive,
            // implementing this is not necessary. What we could
            // do here is call /availability or /capabilities
            // but that would require implementing VOSI which we
            // won't do until working on the TAP server.

            // However, we require calling this for consistency reasons
            // and set the state to Open

            this.state = ConnectionState.Open;
        }

        public override void Close()
        {
            // As with open, this is not necessary as we don't keep HTTP
            // connection open. Just set the state to Closed.

            this.state = ConnectionState.Closed;
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            // TAP doesn't support any kind of transactions but
            // we return a dummy transaction object here for compatibility reasons.

            return new TapTransaction(this, isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new TapCommand();
        }
    }
}
