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

        /// <summary>
        /// When overriden in derived classes, initializes the
        /// <see cref="Steps"/> collection.
        /// </summary>
        /// <param name="tables"></param>
        public abstract void GenerateSteps(XMatchTableSpecification[] tables);

        #region ZoneDef table function

        public void CreateZoneDefTable(XMatchQueryStep step)
        {
            // This is done only for the second catalog and on
            if (step.StepNumber > 0)
            {
                var zonedeftable = CodeGenerator.GetZoneDefTable(step.StepNumber);

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
            // TODO: modify this to create a zone table only if the 
            // source catalog doesn't have a zone index or it has a strong
            // filter (i.e. small region) defined in the query

            // Create zone table from match table
            if (step.StepNumber > 0)
            {
                var table = Query.XMatchTables[step.XMatchTable];
                var zonetable = CodeGenerator.GetZoneTable(table.TableReference);
                var zonedeftable = CodeGenerator.GetZoneDefTable(step.StepNumber);

                // Drop table if it exists (unlikely, but might happen during debugging)
                zonetable.Drop();

                using (var cmd = CodeGenerator.GetCreateZoneTableCommand(table, zonetable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Temp);
                }

                using (var cmd = CodeGenerator.GetPopulateZoneTableCommand(table, zonedeftable, zonetable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Code);
                }

                TemporaryTables.TryAdd(zonetable.TableName, zonetable);
            }
        }

        /// <summary>
        /// Drops a temporary zone table.
        /// </summary>
        /// <param name="step">Number of XMatch step.</param>
        public void DropZoneTable(XMatchQueryStep step)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Link table functions


        public void CreateLinkTable(XMatchQueryStep step)
        {
            if (step.StepNumber != 0)
            {
                var linktable = CodeGenerator.GetLinkTable(step.StepNumber);

                // Drop table if it exists (unlikely, but might happen during debugging)
                linktable.Drop();

                using (var cmd = CodeGenerator.GetCreateLinkTableCommand(linktable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Temp);
                }

                using (var cmd = CodeGenerator.GetPopulateLinkTableCommand(/* TODO */))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Code);
                }

                TemporaryTables.TryAdd(linktable.TableName, linktable);
            }
        }

        /// <summary>
        /// Drops a link table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        public void DropLinkTable(XMatchQueryStep step)
        {
            throw new NotImplementedException();
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
                var pairtable = CodeGenerator.GetPairTable(step.StepNumber);

                // Drop table if it exists (unlikely, but might happen during debugging)
                pairtable.Drop();

                using (var cmd = CodeGenerator.GetCreatePairTableCommand(table, step.StepNumber, pairtable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Temp);
                }

                using (var cmd = CodeGenerator.GetPopulatePairTableCommand(pairtable))
                {
                    ExecuteSqlCommand(cmd, CommandTarget.Code);
                }

                TemporaryTables.TryAdd(pairtable.TableName, pairtable);
            }
        }

        /// <summary>
        /// Drops a pair table.
        /// </summary>
        /// <param name="step"></param>
        public void DropPairTable(XMatchQueryStep step)
        {
            throw new NotImplementedException();
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
            // Create real match tables
            var matchtable = CodeGenerator.GetMatchTable(step.StepNumber);

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

            using (var cmd = CodeGenerator.GetBuildInitialMatchTableIndexCommand(step, matchtable))
            {
                ExecuteSqlCommand(cmd, CommandTarget.Temp);
            }

            TemporaryTables.TryAdd(matchtable.TableName, matchtable);
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

        protected double GetWeight(double sigma)
        {
            return 1 / Math.Pow(sigma / 3600 / 180 * Math.PI, 2);
        }

        private double GetBufferZoneSize(XMatchQueryStep step)
        {
            return 2 * step.SearchRadius;
        }
    }
}
