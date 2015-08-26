using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.SqlParser;
using Jhu.SkyQuery.Parser;
using Jhu.Spherical;

namespace Jhu.SkyQuery.Jobs.Query
{
    public class AugmentedTableQueryOptions
    {
        private SkyQuery.Parser.SimpleTableSource table;
        private Region region;
        private bool useRegion;
        private bool useConditions;
        private bool useHtm;
        private bool usePartitioning;
        private ColumnContext columnContext;
        private bool escapeColumnNames;

        public SkyQuery.Parser.SimpleTableSource Table
        {
            get { return table; }
            set { table = value; }
        }

        public Region Region
        {
            get { return region; }
            set { region = value; }
        }
        public bool UseRegion
        {
            get { return useRegion; }
            set { useRegion = value; }
        }

        public bool UseConditions
        {
            get { return useConditions; }
            set { useConditions = value; }
        }

        public bool UseHtm
        {
            get { return useHtm; }
            set { useHtm = value; }
        }

        public bool UsePartitioning
        {
            get { return usePartitioning; }
            set { usePartitioning = value; }
        }

        public ColumnContext ColumnContext
        {
            get { return columnContext; }
            set { columnContext = value; }
        }

        public bool EscapeColumnNames
        {
            get { return escapeColumnNames; }
            set { escapeColumnNames = value; }
        }

        public AugmentedTableQueryOptions()
        {
            InitializeMembers();
        }

        public AugmentedTableQueryOptions(SkyQuery.Parser.SimpleTableSource table, Region region)
        {
            InitializeMembers();

            this.table = table;
            this.region = region;
        }

        private void InitializeMembers()
        {
            this.table = null;
            this.region = null;
            this.useRegion = true;
            this.useConditions = true;
            this.useHtm = true;
            this.usePartitioning = true;
            this.columnContext = ColumnContext.Default;
            this.escapeColumnNames = true;
        }
    }
}
