using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public class SkyQueryValidator : SqlValidator
    {

        public SkyQueryValidator()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }

        public override void Execute(Graywulf.SqlParser.SelectStatement selectStatement)
        {
            base.Execute(selectStatement);

            if (selectStatement is XMatchSelectStatement)
            {
                ValidateXMatchQuery((XMatchSelectStatement)selectStatement);
            }
            else if (selectStatement is RegionSelectStatement)
            {
                ValidateRegionQuery((RegionSelectStatement)selectStatement);
            }

            // Make sure subqueries are not xmatch queries
            CheckSubqueries(selectStatement);
        }

        private void ValidateRegionQuery(RegionSelectStatement selectStatement)
        {
            // Parse region string or try to download from URI if necessary
            if (selectStatement.RegionClause.IsUri)
            {
                throw new NotImplementedException();
            }
            else
            {
                var p = new Jhu.Spherical.Parser.Parser(selectStatement.RegionClause.RegionString);
                p.Simplify = false;
                p.Normalize = false;
                p.ParseRegion();
            }
        }

        private void ValidateXMatchQuery(XMatchSelectStatement selectStatement)
        {
            // Make sure xmatch table sources don't contain any views.
            var xmatch = selectStatement.EnumerateQuerySpecifications().First().FindDescendant<XMatchClause>();
            if (xmatch != null)
            {
                var xmatchTables = new List<XMatchTableSpecification>(xmatch.EnumerateXMatchTableSpecifications());
                foreach (var xt in xmatchTables)
                {
                    if (!(xt.TableReference.DatabaseObject is TableOrView))
                    {
                        throw new ValidatorException(ExceptionMessages.OnlyTablesAllowed);
                    }
                    else if (((TableOrView)xt.TableReference.DatabaseObject).PrimaryKey == null)
                    {
                        throw new ValidatorException(ExceptionMessages.PrimaryKeyRequired);
                    }
                }
            }
        }

        /// <summary>
        /// Makes sure query does not contain xmatch subqueries
        /// </summary>
        /// <param name="selectStatement"></param>
        protected void CheckXMatchSubqueries(Graywulf.SqlParser.SelectStatement selectStatement)
        {
            foreach (var qs in selectStatement.EnumerateQuerySpecifications())
            {
                foreach (var sq in qs.EnumerateSubqueries())
                {
                    if (sq.FindDescendantRecursive<XMatchClause>() != null)
                    {
                        throw CreateException(ExceptionMessages.XMatchSubqueryNotAllowed, sq);
                    }
                }
            }
        }

        /// <summary>
        /// Makes sure query does not reference invalid table sources in the xmatch clause
        /// </summary>
        /// <param name="selectStatement"></param>
        protected void CheckSubqueries(Graywulf.SqlParser.SelectStatement selectStatement)
        {
            var xmc = selectStatement.FindDescendantRecursive<XMatchClause>();
            if (xmc != null)
            {
                foreach (var xmts in xmc.EnumerateXMatchTableSpecifications())
                {
                    if (xmts.TableReference.IsSubquery)
                    {
                        throw CreateException(ExceptionMessages.SubqueryInXMatchNotAllowed, xmts);
                    }
                    else if (xmts.TableReference.IsUdf)
                    {
                        throw CreateException(ExceptionMessages.FunctionInXMatchNotAllowed, xmts);
                    }
                }
            }
        }
    }
}
