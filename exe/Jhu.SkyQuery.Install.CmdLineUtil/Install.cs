using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Install.CmdLineUtil;
using Jhu.Graywulf.CommandLineParser;

namespace Jhu.SkyQuery.Install.CmdLineUtil
{
    [Verb(Name = "Install", Description = "Creates a new Federation for SkyQuery in the Graywulf registry")]
    class Install : CreateRegistry
    {
        private string domainName;

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

            using (RegistryContext context = ContextManager.Instance.CreateContext(TransactionMode.ManualCommit))
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
