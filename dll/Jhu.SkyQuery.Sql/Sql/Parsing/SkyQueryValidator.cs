using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Sql.Validation;
using Jhu.Graywulf.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public class SkyQueryValidator : SqlValidator
    {
        #region Constructors and initializers

        public SkyQueryValidator()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }

        #endregion
        
        protected override void OnValidateStatement(IStatement statement)
        {
            // and make sure xmatch queries are single-statement scripts

            // TODO: validate source tables whether they have the neccessary columns
            // for a region/XMatch query

            if (statement is XMatchSelectStatement)
            {
                ValidateXMatchQuery((XMatchSelectStatement)statement, 0);
            }
            else if (statement is RegionSelectStatement)
            {
                ValidateRegionQuery((RegionSelectStatement)statement, 0);
            }
            else
            {
                base.OnValidateStatement(statement);
            }
        }
        
        private void ValidateRegionQuery(RegionSelectStatement selectStatement, int depth)
        {
            // Parse region string or try to download from URI if necessary
            if (selectStatement.RegionClause.IsUri)
            {
                throw new NotImplementedException();
            }
            else if (selectStatement.RegionClause.IsString)
            {
                var p = new Jhu.Spherical.Parser.Parser(selectStatement.RegionClause.RegionString);
                p.Simplify = false;
                p.Normalize = false;
                p.ParseRegion();
            }
        }

        private void ValidateXMatchQuery(XMatchSelectStatement selectStatement, int depth)
        {
            int qsi = 0;    // counts query specifications
            int xmi = 0;    // counts xmatch constructs
            foreach (var qs in selectStatement.QueryExpression.EnumerateQuerySpecifications())
            {
                if (qsi > 0)
                {
                    throw new ValidatorException(ExceptionMessages.OnlySingleQueryAllowed);
                }

                // See if it's a XMatchQuery
                foreach (var ts in qs.EnumerateSourceTables(false))
                {
                    ValidateSourceTable(ts, ref xmi, depth);
                }

                qsi++;
            }
        }

        private void ValidateSourceTable(ITableSource ts, ref int xmi, int depth)
        {
            if (ts is XMatchTableSource)
            {
                if (xmi > 0)
                {
                    throw new ValidatorException(ExceptionMessages.OnlySingleXMatchAllowed);
                }

                if (depth > 0)
                {
                    // This is unlikely to happen as limited by the grammar
                    throw CreateException(ExceptionMessages.XMatchSubqueryNotAllowed, (Token)ts);
                }

                // Make sure xmatch table sources don't contain any subqueries or functions.
                foreach (var xt in ((XMatchTableSource)ts).EnumerateXMatchTableSpecifications())
                {
                    if (!(xt.TableReference.DatabaseObject is TableOrView))
                    {
                        // This is unlikely to happen as limited by the grammar
                        throw new ValidatorException(ExceptionMessages.OnlyTablesAllowed);
                    }

                    // TODO: allow no-PK logic for tables only that will be copied to the worker nodes
                    // Use logic from QueryObject.IsRemoteDataset
                    /*else if (((TableOrView)xt.TableReference.DatabaseObject).Dataset ....
                        ((TableOrView)xt.TableReference.DatabaseObject).PrimaryKey == null)
                    {
                        throw new ValidatorException(ExceptionMessages.PrimaryKeyRequired);
                    }*/
                }

                xmi++;
            }
            else if (ts.IsSubquery)
            {
                // Call validator recursively
                foreach (var tts in ts.EnumerateSubqueryTableSources(false))
                {
                    ValidateSourceTable(tts, ref xmi, depth + 1);
                }
            }
        }
    }
}
