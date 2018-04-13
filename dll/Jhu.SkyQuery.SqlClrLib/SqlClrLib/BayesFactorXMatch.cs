using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace Jhu.SkyQuery.SqlClrLib
{
    public partial class UserDefinedFunctions
    {
        private static readonly double log2 = Math.Log(2);

        struct Match
        {
            public double cx;       // coordinates
            public double cy;
            public double cz;
            public short n;         // number of catalogs matched so far
            public double a;        // a from Eq. 38
            public double l;        // l
            public double q;        // q
            public double logBF;    // log BF

            public void Init(double cx, double cy, double cz, double w)
            {
                // initial values (for the very first catalog)
                // see SelectAugmentedTable.sql

                this.cx = cx;
                this.cy = cy;
                this.cz = cz;
                this.n = 1;
                this.a = w;
                this.l = Math.Log(w);
                this.q = 0;
                this.logBF = 0;
            }

            public bool Update(
                double cx_k, double cy_k, double cz_k, 
                int k, double w_k,
                short n_max, double a_max, double l_min, 
                double logBF_limit)
            {
                // Separation (Eq. 35)
                var dx = cx - cx_k;
                var dy = cy - cy_k;
                var dz = cz - cz_k;
                var D2 = dx * dx + dy * dy + dz * dz;

                // Eq. 34 and 38
                var a_k = a + w_k;              // sum of weights
                var l_k = l + Math.Log(w_k);    // sum of log weights

                // Eq. 32, 39 and 35
                var q_k = q + a / a_k * w_k * D2;

                // Number of catalogs matched so far
                // TODO: update this to implement outer match
                var n_k = (short)(n + 1);

                // Calculate a lower limit on the achievable bayes factor assuming that all
                // subsequent matches are perfect (i.e. D2 = 0)
                var logBF_min = (n_max - 1) * log2 + (l_k + l_min) - Math.Log(a_k + a_max) - 0.5 * q_k;

                if (logBF_min >= logBF_limit)
                {
                    // This is a match as we are inside the limit

                    // Calculate the updated bayes factor
                    // log of Eq. 33 and 32
                    var logBF = n * log2 + l_k - Math.Log(a_k) - 0.5 * q_k;

                    // Update best match coordinates Eq. 40
                    var wc = w_k / a_k;
                    cx += wc * dx;
                    cy += wc * dy;
                    cz += wc * dz;

                    // Update other variables
                    n = n_k;
                    a = a_k;
                    l = l_k;
                    q = q_k;

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

                    return true;
                }
                else
                {
                    return false;
                }
            }

            public BayesFactorPoint AsBayesFactorPoint()
            {
                var c = new Spherical.Cartesian(cx, cy, cz, true);

                // Return updated values
                return new BayesFactorPoint()
                {
                    IsMatch = true,
                    Ra = c.RA,
                    Dec = c.Dec,
                    Cx = c.X,
                    Cy = c.Y,
                    Cz = c.Z,
                    N = n,
                    A = a,
                    L = l,
                    Q = q,
                    LogBF = logBF,
                };
            }
        }

        [Microsoft.SqlServer.Server.SqlFunction(
            Name = "skyquery.BayesFactorCalcPosition",
            DataAccess = DataAccessKind.None,
            IsDeterministic = true,
            IsPrecise = false,
            SystemDataAccess = SystemDataAccessKind.None)]
        public static BayesFactorPoint BayesFactorCalcPosition(
            SqlDouble s_cx_p, SqlDouble s_cy_p, SqlDouble s_cz_p,       // c_{k-1}
            SqlInt16 s_n_p,             // number of catalogs matched so far
            SqlDouble s_a_p,            // a_{k-1} from Eq. 38
            SqlDouble s_l_p,            // l_{k-1}
            SqlDouble s_q_p,            // q_{k-1}
            SqlDouble s_cx_k, SqlDouble s_cy_k, SqlDouble s_cz_k,       // c_k
            SqlDouble s_w_k,            // w_k from Eq. 39
            SqlInt16 s_k,               // step number (kth catalog being matched)
            SqlDouble s_a_max,          // max of sum of weights for remaining catalogs
            SqlDouble s_l_min,          // min of sum of log weights for remaining catalogs
            SqlInt16 s_n_max,           // total number of catalogs
            SqlDouble s_logBF_limit
            )
        {
            var m_p = new Match()
            {
                cx = s_cx_p.Value,        // Best match coordinates after previous match
                cy = s_cy_p.Value,
                cz = s_cz_p.Value,
                n = s_n_p.Value,          // Number of matches till the previous match
                a = s_a_p.Value,          // Sum of weights till the previous match (Eq. 38)
                l = s_l_p.Value,          // Sum of the log-weights
                q = s_q_p.Value,          // (Eq. 39)
            };

            var cx_k = s_cx_k.Value;        // Coordinates in the current catalog
            var cy_k = s_cy_k.Value;
            var cz_k = s_cz_k.Value;

            var k = s_k.Value;              // Index of the current catalog
            var w_k = s_w_k.Value;          // Weight in the current catalog

            var n_max = s_n_max.Value;      // Total number of catalogs, i.e. the upper limit on the number of matches
            var a_max = s_a_max.Value;      // Upper limit on sum of weights of the unmatched catalogs
            var l_min = s_l_min.Value;      // Upper limit on sum of log-weights

            var logBF_limit = s_logBF_limit.Value;  // Bayes factor limit as specified by the user

            var match = m_p.Update(cx_k, cy_k, cz_k, k, w_k, n_max, a_max, l_min, logBF_limit);

            if (match)
            {
                return m_p.AsBayesFactorPoint();
            }
            else
            {
                return BayesFactorPoint.NoMatch;
            }
        }

        [Microsoft.SqlServer.Server.SqlFunction(
            Name = "skyquery.BayesFactorCalcSearchRadius",
            DataAccess = DataAccessKind.None,
            IsDeterministic = true,
            IsPrecise = false,
            SystemDataAccess = SystemDataAccessKind.None)]
        public static SqlDouble BayesFactorCalcSearchRadius(
            SqlDouble s_a,
            SqlDouble s_l,
            SqlDouble s_q,
            SqlDouble s_w_min,
            SqlDouble s_a_min,
            SqlDouble s_l_max,
            SqlInt16 s_n_max,           // total number of catalogs
            SqlDouble s_logBF_limit
            )
        {
            // Eq. 37

            var a = s_a.Value;
            var l = s_l.Value;
            var q = s_q.Value;
            var w_min = s_w_min.Value;
            var a_min = s_a_min.Value;
            var l_max = s_l_max.Value;
            var n_max = s_n_max.Value;
            var logBF_limit = s_logBF_limit.Value;

            var logN = log2 * (n_max - 1) + l + l_max - Math.Log(a + a_min);    // Eq. 33
            var b = a * w_min / (a + w_min);       // Eq. 37
            var r2 = (2 * (logN - logBF_limit) - q) / b;

            return new SqlDouble(r2);
        }
    }
}