using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace DDNGeneralLibrary
{
    [TestFixture]
    public class TimeExtensionTest 
    {
        [Test]
        public void ExtTimeStampTest()
        {
            MessageBox.Show( TimeExtension.ExtTimeStamp());
            Assert.True(true);
        }
    }
}