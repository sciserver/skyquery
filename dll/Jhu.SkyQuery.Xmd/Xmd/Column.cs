using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Xmd
{
	public class Column
	{
        private string expression;
        private string alias;

        [XmlAttribute]
        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        [XmlAttribute]
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        public Column()
        {
            InitializeMembers();
        }

        public Column(string expression, string alias)
        {
            InitializeMembers();
            
            this.expression = expression;
            this.alias = alias; 
        }

        private void InitializeMembers()
        {
            this.expression = String.Empty;
            this.alias = String.Empty;
        }
	}
}
