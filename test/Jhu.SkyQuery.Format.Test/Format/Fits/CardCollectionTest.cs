using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.SkyQuery.Format.Fits;

namespace Jhu.SkyQuery.Format.Fits.Test
{
    [TestClass]
    public class CardCollectionTest
    {
        [TestMethod]
        public void AddTest()
        {
            var cc = new CardCollection();

            cc.Add(new Card("TEST1", "Test1"));
            cc.Add("TEST2", new Card("TEST2", "Test2"));

            // Test add with existing keyword
        }
    }
}
