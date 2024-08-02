using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DDNGeneralLibrary
{
    [TestFixture]
    public class FileInfoExtensionTest
    {
        [Test]
        public void FileExistsTest()
        {
            Assert.True(FileInfoExtension.FileExists(@"c:\Windows\win.ini"));
            Assert.False(FileInfoExtension.FileExists(@"c:\Windows\win.ini1223"));
            Assert.False(FileInfoExtension.FileExists(""));
            Assert.False(FileInfoExtension.FileExists(null));
        }
    }
}
