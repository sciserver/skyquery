using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.Schema.SqlServer;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public abstract class SkyQueryParserTestBase : SkyQueryTestBase
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

        protected Jhu.Graywulf.Sql.QueryRendering.SqlServer.SqlServerQueryRenderer QueryRenderer
        {
            get
            {
                return new Jhu.Graywulf.Sql.QueryRendering.SqlServer.SqlServerQueryRenderer()
                {
                    Options = new Graywulf.Sql.QueryRendering.QueryRendererOptions()
                    {
                        TableNameRendering = Graywulf.Sql.QueryRendering.NameRendering.FullyQualified,
                        TableAliasRendering = Graywulf.Sql.QueryRendering.AliasRendering.Default,
                        ColumnNameRendering = Graywulf.Sql.QueryRendering.NameRendering.FullyQualified,
                        ColumnAliasRendering = Graywulf.Sql.QueryRendering.AliasRendering.Default,
                        DataTypeNameRendering = Graywulf.Sql.QueryRendering.NameRendering.FullyQualified,
                    }
                };
            }
        }
    }
}
