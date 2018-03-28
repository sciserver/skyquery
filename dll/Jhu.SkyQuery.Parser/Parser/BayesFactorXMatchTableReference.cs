using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.Schema;

namespace Jhu.SkyQuery.Parser
{
    public class BayesFactorXMatchTableReference : TableReference
    {
        public BayesFactorXMatchTableReference()
            : base()
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

        private Column AddColumn(string name, Jhu.Graywulf.Sql.Schema.DataType type)
        {
            var col = new Column();
            col.Name = name;
            col.DataType = type;
            ((IColumns)this.DatabaseObject).Columns.TryAdd(col.Name, col);

            ColumnReference cr = new ColumnReference();
            cr.ColumnName = name;
            cr.DataType = type;
            cr.TableReference = this;
            this.ColumnReferences.Add(cr);

            return col;
        }
    }
}
