using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.SkyQuery.Parser;
using Jhu.Graywulf.Schema;

namespace Jhu.SkyQuery.Parser
{
    public class SkyQueryNameResolver : SqlNameResolver
    {
        private static readonly HashSet<string> SystemFunctionNames = new HashSet<string>(Jhu.Graywulf.Schema.SchemaManager.Comparer)
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


        protected override ColumnContext GetColumnContext(Node n, ColumnContext context)
        {
            context = base.GetColumnContext(n, context);

            // TODO: test
            //if (n is XMatchClause)
            if (n is CoordinateHintList)
            {
                context = ColumnContext.Special;
            }

            return context;
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
