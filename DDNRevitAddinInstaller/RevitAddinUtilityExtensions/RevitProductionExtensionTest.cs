using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DDNRevitAddinInstaller.RevitAddinUtilityExtensions
{
    [TestFixture]
    public class RevitProductionExtensionTest : RevitAddinTestBase
    {
        public const string CurrentRevitAddinPath = @"C:\ProgramData\Autodesk\Revit\AddIns\2019\";

        [Test]
        public void GetCurrentProductAllUsersAddInFolderTest()
        {
            Assert.AreEqual(CurrentRevitAddinPath, RevitProductExtension.GetCurrentProductAllUsersAddInFolder());
        }
        [Test]
        public void GetAddinFileTest()
        {
            Assert.AreEqual(1, AddinManagerFile.Count);
            Assert.AreEqual(0, RevitProductExtension.GetAddinFiles("nothing").Count);
        }
    }
}
