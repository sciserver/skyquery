using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.SqlCodeGen.SqlServer;

namespace Jhu.SkyQuery.Jobs.Query
{
    public class RegionQueryCodeGenerator : SqlServerCodeGenerator
    {
        public string GetRegionConditionText(SkyQuery.Parser.SimpleTableSource ts, SqlServerDataset codeDataset)
        {
            var coords = ts.Coordinates;
            string sql;

            if (coords.IsEqSpecified)
            {
                sql = String.Format("(@r.ContainsEq({0}, {1}) = 1)",
                    coords.GetRAString(codeDataset),
                    coords.GetDecString(codeDataset));
            }
            else if (coords.IsCartesianSpecified)
            {
                sql = String.Format("(@r.ContainsXyz({0}, {1}, {2}) = 1)",
                    coords.GetXString(codeDataset),
                    coords.GetYString(codeDataset),
                    coords.GetZString(codeDataset));
            }
            else
            {
                throw new NotImplementedException();
            }

            return sql;
        }

        public SearchConditionBrackets GetRegionCondition(SkyQuery.Parser.SimpleTableSource ts, SqlServerDataset codeDataset)
        {
            var sql = GetRegionConditionText(ts, codeDataset);

            var p = new SqlParser();
            var sc = (SearchConditionBrackets)p.Execute(new SearchConditionBrackets(), sql);

            return sc;
        }

        public string GetHtmJoinConditionText(SkyQuery.Parser.SimpleTableSource ts, TableReference htmTable)
        {
            var coords = ts.Coordinates;

            var sql = String.Format(
                "({0} BETWEEN __htm.HtmIDStart AND __htm.HtmIDEnd)",
                coords.GetHtmIdString(),
                GetUniqueName(htmTable));

            return sql;
        }

        public SearchConditionBrackets GetHtmJoinCondition(SkyQuery.Parser.SimpleTableSource ts, TableReference htmTable)
        {
            var sql = GetHtmJoinConditionText(ts, htmTable);

            var p = new SqlParser();
            var sc = (SearchConditionBrackets)p.Execute(new SearchConditionBrackets(), sql);

            return sc;
        }
    }
}
