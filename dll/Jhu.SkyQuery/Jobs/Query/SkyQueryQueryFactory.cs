using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    // TODO: Rename to SkyQueryQueryFactory once build is stable enough for
    // testing so that config can be safely changed.
    public class SkyQueryQueryFactory : SqlQueryFactory
    {
        #region Constructors and initializers

        public SkyQueryQueryFactory()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public SkyQueryQueryFactory(RegistryContext context)
            : base(context)
        {
            InitializeMembers(new StreamingContext());
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
        }

        #endregion

        protected override Type[] LoadQueryTypes()
        {
            return new Type[] { 
                typeof(SqlQuery), 
                typeof(BayesFactorXMatchQuery) 
            };
        }

        protected override SqlQuery CreateQueryBase(Node root)
        {
            SqlQuery res;
            if (root is XMatchSelectStatement)
            {
                res = new BayesFactorXMatchQuery(RegistryContext);
            }
            else if (root is RegionSelectStatement)
            {
                res = new RegionQuery(RegistryContext);
            }
            else if (root is SelectStatement)
            {
                res = new SqlQuery(RegistryContext);
            }
            else
            {
                throw new NotImplementedException();
            }

            return res;
        }

        public override Jhu.Graywulf.ParserLib.Parser CreateParser()
        {
            return new SkyQueryParser();
        }

        public override Graywulf.SqlParser.SqlValidator CreateValidator()
        {
            return new SkyQueryValidator();
        }

        public override Graywulf.SqlParser.SqlNameResolver CreateNameResolver()
        {
            return new SkyQueryNameResolver();
        }

        public override JobInstance ScheduleAsJob(string jobName, SqlQuery query, string queueName, TimeSpan timeout, string comments)
        {
            if (!(query is XMatchQuery))
            {
                return base.ScheduleAsJob(jobName, query, queueName, timeout, comments);
            }
            else
            {
                var ef = new EntityFactory(RegistryContext);

                var job = CreateJobInstance(
                    jobName,
                    EntityFactory.CombineName(EntityType.JobDefinition, Jhu.Graywulf.Registry.ContextManager.Configuration.FederationName, typeof(XMatchQueryJob).Name),
                    queueName,
                    timeout,
                    comments);

                job.Parameters[Jhu.Graywulf.Jobs.Constants.JobParameterQuery].Value = query;

                return job;
            }
        }
    }
}
