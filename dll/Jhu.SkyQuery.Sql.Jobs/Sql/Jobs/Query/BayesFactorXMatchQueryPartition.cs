using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using Jhu.Graywulf.Sql.CodeGeneration.SqlServer;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.SkyQuery.Sql.Parsing;


namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    /// <summary>
    /// Implements functions to perform a bayesian cross-match operation
    /// over a partition of a table.
    /// </summary>
    [Serializable]
    public class BayesFactorXMatchQueryPartition : XMatchQueryPartition
    {
        #region Properties

        public new BayesFactorXMatchQuery Query
        {
            get { return (BayesFactorXMatchQuery)base.Query; }
        }

        private BayesFactorXMatchQueryCodeGenerator CodeGenerator
        {
            get { return (BayesFactorXMatchQueryCodeGenerator)CreateCodeGenerator(); }
        }

        #endregion
        #region Constructors and initializer functions

        public BayesFactorXMatchQueryPartition()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public BayesFactorXMatchQueryPartition(BayesFactorXMatchQueryPartition old)
            : base(old)
        {
            CopyMembers(old);
        }

        public BayesFactorXMatchQueryPartition(BayesFactorXMatchQuery query)
            : base(query)
        {
            InitializeMembers(new StreamingContext());
        }

        /// <summary>
        /// Initializes private member variables
        /// </summary>
        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
        }

        private void CopyMembers(BayesFactorXMatchQueryPartition old)
        {
        }

        public override object Clone()
        {
            return new BayesFactorXMatchQueryPartition(this);
        }

        #endregion

        protected override SqlQueryCodeGenerator CreateCodeGenerator()
        {
            return new BayesFactorXMatchQueryCodeGenerator(this)
            {
                TableNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                TableAliasRendering = Graywulf.Sql.CodeGeneration.AliasRendering.Default,
                ColumnNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                ColumnAliasRendering = Graywulf.Sql.CodeGeneration.AliasRendering.Always,
                DataTypeNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                FunctionNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified
            };
        }

        #region Step generation

        /// <summary>
        /// Generates cross-match steps.
        /// </summary>
        /// <param name="tables">Source tables for cross-matching</param>
        /// <remarks>
        /// This function generates n steps, where n is the number of the
        /// source tables. The first step will be slightly different, since
        /// we don't build the Match and Zone_Match tables, only views on
        /// the zone table of source table.
        /// </remarks>
        public override void GenerateSteps(IEnumerable<XMatchTableSpecification> tables)
        {
            int i = 0;
            foreach (var t in tables)
            {
                var s = new BayesFactorXMatchQueryStep(this, RegistryContext);

                s.StepNumber = i;
                s.XMatchTable = t.TableSource.UniqueKey;

                steps.Add(s);

                i++;
            }
        }

        #endregion
        #region Compute search radius

        public override void OnPrepareComputeSearchRadius(XMatchQueryStep step, out SqlCommand computeSearchRadiusCommand)
        {
            if (step.StepNumber > 0)
            {
                // Calculate search radius from the first source table our the output of
                // the previous match
                var pstep = Steps[step.StepNumber - 1];

                computeSearchRadiusCommand = CodeGenerator.GetComputeSearchRadiusCommand(pstep, step);
            }
            else
            {
                computeSearchRadiusCommand = null;
            }
        }

        /// <summary>
        /// Calculates the search radius by taking the constant error value or error limits of each
        /// catalog into account.
        /// </summary>
        /// <param name="step"></param>
        public override async Task OnComputeSearchRadiusAsync(XMatchQueryStep step, SqlCommand computeSearchRadiusCommand)
        {
            // Calculate search radius from the first sourca table our the output of
            // the previous match
            var pstep = Steps[step.StepNumber - 1];

            using (computeSearchRadiusCommand)
            {
                var res = await ExecuteSqlOnAssignedServerScalarAsync(computeSearchRadiusCommand, CommandTarget.Code);
                var theta = res != DBNull.Value ? (double)res : 0;
                var radius = Math.Sqrt(theta) * 180.0 / Math.PI;       // radius in degrees

                if (radius / Query.ZoneHeight > 100)
                {
                    throw Error.SearchRadiusTooLarge(radius);
                }

                step.SearchRadius = radius;
            }
        }

        #endregion
    }
}
