using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.Schema;

namespace Jhu.SkyQuery.Parser
{
    public class ConeXMatchTableReference : XMatchTableReference
    {
        public ConeXMatchTableReference()
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
        }
    }
}
