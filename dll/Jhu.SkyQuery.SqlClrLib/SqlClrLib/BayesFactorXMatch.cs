using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace Jhu.SkyQuery.SqlClrLib
{
    public partial class UserDefinedFunctions
    {
        [Microsoft.SqlServer.Server.SqlFunction(
            Name = "skyquery.BayesFactorCalcPosition",
            DataAccess = DataAccessKind.None,
            IsDeterministic = true,
            IsPrecise = false,
            SystemDataAccess = SystemDataAccessKind.None)]
        public static BayesFactorPoint BayesFactorCalcPosition(
            double cx, double cy, double cz,        // c_{k-1}
            double a,       // a_{k-1} from Eq. 38
            double l,       // l_{k-1}
            double q,       // q_{k-1}
            double logBF,   // logBF_{k-1}
            double cx_k, double cy_k, double cz_k,   // c_k
            double w_k      // w_k from Eq. 39
            )
        {
            // Calculate new coordinates and update parameters for every match
            /* 
             * variables without _k index: previous step
             * variables with _k: current step
             * 
             * c[x,y,z] : coordinates in the k-1st catalog
             * c[x,y,z]_k : norm(sum(w_i * c_i, i=1..k)), best position till matching with the kth catalog (Eq. 36)
             * w_k : weight of point (inverse squared error) in the kth catalog
             * a_k : sum(w_i, i=1..k), accumulated weight till matching with the kth catalog (Eq. 34)
             * l_k : sum(log(w_i), i=1..k), accumulated log weight till matching with the kth catalog
             * logBF_k : log(N) - 0.5 * sum(a_{i-1}/a_i*w_i*d_i^2, i=2..k), log of Bayes factor (Eq. 32)
             * N = 2^{n-1} * prod(w_i, i=1..n) / sum(w_i, i=1..n), (Eq. 33)
             * n : number of catalogs
             * d[x,y,z]: separation from previous best match (Eq. 35)
             * q_k : (Eq. 39)
             * */

            // initial values (for the very first catalog) -- see SelectAugmentedTable.sql
            // c[x,y,z] = c[x,y,z] of the particular catalog
            // a_1 = w_1
            // l_1 = log(w_1)
            // q_1 = 0
            // logBF_1 = (n - 1) * log(2)       : ln(N) of Eq. 33


            // Separation
            var dx = cx - cx_k;
            var dy = cy - cy_k;
            var dz = cz - cz_k;

            // Eq. 38
            var a_k = a + w_k;
            var l_k = l + Math.Log(w_k);

            // Eq. 39
            var dq = a / a_k * w_k * (dx * dx + dy * dy + dz * dz);
            var q_k = q + dq;

            // Eq. 37
            // TODO: delete old code
            //var logBF_k = logBF + Math.Log(w_k) + Math.Log(a) - Math.Log(a + w) - (a / (a + w) * w * (dx * dx + dy * dy + dz * dz)) / 2.0,
            var logBF_k = logBF + Math.Log(w_k * a / a_k) - 0.5 * dq;

            // Eq. 40
            var c_k = new Spherical.Cartesian(
                cx + w_k / (a + w_k) * dx,
                cy + w_k / (a + w_k) * dy,
                cz + w_k / (a + w_k) * dz,
                true);

            // The maximum search radius is calculated as (Eq. 37)
            // 1 / a + 1 / w_min is an upper bound of b_k of Eq. 37
            // (1 / a + 1 / w_min) * (2 * (factor + l + l_max - ln(a + a_min) - limit) - q)
            // where
            // factor: (n_max - 1) * ln(2)
            // n_max = the maximum number of matches
            // w_min = w_k, weight of the current catalog
            // a_min = sum(w_max_i,i=1..k), where w_max_i is the max error specified for the catalog
            // l_max = sum(log(w_min_i,i=1..k), where w_min_i is the min error specified for the catalog
            // limit = bayes factor limit, as specified by the user
            // q = sum(a/a_i*w_i*d_k^2,i=1..k), weighted separation


            // The final cut will be on q_k
            // q_k < 2 * (factor + l_k + l_min - Math.Log(a + a_max) - limit)
            // where
            // factor = (n - 1) * ln(2);
            // l_min = sum(log(w_i), i=k+1..n)
            // limit: bayes factor limit as specified by user
            


            return new BayesFactorPoint()
            {
                Ra = c_k.RA,
                Dec = c_k.Dec,
                Cx = c_k.X,
                Cy = c_k.Y,
                Cz = c_k.Z,
                A = a_k,
                L = l_k,    
                Q = q_k,
                LogBF = logBF_k,
            };

        }
    }

}