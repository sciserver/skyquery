using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.Schema;

namespace Jhu.SkyQuery.Parser
{
    public class BayesianXMatchTableReference : TableReference
    {
        public BayesianXMatchTableReference()
            : base()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            IsComputed = true;

            DatabaseObject = new Table();

            AddColumn("LogBF", DataType.Float);
            AddColumn("RA", DataType.Float);
            AddColumn("Dec", DataType.Float);
            AddColumn("Q", DataType.Float);
            AddColumn("L", DataType.Float);
            AddColumn("A", DataType.Float);
            AddColumn("Cx", DataType.Float);
            AddColumn("Cy", DataType.Float);
            AddColumn("Cz", DataType.Float);
        }

        private void AddColumn(string name, DataType type)
        {
            Column col = new Column();
            col.Name = name;
            col.DataType = type;
            ((IColumns)this.DatabaseObject).Columns.TryAdd(col.Name, col);

            ColumnReference cr = new ColumnReference();
            cr.ColumnName = name;
            cr.DataType = type;
            cr.TableReference = this;
            this.ColumnReferences.Add(cr);
        }
    }
}
