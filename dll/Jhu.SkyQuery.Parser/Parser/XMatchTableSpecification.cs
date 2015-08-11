using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public partial class XMatchTableSpecification : ICloneable
    {
        private SimpleTableSource tableSource;
        private TableCoordinates coordinates;

        public XMatchInclusionMethod InclusionMethod
        {
            get
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

        public TableReference TableReference
        {
            get { return tableSource.TableReference; }
        }

        public SimpleTableSource TableSource
        {
            get { return tableSource; }
        }

        public TableCoordinates Coordinates
        {
            get { return coordinates; }
        }

        #region Constructors and initializers

        public XMatchTableSpecification()
            : base()
        {
            InitializeMembers();
        }

        public XMatchTableSpecification(XMatchTableSpecification old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(XMatchTableSpecification old)
        {
        }

        public virtual object Clone()
        {
            return new XMatchTableSpecification(this);
        }

        #endregion

        public override Node Interpret()
        {
            tableSource = FindDescendant<SimpleTableSource>();
            coordinates = new TableCoordinates(this.tableSource);

            return base.Interpret();
        }
    }
}
