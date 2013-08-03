using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public partial class XMatchTableSpecification : ICloneable
    {
        private const int positionIndex = 0;
        private const int errorIndex = 1;

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
            get
            {
                return this.FindDescendant<TableOrViewName>().TableReference;
            }
        }

        public AdqlPoint Position
        {
            get
            {
                return this.FindDescendant<XMatchArgumentList>().FindDescendant<AdqlPoint>();
            }
        }

        public bool IsConstantError
        {
            get
            {
                XMatchArgumentList xal = this.FindDescendant<XMatchArgumentList>();
                ArgumentList al = xal.FindDescendant<ArgumentList>();
                var ars = al.EnumerateDescendants<Argument>().ToArray();
                return ars.Length == 1 && ars[0].FindDescendant<Expression>().IsConstantNumber;
            }
        }

        public Expression ErrorExpression
        {
            get
            {
                var xal = this.FindDescendant<XMatchArgumentList>();
                var al = xal.FindDescendant<ArgumentList>();
                var ar = al.FindDescendant<Argument>(0);
                return ar == null ? null : ar.FindDescendant<Expression>();
            }
        }

        public Expression MinErrorExpression
        {
            get
            {
                var xal = this.FindDescendant<XMatchArgumentList>();
                var al = xal.FindDescendant<ArgumentList>();
                var ar = al.FindDescendant<Argument>(1);
                return ar == null ? null : ar.FindDescendant<Expression>();
            }
        }

        public Expression MaxErrorExpression
        {
            get
            {
                var xal = this.FindDescendant<XMatchArgumentList>();
                var al = xal.FindDescendant<ArgumentList>();
                var ar = al.FindDescendant<Argument>(2);
                return ar == null ? null : ar.FindDescendant<Expression>();
            }
        }

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

        public override Node Interpret()
        {
            if (FindAscendant<XMatchClause>() is BayesianXMatchClause)
            {
                BayesianXMatchTableSpecification xmt = new BayesianXMatchTableSpecification(this);
                xmt.InterpretChildren();
                return xmt;
            }
            else
            {
                return base.Interpret();
            }
        }

    }
}
