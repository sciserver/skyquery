using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.NameResolution;

namespace Jhu.SkyQuery.Sql.Parsing
{
    // TODO: Remove IComparable<XMatchTableSpecification>
    public partial class XMatchTableSpecification : ICloneable
    {
        private XMatchInclusionMethod inclusionMethod;

        #region Properties

        public XMatchInclusionMethod InclusionMethod
        {
            get { return inclusionMethod; }
        }
        
        public TableReference TableReference
        {
            get { return TableSource.TableReference; }
        }

        public CoordinatesTableSource TableSource
        {
            get { return FindDescendant<CoordinatesTableSource>(); }
        }

        public TableCoordinates Coordinates
        {
            get { return TableSource.Coordinates; }
        }

        // TODO: move this somewhere from here
        /*
        public Spherical.Region Region
        {
            get
            {
                var ss = FindAscendant<XMatchSelectStatement>();
                return ss.Region;
            }
        }*/

        #endregion
        #region Constructors and initializers

        protected override void OnInitializeMembers()
        {
            base.OnInitializeMembers();

            this.inclusionMethod = XMatchInclusionMethod.Unknown;
        }

        protected override void OnCopyMembers(object other)
        {
            base.OnCopyMembers(other);

            var old = (XMatchTableSpecification)other;

            this.inclusionMethod = old.inclusionMethod;
        }

        #endregion

        protected override void OnInterpret()
        {
            base.OnInterpret();

            inclusionMethod = InterpretInclusionMethod();
        }

        private XMatchInclusionMethod InterpretInclusionMethod()
        {
            var tokens = this.FindDescendant<XMatchTableInclusion>().Stack.ToArray();

            if (tokens.Length > 2)
            {
                if (SkyQueryParser.ComparerInstance.Compare(tokens[0].Value, Constants.InclusionMethodMay) == 0)
                {
                    return XMatchInclusionMethod.May;
                }
                else if (SkyQueryParser.ComparerInstance.Compare(tokens[0].Value, Constants.InclusionMethodNot) == 0)
                {
                    return XMatchInclusionMethod.Drop;
                }
                else if (SkyQueryParser.ComparerInstance.Compare(tokens[0].Value, Constants.InclusionMethodMust) == 0)
                {
                    return XMatchInclusionMethod.Must;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                return XMatchInclusionMethod.Must;
            }
        }
    }
}
