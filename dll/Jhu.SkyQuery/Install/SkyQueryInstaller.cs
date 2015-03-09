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
            Federation.ImportTablesJobFactory = GetUnversionedTypeName(typeof(Jhu.Graywulf.SciDrive.SciDriveImportTablesJobFactory));
            Federation.ExportTablesJobFactory = GetUnversionedTypeName(typeof(Jhu.Graywulf.SciDrive.SciDriveExportTablesJobFactory));

            Federation.ShortTitle = "SkyQuery";
            Federation.LongTitle = "SkyQuery Astronomical Cross-Match Engine";
            Federation.Copyright = Copyright.InfoCopyright;
            Federation.Disclaimer = Domain.Disclaimer;
            Federation.Email = Domain.Email;
        }

        protected override void GenerateQueryJobs()
        {
            // SkyQuery extends query framework with the ability to run
            // XMatch queries, but non-xmatch queries still use the
            // generic query workflow.

            base.GenerateQueryJobs();

            var jdi = new XMatchQueryJobInstaller(Federation);
            jdi.Install();
        }
    }
}