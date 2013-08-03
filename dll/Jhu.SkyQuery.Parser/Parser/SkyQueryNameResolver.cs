using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.SkyQuery.Parser;
using Jhu.Graywulf.Schema;

namespace Jhu.SkyQuery.Parser
{
    public class SkyQueryNameResolver : SqlNameResolver
    {
        // **** delete private SchemaManagerBase schemaManager;

        public SkyQueryNameResolver()
            : base()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }


        protected override ColumnContext GetColumnContext(Node n, ColumnContext context)
        {
            context = base.GetColumnContext(n, context);

            if (n is XMatchClause)
            {
                context = ColumnContext.Special;
            }

            return context;
        }


#if false
        /// <summary>
        /// Resolve column identifiers under a parsing tree node
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="parentNode"></param>
        /// <param name="node"></param>
        /// <param name="context"></param>
        protected override void ResolveColumnAliases(Graywulf.SqlParser.QuerySpecification qs, Node parentNode, Node node, ColumnContext context)
        {
            if (node != null)
            {
                foreach (object n in node.Nodes)
                {
                    if (n is ColumnIdentifier)
                    {
                        var ci = (ColumnIdentifier)n;

                        // See if it's a SELECT * query.
                        if (ci.ColumnReference.IsStar)
                        {
                            // Mark all columns as part of the current context
                            foreach (var cr in qs.ColumnReferences)
                            {
                                if (ci.TableReference.IsUndefined || cr.TableReference.Compare(ci.TableReference))
                                {
                                    cr.ColumnContext |= context;
                                }
                            }
                        }
                        else
                        {
                            int q = 0;
                            foreach (var cr in qs.ColumnReferences)
                            {
                                if (ci.ColumnReference.Compare(cr))
                                {
                                    if (q != 0)
                                    {
                                        throw CreateException(Jhu.Graywulf.SqlParser.ExceptionMessages.AmbigousColumnReference, null, ci.ColumnReference.ColumnName, ci);
                                    }

                                    // TODO: delete?
                                    /*if (!(parentNode is XMatchClause))
                                    {
                                        cr.ColumnContext |= ColumnContext.Special;
                                    }*/

                                    ci.ColumnReference = cr;
                                    cr.ColumnContext |= context;

                                    q++;
                                }
                            }

                            if (q == 0)
                            {
                                throw CreateException(Jhu.Graywulf.SqlParser.ExceptionMessages.UnresolvableColumnReference, null, ci.ColumnReference.ColumnName, ci);
                            }
                        }
                    }
                    // TODO: delete?
                    /*else if (n is Node && !(n is Subquery) && !(n is XMatchHavingClause))
                    {
                        ResolveColumnAliases(qs, parentNode, (Node)n);
                    }*/
                    else if (n is Node && !(n is Subquery))
                    {
                        ResolveColumnAliases(qs, parentNode, (Node)n, context);
                    }
                }
            }

        }
#endif
    }
}
