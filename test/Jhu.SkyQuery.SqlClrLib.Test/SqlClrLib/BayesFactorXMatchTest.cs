using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Spherical;

namespace Jhu.SkyQuery.SqlClrLib
{
    [TestClass]
    public class BayesFactorXMatchTest
    {
        [TestMethod]
        public void UpdateBayesFactorTest()
        {
            int N = 30;
            var bf = new BayesFactorPoint[N];

            double sigma_1 = 1 / 3600.0 / 180.0 * Math.PI;
            double sigma_2 = 3 / 3600.0 / 180.0 * Math.PI;
            double w_1 = 1 / Math.Pow(sigma_1, 2);
            double w_2 = 1 / Math.Pow(sigma_2, 2);

            for (int i = 0; i < N; i++)
            {
                var ra = 0;
                var dec = i / 3600.0;
                double cx, cy, cz;
                Cartesian.Radec2Xyz(ra, dec, out cx, out cy, out cz);

                bf[i] = UserDefinedFunctions.BayesFactorCalcPosition(
                    1, 0, 0,
                    1,
                    w_1,
                    Math.Log(w_1),
                    0,
                    cx, cy, cz,
                    w_2,
                    2,      // Step number
                    0,
                    0,      // min of sum of log weights for remaining catalogs
                    2,      // Total number of catalogs
                    Math.Log(1e3));

                
            }

        }
    }
}
