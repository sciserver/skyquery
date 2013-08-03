using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Xmd
{
	public class And
	{
        private string expression;

        [XmlText]
        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public And()
        {
            InitializeMembers();
        }

        public And(string expression)
        {
            InitializeMembers();

            this.expression = expression;
        }

        private void InitializeMembers()
        {
            this.expression = String.Empty;
        }
	}
}
