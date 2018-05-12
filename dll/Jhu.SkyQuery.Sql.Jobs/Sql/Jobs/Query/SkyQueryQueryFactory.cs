using System;
using System.Runtime.Serialization;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.Tasks;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    [Serializable]
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
                typeof(BayesFactorXMatchQuery),
                typeof(ConeXMatchQuery)
            };
        }

        protected override SqlQuery OnCreateQuery(Graywulf.Sql.Parsing.StatementBlock parsingTree)
        {
            StatementType type = StatementType.Unknown;
            RegionSelectStatement region = null;
            XMatchSelectStatement xmatch = null;
            int count = 0;

            foreach (var s in parsingTree.EnumerateDescendantsRecursive<IStatement>())
            {
                if (s.StatementType != StatementType.Block)
                {
                    type |= s.StatementType;
                    count++;

                    switch (s)
                    {
                        case XMatchSelectStatement xm:
                            if (xmatch != null)
                            {
                                throw Error.XMatchMultipleStatementsNotSupported();
                            }
                            else
                            {
                                xmatch = xm;
                            }
                            break;
                        case RegionSelectStatement rs:
                            if (region == null)
                            {
                                region = rs;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            // TODO: do we need the cancellation context here?
            using (var cancellationContext = new CancellationContext())
            {
                if (xmatch != null)
                {
                    var xmts = xmatch.FindDescendantRecursive<XMatchTableSource>();

                    switch (xmts.Algorithm.ToUpperInvariant())
                    {
                        case SkyQuery.Sql.Parsing.Constants.AlgorithmBayesFactor:
                            return new BayesFactorXMatchQuery(cancellationContext, RegistryContext);
                        case SkyQuery.Sql.Parsing.Constants.AlgorithmCone:
                            return new ConeXMatchQuery(cancellationContext, RegistryContext);
                        default:
                            throw new NotImplementedException();
                    }
                }
                else if (region != null)
                {
                    return new RegionQuery(cancellationContext, RegistryContext);
                }
                else
                {
                    return new SqlQuery(cancellationContext, RegistryContext);
                }
            }
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
