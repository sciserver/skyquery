using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    public class RegionQueryPartition : SqlQueryPartition, ICloneable
    {
        protected const string regionParameterName = "@region";

        #region Private member variables

        [NonSerialized]
        private Jhu.Spherical.Region region;

        #endregion
        #region Properties

        public Spherical.Region Region
        {
            get { return region; }
        }

        #endregion
        #region Constructors and initializers

        public RegionQueryPartition()
            : base()
        {
            InitializeMembers();
        }

        public RegionQueryPartition(RegionQuery query, Context context)
            : base(query, context)
        {
            InitializeMembers();
        }

        public RegionQueryPartition(RegionQueryPartition old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.region = null;
        }

        private void CopyMembers(RegionQueryPartition old)
        {
            this.region = old.region;
        }

        public override object Clone()
        {
            return new RegionQueryPartition(this);
        }

        #endregion

        protected override void FinishInterpret(bool forceReinitialize)
        {
            region = RegionQuery.InterpretRegion(SelectStatement);

            base.FinishInterpret(forceReinitialize);
        }

        protected override string GetExecuteQueryText()
        {
            return base.GetExecuteQueryText();
        }

        protected override void RewriteQueryForExecute()
        {
            base.RewriteQueryForExecute();

            if (region != null)
            {
                foreach (var qs in SelectStatement.EnumerateQuerySpecifications())
                {
                    var htminner = GetHtmTable(false);
                    var htmpartial = GetHtmTable(true);

                    AppendRegionJoinsAndConditions((QuerySpecification)qs, new TableReference(htminner, "__htm"));
                }
            }
        }

        private void RemoveExtraTokens()
        {
            // Strip off table hints
            foreach (var qs in SelectStatement.EnumerateQuerySpecifications())
            {
                foreach (var ts in qs.EnumerateDescendantsRecursive<SkyQuery.Parser.SimpleTableSource>())
                {
                    // TODO: update this to leave standard SQL Server hints?
                    var hint = ts.FindDescendant<Jhu.Graywulf.SqlParser.TableHintClause>();

                    if (hint != null)
                    {
                        hint.Parent.Stack.Remove(hint);
                    }
                }

                // Strip off region clause
                var region = SelectStatement.FindDescendant<Jhu.SkyQuery.Parser.RegionClause>();
                if (region != null)
                {
                    region.Parent.Stack.Remove(region);
                }
            }
        }

        private void AppendRegionJoinsAndConditions(QuerySpecification qs, TableReference htmtable)
        {
            var cg = new RegionQueryCodeGenerator();

            Jhu.Graywulf.SqlParser.SearchCondition joinConditions = null;
            Jhu.Graywulf.SqlParser.SearchCondition whereConditions = null;

            // Assume that region is not null
            foreach (var ts in qs.EnumerateDescendantsRecursive<SkyQuery.Parser.SimpleTableSource>())
            {
                var coords = ts.Coordinates;

                if (coords == null || coords.IsNoRegion)
                {
                    // The region contraint doesn't apply to the table
                    return;
                }
                else if (coords.IsHtmIdSpecified)
                {
                    var sc = cg.GetHtmJoinCondition(ts, htmtable);

                    if (joinConditions == null)
                    {
                        joinConditions = SearchCondition.Create(false, sc);
                    }
                    else
                    {
                        joinConditions = SearchCondition.Create(
                            sc, LogicalOperator.CreateAnd(), joinConditions);
                    }
                }
                else
                {
                    // Filter using region.Contains
                    var sc = cg.GetRegionCondition(ts, CodeDataset);

                    if (whereConditions == null)
                    {
                        whereConditions = SearchCondition.Create(false, sc);
                    }
                    else
                    {
                        whereConditions = SearchCondition.Create(
                            sc, LogicalOperator.CreateAnd(), whereConditions);
                    }
                }    
            }

            // Append where condition, if necessary
            if (whereConditions != null)
            {
                qs.AppendSearchCondition(whereConditions, "AND");
            }
            
            // Append htm table, if necessary
            if (joinConditions != null)
            {
                var jt = JoinedTable.Create(
                    JoinType.CreateInner(),
                    TableSource.Create(htmtable),
                    joinConditions);

                // Filter using HTM index, add a join clause
                qs.FromClause.AppendJoinedTable(jt);
            }
        }

        protected Table GetHtmTable(bool partial)
        {
            return GetTemporaryTable(String.Format(
                "Htm_{0}",
                partial ? "Partial" : "Inner"));
        }

        protected Table GetHtmTable(int stepNumber, bool partial)
        {
            return GetTemporaryTable(String.Format(
                "Htm_{0}_{1}",
                partial ? "Partial" : "Inner",
                stepNumber));
        }
    }
}
