using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapCommand : DbCommand
    {
        #region Private member variables

        private TapConnection connection;
        private TapTransaction transaction;
        private string commandText;
        private int commandTimeout;
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

        public override CommandType CommandType
        {
            get { return commandType; }
            set
            {
                if (commandType != CommandType.Text)
                {
                    throw Error.InvalidTapCommandType();
                }
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                throw new NotImplementedException();
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

        public TapCommand()
        {
        }

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

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }
    }
}
