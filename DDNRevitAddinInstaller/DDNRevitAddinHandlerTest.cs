using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDNGeneralLibrary;
using DDNRevitAddinInstaller.RevitAddinUtilityExtensions;
using NUnit.Framework;

namespace DDNRevitAddinInstaller
{
    [TestFixture]
    public class DDNRevitAddinHandlerTest:RevitAddinTestBase
    {
        private const string Ddnrevitaddin = "DDNRevitAddin";
        public const int DDNAddinCount = 7;

        [Test]
        public void InstalledClassTest()
        {
            Assert.AreEqual(DDNAddinCount, Handler.ClassesInstalled.Count);
        }


        [Test]
        public void RevitAddinFilesTest()
        {
            Assert.AreEqual(DDNAddinCount,HandlerFileAddins.Count);
        }
        
        [Test]
        public void RevitAddinsPathTest()
        {
            Assert.AreEqual(RevitProductionExtensionTest.CurrentRevitAddinPath, Handler.RevitAddinsPath);
        }

        [Test]
        public void AddinDllPathTest()
        {
            Assert.NotNull(FileInfoExtension.GetDllFileInfo(DDNRevitAddinHandler.AddinDllPath, Ddnrevitaddin));

        }
    }
}
