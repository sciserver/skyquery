using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.Types;
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

            AddColumn("LogBF", DataType.SqlFloat);
            AddColumn("RA", DataType.SqlFloat);
            AddColumn("Dec", DataType.SqlFloat);
            AddColumn("Q", DataType.SqlFloat);
            AddColumn("L", DataType.SqlFloat);
            AddColumn("A", DataType.SqlFloat);
            AddColumn("Cx", DataType.SqlFloat);
            AddColumn("Cy", DataType.SqlFloat);
            AddColumn("Cz", DataType.SqlFloat);
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
