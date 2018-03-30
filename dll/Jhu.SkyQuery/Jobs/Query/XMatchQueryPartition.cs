using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.SkyQuery.Parser;

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

        public abstract void PrepareComputeSearchRadius(XMatchQueryStep step, out SqlCommand computeSearchRadiusCommand);

        public abstract Task ComputeSearchRadiusAsync(XMatchQueryStep step, SqlCommand computeSearchRadiusCommand);

        #endregion
        #region ZoneDef table function

        public async Task CreateZoneDefTableAsync(XMatchQueryStep step)
        {
            if (step.StepNumber > 0)
            {
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

                TemporaryTables.TryAdd(zonedeftable.TableName, zonedeftable);
            }
        }

        #endregion
        #region Link table functions

        public async Task CreateLinkTableAsync(XMatchQueryStep step)
        {
            if (step.StepNumber > 0)
            {
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
                    await ExecuteSqlOnAssignedServerAsync(cmd, CommandTarget.Code);
                }

                TemporaryTables.TryAdd(linktable.TableName, linktable);
            }
        }

        #endregion
        #region Zone table functions

        public void PrepareCreateZoneTable(XMatchQueryStep step, out Graywulf.Sql.Schema.Table zoneTable, out SqlCommand createZoneTableCommand, out SqlCommand populateZoneTableCommand)
        {
            var table = Query.XMatchTables[step.XMatchTable];

            if (CodeGenerator.IsZoneTableNecessary(table))
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
                // Drop table if it exists (unlikely, but might happen during debugging)
                zoneTable.Drop();

                TemporaryTables.TryAdd(zoneTable.TableName, zoneTable);

                using (createZoneTableCommand)
                {
                    await ExecuteSqlOnAssignedServerAsync(createZoneTableCommand, CommandTarget.Temp);
                }

                using (populateZoneTableCommand)
                {
                    await ExecuteSqlOnAssignedServerAsync(populateZoneTableCommand, CommandTarget.Code);
                }
            }
        }

        #endregion
        #region Pair table functions

        /// <summary>
        /// Creates a pair table from a link table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        /// <remarks>
        /// Call <see cref="PrepareCreatePairTable"/> before calling this function.
        /// Function calls <see cref="PopulatePairTable"/> to populate
        /// pair table form a link table.
        /// </remarks>
        public async Task CreatePairTableAsync(XMatchQueryStep step)
        {
            if (step.StepNumber > 0)
            {
                var table = Query.XMatchTables[step.XMatchTable];
                var linktable = CodeGenerator.GetLinkTable(step);
                var pairtable = CodeGenerator.GetPairTable(step);

                // Drop table if it exists (unlikely, but might happen during debugging)
                pairtable.Drop();

                using (var cmd = CodeGenerator.GetPopulatePairTableCommand(step, linktable, pairtable))
                {
                    await ExecuteSqlOnAssignedServerAsync(cmd, CommandTarget.Code);
                }

                TemporaryTables.TryAdd(pairtable.TableName, pairtable);
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
                // Drop table if it exists (unlikely, but might happen during debugging)
                matchTable.Drop();

                TemporaryTables.TryAdd(matchTable.TableName, matchTable);

                using (createMatchTableCommand)
                {
                    await ExecuteSqlOnAssignedServerAsync(createMatchTableCommand, CommandTarget.Temp);
                }

                using (populateMatchTableCommand)
                {
                    await ExecuteSqlOnAssignedServerAsync(populateMatchTableCommand, CommandTarget.Code);
                }

                using (buildMatchTableIndexCommand)
                {
                    await ExecuteSqlOnAssignedServerAsync(buildMatchTableIndexCommand, CommandTarget.Temp);
                }
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
    }
}
