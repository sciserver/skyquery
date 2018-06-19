using System;
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
using Jhu.Graywulf.Sql.Validation;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    [Serializable]
    [DataContract(Name = "Query", Namespace = "")]
    public class ConeXMatchQuery : XMatchQuery
    {

        // TODO: make these into a class
        private TableHint regionHint;
        private string regionHintIdentifier;
        private XMatchTableSpecification regionHintTable;
        private Expression[] regionHintArguments;

        public TableHint RegionHint
        {
            get { return regionHint; }
        }

        public string RegionHintIdentifier
        {
            get { return regionHintIdentifier; }
        }

        public XMatchTableSpecification RegionHintTable
        {
            get { return regionHintTable; }
        }

        public Expression[] RegionHintArguments
        {
            get { return regionHintArguments; }
        }

        private ConeXMatchQueryCodeGenerator CodeGenerator
        {
            get { return (ConeXMatchQueryCodeGenerator)CreateCodeGenerator(); }
        }

        public ConeXMatchQuery()
        {
        }

        protected ConeXMatchQuery(CancellationContext cancellationContext)
            : base(cancellationContext)
        {
            InitializeMembers(new StreamingContext());
        }

        public ConeXMatchQuery(CancellationContext cancellationContext, gw.RegistryContext context)
            : base(cancellationContext, context)
        {
            InitializeMembers(new StreamingContext());
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
        }

        protected override SqlQueryCodeGenerator CreateCodeGenerator()
        {
            return new ConeXMatchQueryCodeGenerator(this)
            {
                TableNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                TableAliasRendering = Graywulf.Sql.CodeGeneration.AliasRendering.Default,
                ColumnNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                ColumnAliasRendering = Graywulf.Sql.CodeGeneration.AliasRendering.Always,
                DataTypeNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                FunctionNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified
            };
        }

        protected override SqlQueryPartition CreatePartition()
        {
            return new ConeXMatchQueryPartition(this);
        }

        protected override void InterpretLimit(XMatchTableSource xmts)
        {
            var xmc = xmts.FindDescendant<XMatchConstraint>();
            regionHint = xmc.FindDescendant<TableHint>();
            regionHintIdentifier = regionHint.Identifier.Value.ToUpperInvariant();
            regionHintArguments = regionHint.GetArguments();

            var column = regionHintArguments[0].FindDescendantRecursive<ColumnIdentifier>();
            var table = column.TableReference;

            foreach (var key in xmatchTables.Keys)
            {
                if (xmatchTables[key].TableReference == table)
                {
                    regionHintTable = xmatchTables[key];
                    break;
                }
            }
        }

        protected override List<XMatchTableSpecification> SortXMatchTableSpecifications()
        {
            // First table must be the one with the region centered on
            // TODO: update this to be more robust
            
            var tables = new List<XMatchTableSpecification>();

            foreach (var key in xmatchTables.Keys)
            {
                if (xmatchTables[key] != regionHintTable)
                {
                    tables.Add(xmatchTables[key]);
                }
            }

            // TODO: rename to cardinality comparer or similar
            tables.Sort(new XMatchTableComparer(this));
            tables.Insert(0, regionHintTable);

            return tables;
        }
    }
}
