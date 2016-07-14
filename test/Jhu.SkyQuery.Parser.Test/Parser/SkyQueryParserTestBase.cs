using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.SqlParser;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Parser
{
    public abstract class SkyQueryParserTestBase
    {
        protected SqlServerDataset CodeDataset
        {
            get
            {
                return new SqlServerDataset("CODE", "Initial Catalog=SkyQuery_CODE");
            }
        }

        protected virtual SkyQueryParser Parser
        {
            get { return new SkyQueryParser(); }
        }

        protected virtual QuerySpecification Parse(string query)
        {
            return (QuerySpecification)((SelectStatement)Parser.Execute(query)).EnumerateQuerySpecifications().First(); ;
        }

        protected Jhu.Graywulf.SqlCodeGen.SqlServer.SqlServerCodeGenerator CodeGenerator
        {
            get
            {
                return new Jhu.Graywulf.SqlCodeGen.SqlServer.SqlServerCodeGenerator()
                {
                    ResolveNames = true
                };
            }
        }
    }
}
