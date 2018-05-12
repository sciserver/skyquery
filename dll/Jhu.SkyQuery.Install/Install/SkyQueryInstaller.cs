using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Sql.Jobs.Query;
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

        protected override void GenerateCluster()
        {
            base.GenerateCluster();

            // Create SkyNode and UserDBHost roles

            MachineRole skynodeMachineRole = new MachineRole(Cluster)
            {
                Name = Constants.SkyNodeMachineRoleName,
                MachineRoleType = MachineRoleType.MirroredSet
            };
            skynodeMachineRole.Save();

            ServerVersion skynodeServerVersion = new ServerVersion(skynodeMachineRole)
            {
                Name = Constants.SkyNodeServerVersionName,

            };
            skynodeServerVersion.Save();
        }

        protected override DatabaseVersion GetTempDatabaseVersion()
        {
            Cluster.LoadDomains(true);
            var shareddomain = Cluster.Domains[Jhu.Graywulf.Registry.Constants.SystemDomainName];

            shareddomain.LoadFederations(false);
            var sharedfederation = shareddomain.Federations[Jhu.Graywulf.Registry.Constants.SystemFederationName];

            sharedfederation.LoadDatabaseDefinitions(false);

            var tempdbdd = sharedfederation.DatabaseDefinitions[Jhu.Graywulf.Registry.Constants.TempDbName];
            tempdbdd.LoadDatabaseVersions(false);

            DatabaseVersion tempDatabaseVersion;
            var skynodeServerVersion = GetNodeServerVersion();

            if (!tempdbdd.DatabaseVersions.ContainsKey(Jhu.Graywulf.Registry.Constants.TempDbName))
            {
                var tempddi = new DatabaseDefinitionInstaller(tempdbdd);
                tempddi.GenerateDefaultChildren(skynodeServerVersion, Jhu.Graywulf.Registry.Constants.TempDbName);
                tempdbdd.LoadDatabaseVersions(true);
            }

            tempDatabaseVersion = tempdbdd.DatabaseVersions[Jhu.Graywulf.Registry.Constants.TempDbName];

            tempDatabaseVersion.ServerVersion = skynodeServerVersion;
            tempDatabaseVersion.Save();

            return tempDatabaseVersion;
        }

        protected override ServerVersion GetNodeServerVersion()
        {
            Cluster.LoadMachineRoles(true);
            var nodeRole = Cluster.MachineRoles[Constants.SkyNodeMachineRoleName];
            nodeRole.LoadServerVersions(true);
            var nodeServerVersion = nodeRole.ServerVersions[Constants.SkyNodeServerVersionName];
            return nodeServerVersion;
        }

        public override void GenerateDefaultSettings()
        {
            Federation.System = true;

            Federation.SchemaManager = GetUnversionedTypeName(typeof(Jhu.Graywulf.Sql.Schema.GraywulfSchemaManager));
            Federation.UserDatabaseFactory = GetUnversionedTypeName(typeof(Jhu.Graywulf.CasJobs.CasJobsUserDatabaseFactory));
            Federation.QueryFactory = GetUnversionedTypeName(typeof(Jhu.SkyQuery.Sql.Jobs.Query.SkyQueryQueryFactory));
            Federation.FileFormatFactory = GetUnversionedTypeName(typeof(Jhu.SkyQuery.Format.SkyQueryFileFormatFactory));
            Federation.StreamFactory = GetUnversionedTypeName(typeof(Jhu.SkyQuery.IO.SkyQueryStreamFactory));
            Federation.ImportTablesJobFactory = GetUnversionedTypeName(typeof(Jhu.Graywulf.SciDrive.SciDriveImportTablesJobFactory));
            Federation.ExportTablesJobFactory = GetUnversionedTypeName(typeof(Jhu.Graywulf.SciDrive.SciDriveExportTablesJobFactory));

            Federation.ShortTitle = "SkyQuery";
            Federation.LongTitle = "SkyQuery Astronomical Cross-Match Engine";
            Federation.Copyright = Jhu.Graywulf.Util.AssemblyReflector.GetCopyright(GetType().Assembly);
            Federation.Disclaimer = Domain.Disclaimer;
            Federation.Email = Domain.Email;
        }

        protected override void GenerateQueryJobs()
        {
            // SkyQuery extends query framework with the ability to run
            // XMatch queries, but non-xmatch queries still use the
            // generic query workflow.

            base.GenerateQueryJobs();

            var jdi = new Jhu.SkyQuery.Sql.Jobs.Query.XMatchQueryJobInstaller(Federation);
            jdi.Install();
        }
    }
}