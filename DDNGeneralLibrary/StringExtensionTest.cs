using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DDNGeneralLibrary
{
    [TestFixture]
    public class StringExtensionTest:StringExtensionTestBase
    {
        [Test]
        public void ExtSplitTest()
        {
            Assert.AreEqual(1,("test".ExtSplit(new []{'.'}).Count));
            Assert.AreEqual(3, (Line1.ExtSplit(new[] { '.' }).Count));
            IList<string> texts = Line1.ExtSplit('.');
            Assert.AreEqual(3, (texts.Count));
            Assert.AreEqual(Text2,texts[1]);
        }
    }
}
