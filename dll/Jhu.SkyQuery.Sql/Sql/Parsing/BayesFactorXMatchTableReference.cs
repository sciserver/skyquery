using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.Schema;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public class BayesFactorXMatchTableReference : XMatchTableReference
    {
        public BayesFactorXMatchTableReference()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            IsComputed = true;

            DatabaseObject = new Table();

            AddColumn("MatchID", DataTypes.SqlBigInt).IsKey = true;

            AddColumn("RA", DataTypes.SqlFloat);
            AddColumn("Dec", DataTypes.SqlFloat);
            AddColumn("Cx", DataTypes.SqlFloat);
            AddColumn("Cy", DataTypes.SqlFloat);
            AddColumn("Cz", DataTypes.SqlFloat);
            AddColumn("N", DataTypes.SqlInt);
            AddColumn("A", DataTypes.SqlFloat);
            AddColumn("L", DataTypes.SqlFloat);
            AddColumn("Q", DataTypes.SqlFloat);
            AddColumn("LogBF", DataTypes.SqlFloat);
        }
    }
}
