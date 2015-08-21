using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.IO;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.SqlCodeGen.SqlServer;
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

        public void ComputeMinMaxError(XMatchQueryStep step)
        {
            double min, max;

            using (var cmd = CodeGenerator.GetComputeMinMaxErrorCommand(step))
            {
                ExecuteSqlCommandReader(cmd, CommandTarget.Temp, dr =>
                    {
                        if (dr.Read())
                        {
                            min = dr.GetDouble(0);
                            max = dr.GetDouble(1);
                        }
                    });
            }

            // TODO: propagate these results to the table
        }

        public abstract void ComputeSearchRadius(XMatchQueryStep step);

        #endregion
        #region ZoneDef table function

        public void CreateZoneDefTable(XMatchQueryStep step)
        {
            // This is done only for the second catalog and on
            if (step.StepNumber > 0)
            {
                var zonedeftable = CodeGenerator.GetZoneDefTable(step);

                // Drop table if it exists (unlikely, but might happen during debugging)
                zonedeftable.Drop();

                using (var cmd = CodeGenerator.GetCreateZoneDefTableCommand(step, zonedeftable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Temp);
                }

                using (var cmd = CodeGenerator.GetPopulateZoneDefTableCommand(step, zonedeftable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Code);
                }

                TemporaryTables.TryAdd(zonedeftable.TableName, zonedeftable);
            }
        }

        #endregion
        #region Link table functions

        public void CreateLinkTable(XMatchQueryStep step)
        {
            if (step.StepNumber != 0)
            {
                var zonedeftable = CodeGenerator.GetZoneDefTable(step);
                var linktable = CodeGenerator.GetLinkTable(step);

                // Drop table if it exists (unlikely, but might happen during debugging)
                linktable.Drop();

                using (var cmd = CodeGenerator.GetCreateLinkTableCommand(step, linktable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Temp);
                }

                using (var cmd = CodeGenerator.GetPopulateLinkTableCommand(step, zonedeftable, linktable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Code);
                }

                TemporaryTables.TryAdd(linktable.TableName, linktable);
            }
        }

        #endregion
        #region Zone table functions

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
        public void CreateZoneTable(XMatchQueryStep step)
        {
            var table = Query.XMatchTables[step.XMatchTable];

            if (table.IsZoneTableNecessary)
            {
                var zonetable = CodeGenerator.GetZoneTable(step);

                // Drop table if it exists (unlikely, but might happen during debugging)
                zonetable.Drop();

                using (var cmd = CodeGenerator.GetCreateZoneTableCommand(step, zonetable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Temp);
                }

                using (var cmd = CodeGenerator.GetPopulateZoneTableCommand(step, zonetable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Code);
                }

                TemporaryTables.TryAdd(zonetable.TableName, zonetable);
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
        public void CreatePairTable(XMatchQueryStep step)
        {
            if (step.StepNumber != 0)
            {
                var table = Query.XMatchTables[step.XMatchTable];
                var pairtable = CodeGenerator.GetPairTable(step);

                // Drop table if it exists (unlikely, but might happen during debugging)
                pairtable.Drop();

                using (var cmd = CodeGenerator.GetCreatePairTableCommand(step, table, pairtable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Temp);
                }

                using (var cmd = CodeGenerator.GetPopulatePairTableCommand(step, table, pairtable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Code);
                }

                TemporaryTables.TryAdd(pairtable.TableName, pairtable);
            }
        }

        #endregion
        #region Match table functions

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
        public void CreateMatchTable(XMatchQueryStep step)
        {
            if (step.StepNumber > 0)
            {
                // Create real match tables
                var matchtable = CodeGenerator.GetMatchTable(step);

                // Drop table if it exists (unlikely, but might happen during debugging)
                matchtable.Drop();

                using (var cmd = CodeGenerator.GetCreateMatchTableCommand(step, matchtable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Temp);
                }

                using (var cmd = CodeGenerator.GetPopulateMatchTableCommand(step, matchtable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Code);
                }

                using (var cmd = CodeGenerator.GetBuildMatchTableIndexCommand(step, matchtable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Temp);
                }

                TemporaryTables.TryAdd(matchtable.TableName, matchtable);
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



        private double GetBufferZoneSize(XMatchQueryStep step)
        {
            return 2 * step.SearchRadius;
        }
    }
}
