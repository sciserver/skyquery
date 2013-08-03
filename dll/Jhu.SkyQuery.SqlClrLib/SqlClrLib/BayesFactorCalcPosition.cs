using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction(
        DataAccess=DataAccessKind.None,
        IsDeterministic=true,
        IsPrecise=false,
        SystemDataAccess=SystemDataAccessKind.None)]
    public static BayesFactorPoint BayesFactorCalcPosition(
        double cx, double cy, double cz,    
        double a,
        double l,
        double logBF,
        double w,                           
        double dx, double dy, double dz     // separation
        )
    {
        double rx, ry, rz, r, ra, dec;
        rx = cx + w / (a + w) * dx;
        ry = cy + w / (a + w) * dy;
        rz = cz + w / (a + w) * dz;
        r = Math.Sqrt(rx * rx + ry * ry + rz * rz);

        rx /= r;
        ry /= r;
        rz /= r;

        if (rz >= 1)
        {
            dec = Const.HalfPI;
        }
        else if (rz <= -1)
        {
            dec = Const.MinusHalfPI;
        }
        else
        {
            dec = Math.Asin(rz);
        }

        const double eps = 4e-15;
        double cd = Math.Cos(dec);

        if (cd > eps || cd < -eps)
        {
            if (ry > eps || ry < -eps)
            {
                double arg = rx / cd;
                double acos;

                if (arg <= -1)
                {
                    acos = Const.HalfPI;
                }
                else if (arg >= 1)
                {
                    acos = 0;
                }
                else
                {
                    acos = Math.Acos(arg);
                }
                if (ry < 0)
                {
                    ra = Const.TwoPI - acos;
                }
                else
                {
                    ra = acos;
                }
            }
            else
            {
                if (rx < 0)
                {
                    ra = Const.HalfPI;
                }
                else
                {
                    ra = 0;
                }
            }
        }
        else
        {
            ra = 0;
        }

        return new BayesFactorPoint()
        {
            Ra = ra * 180.0 / Math.PI,
            Dec = dec * 180.0 / Math.PI,
            Cx = rx,
            Cy = ry,
            Cz = rz,
            A = a + w,
            L = l + Math.Log(w),
            LogBF = logBF + Math.Log(w) + Math.Log(a) - Math.Log(a + w) - (a / (a + w) * w * (dx * dx + dy * dy + dz * dz)) / 2.0,
            dQ = a / (a + w) * w * (dx * dx + dy * dy + dz * dz)
        };

    }
};

