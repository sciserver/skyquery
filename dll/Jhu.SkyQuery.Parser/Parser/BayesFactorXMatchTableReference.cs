﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.Schema;

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

            AddColumn("MatchID", DataTypes.SqlBigInt);
            AddColumn("LogBF", DataTypes.SqlFloat);
            AddColumn("RA", DataTypes.SqlFloat);
            AddColumn("Dec", DataTypes.SqlFloat);
            AddColumn("Q", DataTypes.SqlFloat);
            AddColumn("L", DataTypes.SqlFloat);
            AddColumn("A", DataTypes.SqlFloat);
            AddColumn("Cx", DataTypes.SqlFloat);
            AddColumn("Cy", DataTypes.SqlFloat);
            AddColumn("Cz", DataTypes.SqlFloat);
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