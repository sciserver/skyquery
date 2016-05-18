using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace Jhu.SkyQuery.SqlClrLib
{
    public partial class UserDefinedFunctions
    {
        private const double log2 = 0.30102999566398119521373889472449;

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

            public void First(double cx, double cy, double cz, double w, int n)
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
                this.logBF = (n - 1) * log2;    // : ln(N) of Eq. 33
            }

            public bool Update(double cx_k, double cy_k, double cz_k, int k, double w_k, short n_max, double a_max, double l_min, double logBF_limit)
            {
                // Separation (Eq. 35)
                var dx = cx - cx_k;
                var dy = cy - cy_k;
                var dz = cz - cz_k;

                // Eq. 34 and 38
                var a_k = a + w_k;
                var l_k = l + Math.Log(w_k);

                // Eq. 32, 39 and 35
                var da = a / a_k * w_k;
                var dq = da * (dx * dx + dy * dy + dz * dz);
                var q_k = q + dq;

                // Number of catalogs matched so far
                // TODO: update this to implement outer match
                var n_k = (short)(n + 1);

                // Cut on Bayes factor
                // The formula is an upper limit on the Bayes factor, considering the weights of all remaining catalogs
                // n is now the number of previously matched catalogs
                var q_max = 2 * (log2 * (n_max - k + n_k) + l_k + l_min - Math.Log(a_k + a_max)) - logBF_limit;

                if (q_k < q_max)
                {
                    // This is a match as we are inside the limit

                    // Update best match coordinates Eq. 40
                    var wc = w_k / a_k;
                    cx += wc * dx;
                    cy += wc * dy;
                    cz += wc * dz;

                    // Update other variables
                    n = n_k;
                    a = w_k;
                    l = l_k;
                    q = q_k;

                    // Calculate updated bayes factor
                    // log of Eq. 33 and 32
                    var logN_k = (n_k - 1) * log2 + l_k - Math.Log(a_k);
                    logBF = logN_k - 0.5 * q_k;

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
            SqlDouble s_a_max,
            SqlDouble s_l_min,
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

#if false
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
            SqlDouble s_a_max,
            SqlDouble s_l_min,
            SqlInt16 s_n_max,           // total number of catalogs
            SqlDouble s_logBF_limit
            )
        {
            var cx_p = s_cx_p.Value;        // Best match coordinates after previous match
            var cy_p = s_cy_p.Value;
            var cz_p = s_cz_p.Value;
            var n_p = s_n_p.Value;          // Number of matches till the previous match
            var a_p = s_a_p.Value;          // Sum of weights till the previous match (Eq. 38)
            var l_p = s_l_p.Value;          // Sum of the log-weights
            var q_p = s_q_p.Value;          // (Eq. 39)

            var cx_k = s_cx_k.Value;        // Coordinates in the current catalog
            var cy_k = s_cy_k.Value;        
            var cz_k = s_cz_k.Value;
            var k = s_k.Value;              // Index of the current catalog
            var w_k = s_w_k.Value;          // Weight in the current catalog

            var a_max = s_a_max.Value;      // Upper limit on sum of weights of the unmatched catalogs
            var l_min = s_l_min.Value;      // Upper limit on sum of log-weights
            var n_max = s_n_max.Value;      // Total number of catalogs, i.e. the upper limit on the number of matches

            var logBF_limit = s_logBF_limit.Value;  // Bayes factor limit as specified by the user

            
            // initial values (for the very first catalog) -- see SelectAugmentedTable.sql
            // c[x,y,z] = c[x,y,z] of the particular catalog
            // a_1 = w_1
            // l_1 = log(w_1)
            // q_1 = 0
            // logBF_1 = (n - 1) * log(2)       : ln(N) of Eq. 33

            // Separation (Eq. 35)
            var dx = cx_p - cx_k;
            var dy = cy_p - cy_k;
            var dz = cz_p - cz_k;

            // Eq. 34 and 38
            var a_k = a_p + w_k;
            var l_k = l_p + Math.Log(w_k);

            // Eq. 32, 39 and 35
            var da = a_p / a_k * w_k;
            var dq = da * (dx * dx + dy * dy + dz * dz);
            var q_k = q_p + dq;

            // Number of catalogs matched so far
            // TODO: update this to implement outer match
            var n_k = (short)(n_p + 1);

            // Cut on Bayes factor
            // The formula is an upper limit on the Bayes factor, considering the weights of all remaining catalogs
            // n is now the number of previously matched catalogs
            var q_max = 2 * (log2 * (n_max - k + n_k) + l_k + l_min - Math.Log(a_k + a_max)) - logBF_limit;

            if (q_k < q_max)
            {
                // This is a match as we are inside the limit

                // Calculate updated bayes factor
                // log of Eq. 33 and 32
                var logN_k = (n_k - 1) * log2 + l_k - Math.Log(a_k);
                var logBF_k = logN_k - 0.5 * q_k;

                // Update best match coordinates Eq. 40
                var wc = w_k / a_k;
                cx_k = cx_p + wc * dx;
                cy_k = cy_p + wc * dy;
                cz_k = cz_p + wc * dz;

                var c_k = new Spherical.Cartesian(cx_k, cy_k, cz_k, true);

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

                // Return updated values
                return new BayesFactorPoint()
                {
                    IsMatch = true,
                    Ra = c_k.RA,
                    Dec = c_k.Dec,
                    Cx = c_k.X,
                    Cy = c_k.Y,
                    Cz = c_k.Z,
                    N = n_k,
                    A = a_k,
                    L = l_k,
                    Q = q_k,
                    LogBF = logBF_k,
                };
            }
            else
            {
                // Return a non-match
                return new BayesFactorPoint()
                {
                    IsMatch = false,
                };
            }
        }
#endif

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

            var logN = log2 * (n_max - 1) + l + l_max - Math.Log(a + a_min);
            var b = 1 / a + 1 / w_min;
            var r = b * (2 * (logN - logBF_limit) - q);
                        
            //(1 / a + 1 / @weightMin) * (2 * (@factor + l + @lmax - LOG(a + @amin) - @limit) - q)

            return new SqlDouble(r);
        }
    }
}