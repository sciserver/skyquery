using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.Install;

namespace Jhu.SkyQuery.Install
{
    public class SkyQueryInstaller : FederationInstaller
    {
        public SkyQueryInstaller(Domain domain)
            : base(domain)
        {
        }

        public Federation Install()
        {
            return base.Install("SkyQuery");
        }

        public override void GenerateDefaultSettings()
        {
            Federation.System = true;

            Federation.SchemaManager = GetUnversionedTypeName(typeof(Jhu.Graywulf.Schema.GraywulfSchemaManager));
            Federation.UserDatabaseFactory = GetUnversionedTypeName(typeof(Jhu.Graywulf.CasJobs.CasJobsUserDatabaseFactory));
            Federation.QueryFactory = GetUnversionedTypeName(typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryFactory));
            Federation.FileFormatFactory = GetUnversionedTypeName(typeof(Jhu.SkyQuery.Format.SkyQueryFileFormatFactory));
            Federation.StreamFactory = GetUnversionedTypeName(typeof(Jhu.SkyQuery.IO.SkyQueryStreamFactory));

            Federation.ShortTitle = "SkyQuery";
            Federation.LongTitle = "SkyQuery Astronomical Cross-Match Engine";
            Federation.Copyright = Copyright.InfoCopyright;
            Federation.Disclaimer = Domain.Disclaimer;
            Federation.Email = Domain.Email;
        }

        protected override void GenerateQueryJobs()
        {
            var jdi = new XMatchQueryJobInstaller(Federation);
            jdi.Install();
        }

#if false   // TODO: old code, delete

            // ---
            var ef = new EntityFactory(Context);

            var clusterName = domain.Cluster.Name;
            var domainName = domain.Name;

            // Load default settings
            // --- Temp database version
            var tempDatabaseVersion = ef.LoadEntity<DatabaseVersion>(clusterName, Constants.TempDbName, Constants.TempDbName);

            // --- Controller machine
            var controllerMachine = ef.LoadEntity<Machine>(clusterName, Constants.ControllerMachineRoleName, Constants.ControllerMachineName);

            // --- Schema source server
            var schemaSourceServerInstance = ef.LoadEntity<ServerInstance>(clusterName, Constants.ControllerMachineRoleName, Constants.ControllerMachineName, Constants.ServerInstanceName);

            var federation = new Federation(domain)
            {
                QueryFactory = GetUnversionedTypeName(typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryFactory)),
                FileFormatFactory = GetUnversionedTypeName(typeof(Jhu.SkyQuery.Format.SkyQueryFileFormatFactory)),
                StreamFactory = GetUnversionedTypeName(typeof(Jhu.SkyQuery.IO.SkyQueryStreamFactory)),
                Name = "SkyQuery",
                System = true,
                ShortTitle = "SkyQuery",
                LongTitle = "SkyQuery Astronomical Cross-Match Engine",
                Email = "admin@skyquery.net",
                Copyright = Copyright.InfoCopyright,
                TempDatabaseVersion = tempDatabaseVersion,
                ControllerMachine = controllerMachine,
                SchemaSourceServerInstance = schemaSourceServerInstance,

                //CodeDatabaseVersion
                //Disclaimer
                //MyDBDatabaseVersion
                
            };
            federation.Save();

            var fi = new FederationInstaller(federation);
            fi.GenerateDefaultChildren(myDBServerVersion, codeDatabaseServerVersion);

            // XMatch job
            var jd = new JobDefinition(federation)
            {
                Name = typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryJob).Name,
                System = true,
                WorkflowTypeName = GetUnversionedTypeName(typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryJob)),
                Settings = new SqlQueryJobSettings()
                {
                    HotDatabaseVersionName = Constants.HotDatabaseVersionName,
                    StatDatabaseVersionName = Constants.StatDatabaseVersionName,
                    DefaultDatasetName = Constants.UserDbName,
                    DefaultSchemaName = Constants.DefaultSchemaName,
                    QueryTimeout = 7200,
                },
            };
            jd.DiscoverWorkflowParameters();
            jd.Save();

            var codedd = new DatabaseDefinition(federation)
            {
                Name = Constants.CodeDbName,
                System = true,
                DatabaseInstanceNamePattern = Constants.CodeDbInstanceNamePattern,
                DatabaseNamePattern = Constants.CodeDbNamePattern,
                LayoutType = DatabaseLayoutType.Monolithic,
            };
            codedd.Save();

            var codeddi = new DatabaseDefinitionInstaller(codedd);
            codeddi.GenerateDefaultChildren(codeDatabaseServerVersion, Constants.CodeDbName);

            codedd.LoadDatabaseVersions(true);
            federation.CodeDatabaseVersion = codedd.DatabaseVersions[Constants.CodeDbName];

            federation.Save();
        }

#endif
    }
}