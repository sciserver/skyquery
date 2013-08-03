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

        [Parameter(Name = "MyDB", Description = "Server version to store MyDBs", Required = false)]
        public string MyDBServerVersionName
        {
            get { return myDBServerVersionName; }
            set { myDBServerVersionName = value; }
        }

        [Parameter(Name = "CodeDB", Description = "Server version to store code DBs", Required = false)]
        public string CodeDatabaseServerVersionName
        {
            get { return codeDatabaseServerVersionName; }
            set { codeDatabaseServerVersionName = value; }
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
                var d = ef.LoadEntity<Domain>(domainName);

                // Load or figure out server versions
                ServerVersion mydbsv;
                if (myDBServerVersionName != null)
                {
                    mydbsv = ef.LoadEntity<ServerVersion>(myDBServerVersionName);
                }
                else
                {
                    mydbsv = ef.LoadEntity<ServerVersion>(d.Cluster.Name, Constants.NodeMachineRoleName, Constants.ServerVersionName);
                }

                ServerVersion codedbsv;
                if (codeDatabaseServerVersionName != null)
                {
                    codedbsv = ef.LoadEntity<ServerVersion>(codeDatabaseServerVersionName);
                }
                else
                {
                    codedbsv = ef.LoadEntity<ServerVersion>(d.Cluster.Name, Constants.NodeMachineRoleName, Constants.ServerVersionName);
                }
                
                var i = new Jhu.SkyQuery.Install.SkyQueryInstaller(context);
                i.Install(d, mydbsv, codedbsv);

                context.CommitTransaction();
            }

            Console.WriteLine("done.");
        }

    }
}
