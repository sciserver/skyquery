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
    public class XMatchQueryFactory : SqlQueryFactory
    {
        public XMatchQueryFactory()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public XMatchQueryFactory(Context context)
            : base(context)
        {
            InitializeMembers(new StreamingContext());
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
        }

        protected override Type[] LoadQueryTypes()
        {
            return new Type[] { 
                typeof(SqlQuery), 
                typeof(BayesFactorXMatchQuery) 
            };
        }

        protected override QueryBase CreateQueryBase(Node root)
        {
            QueryBase res;
            if (root is XMatchSelectStatement)
            {
                res = new BayesFactorXMatchQuery(Context);
            }
            else if (root is SelectStatement)
            {
                res = new SqlQuery(Context);
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

        public override JobInstance ScheduleAsJob(string jobName, QueryBase query, string queueName, string comments)
        {
            if (!(query is XMatchQuery))
            {
                return base.ScheduleAsJob(jobName, query, queueName, comments);
            }
            else
            {
                var ef = new EntityFactory(Context);

                var job = CreateJobInstance(
                    jobName,
                    EntityFactory.CombineName(EntityType.JobDefinition, Jhu.Graywulf.Registry.AppSettings.FederationName, typeof(XMatchQueryJob).Name),
                    queueName,
                    comments);

                job.Parameters[Jhu.Graywulf.Jobs.Constants.JobParameterQuery].Value = query;

                return job;
            }
        }
    }
}
