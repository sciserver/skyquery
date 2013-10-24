using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Scheduler;

namespace Jhu.SkyQuery.Test
{
    [TestClass]
    public class CatalogTest : XMatchQueryTestBase
    {

        [TestMethod]
        public void NDWFSTest()
        {
            var sql = @"
SELECT s.objid, s.cx, s.cy, s.cz, n.objid, n.cx, n.cy, n.cz, x.cx, x.cy, x.cz
INTO CatalogTest_NDWFSTest
FROM SDSSDR7:PhotoObjAll AS s
	CROSS JOIN NDWFS:PhotoObj AS n
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.cx, s.cy, s.cz), 0.1, 0.1, 0.1
MUST EXIST n ON POINT(n.cx, n.cy, n.cz), 0.2, 0.2, 0.2
HAVING LIMIT 1e2
WHERE s.cx BETWEEN 0 AND 5 AND s.cy BETWEEN 0 AND 5 AND s.cz BETWEEN 0 AND 5
  AND n.cx BETWEEN 0 AND 5 AND n.cy BETWEEN 0 AND 5 AND n.cz BETWEEN 0 AND 5
";

            using (SchedulerTester.Instance.GetToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var guid = ScheduleQueryJob(
                    sql,
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

    }
}
