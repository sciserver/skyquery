﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Tasks;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    [Serializable]
    [DataContract(Name = "Query", Namespace = "")]
    public class BayesFactorXMatchQuery : XMatchQuery
    {
        private double limit;

        [DataMember]
        public double Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        private BayesFactorXMatchQueryCodeGenerator CodeGenerator
        {
            get { return (BayesFactorXMatchQueryCodeGenerator)CreateCodeGenerator(); }
        }

        public BayesFactorXMatchQuery()
        {
            // overload
        }

        protected BayesFactorXMatchQuery(CancellationContext cancellationContext)
            : base(cancellationContext)
        {
            InitializeMembers(new StreamingContext());
        }

        public BayesFactorXMatchQuery(CancellationContext cancellationContext, gw.RegistryContext context)
            : base(cancellationContext, context)
        {
            InitializeMembers(new StreamingContext());
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            this.limit = -1;
        }

        private void CopyMembers(BayesFactorXMatchQuery old)
        {
            this.limit = old.limit;
        }

        protected override SqlQueryCodeGenerator CreateCodeGenerator()
        {
            return new BayesFactorXMatchQueryCodeGenerator(this)
            {
                TableNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified,
                TableAliasRendering = Graywulf.Sql.QueryGeneration.AliasRendering.Default,
                ColumnNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified,
                ColumnAliasRendering = Graywulf.Sql.QueryGeneration.AliasRendering.Always,
                DataTypeNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified,
                FunctionNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified
            };
        }

        protected override SqlQueryPartition CreatePartition()
        {
            return new BayesFactorXMatchQueryPartition(this);
        }

        protected override void InterpretLimit(XMatchTableSourceSpecification xmts)
        {
            var number = xmts.FindDescendant<XMatchConstraint>().FindDescendant<NumericConstant>().Value;
            limit = double.Parse(number, System.Globalization.CultureInfo.InvariantCulture);
        }

        protected override List<XMatchTableSpecification> SortXMatchTableSpecifications()
        {
            var tables = new List<XMatchTableSpecification>();

            // Order tables based on incusion method and statistics
            foreach (var key in xmatchTables.Keys)
            {
                tables.Add(xmatchTables[key]);
            }

            // TODO: rename to cardinality comparer or similar
            tables.Sort(new XMatchTableComparer(this));

            return tables;
        }
    }
}
