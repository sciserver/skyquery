using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.Schema.SqlServer;

namespace Jhu.SkyQuery.Sql.Parsing
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

        protected StatementBlock ParseScript(string query)
        {
            var script = Parser.Execute<StatementBlock>(query);
            return script;
        }

        protected QuerySpecification Parse(string query)
        {
            var script = Parser.Execute<StatementBlock>(query);
            var statement = script.FindDescendantRecursive<Statement>();
            var select = statement.FindDescendant<SelectStatement>();
            var qs = select.QueryExpression.EnumerateQuerySpecifications().FirstOrDefault();

            return qs;
        }

        protected Jhu.Graywulf.Sql.CodeGeneration.SqlServer.SqlServerCodeGenerator CodeGenerator
        {
            get
            {
                return new Jhu.Graywulf.Sql.CodeGeneration.SqlServer.SqlServerCodeGenerator()
                {
                    //ResolveNames = true
                    // TODO: use *Rendering properties
                    TableNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                    TableAliasRendering = Graywulf.Sql.CodeGeneration.AliasRendering.Default,
                    ColumnNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                    ColumnAliasRendering = Graywulf.Sql.CodeGeneration.AliasRendering.Default,
                };
            }
        }
    }
}
