using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Install;
using Jhu.Graywulf.CommandLineParser;

namespace Jhu.SkyQuery.Install.CmdLineUtil
{
    [Verb(Name = "Install", Description = "Creates a new Federation for SkyQuery in the Graywulf registry")]
    class Install : Verb
    {
        private string domainName;
        private string myDBServerVersionName;
        private string codeDatabaseServerVersionName;

        [Parameter(Name = "Domain", Description = "Name of the domain", Required = true)]
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        public Install()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.domainName = null;
        }

        public override void Run()
        {
            Console.Write("Creating federation for SkyQuery... ");

            using (Context context = ContextManager.Instance.CreateContext(ConnectionMode.AutoOpen, TransactionMode.ManualCommit))
            {
                var ef = new EntityFactory(context);
                var domain = ef.LoadEntity<Domain>(domainName);

                var i = new Jhu.SkyQuery.Install.SkyQueryInstaller(domain);
                i.Install();

                context.CommitTransaction();
            }

            Console.WriteLine("done.");
        }

    }
}
