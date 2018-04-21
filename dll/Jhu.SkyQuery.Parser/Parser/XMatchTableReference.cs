using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.Schema;

namespace Jhu.SkyQuery.Parser
{
    public abstract class XMatchTableReference : TableReference
    {
        protected XMatchTableReference()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }

        protected Column AddColumn(string name, Jhu.Graywulf.Sql.Schema.DataType type)
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
