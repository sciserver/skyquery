using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Threading;
using Jhu.SkyQuery.Format.VOTable;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapCommand : DbCommand
    {
        private enum TapCommandState
        {
            Initializing,
            Submitting,
            Executing,
            Timeout,
            Error,
            Completed,
        }

        #region Private member variables

        private TapConnection connection;
        private TapTransaction transaction;
        private TapParameterCollection parameters;
        private TapQueryLanguage queryLanguage;
        private string commandText;
        private int commandTimeout;
        private CommandType commandType;

        private bool designTimeVisible;
        private UpdateRowSource updateRowSource;

        private TapCommandState state;
        private CancellationTokenSource cancellationSource;

        private DbDataReader reader;

        #endregion
        #region Properties

        protected override DbConnection DbConnection
        {
            get { return connection; }
            set
            {
                EnsureNotExecuting();
                connection = (TapConnection)value;
            }
        }

        public new TapConnection Connection
        {
            get { return connection; }
            set
            {
                EnsureNotExecuting();
                connection = value;
            }
        }

        protected override DbTransaction DbTransaction
        {
            get { return transaction; }
            set
            {
                EnsureNotExecuting();
                transaction = (TapTransaction)value;
            }
        }

        public new TapTransaction Transaction
        {
            get { return transaction; }
            set
            {
                EnsureNotExecuting();
                transaction = value;
            }
        }

        public new TapParameterCollection Parameters
        {
            get { return parameters; }
            set
            {
                EnsureNotExecuting();
                parameters = value;
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return parameters; }
        }

        public TapQueryLanguage QueryLanguage
        {
            get { return queryLanguage; }
            set
            {
                EnsureNotExecuting();
                SetQueryLanguage(value);
            }
        }

        public override string CommandText
        {
            get { return commandText; }
            set
            {
                EnsureNotExecuting();
                commandText = value;
            }
        }

        public override int CommandTimeout
        {
            get { return commandTimeout; }
            set
            {
                EnsureNotExecuting();
                commandTimeout = value;
            }
        }

        public override CommandType CommandType
        {
            get { return commandType; }
            set
            {
                EnsureNotExecuting();
                SetCommandType(value);
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

            this.state = TapCommandState.Initializing;
            this.cancellationSource = null;

            this.reader = null;
        }

        #endregion
        #region Validation functions

        private void EnsureNotExecuting()
        {
            if (state != TapCommandState.Initializing)
            {
                throw Error.CommandExecuting();
            }
        }

        private void SetCommandType(CommandType commandType)
        {
            if (commandType != CommandType.Text)
            {
                throw Error.InvalidTapCommandType();
            }

            this.commandType = commandType;
        }

        private void SetQueryLanguage(TapQueryLanguage queryLanguage)
        {
            if (queryLanguage != TapQueryLanguage.Adql)
            {
                throw Error.UnsupportedQueryLanguage(queryLanguage);
            }

            this.queryLanguage = queryLanguage;
        }

        #endregion
        #region Cancellation logic

        private CancellationTokenRegistration RegisterCancellation(CancellationToken cancellationToken)
        {
            if (cancellationToken.CanBeCanceled)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw Error.OperationCancelled(cancellationToken);
                }

                return cancellationToken.Register(HandleCancelRequest);
            }
            else
            {
                return new CancellationTokenRegistration();
            }
        }

        private void HandleCancelRequest()
        {
            // This is a cancel request that came from the outside and not
            // initiated by the Cancel method

            Cancel();
        }

        public override void Cancel()
        {
            // TODO

            // Cancel everything async called from this class
            if (cancellationSource != null)
            {
                cancellationSource.Cancel();
            }

            // Cancel the executing reader
            if (reader != null)
            {
                // TODO: cancel reader if still reading response stream    
                // TODO: implement cancel logic into IO library
            }
        }

        #endregion

        private TapJob CreateTapJob()
        {
            var job = new TapJob()
            {
                Query = commandText,
                Language = queryLanguage,
            };

            if (commandTimeout != 0)
            {
                job.Destruction = DateTime.Now.AddSeconds(commandTimeout);
            }

            return job;
        }

        private TapClient CreateTapClient()
        {
            return new Client.TapClient()
            {
                BaseAddress = new Uri(connection.DataSource),
                HttpTimeout = TimeSpan.FromSeconds(connection.ConnectionTimeout),
                PollTimeout = TimeSpan.FromSeconds(commandTimeout),
            };
        }

        protected override DbParameter CreateDbParameter()
        {
            return new TapParameter();
        }

        public override void Prepare()
        {
            // This does nothing on a TAP data source
        }

        public override int ExecuteNonQuery()
        {
            // This does nothing on a TAP data source
            throw new NotImplementedException();
        }

        public override object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public override async Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        {
            EnsureNotExecuting();

            var registration = RegisterCancellation(cancellationToken);
            var job = CreateTapJob();
            var client = CreateTapClient();

            try
            {
                // Create source that will be used to cancel async http calls
                cancellationSource = new CancellationTokenSource();

                // 1. Submit job
                state = TapCommandState.Submitting;
                await client.SubmitAsync(job, cancellationSource.Token);

                // 2. Poll status
                state = TapCommandState.Executing;
                await client.PollAsync(job, new[] { TapJobPhase.Completed, TapJobPhase.Aborted, TapJobPhase.Error }, null, cancellationSource.Token);

                // 3. Process output
                if (job.Phase == TapJobPhase.Completed)
                {
                    // TODO: return data reader
                    var stream = await client.GetResultsAsync(job, cancellationSource.Token);
                    var votable = new VOTable(stream, Graywulf.IO.DataFileMode.Read);
                    reader = new TapDataReader(this, votable);

                    return reader;
                }
                else if (job.Phase == TapJobPhase.Aborted)
                {
                    throw Error.CommandCancelled();
                }
                else if (job.Phase == TapJobPhase.Error)
                {
                    var stream = await client.GetErrorAsync(job, cancellationSource.Token);
                    var votable = new VOTable(stream, Graywulf.IO.DataFileMode.Read);

                    // TODO: read error document, parse VOTable and report error
                    // TODO: throw TAP exception with message from server

                    throw new TapException();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (TapException)
            {
                state = TapCommandState.Error;
                throw;
            }
            catch (OperationCanceledException ex)
            {
                state = TapCommandState.Error;
                throw Error.CommandCancelled(ex);
            }
            catch (Exception ex)
            {
                state = TapCommandState.Error;
                throw Error.CommunicationException(ex);
            }
            finally
            {
                cancellationSource.Dispose();
                cancellationSource = null;

                client.Dispose();
                registration.Dispose();
            }
        }
        
    }
}
