using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public class BayesianXMatchQuerySpecification : XMatchQuerySpecification
    {
        public BayesianXMatchQuerySpecification()
            : base()
        {
        }

        public BayesianXMatchQuerySpecification(QuerySpecification qs)
            :base(qs)
        {
        }

        public BayesianXMatchQuerySpecification(BayesianXMatchQuerySpecification old)
        {
        }

        public override IEnumerable<TableReference> EnumerateSourceTableReferences(bool recursive)
        {
            foreach (var tr in base.EnumerateSourceTableReferences(recursive))
            {
                yield return tr;
            }

            var btr = new BayesianXMatchTableReference();
            btr.Alias = this.FindDescendant<XMatchClause>().Alias;
            yield return btr;
        }
    }
}
