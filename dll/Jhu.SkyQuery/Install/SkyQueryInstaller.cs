using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.Install;

namespace Jhu.SkyQuery.Install
{
    public class SkyQueryInstaller : ContextObject
    {
        public SkyQueryInstaller(Context context)
            : base(context)
        {
        }

        public void Install(Domain domain, ServerVersion myDBServerVersion, ServerVersion codeDatabaseServerVersion)
        {
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
                Name = "SkyQuery",
                System = true,
                ShortTitle = "SkyQuery",
                LongTitle = "SkyQuery Astronomical Cross-Match Engine",
                Email = "admin@skyquery.net",
                TempDatabaseVersion = tempDatabaseVersion,
                ControllerMachine = controllerMachine,
                SchemaSourceServerInstance = schemaSourceServerInstance,
            };
            federation.Save();

            var fi = new FederationInstaller(federation);
            fi.GenerateDefaultChildren(myDBServerVersion);

            // XMatch job
            var jd = new JobDefinition(federation)
            {
                Name = typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryJob).Name,
                System = true,
                WorkflowTypeName = typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryJob).AssemblyQualifiedName,
                Settings = Util.SaveSettings(new Dictionary<SqlQueryFactory.Settings, string>()
                {
                    // TODO: update these in the query factory
                    {SqlQueryFactory.Settings.HotDatabaseVersionName, Constants.HotDatabaseVersionName},
                    {SqlQueryFactory.Settings.StatDatabaseVersionName, Constants.StatDatabaseVersionName},
                    {SqlQueryFactory.Settings.DefaultSchemaName, Constants.DefaultSchemaName},
                    {SqlQueryFactory.Settings.DefaultDatasetName, Constants.MyDbName},
                    {SqlQueryFactory.Settings.DefaultTableName, "outputtable"},
                    {SqlQueryFactory.Settings.TemporarySchemaName, Constants.DefaultSchemaName},
                    {SqlQueryFactory.Settings.LongQueryTimeout, "7200"},
                }),
            };

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
    }
}