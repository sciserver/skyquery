using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapFactory : DbProviderFactory
    {
        public override bool CanCreateDataSourceEnumerator
        {
            get { return false; }
        }

        public override DbCommand CreateCommand()
        {
            return new TapCommand();
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            throw new NotImplementedException();
        }

        public override DbConnection CreateConnection()
        {
            return new TapConnection();
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new TapConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            throw new NotImplementedException();
        }

        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            throw new NotImplementedException();
        }

        public override DbParameter CreateParameter()
        {
            return new TapParameter();
        }

        public override CodeAccessPermission CreatePermission(PermissionState state)
        {
            throw new NotImplementedException();
        }
    }
}
