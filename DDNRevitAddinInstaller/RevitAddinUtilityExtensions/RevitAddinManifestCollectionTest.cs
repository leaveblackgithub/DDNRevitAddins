using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDNGeneralLibrary;
using NUnit.Framework;

namespace DDNRevitAddinInstaller.RevitAddinUtilityExtensions
{
    [TestFixture]
    public class RevitAddinManifestCollectionTest:RevitAddinTestBase
    {
        private const int AddinManagerAddinCount = 3;

        [Test]
        public void InstalledClassTest()
        {
            RevitAddInManifestCollection addInManifestCollection =new RevitAddInManifestCollection(AddinManagerFile);
            IDictionary<string, FileInfo> installedClasses = addInManifestCollection.InstalledClasses;
            Assert.AreEqual(AddinManagerAddinCount, installedClasses.Count);
            Assert.AreEqual("AddInManager.CAddInManager", installedClasses.ElementAt(0).Key);

            addInManifestCollection = new RevitAddInManifestCollection(HandlerFileAddins);
            installedClasses = addInManifestCollection.InstalledClasses;
            Assert.AreEqual(DDNRevitAddinHandlerTest.DDNAddinCount, installedClasses.Count);
        }
    }
}
