using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Activities;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.SqlCodeGen.SqlServer;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Parser;


namespace Jhu.SkyQuery.Jobs.Query
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

        protected override SqlServerCodeGenerator CreateCodeGenerator()
        {
            return new BayesFactorXMatchQueryCodeGenerator(this);
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
            System.Diagnostics.Debug.Assert(steps.Count == 0);

            int i = 0;
            foreach (var t in tables)
            {
                var s = new BayesFactorXMatchQueryStep(this, Context);

                s.StepNumber = i;
                s.XMatchTable = t.TableReference.UniqueName;

                steps.Add(s);

                i++;
            }
        }

        #endregion
        #region Compute search radius

        /// <summary>
        /// Calculates the search radius by taking the constant error value or error limits of each
        /// catalog into account.
        /// </summary>
        /// <param name="step"></param>
        public override void ComputeSearchRadius(XMatchQueryStep step)
        {
            if (step.StepNumber > 0)
            {
                // Calculate search radius from the first sourca table our the output of
                // the previous match
                var pstep = Steps[step.StepNumber - 1];
                
                using (var cmd = CodeGenerator.GetComputeSearchRadiusCommand(pstep))
                {
                    var res = ExecuteSqlOnAssignedServerScalar(cmd, CommandTarget.Code);
                    var theta = res != DBNull.Value ? (double)res : 0;
                    step.SearchRadius = Math.Sqrt(theta) * 180.0 / Math.PI;
                }
            }
        }

        #endregion
    }
}
