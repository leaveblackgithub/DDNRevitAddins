using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DDNGeneralLibrary
{
    [TestFixture]
    public class StreamReaderExtensionTest : StringExtensionTestBase
    {
        [Test]
        public void ReadLinesTest()
        {
            IList<string> lines =
                StreamReaderExtension.ReadLines(
                   TestClassBase.GetTestFileFullName( "test.txt"));
            string line = lines[0];
            Assert.AreEqual(Line1,line);
        }
    }
}
