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

        protected Jhu.Graywulf.Sql.QueryGeneration.SqlServer.SqlServerQueryGenerator CodeGenerator
        {
            get
            {
                return new Jhu.Graywulf.Sql.QueryGeneration.SqlServer.SqlServerQueryGenerator()
                {
                    //ResolveNames = true
                    // TODO: use *Rendering properties
                    TableNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified,
                    TableAliasRendering = Graywulf.Sql.QueryGeneration.AliasRendering.Default,
                    ColumnNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified,
                    ColumnAliasRendering = Graywulf.Sql.QueryGeneration.AliasRendering.Default,
                    DataTypeNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified,
                };
            }
        }
    }
}
