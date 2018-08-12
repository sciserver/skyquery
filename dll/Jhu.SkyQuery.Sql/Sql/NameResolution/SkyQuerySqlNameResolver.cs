﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Sql.QueryTraversal;
using Jhu.SkyQuery.Sql.QueryTraversal;

namespace Jhu.SkyQuery.Sql.NameResolution
{
    public class SkyQuerySqlNameResolver : Jhu.Graywulf.Sql.Extensions.NameResolution.GraywulfSqlNameResolver
    {
        #region Properties

        public new SkyQuerySqlNameResolverOptions Options
        {
            get { return (SkyQuerySqlNameResolverOptions)base.Options; }
            set { base.Options = value; }
        }


        protected new SkyQuerySqlQueryVisitor Visitor
        {
            get { return (SkyQuerySqlQueryVisitor)base.Visitor; }
        }

        #endregion
        #region Constructors and initializers

        public SkyQuerySqlNameResolver()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }

        protected override Jhu.Graywulf.Sql.NameResolution.SqlNameResolverOptions CreateOptions()
        {
            return new SkyQuerySqlNameResolverOptions();
        }

        protected override Jhu.Graywulf.Sql.QueryTraversal.SqlQueryVisitor CreateVisitor()
        {
            return new SkyQuerySqlQueryVisitor(this)
            {
                Options = new SkyQuerySqlQueryVisitorOptions()
                {
                    LogicalExpressionTraversal = ExpressionTraversalMethod.Infix,
                    ExpressionTraversal = ExpressionTraversalMethod.Infix,
                    VisitExpressionSubqueries = true,
                    VisitPredicateSubqueries = true,
                    VisitSchemaReferences = false,
                }
            };
        }

        #endregion
        #region Reference resolution

        protected override bool IsSystemFunctionName(string name)
        {
            return base.IsSystemFunctionName(name) || Constants.SkyQuerySystemFunctionNames.Contains(name);
        }

        protected override bool IsSystemVariableName(string name)
        {
            return base.IsSystemVariableName(name) || Constants.SkyQuerySystemFunctionNames.Contains(name);
        }

        #endregion
    }
}
