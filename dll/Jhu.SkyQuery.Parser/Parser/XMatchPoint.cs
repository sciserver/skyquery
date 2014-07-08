using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.SqlCodeGen.SqlServer;

namespace Jhu.SkyQuery.Parser
{
    public partial class XMatchPoint
    {
        private int eqIndex;
        private int xyzIndex;

        private bool isEqSpecified;
        private bool isXyzSpecified;

        private List<Argument> arguments;

        /// <summary>
        /// Gets the string specifying the coordinate system
        /// </summary>
        public string CoordinateSystem
        {
            get
            {
                // The only string parameter is the coordinate system
                var sc = this.FindDescendant<StringConstant>();

                if (sc != null)
                {
                    return sc.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the expression for right ascension
        /// </summary>
        public string RA
        {
            get
            {
                // Check if RA is specified in the query or we have to compute from cartesian coordinates
                if (isEqSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(arguments[eqIndex].FindDescendant<Expression>(), true);
                }
                else if (isXyzSpecified)
                {
                    // TODO: change it to use spherical lib function from SkyQuery_CODE?
                    return String.Format("SkyQuery_Code.dbo.CartesianToEqRa(({0}),({1}),({2}))", Cx, Cy, Cz);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the expression for declination
        /// </summary>
        public string Dec
        {
            get
            {
                // Check if Dec is specified in the query or we have to compute from cartesian coordinates
                if (isEqSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(arguments[eqIndex + 1].FindDescendant<Expression>(), true);
                }
                else if (isXyzSpecified)
                {
                    // TODO: change it to use spherical lib function from SkyQuery_CODE?
                    return String.Format("SkyQuery_Code.dbo.CartesianToEqDec(({0}),({1}),({2}))", Cx, Cy, Cz);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the X coordinate of the cartesian unit vector
        /// </summary>
        public string Cx
        {
            get
            {
                // Check if X is specified in the query or we have to compute from equatorial coordinates
                if (isXyzSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(arguments[xyzIndex].FindDescendant<Expression>(), true);
                }
                else if (isEqSpecified)
                {
                    // TODO: change it to use spherical lib function from SkyQuery_CODE
                    return String.Format("SkyQuery_Code.dbo.EqToCartesianX(({0}),({1}))", RA, Dec);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the Y coordinate of the cartesian unit vector
        /// </summary>
        public string Cy
        {
            get
            {
                // Check if Y is specified in the query or we have to compute from equatorial coordinates
                if (isXyzSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(arguments[xyzIndex + 1].FindDescendant<Expression>(), true);
                }
                else if (isEqSpecified)
                {
                    // TODO: change it to use spherical lib function from SkyQuery_CODE
                    return String.Format("SkyQuery_Code.dbo.EqToCartesianY(({0}),({1}))", RA, Dec);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the Z coordinate of the cartesian unit vector
        /// </summary>
        public string Cz
        {
            get
            {
                // Check if Z is specified in the query or we have to compute from equatorial coordinates
                if (isXyzSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(arguments[xyzIndex + 2].FindDescendant<Expression>(), true);
                }
                else if (isEqSpecified)
                {
                    //return String.Format("SIN(RADIANS({1}))", RA, Dec);
                    return String.Format("SkyQuery_Code.dbo.EqToCartesianZ(({0}),({1}))", RA, Dec);
                }
                else
                {
                    return null;
                }
            }
        }

        public XMatchPoint()
            : base()
        {
            InitializeMembers();
        }

        public XMatchPoint(XMatchPoint old)
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

        private void CopyMembers(XMatchPoint old)
        {
            this.arguments = new List<Argument>(old.arguments);
        }

        public override Node Interpret()
        {
            arguments.Clear();
            arguments.AddRange(this.FindDescendant<FunctionArguments>().FindDescendant<ArgumentList>().EnumerateDescendants<Argument>());

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
            return new XMatchPoint(this);
        }
    }
}