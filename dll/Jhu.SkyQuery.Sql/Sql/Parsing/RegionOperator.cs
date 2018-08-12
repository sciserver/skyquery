using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public partial class RegionOperator
    {
        private RegionOperatorType type;

        public override int Precedence
        {
            get
            {
                // This is tested SQL Server behavior for resultset operators

                switch (type)
                {
                    case RegionOperatorType.Intersect:
                        return 2;
                    case RegionOperatorType.Union:
                    case RegionOperatorType.Except:
                        return 3;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public override bool IsLeftAssociative
        {
            get { return false; }
        }

        public override void Interpret()
        {
            base.Interpret();

            switch (this.Value.ToUpperInvariant())
            {
                case "UNION":
                    type = RegionOperatorType.Union;
                    break;
                case "INTERSECT":
                    type = RegionOperatorType.Intersect;
                    break;
                case "EXCEPT":
                    type = RegionOperatorType.Except;
                    break;
                default:
                    throw new NotImplementedException();
            }

        }
    }
}
