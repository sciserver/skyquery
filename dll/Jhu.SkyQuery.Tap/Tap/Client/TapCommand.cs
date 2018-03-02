using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Threading;
using Jhu.SkyQuery.Format.VoTable;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapCommand : DbCommand, IDisposable
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

        private bool isAsync;
        private TimeSpan destruction;

        private bool designTimeVisible;
        private UpdateRowSource updateRowSource;

        private TapCommandState state;

        private Stream stream;
        private VoTableWrapper votable;
        private DbDataReader reader;
        private CancellationTokenSource cancellationSource;
        private CancellationTokenRegistration cancellationRegistration;

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

        public bool IsAsync
        {
            get { return isAsync; }
            set { isAsync = value; }
        }

        public TimeSpan Destruction
        {
            get { return destruction; }
            set { destruction = value; }
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

        public TapCommand(string cmdText)
        {
            InitializeMembers();

            this.commandText = cmdText;
        }

        public TapCommand(string cmdText, TapConnection connection)
        {
            InitializeMembers();

            this.commandText = cmdText;
            this.connection = connection;
        }

        private void InitializeMembers()
        {
            this.connection = null;
            this.transaction = null;
            this.queryLanguage = TapQueryLanguage.Adql;
            this.commandText = null;
            this.commandTimeout = Constants.DefaultCommandTimeout;
            this.commandType = CommandType.Text;

            this.isAsync = true;
            this.destruction = TimeSpan.FromDays(2);

            this.designTimeVisible = false;
            this.updateRowSource = UpdateRowSource.None;

            this.state = TapCommandState.Initializing;

            this.stream = null;
            this.votable = null;
            this.reader = null;
            this.cancellationSource = null;
            this.cancellationRegistration = new CancellationTokenRegistration();
        }

        protected override void Dispose(bool disposing)
        {
            if (cancellationSource != null)
            {
                cancellationSource.Dispose();
                cancellationSource = null;
            }

            if (votable != null)
            {
                votable.Dispose();
                votable = null;
            }

            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }

            base.Dispose(disposing);
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
            connection.Client.GetBestFormat(out TapOutputFormat format, out string mime);

            var job = new TapJob()
            {
                Query = commandText,
                Language = queryLanguage,
                Format = mime,
                Destruction = DateTime.UtcNow + destruction,
                IsAsync = isAsync
            };

            return job;
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
            // TODO
            throw new NotImplementedException();
        }

        public override async Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
        {
            // TODO
            throw new NotImplementedException();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return Jhu.Graywulf.Util.TaskHelper.Wait(ExecuteDbDataReaderAsync(behavior, CancellationToken.None));
        }

        protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        {
            EnsureNotExecuting();

            // TODO: support the following special behaviors:
            // CommandBehavior.SchemaOnly
            // CommandBehavior.KeyInfo (can TAP give back key info?)

            var job = CreateTapJob();

            // Create source that will be used to cancel async http calls
            cancellationRegistration = RegisterCancellation(cancellationToken);
            cancellationSource = new CancellationTokenSource();

            try
            {
                // 1. Submit job
                state = TapCommandState.Submitting;
                connection.Client.PollTimeout = TimeSpan.FromSeconds(commandTimeout);
                var result = await connection.Client.SubmitAsync(job, cancellationSource.Token);

                if (!isAsync)
                {
                    if (result.StatusCode == System.Net.HttpStatusCode.RedirectMethod)
                    {
                        stream = await connection.Client.GetResultsAsync(job, cancellationSource.Token);
                    }
                    else
                    {
                        stream = await result.Content.ReadAsStreamAsync();
                    }
                }
                else
                {
                    // 2. Poll status
                    state = TapCommandState.Executing;
                    await connection.Client.PollAsync(job, new[] { TapJobPhase.Completed, TapJobPhase.Aborted, TapJobPhase.Error }, null, cancellationSource.Token);

                    // 3. Process output
                    if (job.Phase == TapJobPhase.Completed)
                    {
                        stream = await connection.Client.GetResultsAsync(job, cancellationSource.Token);
                    }
                    else if (job.Phase == TapJobPhase.Aborted)
                    {
                        throw Error.CommandCancelled();
                    }
                    else if (job.Phase == TapJobPhase.Error)
                    {
                        stream = await connection.Client.GetErrorAsync(job, cancellationSource.Token);
                        votable = new VoTableWrapper(stream, Graywulf.IO.DataFileMode.Read);

                        // TODO: read error document, parse VOTable and report error
                        // TODO: throw TAP exception with message from server

                        throw new TapException();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                votable = new VoTableWrapper()
                {
                    GenerateIdentityColumn = false,
                };
                await votable.OpenAsync(stream, Graywulf.IO.DataFileMode.Read);
                reader = new TapDataReader(this, votable);

                // TODO: get query status from VOTable

                return reader;
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
        }
    }
}
