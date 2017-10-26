using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapConnectionStringBuilder : DbConnectionStringBuilder
    {
        public string DataSource
        {
            get
            {
                if (ContainsKey(Constants.KeyDataSource))
                {
                    return (string)this[Constants.KeyDataSource];
                }
                else
                {
                    return null;
                }
            }
            set { this[Constants.KeyDataSource] = value; }
        }

        public int ConnectTimeout
        {
            get
            {
                if (ContainsKey(Constants.KeyConnectionTimeout))
                {
                    return (int)this[Constants.KeyConnectionTimeout];
                }
                else
                {
                    return Constants.DefaultConnectionTimeout;
                }
            }
            set { this[Constants.KeyConnectionTimeout] = value; }
        }

        public TapConnectionStringBuilder()
        {
        }

        public TapConnectionStringBuilder(string connectionString)
        {
            base.ConnectionString = connectionString;
        }
    }
}
