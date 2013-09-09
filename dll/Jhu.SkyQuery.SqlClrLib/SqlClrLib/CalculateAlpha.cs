using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace Jhu.SkyQuery.SqlClrLib
{
    public partial class UserDefinedFunctions
    {
        [Microsoft.SqlServer.Server.SqlFunction]
        public static double CalculateAlpha(double theta, double decMin, double decMax, double zoneHeight)
        {
            double dec;

            if (Math.Abs(decMax) < Math.Abs(decMin))
            {
                dec = decMin - zoneHeight / 100;
            }
            else
            {
                dec = decMax + zoneHeight / 100;
            }

            if (Math.Abs(dec) + theta > 89.9)
            {
                return 180; // hack
            }
            else
            {
                return Constants.Degrees * Math.Abs(Math.Atan(Math.Sin(Constants.Radians * theta)
                    / Math.Sqrt(Math.Abs(Math.Cos(Constants.Radians * (dec - theta))
                    * Math.Cos(Constants.Radians * (dec + theta))))));
            }
        }
    }
}