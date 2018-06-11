using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.CodeGeneration
{
    [Serializable]
    public class Catalog
    {
        private int id;
        private XMatchInclusionMethod inclusionMethod;
        private string datasetName;
        private string tableUniqueKey;
        private string alias;
        private CoordinateMode coordinateMode;
        private string raColumn;
        private string decColumn;
        private ErrorMode errorMode;
        private string errorColumn;
        private double errorValue;
        private double errorMin;
        private double errorMax;
        private string where;
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

        public string TableUniqueKey
        {
            get { return tableUniqueKey; }
            set { tableUniqueKey = value; }
        }

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
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
            this.tableUniqueKey = null;
            this.alias = null;
            this.coordinateMode = CoordinateMode.Automatic;
            this.raColumn = "ra";
            this.decColumn = "dec";
            this.errorMode = ErrorMode.Constant;
            this.errorColumn = null;
            this.errorValue = 0.1;
            this.errorMin = 0.1;
            this.errorMax = 0.1;
            this.where = null;
            this.columns = new List<string>();
        }
    }
}