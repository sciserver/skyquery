using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.SqlParser.SqlCodeGen;

namespace Jhu.SkyQuery.Parser
{
    public partial class AdqlPoint
    {
        private int eqIndex;
        private int xyzIndex;

        private bool isEqSpecified;
        private bool isXyzSpecified;

        private List<Argument> arguments;

        public string CoordinateSystem
        {
            get
            {
                //StringConstant sc = this.FindAscendant<StringConstant>();
                StringConstant sc = this.FindDescendant<StringConstant>();

                if (sc != null)
                {
                    //return sc.FirstToken.ToString();
                    return sc.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        public string RA
        {
            get
            {
                if (isEqSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(arguments[eqIndex].FindDescendant<Expression>(), true);
                }
                else if (isXyzSpecified)
                {
                    return "";
                }
                else
                {
                    return null;
                }
            }
        }

        public string Dec
        {
            get
            {
                if (isEqSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(arguments[eqIndex + 1].FindDescendant<Expression>(), true);
                }
                else if (isXyzSpecified)
                {
                    return "";
                }
                else
                {
                    return null;
                }
            }
        }

        public string Cx
        {
            get
            {
                if (isXyzSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(arguments[xyzIndex].FindDescendant<Expression>(), true);
                }
                else if (isEqSpecified)
                {
                    return String.Format("COS(RADIANS({1}))*COS(RADIANS({0}))", RA, Dec);
                }
                else
                {
                    return null;
                }
            }
        }

        public string Cy
        {
            get
            {
                if (isXyzSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(arguments[xyzIndex + 1].FindDescendant<Expression>(), true);
                }
                else if (isEqSpecified)
                {
                    return String.Format("COS(RADIANS({1}))*SIN(RADIANS({0}))", RA, Dec);
                }
                else
                {
                    return null;
                }
            }
        }

        public string Cz
        {
            get
            {
                if (isXyzSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(arguments[xyzIndex + 2].FindDescendant<Expression>(), true);
                }
                else if (isEqSpecified)
                {
                    return String.Format("SIN(RADIANS({1}))", RA, Dec);
                }
                else
                {
                    return null;
                }
            }
        }

        public AdqlPoint()
            : base()
        {
            InitializeMembers();
        }

        public AdqlPoint(AdqlPoint old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.eqIndex = 0;
            this.xyzIndex = 0;

            this.isEqSpecified = false;
            this.isXyzSpecified = false;

            this.arguments = new List<Argument>();
        }

        private void CopyMembers(AdqlPoint old)
        {


            this.arguments = new List<Argument>(old.arguments);
        }

        public override Node Interpret()
        {
            arguments.Clear();
            arguments.AddRange(this.FindDescendant<FunctionCall>().FindDescendant<ArgumentList>().EnumerateDescendants<Argument>());

            switch (arguments.Count)
            {
                case 2:
                    isEqSpecified = true;
                    isXyzSpecified = false;
                    eqIndex = 0;
                    break;
                case 3:
                    isEqSpecified = false;
                    isXyzSpecified = true;
                    xyzIndex = 0;
                    break;
                case 5:
                    isEqSpecified = true;
                    isXyzSpecified = true;
                    eqIndex = 0;
                    xyzIndex = 2;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return base.Interpret();
        }

        public object Clone()
        {
            return new AdqlPoint(this);
        }
    }
}