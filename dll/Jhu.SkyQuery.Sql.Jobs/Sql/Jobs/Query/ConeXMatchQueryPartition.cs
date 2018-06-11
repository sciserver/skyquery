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
    [Serializable]
    public class ConeXMatchQueryPartition : XMatchQueryPartition
    {
        #region Properties

        public new ConeXMatchQuery Query
        {
            get { return (ConeXMatchQuery)base.Query; }
        }

        private ConeXMatchQueryCodeGenerator CodeGenerator
        {
            get { return (ConeXMatchQueryCodeGenerator)CreateCodeGenerator(); }
        }

        #endregion
        #region Constructors and initializer functions

        public ConeXMatchQueryPartition()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public ConeXMatchQueryPartition(ConeXMatchQueryPartition old)
            : base(old)
        {
            CopyMembers(old);
        }

        public ConeXMatchQueryPartition(ConeXMatchQuery query)
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

        private void CopyMembers(ConeXMatchQueryPartition old)
        {
        }

        public override object Clone()
        {
            return new ConeXMatchQueryPartition(this);
        }

        #endregion

        protected override SqlQueryCodeGenerator CreateCodeGenerator()
        {
            return new ConeXMatchQueryCodeGenerator(this)
            {
                TableNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                TableAliasRendering = Graywulf.Sql.CodeGeneration.AliasRendering.Default,
                ColumnNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                ColumnAliasRendering = Graywulf.Sql.CodeGeneration.AliasRendering.Always,
                FunctionNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified
            };
        }

        public override void GenerateSteps(IEnumerable<XMatchTableSpecification> tables)
        {
            int i = 0;
            foreach (var t in tables)
            {
                var s = new ConeXMatchQueryStep(this, RegistryContext);

                s.StepNumber = i;
                s.XMatchTable = t.TableSource.UniqueKey;

                steps.Add(s);

                i++;
            }
        }

        public override void OnPrepareComputeSearchRadius(XMatchQueryStep step, out SqlCommand computeSearchRadiusCommand)
        {
            // Search radius is the diameter of the MEC of the largest region
            // TODO: generalize to non-circles
            // Compute search radius only once

            if (step.StepNumber == 0)
            {
                computeSearchRadiusCommand = CodeGenerator.GetComputeSearchRadiusCommand(step);
            }
            else
            {
                computeSearchRadiusCommand = null;
            }
        }

        public override async Task OnComputeSearchRadiusAsync(XMatchQueryStep step, SqlCommand computeSearchRadiusCommand)
        {
            using (computeSearchRadiusCommand)
            {
                var res = await ExecuteSqlOnAssignedServerScalarAsync(computeSearchRadiusCommand, CommandTarget.Code);
                var theta = res != DBNull.Value ? (double)res : 0;
                var radius = theta / 3600.0;      // radius in degrees 

                if (radius / Query.ZoneHeight > 100)
                {
                    throw Error.SearchRadiusTooLarge(radius);
                }

                // Cone search uses the same radius at every step
                foreach (var s in steps)
                {
                    s.SearchRadius = radius;
                }
            }
        }
    }
}
