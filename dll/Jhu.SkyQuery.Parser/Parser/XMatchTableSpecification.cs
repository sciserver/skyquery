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
        private bool hasHtmIndex;
        private bool hasZoneIndex;

        #region Properties

        public XMatchInclusionMethod InclusionMethod
        {
            get { return inclusionMethod; }
        }

        public bool HasHtmIndex
        {
            get { return hasHtmIndex; }
        }

        public bool HasZoneIndex
        {
            get { return hasZoneIndex; }
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
            return Math.Sign(other.TableReference.Statistics.RowCount - this.TableReference.Statistics.RowCount);
        }

        /// <summary>
        /// Returns true if building a zone table on the fly is necessary
        /// </summary>
        /// <returns></returns>
        public bool IsZoneTableNecessary()
        {
            // It might be worth building a zone table if:
            // - the doesn't have a zoneID or it's not indexed
            // - the region constraint is small
            // - a region constraint is specified but the table has no HTMID
            
            var coords = this.Coordinates;

            if (!coords.IsZoneIdSpecified || coords.FindZoneIndex() == null)
            {
                return true;
            }

            if (this.Region != null)
            {
                // TODO: is 10 sq deg enough?
                if (!coords.IsHtmIdSpecified || Region.Area < 10)
                {
                    return true;
                }
            }

            // TODO: compare statistics with total row count to find tables with strong
            // where clause conditions

            return false;
        }
    }
}
