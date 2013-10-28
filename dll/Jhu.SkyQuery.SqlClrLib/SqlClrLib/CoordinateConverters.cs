using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace Jhu.SkyQuery.SqlClrLib
{
    public class CoordinateConverters
    {
        // This is not optimized, should use TVFs instead!

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlDouble EqToCartesianX(SqlDouble ra, SqlDouble dec)
        {
            double cx, cy, cz;
            Cartesian.Radec2Xyz(ra.Value, dec.Value, out cx, out cy, out cz);

            return new SqlDouble(cx);
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlDouble EqToCartesianY(SqlDouble ra, SqlDouble dec)
        {
            double cx, cy, cz;
            Cartesian.Radec2Xyz(ra.Value, dec.Value, out cx, out cy, out cz);

            return new SqlDouble(cy);
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlDouble EqToCartesianZ(SqlDouble ra, SqlDouble dec)
        {
            double cx, cy, cz;
            Cartesian.Radec2Xyz(ra.Value, dec.Value, out cx, out cy, out cz);

            return new SqlDouble(cz);
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlDouble CartesianToEqRa(SqlDouble cx, SqlDouble cy, SqlDouble cz)
        {
            double ra, dec;
            Cartesian.Xyz2Radec(cx.Value, cy.Value, cz.Value, out ra, out dec);

            return new SqlDouble(ra);
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlDouble CartesianToEqDec(SqlDouble cx, SqlDouble cy, SqlDouble cz)
        {
            double ra, dec;
            Cartesian.Xyz2Radec(cx.Value, cy.Value, cz.Value, out ra, out dec);

            return new SqlDouble(dec);
        }
    }
}
