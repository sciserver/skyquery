using System;
using System.Runtime.Serialization;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.Tasks;
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

            // TODO: do we need the cancellation context here?
            using (var cancellationContext = new CancellationContext())
            {
                if (root is XMatchSelectStatement)
                {
                    res = new BayesFactorXMatchQuery(cancellationContext, RegistryContext);
                }
                else if (root is RegionSelectStatement)
                {
                    res = new RegionQuery(cancellationContext, RegistryContext);
                }
                else if (root is SelectStatement)
                {
                    res = new SqlQuery(cancellationContext, RegistryContext);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return res;
        }

        public override Jhu.Graywulf.Parsing.Parser CreateParser()
        {
            return new SkyQueryParser();
        }

        public override Graywulf.Sql.Validation.SqlValidator CreateValidator()
        {
            return new SkyQueryValidator();
        }

        public override Graywulf.Sql.NameResolution.SqlNameResolver CreateNameResolver()
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

                job.Parameters[Jhu.Graywulf.Registry.Constants.JobParameterParameters].Value = query.Parameters;

                return job;
            }
        }
    }
}
