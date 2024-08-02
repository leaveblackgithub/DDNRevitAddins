using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DDNGeneralLibrary
{
    [TestFixture]
    public class DictionaryExtensionTest
    {
        [Test]
        public void MergeTest()
        {
            Dictionary<string, string> dict1 = new Dictionary<string, string>
            {
                { "a", "b" },
                { "c", "d" }
            };
            Dictionary<string, string> dict2 = new Dictionary<string, string>
            {
                { "e", "f" },
                { "c", "d" }
            };
            dict1.MergeDict(dict2);
            Assert.AreEqual(2,dict1.Count);
            Assert.AreEqual(3, dict1.MergeDict(dict2).Count);
        }
    }
}
