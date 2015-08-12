using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public partial class SimpleTableSource : ITableSource
    {
        #region Private member variables

        private TableCoordinates coordinates;

        #endregion
        #region Properties

        public TableCoordinates Coordinates
        {
            get { return coordinates; }
        }

        #endregion

        #region Constructors and initializers

        public SimpleTableSource()
            : base()
        {
            InitializeMembers();
        }

        public SimpleTableSource(SimpleTableSource old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.coordinates = null;
        }

        private void CopyMembers(SimpleTableSource old)
        {
            this.coordinates = new TableCoordinates(this);
        }

        public virtual object Clone()
        {
            return new SimpleTableSource(this);
        }

        #endregion

        public override Jhu.Graywulf.ParserLib.Node Interpret()
        {
            coordinates = new TableCoordinates(this);

            return base.Interpret();
        }
    }
}
