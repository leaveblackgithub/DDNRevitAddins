using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DDNGeneralLibrary
{
    
    [TestFixture]
    public class FailTest
    {
        [Test]
        public void FailTestMethod()
        {
            Assert.AreEqual(0,1);
        }
    }
}
