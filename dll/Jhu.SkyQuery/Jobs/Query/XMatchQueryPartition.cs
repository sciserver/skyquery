using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.SkyQuery.Parser;
using Jhu.Graywulf.IO.Tasks;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    public abstract class XMatchQueryPartition : RegionQueryPartition
    {
        #region Property storage variables

        /// <summary>
        /// XMatch execution steps,
        /// initialized by <see cref="GenerateSteps"/>
        /// </summary>
        protected List<XMatchQueryStep> steps;

        #endregion
        #region Properties

        /// <summary>
        /// Gets the list of XMatch steps
        /// </summary>
        public List<XMatchQueryStep> Steps
        {
            get { return steps; }
        }

        public new XMatchQuery Query
        {
            get { return (XMatchQuery)base.Query; }
        }

        private XMatchQueryCodeGenerator CodeGenerator
        {
            get { return (XMatchQueryCodeGenerator)CreateCodeGenerator(); }
        }

        #endregion
        #region Constructors and initializers

        public XMatchQueryPartition()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public XMatchQueryPartition(XMatchQueryPartition old)
            : base(old)
        {
            CopyMembers(old);
        }

        public XMatchQueryPartition(XMatchQuery query)
            : base(query)
        {
            InitializeMembers(new StreamingContext());
        }

        /// <summary>
        /// Initializes members variables.
        /// </summary>
        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            this.steps = new List<XMatchQueryStep>();
        }

        private void CopyMembers(XMatchQueryPartition old)
        {
            this.steps = new List<XMatchQueryStep>();   // TODO: do deep copy here?
        }

        #endregion
        #region Step generation

        /// <summary>
        /// When overriden in derived classes, initializes the
        /// <see cref="Steps"/> collection.
        /// </summary>
        /// <param name="tables"></param>
        public abstract void GenerateSteps(IEnumerable<XMatchTableSpecification> tables);

        #endregion
        #region Compute search radius

        public async Task ComputeMinMaxError(XMatchQueryStep step)
        {
            // TODO: implement min/max compute if no error is specified
            // TODO: add logging

            double min, max;

            using (var cmd = CodeGenerator.GetComputeMinMaxErrorCommand(step))
            {
                await ExecuteSqlOnAssignedServerReaderAsync(cmd, CommandTarget.Temp,
                    async (dr, ct) =>
                    {
                        if (await dr.ReadAsync(ct))
                        {
                            min = dr.GetDouble(0);
                            max = dr.GetDouble(1);
                        }
                    });
            }

            // TODO: propagate these results to the table
        }

        public void PrepareComputeSearchRadius(XMatchQueryStep step, out SqlCommand computeSearchRadiusCommand)
        {
            OnPrepareComputeSearchRadius(step, out computeSearchRadiusCommand);
        }

        public abstract void OnPrepareComputeSearchRadius(XMatchQueryStep step, out SqlCommand computeSearchRadiusCommand);

        public async Task ComputeSearchRadiusAsync(XMatchQueryStep step, SqlCommand computeSearchRadiusCommand)
        {
            if (computeSearchRadiusCommand != null)
            {
                var w = System.Diagnostics.Stopwatch.StartNew();

                await OnComputeSearchRadiusAsync(step, computeSearchRadiusCommand);

                LogOperation(LogMessages.SearchRadiusComputed, step.StepNumber, this.ID, w.Elapsed.TotalSeconds, step.SearchRadius, step.ZoneBuffer);
            }
        }

        public abstract Task OnComputeSearchRadiusAsync(XMatchQueryStep step, SqlCommand computeSearchRadiusCommand);

        #endregion
        #region ZoneDef table function

        public async Task CreateZoneDefTableAsync(XMatchQueryStep step)
        {
            if (step.StepNumber > 0)
            {
                var w = System.Diagnostics.Stopwatch.StartNew();

                var zonedeftable = CodeGenerator.GetZoneDefTable(step);

                // Drop table if it exists (unlikely, but might happen during debugging)
                zonedeftable.Drop();

                using (var cmd = CodeGenerator.GetCreateZoneDefTableCommand(step, zonedeftable))
                {
                    await ExecuteSqlOnAssignedServerAsync(cmd, CommandTarget.Temp);
                }

                using (var cmd = CodeGenerator.GetPopulateZoneDefTableCommand(step, zonedeftable))
                {
                    await ExecuteSqlOnAssignedServerAsync(cmd, CommandTarget.Code);
                }

                TemporaryTables.Add(zonedeftable, zonedeftable);

                LogOperation(LogMessages.ZoneDefTableCreated, step.StepNumber, this.ID, w.Elapsed.TotalSeconds);
            }
        }

        #endregion
        #region Link table functions

        public async Task CreateLinkTableAsync(XMatchQueryStep step)
        {
            if (step.StepNumber > 0)
            {
                var w = System.Diagnostics.Stopwatch.StartNew();

                int res;
                var zonedeftable = CodeGenerator.GetZoneDefTable(step);
                var linktable = CodeGenerator.GetLinkTable(step);

                // Drop table if it exists (unlikely, but might happen during debugging)
                linktable.Drop();

                using (var cmd = CodeGenerator.GetCreateLinkTableCommand(step, linktable))
                {
                    await ExecuteSqlOnAssignedServerAsync(cmd, CommandTarget.Temp);
                }

                using (var cmd = CodeGenerator.GetPopulateLinkTableCommand(step, zonedeftable, linktable))
                {
                    res = await ExecuteSqlOnAssignedServerAsync(cmd, CommandTarget.Code);
                }

                TemporaryTables.Add(linktable, linktable);

                LogOperation(LogMessages.LinkTableCreated, step.StepNumber, this.ID, w.Elapsed.TotalSeconds, res);
            }
        }

        #endregion
        #region Zone table functions

        public void PrepareCreateZoneTable(XMatchQueryStep step, out Graywulf.Sql.Schema.Table zoneTable, out SqlCommand createZoneTableCommand, out SqlCommand populateZoneTableCommand)
        {
            var table = Query.XMatchTables[step.XMatchTable];

            if (CodeGenerator.IsZoneTableNecessary(table, out CreateZoneTableReason reason))
            {
                zoneTable = CodeGenerator.GetZoneTable(step);
                createZoneTableCommand = CodeGenerator.GetCreateZoneTableCommand(step, zoneTable);
                populateZoneTableCommand = CodeGenerator.GetPopulateZoneTableCommand(step, zoneTable);
            }
            else
            {
                zoneTable = null;
                createZoneTableCommand = null;
                populateZoneTableCommand = null;
            }
        }

        /// <summary>
        /// Creates a zone table without populating it.
        /// </summary>
        /// <param name="step">XMatch query step.</param>
        /// <remarks>
        /// If <see cref="fromMatchTable"/> is true, zone table created
        /// according to a match table, otherwise for a source table.
        /// This is a slow operation and cannot be called inside a
        /// Graywulf context.
        /// Call <see cref="PrepareCreateZoneTable"/> before this.
        /// </remarks>
        public async Task CreateZoneTableAsync(XMatchQueryStep step, Graywulf.Sql.Schema.Table zoneTable, SqlCommand createZoneTableCommand, SqlCommand populateZoneTableCommand)
        {
            if (zoneTable != null)
            {
                var w = System.Diagnostics.Stopwatch.StartNew();
                int res;

                // Drop table if it exists (unlikely, but might happen during debugging)
                zoneTable.Drop();

                TemporaryTables.Add(zoneTable, zoneTable);

                using (createZoneTableCommand)
                {
                    await ExecuteSqlOnAssignedServerAsync(createZoneTableCommand, CommandTarget.Temp);
                }

                using (populateZoneTableCommand)
                {
                    res = await ExecuteSqlOnAssignedServerAsync(populateZoneTableCommand, CommandTarget.Code);
                }

                LogOperation(LogMessages.ZoneTableCreated, step.StepNumber, this.ID, w.Elapsed.TotalSeconds, Query.XMatchTables[step.XMatchTable].TableReference.DatabaseObject.FullyResolvedName, res);
            }
        }

        #endregion
        #region Pair table functions

        public void PrepareCreatePairTable(XMatchQueryStep step, out Table linkTable, out Table pairTable, out SqlCommand createPairTableCommand)
        {
            if (step.StepNumber > 0)
            {
                linkTable = CodeGenerator.GetLinkTable(step);
                pairTable = CodeGenerator.GetPairTable(step);
                createPairTableCommand = CodeGenerator.GetPopulatePairTableCommand(step, linkTable, pairTable);
            }
            else
            {
                linkTable = null;
                pairTable = null;
                createPairTableCommand = null;
            }
        }

        /// <summary>
        /// Creates a pair table from a link table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        /// <remarks>
        /// Call <see cref="PrepareCreatePairTable"/> before calling this function.
        /// Function calls <see cref="PopulatePairTable"/> to populate
        /// pair table form a link table.
        /// </remarks>
        public async Task CreatePairTableAsync(XMatchQueryStep step, Table linkTable, Table pairTable, SqlCommand createPairTableCommand)
        {
            if (step.StepNumber > 0)
            {
                var w = System.Diagnostics.Stopwatch.StartNew();
                int res;

                // Drop table if it exists (unlikely, but might happen during debugging)
                pairTable.Drop();

                using (createPairTableCommand)
                {
                    res = await ExecuteSqlOnAssignedServerAsync(createPairTableCommand, CommandTarget.Code);
                }

                TemporaryTables.Add(pairTable, pairTable);

                LogOperation(LogMessages.PairTableCreated, step.StepNumber, this.ID, w.Elapsed.TotalSeconds, Query.XMatchTables[step.XMatchTable].TableReference.DatabaseObject.FullyResolvedName, res);
            }
        }

        #endregion
        #region Match table functions

        public void PrepareCreateMatchTable(XMatchQueryStep step, out Table pairTable, out Table matchTable, out SqlCommand createMatchTableCommand, out SqlCommand populateMatchTableCommand, out SqlCommand buildMatchTableIndexCommand)
        {
            if (step.StepNumber > 0)
            {
                pairTable = CodeGenerator.GetPairTable(step);
                matchTable = CodeGenerator.GetMatchTable(step);

                createMatchTableCommand = CodeGenerator.GetCreateMatchTableCommand(step, matchTable);
                populateMatchTableCommand = CodeGenerator.GetPopulateMatchTableCommand(step, pairTable, matchTable);
                buildMatchTableIndexCommand = CodeGenerator.GetBuildMatchTableIndexCommand(step, matchTable);
            }
            else
            {
                pairTable = null;
                matchTable = null;

                createMatchTableCommand = null;
                populateMatchTableCommand = null;
                buildMatchTableIndexCommand = null;
            }
        }

        /// <summary>
        /// Creates a match table from a pair table or, in the first
        /// iteration, created a view over an existing zone table to
        /// look like as a match table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step</param>
        /// <remarks>
        /// Call <see cref="PrepareCreateMatchTable"/> before this function.
        /// This function calls <see cref="CreateMatchTableSpecific"/> and
        /// <see cref="PopulateMatchTable"/> that must be implemented in
        /// derived classes.
        /// 
        /// 08/27/2010: Modified not to create a view in the 0th iteration as
        ///             the non-indexes view caused wrong query plan (non-parallel)
        /// </remarks>
        public async Task CreateMatchTableAsync(XMatchQueryStep step, Table pairTable, Table matchTable, SqlCommand createMatchTableCommand, SqlCommand populateMatchTableCommand, SqlCommand buildMatchTableIndexCommand)
        {
            if (step.StepNumber > 0)
            {
                var w = System.Diagnostics.Stopwatch.StartNew();
                int res;

                // Drop table if it exists (unlikely, but might happen during debugging)
                matchTable.Drop();

                TemporaryTables.Add(matchTable, matchTable);

                using (createMatchTableCommand)
                {
                    await ExecuteSqlOnAssignedServerAsync(createMatchTableCommand, CommandTarget.Temp);
                }

                using (populateMatchTableCommand)
                {
                    res = await ExecuteSqlOnAssignedServerAsync(populateMatchTableCommand, CommandTarget.Code);
                }

                using (buildMatchTableIndexCommand)
                {
                    await ExecuteSqlOnAssignedServerAsync(buildMatchTableIndexCommand, CommandTarget.Temp);
                }

                LogOperation(LogMessages.MatchTableCreated, step.StepNumber, this.ID, w.Elapsed.TotalSeconds, Query.XMatchTables[step.XMatchTable].TableReference.DatabaseObject.FullyResolvedName, res);
            }
        }

        /// <summary>
        /// Drops a temporary match table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        public void DropMatchTable(XMatchQueryStep step)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Final query execution

        public override void PrepareExecuteQuery(out SourceQuery sourceQuery, out DestinationTable destinationTable)
        {
            base.PrepareExecuteQuery(out sourceQuery, out destinationTable);

            // In contrast to simple queries where PK is not necessarily important, here
            // make sure a PK is created on the output
            destinationTable.Options |= TableInitializationOptions.CreatePrimaryKey;
        }

        #endregion
    }
}
