using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.SqlCodeGen.SqlServer;
using Jhu.Graywulf.SqlParser;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.CodeGen
{
    [Serializable]
    public class Catalog : ITableReference
    {
        private int id;
        private XMatchInclusionMethod inclusionMethod;
        private string datasetName;
        private string tableOrViewName;
        private string alias;
        private TableReference tableReference;
        private CoordinateMode coordinateMode;
        private string raColumn;
        private string decColumn;
        private ErrorMode errorMode;
        private string errorColumn;
        private double errorValue;
        private double errorMin;
        private double errorMax;
        private string where;
        private SkyQuery.Parser.SearchCondition searchCondition;
        private List<string> columns;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public XMatchInclusionMethod InclusionMethod
        {
            get { return inclusionMethod; }
            set { inclusionMethod = value; }
        }

        public string DatasetName
        {
            get { return datasetName; }
            set { datasetName = value; }
        }

        public string TableOrViewName
        {
            get { return tableOrViewName; }
            set { tableOrViewName = value; }
        }

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        public TableReference TableReference
        {
            get { return tableReference; }
            set { tableReference = value; }
        }

        public CoordinateMode CoordinateMode
        {
            get { return coordinateMode; }
            set { coordinateMode = value; }
        }
        
        public string RaColumn
        {
            get { return raColumn; }
            set { raColumn = value; }
        }

        public string DecColumn
        {
            get { return decColumn; }
            set { decColumn = value; }
        }

        public ErrorMode ErrorMode
        {
            get { return errorMode; }
            set { errorMode = value; }
        }

        public string ErrorColumn
        {
            get { return errorColumn; }
            set { errorColumn = value; }
        }

        public double ErrorValue
        {
            get { return errorValue; }
            set { errorValue = value; }
        }

        public double ErrorMin
        {
            get { return errorMin; }
            set { errorMin = value; }
        }

        public double ErrorMax
        {
            get { return errorMax; }
            set { errorMax = value; }
        }

        public string Where
        {
            get { return where; }
            set { where = value; }
        }

        public SkyQuery.Parser.SearchCondition SearchCondition
        {
            get { return searchCondition; }
            set { searchCondition = value; }
        }

        public List<string> Columns
        {
            get { return columns; }
        }

        public Catalog()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.id = 0;
            this.inclusionMethod = XMatchInclusionMethod.Must;
            this.datasetName = null;
            this.tableOrViewName = null;
            this.alias = null;
            this.tableReference = null;
            this.coordinateMode = CoordinateMode.Automatic;
            this.raColumn = "ra";
            this.decColumn = "dec";
            this.errorMode = ErrorMode.Constant;
            this.errorColumn = null;
            this.errorValue = 0.1;
            this.errorMin = 0.1;
            this.errorMax = 0.1;
            this.where = null;
            this.searchCondition = null;
            this.columns = new List<string>();
        }
    }
}