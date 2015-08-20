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

        protected override void OnInitializeMembers()
        {
            base.OnInitializeMembers();

            this.coordinates = null;
        }

        protected override void OnCopyMembers(object other)
        {
            base.OnCopyMembers(other);

            var old = (SimpleTableSource)other;

            this.coordinates = new TableCoordinates(this);
        }
        
        #endregion

        public override void Interpret()
        {
            base.Interpret();

            coordinates = new TableCoordinates(this);
        }
    }
}
