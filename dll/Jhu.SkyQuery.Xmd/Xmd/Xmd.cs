using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Xmd
{
	public class Xmd
	{
        private List<Column> columns;
        private List<Table> tables;
        private List<List<And>> where;

        [XmlArray("Columns")]
        public List<Column> Columns
        {
            get { return columns; }
        }

        [XmlArray("Tables")]
        public List<Table> Tables
        {
            get { return tables; }
        }

        [XmlArrayItem("Or")]
        public List<List<And>> Where
        {
            get { return where; }
        }

        public Xmd()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.columns = new List<Column>();
            this.tables = new List<Table>();
            this.where = new List<List<And>>();
        }
	}
}
