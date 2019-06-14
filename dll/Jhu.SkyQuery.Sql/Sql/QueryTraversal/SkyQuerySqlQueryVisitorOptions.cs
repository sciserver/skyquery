using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Sql.QueryTraversal;

namespace Jhu.SkyQuery.Sql.QueryTraversal
{
    public class SkyQuerySqlQueryVisitorOptions : Jhu.Graywulf.Sql.Extensions.QueryTraversal.GraywulfSqlQueryVisitorOptions
    {
        #region Private member variables

        private ExpressionTraversalMethod regionExpressionTraversal;

        #endregion
        #region Properties

        public ExpressionTraversalMethod RegionExpressionTraversal
        {
            get { return regionExpressionTraversal; }
            set { regionExpressionTraversal = value; }
        }

        #endregion
        #region Constructors and initializers

        public SkyQuerySqlQueryVisitorOptions()
        {
            InitializeMembers();
        }

        public SkyQuerySqlQueryVisitorOptions(SkyQuerySqlQueryVisitorOptions old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.regionExpressionTraversal = ExpressionTraversalMethod.Infix;
        }

        private void CopyMembers(SkyQuerySqlQueryVisitorOptions old)
        {
            this.regionExpressionTraversal = old.regionExpressionTraversal;
        }

        #endregion
    }
}
