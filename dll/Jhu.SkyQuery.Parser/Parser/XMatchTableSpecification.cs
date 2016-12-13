using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public partial class XMatchTableSpecification : ICloneable, IComparable<XMatchTableSpecification>
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

        public SimpleTableSource TableSource
        {
            get { return FindDescendant<SimpleTableSource>(); }
        }

        public TableCoordinates Coordinates
        {
            get { return TableSource.Coordinates; }
        }

        // TODO: move this somewhere from here
        public Spherical.Region Region
        {
            get
            {
                var qs = (XMatchQuerySpecification)FindAscendant<SkyQuery.Parser.QuerySpecification>();
                return qs.Region;
            }
        }

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

        public override void Interpret()
        {
            base.Interpret();

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

        public int CompareTo(object obj)
        {
            return CompareTo((XMatchTableSpecification)obj);
        }

        public int CompareTo(XMatchTableSpecification other)
        {
            // Inclusion method ordering always preceeds table statistics

            if (other.InclusionMethod != this.InclusionMethod)
            {
                return other.InclusionMethod - this.InclusionMethod;
            }

            // Order tables by cardinality
            return Math.Sign(this.TableReference.Statistics.RowCount - other.TableReference.Statistics.RowCount);
        }
    }
}
