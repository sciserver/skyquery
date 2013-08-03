using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Xmd
{
    public class Table
    {
        private JoinMethod joinMethod;
        private XMatchMethod xMatchMethod;
        private string name;
        private string pointExpression;
        private string errorExpression;
        private List<List<And>> on;

        [XmlIgnore]
        public JoinMethod JoinMethod
        {
            get { return joinMethod; }
            set { joinMethod = value; }
        }

        [XmlAttribute]
        public XMatchMethod XMatchMethod
        {
            get { return xMatchMethod; }
            set { xMatchMethod = value; }
        }

        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute]
        public string PointExpression
        {
            get { return pointExpression; }
            set { pointExpression = value; }
        }

        [XmlAttribute]
        public string ErrorExpression
        {
            get { return errorExpression; }
            set { errorExpression = value; }
        }

        [XmlArrayItem("Or")]
        public List<List<And>> On
        {
            get { return on; }
            set { on = value; }
        }

        public Table()
        {
            InitializeMembers();
        }

        public Table(JoinMethod joinMethod, string name)
        {
            InitializeMembers();

            this.joinMethod = joinMethod;
            this.name = name;
        }

        private void InitializeMembers()
        {
            this.joinMethod = JoinMethod.NA;
            this.xMatchMethod = XMatchMethod.None;
            this.name = string.Empty;
            this.pointExpression = null;
            this.errorExpression = null;
            this.on = new List<List<And>>();
        }
    }
}
