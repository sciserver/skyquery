using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Schema.SqlServer;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.SkyQuery.Sql.Parsing;
using Jhu.Graywulf.Sql.NameResolution;

namespace Jhu.SkyQuery.Sql
{
    public class SkyQueryTestBase : Jhu.Graywulf.Sql.SqlServerTestBase
    {
        private RegistryContext registryContext;
        private FederationContext federationContext;

        protected SqlServerDataset CodeDataset
        {
            get
            {
                return new SqlServerDataset("CODE", "Initial Catalog=SkyQuery_CODE");
            }
        }

        public User RegistryUser
        {
            get
            {
                return null;
            }
        }

        public RegistryContext RegistryContext
        {
            get
            {
                if (registryContext == null)
                {
                    registryContext = ContextManager.Instance.CreateReadOnlyContext();
                    registryContext.ClusterReference.Name = ContextManager.Configuration.ClusterName;
                    registryContext.DomainReference.Name = ContextManager.Configuration.DomainName;
                    registryContext.FederationReference.Name = ContextManager.Configuration.FederationName;
                }

                return registryContext;
            }
        }

        public FederationContext FederationContext
        {
            get
            {
                if (federationContext == null)
                {
                    federationContext = new FederationContext(RegistryContext, RegistryUser);
                }

                return federationContext;
            }
        }

        /*
        protected override UserDatabaseFactory CreateUserDatabaseFactory(FederationContext context)
        {
            return UserDatabaseFactory.Create(
                typeof(Jhu.Graywulf.CasJobs.CasJobsUserDatabaseFactory).AssemblyQualifiedName,
                context);
        }

        protected override QueryFactory CreateQueryFactory(RegistryContext context)
        {
            return QueryFactory.Create(
                typeof(SkyQueryQueryFactory).AssemblyQualifiedName,
                context);
        }
        */

        protected override SchemaManager CreateSchemaManager()
        {
            return new GraywulfSchemaManager(FederationContext);
        }

        protected override SqlParser CreateParser()
        {
            return new SkyQueryParser();
        }

        protected override SqlNameResolver CreateNameResolver()
        {
            return new Jhu.SkyQuery.Sql.NameResolution.SkyQuerySqlNameResolver()
            {
                Options = new NameResolution.SkyQuerySqlNameResolverOptions()
                {
                    DefaultTableDatasetName = Jhu.Graywulf.Test.Constants.TestDatasetName,
                    DefaultDataTypeDatasetName = Jhu.Graywulf.Test.Constants.TestDatasetName,
                    DefaultFunctionDatasetName = Jhu.Graywulf.Test.Constants.TestDatasetName,
                    DefaultOutputDatasetName = Jhu.Graywulf.Test.Constants.TestDatasetName,
                },
                SchemaManager = CreateSchemaManager()
            };
        }

        /*
        protected void FinishQueryJob(Guid guid)
        {
            FinishQueryJob(guid, new TimeSpan(0, 5, 0));
        }

        protected void FinishQueryJob(Guid guid, TimeSpan timeout)
        {
            WaitJobComplete(guid, TimeSpan.FromSeconds(10), timeout);

            var ji = LoadJob(guid);

            if (ji.JobExecutionStatus == JobExecutionState.Failed)
            {
                throw new Exception(ji.ExceptionMessage);
            }

            Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
        }*/
    }
}
