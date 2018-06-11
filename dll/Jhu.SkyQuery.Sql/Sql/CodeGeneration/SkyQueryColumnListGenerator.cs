using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.CodeGeneration;
using Jhu.Graywulf.Sql.CodeGeneration.SqlServer;

namespace Jhu.SkyQuery.Sql.CodeGeneration
{
    public class SkyQueryColumnListGenerator : SqlServerColumnListGenerator
    {
        public SkyQueryColumnListGenerator()
            : base()
        {
        }

        public SkyQueryColumnListGenerator(IEnumerable<ColumnReference> columns)
            : base(columns)
        {
        }

        public SkyQueryColumnListGenerator(TableReference tr, ColumnContext context, ColumnListType listType)
            : base(tr, context, listType)
        {
        }
    }
}
