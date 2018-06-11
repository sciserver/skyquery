using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.Schema;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public class SkyQueryNameResolver : SqlNameResolver
    {
        private static readonly HashSet<string> SystemFunctionNames = new HashSet<string>(SchemaManager.Comparer)
        {
            "POINT"
        };

        public SkyQueryNameResolver()
            : base()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }

        protected override bool IsSystemFunctionName(string name)
        {
            if (!base.IsSystemFunctionName(name))
            {
                return SystemFunctionNames.Contains(name);
            }
            else
            {
                return true;
            }
        }
    }
}
