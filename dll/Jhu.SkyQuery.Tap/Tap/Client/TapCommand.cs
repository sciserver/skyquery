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
    public class TapCommand : DbCommand
    {
        #region Private member variables

        private TapConnection connection;
        private TapTransaction transaction;
        private TapParameterCollection parameters;
        private TapQueryLanguage queryLanguage;
        private string commandText;
        private int commandTimeout;
        private int pollingTimeout;
        private CommandType commandType;

        private bool designTimeVisible;
        private UpdateRowSource updateRowSource;

        #endregion
        #region Properties

        protected override DbConnection DbConnection
        {
            get { return connection; }
            set { connection = (TapConnection)value; }
        }

        public new TapConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        protected override DbTransaction DbTransaction
        {
            get { return transaction; }
            set { transaction = (TapTransaction)value; }
        }

        public new TapTransaction Transaction
        {
            get { return transaction; }
            set { transaction = value; }
        }

        public new TapParameterCollection Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return parameters; }
        }

        public TapQueryLanguage QueryLanguage
        {
            get { return queryLanguage; }
            set { queryLanguage = value; }
        }

        public override string CommandText
        {
            get { return commandText; }
            set { commandText = value; }
        }

        public override int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        public int PollingTimeout
        {
            get { return pollingTimeout; }
            set { pollingTimeout = value; }
        }

        public override CommandType CommandType
        {
            get { return commandType; }
            set
            {
                if (commandType != CommandType.Text)
                {
                    throw Error.InvalidTapCommandType();
                }

                commandType = value;
            }
        }
        
        public override bool DesignTimeVisible
        {
            get { return designTimeVisible; }
            set { designTimeVisible = value; }
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get { return updateRowSource; }
            set { updateRowSource = value; }
        }


        #endregion
        #region Constructors and initializers

        public TapCommand()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.connection = null;
            this.transaction = null;
            this.queryLanguage = TapQueryLanguage.Adql;
            this.commandText = null;
            this.commandTimeout = Constants.DefaultCommandTimeout;
            this.commandType = CommandType.Text;

            this.designTimeVisible = false;
            this.updateRowSource = UpdateRowSource.None;
        }

        #endregion

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar()
        {
            throw new NotImplementedException();
        }
        
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            // TODO: send async request, start polling and block until header
            // data is available. Then return data reader and start streaming.

            throw new NotImplementedException();
        }

        protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        {
            // TODO: send async request and yield until response arrives. Then yield and
            // start polling thread. Once results start streaming, return datareader and
            // stream back data.
            
            return base.ExecuteDbDataReaderAsync(behavior, cancellationToken);
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }


        #region TAP HTTP implementation

        

        private async Task<string> SendTapRequest()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
