using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Threading;

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

        private TapClient client;

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

        internal TapClient Client
        {
            get { return client; }
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

            this.client = null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (state != ConnectionState.Closed)
            {
                Close();
            }

            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        #endregion

        private TapClient CreateTapClient()
        {
            return new Client.TapClient()
            {
                BaseAddress = new Uri(dataSource),
                HttpTimeout = TimeSpan.FromSeconds(connectionTimeout),
            };
        }

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
            Jhu.Graywulf.Util.TaskHelper.Wait(OpenAsync(CancellationToken.None));
        }

        public override async Task OpenAsync(CancellationToken cancellationToken)
        {
            this.client = CreateTapClient();

            var avail = await client.GetAvailabilityAsync(cancellationToken);

            if (avail.Available)
            {
                var cap = await client.GetCapabilitiesAsync(cancellationToken);
                this.state = ConnectionState.Open;
            }
            else
            {
                Close();
                throw Error.ServiceNotAvailable(avail);
            }
        }

        public override void Close()
        {
            // As with open, this is not necessary as we don't keep HTTP
            // connection open. Just set the state to Closed.

            this.state = ConnectionState.Closed;

            if (client != null)
            {
                this.client.Dispose();
                this.client = null;
            }
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
