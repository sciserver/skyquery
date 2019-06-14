using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public partial class CoordinatesTableSource
    {
        #region Private member variables

        private TableCoordinates coordinates;

        #endregion
        #region Properties

        public TableCoordinates Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }

        #endregion
        #region Constructors and initializers

        public CoordinatesTableSource(SimpleTableSource other)
            : base(other)
        {
        }

        protected override void OnInitializeMembers()
        {
            base.OnInitializeMembers();

            this.coordinates = null;
        }

        protected override void OnCopyMembers(object other)
        {
            base.OnCopyMembers(other);
        }

        #endregion
    }
}
